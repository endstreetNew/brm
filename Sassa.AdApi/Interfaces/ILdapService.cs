namespace Sassa.AdApi.Interfaces;

public interface ILdapService
{
    Task<bool> Authenticate(string username, string password);
}