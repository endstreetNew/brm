﻿using Sassa.BRM.Models;
using Sassa.BRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sassa.BRM.Services
{
    public class StaticD
    {
        public StaticD()
        {
        }

        public static string RsWeb { get; set; }
        public static string ReportFolder { get; set; }
        public static string DocumentFolder { get; set; }
        public static Dictionary<int, string> TransactionTypes { get; set; }
        public static List<DcRegion> Regions { get; set; }
        public static string RegionCode(string RegionId)
        {
            if (RegionId == null) return "UKN";
            return Regions.Where(r => r.RegionId == RegionId).First().RegionCode;
        }
        public static string RegionName(string RegionId)
        {
            if (RegionId == null) return "Unknown";
            return Regions.Where(r => r.RegionId == RegionId).First().RegionName;
        }
        public static List<DcLocalOffice> LocalOffices { get; set; }
        public static List<DcFixedServicePoint> ServicePoints { get; set; }
        public static List<DcOfficeKuafLink> DcOfficeKuafLinks { get; set; }
        public static Dictionary<string, string> GrantTypes { get; set; }
        public static string GrantName(string GrantId)
        {
            return GrantTypes[GrantId];
        }
        public static Dictionary<decimal, string> LcTypes { get; set; }
        public static List<RequiredDocsView> RequiredDocs { get; set; }
        public static List<DcBatch> ActiveBatches { get; set; }
        public static List<DcBoxType> BoxTypes { get; set; }
        public static List<DcReqCategory> RequestCategories { get; set; }
        public static List<DcStakeholder> StakeHolders { get; set; }
        public static List<DcReqCategoryType> RequestCategoryTypes { get; set; }
        public static List<DcReqCategoryTypeLink> RequestCategoryTypeLinks { get; set; }
        public static Dictionary<string, string> RegionEmails { get; set; }
        public static Dictionary<string, string> RegionIDEmails { get; set; }
        public static Dictionary<string, List<FileEntity>> UserBox { get; set; }
        public static string RegionId(string region)
        {
            switch (region.ToUpper())
            {
                case "GAUTENG":
                    return "7";
                case "FREE STATE":
                    return "4";
                case "KWA-ZULU NATAL":
                case "KWAZULU NATAL":
                    return "5";
                case "NORTH WEST":
                    return "6";
                case "MPUMALANGA":
                    return "8";
                case "EASTERN CAPE":
                    return "2";
                case "WESTERN CAPE":
                    return "1";
                case "LIMPOPO":
                    return "9";
                case "NORTHERN CAPE":
                    return "3";
                default:
                    return "";
            }
        }

        public static string GetArchiveYear(DateTime? appDate, string statusCode)
        {
            if (appDate == null) return null;
            return statusCode.ToUpper() != "ACTIVE" ? ((DateTime)appDate).ToString("yyyy") : null;

        }
        public static List<string> PastelColors
        {
            get
            {
                return new List<string> { "#98D4BB", "#E5DB9C", "#D0BCAC", "#BEB4C5", "#E6A57E", "#218B82", "#F5BFD2" };
            }
        }

        private static Dictionary<string, string> _requestStatus;
        public static Dictionary<string, string> RequestStatus
        {
            get
            {
                if (_requestStatus != null) return _requestStatus;

                _requestStatus = new Dictionary<string, string>();
                _requestStatus.Add("TDWPicklist", "Awaiting request.");
                //_requestStatus.Add("NotAtTDW", "Not At TDW");
                _requestStatus.Add("Requested", "Requested from TDW");
                _requestStatus.Add("Received", "Received from TDW (Scanned)");
                //_requestStatus.Add("Scanned", "Scanned image available.");
                _requestStatus.Add("Returned", "Compliant (Returned).");
                //_requestStatus.Add("NonCompliant", "Non Compliant.");
                //_requestStatus.Add("Exception", "Exception.");
                return _requestStatus;
            }
        }

        private static Dictionary<string, string> _picklistStatus;
        public static Dictionary<string, string> PickListStatus
        {
            get
            {
                if (_picklistStatus != null) return _picklistStatus;
                _picklistStatus = new Dictionary<string, string>();
                _picklistStatus.Add("Requested", "Requested from TDW");
                _picklistStatus.Add("Received", "Received");
                //_picklistStatus.Add("Scanned", "Scanned.");
                _picklistStatus.Add("Returned", "Returned.");
                return StaticD._picklistStatus;
            }

        }

        private static Dictionary<string, string> _tdwregions;
        public static Dictionary<string, string> TdwRegions
        {
            get
            {
                if (_tdwregions != null) return _tdwregions;
                _tdwregions = new Dictionary<string, string>();
                _tdwregions.Add("7", "GAUTENG");
                _tdwregions.Add("4", "FREE STATE");
                _tdwregions.Add("5", "KWA-ZULU NATAL");
                _tdwregions.Add("6", "NORTH WEST");
                _tdwregions.Add("8", "MPUMALANGA");
                _tdwregions.Add("2", "EASTERN CAPE");
                _tdwregions.Add("1", "WESTERN CAPE");
                _tdwregions.Add("9", "LIMPOPO");
                _tdwregions.Add("3", "NORTHERN CAPE");
                return _tdwregions;

            }
        }

        private static Dictionary<string, ReportPeriod> _quarters = new Dictionary<string, ReportPeriod>();

        private static Dictionary<string, string> _months = new Dictionary<string, string>();

        public static ReportPeriod AfterLastQuarter
        {
            get
            {
                ReportPeriod result = new ReportPeriod();
                result.FinancialQuarter = "After Last Quarter";
                result.QuarterName = "After Last Quarter";
                result.FromDate = _quarters.Last().Value.ToDate.AddDays(1);
                result.ToDate = DateTime.Now;
                return result;
            }
        }
        public static Dictionary<string, ReportPeriod> QuarterList(int startyear = 2004, int startQuarter = 1)
        {

            if (_quarters.Count == 0)
            {
                for (int year = startyear; year <= DateTime.Now.Year; year++)
                {

                    for (int quarter = startQuarter; quarter <= 4; quarter++) //each quarter
                    {
                        DateTime date = new DateTime(year, quarter * 3, 1);
                        if (date > DateTime.Now.AddDays(-100)) continue;
                        int quarterNumber = (date.Month - 1) / 3 + 1;
                        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
                        DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
                        _quarters.Add(FinancialQuarter(year, quarterNumber), new ReportPeriod { FinancialQuarter = FinancialQuarter(year, quarterNumber), QuarterName = $"{year}-Q{quarterNumber}", FromDate = firstDayOfQuarter, ToDate = lastDayOfQuarter });
                    }
                    startQuarter = 1;
                }
            }
            return _quarters;
        }

        private static string FinancialQuarter(int year, int quarterNumber)
        {
            int finQuarter = quarterNumber - 1 == 0 ? 4 : quarterNumber - 1;
            int finYear = quarterNumber - 1 == 0 ? year - 1 : year;
            return $"FinQuarter {finQuarter} of {finYear} ({year}-Q{quarterNumber})";
        }
        /// <summary>
        /// Selected QuarterList
        /// </summary>
        /// <param name="fromQuarter"></param>
        /// <param name="toQuarter"></param>
        /// <returns></returns>
        public static Dictionary<string, ReportPeriod> QuarterList(ReportPeriod fromQuarter, ReportPeriod toQuarter)
        {
            Dictionary<string, ReportPeriod> quarters = new Dictionary<string, ReportPeriod>();
            foreach (ReportPeriod period in _quarters.Values)
            {
                if (period.FromDate >= fromQuarter.FromDate && period.ToDate <= toQuarter.ToDate)
                {
                    quarters.Add(period.FinancialQuarter, period);
                }
            }
            return quarters;
        }

        public static Dictionary<string, string> MonthList()
        {
            if (_months.Count == 0)
            {
                string[] monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;
                for (int year = DateTime.Now.Year; year >= 2014; year--)
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        string name = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                        _months.Add($"{month}|{year}", $"{DateTimeFormatInfo.CurrentInfo.GetMonthName(month)} - {year.ToString()}");
                    }
                }
            }
            return _months;
        }

        public static List<String> _exclusionTypes;
        public static List<String> ExclusionTypes
        {
            get
            {
                if (_exclusionTypes == null)
                {
                    _exclusionTypes = new List<string>() { "PAIA", "FRAUD", "LEGAL", "DEBTORS", "CCA", "AUDIT" };
                }
                return _exclusionTypes;
            }
        }

        public static List<string> DestructionYears
        {
            get
            {
                List<string> years = new List<string>();
                years.Add((DateTime.Now.Year - 1).ToString());
                years.Add(DateTime.Now.Year.ToString());
                return years;
            }
        }

        private static string _version;
        public static string Version()
        {
            if (string.IsNullOrEmpty(_version))
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                _version = fvi.FileVersion;
            }
            return _version;
        }

        public static void WriteEvent(string eventLogEntry)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(eventLogEntry, EventLogEntryType.Error, 101, 1);
            }
        }
    }
}
