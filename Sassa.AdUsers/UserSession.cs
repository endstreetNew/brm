using System.DirectoryServices.AccountManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Sassa.BRM.Models;


namespace Sassa.BRM.Services
{
    public class UserSession
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string SamName { get; set; }
        public List<string> Roles { get; set; }
        public UserOffice Office { get; set; }

        public UserSession()
        {

        }
        //public UserSession(string loginname)
        //{
        //    PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "SASSA");
        //    UserPrincipal user = UserPrincipal.FindByIdentity(ctx, loginname);

        //    Roles = new List<string>();
        //    Office = new UserOffice();
        //    SamName = user.SamAccountName;
        //    Name = user.Name;
        //    Surname = user.Surname;
        //    Email = user.EmailAddress;
        //    //SetUserGroups();
        //}

        public bool IsInRole(string role)
        {
            return Roles.Any(r => r == role);
        }
        private void SetUserGroups()
        {
            var wi = WindowsIdentity.GetCurrent();

            if (wi.Groups != null)
            {
                foreach (var group in wi.Groups)
                {
                    try
                    {
                        string role = group.Translate(typeof(NTAccount)).ToString();
                        if (!role.Contains("SASSA")) continue;
                        if(role.Contains("\\"))
                        {
                            role = role.Substring(role.LastIndexOf("\\")+1);
                        }
                        Roles.Add(role);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
            }
        }
    }


}
