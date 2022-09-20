using System;
using System.Collections.Generic;
using System.Text;

namespace Sassa.BRM.Models

{
    public class AdUser
    {
        public AdUser()
        {
            Roles = new List<string>();
            Office = new UserOffice();
            SamName = "";
            Name = "";
            Surname = "";
            Email = "";
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string SamName { get; set; }
        public List<string> Roles { get; set; }
        public UserOffice Office { get; set; }
    }
}
