﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sassa.Brm.Common.Models;
using Sassa.BRM.Models;
using Sassa.BRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sassa.Brm.Common.Services;
using Microsoft.EntityFrameworkCore.Internal;
using DocumentFormat.OpenXml.Bibliography;

namespace Sassa.Brm.Common.Services
{
    public class StaticService
    {

        public bool IsInitialized { get; set; }
        IDbContextFactory<ModelContext> _contextFactory;
        public StaticService(IDbContextFactory<ModelContext> contextFactory, IConfiguration config, IWebHostEnvironment env)
        {
            _contextFactory = contextFactory;
            StaticDataService.ReportFolder = Path.Combine(env.ContentRootPath, @$"wwwroot\{config["Folders:Reports"]!}\");
            StaticDataService.DocumentFolder = $"{env.WebRootPath}\\{config.GetValue<string>("Folders:CS")}\\";
            Initialize();
        }

        #region Static Data access

        private void Initialize()
        {

            StaticDataService.TransactionTypes = new Dictionary<int, string>();
            StaticDataService.TransactionTypes.Add(0, "Application");
            StaticDataService.TransactionTypes.Add(1, "Loose Correspondence");
            StaticDataService.TransactionTypes.Add(2, "Review");

            using (var context = _contextFactory.CreateDbContext())
            {
                StaticDataService.Regions = context.DcRegions.AsNoTracking().ToList();
                StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
                StaticDataService.DcOfficeKuafLinks = context.DcOfficeKuafLinks.AsNoTracking().ToList();
                StaticDataService.GrantTypes =  context.DcGrantTypes.AsNoTracking().ToDictionary(key => key.TypeId, value => value.TypeName);
                StaticDataService.LcTypes =  context.DcLcTypes.AsNoTracking().ToDictionary(key => key.Pk, value => value.Description);
                StaticDataService.ServicePoints =  context.DcFixedServicePoints.AsNoTracking().ToList();
                StaticDataService.DcOfficeKuafLinks =  context.DcOfficeKuafLinks.AsNoTracking().ToList();
                StaticDataService.RequiredDocs =  (from reqDocGrant in context.DcGrantDocLinks
                                                  join reqDoc in context.DcDocumentTypes on reqDocGrant.DocumentId equals reqDoc.TypeId
                                                  where reqDocGrant.CriticalFlag == "Y"
                                                  orderby reqDocGrant.Section, reqDoc.TypeId ascending
                                                  select new RequiredDocsView
                                                  {
                                                      GrantType = reqDocGrant.GrantId,
                                                      DOC_ID = reqDoc.TypeId,
                                                      DOC_NAME = reqDoc.TypeName,
                                                      DOC_SECTION = reqDocGrant.Section,
                                                      DOC_CRITICAL = reqDocGrant.CriticalFlag
                                                  }).Distinct().AsNoTracking().ToList();
                StaticDataService.BoxTypes =  context.DcBoxTypes.AsNoTracking().ToList();
                StaticDataService.RequestCategoryTypeLinks =  context.DcReqCategoryTypeLinks.AsNoTracking().ToList();
                StaticDataService.RequestCategoryTypes =  context.DcReqCategoryTypes.AsNoTracking().ToList();
                StaticDataService.RequestCategories =  context.DcReqCategories.OrderBy(e => e.CategoryDescr).AsNoTracking().ToList();
                StaticDataService.StakeHolders =  context.DcStakeholders.Distinct().AsNoTracking().ToList();
                
            }
            IsInitialized = true;
        }

        public string GetTransactionType(int key)
        {
            return StaticDataService.TransactionTypes![key];
        }

        /// <summary>
        /// User office change
        /// Used by session service to update user office
        /// </summary>
        /// <param name="samName"></param>
        /// <param name="supervisor"></param>
        /// <returns></returns>
        public UserOffice GetUserLocalOffice(string userName)
        {

            var office = StaticDataService.LocalOffices!
            .Join(StaticDataService.DcOfficeKuafLinks!.Where(l => l.Username == userName),
            lo => lo.OfficeId,
            link => link.OfficeId,
            (lo, link) => new UserOffice(lo, link.FspId)).FirstOrDefault();

            if (office is null)
            {
                //Add new users to Gauteng office by default
                DcLocalOffice defaultOffice = GetOffices("7").FirstOrDefault()!;
                office = new UserOffice(defaultOffice, null);
            }
            office.RegionName = GetRegion(office.RegionId);
            office.RegionCode = GetRegionCode(office.RegionId);
            return office;

        }


        public DcLocalOffice GetLocalOffice(string officeId)
        {
            return StaticDataService.LocalOffices!.Where(lo => lo.OfficeId == officeId).FirstOrDefault()!;
        }
        public List<DcFixedServicePoint> GetServicePoints(string regionID)
        {
            return StaticDataService.ServicePoints!.Where(sp => StaticDataService.LocalOffices!.Where(lo => lo.RegionId == regionID).Select(l => l.OfficeId).ToList().Contains(sp.OfficeId.ToString())).ToList();
        }
        public List<DcFixedServicePoint> GetOfficeServicePoints(string officeID)
        {
            return StaticDataService.ServicePoints!.Where(sp => sp.OfficeId == officeID).ToList();
        }
        public string GetServicePointName(decimal? fspID)
        {
            var result = StaticDataService.ServicePoints!.Where(sp => sp.Id == fspID);
            if (result.Any())
            {
                return result.First().ServicePointName;
            }
            return "";
        }

        public async Task<bool> UpdateUserLocalOffice(string officeId, UserSession session)
        {
            DcOfficeKuafLink officeLink;
            using (var context = _contextFactory.CreateDbContext())
            {
                var query = await context.DcOfficeKuafLinks.Where(okl => okl.Username == session.SamName).ToListAsync();
                if (query.Count() > 1)
                {
                    foreach (var ol in query)
                    {
                        context.DcOfficeKuafLinks.Remove(ol);
                    }
                    await context.SaveChangesAsync();
                    query = await context.DcOfficeKuafLinks.Where(okl => okl.Username == session.SamName).ToListAsync();
                }

                if (query.Any())
                {
                    officeLink = query.First();
                    officeLink.OfficeId = officeId;
                    officeLink.FspId = session.Office?.FspId;
                }
                else
                {
                    officeLink = new DcOfficeKuafLink() { OfficeId = officeId, FspId = session.Office?.FspId, Username = session.SamName, Supervisor = session.IsInRole("GRP_BRM_Supervisors") ? "Y" : "N" };
                    context.DcOfficeKuafLinks.Add(officeLink);
                }
                try
                {
                    await context.SaveChangesAsync();
                    //Update the staticData
                    StaticDataService.DcOfficeKuafLinks = await context.DcOfficeKuafLinks.AsNoTracking().ToListAsync();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return true;
        }
        public string GetRegion(string regionId)
        {
            if (regionId == null) return "Unknown";
            return StaticDataService.Regions!.Where(r => r.RegionId == regionId).First().RegionName;
        }
        public string GetRegionCode(string regionId)
        {
            return StaticDataService.Regions!.Where(r => r.RegionId == regionId).First().RegionCode;
        }
        public Dictionary<string, string> GetRegions()
        {
            return StaticDataService.Regions!.ToDictionary(key => key.RegionId, value => value.RegionName); ;
        }
        public List<DcLocalOffice> GetOffices(string regionId)
        {
            return StaticDataService.LocalOffices!.Where(o => o.RegionId == regionId).ToList();
        }
        public async Task ChangeOfficeStatus(string officeId, string status)
        {
            using(var context = _contextFactory.CreateDbContext())
            {
                DcLocalOffice lo = await context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
                lo.ActiveStatus = status;
                await context.SaveChangesAsync();
                StaticDataService.LocalOffices = await context.DcLocalOffices.AsNoTracking().ToListAsync();
            }
        }
        public async Task ChangeOfficeName(string officeId, string name)
        {
            using(var context = _contextFactory.CreateDbContext())
            {
                DcLocalOffice lo = await context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
                lo.OfficeName = name;
                await context.SaveChangesAsync();
                StaticDataService.LocalOffices = await context.DcLocalOffices.AsNoTracking().ToListAsync();
            }
        }
        public async Task MoveOffice(string fromOfficeId, int toOfficeId)
        {
            
            using(var context = _contextFactory.CreateDbContext())
            {
                //DC_FIle
                var oldOfficerecs = await context.DcFiles.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
                foreach (var file in oldOfficerecs)
                {
                    file.OfficeId = toOfficeId.ToString();
                }
                await context.SaveChangesAsync();
                //DC_FIXED_SERVICE_POINT
                var oldFsprecs = await context.DcFixedServicePoints.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
                foreach (var fsp in oldFsprecs)
                {
                    fsp.OfficeId = toOfficeId.ToString();
                }
                await context.SaveChangesAsync();
                //DC_OFFICE_KUAF_LINK
                var oldKuafrecs = await context.DcOfficeKuafLinks.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
                foreach (var kuaf in oldKuafrecs)
                {
                    kuaf.OfficeId = toOfficeId.ToString();
                }
                await context.SaveChangesAsync();

                //DC_Batches
                var oldBatchRecs = await context.DcBatches.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
                foreach (var batch in oldBatchRecs)
                {
                    batch.OfficeId = toOfficeId.ToString();
                }
                await context.SaveChangesAsync();
            }
            await DeleteLocalOffice(fromOfficeId);
        }
        public async Task SaveManualBatch(string officeId, string manualBatch)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                DcLocalOffice lo = await context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
                lo.ManualBatch = manualBatch;
                await context.SaveChangesAsync();
                StaticDataService.LocalOffices = await context.DcLocalOffices.AsNoTracking().ToListAsync();
            }
        }
        public async Task DeleteLocalOffice(string officeId)
        {
            using(var context = _contextFactory.CreateDbContext())
            {
                DcLocalOffice lo = await context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
                context.DcLocalOffices.Remove(lo);
                await context.SaveChangesAsync();
                StaticDataService.LocalOffices = await context.DcLocalOffices.AsNoTracking().ToListAsync();
            }
        }
        public async Task UpdateServicePoint(DcFixedServicePoint s)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                DcFixedServicePoint sp = await context.DcFixedServicePoints.Where(o => o.Id == s.Id).FirstAsync();
                sp.ServicePointName = s.ServicePointName;
                sp.OfficeId = s.OfficeId;
                await context.SaveChangesAsync();
                StaticDataService.ServicePoints = await context.DcFixedServicePoints.AsNoTracking().ToListAsync();
            }
        }
        public async Task CreateOffice(RegionOffice office)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                DcLocalOffice lo = new DcLocalOffice();
                lo.OfficeName = office.OfficeName;
                lo.OfficeId = (int.Parse(await context.DcLocalOffices.MaxAsync(o => o.OfficeId)!) + 1).ToString();
                lo.RegionId = office.RegionId;
                lo.ActiveStatus = "A";
                lo.OfficeType = "LO";
                context.DcLocalOffices.Add(lo);
                await context.SaveChangesAsync();
                StaticDataService.LocalOffices = await context.DcLocalOffices.AsNoTracking().ToListAsync();
            }
        }
        public async Task CreateServicePoint(DcFixedServicePoint s)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.DcFixedServicePoints.Add(s);
                await context.SaveChangesAsync();

                StaticDataService.ServicePoints = await context.DcFixedServicePoints.AsNoTracking().ToListAsync();
            }
        }
        public List<string> GetOfficeIds(string regionId)
        {
            List<DcLocalOffice> offices = GetOffices(regionId);
            return (from office in offices select office.OfficeId).ToList();
        }
        public string GetOfficeName(string officeId)
        {
            return StaticDataService.LocalOffices!.Where(o => o.OfficeId == officeId).First().OfficeName;
        }
        public string GetFspName(decimal? fspId)
        {

            if (fspId == null) return "";
            if (StaticDataService.ServicePoints!.Where(o => o.Id == fspId).Any())
            {
                return StaticDataService.ServicePoints!.Where(o => o.Id == fspId).First().ServicePointName;
            }
            return "";
        }
        public string GetOfficeType(string officeId)
        {
            return StaticDataService.LocalOffices!.Where(o => o.OfficeId == officeId).First().OfficeType;
        }
        public string GetGrantType(string grantId)
        {
            return StaticDataService.GrantTypes![grantId];
        }
        public string GetGrantId(string grantType)
        {
            return StaticDataService.GrantTypes!.Where(g => g.Value == grantType).First().Key;
        }
        public Dictionary<string, string> GetGrantTypes()
        {
            return StaticDataService.GrantTypes!;
        }
        public string GetLcType(decimal lcId)
        {
            return StaticDataService.LcTypes![lcId];
        }
        public Dictionary<decimal, string> GetLcTypes()
        {
            return StaticDataService.LcTypes!;
        }
        public List<RequiredDocsView> GetGrantDocuments(string grantType)
        {
            return StaticDataService.RequiredDocs!.Where(r => r.GrantType == grantType).OrderBy(g => g.DOC_SECTION).ThenBy(g => g.DOC_ID).ToList();
        }
        /// <summary>
        /// Transport Y or N
        /// </summary>
        /// <param name="transport"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetBoxTypes(string transport)
        {
            var result = StaticDataService.BoxTypes!.Where(d => d.IsTransport == transport).ToDictionary(i => i.BoxTypeId.ToString(), i => i.BoxType);
            return result;
        }
        public Dictionary<string, string> GetBoxTypes()
        {
            var result = StaticDataService.BoxTypes!.ToDictionary(i => i.BoxTypeId.ToString(), i => i.BoxType);
            return result;
        }
        public Dictionary<string, string> GetYearList()
        {
            Dictionary<string, string> years = new Dictionary<string, string>();
            int start = 2000;
            int end = DateTime.Now.Year;
            for (int i = end; i >= start; i--)
            {
                years.Add(i.ToString(), i.ToString());
            }
            return years;
        }
        public Dictionary<string, string> GetRequestCategories()
        {
            return StaticDataService.RequestCategories!.ToDictionary(i => i.CategoryId.ToString(), i => i.CategoryDescr);
         }
        public Dictionary<string, string> GetRequestCategoryTypes()
        {
            return StaticDataService.RequestCategoryTypes!.ToDictionary(i => i.TypeId.ToString(), i => i.TypeDescr);
        }
        public Dictionary<string, string> GetRequestCategoryTypes(string CategoryId)
        {
            if (string.IsNullOrEmpty(CategoryId)) return new Dictionary<string, string>();
            decimal.TryParse(CategoryId, out decimal catid);
            return (from r in StaticDataService.RequestCategoryTypes!
                      join c in StaticDataService.RequestCategoryTypeLinks!
                             on r.TypeId equals c.TypeId
                      where c.CategoryId == catid
                      select r).ToDictionary(i => i.TypeId.ToString(), i => i.TypeDescr);

        }
        public Dictionary<string, string> GetStakeHolders()
        {
            var result = StaticDataService.StakeHolders!.Distinct().ToDictionary(i => i.StakeholderId.ToString(), i => i.Name + " " + i.Surname);
            result.Add("", "");
            return result;
        }
        public Dictionary<string, string> GetStakeHolders(string DepartmentId)
        {
            decimal did;
            decimal.TryParse(DepartmentId, out did);
            var result = StaticDataService.StakeHolders!.Where(s => s.DepartmentId == did).ToDictionary(i => i.StakeholderId.ToString(), i => i.Name + " " + i.Surname);
            result.Add("", "");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeType">LO or RMC</param>
        /// <returns></returns>
        public Dictionary<string, string> GetBatchStatus(string officeType)
        {
            Dictionary<string, string> batchStatus = new Dictionary<string, string>();
            if (officeType == "LO")
            {
                batchStatus.Add("Open", "Open");
                batchStatus.Add("Closed", "Closed");
                batchStatus.Add("Transport", "Transport");
            }
            else
            {
                batchStatus.Add("Transport", "Transport");
                batchStatus.Add("Received", "Received");
                batchStatus.Add("RMCBatch", "RMCBatch");
            }
            return batchStatus;
        }

        #endregion
    }
}