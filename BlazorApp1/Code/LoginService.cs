using BlazorApp1.HttpClients;
using BlazorApp1.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorApp1.Code;

public class LoginService
{
    private const string AccessToken = nameof(AccessToken);
    private const string RefreshToken = nameof(RefreshToken);

    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;
    private readonly IConfiguration _configuration;
    private readonly IBackendApiHttpClient _backendApiHttpClient;

    public LoginService(ILocalStorageService localStorage, NavigationManager navigation, IConfiguration configuration, IBackendApiHttpClient backendApiHttpClient)
    {
        _localStorage = localStorage;
        _navigation = navigation;
        _configuration = configuration;
        _backendApiHttpClient = backendApiHttpClient;
    }

    public async Task<bool> LoginAsync(LoginModel model)
    {
        var response = await _backendApiHttpClient.LoginUserAsync(model);
        if (string.IsNullOrEmpty(response?.Result?.JwtToken))
            return false;

        await _localStorage.SetItemAsync(AccessToken, response.Result.JwtToken);
        await _localStorage.SetItemAsync(RefreshToken, response.Result.RefreshToken);

        return true;
    }


    public async Task<List<Claim>> GetLoginInfoAsync()
    {
        var emptyResult = new List<Claim>();
        ProtectedBrowserStorageResult<string> accessToken;
        ProtectedBrowserStorageResult<string> refreshToken;
        try
        {
            accessToken = await _localStorage.GetItemAsync<ProtectedBrowserStorageResult<string>>(AccessToken);
            refreshToken = await _localStorage.GetItemAsync<ProtectedBrowserStorageResult<string>>(RefreshToken);
        }
        catch (CryptographicException)
        {
            await LogoutAsync();
            return emptyResult;
        }

        if (accessToken.Success is false || accessToken.Value == default)
            return emptyResult;

        var claims = JwtTokenHelper.ValidateDecodeToken(accessToken.Value, _configuration);

        if (claims.Count != 0)
            return claims;

        if (refreshToken.Value != default)
        {
            var response = await _backendApiHttpClient.RefreshTokenAsync(refreshToken.Value);
            if (string.IsNullOrWhiteSpace(response?.Result?.JwtToken) is false)
            {
                await _localStorage.SetItemAsync(AccessToken, response.Result.JwtToken);
                await _localStorage.SetItemAsync(RefreshToken, response.Result.RefreshToken);
                claims = JwtTokenHelper.ValidateDecodeToken(response.Result.JwtToken, _configuration);
                return claims;
            }
            else
            {
                await LogoutAsync();
            }
        }
        else
        {
            await LogoutAsync();
        }
        return claims;
    }

    public async Task LogoutAsync()
    {
        await RemoveAuthDataFromStorageAsync();
        _navigation.NavigateTo("/", true);
    }

    private async Task RemoveAuthDataFromStorageAsync()
    {
        await _localStorage.RemoveItemAsync(AccessToken);
        await _localStorage.RemoveItemAsync(RefreshToken);
    }
}