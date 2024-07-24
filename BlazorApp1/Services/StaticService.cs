using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Models;
using Sassa.BRM.ViewModels;

namespace Sassa.BRM.Services
{
    public class StaticService
    {

        ModelContext _context;
        public StaticService(ModelContext context)
        {
            _context = context;
        }

        #region Static Data access

        public string GetTransactionType(int key)
        {
            if (StaticD.TransactionTypes == null)
            {
                StaticD.TransactionTypes = new Dictionary<int, string>();
                StaticD.TransactionTypes.Add(0, "Application");
                StaticD.TransactionTypes.Add(1, "Loose Correspondence");
                StaticD.TransactionTypes.Add(2, "Review");
            }
            return StaticD.TransactionTypes[key];
        }
        public async Task<UserOffice> GetUserLocalOffice(string samName, string supervisor)
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            if (StaticD.DcOfficeKuafLinks == null)
            {
                StaticD.DcOfficeKuafLinks = _context.DcOfficeKuafLinks.AsNoTracking().ToList();
            }

            //Try local office from static.
            if (!(from lo in StaticD.LocalOffices
                  join lou in StaticD.DcOfficeKuafLinks
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
                DcLocalOffice ioffice = GetOffices("7").FirstOrDefault();
                //Attach to first or default office in Gauteng.
                await UpdateUserLocalOffice(ioffice.OfficeId, null, samName, supervisor);
                //try again..
                await GetUserLocalOffice(samName, supervisor);
            }

            var value = (from lo in StaticD.LocalOffices
                         join lou in StaticD.DcOfficeKuafLinks
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
            Office.OfficeName = value.OfficeName;
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
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticD.LocalOffices.Where(lo => lo.OfficeId == officeId).FirstOrDefault();
        }

        public UserSession SetUserOffice(string officeId)
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            var office = StaticD.LocalOffices.Where(lo => lo.OfficeId == officeId).FirstOrDefault();
            UserSession _session = new UserSession();
            _session.Office.OfficeName = office.OfficeName;
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
            if (StaticD.ServicePoints == null)
            {
                StaticD.ServicePoints = _context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            return StaticD.ServicePoints.Where(sp => StaticD.LocalOffices.Where(lo => lo.RegionId == regionID).Select(l => l.OfficeId).ToList().Contains(sp.OfficeId.ToString())).ToList();
        }
        public List<DcFixedServicePoint> GetOfficeServicePoints(string officeID)
        {
            if (StaticD.ServicePoints == null)
            {
                StaticD.ServicePoints = _context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            return StaticD.ServicePoints.Where(sp => sp.OfficeId == officeID).ToList();
        }
        public string GetServicePointName(decimal? fspID)
        {
            if (StaticD.ServicePoints == null)
            {
                StaticD.ServicePoints = _context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            var result = StaticD.ServicePoints.Where(sp => sp.Id == fspID);
            if (result.Any())
            {
                return result.First().ServicePointName;
            }
            return "";
        }
        public async Task<bool> UpdateUserLocalOffice(string officeId, decimal? fspId, string SamName, string supervisor)
        {
            DcOfficeKuafLink officeLink;
            var query = await _context.DcOfficeKuafLinks.Where(okl => okl.Username == SamName).ToListAsync();
            if (query.Count() > 1)
            {
                foreach (var ol in query)
                {
                    _context.DcOfficeKuafLinks.Remove(ol);
                }
                await _context.SaveChangesAsync();
                query = await _context.DcOfficeKuafLinks.Where(okl => okl.Username == SamName).ToListAsync();
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
                _context.DcOfficeKuafLinks.Add(officeLink);
            }
            try
            {
                await _context.SaveChangesAsync();
                StaticD.DcOfficeKuafLinks = await _context.DcOfficeKuafLinks.ToListAsync();
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
            if (StaticD.Regions == null)
            {
                StaticD.Regions = _context.DcRegions.AsNoTracking().ToList();
            }
            return StaticD.Regions.Where(r => r.RegionId == regionId).First().RegionName;
        }

        public string GetRegionCode(string regionId)
        {
            if (StaticD.Regions == null)
            {
                StaticD.Regions = _context.DcRegions.AsNoTracking().ToList();
            }
            return StaticD.Regions.Where(r => r.RegionId == regionId).First().RegionCode;
        }
        public Dictionary<string, string> GetRegions()
        {
            if (StaticD.Regions == null)
            {
                StaticD.Regions = _context.DcRegions.AsNoTracking().ToList();
            }
            return StaticD.Regions.ToDictionary(key => key.RegionId, value => value.RegionName); ;
        }
        public List<DcLocalOffice> GetOffices(string regionId)
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticD.LocalOffices.Where(o => o.RegionId == regionId).ToList();
        }

        public async Task ChangeOfficeStatus(string officeId, string status)
        {
            DcLocalOffice lo = await _context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
            lo.ActiveStatus = status;
            await _context.SaveChangesAsync();
            StaticD.LocalOffices = null;
            GetOffices(lo.RegionId);
        }
        public async Task ChangeOfficeName(string officeId, string name)
        {
            DcLocalOffice lo = await _context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
            lo.OfficeName = name;
            await _context.SaveChangesAsync();
            StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
        }
        public async Task MoveOffice(string fromOfficeId, int toOfficeId)
        {
            //DC_FIle
            var oldOfficerecs = await _context.DcFiles.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
            foreach (var file in oldOfficerecs)
            {
                file.OfficeId = toOfficeId.ToString();
            }
            await _context.SaveChangesAsync();
            //DC_FIXED_SERVICE_POINT
            var oldFsprecs = await _context.DcFixedServicePoints.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
            foreach (var fsp in oldFsprecs)
            {
                fsp.OfficeId = toOfficeId.ToString();
            }
            await _context.SaveChangesAsync();
            //DC_OFFICE_KUAF_LINK
            var oldKuafrecs = await _context.DcOfficeKuafLinks.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
            foreach (var kuaf in oldKuafrecs)
            {
                kuaf.OfficeId = toOfficeId.ToString();
            }
            await _context.SaveChangesAsync();

            //DC_Batches
            var oldBatchRecs = await _context.DcBatches.Where(o => o.OfficeId == fromOfficeId).ToListAsync();
            foreach (var batch in oldBatchRecs)
            {
                batch.OfficeId = toOfficeId.ToString();
            }
            await _context.SaveChangesAsync();

            await DeleteLocalOffice(fromOfficeId);
        }

        public async Task SaveManualBatch(string officeId, string manualBatch)
        {
            DcLocalOffice lo = await _context.DcLocalOffices.Where(o => o.OfficeId == officeId).FirstAsync();
            lo.ManualBatch = manualBatch;
            await _context.SaveChangesAsync();
            StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
        }
        public async Task DeleteLocalOffice(string officeId)
        {
            var lo = await _context.DcLocalOffices.FirstAsync(o => o.OfficeId == officeId);
            if (lo == null) return;
            _context.DcLocalOffices.Remove(lo);
            await _context.SaveChangesAsync();
            StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
        }
        public async Task UpdateServicePoint(DcFixedServicePoint s)
        {
            DcFixedServicePoint sp = await _context.DcFixedServicePoints.Where(o => o.Id == s.Id).FirstAsync();
            sp.ServicePointName = s.ServicePointName;
            sp.OfficeId = s.OfficeId;
            await _context.SaveChangesAsync();
            StaticD.ServicePoints = _context.DcFixedServicePoints.AsNoTracking().ToList();
        }
        public async Task CreateOffice(RegionOffice office)
        {
            DcLocalOffice lo = new DcLocalOffice();
            lo.OfficeName = office.OfficeName;
            lo.OfficeId = (int.Parse(_context.DcLocalOffices.Max(o => o.OfficeId)) + 1).ToString();
            lo.RegionId = office.RegionId;
            lo.ActiveStatus = "A";
            lo.OfficeType = "LO";
            _context.DcLocalOffices.Add(lo);
            await _context.SaveChangesAsync();
            StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
        }

        public async Task CreateServicePoint(DcFixedServicePoint s)
        {
            _context.DcFixedServicePoints.Add(s);
            await _context.SaveChangesAsync();

            StaticD.ServicePoints = _context.DcFixedServicePoints.AsNoTracking().ToList();
        }

        public List<string> GetOfficeIds(string regionId)
        {
            List<DcLocalOffice> offices = GetOffices(regionId);
            return (from office in offices select office.OfficeId).ToList();
        }
        public string GetOfficeName(string officeId)
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticD.LocalOffices.Where(o => o.OfficeId == officeId).First().OfficeName;
        }

        public string GetFspName(decimal? fspId)
        {
            if (StaticD.ServicePoints == null)
            {
                StaticD.ServicePoints = _context.DcFixedServicePoints.AsNoTracking().ToList();
            }
            if (fspId == null) return "";
            if (StaticD.ServicePoints.Where(o => o.Id == fspId).Any())
            {
                return StaticD.ServicePoints.Where(o => o.Id == fspId).First().ServicePointName;
            }
            return "";
        }
        public string GetOfficeType(string officeId)
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticD.LocalOffices.Where(o => o.OfficeId == officeId).First().OfficeType;
        }
        public string GetGrantType(string grantId)
        {
            if (StaticD.GrantTypes == null)
            {
                StaticD.GrantTypes = _context.DcGrantTypes.AsNoTracking().ToDictionary(key => key.TypeId, value => value.TypeName);
            }
            return StaticD.GrantTypes[grantId];
        }
        public string GetGrantId(string grantType)
        {
            if (StaticD.GrantTypes == null)
            {
                StaticD.GrantTypes = _context.DcGrantTypes.AsNoTracking().ToDictionary(key => key.TypeId, value => value.TypeName);
            }
            return StaticD.GrantTypes.Where(g => g.Value == grantType).First().Key;
        }
        public Dictionary<string, string> GetGrantTypes()
        {
            if (StaticD.GrantTypes == null)
            {
                StaticD.GrantTypes = _context.DcGrantTypes.AsNoTracking().ToDictionary(key => key.TypeId, value => value.TypeName);
            }
            return StaticD.GrantTypes;
        }
        public string GetLcType(decimal lcId)
        {
            if (StaticD.LcTypes == null)
            {
                StaticD.LcTypes = _context.DcLcTypes.AsNoTracking().ToDictionary(key => key.Pk, value => value.Description);
            }
            return StaticD.LcTypes[lcId];
        }
        public Dictionary<decimal, string> GetLcTypes()
        {
            if (StaticD.LcTypes == null)
            {
                StaticD.LcTypes = _context.DcLcTypes.ToDictionary(key => key.Pk, value => value.Description);
            }
            return StaticD.LcTypes;
        }
        public List<RequiredDocsView> GetGrantDocuments(string grantType)
        {
            if (StaticD.RequiredDocs == null)
            {
                StaticD.RequiredDocs = (from reqDocGrant in _context.DcGrantDocLinks
                                        join reqDoc in _context.DcDocumentTypes on reqDocGrant.DocumentId equals reqDoc.TypeId
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
            return StaticD.RequiredDocs.Where(r => r.GrantType == grantType).OrderBy(g => g.DOC_SECTION).ThenBy(g => g.DOC_ID).ToList();
        }
        /// <summary>
        /// Transport Y or N
        /// </summary>
        /// <param name="transport"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetBoxTypes(string transport)
        {

            if (StaticD.BoxTypes == null)
            {
                StaticD.BoxTypes = _context.DcBoxTypes.AsNoTracking().ToList();
            }
            var result = StaticD.BoxTypes.Where(d => d.IsTransport == transport).ToDictionary(i => i.BoxTypeId.ToString(), i => i.BoxType);
            return result;
        }
        public Dictionary<string, string> GetBoxTypes()
        {

            if (StaticD.BoxTypes == null)
            {
                StaticD.BoxTypes = _context.DcBoxTypes.AsNoTracking().ToList();
            }
            var result = StaticD.BoxTypes.ToDictionary(i => i.BoxTypeId.ToString(), i => i.BoxType);
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
            if (StaticD.RequestCategories == null)
            {
                StaticD.RequestCategories = _context.DcReqCategories.OrderBy(e => e.CategoryDescr).AsNoTracking().ToList();
            }
            var result = StaticD.RequestCategories.ToDictionary(i => i.CategoryId.ToString(), i => i.CategoryDescr);
            //result.Add("", "select...");
            return result;
        }

        public Dictionary<string, string> GetRequestCategoryTypes()
        {
            if (StaticD.RequestCategoryTypes == null)
            {
                StaticD.RequestCategoryTypes = _context.DcReqCategoryTypes.AsNoTracking().ToList();
            }
            var result = StaticD.RequestCategoryTypes.ToDictionary(i => i.TypeId.ToString(), i => i.TypeDescr);
            //result.Add("", "select...");
            return result;
        }

        public Dictionary<string, string> GetRequestCategoryTypes(string CategoryId)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(CategoryId)) return result;
            decimal.TryParse(CategoryId, out decimal catid);
            if (StaticD.RequestCategoryTypes == null)
            {
                StaticD.RequestCategoryTypes = _context.DcReqCategoryTypes.AsNoTracking().ToList();
            }
            if (StaticD.RequestCategoryTypeLinks == null)
            {
                StaticD.RequestCategoryTypeLinks = _context.DcReqCategoryTypeLinks.AsNoTracking().ToList();
            }
            result = (from r in StaticD.RequestCategoryTypes
                      join c in StaticD.RequestCategoryTypeLinks
                             on r.TypeId equals c.TypeId
                      where c.CategoryId == catid
                      select r).ToDictionary(i => i.TypeId.ToString(), i => i.TypeDescr);
            //result.Add("", "select...");
            return result;
        }
        public Dictionary<string, string> GetStakeHolders()
        {
            if (StaticD.StakeHolders == null)
            {
                StaticD.StakeHolders = _context.DcStakeholders.Distinct().AsNoTracking().ToList();
            }
            var result = StaticD.StakeHolders.Distinct().ToDictionary(i => i.StakeholderId.ToString(), i => i.Name + " " + i.Surname);
            result.Add("", "");
            return result;
        }
        public Dictionary<string, string> GetStakeHolders(string DepartmentId)
        {
            decimal did;
            decimal.TryParse(DepartmentId, out did);
            if (StaticD.StakeHolders == null)
            {
                StaticD.StakeHolders = _context.DcStakeholders.Distinct().AsNoTracking().ToList();
            }
            var result = StaticD.StakeHolders.Where(s => s.DepartmentId == did).ToDictionary(i => i.StakeholderId.ToString(), i => i.Name + " " + i.Surname);
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
