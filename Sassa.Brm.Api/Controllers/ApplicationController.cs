using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sassa.Brm.Api.Helpers;
using Sassa.BRM.Models;
using Sassa.BRM.Services;
//using System.ServiceModel.Channels;

namespace Sassa.BRM.Controller
{
    [Route("[controller]")]
    [ApiController]

    public class ApplicationController : ControllerBase
    {

        private readonly BRMDbService _brmService;
        IConfiguration _config;

        public ApplicationController(BRMDbService context, IConfiguration config)
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

            if (string.IsNullOrEmpty(app.BrmUserName))
            {
                app.BrmUserName = _config.GetValue<string>("BrmUser")!;
            }

            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                if (_brmService.session == null)
                {
                    _brmService.SetUserSession(app.BrmUserName);
                }
                _brmService.SetUserOffice(app.OfficeId);
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

        }


    }
}
