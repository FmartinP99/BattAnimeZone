using BattAnimeZone.Client.Extensions;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BattAnimeZone.Shared.Models.User.SessionStorageModels;
using Microsoft.AspNetCore.Components;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http.Headers;
using Microsoft.JSInterop;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;


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

        private readonly ISessionStorageService _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage, HttpClient _httpClient, NavigationManager _navManager, IJSRuntime _JSRuntime)
        {
            _sessionStorage = sessionStorage;
            httpClient = _httpClient;
            JSRuntime = _JSRuntime;
            navManager = _navManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                if(DateTime.Now.ToUniversalTime() > userSession.ExpiryTimeStamp)
                    return await Task.FromResult(new AuthenticationState(_anonymous));

                /*
                string jwtTokenString = userSession.Token;
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(jwtTokenString);
                IEnumerable<Claim> claims = jwtToken.Claims;
                var claimsDictionary = claims.ToDictionary(claim => claim.Type, claim => claim.Value);
                */

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
                }));
                userSession.ExpiryTimeStamp = DateTime.Now.ToUniversalTime().AddSeconds(userSession.ExpiresIn);
                await Task.Delay(5000);
                await _sessionStorage.SaveItemEncryptedAsync("UserSession", userSession);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userSession.Token);

                try
                {
                    var response = await httpClient.GetAsync($"{navManager.BaseUri}api/AccountController/GetInteractedAnimes/{userSession.UserName}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var interactedAnimes = await response.Content.ReadFromJsonAsync<Dictionary<int, InteractedAnime>>();
                        await _sessionStorage.SaveItemEncryptedAsync("InteractedAnimes", interactedAnimes);
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
                }
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _sessionStorage.RemoveItemAsync("UserSession");
                await _sessionStorage.RemoveItemAsync("InteractedAnimes");
           
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

    
    }
}
