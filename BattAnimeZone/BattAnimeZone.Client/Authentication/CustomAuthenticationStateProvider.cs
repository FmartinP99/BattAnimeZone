﻿using BattAnimeZone.Client.Extensions;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
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
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        

        private  static Timer _timer;

        private AuthenticationState previousState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public CustomAuthenticationStateProvider( ILocalStorageService localStorage,HttpClient _httpClient, NavigationManager _navManager)
        {
            _localStorage = localStorage;
            httpClient = _httpClient;
            navManager = _navManager;

            
            //timer hack to check if the authentication state changed, and if it did it'd force-refresh the current page to update the UI
            if(_timer == null)
            _timer = new Timer(CheckAuthenticationState, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            
        }

        private async void CheckAuthenticationState(object state)
        {
            await AuthenticationStateChecker();
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
                    if (DateTime.Now.ToUniversalTime() > userSession.RefreshTokenExpiryTimestamp)
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

            previousState = newState;
            bool areEqual = await AreClaimsEqual(previousState.User, newState.User);
            if (!areEqual)
            {
                navManager.NavigateTo("anime/52701", forceLoad: true);
                previousState = newState;
            }


        }

        private async Task<bool> AreClaimsEqual (ClaimsPrincipal previousState, ClaimsPrincipal newState) {
            
            if (previousState.Claims.Count() != newState.Claims.Count()) {
                return false;
            }

            var orderedPreviousClaims = previousState.Claims.OrderBy(c => c.Type).ThenBy(c => c.Value).ToList();
            var orderedNewClaims = newState.Claims.OrderBy(c => c.Type).ThenBy(c => c.Value).ToList();

            for (int i = 0; i < orderedPreviousClaims.Count; i++)
            {
                var previousClaim = orderedPreviousClaims[i];
                var newClaim = orderedNewClaims[i];
               

                if (previousClaim.Type != newClaim.Type || previousClaim.Value != newClaim.Value || previousClaim.Issuer != newClaim.Issuer)
                {
                    return false;
                }
            }
            return true;

            
        }

    }
}
