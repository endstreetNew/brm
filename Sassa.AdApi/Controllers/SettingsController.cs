using Sassa.Brm.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Security.Claims;

namespace BlazorExample.Api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        //AdAuthenticationStateProvider _authenticationStateProvider;
        //public SettingsController(AdAuthenticationStateProvider authenticationStateProvider)
        //{
        //    _authenticationStateProvider = authenticationStateProvider;
        //}
        // GET api/settings/user
        [HttpGet("user")]
        public UserSession GetUser()
        {
            // User not signed in:
            return new UserSession();

            // User signed in:
            //return new AuthorizedUser { Name = "John Doe" };
        }

        //public async void GetUserAD()
        //{
        //    var auth = await authenticationStateProvider.GetAuthenticationStateAsync();
        //    var user = (System.Security.Principal.WindowsPrincipal)auth.User;

        //    //using PrincipalContext pc = new PrincipalContext(ContextType.Domain);
        //    //UserPrincipal up = UserPrincipal.FindByIdentity(pc, user.Identity.Name);

        //    //FirstName = up.GivenName;
        //    //LastName = up.Surname;
        //    //UserEmail = up.EmailAddress;
        //    //LastLogon = up.LastLogon;
        //    //FixPhone = up.VoiceTelephoneNumber;
        //    //UserDisplayName = up.DisplayName;
        //    //JobTitle = up.Description;
        //    //DirectoryEntry directoryEntry = up.GetUnderlyingObject() as DirectoryEntry;
        //    //Department = directoryEntry.Properties["department"]?.Value as string;
        //    //MobilePhone = directoryEntry.Properties["mobile"]?.Value as string;
        //    //MemberOf = directoryEntry.Properties["memberof"]?.OfType<string>()?.ToList();

        //    //if (MemberOf.Any(x => x.Contains("management-team") && x.Contains("OU=Distribution-Groups")))
        //    //{
        //    //    var userClaims = new ClaimsIdentity(new List<Claim>()
        //    //{
        //    //    new Claim(ClaimTypes.Role,"Big-Boss")
        //    //});
        //    //    user.AddIdentity(userClaims);
        //    //}
        //}
    }
}
