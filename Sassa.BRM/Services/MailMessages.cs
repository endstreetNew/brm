using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Sassa.BRM.Services
{
    public class MailMessages
    {
        private string _SMTPserver;
        private int _SMTPPort;
        private string _SMTPUser;
        private string _SMTPPassword;
        IConfiguration _config;

        public MailMessages(IConfiguration config, IWebHostEnvironment _env)
        {
            StaticD.ReportFolder = $"{_env.ContentRootPath}\\{config.GetValue<string>("Folders:Reports")}\\";
            StaticD.DocumentFolder = $"{_env.WebRootPath}\\{config.GetValue<string>("Folders:Documents")}\\";
            _SMTPserver = config.GetValue<string>("Email:SMTPServer");
            _SMTPPort = int.Parse(config.GetValue<string>("Email:SMTPPort"));
            _SMTPUser = config.GetValue<string>("Email:SMTPUser");
            _SMTPPassword = config.GetValue<string>("Email:SMTPPassword");
            _config = config;
            GetRegionEmails();
            GetRegionIDEmails();
        }

        public Dictionary<string, string> GetRegionEmails()
        {
            if (StaticD.RegionEmails != null) return StaticD.RegionEmails;
            StaticD.RegionEmails = new Dictionary<string, string>();
            StaticD.RegionEmails.Add("GAUTENG", _config.GetValue<string>("TDWEmail:GAUTENG"));
            StaticD.RegionEmails.Add("FREE STATE", _config.GetValue<string>("TDWEmail:FREE STATE"));
            StaticD.RegionEmails.Add("KWA-ZULU NATAL", _config.GetValue<string>("TDWEmail:KWA-ZULU NATAL"));
            StaticD.RegionEmails.Add("KWAZULU NATAL", _config.GetValue<string>("TDWEmail:KWA-ZULU NATAL"));
            StaticD.RegionEmails.Add("NORTH WEST", _config.GetValue<string>("TDWEmail:NORTH WEST"));
            StaticD.RegionEmails.Add("MPUMALANGA", _config.GetValue<string>("TDWEmail:MPUMALANGA"));
            StaticD.RegionEmails.Add("EASTERN CAPE", _config.GetValue<string>("TDWEmail:EASTERN CAPE"));
            StaticD.RegionEmails.Add("WESTERN CAPE", _config.GetValue<string>("TDWEmail:WESTERN CAPE"));
            StaticD.RegionEmails.Add("LIMPOPO", _config.GetValue<string>("TDWEmail:LIMPOPO"));
            StaticD.RegionEmails.Add("NORTHERN CAPE", _config.GetValue<string>("TDWEmail:NORTHERN CAPE"));
            return StaticD.RegionEmails;

        }
        public Dictionary<string, string> GetRegionIDEmails()
        {
            if (StaticD.RegionIDEmails != null) return StaticD.RegionIDEmails;
            StaticD.RegionIDEmails = new Dictionary<string, string>();
            StaticD.RegionIDEmails.Add("7", _config.GetValue<string>("TDWEmail:GAUTENG"));
            StaticD.RegionIDEmails.Add("4", _config.GetValue<string>("TDWEmail:FREE STATE"));
            StaticD.RegionIDEmails.Add("5", _config.GetValue<string>("TDWEmail:KWA-ZULU NATAL"));
            StaticD.RegionIDEmails.Add("6", _config.GetValue<string>("TDWEmail:NORTH WEST"));
            StaticD.RegionIDEmails.Add("8", _config.GetValue<string>("TDWEmail:MPUMALANGA"));
            StaticD.RegionIDEmails.Add("2", _config.GetValue<string>("TDWEmail:EASTERN CAPE"));
            StaticD.RegionIDEmails.Add("1", _config.GetValue<string>("TDWEmail:WESTERN CAPE"));
            StaticD.RegionIDEmails.Add("9", _config.GetValue<string>("TDWEmail:LIMPOPO"));
            StaticD.RegionIDEmails.Add("3", _config.GetValue<string>("TDWEmail:NORTHERN CAPE"));
            return StaticD.RegionIDEmails;
        }

        public void SendTDWReceipt(UserSession session, string tdwOfficemail, string PickListNo, List<string> files)
        {
            using (EmailClient client = new EmailClient(_SMTPserver, _SMTPPort, new System.Net.NetworkCredential(_SMTPUser, _SMTPPassword)))
            {
                //send mail to TDW
                string body = $"Thank you,<br/><br/>Piclist no: {PickListNo} has been received by our office.<br/><br/>Kind Regards<br/><br/>{session.Name}<br/><br/><br/><br/>";
                client.SendMail($"no-reply@sassa.gov.za", tdwOfficemail, @"SASSA Picklist receipt.", body, files);
                //send mail to Originator
                body = $"A receipt for Picklist no: {PickListNo} has been sent to TDW.<br/><br/>Kind Regards<br/><br/>BRM System<br/>Please do not reply to this mail<br/><br/><br/>";
                client.SendMail($"no-reply@sassa.gov.za", session.Email, "SASSA Picklist receipt sent", body, files);
            }
        }
        public void SendTDWRequest(UserSession session, string tdwOfficemail, string PickListNo, List<string> files)
        {
            using (EmailClient client = new EmailClient(_SMTPserver, _SMTPPort, new System.Net.NetworkCredential(_SMTPUser, _SMTPPassword)))
            {
                //send mail to TDW
                string body = $"Please find attached file request(s) for processing (Picklist#{PickListNo}).<br/><br/>Kind Regards<br/><br/>{session.Name}<br/><br/><br/><br/>";
                client.SendMail($"no-reply@sassa.gov.za", tdwOfficemail, @"SASSA File request.", body, files);
                //send mail to Originator
                body = $"Please find attached file request(s) sent to TDW for processing (PickList#{PickListNo}).<br/><br/>Kind Regards<br/><br/>BRM System<br/>Please do not reply to this mail<br/><br/><br/>";
                client.SendMail($"no-reply@sassa.gov.za", session.Email, "SASSA File request confirmation", body, files);
            }
        }

        //Returned box detail
        public void SendTDWIncoming(UserSession session, string tdwOfficemail, string Boxno, List<string> files)
        {
            string returnedBox = _config.GetValue<string>("TDWReturnedBox:NatashaT@tdw.co.za");
            using (EmailClient client = new EmailClient(_SMTPserver, _SMTPPort, new System.Net.NetworkCredential(_SMTPUser, _SMTPPassword)))
            {
                //send mail to TDW
                string body = $"Please find attached incoming box for processing (BoxNo#{Boxno}).<br/><br/>Kind Regards<br/><br/>{session.Name}<br/><br/><br/><br/>";
                client.SendMail($"no-reply@sassa.gov.za", returnedBox, @"SASSA Incoming files.", body, files);
                //send mail to Originator
                body = $"Please find attached box detail sent to TDW for processing (BoxNo#{Boxno}).<br/><br/>Kind Regards<br/><br/>BRM System<br/>Please do not reply to this mail<br/><br/><br/>";
                client.SendMail($"no-reply@sassa.gov.za", session.Email, "SASSA Files returned to TDW", body, files);
            }
        }

        public void SendRequestStatusChange(UserSession session, string status, string idNo, string ToMail)
        {
            using (EmailClient client = new EmailClient(_SMTPserver, _SMTPPort, new System.Net.NetworkCredential(_SMTPUser, _SMTPPassword)))
            {
                //send mail to Requestor
                string body = $"<br/>The Status has been updated to {status} on Picklist: {idNo} requested by you.<br/><br/>Kind Regards<br/><br/>BRM System<br/>Please do not reply to this mail<br/><br/><br/>";
                client.SendMail($"no-reply@sassa.gov.za", ToMail, "FILE REQUEST STATUS CHANGE NOTIFICATION", body, new List<string>());
            }
        }
    }
}
