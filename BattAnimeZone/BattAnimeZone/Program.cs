using BattAnimeZone.Authentication;
using BattAnimeZone.Components;
using BattAnimeZone.DatabaseInitializer;
using BattAnimeZone.DbContexts;
using BattAnimeZone.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var jwtSecurityKey = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(o => 
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", o=>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(jwtSecurityKey)),
        ValidateIssuer = false,
        ValidateAudience = false,

    };
});
//dont read/write from database, yet
//builder.Services.AddSingleton<AnimeService>();
builder.Services.AddSingleton<UserAccountService>();

builder.Services.AddTransient<DataBaseService>();

//this one is responsible for the string similarity searching. It stores all the anime titles in memory so theres no need to query it for every search
builder.Services.AddSingleton<SingletonSearchService>();


//databasecontexts
builder.Services.AddDbContextFactory<AnimeDbContext>((DbContextOptionsBuilder options) => options.UseSqlite(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddTransient<DbInitializer>();


builder.Services.AddScoped<Radzen.DialogService>();
builder.Services.AddScoped<Radzen.TooltipService>();
builder.Services.AddScoped<Radzen.ContextMenuService>();
builder.Services.AddScoped<Radzen.NotificationService>();

builder.Services.AddBlazorBootstrap();


builder.Services.AddHttpClient();


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins", builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BattAnimeZone.Client._Imports).Assembly);


#pragma warning disable CS8602 // Dereference of a possibly null reference.
using (IServiceScope? serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    if (serviceScope == null) return;
    var contextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<AnimeDbContext>>();
	var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();

	var dbInitializer = serviceScope.ServiceProvider.GetRequiredService<DbInitializer>();
    dbInitializer.Initialize(configuration, contextFactory);
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.


//var animeService = app.Services.GetRequiredService<AnimeService>();
//await animeService.Initialize();

app.MapControllers();

GC.Collect();
app.Run();
