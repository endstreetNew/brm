using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sassa.BRM.Models;
using Sassa.BRMDcSocpenService.Data;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Xml;

namespace Sassa.BRM.Services
{

    public class TimedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedService> _logger;

        public GlobalVars Globals = new GlobalVars();

        DateTime bookmark;

        RawSqlService _raw;

        private Timer schedule = null;

        public string? SassaTeamsUrl { get; set; }

        public string fileName;

        //private PeriodicTimer schedule = null;
        //private Timer _timerTDW = null!;
        //private Timer _timerBRM = null!;
        //private Timer _timerLO = null!;
        //private Timer _timerProgress = null!;

        public TimedService(IWebHostEnvironment env,RawSqlService raw)
        {
            fileName = Path.Combine(env.ContentRootPath, "bookmarks") + "\\bookMarks.json";
            _raw = raw;
            //_logger = logger;
        }

        //public Task StartAsync(CancellationToken stoppingToken)
        //{
        //    schedule = new Timer(SyncSOCPEN, null, TimeSpan.Zero, TimeSpan.FromHours(24));
        //    return Task.CompletedTask;
        //}
        public Task StartAsync(CancellationToken stoppingToken)
        {
            
            Globals.Progress = "Stopped.";
            if (!Globals.Status) return Task.CompletedTask;
            Globals.Progress = $"Waiting Schedule {Globals.NextRefreshTime}.";

            try
            {
                long dueTime = (long)((long)Globals.NextRefreshTime.ToTimeSpan().TotalMilliseconds - TimeOnly.FromDateTime(DateTime.Now).ToTimeSpan().TotalMilliseconds);
                if (dueTime < 0)
                {
                    dueTime = (long)(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1).AddMilliseconds((long)Globals.NextRefreshTime.ToTimeSpan().TotalMilliseconds) - DateTime.Now).TotalMilliseconds;
                }
                JsonFileUtils.WriteJson(Globals, fileName);
                schedule = new Timer(SyncSOCPEN, null, dueTime, (long)TimeSpan.FromHours(24).TotalMilliseconds);
            }
            catch(Exception ex)
            {
                Globals.Progress = "Invalid scedule. (Use todays Date and a startime after current time).";
            }
            
            return Task.CompletedTask;
        }

        private async void SyncSOCPEN(object? state)
        {

            Globals.LastRunDate = DateTime.Now;
            Globals.Progress = "Starting.";
            JsonFileUtils.WriteJson(Globals, fileName);
            bookmark = _raw.GetBookMark("Select max(Capture_date) from dc_socpen");
            bookmark = bookmark.AddDays(-14); //Start two weeks ago.
            try 
            { 
                //New SRD's
                string sql = $@"INSERT INTO DC_SOCPEN (UNIQUE_ID, ADABAS_ISN_SRD,SRD_NO,BENEFICIARY_ID,NAME,SURNAME,GENDER,GENDER_DESC,GRANT_TYPE,REGION_ID,
                            APPLICATION_DATE,APPROVAL_DATE,STATUS_CODE,PAYPOINT)
                            SELECT
                                A.UNIQUE_ID,
                                A.ADABAS_ISN,
                                A.SRD_NO,
                                A.BENEFICIARY_ID,
                                A.NAME,
                                A.SURNAME,
                                A.GENDER,
                                A.GENDER_DESC,
                                'S' AS GRANT_TYPE,
                                A.PROVINCE AS REGION_ID,
                                A.APPLICATION_DATE,
                                A.APPROVAL_DATE,
                                A.STATUS_CODE,
                                A.PAYPOINT
                            FROM
                                VW_MAX_SRD_TYPE A
                            WHERE
                                NOT EXISTS (SELECT B.SRD_NO FROM DC_SOCPEN B WHERE B.SRD_NO = A.SRD_NO)
                                AND A.Application_date >= to_date('{bookmark.ToString("dd/MMM/yyyy")}')";
                Globals.Progress = "INSERT new SRD.";
                JsonFileUtils.WriteJson(Globals, fileName);
                await _raw.ExecuteNonQuery(sql);

                //sql = $@"update DC_SOCPEN f set CAPTURE_REFERENCE =
                //    (
                //    select unq_file_no from dc_file b
                //    where b.SRD_NO = f.SRD_NO
                //    and rownum = 1
                //    AND b.GRANT_TYPE = 'S'
                //    AND b.GRANT_TYPE = f.GRANT_TYPE
                //    and f.APPlication_DATE <= b.updated_date
                //    and f.CAPTURE_REFERENCE is null
                //    and brm_barcode is not null
                //    and not exists(Select capture_reference from DC_SOCPEN dc where dc.capture_reference = b.unq_file_no)
                //    ) 
                //    where f.CAPTURE_REFERENCE is null AND f.GRant_Type = 'S' and f.Application_date >= to_date('{bookmark.ToString("dd/MMM/yyyy")}')";
                //Globals.Progress = "SRD Capture from dc file.";
                //await _raw.ExecuteNonQuery(sql);

                //Insert TRELAtional Apps

                sql = $@"INSERT INTO DC_SOCPEN (ADABAS_ISN_MAIN, APPLICATION_NO, BENEFICIARY_ID, CHILD_ID, NAME, SURNAME,  GRANT_TYPE, REGION_ID, APPLICATION_DATE, APPROVAL_DATE, STATUS_DATE, STATUS_CODE, UNIQUE_ID,PAYPOINT) 
                        SELECT 
                            A.ADABAS_ISN AS ADABAS_ISN_MAIN,
                            A.APPLICATION_NO,
                            LPAD(A.ID_NO,13,0) AS BENEFICIARY_ID,
                            LPAD(A.CHILD_ID_NO,13,0) AS CHILD_ID,
                            B.NAME_EXT,
                            B.SURNAME_EXT,
                            A.GRANT_TYPE,
                            D.REGION_CODE AS REGION_ID,
                            NVL(A.APPLICATION_DATE,C.APPLICATION_DATE) AS APPLICATION_DATE,
                            C.APPROVAL_DATE,
                            C.STATUS_DATE,
                            CASE
                                WHEN A.PRIM_STATUS IN ('B','A','9') AND A.SEC_STATUS IN ('2') THEN 'ACTIVE'
                                ELSE 'INACTIVE'
                            END AS STATUS_CODE,
                            A.ADABAS_ISN||'-'||A.APPLICATION_NO||'-'||LPAD(A.ID_NO,13,0) AS UNIQUE_ID,
                            B.secondary_paypoint as paypoint  
                        FROM SOCPEN_DOW_APPLICATIONS_CHEC01 A
                        JOIN SOCPEN_PERSONAL B ON  A.ID_NO = B.PENSION_NO
                        LEFT JOIN SOCPEN_P12_CHILDREN C ON A.ID_NO = c.pension_no AND A.GRANT_TYPE = C.GRANT_TYPE AND LPAD(A.CHILD_ID_NO,13,0) = LPAD(C.ID_NO,13,0)
                        LEFT JOIN cust_rescodes@sassa_socpen D ON b.secondary_paypoint = d.res_code
                        WHERE A.Application_date >= to_date('{bookmark.ToString("dd/MMM/yyyy")}') 
                        AND not exists (SELECT E.ADABAS_ISN_MAIN FROM DC_SOCPEN E WHERE E.ADABAS_ISN_MAIN = A.ADABAS_ISN)";

                Globals.Progress = "Insert new SOCPEN Grants.";
                JsonFileUtils.WriteJson(Globals, fileName);
                await _raw.ExecuteNonQuery(sql);

                sql = @"update dc_socpen a
                set a.status_code = (select b.status_code from vw_grant_applications b where b.adabas_isn_main = a.adabas_isn_main)
                WHERE a.adabas_isn_main is not null";

                Globals.Progress = "Sync Status.";
                JsonFileUtils.WriteJson(Globals, fileName);
                await _raw.ExecuteNonQuery(sql);

                //Insert TRELAtional Archive Apps

                //sql = $@"update DC_SOCPEN f set CAPTURE_REFERENCE =
                //(
                //        select b.unq_file_no from dc_file b
                //        where b.APPLICANT_NO = f.Beneficiary_id
                //        and rownum = 1
                //        AND (b.GRANT_TYPE = f.GRANT_TYPE or b.GRANT_TYPE = '3' AND f.GRANT_TYPE = '0')
                //        and f.APPlication_DATE <= b.updated_date
                //        and b.updated_date is not null
                //        and f.CAPTURE_DATE is null
                //        and not exists(Select capture_reference from DC_SOCPEN dc where dc.capture_reference = b.unq_file_no)
                //) 
                //where CAPTURE_DATE is null
                //AND Application_date >= to_date('{bookmark.ToString("dd/MMM/yyyy")}')";
                //Globals.Progress = "Capture ref from dc file.";
                //await _raw.ExecuteNonQuery(sql);

                //sql = $@"UPDATE
                //    (SELECT DC_SOCPEN.LOCALOFFICE_ID as OLD, DC_FILE.OFFICE_ID as NEW
                //        FROM DC_SOCPEN
                //        INNER JOIN DC_FILE
                //        ON DC_SOCPEN.capture_reference = DC_FILE.unq_file_no
                //    ) t
                //    SET t.OLD = t.NEW";
                //Globals.Progress = "Local Office from dc file.";
                //await _raw.ExecuteNonQuery(sql);


                //sql = $@"UPDATE
                //    (SELECT DC_SOCPEN.BRM_BARCODE as OLD, DC_FILE.BRM_BARCODE as NEW
                //        FROM DC_SOCPEN
                //        INNER JOIN DC_FILE
                //        ON DC_SOCPEN.capture_reference = DC_FILE.unq_file_no
                //    ) t
                //    SET t.OLD = t.NEW";
                //Globals.Progress = "BarCode from dc file.";
                //await _raw.ExecuteNonQuery(sql);


                //Update Status from Trelational

                Globals.Progress = "Done.";
                //string message = $"'text':'BRM {DateTime.Now.ToShortTimeString()}: Socpen data from TRelational sucessfully refreshed.'";
                //using (var httpClient = new HttpClient())
                //{
                //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), SassaTeamsUrl))
                //    {
                //        request.Content = new StringContent($"{message}");
                //        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                //        var response = await httpClient.SendAsync(request);
                //    }
                //}
                JsonFileUtils.WriteJson(Globals, fileName);
            }
            catch (Exception ex)
            {
                Globals.Progress = $"{Globals.Progress} : {ex.Message}";
                JsonFileUtils.WriteJson(Globals, fileName);
            }
        }

        private async Task SyncBRM(object? state)
        {
            try 
            {
                string sql = $@"update DC_SOCPEN f set CAPTURE_REFERENCE =
                (
                        select b.unq_file_no from dc_file b
                        where b.APPLICANT_NO = f.Beneficiary_id
                        and rownum = 1
                        AND (b.GRANT_TYPE = f.GRANT_TYPE or b.GRANT_TYPE = '3' AND f.GRANT_TYPE = '0')
                        and f.APPlication_DATE <= b.updated_date
                        and b.updated_date is not null
                        and f.CAPTURE_DATE is null
                        and brm_barcode is not null
                        and not exists(Select capture_reference from DC_SOCPEN dc where dc.capture_reference = b.unq_file_no)
                ) 
                where CAPTURE_DATE is null
                and localOffice_id is not null
                AND Application_date >= to_date('{bookmark.ToString("dd/MMM/yyyy")}')";
                Globals.Progress = "Capture ref from dc file.";
                await _raw.ExecuteNonQuery(sql);

            }
            catch(Exception ex)
            {
                Globals.Progress = $"{Globals.Progress} : {ex.Message}";
            }

        }

        //todo: batch this update monthly
        //for now manual run on TDW data receipt
        private async void SyncTDW(object? state)
        {
            try
            {
                //22/Mar/2017
                int startyear = 2004;
                int startmonth = 1;
                //int startday = 1;


                for (int year = startyear; year <= 2022; year++)
                {
                    for (int month = startmonth; month <= 12; month++)
                    {
                        //for (int day = startday; day <= DateTime.DaysInMonth(year, month); day++)
                        //{
                            Globals.Progress = new DateTime(year, month, 1).ToString("dd/MMM/yyyy");
                            string sql = "update DC_SOCPEN f set BRM_BARCODE = ( " +
                            " select filefolder_code from tdw_file_location b" +
                            " where b.Description = f.Beneficiary_id" +
                            " and rownum = 1" +
                            " AND (b.GRANT_TYPE = f.GRANT_TYPE OR b.GRANT_TYPE = '3' AND f.GRANT_TYPE = '0')" +
                            " and f.CAPTURE_REFERENCE is null" +
                            " and b.filefolder_code is not null" +
                            //" and not exists(Select BRM_BARCODE from DC_SOCPEN dc where dc.BRM_BARCODE = b.filefolder_code)" +
                            ") " +
                            $" where f.BRM_BARCODE is null and Application_date >= to_date('{new DateTime(year, month, 1).ToString("dd/MMM/yyyy")}') and Application_date <= to_date('{new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("dd/MMM/yyyy")}')";
                            await _raw.ExecuteNonQuery(sql);

                            sql = $"UPDATE dc_SOCPEN dc SET dc.TDW_REC = Application_date  where dc.BRM_BARCODE is not null and dc.Capture_reference is null AND Application_date >= to_date('{new DateTime(year, month,1).ToString("dd/MMM/yyyy")}') and Application_date <= to_date('{new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("dd/MMM/yyyy")}')";
                            await _raw.ExecuteNonQuery(sql);
                           
                        //}
                        //startday = 1;

                    }
                    startmonth = 1;
                }
                Globals.Progress = "Done";
            }
            catch(Exception ex)
            {
                Globals.Progress = $"{Globals.Progress} : {ex.Message}";
            }

        }
        private void NewCodeForPersonal()
        {
//            INSERT INTO DC_SOCPENPG(ADABAS_ISN_ARCHIVE, BENEFICIARY_ID, CHILD_ID, NAME, SURNAME, GRANT_TYPE, REGION_ID, APPLICATION_DATE, APPROVAL_DATE, STATUS_DATE, STATUS_CODE, UNIQUE_ID, PAYPOINT)
//SELECT
//    A.ADABAS_ISN AS ADABAS_ISN_ARCHIVE,
//    LPAD(A.PENSION_NO, 13, 0) AS BENEFICIARY_ID,
//    C.ID_NO AS CHILD_ID,
//    B.NAME_EXT,
//    B.SURNAME_EXT,
//    A.GRANT_TYPE,
//    D.REGION_CODE AS REGION_ID,
//    NVL(A.APPLICATION_DATE, C.APPLICATION_DATE) AS APPLICATION_DATE,
//    C.APPROVAL_DATE,
//    C.STATUS_DATE,
//    CASE
//        WHEN A.PRIM_STATUS IN('B','A','9') AND A.SEC_STATUS IN('2') THEN 'ACTIVE'
//        ELSE 'INACTIVE'
//    END AS STATUS_CODE,
//    NULL AS UNIQUE_ID,
//    B.secondary_paypoint as paypoint
//FROM sassa_archive.socpen_personal_grants_archive A
//JOIN sassa_archive.SOCPEN_PERSONAL_ARCHIVE B ON A.PENSION_NO = B.PENSION_NO
//LEFT JOIN sassa_archive.SOCPEN_P12_CHILDREN_ARCHIVE C ON A.PENSION_NO = c.pension_no AND A.GRANT_TYPE = C.GRANT_TYPE
//LEFT JOIN sassa.cust_rescodes D ON b.secondary_paypoint = d.res_code;
//            commit;
//            INSERT INTO DC_SOCPENPG(ADABAS_ISN_MAIN, BENEFICIARY_ID, CHILD_ID, NAME, SURNAME, GRANT_TYPE, REGION_ID, APPLICATION_DATE, APPROVAL_DATE, STATUS_DATE, STATUS_CODE, UNIQUE_ID, PAYPOINT)
//SELECT
//    A.ADABAS_ISN AS ADABAS_ISN_MAIN,
//    LPAD(A.PENSION_NO, 13, 0) AS BENEFICIARY_ID,
//    C.ID_NO AS CHILD_ID,
//    B.NAME_EXT,
//    B.SURNAME_EXT,
//    A.GRANT_TYPE,
//    D.REGION_CODE AS REGION_ID,
//    NVL(A.APPLICATION_DATE, C.APPLICATION_DATE) AS APPLICATION_DATE,
//    C.APPROVAL_DATE,
//    C.STATUS_DATE,
//    CASE
//        WHEN A.PRIM_STATUS IN('B','A','9') AND A.SEC_STATUS IN('2') THEN 'ACTIVE'
//        ELSE 'INACTIVE'
//    END AS STATUS_CODE,
//    NULL AS UNIQUE_ID,
//    B.secondary_paypoint as paypoint
//FROM sassa.socpen_personal_grants A
//JOIN SOCPEN_PERSONAL B ON  A.PENSION_NO = B.PENSION_NO
//LEFT JOIN SOCPEN_P12_CHILDREN C ON A.PENSION_NO = c.pension_no AND A.GRANT_TYPE = C.GRANT_TYPE
//LEFT JOIN sassa.cust_rescodes D ON b.secondary_paypoint = d.res_code;

//            commit;
        }
        /// <summary>
        /// This SQL needs to run if we receive updates for MIS or ss_application
        /// </summary>
        private void MISSQL()
        {
//            update DC_SOCPEN f set MIS_FILES =
//(select File_number from mis_livelink_file b
//where b.ID_NUMBER = f.Beneficiary_id
//and rownum = 1
//AND(b.GRANT_TYPE = f.GRANT_TYPE OR b.GRANT_TYPE = '3' AND f.GRANT_TYPE = '0')
//and f.CAPTURE_REFERENCE is null)
//where f.MIS_FILES is null and f.BRM_BARCODE is null and Application_date >= to_date('30/Mar/2016')  and Application_date<to_date('01/Jan/2019');
//            commit;

//            update DC_SOCPEN f set MIS_FILES =
//            (select 'SSARecord' from SS_APPLICATION b
//            where b.ID_NUMBER = f.Beneficiary_id
//            and rownum = 1
//AND(b.GRANT_TYPE = f.GRANT_TYPE OR b.GRANT_TYPE = '3' AND f.GRANT_TYPE = '0')
//and f.CAPTURE_REFERENCE is null)
//where f.MIS_FILES is null and f.BRM_BARCODE is null;
//            commit;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Timed Hosted Service is stopping.");
            schedule?.Change(Timeout.Infinite, 0);
            //_timerTDW?.Change(Timeout.Infinite, 0);
            //_timerBRM?.Change(Timeout.Infinite, 0);
            //_timerLO?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            schedule?.Dispose();
            //_timerTDW?.Dispose();
            //_timerBRM?.Dispose();
            //_timerLO?.Dispose();

        }

        public class GlobalVars
        {
            public DateTime? LastRunDate { get; set; }
            public bool Status { get; set; }
            public string? Progress { get; set; }
            public DateTime NextRefreshDate { get; set; } = System.DateTime.Now;
            public string TimeString { 
                get
                {
                    return NextRefreshTime.ToString("HH:mm");
                }
                set 
                {
                    var hhmm = value.Split(":");

                    NextRefreshTime = new TimeOnly(int.Parse(hhmm[0]), int.Parse(hhmm[1]));
                } 
            }
            public TimeOnly NextRefreshTime  = TimeOnly.FromDateTime(DateTime.Now);
        }
    }
}