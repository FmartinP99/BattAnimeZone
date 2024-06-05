using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(http => new HttpClient
{
	BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();
