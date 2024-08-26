﻿using Microsoft.AspNetCore.Http;
using Sassa.Brm.Common.Helpers;
using Sassa.Brm.Common.Models;
using Sassa.BRM.Models;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;
using System.Security.Principal;

namespace Sassa.Brm.Common.Services;

public class SessionService(StaticService _staticservice)
{
    private UserSession? _session;
    public UserSession? session {
        get
        {
            if (_session == null)
            {
                _session = GetUserSessionFromLDAP(); 
            }
            return _session;
        } 
    }
    public event EventHandler? UserOfficeChanged;

    public UserSession? GetUserSessionFromLDAP()
    {
        //get the user from the environment
        string? userName = Environment.UserName;
        if (string.IsNullOrEmpty(userName)) return null;
        // set up domain context
        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
        // find the user
        UserPrincipal user = UserPrincipal.FindByIdentity(ctx, userName);

        //Set the user session values
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
    //public UserSession? GetUserSession(WindowsIdentity identity)
    //{
    //    //S-1-5-21-1204054820-1125754781-535949388-513 (test value)
    //    _session = new UserSession();

    //    DirectoryEntry user = new DirectoryEntry($"LDAP://<SID={identity.User!.Value}>");
    //    _session.SamName = (string)user.Properties["SAMAccountName"].Value!;
    //    _session.Roles = identity.GetRoles();
    //    _session.Email = (string)user.Properties["mail"].Value!;
    //    _session.Name = (string)user.Properties["name"].Value!;
    //    _session.Surname = (string)user.Properties["sn"].Value!;
    //    _session!.Office = _staticservice.GetUserLocalOffice( _session).Result;
    //    UserOfficeChanged?.Invoke(this, EventArgs.Empty);
    //    return _session;
    //}
}
