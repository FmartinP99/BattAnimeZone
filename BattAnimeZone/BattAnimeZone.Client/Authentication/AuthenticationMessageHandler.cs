

using Microsoft.AspNetCore.Components.Authorization;

namespace BattAnimeZone.Client.Authentication
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        public AuthenticationMessageHandler(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
            _customAuthenticationStateProvider = (CustomAuthenticationStateProvider)_authStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //DO SOMETHING BEFORE EVERY HTTPRQUEST SENT
            //await _customAuthenticationStateProvider.AuthenticationStateChecker();
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
