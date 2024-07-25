using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sassa.BRM.Models;
using Sassa.Activity.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Sassa.Activity.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IDbContextFactory<ModelContext> _contextFactory;

        public ActivityController(IDbContextFactory<ModelContext> contextFactory) //ILogger<ActivityController> logger)
        {
            _contextFactory = contextFactory;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostActivity(DcActivity activity)
        {

            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                using (var _context = _contextFactory.CreateDbContext())
                {
                    try
                    {
                        _context.DcActivities.Add(activity);
                        await _context.SaveChangesAsync();
                        response.Success = true;
                    }
                    catch
                    {
                        //just ignoring activity post errors for now.
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return Ok(response);
        }
    }
}
