using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Sassa.Brm.Api.Helpers;
using Sassa.BRM.Models;
using Sassa.BRM.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
//using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Sassa.BRM.Controller
{
    [Route("[controller]")]
    [ApiController]

    public class ApplicationController : ControllerBase
    {

        private readonly BRMDbService _brmService;
        IConfiguration _config;

        public ApplicationController(BRMDbService context,IConfiguration config)
        {
            _brmService = context;
            _config = config;
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

        //private string? lastError;
        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DcFile>> PostApplication(Application app)
        {
            DcFile result;

            if(string.IsNullOrEmpty(app.BrmUserName))
            {
                app.BrmUserName = _config.GetValue<string>("BrmUser")!;
            }

            //try
            //{
            if (_brmService.session == null)
            {
                _brmService.SetUserSession(app.BrmUserName);
                _brmService.SetUserOffice(app.OfficeId);
            }
            //result = await _brmService.CreateBRM(app, "Inserted via API.");
            //if (result == null)
            //{
            //    return BadRequest();
            //}
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                result = await _brmService.CreateBRM(app, "Inserted via API.");
                return result;
            }
            catch (Exception ex)
            {
                // Handle both ValidationException and InternalServerErrorException here
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return Ok(response);
            //} 
            //catch (Exception ex)
            //{
            //    return GetLastError();
            //}
            //var xx = CreatedAtAction("GetUser", new { id = user.Id }, user);
            //return xx;
           
        }
        //[HttpGet]
        //public ActionResult<string> GetLastError()
        //{
        //    return lastError;
        //}   

    }
}
