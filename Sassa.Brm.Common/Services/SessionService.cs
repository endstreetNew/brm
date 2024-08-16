using Microsoft.AspNetCore.Http;
using Sassa.Brm.Common.Helpers;
using Sassa.Brm.Common.Models;
using System.DirectoryServices;
using System.Security.Principal;

namespace Sassa.Brm.Common.Services
{
    public class SessionService 
    {
        private UserSession? _session;
        public UserSession? session
        {
            get
            {
                return _session;
            }
            set
            { _session = value; }
        }
        public event EventHandler? SessionInitialized;
        //ModelContext _context;

        private readonly WindowsIdentity _windowsIdentity;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            //_context = context;
            try
            {
                _windowsIdentity = (WindowsIdentity)httpContextAccessor.HttpContext!.User.Identity!;
                _session = GetUserSession(_windowsIdentity);
                SessionInitialized?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                _session = null;
                throw new Exception($"Some user details not found in AD {ex.Message}");
            }
        }

        public UserSession? GetUserSession(WindowsIdentity identity)
        {
            //S-1-5-21-1204054820-1125754781-535949388-513 (test value)
            _session = new UserSession();

            DirectoryEntry user = new DirectoryEntry($"LDAP://<SID={identity.User!.Value}>");
            _session.SamName = (string)user.Properties["SAMAccountName"].Value!;
            _session.Roles = identity.GetRoles();
            _session.Email = (string)user.Properties["mail"].Value!;
            _session.Name = (string)user.Properties["name"].Value!;
            _session.Surname = (string)user.Properties["sn"].Value!;

            return _session;
        }
    }
}
