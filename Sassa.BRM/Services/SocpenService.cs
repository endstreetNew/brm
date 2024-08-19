﻿using Microsoft.EntityFrameworkCore;
using Sassa.Socpen.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sassa.BRM.Services;

public class SocpenService(IDbContextFactory<SocpenContext> dbContextFactory)
{
    public async Task<List<CustRescode>> GetResCodes(string RegionId)
    {
        using(var spctx = dbContextFactory.CreateDbContext())
        {
            return await spctx.CustRescodes.Where(r => r.RegionCode == decimal.Parse(RegionId)).ToListAsync();
        }

    }

    public async Task ChangeOfficeStatus(decimal rescode, string status)
    {
        using(var spctx = dbContextFactory.CreateDbContext())
        {
            CustRescode lo = await spctx.CustRescodes.Where(o => o.ResCode == rescode).FirstAsync();
            lo.Status = status;
            await spctx.SaveChangesAsync();
        }
    }
    public async Task ChangeOfficeName(decimal rescode, string name)
    {
        using(var spctx = dbContextFactory.CreateDbContext())
        {
            CustRescode lo = await spctx.CustRescodes.Where(o => o.ResCode == rescode).FirstAsync();
            lo.LocalOffice = name;
            await spctx.SaveChangesAsync();
        }

    }
    public async Task LinkBrmOffice(decimal rescode, decimal officeId)
    {
        using (var spctx = dbContextFactory.CreateDbContext())
        {
            CustRescode lo = await spctx.CustRescodes.FindAsync(rescode);
            lo.OfficeId = officeId;
            lo.Status = "A";
            await spctx.SaveChangesAsync();
        }
    }

    //public async Task LinkBrmOffice(decimal rescode, decimal officeId)
    //{
    //    CustRescode lo = await spctx.CustRescodes.Where(o => o.ResCode == rescode).FirstAsync();
    //    lo.OfficeId = officeId;
    //    await spctx.SaveChangesAsync();
    //}

}
