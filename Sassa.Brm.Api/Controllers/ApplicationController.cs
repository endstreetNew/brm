using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Models;
using Sassa.BRM.Services;
using System;
using System.Security.Principal;
//using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Sassa.BRM.Controller
{
    [Route("[controller]")]
    [ApiController]

    public class ApplicationController : ControllerBase
    {

        private readonly BRMDbService _brmService;

        public ApplicationController(BRMDbService context)
        {
            _brmService = context;
            //try
            //{
            //    context.GetUserSession((WindowsIdentity)ctx.HttpContext.User.Identity);
            //    //if (!StaticD.Users.Contains(_session.SamName)) StaticD.Users.Add(_session.SamName);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //    //WriteEvent($"{ctx.HttpContext.User.Identity.Name} : {ex.Message}");
            //}
        }

        private string lastError;
        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DcFile>> PostApplication(Application app)
        {
            DcFile result;
            //try
            //{
            if (_brmService.session == null)
            {
                _brmService.SetUserSession();
                _brmService.SetUserOffice(app.OfficeId);
            }
            result = await _brmService.CreateBRM(app, "Inserted via API.");
            //}
            //catch (Exception ex)
            //{
            //    return GetLastError();
            //}
            //var xx = CreatedAtAction("GetUser", new { id = user.Id }, user);
            //return xx;
            return result;
        }
        [HttpGet]
        public ActionResult<string> GetLastError()
        {
            return lastError;
        }   

    }
}
