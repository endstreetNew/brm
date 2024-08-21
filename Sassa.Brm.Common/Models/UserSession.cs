namespace Sassa.Brm.Common.Models
{
    public class UserSession
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? SamName { get; set; }
        public List<string> Roles { get; set; }
        public UserOffice? Office { get; set; }
        public BookMarks BookMark { get; set; }

        public UserSession()
        {

            BookMark = new BookMarks();
            BookMark.BoxingTab = 1;
            Roles = new List<string>();
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => r == role);
        }

        public bool IsRmc()
        {
            if(Office == null)return false;
            return Office.OfficeType == "RMC";
        }
        public bool IsBrmUser()
        {
            return Roles.Any(r => r.Contains("GRP_BRM"));
        }
    }
}
