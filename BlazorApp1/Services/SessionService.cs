using Microsoft.AspNetCore.Http;
using Sassa.BRM.Helpers;
using Sassa.BRM.Models;
using System.DirectoryServices;
using System.Security.Principal;

namespace Sassa.BRM.Services
{
    public class SessionService
    {
        private UserSession _session;
        public UserSession session
        {
            get
            {
                return _session;
            }
            set
            { _session = value; }
        }
        public event EventHandler SessionInitialized;
        ModelContext _context;
        public SessionService(ModelContext context, IHttpContextAccessor ctx)
        {
            _context = context;

            try
            {
                _session = GetUserSession((WindowsIdentity)ctx.HttpContext.User.Identity);

                //if (!StaticD.Users.Contains(_session.SamName)) StaticD.Users.Add(_session.SamName);
            }
            catch //(Exception ex)
            {
                _session = null;
                //WriteEvent($"{ctx.HttpContext.User.Identity.Name} : {ex.Message}");
            }
        }

        public UserSession GetUserSession(WindowsIdentity identity)
        {
            //S-1-5-21-1204054820-1125754781-535949388-513
            _session = new UserSession();
            DirectoryEntry user = new DirectoryEntry($"LDAP://<SID={identity.User.Value}>");
            _session.SamName = (string)user.Properties["SAMAccountName"].Value;
            _session.Roles = identity.GetRoles();
            _session.Name = (string)user.Properties["name"].Value;
            _session.Surname = (string)user.Properties["sn"].Value;
            try
            {
                _session.Email = (string)user.Properties["mail"].Value;
            }
            catch
            {
                //WriteEvent("User has no email in Active Directory");
            }
            return _session;
        }
    }
}
