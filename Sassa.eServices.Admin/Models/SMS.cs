using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sassa.eServices.Admin.Models
{
    public class SMSRequest
    {

     public string reference
        {
            get
            {
                Random _rdm = new Random();
                return _rdm.Next(1000, 9999).ToString().Trim();
            }
        }
     public string cellNumber { get; set; }
     public string message { get; set; }
    }

    public class SMSResponse
    {

        public string Action { get; set; }
        public string Key { get; set; }
        public string Result { get; set; }
        public string Number { get; set; }
        public string Error { get; set; }
    }
}
