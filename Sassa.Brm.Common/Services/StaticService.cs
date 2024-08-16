using Microsoft.AspNetCore.Hosting;
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

namespace Sassa.Brm.Common.Services
{
    public class StaticService
    {
        ModelContext context;
        public StaticService(IDbContextFactory<ModelContext> _contextFactory, IConfiguration config, IWebHostEnvironment env)
        {

            StaticDataService.ReportFolder = Path.Combine(env.ContentRootPath, config["Folders:Reports"]!) + "\\";
            StaticDataService.DocumentFolder = $"{env.WebRootPath}\\{config.GetValue<string>("Folders:CS")}\\"; //env.ContentRootPath  + "//" + config.GetValue<string>("Folders:CS") + "/";
            context = _contextFactory.CreateDbContext();
        }

        #region Static Data access

        public string GetTransactionType(int key)
        {
            if (StaticDataService.TransactionTypes == null)
            {
                StaticDataService.TransactionTypes = new Dictionary<int, string>();
                StaticDataService.TransactionTypes.Add(0, "Application");
                StaticDataService.TransactionTypes.Add(1, "Loose Correspondence");
                StaticDataService.TransactionTypes.Add(2, "Review");
            }
            return StaticDataService.TransactionTypes[key];
        }
        public async Task<UserOffice> GetUserLocalOffice(string samName, string supervisor)
        {
            if (StaticDataService.LocalOffices == null)
            {
                StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
            }
            if (StaticDataService.DcOfficeKuafLinks == null)
            {
                StaticDataService.DcOfficeKuafLinks = context.DcOfficeKuafLinks.AsNoTracking().ToList();
            }

            //Try local office from static.
            if (!(from lo in StaticDataService.LocalOffices
                  join lou in StaticDataService.DcOfficeKuafLinks
                      on lo.OfficeId equals lou.OfficeId
                  where lou.Username == samName
                  select new
                  {
                      OfficeName = lo.OfficeName,
                      OfficeId = lo.OfficeId,
                      OfficeType = lo.OfficeType,
                      RegionId = lo.RegionId,
                      FspId = lou.FspId

                  }).Any())
            {
                DcLocalOffice ioffice = GetOffices("7").FirstOrDefault()!;
                //Attach to first or default office in Gauteng.
                await UpdateUserLocalOffice(ioffice.OfficeId, null, samName, supervisor);
                //try again..
                await GetUserLocalOffice(samName, supervisor);
            }

            var value = (from lo in StaticDataService.LocalOffices
                         join lou in StaticDataService.DcOfficeKuafLinks
                         on lo.OfficeId equals lou.OfficeId
                         where lou.Username == samName
                         select new
                         {
                             OfficeName = lo.OfficeName,
                             OfficeId = lo.OfficeId,
                             OfficeType = lo.OfficeType,
                             RegionId = lo.RegionId,
                             FspId = lou.FspId

                         }).FirstOrDefault();

            UserOffice Office = new UserOffice();
            Office.OfficeName = value!.OfficeName;
            Office.OfficeId = value.OfficeId;
            Office.OfficeType = value.OfficeType;
            Office.RegionId = value.RegionId;
            Office.FspId = value.FspId;
            Office.RegionName = GetRegion(value.RegionId);
            Office.RegionCode = GetRegionCode(value.RegionId);
            Office.OfficeType = !string.IsNullOrEmpty(value.OfficeType) ? value.OfficeType : "LO"; //Default to local office
            return Office;
            //if (_sessionservice.SessionInitialized != null) _sessionservice.SessionInitialized(this, null);
        }
        public DcLocalOffice GetLocalOffice(string officeId)
        {
            if (StaticDataService.LocalOffices == null)
            {
                StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticDataService.LocalOffices.Where(lo => lo.OfficeId == officeId).FirstOrDefault()!;
        }
        public UserSession SetUserOffice(string officeId)
        {
            if (StaticDataService.LocalOffices == null)
            {
                StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
            }
            var office = StaticDataService.LocalOffices.Where(lo => lo.OfficeId == officeId).FirstOrDefault();
            UserSession _session = new UserSession();
            _session.Office.OfficeName = office!.OfficeName;
            _session.Office.OfficeId = office.OfficeId;
            _session.Office.OfficeType = office.OfficeType;
            _session.Office.RegionId = office.RegionId;
            //_session.Office.FspId = office.FspId;
            _session.Office.RegionName = GetRegion(office.RegionId);
            _session.Office.RegionCode = GetRegionCode(office.RegionId);
            _session.Office.OfficeType = !string.IsNullOrEmpty(office.OfficeType) ? office.OfficeType : "LO"; //Default to local office
            return _session;
        }
        public List<DcFixedServicePoint> GetServicePoints(string regionID)
        {
            if (StaticDataService.ServicePoints == null)
            {
                StaticDataService.ServicePoints = context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            return StaticDataService.ServicePoints.Where(sp => StaticDataService.LocalOffices!.Where(lo => lo.RegionId == regionID).Select(l => l.OfficeId).ToList().Contains(sp.OfficeId.ToString())).ToList();
        }
        public List<DcFixedServicePoint> GetOfficeServicePoints(string officeID)
        {
            if (StaticDataService.ServicePoints == null)
            {
                StaticDataService.ServicePoints = context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            return StaticDataService.ServicePoints.Where(sp => sp.OfficeId == officeID).ToList();
        }
        public string GetServicePointName(decimal? fspID)
        {
            if (StaticDataService.ServicePoints == null)
            {
                StaticDataService.ServicePoints = context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            var result = StaticDataService.ServicePoints.Where(sp => sp.Id == fspID);
            if (result.Any())
            {
                return result.First().ServicePointName;
            }
            return "";
        }
        public async Task<bool> UpdateUserLocalOffice(string officeId, decimal? fspId, string SamName, string supervisor)
        {
            DcOfficeKuafLink officeLink;
            var query = await context.DcOfficeKuafLinks.Where(okl => okl.Username == SamName).ToListAsync();
            if (query.Count() > 1)
            {
                foreach (var ol in query)
                {
                    context.DcOfficeKuafLinks.Remove(ol);
                }
                await context.SaveChangesAsync();
                query = await context.DcOfficeKuafLinks.Where(okl => okl.Username == SamName).ToListAsync();
            }

            if (query.Any())
            {
                officeLink = query.First();
                officeLink.OfficeId = officeId;
                officeLink.FspId = fspId;
            }
            else
            {
                officeLink = new DcOfficeKuafLink() { OfficeId = officeId, FspId = fspId, Username = SamName, Supervisor = supervisor };
                context.DcOfficeKuafLinks.Add(officeLink);
            }
            try
            {
                await context.SaveChangesAsync();
                StaticDataService.DcOfficeKuafLinks = await context.DcOfficeKuafLinks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        public string GetRegion(string regionId)
        {
            if (regionId == null) return "Unknown";
            if (StaticDataService.Regions == null)
            {
                StaticDataService.Regions = context.DcRegions.AsNoTracking().ToList();
            }
            return StaticDataService.Regions.Where(r => r.RegionId == regionId).First().RegionName;
        }

        public string GetRegionCode(string regionId)
        {
            if (StaticDataService.Regions == null)
            {
                StaticDataService.Regions = context.DcRegions.AsNoTracking().ToList();
            }
            return StaticDataService.Regions.Where(r => r.RegionId == regionId).First().RegionCode;
        }
        public Dictionary<string, string> GetRegions()
        {
            if (StaticDataService.Regions == null)
            {
                StaticDataService.Regions = context.DcRegions.AsNoTracking().ToList();
            }
            return StaticDataService.Regions.ToDictionary(key => key.RegionId, value => value.RegionName); ;
        }
        public List<DcLocalOffice> GetOffices(string regionId)
        {
            if (StaticDataService.LocalOffices == null)
            {
                StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticDataService.LocalOffices.Where(o => o.RegionId == regionId).ToList();
        }
        public async Task ChangeOfficeStatus(string officeId, string status)
        {
            DcLocalOffice lo = await context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
            lo.ActiveStatus = status;
            await context.SaveChangesAsync();
            StaticDataService.LocalOffices = null;
            GetOffices(lo.RegionId);
        }
        public async Task ChangeOfficeName(string officeId, string name)
        {
            DcLocalOffice lo = await context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
            lo.OfficeName = name;
            await context.SaveChangesAsync();
            StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
        }
        public async Task MoveOffice(string fromOfficeId, int toOfficeId)
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

            await DeleteLocalOffice(fromOfficeId);
        }
        public async Task SaveManualBatch(string officeId, string manualBatch)
        {
            DcLocalOffice lo = await context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
            lo.ManualBatch = manualBatch;
            await context.SaveChangesAsync();
            StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
        }
        public async Task DeleteLocalOffice(string officeId)
        {
            var lo = await context.DcLocalOffices.FirstAsync(o => o.OfficeId == officeId);
            if (lo == null) return;
            context.DcLocalOffices.Remove(lo);
            await context.SaveChangesAsync();
            StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
        }
        public async Task UpdateServicePoint(DcFixedServicePoint s)
        {
            DcFixedServicePoint sp = await context.DcFixedServicePoints.Where(o => o.Id == s.Id).FirstAsync();
            sp.ServicePointName = s.ServicePointName;
            sp.OfficeId = s.OfficeId;
            await context.SaveChangesAsync();
            StaticDataService.ServicePoints = context.DcFixedServicePoints.AsNoTracking().ToList();
        }
        public async Task CreateOffice(RegionOffice office)
        {
            DcLocalOffice lo = new DcLocalOffice();
            lo.OfficeName = office.OfficeName;
            lo.OfficeId = (int.Parse(context.DcLocalOffices.Max(o => o.OfficeId)!) + 1).ToString();
            lo.RegionId = office.RegionId;
            lo.ActiveStatus = "A";
            lo.OfficeType = "LO";
            context.DcLocalOffices.Add(lo);
            await context.SaveChangesAsync();
            StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
        }
        public async Task CreateServicePoint(DcFixedServicePoint s)
        {
            context.DcFixedServicePoints.Add(s);
            await context.SaveChangesAsync();

            StaticDataService.ServicePoints = context.DcFixedServicePoints.AsNoTracking().ToList();
        }
        public List<string> GetOfficeIds(string regionId)
        {
            List<DcLocalOffice> offices = GetOffices(regionId);
            return (from office in offices select office.OfficeId).ToList();
        }
        public string GetOfficeName(string officeId)
        {
            if (StaticDataService.LocalOffices == null)
            {
                StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticDataService.LocalOffices.Where(o => o.OfficeId == officeId).First().OfficeName;
        }

        public string GetFspName(decimal? fspId)
        {
            if (StaticDataService.ServicePoints == null)
            {
                StaticDataService.ServicePoints = context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            if (fspId == null) return "";
            if (StaticDataService.ServicePoints.Where(o => o.Id == fspId).Any())
            {
                return StaticDataService.ServicePoints.Where(o => o.Id == fspId).First().ServicePointName;
            }
            return "";
        }
        public string GetOfficeType(string officeId)
        {
            if (StaticDataService.LocalOffices == null)
            {
                StaticDataService.LocalOffices = context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticDataService.LocalOffices.Where(o => o.OfficeId == officeId).First().OfficeType;
        }
        public string GetGrantType(string grantId)
        {
            if (StaticDataService.GrantTypes == null)
            {
                StaticDataService.GrantTypes = context.DcGrantTypes.AsNoTracking().ToDictionary(key => key.TypeId, value => value.TypeName);
            }
            return StaticDataService.GrantTypes[grantId];
        }
        public string GetGrantId(string grantType)
        {
            if (StaticDataService.GrantTypes == null)
            {
                StaticDataService.GrantTypes = context.DcGrantTypes.AsNoTracking().ToDictionary(key => key.TypeId, value => value.TypeName);
            }
            return StaticDataService.GrantTypes.Where(g => g.Value == grantType).First().Key;
        }
        public Dictionary<string, string> GetGrantTypes()
        {
            if (StaticDataService.GrantTypes == null)
            {
                StaticDataService.GrantTypes = context.DcGrantTypes.AsNoTracking().ToDictionary(key => key.TypeId, value => value.TypeName);
            }
            return StaticDataService.GrantTypes;
        }
        public string GetLcType(decimal lcId)
        {
            if (StaticDataService.LcTypes == null)
            {
                StaticDataService.LcTypes = context.DcLcTypes.AsNoTracking().ToDictionary(key => key.Pk, value => value.Description);
            }
            return StaticDataService.LcTypes[lcId];
        }
        public Dictionary<decimal, string> GetLcTypes()
        {
            if (StaticDataService.LcTypes == null)
            {
                StaticDataService.LcTypes = context.DcLcTypes.ToDictionary(key => key.Pk, value => value.Description);
            }
            return StaticDataService.LcTypes;
        }
        public List<RequiredDocsView> GetGrantDocuments(string grantType)
        {
            if (StaticDataService.RequiredDocs == null)
            {
                StaticDataService.RequiredDocs = (from reqDocGrant in context.DcGrantDocLinks
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
            }
            return StaticDataService.RequiredDocs.Where(r => r.GrantType == grantType).OrderBy(g => g.DOC_SECTION).ThenBy(g => g.DOC_ID).ToList();
        }
        /// <summary>
        /// Transport Y or N
        /// </summary>
        /// <param name="transport"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetBoxTypes(string transport)
        {

            if (StaticDataService.BoxTypes == null)
            {
                StaticDataService.BoxTypes = context.DcBoxTypes.AsNoTracking().ToList();
            }
            var result = StaticDataService.BoxTypes.Where(d => d.IsTransport == transport).ToDictionary(i => i.BoxTypeId.ToString(), i => i.BoxType);
            return result;
        }
        public Dictionary<string, string> GetBoxTypes()
        {

            if (StaticDataService.BoxTypes == null)
            {
                StaticDataService.BoxTypes = context.DcBoxTypes.AsNoTracking().ToList();
            }
            var result = StaticDataService.BoxTypes.ToDictionary(i => i.BoxTypeId.ToString(), i => i.BoxType);
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
            if (StaticDataService.RequestCategories == null)
            {
                StaticDataService.RequestCategories = context.DcReqCategories.OrderBy(e => e.CategoryDescr).AsNoTracking().ToList();
            }
            var result = StaticDataService.RequestCategories.ToDictionary(i => i.CategoryId.ToString(), i => i.CategoryDescr);
            //result.Add("", "select...");
            return result;
        }

        public Dictionary<string, string> GetRequestCategoryTypes()
        {
            if (StaticDataService.RequestCategoryTypes == null)
            {
                StaticDataService.RequestCategoryTypes = context.DcReqCategoryTypes.AsNoTracking().ToList();
            }
            var result = StaticDataService.RequestCategoryTypes.ToDictionary(i => i.TypeId.ToString(), i => i.TypeDescr);
            //result.Add("", "select...");
            return result;
        }

        public Dictionary<string, string> GetRequestCategoryTypes(string CategoryId)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(CategoryId)) return result;
            decimal.TryParse(CategoryId, out decimal catid);
            if (StaticDataService.RequestCategoryTypes == null)
            {
                StaticDataService.RequestCategoryTypes = context.DcReqCategoryTypes.AsNoTracking().ToList();
            }
            if (StaticDataService.RequestCategoryTypeLinks == null)
            {
                StaticDataService.RequestCategoryTypeLinks = context.DcReqCategoryTypeLinks.AsNoTracking().ToList();
            }
            result = (from r in StaticDataService.RequestCategoryTypes
                      join c in StaticDataService.RequestCategoryTypeLinks
                             on r.TypeId equals c.TypeId
                      where c.CategoryId == catid
                      select r).ToDictionary(i => i.TypeId.ToString(), i => i.TypeDescr);
            //result.Add("", "select...");
            return result;
        }
        public Dictionary<string, string> GetStakeHolders()
        {
            if (StaticDataService.StakeHolders == null)
            {
                StaticDataService.StakeHolders = context.DcStakeholders.Distinct().AsNoTracking().ToList();
            }
            var result = StaticDataService.StakeHolders.Distinct().ToDictionary(i => i.StakeholderId.ToString(), i => i.Name + " " + i.Surname);
            result.Add("", "");
            return result;
        }
        public Dictionary<string, string> GetStakeHolders(string DepartmentId)
        {
            decimal did;
            decimal.TryParse(DepartmentId, out did);
            if (StaticDataService.StakeHolders == null)
            {
                StaticDataService.StakeHolders = context.DcStakeholders.Distinct().AsNoTracking().ToList();
            }
            var result = StaticDataService.StakeHolders.Where(s => s.DepartmentId == did).ToDictionary(i => i.StakeholderId.ToString(), i => i.Name + " " + i.Surname);
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