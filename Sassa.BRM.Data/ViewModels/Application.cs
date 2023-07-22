using System.ComponentModel.DataAnnotations.Schema;

namespace Sassa.BRM.Models
{
    public class Application
    {
        public decimal SocpenIsn { get; set; }
        public string ARCHIVE_YEAR { get; set; }
        public string Id { get; set; }
        //Change to accomodate invalid child ids from socpen
        private string child_id;
        public string ChildId 
        {
            get { return child_id; }
            set { child_id = child_id.PadLeft(13, ' '); }
        }//-----------------------------------------------
        public string Name { get; set; }
        public string SurName { get; set; }
        public string RegionId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string GrantType { get; set; }
        public string GrantName { get; set; }
        public string AppDate { get; set; }
        /// <summary>
        /// MAIN / ARCHIVE
        /// </summary>
        public string AppStatus { get; set; }
        public string StatusDate { get; set; }
        public string DocsPresent { get; set; }
        public string LastReviewDate { get; set; }
        public string DateApproved { get; set; }
        public string Prim_Status { get; set; }
        public string Sec_Status { get; set; }
        public string LcType { get; set; }
        public string Child_App_Date { get; set; }
        public string Child_Status_Date { get; set; }
        public string Child_Status_Code { get; set; }
        public string Srd_No { get; set; }
        public string Brm_Parent { get; set; }
        public string Brm_BarCode { get; set; }
        public string Clm_No { get; set; }
        public string TDW_BOXNO { get; set; }
        public int MiniBox { get; set; }
        public string BATCH_NO { get; set; }
        public string IdHistory { get; set; }
        public string IsRMC { get; set; }
        public decimal? TRANS_TYPE { get; set; }
        public bool IsNew { get; set; }
        public string RowType { get; set; }
        public string Source { get; set; }
        public bool IsMergeCandidate { get; set; }
        public bool IsCombinationCandidate { get; set; }
        public string FullName
        {
            get
            {
                return Name + " " + SurName;
            }
        }
        public string Status
        {
            get
            {
                return AppStatus.Contains("MAIN") ? "Active" : "Inactive";
            }
        }
        public bool IsPreservedType
        {
            get
            {
                return "MAIN|LC-MAIN|ARCHIVE|LC-ARCHIVE".Contains(AppStatus);
            }
        }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            if (string.IsNullOrEmpty(SurName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Id))
            {
                return false;
            }
            if (string.IsNullOrEmpty(GrantType))
            {
                return false;
            }
            if (!"MAIN|ARCHIVE|LC-MAIN|LC-ARCHIVE".Contains(AppStatus))
            {
                return false;
            }
            if (string.IsNullOrEmpty(DocsPresent))
            {
                return false;
            }
            return true;
        }
    }
}
