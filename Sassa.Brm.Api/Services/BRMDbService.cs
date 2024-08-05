//using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.EntityFrameworkCore;
using Sassa.Brm.Common.Models;
using Sassa.BRM.Helpers;
using Sassa.BRM.Models;
//using Sassa.BRM.Pages.Components;
using System.Data;

namespace Sassa.BRM.Services
{
    public class BRMDbService
    {

        ModelContext _context;
        ActivityService _activity;
        UserSession _session;
        //MailMessages _mail;

        public UserSession? session
        {
            get
            {
                return _session;
            }
        }
        public BRMDbService(ModelContext context, IConfiguration config, ActivityService activity)
        {
            //if (StaticD.Users == null) StaticD.Users = new List<string>();
            _context = context;
            _session = new UserSession();
            _activity = activity;
        }

        public void SetUserSession(string user)
        {
            //S-1-5-21-1204054820-1125754781-535949388-513
            _session = new UserSession();

            _session.SamName = user;
            //_session.Roles = identity.GetRoles();
            //_session.Name = (string)user.Properties["name"].Value;
            //_session.Surname = (string)user.Properties["sn"].Value;

        }

        public event EventHandler? SessionInitialized;

        #region Static Data

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
        public async Task GetLocalOffice()
        {
            if (_session == null)
            {
                throw new Exception("Session not initialized");
            }
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
                  where lou.Username == _session.SamName
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
                await UpdateUserLocalOffice(ioffice.OfficeId, null);
                //try again..
                await GetLocalOffice();
            }

            var value = (from lo in StaticD.LocalOffices
                         join lou in StaticD.DcOfficeKuafLinks
                         on lo.OfficeId equals lou.OfficeId
                         where lou.Username == _session.SamName
                         select new
                         {
                             OfficeName = lo.OfficeName,
                             OfficeId = lo.OfficeId,
                             OfficeType = lo.OfficeType,
                             RegionId = lo.RegionId,
                             FspId = lou.FspId

                         }).FirstOrDefault();
            if (value != null)
            {
                _session.Office.OfficeName = value.OfficeName;
                _session.Office.OfficeId = value.OfficeId;
                _session.Office.OfficeType = value.OfficeType;
                _session.Office.RegionId = value.RegionId;
                _session.Office.FspId = value.FspId;
                _session.Office.RegionName = GetRegion(value.RegionId);
                _session.Office.RegionCode = GetRegionCode(value.RegionId);
                _session.Office.OfficeType = !string.IsNullOrEmpty(value.OfficeType) ? value.OfficeType : "LO"; //Default to local office
            }
            if (SessionInitialized != null) SessionInitialized(this, EventArgs.Empty);
        }

        public async Task GetLocalOffices()
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = await _context.DcLocalOffices.AsNoTracking().ToListAsync();
            }
            if (StaticD.DcOfficeKuafLinks == null)
            {
                StaticD.DcOfficeKuafLinks = await _context.DcOfficeKuafLinks.AsNoTracking().ToListAsync();
            }

        }

        public DcLocalOffice? GetLocalOffice(string officeId)
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            return StaticD.LocalOffices.Where(lo => lo.OfficeId == officeId).FirstOrDefault();
        }

        public void SetUserOffice(string officeId)
        {
            if (StaticD.LocalOffices == null)
            {
                StaticD.LocalOffices = _context.DcLocalOffices.AsNoTracking().ToList();
            }
            var office = StaticD.LocalOffices.Where(lo => lo.OfficeId == officeId).FirstOrDefault();
            if (office != null)
            {
                _session.Office.OfficeName = office.OfficeName;
                _session.Office.OfficeId = office.OfficeId;
                _session.Office.OfficeType = office.OfficeType;
                _session.Office.RegionId = office.RegionId;
                //_session.Office.FspId = office.FspId;
                _session.Office.RegionName = GetRegion(office.RegionId);
                _session.Office.RegionCode = GetRegionCode(office.RegionId);
                _session.Office.OfficeType = !string.IsNullOrEmpty(office.OfficeType) ? office.OfficeType : "LO"; //Default to local office
            }
        }
        public async Task<bool> UpdateUserLocalOffice(string officeId, decimal? fspId)
        {
            DcOfficeKuafLink officeLink;
            var query = await _context.DcOfficeKuafLinks.Where(okl => okl.Username == _session.SamName).ToListAsync();
            if (query.Count() > 1)
            {
                foreach (var ol in query)
                {
                    _context.DcOfficeKuafLinks.Remove(ol);
                }
                await _context.SaveChangesAsync();
                query = await _context.DcOfficeKuafLinks.Where(okl => okl.Username == _session.SamName).ToListAsync();
            }

            if (query.Any())
            {
                officeLink = query.First();
                officeLink.OfficeId = officeId;
                officeLink.FspId = fspId;
            }
            else
            {
                string supervisor = _session.IsInRole("GRP_BRM_Supervisors") ? "Y" : "N";
                officeLink = new DcOfficeKuafLink() { OfficeId = officeId, FspId = fspId, Username = _session.SamName, Supervisor = supervisor };
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
            lo.OfficeId = (int.Parse(_context.DcLocalOffices.Max(o => o.OfficeId) ?? "") + 1).ToString();
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

        #region BRM Records

        public bool checkBRMExists(string brmno)
        {
            return _context.DcFiles.Where(f => f.BrmBarcode == brmno).Any();
            //return _context.DcFiles.Where(k => k.BrmBarcode.ToLower() == brmno.ToLower()).Any();
        }


        public async Task EditBarCode(Application brm, string barCode)
        {

            DcFile file = await _context.DcFiles.Where(d => d.BrmBarcode == brm.Brm_BarCode).FirstAsync();
            file.BrmBarcode = barCode;
            await _context.SaveChangesAsync();
            CreateActivity("Update" + GetFileArea(file.SrdNo, file.Lctype), "Update BRM Barcode", file.UnqFileNo);

        }

        public async Task<DcFile> CreateBRM(Application application, string reason)
        {

            application.ChildId = application.ChildId.Trim();
            if (application.ChildId.Length > 13)
            {
                throw new Exception("Child ID too long.");
            }
            decimal? batch = 0;
            var office = _context.DcLocalOffices.Where(o => o.OfficeId == application.OfficeId).First();
            if (office.ManualBatch == "A")
            {
                batch = 0;
            }
            else
            {
                throw new Exception("Manual batch not set for this office.");
            }
            string region;

            if (StaticD.LocalOffices != null && StaticD.LocalOffices.Any())
            {
                region = StaticD.LocalOffices.Where(o => o.OfficeId == application.OfficeId).FirstOrDefault()!.RegionId;
            }
            else
            {
                throw new Exception("Office not found");
            }
            //Removes all duplicates in case LO retries after DC_Activity failure
            //await RemoveBRM(application.Brm_BarCode, "Api retry");
            if (_context.DcFiles.Where(f => f.BrmBarcode == application.Brm_BarCode).ToList().Any())
            {
                throw new Exception("Duplicate BRM");
            }

            DcFile file = new DcFile()
            {
                UnqFileNo = "",
                ApplicantNo = application.Id,
                BrmBarcode = application.Brm_BarCode,
                BatchAddDate = DateTime.Now,
                TransType = application.TRANS_TYPE,
                BatchNo = batch,
                GrantType = application.GrantType,
                OfficeId = application.OfficeId,
                RegionId = region,
                FspId = null,
                DocsPresent = application.DocsPresent,
                UpdatedDate = DateTime.Now,
                UserFirstname = application.Name,
                UserLastname = application.SurName,
                ApplicationStatus = application.AppStatus,
                TransDate = application.AppDate.ToDate("dd/MMM/yy"),
                SrdNo = application.Srd_No,
                ChildIdNo = application.ChildId.Trim(),
                Isreview = application.TRANS_TYPE == 2 ? "Y" : "N",
                Lastreviewdate = application.LastReviewDate.ToDate("dd/MMM/yy"),
                ArchiveYear = application.AppStatus.Contains("ARCHIVE") ? application.ARCHIVE_YEAR : null,
                Lctype = string.IsNullOrEmpty(application.LcType.Trim('0')) ? null : (Decimal?)Decimal.Parse(application.LcType),
                TdwBoxno = application.TDW_BOXNO,
                MiniBoxno = application.MiniBox,
                FileComment = reason,
                UpdatedByAd = application.BrmUserName,
                TdwBatch = 0
            };
            _context.ChangeTracker.Clear();
            _context.DcFiles.Add(file);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                CreateActivity("Capture" + GetFileArea(file.SrdNo, file.Lctype), "Error:" + ex.Message.Substring(0, 200), file.UpdatedByAd, file.UnqFileNo);
                throw;
            }


            file = _context.DcFiles.Where(k => k.BrmBarcode == application.Brm_BarCode).FirstOrDefault()!;
            //await SetBatchCount((decimal)file.BatchNo);
            CreateActivity("Capture" + GetFileArea(file.SrdNo, file.Lctype), "Print Coversheet", file.UpdatedByAd, file.UnqFileNo);
            DcSocpen dc_socpen;
            long? srd = null;
            try
            {
                if (application.Srd_No.IsNumeric())
                {
                    srd = long.Parse(application.Srd_No);
                }
            }
            catch
            {
                srd = null;
            }
            //Remove existing Barcode for this id/grant from dc_socpen
            _context.DcSocpen.Where(s => s.BrmBarcode == application.Brm_BarCode).ToList().ForEach(s => s.BrmBarcode = null);
            await _context.SaveChangesAsync();

            var result = await _context.DcSocpen.Where(s => s.BeneficiaryId == application.Id && s.GrantType == application.GrantType && s.SrdNo == srd || ("C95".Contains(s.GrantType) && s.ChildId.Trim() == application.ChildId.Trim())).ToListAsync();
            if (result.ToList().Any())
            {
                dc_socpen = result.First();
                dc_socpen.CaptureReference = file.UnqFileNo;
                dc_socpen.BrmBarcode = file.BrmBarcode;
                dc_socpen.CaptureDate = DateTime.Now;
                dc_socpen.RegionId = _session.Office.RegionId;
                dc_socpen.LocalofficeId = _session.Office.OfficeId;
                dc_socpen.StatusCode = application.AppStatus.Contains("MAIN") ? "ACTIVE" : "INACTIVE";
                dc_socpen.ApplicationDate = application.AppDate.ToDate("dd/MMM/yy");
                dc_socpen.SocpenDate = application.AppDate.ToDate("dd/MMM/yy");
            }
            else
            {
                dc_socpen = new DcSocpen();
                dc_socpen.ApplicationDate = application.AppDate.ToDate("dd/MMM/yy");
                dc_socpen.SocpenDate = application.AppDate.ToDate("dd/MMM/yy");
                dc_socpen.StatusCode = application.AppStatus.Contains("MAIN") ? "ACTIVE" : "INACTIVE";
                dc_socpen.BeneficiaryId = application.Id;
                dc_socpen.SrdNo = srd;
                dc_socpen.GrantType = application.GrantType;
                dc_socpen.ChildId = application.ChildId.Trim();
                dc_socpen.Name = application.Name;
                dc_socpen.Surname = application.SurName;
                dc_socpen.CaptureReference = file.UnqFileNo;
                dc_socpen.BrmBarcode = file.BrmBarcode;
                dc_socpen.CaptureDate = DateTime.Now;
                dc_socpen.RegionId = _session.Office.RegionId;
                dc_socpen.LocalofficeId = _session.Office.OfficeId;
                dc_socpen.Documents = file.DocsPresent;


                _context.DcSocpen.Add(dc_socpen);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                CreateActivity("Capture" + GetFileArea(file.SrdNo, file.Lctype), "Error:" + ex.Message.Substring(0, 200), file.UpdatedByAd, file.UnqFileNo);
                //throw;
            }

            return file;
        }

        //public async Task<DcFile> GetBRMRecord(string barcode)
        //{
        //    return await _context.DcFiles.Where(f => f.BrmBarcode == barcode).FirstAsync();
        //}

        //public async Task RemoveBRM(string brmNo, string reason)
        //{

        //    var files = _context.DcFiles.Where(k => k.BrmBarcode == brmNo);
        //    if (files.Any())
        //    {
        //        foreach (var dcfile in files)
        //        {
        //            dcfile.FileComment = reason;
        //            await BackupDcFileEntry(dcfile);
        //            CreateActivity("Delete" + GetFileArea(dcfile.SrdNo, dcfile.Lctype), "Delete BRM Record", dcfile.UnqFileNo);
        //            _context.DcFiles.Remove(dcfile);
        //        }
        //    }
        //    var merges = _context.DcMerges.Where(m => m.BrmBarcode == brmNo || m.ParentBrmBarcode == brmNo);
        //    foreach (var merge in merges)
        //    {
        //        _context.DcMerges.Remove(merge);
        //    }
        //    //if (_context.DcBrmGrants.Where(d => d.BrmBarcode == brmNo).Any())
        //    //{
        //    //    _context.DcBrmGrants.Remove(_context.DcBrmGrants.Where(d => d.BrmBarcode == brmNo).First());
        //    //}
        //    if (files.Any() || merges.Any())
        //    {
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch
        //        {

        //        }
        //    }

        //}
        /// <summary>
        /// Backup DcFile entry for removal
        /// </summary>
        /// <param name="file">Original File</param>
        //public async Task BackupDcFileEntry(DcFile file)
        //{
        //    DcFileDeleted removed = new DcFileDeleted();
        //    file.UpdatedByAd = _session.SamName;
        //    file.UpdatedDate = System.DateTime.Now;
        //    removed.FromDCFile(file);
        //    try
        //    {
        //        _context.DcFileDeleteds.Add(removed);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch //(Exception ex)
        //    {
        //        //throw new Exception("Error backing up file: " + ex.Message);
        //    }
        //}
        //public async Task AutoMerge(Application app, List<Application> parents)
        //{
        //    //Is not merged 
        //    if (string.IsNullOrEmpty(app.Brm_Parent))
        //    {
        //        //Is there a valid parent?
        //        if (parents.Where(p => p.TDW_BOXNO == null).Any())
        //        {
        //            var parent = parents.Where(p => p.TDW_BOXNO == null).First();

        //            DcMerge merge = _context.DcMerges.Where(k => k.BrmBarcode == app.Brm_BarCode).FirstOrDefault()!;

        //            if (merge == null)//No existing merge create one
        //            {
        //                //this record will be the parent
        //                DcMerge newMerge = new DcMerge();
        //                newMerge.BrmBarcode = app.Brm_BarCode;
        //                newMerge.ParentBrmBarcode = parent.Brm_BarCode;
        //                app.Brm_Parent = parent.Brm_BarCode;
        //                _context.DcMerges.Add(newMerge);
        //            }
        //            else//Existing merge modify it
        //            {
        //                merge.ParentBrmBarcode = parent.Brm_BarCode;
        //                app.Brm_Parent = parent.Brm_BarCode;
        //            }


        //        }
        //        else
        //        {
        //            //this record will be a parent
        //            DcMerge newMerge = new DcMerge();
        //            newMerge.BrmBarcode = app.Brm_BarCode;
        //            newMerge.ParentBrmBarcode = app.Brm_BarCode;
        //            app.Brm_Parent = app.Brm_BarCode;
        //            _context.DcMerges.Add(newMerge);

        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //}


        //private async Task<decimal?> CreateBatchForUser(string sRegType, string OfficeId, string SamName)
        //{
        //    DcBatch batch;
        //    List<DcBatch> batches = new List<DcBatch>();
        //    //Get open batch for User
        //    if (_session.IsRmc())
        //    {
        //        batches = await _context.DcBatches.Where(b => b.OfficeId == OfficeId && b.BatchStatus == "RMCBatch" && b.UpdatedByAd == SamName && b.RegType == sRegType).ToListAsync();
        //    }
        //    else
        //    {
        //        batches = await _context.DcBatches.Where(b => b.OfficeId == OfficeId && b.BatchStatus == "Open" && b.UpdatedByAd == SamName && b.RegType == sRegType).ToListAsync();
        //    }

        //    if (batches.Any())
        //    {
        //        batch = batches.First();
        //        if (batch.NoOfFiles > 34 && !sRegType.StartsWith("LC"))
        //        {
        //            throw new Exception($"Batch is full. Please verify and close batch before adding more to batch for {sRegType}");
        //        }
        //    }
        //    else
        //    {
        //        batch = new DcBatch
        //        {
        //            BatchStatus = _session.IsRmc() ? "RMCBatch" : "Open",
        //            BatchCurrent = "Y",
        //            OfficeId = OfficeId,
        //            RegType = sRegType,
        //            UpdatedDate = DateTime.Now,
        //            UpdatedBy = 0,
        //            UpdatedByAd = SamName
        //        };
        //        _context.DcBatches.Add(batch);
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch// (Exception ex)
        //        {
        //            throw;
        //        }

        //    }
        //    return batch.BatchNo;
        //}
        #endregion

        /// <summary>
        /// Create Activity Record for the Api 
        /// User is not logged in so the service user is used
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Activity"></param>
        /// <returns></returns>
        public void CreateActivity(string Area, string Activity, string userName, string UniqueFileNo = "")
        {
            DcActivity activity = new DcActivity { ActivityDate = DateTime.Now, RegionId = _session.Office.RegionId, OfficeId = decimal.Parse(_session.Office.OfficeId), Userid = 0, Username = userName, Area = Area, Activity = Activity, Result = "OK", UnqFileNo = UniqueFileNo };
            try
            {

                _activity.PostActivity(activity);

            }
            catch
            {
                //just ignoring activity post errors for now.
            }
        }
        public string GetFileArea(string srdNo, decimal? lcType)
        {
            if (!string.IsNullOrEmpty(srdNo))
            {
                return "-SRD";
            }
            if (lcType != null)
            {
                return "-LC";
            }
            return "-File";
        }

    }
}

