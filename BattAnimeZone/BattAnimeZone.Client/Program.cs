using BattAnimeZone.Client.Authentication;
using BattAnimeZone.Client.Services;
using BattAnimeZone.Shared.Policies;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationMessageHandler>();
builder.Services.AddScoped<StorageService>();


builder.Services.AddBlazorBootstrap();


builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
})
.AddHttpMessageHandler<AuthenticationMessageHandler>();


builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);


/*POLICITES */
builder.Services.AddAuthorizationCore(config =>
{
	config.AddPolicy(Policies.IsAuthenticated, Policies.IsAuthenticatedPolicy());
});

await builder.Build().RunAsync();