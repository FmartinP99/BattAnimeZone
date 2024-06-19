using BattAnimeZone.Client.Extensions;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.AspNetCore.Components;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http.Headers;
using Microsoft.JSInterop;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using BlazorBootstrap;
using Newtonsoft.Json.Linq;
using BattAnimeZone.Shared.Models.User;


namespace BattAnimeZone.Client.Authentication
{
    public partial class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        [Inject]
        private HttpClient httpClient { get; set; }
        [Inject]
        private NavigationManager navManager { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private readonly ILocalStorageService _localStorage;
        private readonly ISessionStorageService _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        private readonly Timer _timer;

        public CustomAuthenticationStateProvider( ILocalStorageService localStorage,
            ISessionStorageService sessionStorage, HttpClient _httpClient, NavigationManager _navManager, IJSRuntime _JSRuntime)
        {
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;
            httpClient = _httpClient;
            JSRuntime = _JSRuntime;
            navManager = _navManager;

            _timer = new Timer(CheckAuthenticationState, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private async void CheckAuthenticationState(object state)
        {
            await GetAuthenticationStateAsync();
        }



        public async Task<RefreshTokenDTO?> RefreshAuthenticationStateAsync(UserSession userSession)
        {
            
                try
                {
                    RefreshTokenDTO rtdto = new RefreshTokenDTO
                    {
                        Token = userSession.Token,
                        RefreshToken = userSession.RefreshToken
                    };

                    var response = await httpClient.PostAsJsonAsync<RefreshTokenDTO>($"{navManager.BaseUri}api/AccountController/Refresh", rtdto);
                    if (response.IsSuccessStatusCode)
                    {
                        var newRtdto = await response.Content.ReadFromJsonAsync<RefreshTokenDTO>();
                       

                        return newRtdto;
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("console.error", $"{response.StatusCode}\n {response.ReasonPhrase}");
                        return null;
                    }

                }
                catch (Exception ex)
                {
                await JSRuntime.InvokeVoidAsync("console.error", $"{ex.Message}");
                return null;

                }


            
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _localStorage.ReadEncryptedItemAsync<UserSession>("UserSession");

                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }
                if (DateTime.Now.ToUniversalTime() > userSession.TokenExpiryTimeStamp)
                {
                    if (DateTime.Now.ToUniversalTime() < userSession.RefreshTokenExpiryTimestamp.AddSeconds(-110))
                    {
                        await _localStorage.RemoveItemAsync("UserSession");
                        await _localStorage.RemoveItemAsync("InteractedAnimes");
                        return await Task.FromResult(new AuthenticationState(_anonymous));
                    }
                    else
                    {
                        RefreshTokenDTO? result = await RefreshAuthenticationStateAsync(userSession);

                        if (result == null)
                        {

                            await _localStorage.RemoveItemAsync("UserSession");
                            await _localStorage.RemoveItemAsync("InteractedAnimes");
                            httpClient.DefaultRequestHeaders.Authorization = null;
                            return await Task.FromResult(new AuthenticationState(_anonymous));
                        }
                        else
                        {
                            userSession.Token = result.Token;
                            userSession.RefreshToken = result.RefreshToken;
                            userSession.TokenExpiryTimeStamp = result.TokenExpiryTimeStamp;
                            userSession.RefreshTokenExpiryTimestamp = result.RefreshTokenExpiryTimeStamp;
                            await _localStorage.SaveItemEncryptedAsync("UserSession", userSession);
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);
                        }

                    }
                }
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "JwtAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "JwtAuth"));
                userSession.TokenExpiryTimeStamp = DateTime.Now.ToUniversalTime().AddSeconds(userSession.ExpiresIn);
                await _localStorage.SaveItemEncryptedAsync("UserSession", userSession);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);

                try
                {
                    var response = await httpClient.GetAsync($"{navManager.BaseUri}api/AccountController/GetInteractedAnimes/{userSession.UserName}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var interactedAnimes = await response.Content.ReadFromJsonAsync<Dictionary<int, InteractedAnime>>();
                        await _localStorage.SaveItemEncryptedAsync("InteractedAnimes", interactedAnimes);
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("console.error", $"{response.StatusCode}\n {response.ReasonPhrase}");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("console.error", $"{ex.Message}");
                    httpClient.DefaultRequestHeaders.Authorization = null;
                }
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _localStorage.RemoveItemAsync("UserSession");
                await _localStorage.RemoveItemAsync("InteractedAnimes");
                httpClient.DefaultRequestHeaders.Authorization = null;

            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task AuthenticationStateChecker()
        {
            var newState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(newState));
        }

    }
}
