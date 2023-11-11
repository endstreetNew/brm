//using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using razor.Components.Models;
using Sassa.BRM.Data.ViewModels;
using Sassa.BRM.Helpers;
using Sassa.BRM.Models;
//using Sassa.BRM.Pages.Components;
using Sassa.BRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Sassa.BRM.Services
{
    public class BRMDbService
    {

        ModelContext _context;
        //RawSqlService _raw;
        UserSession? _session;
        //MailMessages _mail;

        public UserSession? session
        {
            get
            {
                return _session;
            }
        }
        public BRMDbService(ModelContext context)
        {
            //if (StaticD.Users == null) StaticD.Users = new List<string>();
            _context = context;

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
            string username = session.SamName;
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
                  where lou.Username == username
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
                await UpdateUserLocalOffice(ioffice.OfficeId, null);
                //try again..
                await GetLocalOffice();
            }

            var value = (from lo in StaticD.LocalOffices
                         join lou in StaticD.DcOfficeKuafLinks
                         on lo.OfficeId equals lou.OfficeId
                         where lou.Username == username
                         select new
                         {
                             OfficeName = lo.OfficeName,
                             OfficeId = lo.OfficeId,
                             OfficeType = lo.OfficeType,
                             RegionId = lo.RegionId,
                             FspId = lou.FspId

                         }).FirstOrDefault();

            _session.Office.OfficeName = value.OfficeName;
            _session.Office.OfficeId = value.OfficeId;
            _session.Office.OfficeType = value.OfficeType;
            _session.Office.RegionId = value.RegionId;
            _session.Office.FspId = value.FspId;
            _session.Office.RegionName = GetRegion(value.RegionId);
            _session.Office.RegionCode = GetRegionCode(value.RegionId);
            _session.Office.OfficeType = !string.IsNullOrEmpty(value.OfficeType) ? value.OfficeType : "LO"; //Default to local office
            if (SessionInitialized != null) SessionInitialized(this, null);
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
            
            _session.Office.OfficeName = office.OfficeName;
            _session.Office.OfficeId = office.OfficeId;
            _session.Office.OfficeType = office.OfficeType;
            _session.Office.RegionId = office.RegionId;
            //_session.Office.FspId = office.FspId;
            _session.Office.RegionName = GetRegion(office.RegionId);
            _session.Office.RegionCode = GetRegionCode(office.RegionId);
            _session.Office.OfficeType = !string.IsNullOrEmpty(office.OfficeType) ? office.OfficeType : "LO"; //Default to local office
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
        public async Task<bool> UpdateUserLocalOffice(string officeId, decimal? fspId)
        {
            DcOfficeKuafLink officeLink;
            var query = await _context.DcOfficeKuafLinks.Where(okl => okl.Username == session.SamName).ToListAsync();
            if (query.Count() > 1)
            {
                foreach (var ol in query)
                {
                    _context.DcOfficeKuafLinks.Remove(ol);
                }
                await _context.SaveChangesAsync();
                query = await _context.DcOfficeKuafLinks.Where(okl => okl.Username == session.SamName).ToListAsync();
            }

            if (query.Any())
            {
                officeLink = query.First();
                officeLink.OfficeId = officeId;
                officeLink.FspId = fspId;
            }
            else
            {
                string supervisor = session.IsInRole("GRP_BRM_Supervisors") ? "Y" : "N";
                officeLink = new DcOfficeKuafLink() { OfficeId = officeId, FspId = fspId, Username = session.SamName, Supervisor = supervisor };
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
            foreach(var file in oldOfficerecs)
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
            //Removes all duplicates
            await RemoveBRM(application.Brm_BarCode, reason);
            decimal? batch = 0;

            //string batchType = application.Id.StartsWith("S") ? "SrdNoId" : application.AppStatus;
            //batch = string.IsNullOrEmpty(application.TDW_BOXNO) ? await CreateBatchForUser(batchType,application.OfficeId,application.BrmUserName) : 0;
            string region;

            if (StaticD.LocalOffices != null && StaticD.LocalOffices.Any())
            {
                region = StaticD.LocalOffices.Where(o => o.OfficeId == application.OfficeId).FirstOrDefault()!.RegionId;
            }
            else
            {
                throw new Exception("Office not found");
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
                ChildIdNo = application.ChildId,
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
                CreateActivity("Capture" + GetFileArea(file.SrdNo, file.Lctype), "Error:" + ex.Message.Substring(0,200), file.UnqFileNo);
                throw;
            }

            //if (_context.DcBrmGrants.Where(g => g.ApplicantNo == file.ApplicantNo && g.GrantType == file.GrantType).Any())
            //{

            //    DcBrmGrant progress = new DcBrmGrant { ApplicantNo = file.ApplicantNo, GrantType = file.GrantType, BrmBarcode = file.BrmBarcode, CaptureDate = DateTime.Now };
            //    _context.DcBrmGrants.Add(progress);
            //    await _context.SaveChangesAsync();
            //    string sql = $"UPDATE SASSA.SOCPEN_PERSONAL_GRANTS S SET BRM_Reference = '{file.BrmBarcode}', File_DATE = TO_DATE('{DateTime.Now.ToString("yyyy-MM-dd")}','YYYY-MM-DD') where S.PENSION_NO = '{file.ApplicantNo}' AND S.GRANT_TYPE = '{file.GrantType}' ";
            //    _raw.ExecuteNonQuery(sql);
            //}

            file = _context.DcFiles.Where(k => k.BrmBarcode == application.Brm_BarCode).FirstOrDefault()!;
            //await SetBatchCount((decimal)file.BatchNo);
            CreateActivity("Capture" + GetFileArea(file.SrdNo, file.Lctype), "Print Coversheet", file.UnqFileNo);
            DcSocpen dc_socpen;
            //if (application.SocpenIsn > 0)
            //{
            //    dc_socpen = await _context.DcSocpen.FindAsync(application.SocpenIsn);
            //    dc_socpen.CaptureReference = file.UnqFileNo;
            //    dc_socpen.BrmBarcode = file.BrmBarcode;
            //    dc_socpen.CaptureDate = DateTime.Now;
            //    dc_socpen.RegionId = session.Office.RegionId;
            //    dc_socpen.LocalofficeId = session.Office.OfficeId;

            //}
            //else
            //{
            long? srd;
            try
            {
                srd = long.Parse(application.Srd_No);
            }
            catch
            {
                srd = null;
            }
            //Remove existing Barcode for this id/grant
            _context.DcSocpen.Where(s => s.BrmBarcode == application.Brm_BarCode).ToList().ForEach(s => s.BrmBarcode = null);
            await _context.SaveChangesAsync();

            var result = await _context.DcSocpen.Where(s => s.BeneficiaryId == application.Id && s.GrantType == application.GrantType && s.SrdNo == srd && s.ChildId == application.ChildId).ToListAsync();
            if(result.Any())
            {
                dc_socpen = result.First();
                dc_socpen.CaptureReference = file.UnqFileNo;
                dc_socpen.BrmBarcode = file.BrmBarcode;
                dc_socpen.CaptureDate = DateTime.Now;
                dc_socpen.RegionId = session.Office.RegionId;
                dc_socpen.LocalofficeId = session.Office.OfficeId;
                dc_socpen.StatusCode = application.AppStatus.Contains("MAIN") ? "ACTIVE" : "INACTIVE";
                dc_socpen.ApplicationDate = application.AppDate.ToDate("dd/MMM/yy");
                dc_socpen.SocpenDate = application.AppDate.ToDate("dd/MMM/yy");
            }
            else
            {
                dc_socpen = new DcSocpen();
                dc_socpen.ApplicationDate = application.AppDate.ToDate("dd/MMM/yy");
                dc_socpen.SocpenDate = application.AppDate.ToDate("dd/MMM/yy");
                dc_socpen.StatusCode = application.AppStatus.Contains("MAIN") ? "ACTIVE":"INACTIVE";
                dc_socpen.BeneficiaryId = application.Id;
                dc_socpen.SrdNo = srd;
                dc_socpen.GrantType = application.GrantType;
                dc_socpen.ChildId = application.ChildId;
                dc_socpen.Name = application.Name;
                dc_socpen.Surname = application.SurName;
                dc_socpen.CaptureReference = file.UnqFileNo;
                dc_socpen.BrmBarcode = file.BrmBarcode;
                dc_socpen.CaptureDate = DateTime.Now;
                dc_socpen.RegionId = session.Office.RegionId;
                dc_socpen.LocalofficeId = session.Office.OfficeId;
                dc_socpen.Documents = file.DocsPresent;


                _context.DcSocpen.Add(dc_socpen);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                CreateActivity("Capture" + GetFileArea(file.SrdNo, file.Lctype), "Error:" + ex.Message.Substring(0, 200), file.UnqFileNo);
                throw;
            }

            return file;
        }

        public async Task<DcFile> GetBRMRecord(string barcode)
        {
            return await _context.DcFiles.Where(f => f.BrmBarcode == barcode).FirstAsync();
        }

        public async Task RemoveBRM(string brmNo, string reason)
        {

            var files = _context.DcFiles.Where(k => k.BrmBarcode == brmNo);
            if (files.Any())
            {
                foreach (var dcfile in files)
                {
                    dcfile.FileComment = reason;
                    await BackupDcFileEntry(dcfile);
                    CreateActivity("Delete" + GetFileArea(dcfile.SrdNo, dcfile.Lctype), "Delete BRM Record", dcfile.UnqFileNo);
                    _context.DcFiles.Remove(dcfile);
                }
            }
            var merges = _context.DcMerges.Where(m => m.BrmBarcode == brmNo || m.ParentBrmBarcode == brmNo);
            foreach (var merge in merges)
            {
                _context.DcMerges.Remove(merge);
            }
            //if (_context.DcBrmGrants.Where(d => d.BrmBarcode == brmNo).Any())
            //{
            //    _context.DcBrmGrants.Remove(_context.DcBrmGrants.Where(d => d.BrmBarcode == brmNo).First());
            //}
            if (files.Any() || merges.Any())
            {
                await _context.SaveChangesAsync();
            }

        }
        /// <summary>
        /// Backup DcFile entry for removal
        /// </summary>
        /// <param name="file">Original File</param>
        public async Task BackupDcFileEntry(DcFile file)
        {
            DcFileDeleted removed = new DcFileDeleted();
            file.UpdatedByAd = _session.SamName;
            file.UpdatedDate = System.DateTime.Now;
            removed.FromDCFile(file);
            try
            {
                _context.DcFileDeleteds.Add(removed);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //throw new Exception("Error backing up file: " + ex.Message);
            }
        }
        public async Task AutoMerge(Application app, List<Application> parents)
        {
            //Is not merged 
            if (string.IsNullOrEmpty(app.Brm_Parent))
            {
                //Is there a valid parent?
                if (parents.Where(p => p.TDW_BOXNO == null).Any())
                {
                    var parent = parents.Where(p => p.TDW_BOXNO == null).First();

                    DcMerge merge = _context.DcMerges.Where(k => k.BrmBarcode == app.Brm_BarCode).FirstOrDefault();

                    if (merge == null)//No existing merge create one
                    {
                        //this record will be the parent
                        DcMerge newMerge = new DcMerge();
                        newMerge.BrmBarcode = app.Brm_BarCode;
                        newMerge.ParentBrmBarcode = parent.Brm_BarCode;
                        app.Brm_Parent = parent.Brm_BarCode;
                        _context.DcMerges.Add(newMerge);
                    }
                    else//Existing merge modify it
                    {
                        merge.ParentBrmBarcode = parent.Brm_BarCode;
                        app.Brm_Parent = parent.Brm_BarCode;
                    }


                }
                else
                {
                    //this record will be a parent
                    DcMerge newMerge = new DcMerge();
                    newMerge.BrmBarcode = app.Brm_BarCode;
                    newMerge.ParentBrmBarcode = app.Brm_BarCode;
                    app.Brm_Parent = app.Brm_BarCode;
                    _context.DcMerges.Add(newMerge);

                }
                await _context.SaveChangesAsync();
            }
        }


        private async Task<decimal?> CreateBatchForUser(string sRegType,string OfficeId,string SamName)
        {
            DcBatch batch;
            List<DcBatch> batches = new List<DcBatch>();
            //Get open batch for User
            if (_session.IsRmc())
            {
                batches = await _context.DcBatches.Where(b => b.OfficeId == OfficeId && b.BatchStatus == "RMCBatch" && b.UpdatedByAd == SamName && b.RegType == sRegType).ToListAsync();
            }
            else
            {
                batches = await _context.DcBatches.Where(b => b.OfficeId == OfficeId && b.BatchStatus == "Open" && b.UpdatedByAd == SamName && b.RegType == sRegType).ToListAsync();
            }

            if (batches.Any())
            {
                batch = batches.First();
                if (batch.NoOfFiles > 34 && !sRegType.StartsWith("LC"))
                {
                    throw new Exception($"Batch is full. Please verify and close batch before adding more to batch for {sRegType}");
                }
            }
            else
            {
                batch = new DcBatch
                {
                    BatchStatus = _session.IsRmc() ? "RMCBatch" : "Open",
                    BatchCurrent = "Y",
                    OfficeId = OfficeId,
                    RegType = sRegType,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = 0,
                    UpdatedByAd = SamName
                };
                _context.DcBatches.Add(batch);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch// (Exception ex)
                {
                    throw;
                }

            }
            return batch.BatchNo;
        }
        #endregion

        //#region Boxing and Re-Boxing
        //public async Task<PagedResult<ReboxListItem>> GetAllFilesByBoxNo(string boxNo, int page)
        //{
        //    bool repaired = await RepairAltBoxSequence(boxNo);

        //    PagedResult<ReboxListItem> result = new PagedResult<ReboxListItem>();
        //    if (StaticD.GrantTypes == null)
        //    {
        //        _ = GetGrantTypes();
        //    }
        //    result.count = _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).Count();


        //    result.result = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).OrderByDescending(f => f.UpdatedDate).Skip((page - 1) * 20).Take(20).OrderBy(f => f.UnqFileNo).AsNoTracking()
        //                .Select(f => new ReboxListItem
        //                {
        //                    ClmNo = f.UnqFileNo,
        //                    BrmNo = f.BrmBarcode,
        //                    IdNo = f.ApplicantNo,
        //                    FullName = f.FullName,
        //                    GrantType = StaticD.GrantTypes[f.GrantType],
        //                    BoxNo = boxNo,
        //                    AltBoxNo = f.AltBoxNo,
        //                    Scanned = f.ScanDatetime != null,
        //                    MiniBox = (int?)f.MiniBoxno,
        //                    RegType = f.ApplicationStatus,
        //                    TdwBatch = (int)f.TdwBatch
        //                }).ToListAsync();
        //    return result;
        //}

        //public async Task<PagedResult<TdwBatchViewModel>> GetAllBoxes(int page)
        //{

        //    PagedResult<TdwBatchViewModel> result = new PagedResult<TdwBatchViewModel>();

        //    List<DcFile> allFiles = await _context.DcFiles.Where(bn => bn.TdwBatch == 1 && bn.RegionId == _session.Office.RegionId).Where(bn => bn.ApplicationStatus.Contains("LC")).AsNoTracking().ToListAsync();
        //    if (!allFiles.Any()) return result;

        //    //result.count = _context.DcFiles.Where(bn => bn.TdwBatch == 1 && bn.RegionId == _session.Office.RegionId).Where(bn => bn.ApplicationStatus.Contains("LC")).Count();
        //    List<DcFile> dcFiles = allFiles.OrderByDescending(f => f.UpdatedDate).Skip((page - 1) * 20).Take(20).OrderBy(f => f.UnqFileNo).ToList();

        //    if(!dcFiles.Any())
        //    {
        //        return new PagedResult<TdwBatchViewModel>();
        //    }
        //    var boxes = dcFiles.GroupBy(t => t.TdwBoxno)
        //           .Select(grp => grp.First())
        //           .ToList();

        //    result.count = boxes.Count();

        //        foreach(var box in boxes)
        //        {
        //            result.result.Add(
        //                new TdwBatchViewModel
        //                {
        //                    BoxNo = box.TdwBoxno,
        //                    Region = GetRegion(box.RegionId),
        //                    MiniBoxes = (int)dcFiles.Where(f => f.TdwBoxno == box.TdwBoxno).Max(f => f.MiniBoxno),
        //                    Files = allFiles.Where(f => f.TdwBoxno == box.TdwBoxno).Count(),
        //                    User = _session.SamName,
        //                    TdwSendDate = dcFiles.Where(f => f.TdwBoxno == box.TdwBoxno).Max(f => f.TdwBatchDate),
        //                    TdwBatchNo = (int)dcFiles.Where(f => f.TdwBoxno == box.TdwBoxno).Max(f => f.TdwBatch),

        //                });

        //        }

        //    return result;
        //}

        //public async Task<PagedResult<TdwBatchViewModel>> GetHistoryBoxes(int page)
        //{

        //    PagedResult<TdwBatchViewModel> result = new PagedResult<TdwBatchViewModel>();

        //    List<DcFile> allFiles = await _context.DcFiles.Where(bn => bn.TdwBatch > 1).AsNoTracking().ToListAsync();
        //    List<DcFile> dcFiles = allFiles.OrderByDescending(f => f.UpdatedDate).Skip((page - 1) * 20).Take(20).ToList();

        //    var batchFiles = dcFiles.GroupBy(t => t.TdwBatch)
        //       .Select(grp => grp.First())
        //       .ToList();

        //    foreach (var box in batchFiles)
        //    {
        //        result.result.Add(
        //        new TdwBatchViewModel
        //        {
        //            TdwBatchNo = (int)box.TdwBatch,
        //            Region = GetRegion(box.RegionId),
        //            Boxes = (int)dcFiles.Where(f => f.TdwBatch == box.TdwBatch).Count(),
        //            Files = dcFiles.Where(f => f.TdwBatch == box.TdwBatch).Count(),
        //            User = _session.SamName,
        //            TdwSendDate = box.TdwBatchDate
        //        });

        //    }
        //    result.count = batchFiles.Count();

        //    return result;
        //}

        //public async Task<PagedResult<ReboxListItem>> SearchBox(string boxNo, int page, string searchText)
        //{
        //    PagedResult<ReboxListItem> result = new PagedResult<ReboxListItem>();
        //    searchText = searchText.ToUpper();
        //    if (StaticD.GrantTypes == null)
        //    {
        //        _ = GetGrantTypes();
        //    }
        //    result.count = _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo && (bn.ApplicantNo.Contains(searchText) || bn.BrmBarcode.Contains(searchText))).Count();
        //    if (result.count == 0) throw new Exception("No result!");
        //    result.result = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo && (bn.ApplicantNo.Contains(searchText) || bn.BrmBarcode.Contains(searchText))).OrderByDescending(f => f.UpdatedDate).Skip((page - 1) * 20).Take(20).OrderBy(f => f.UnqFileNo).AsNoTracking()
        //                .Select(f => new ReboxListItem
        //                {
        //                    ClmNo = f.UnqFileNo,
        //                    BrmNo = f.BrmBarcode,
        //                    IdNo = f.ApplicantNo,
        //                    FullName = f.FullName,
        //                    GrantType = StaticD.GrantTypes[f.GrantType],
        //                    BoxNo = boxNo,
        //                    AltBoxNo = f.AltBoxNo,
        //                    Scanned = f.ScanDatetime != null,
        //                    MiniBox = (int?)f.MiniBoxno,
        //                    TdwBatch = (int)f.TdwBatch
        //                }).ToListAsync();
        //    return result;
        //}

        //public async Task LockBox(string boxNo)
        //{
        //    List<DcFile> boxfiles = await _context.DcFiles.Where(b => b.TdwBoxno == boxNo).ToListAsync();
        //    boxfiles.ForEach(a => a.BoxLocked = 1);
        //    await _context.SaveChangesAsync();
        //}

        ///// <summary>
        ///// TDW Bat submit reboxing change
        ///// </summary>
        ///// <param name="boxNo"></param>
        ///// <param name="IsOpen"></param>
        ///// <returns></returns>
        //public async Task<bool> OpenCloseBox(string boxNo,bool IsOpen)
        //{
        //    int tdwBatch = IsOpen ? 1 : 0;

        //    await _context.DcFiles.Where(b => b.TdwBoxno == boxNo).ForEachAsync(f => f.TdwBatch = tdwBatch);

        //    await _context.SaveChangesAsync();

        //    return !IsOpen;
        //}
        //public async Task<bool> IsBoxLocked(string boxNo)
        //{
        //    return await _context.DcFiles.Where(b => b.TdwBoxno == boxNo && b.BoxLocked == 1).AnyAsync();
        //}

        //public async Task RemoveFileFromBox(string brmBarcode)
        //{
        //    var files = _context.DcFiles.Where(b => b.BrmBarcode == brmBarcode);
        //    foreach (var file in files)
        //    {
        //        file.TdwBoxno = null;
        //    }
        //    await _context.SaveChangesAsync();
        //}
        //private async Task<bool> RepairAltBoxSequence(string boxNo)
        //{
        //    string AltBoxNo;
        //    //Repair null altbox values

        //    if (_context.DcFiles.Where(b => string.IsNullOrEmpty(b.AltBoxNo) && b.TdwBoxno == boxNo).Any())
        //    {
        //        var altboxes = _context.DcFiles.Where(b => !string.IsNullOrEmpty(b.AltBoxNo) && b.TdwBoxno == boxNo);
        //        if (altboxes.Any())
        //        {
        //            AltBoxNo = altboxes.First().AltBoxNo;
        //        }
        //        else
        //        {
        //            AltBoxNo = await GetNexRegionAltBoxSequence();
        //        }
        //        var fix = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).ToListAsync();
        //        foreach (var file in fix)
        //        {
        //            file.AltBoxNo = AltBoxNo;
        //        }
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    //Repair RegionMisMatch values
        //    if (_context.DcFiles.Where(b => !b.AltBoxNo.Contains(session.Office.RegionCode) && b.TdwBoxno == boxNo).Any())
        //    {
        //        AltBoxNo = await GetNexRegionAltBoxSequence();
        //        var fix = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).ToListAsync();
        //        foreach (var file in fix)
        //        {
        //            file.AltBoxNo = AltBoxNo;
        //        }
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}
        //public async Task<List<ReboxListItem>> GetAllFilesByBoxNo(string boxNo, bool notScanned = false)
        //{


        //    _ = GetGrantTypes();
        //    if (notScanned)
        //    {
        //        return await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo && bn.ScanDatetime == null).OrderBy(f => f.UnqFileNo).AsNoTracking()
        //        .Select(f => new ReboxListItem
        //        {
        //            ClmNo = f.UnqFileNo,
        //            BrmNo = f.BrmBarcode,
        //            IdNo = f.ApplicantNo,
        //            FullName = f.FullName,
        //            GrantType = StaticD.GrantTypes[f.GrantType],
        //            BoxNo = boxNo,
        //            AltBoxNo = f.AltBoxNo,
        //            Scanned = f.ScanDatetime != null
        //        }).ToListAsync();
        //    }
        //    return await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).OrderBy(f => f.UnqFileNo).AsNoTracking()
        //    .Select(f => new ReboxListItem
        //    {
        //        ClmNo = f.UnqFileNo,
        //        BrmNo = f.BrmBarcode,
        //        IdNo = f.ApplicantNo,
        //        FullName = f.FullName,
        //        GrantType = StaticD.GrantTypes[f.GrantType],
        //        BoxNo = boxNo,
        //        AltBoxNo = f.AltBoxNo,
        //        Scanned = f.ScanDatetime != null
        //    }).ToListAsync();
        //}

        //public async Task SetBulkReturned(string boxNo, bool sendTDWMail)
        //{
        //    List<string> parentlist = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).AsNoTracking().Select(f => f.BrmBarcode).ToListAsync();
        //    IQueryable query = _context.DcMerges.AsNoTracking();
        //    foreach (string parent in parentlist)
        //    {
        //        var item = await _context.DcPicklistItems.Where(i => i.BrmNo == parent).FirstOrDefaultAsync();
        //        if (item == null) continue;
        //        item.Status = "Returned";
        //        List<string> childlist = await _context.DcMerges.AsNoTracking().Where(bn => bn.ParentBrmBarcode == parent).Select(c => c.BrmBarcode).ToListAsync();

        //        foreach (string child in childlist)
        //        {
        //            item = await _context.DcPicklistItems.Where(i => i.BrmNo == child).FirstOrDefaultAsync();
        //            if (item == null || item.Status == "Returned") continue;
        //            item.Status = "Returned";
        //        }
        //        await _context.SaveChangesAsync();
        //        await SyncPicklistFromItems(item.UnqPicklist, "Returned");
        //    }

        //    //Return MisFiles
        //    List<string> mislist = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).AsNoTracking().Select(f => f.FileNumber).ToListAsync();
        //    foreach (string parent in mislist)
        //    {
        //        var item = await _context.DcPicklistItems.Where(i => i.BrmNo == parent).FirstOrDefaultAsync();//TDWData has the misfileno i.s.o.brmno in old records
        //        if (item == null) continue;
        //        item.Status = "Returned";
        //        await _context.SaveChangesAsync();
        //        await SyncPicklistFromItems(item.UnqPicklist, "Returned");
        //    }

        //    //Send tdw email with csv of returned files for LC boxes.
        //    if (sendTDWMail)
        //    {
        //        await SendTDWReturnedMail(boxNo);
        //    }

        //}

        //public async Task SendTDWReturnedMail(string boxNo)
        //{
        //    List<TDWRequestMain> tpl = new List<TDWRequestMain>();
        //    List<DcFile> parentlist = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).AsNoTracking().ToListAsync();
        //    TDWRequestMain TdwFormat;
        //    foreach (DcFile parent in parentlist)
        //    {
        //        TdwFormat = new TDWRequestMain
        //        {
        //            BRM_No = parent.BrmBarcode,
        //            CLM_No = parent.UnqFileNo,
        //            Folder_ID = parent.UnqFileNo,
        //            Grant_Type = parent.GrantType,
        //            Firstname = parent.UserFirstname,
        //            Surname = parent.UserLastname,
        //            ID_Number = parent.ApplicantNo,
        //            Year = parent.UpdatedDate.Value.ToString("YYYY"),
        //            Location = parent.TdwBoxno,
        //            Reg = parent.RegType,
        //            //Bin  = parent. ,
        //            Box = parent.MiniBoxno.ToString(),
        //            //Pos  = parent. ,
        //            UserPicked = ""
        //        };
        //        tpl.Add(TdwFormat);

        //    }
        //    string FileName = session.Office.RegionCode + "-" + session.SamName.ToUpper() + $"-TDW_ReturnedBox_{boxNo.Trim()}-" + DateTime.Now.ToShortDateString().Replace("/", "-") + "-" + DateTime.Now.ToShortTimeString().Replace(":", "-");
        //    //attachment list
        //    List<string> files = new List<string>();
        //    //write attachments for manual download/add to mail
        //    File.WriteAllText(StaticD.ReportFolder + $@"{FileName}.csv", tpl.CreateCSV());
        //    files.Add(StaticD.ReportFolder + $@"{FileName}.csv");
        //    //send mail to TDW
        //    try
        //    {
        //        //if (!Environment.MachineName.ToLower().Contains("prod")) return;
        //        _mail.SendTDWIncoming(session, boxNo, files);
        //    }
        //    catch
        //    {
        //        //ignore confirmation errors
        //    }
        //}

        ///// <summary>
        ///// New TDW Batch feature
        ///// </summary>
        ///// <param name="tdwBatchNo"></param>
        ///// <returns></returns>
        //public async Task SendTDWBulkReturnedMail(int tdwBatchNo)
        //{
        //    List<TDWRequestMain> tpl = new List<TDWRequestMain>();
        //    List<DcFile> parentlist;
        //    TDWRequestMain TdwFormat;
        //    foreach (string boxNo in await _context.DcFiles.Where(bn => bn.TdwBatch == tdwBatchNo).AsNoTracking().Select(b => b.TdwBoxno).Distinct().ToListAsync())
        //    {

        //        parentlist = await _context.DcFiles.Where(bn => bn.TdwBoxno == boxNo).AsNoTracking().ToListAsync();
        //        foreach (DcFile parent in parentlist)
        //        {
        //            TdwFormat = new TDWRequestMain
        //            {
        //                BRM_No = parent.BrmBarcode,
        //                CLM_No = parent.UnqFileNo,
        //                Folder_ID = parent.UnqFileNo,
        //                Grant_Type = parent.GrantType,
        //                Firstname = parent.UserFirstname,
        //                Surname = parent.UserLastname,
        //                ID_Number = parent.ApplicantNo,
        //                Year = parent.UpdatedDate.Value.ToString("YYYY"),
        //                Location = parent.TdwBoxno,
        //                Reg = parent.RegType,
        //                //Bin  = parent. ,
        //                Box = parent.MiniBoxno.ToString(),
        //                //Pos  = parent. ,
        //                UserPicked = ""
        //            };
        //            tpl.Add(TdwFormat);
        //        }

        //    }
        //    string FileName = session.Office.RegionCode + "-" + session.SamName.ToUpper() + $"-TDW_ReturnedBatch_{tdwBatchNo}-" + DateTime.Now.ToShortDateString().Replace("/", "-") + "-" + DateTime.Now.ToShortTimeString().Replace(":", "-");
        //    //attachment list
        //    List<string> files = new List<string>();
        //    //write attachments for manual download/add to mail
        //    File.WriteAllText(StaticD.ReportFolder + $@"{FileName}.csv", tpl.CreateCSV());
        //    files.Add(StaticD.ReportFolder + $@"{FileName}.csv");
        //    //send mail to TDW
        //    try
        //    {
        //        //if (!Environment.MachineName.ToLower().Contains("prod")) return;
        //        _mail.SendTDWIncoming(session, tdwBatchNo, files);
        //    }
        //    catch
        //    {
        //        //ignore confirmation errors
        //    }
        //}

        //public async Task<DcFile> GetReboxCandidate(Reboxing rebox)
        //{
        //    List<DcFile> candidates = new List<DcFile>();
        //    //Find Barcode in DCFile
        //    if (!string.IsNullOrEmpty(rebox.BrmBarcode))
        //    {
        //        candidates = await _context.DcFiles.Where(f => f.BrmBarcode == rebox.BrmBarcode && f.BoxLocked != 1).ToListAsync();
        //    }
        //    else
        //    {
        //        //Try the newBarcode in DCFiles
        //        //candidates = await _context.DcFiles.Where(f => f.BrmBarcode == rebox.NewBarcode).ToListAsync();
        //        if (checkBRMExists(rebox.NewBarcode)) throw new Exception("The new barcode already exists!");
        //        //if (!candidates.Any())
        //        //{
        //        //Try the MisFile in BRM
        //        candidates = await _context.DcFiles.Where(k => k.FileNumber == rebox.MisFileNo && k.BoxLocked != 1).ToListAsync();
        //        if (!candidates.Any())
        //        {
        //            //We need to create a record!
        //            var miss = await _context.MisLivelinkTbls.Where(mis => mis.FileNumber == rebox.MisFileNo).Select(
        //                mis => new DcFile
        //                {
        //                    UnqFileNo = "",
        //                    ApplicantNo = mis.IdNumber.GetDigitId(),
        //                    ApplicationStatus = mis.RegistryType.ApplicationStatusFromMIS(),
        //                    TransType = 0,
        //                    BatchAddDate = DateTime.Now,
        //                    BrmBarcode = rebox.NewBarcode,
        //                    FileComment = "Added from TDW/MIS on reboxing.",
        //                    FileNumber = mis.FileNumber,
        //                    FileStatus = "Completed",
        //                    GrantType = mis.GrantType.GrantTypeFromMIS(),
        //                    Isreview = "N",
        //                    MisBoxno = mis.BoxNumber,
        //                    OfficeId = session.Office.OfficeId,
        //                    RegionId = session.Office.RegionId,
        //                    FspId = session.Office.FspId,
        //                    TdwBoxno = "",//rebox.BoxNo,
        //                    TransDate = "2016-05-29".ToDate("yyyy-mm-dd"),
        //                    UpdatedByAd = session.SamName,
        //                    UpdatedDate = DateTime.Now,
        //                    UserFirstname = mis.Name,
        //                    UserLastname = mis.Surname
        //                }).ToListAsync();
        //            if (!miss.Any()) throw new Exception("No suitable MIS record found to create BRM record, please recapture.");
        //            _context.ChangeTracker.Clear();
        //            var misfiledata = miss.First();
        //            if (string.IsNullOrEmpty(misfiledata.ApplicantNo)) throw new Exception("No suitable MIS record found to create BRM record, please recapture.");
        //            _context.DcFiles.Add(misfiledata);
        //            await _context.SaveChangesAsync();
        //            candidates = await _context.DcFiles.Where(f => f.BrmBarcode == rebox.NewBarcode).ToListAsync();
        //            // }
        //        }
        //        rebox.BrmBarcode = rebox.NewBarcode;
        //        rebox.MisFileNo = string.Empty;
        //        rebox.NewBarcode = string.Empty;
        //    }

        //    if (!candidates.Any())
        //    {
        //        throw new Exception("No Suitable BRM record found.");
        //    }

        //    if (candidates.Count() > 1)
        //    {
        //        //Try Repair
        //        if (candidates.Where(c => string.IsNullOrEmpty(c.ApplicantNo) && c.BrmBarcode == rebox.BrmBarcode && c.BoxLocked != 1).Any())
        //        {
        //            DcFile corrupt = _context.DcFiles.Where(c => string.IsNullOrEmpty(c.ApplicantNo) && c.BrmBarcode == rebox.BrmBarcode).First();
        //            _context.DcFiles.Remove(corrupt);
        //            await _context.SaveChangesAsync();
        //            candidates = await _context.DcFiles.Where(f => f.BrmBarcode == rebox.BrmBarcode && f.BoxLocked != 1).ToListAsync();
        //        }
        //        if (candidates.Count() > 1)
        //        {
        //            throw new Exception($"Duplicate BRM No {rebox.BrmBarcode} please delete/recapture this file.");
        //        }
        //    }
        //    if (candidates.Where(f => f.BrmBarcode == rebox.BrmBarcode && f.TdwBoxno == rebox.BoxNo).Any())
        //    {
        //        throw new Exception($"{rebox.BrmBarcode} is already in this box !");
        //    }
        //    return candidates.First();
        //}
        //public async Task Rebox(Reboxing rebox, DcFile file)
        //{

        //    try
        //    {
        //        file.MisReboxDate = DateTime.Now;
        //        file.MisReboxStatus = "Completed";
        //        file.TdwBoxno = rebox.BoxNo;
        //        file.MiniBoxno = rebox.MiniBox;
        //        file.TdwBoxTypeId = decimal.Parse(rebox.SelectedType);
        //        file.TdwBatch = 0;
        //        //file.FileNumber = rebox.MisFileNo;
        //        if ("14|15|16|17|18".Contains(rebox.SelectedType))
        //        {
        //            file.TdwBoxArchiveYear = rebox.ArchiveYear;
        //        }
        //        else
        //        {
        //            file.TdwBoxArchiveYear = null;
        //        }

        //        file.AltBoxNo = rebox.AltBoxNo;
        //        file.UpdatedDate = DateTime.Now;
        //        file.UpdatedByAd = session.SamName;

        //        await _context.SaveChangesAsync();
        //        CreateActivity("Reboxing" + GetFileArea(file.SrdNo, file.Lctype), "Rebox file", file.UnqFileNo);
        //    }
        //    catch //(Exception ex)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<string> GetNexRegionAltBoxSequence()
        //{
        //    return session.Office.RegionCode + await _raw.GetNextAltbox(session.Office.RegionCode);
        //}

        ///// <summary>
        ///// Create TDW batch and send mail
        ///// </summary>
        ///// <param name="boxes"></param>
        ///// <returns></returns>
        //public async Task<int> CreateTdwBatch(List<TdwBatchViewModel> boxes)
        //{
        //    int tdwBatch = await _raw.GetNextTdwBatch();
        //    foreach (var box in boxes)
        //    {
        //        box.TdwSendDate = DateTime.Now;
        //        box.TdwBatchNo = tdwBatch;
        //        box.User = _session.SamName;
        //        await _context.DcFiles.Where(f => f.TdwBoxno == box.BoxNo).ForEachAsync(f => { f.TdwBatch = tdwBatch; f.TdwBatchDate = box.TdwSendDate; });
        //    }
        //    await _context.SaveChangesAsync();
        //    await SendTDWBulkReturnedMail(tdwBatch);
        //    return tdwBatch;
        //}

        ///// <summary>
        ///// Get TDW batch and send mail
        ///// </summary>
        ///// <param name="boxes"></param>
        ///// <returns></returns>
        //public async Task<List<TdwBatchViewModel>> GetTdwBatch(int tdwBatchno)
        //{
        //    List<TdwBatchViewModel> boxes = new List<TdwBatchViewModel>();
        //    var dcFiles = await _context.DcFiles.Where(bn => bn.TdwBatch == tdwBatchno).AsNoTracking().ToListAsync();

        //    var batchFiles = dcFiles.GroupBy(t => t.TdwBoxno)
        //       .Select(grp => grp.First())
        //       .ToList();

        //        foreach(var box in batchFiles)
        //        {
        //            boxes.Add(
        //            new TdwBatchViewModel
        //            {
        //                BoxNo = box.TdwBoxno,
        //                Region = GetRegion(box.RegionId),
        //                MiniBoxes = (int) dcFiles.Where(f => f.TdwBoxno == box.TdwBoxno).Max(f => f.MiniBoxno),
        //                Files = dcFiles.Where(f => f.TdwBoxno == box.TdwBoxno).Count(),
        //                User = _session.SamName,
        //                TdwSendDate = dcFiles.Where(f => f.TdwBoxno == box.TdwBoxno).Max(f => f.TdwBatchDate)
        //            });

        //        }
        //        return boxes;
        //}

        //public async Task<List<TdwBatchViewModel>> GetTdwBatches()
        //{
        //    List<TdwBatchViewModel> boxes = new List<TdwBatchViewModel>();
        //    var dcFiles = await _context.DcFiles.Where(bn => bn.TdwBatch > 1).AsNoTracking().ToListAsync();

        //    var batchFiles = dcFiles.GroupBy(t => t.TdwBatch)
        //       .Select(grp => grp.First())
        //       .ToList();

        //    foreach (var box in batchFiles)
        //    {
        //        boxes.Add(
        //        new TdwBatchViewModel
        //        {
        //            Region = GetRegion(box.RegionId),
        //            Boxes = (int)dcFiles.Where(f => f.TdwBatch == box.TdwBatch).Count(),
        //            Files = dcFiles.Where(f => f.TdwBatch == box.TdwBatch).Count(),
        //            User = _session.SamName,
        //            TdwSendDate = box.TdwBatchDate
        //        });

        //    }
        //    return boxes;
        //}

        //#endregion



        #region SocpenData
        public async Task<string> GetSocpenSearchId(string srdNo)
        {
            if (!srdNo.IsNumeric()) throw new Exception("SRD is Invalid.");

            long srd = long.Parse(srdNo);
            var result = await _context.DcSocpen.Where(s => s.SrdNo == srd).ToListAsync();

            if (!result.Any()) throw new Exception("SRD not found.");

            return result.First().BeneficiaryId;
        }
        #endregion

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Create standard report file name
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public string GetFileName(string reportName)
        {
            return $"{_session.Office.RegionCode}-{_session.SamName.ToUpper()}-{reportName}-{DateTime.Now.ToShortDateString().Replace("/", "-")}-{DateTime.Now.ToString("HH-mm")}";
        }
        /// <summary>
        /// Create Activity Record
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Activity"></param>
        /// <returns></returns>
        public void CreateActivity(string Area, string Activity, string UniqueFileNo = "")
        {
            DcActivity activity = new DcActivity { ActivityDate = DateTime.Now, RegionId = session.Office.RegionId, OfficeId = decimal.Parse(session.Office.OfficeId), Userid = 0, Username = session.SamName, Area = Area, Activity = Activity, Result = "OK", UnqFileNo = UniqueFileNo };
            try
            {

                _context.DcActivities.Add(activity);
                _context.SaveChanges();

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

        /// <summary>
        /// Write System Event log entry
        /// </summary>
        /// <param name="eventLogEntry"></param>
        public void WriteEvent(string eventLogEntry)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(eventLogEntry, EventLogEntryType.Error, 101, 1);
            }
        }


    }
}

