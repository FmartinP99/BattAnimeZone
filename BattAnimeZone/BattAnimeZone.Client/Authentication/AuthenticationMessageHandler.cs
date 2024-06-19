using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BattAnimeZone.Client.Authentication
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationMessageHandler(CustomAuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _authenticationStateProvider.AuthenticationStateChecker();
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
