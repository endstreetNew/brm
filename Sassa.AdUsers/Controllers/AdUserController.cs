
using System;
using System.Collections.Generic;
using System.Linq;
using Sassa.BRM.Models;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;

namespace Sassa.AdUsers
{

    public class AdUserController : ControllerBase
    {

        public List<string> GetLORoles()
        {
            //PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "SASSA");
            //UserPrincipal user = UserPrincipal.FindByIdentity(ctx, loginname);
            List<string> roles = new List<string>();
            WindowsIdentity.RunImpersonated(new Microsoft.Win32.SafeHandles.SafeAccessTokenHandle(new IntPtr()), () => {
                var wi = WindowsIdentity.GetCurrent();
                if (wi.Groups != null)
                {
                    foreach (var group in wi.Groups)
                    {
                        try
                        {
                            string role = group.Translate(typeof(NTAccount)).ToString();
                            if (!role.Contains("SASSA")) continue;
                            if (role.Contains("\\"))
                            {
                                role = role.Substring(role.LastIndexOf("\\") + 1);
                            }
                            if (!role.Contains("GRP_LO_")) continue;
                            roles.Add(role);
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }
                    }

                }
            }
            );
            return roles;
        }

        public List<string> GetRoles(WindowsPrincipal principal)
        {

            List<string> roles = new List<string>();
            var wi = principal.Identity;
            var ss = "";
            //WindowsIdentity.RunImpersonated(new Microsoft.Win32.SafeHandles.SafeAccessTokenHandle(new IntPtr()) ,() => {
            //    var wi = (ClaimsPrincipal)principal.Identity;

            //    foreach(var role in userclaims.Where(c => c.Type == ClaimTypes.Role))
            //    {

            //    }
            //    if (principal.Identity.Groups != null)
            //    {
            //        foreach (var group in wi.Groups)
            //        {
            //            try
            //            {
            //                string role = group.Translate(typeof(NTAccount)).ToString();
            //                if (!role.Contains("SASSA")) continue;
            //                if (role.Contains("\\"))
            //                {
            //                    role = role.Substring(role.LastIndexOf("\\") + 1);
            //                }
            //                roles.Add(role);
            //            }
            //            catch (Exception e)
            //            {
            //                // ignored
            //            }
            //        }

            //    }
            //}
            //);
            return roles;
        }
    }
}
