using Sassa.Brm.Common.Models;
using System.DirectoryServices.AccountManagement;

namespace Sassa.Brm.Common.Services;

public class SessionService(StaticService _staticservice)
{
    private UserSession _session = new UserSession("", "", "", ""); 
    public UserSession session {
        get
        {
            if (!_session.IsLoggedIn())
            {
                _session = GetUserSessionFromLDAP(); 
            }
            return _session;
        } 
    }
    public event EventHandler? UserOfficeChanged;

    public UserSession GetUserSessionFromLDAP()
    {
        // get the user from the environment
        string? userName = Environment.UserName;
        if (string.IsNullOrEmpty(userName)) return _session;
        // set up domain context
        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
        // find the user
        UserPrincipal user = UserPrincipal.FindByIdentity(ctx, userName);
        // set user session values
        _session = new UserSession(user.Name, user.Surname, user.SamAccountName, user.EmailAddress);
        _session.Roles = user.GetGroups().Select(x => x.Name).ToList();

        UpdateUserOffice();

        return _session;
    }

    public void UpdateUserOffice()
    {
        _session!.Office = _staticservice.GetUserLocalOffice(_session.SamName);
        //Trigger the change for UI update
        UserOfficeChanged?.Invoke(this, EventArgs.Empty);
    }

}
