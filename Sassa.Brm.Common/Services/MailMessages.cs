//using Microsoft.AspNetCore.Hosting;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.Configuration;
using Sassa.Brm.Common.Models;
using Sassa.Brm.Common.Services;
using System.Collections.Generic;

namespace Sassa.Brm.Common.Services;


public class MailMessages
{
    private string _SMTPserver;
    private int _SMTPPort;
    private string _SMTPUser;
    private string _SMTPPassword;
    IEmailSettings _addresses;
    public MailMessages(IConfiguration config,IEmailSettings addresses)
    {

        _SMTPserver = config.GetValue<string>("Email:SMTPServer")!;
        _SMTPUser = config.GetValue<string>("Email:SMTPUser")!;
        _SMTPPassword = config.GetValue<string>("Email:SMTPPassword")!;
        _SMTPPort= config.GetValue<int>("Email:SMTPPort")!;
        _addresses = addresses;
        //"SMTPHost": "mail.sassa.gov.za",
        GetRegionEmails();
        GetRegionIDEmails();
    }

    public Dictionary<string, string> GetRegionEmails()
    {
        if (StaticDataService.RegionEmails != null) return StaticDataService.RegionEmails;
        StaticDataService.RegionEmails = _addresses.RegionEmails;
        return StaticDataService.RegionEmails;

    }
    public Dictionary<string, string> GetRegionIDEmails()
    {
        if (StaticDataService.RegionIDEmails != null) return StaticDataService.RegionIDEmails;
        StaticDataService.RegionIDEmails = _addresses.RegionIDEmails;
        return StaticDataService.RegionIDEmails;
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
            client.SendMail($"no-reply@sassa.gov.za", session.Email!, "SASSA File request confirmation", body, files);
        }
    }

    //Returned box detail
    public void SendTDWIncoming(UserSession session, string Boxno, List<string> files,string? file = null)
    {
        if (file != null)
        {
            files = new List<string>();
            files.Add(file);
        }
        using (EmailClient client = new EmailClient(_SMTPserver, _SMTPPort, new System.Net.NetworkCredential(_SMTPUser, _SMTPPassword)))
        {
            //send mail to TDW
            string body = $"Please find attached incoming box for processing (BoxNo#{Boxno}).<br/><br/>Kind Regards<br/><br/>{session.Name}<br/><br/><br/><br/>";
            client.SendMail($"no-reply@sassa.gov.za", _addresses.TdwReturnedBox!, @"SASSA Incoming files.", body, files);
            //send mail to Originator
            body = $"Please find attached box detail sent to TDW for processing (BoxNo#{Boxno}).<br/><br/>Kind Regards<br/><br/>BRM System<br/>Please do not reply to this mail<br/><br/><br/>";
            client.SendMail($"no-reply@sassa.gov.za", session.Email!, "SASSA Files returned to TDW", body, files);
        }
    }

    //Returned batch detail
    //To replace the above method
    public void SendTDWIncoming(UserSession session, int tdwBatchNo, List<string> files)
    {
        
        using (EmailClient client = new EmailClient(_SMTPserver, _SMTPPort, new System.Net.NetworkCredential(_SMTPUser, _SMTPPassword)))
        {
            //send mail to TDW
            string body = $"Please find attached incoming batch for processing (BatchNo#{tdwBatchNo}).<br/><br/>Kind Regards<br/><br/>{session.Name}<br/><br/><br/><br/>";
            client.SendMail($"no-reply@sassa.gov.za", _addresses.TdwReturnedBox!, @"SASSA Incoming files.", body, files);
            //send mail to Originator
            body = $"Please find attached box detail sent to TDW for processing (BatchNo#{tdwBatchNo}).<br/><br/>Kind Regards<br/><br/>BRM System<br/>Please do not reply to this mail<br/><br/><br/>";
            client.SendMail($"no-reply@sassa.gov.za", session.Email!, "SASSA Files returned to TDW", body, files);
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
