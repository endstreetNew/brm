using razor.Components.Models;
using Sassa.BRM.Data.ViewModels;
using Sassa.BRM.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sassa.BRM.Services
{

    
    public class TdwBatchService
    {
        ModelContext _context;
        StaticService sservice;
        UserSession _session;
        public TdwBatchService(ModelContext context,StaticService staticService)
        {
            _context = context;
            sservice = staticService;
        }

    

    public async Task<PagedResult<TdwBatchViewModel>> GetAllBoxes(int page)
    {

        PagedResult<TdwBatchViewModel> result = new PagedResult<TdwBatchViewModel>();
        try
        {
            List<DcFile> allFiles = new List<DcFile>();

            allFiles = await _context.DcFiles.Where(bn => bn.TdwBatch == 0 && bn.RegionId == _session.Office.RegionId && bn.ApplicationStatus.Contains("LC") && !string.IsNullOrEmpty(bn.TdwBoxno)).ToListAsync();


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
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public async Task<PagedResult<TdwBatchViewModel>> GetHistoryBoxes(int page)
    {


        PagedResult<TdwBatchViewModel> result = new PagedResult<TdwBatchViewModel>();
        try
        {


            List<DcFile> allFiles = await _context.DcFiles.Where(bn => bn.RegionId == _session.Office.RegionId && !string.IsNullOrEmpty(bn.TdwBoxno)).AsNoTracking().ToListAsync();

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
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return result;
    }
    }
}
