using razor.Components.Models;
using Sassa.BRM.Data.ViewModels;
using Sassa.BRM.ViewModels;
using Sassa.BRM.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using Sassa.BRM.Helpers;

namespace Sassa.BRM.Services
{

    
    public class TdwBatchService
    {
        ModelContext _context;
        StaticService sservice;
        UserSession _session;
        RawSqlService _raw;
        MailMessages _mail;
        public TdwBatchService(ModelContext context,StaticService staticService,SessionService session,RawSqlService raw, MailMessages mail)
        {
            _context = context;
            _session = session.session; 
            sservice = staticService;
            _raw = raw;
            _mail = mail;
        }

    

    public async Task<PagedResult<TdwBatchViewModel>> GetAllBoxes(int page)
    {

        
        try
        {
            //List<DcFile> allFiles = new List<DcFile>();

            List<DcFile> allFiles = await _context.DcFiles.Where(bn => bn.TdwBatch == 0 && bn.RegionId == _session.Office.RegionId && bn.ApplicationStatus.Contains("LC") && !string.IsNullOrEmpty(bn.TdwBoxno)).ToListAsync();
            PagedResult<TdwBatchViewModel> result = new PagedResult<TdwBatchViewModel>();

                if (!allFiles.Any()) return result;


            foreach (var box in allFiles.Select(f => f.TdwBoxno).Distinct().Skip((page - 1) * 20).Take(20).ToList())
            {
                var dcFiles = allFiles.Where(f => f.TdwBoxno == box).ToList();
                result.result.Add(
               new TdwBatchViewModel
               {
                   BoxNo = box,
                   Region = sservice.GetRegion(_session.Office.RegionId),
                   MiniBoxes = (int)dcFiles.Sum(f => f.MiniBoxno),
                   Files = dcFiles.Count(),
                   User = _session.SamName,
                   TdwSendDate = dcFiles.First().TdwBatchDate
               });
            }

            result.count = allFiles.Select(f => f.TdwBoxno).Distinct().Count();
                return result;
            }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }

    public async Task<PagedResult<TdwBatchViewModel>> GetTdwBatches(int page)
    {


        
        try
        {


            List<DcFile> allFiles = await _context.DcFiles.Where(bn => bn.RegionId == _session.Office.RegionId && !string.IsNullOrEmpty(bn.TdwBoxno)).AsNoTracking().ToListAsync();
                PagedResult<TdwBatchViewModel> result = new PagedResult<TdwBatchViewModel>();
                List<DcFile> batchFiles = new List<DcFile>();
            foreach (var batch in allFiles.Select(f => f.TdwBatch).Distinct().Skip((page - 1) * 20).Take(20).ToList())
            {
                var dcFiles = allFiles.Where(f => f.TdwBatch == batch).ToList();
                result.result.Add(
               new TdwBatchViewModel
               {
                   TdwBatchNo = (int)batch,
                   Region = sservice.GetRegion(_session.Office.RegionId),
                   Boxes = dcFiles.Select(a => a.TdwBoxno).Distinct().Count(),
                   Files = dcFiles.Count(),
                   User = dcFiles.First().UpdatedByAd,
                   TdwSendDate = dcFiles.First().TdwBatchDate
               });
            }

            result.count = allFiles.Select(f => f.TdwBatch).Distinct().Count();
                return result;
            }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        
    }

        /// <summary>
        /// Create TDW batch and send mail
        /// </summary>
        /// <param name="boxes"></param>
        /// <returns></returns>
        public async Task<int> CreateTdwBatch(List<TdwBatchViewModel> boxes)
        {
            int tdwBatch = await _raw.GetNextTdwBatch();
            foreach (var box in boxes.Where(b => !string.IsNullOrEmpty(b.BoxNo)))
            {
                box.TdwSendDate = DateTime.Now;
                box.TdwBatchNo = tdwBatch;
                box.User = _session.SamName;
                await _context.DcFiles.Where(f => f.TdwBoxno == box.BoxNo).ForEachAsync(f => { f.TdwBatch = tdwBatch; f.TdwBatchDate = box.TdwSendDate; });
            }
            await _context.SaveChangesAsync();
            await SendTDWBulkReturnedMail(tdwBatch);
            return tdwBatch;
        }

        /// <summary>
        /// New TDW Batch feature
        /// </summary>
        /// <param name="tdwBatchNo"></param>
        /// <returns></returns>
        public async Task SendTDWBulkReturnedMail(int tdwBatchNo)
        {
            List<TDWRequestMain> tpl = new List<TDWRequestMain>();
            List<DcFile> parentlist;
            TDWRequestMain TdwFormat;
            foreach (string boxNo in await _context.DcFiles.Where(bn => bn.TdwBatch == tdwBatchNo).AsNoTracking().Select(b => b.TdwBoxno).Distinct().ToListAsync())
            {

                parentlist = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).AsNoTracking().ToListAsync();
                foreach (DcFile parent in parentlist)
                {
                    TdwFormat = new TDWRequestMain
                    {
                        BRM_No = parent.BrmBarcode,
                        CLM_No = parent.UnqFileNo,
                        Folder_ID = parent.UnqFileNo,
                        Grant_Type = parent.GrantType,
                        Firstname = parent.UserFirstname,
                        Surname = parent.UserLastname,
                        ID_Number = parent.ApplicantNo,
                        Year = parent.UpdatedDate.Value.ToString("YYYY"),
                        Location = parent.TdwBoxno,
                        Reg = parent.RegType,
                        //Bin  = parent. ,
                        Box = parent.MiniBoxno.ToString(),
                        //Pos  = parent. ,
                        UserPicked = ""
                    };
                    tpl.Add(TdwFormat);
                }

            }
            string FileName = _session.Office.RegionCode + "-" + _session.SamName.ToUpper() + $"-TDW_ReturnedBatch_{tdwBatchNo}-" + DateTime.Now.ToShortDateString().Replace("/", "-") + "-" + DateTime.Now.ToShortTimeString().Replace(":", "-");
            //attachment list
            List<string> files = new List<string>();
            //write attachments for manual download/add to mail
            File.WriteAllText(StaticD.ReportFolder + $@"{FileName}.csv", tpl.CreateCSV());
            files.Add(StaticD.ReportFolder + $@"{FileName}.csv");
            //send mail to TDW
            try
            {
                //if (!Environment.MachineName.ToLower().Contains("prod")) return;
                _mail.SendTDWIncoming(_session, tdwBatchNo, files);
            }
            catch
            {
                //ignore confirmation errors
            }
        }
    }
}
