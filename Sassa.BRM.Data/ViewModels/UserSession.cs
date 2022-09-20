using Sassa.BRM.Models;
using Sassa.BRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;


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
        public BookMarks BookMark { get; set; }

        public UserSession()
        {
            Office = new UserOffice();
            BookMark = new BookMarks();
            BookMark.BoxingTab = 1;
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => r == role);
        }

        public bool IsRmc()
        {
            return Office.OfficeType == "RMC";
        }
        public bool IsBrmUser()
        {
            return Roles.Any(r => r.Contains("GRP_BRM"));
        }
        //private void SetUserGroups()
        //{
        //    var wi = WindowsIdentity.GetCurrent();

        //    if (wi.Groups != null)
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
        //                Roles.Add(role);
        //            }
        //            catch //(Exception e)
        //            {
        //                // ignored
        //            }
        //        }
        //    }
        //}
    }
}
