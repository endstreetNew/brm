using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Models;
using Sassa.BRM.Services;
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
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DcFile>> PostUser(Application app)
        {
            DcFile result;
            try
            {
                result  = await  _brmService.CreateBRM(app,"Inserted via API.");
            }
            catch
            {
                return NoContent();
            }
            //var xx = CreatedAtAction("GetUser", new { id = user.Id }, user);
            //return xx;
            return result;
        }
    }
}
