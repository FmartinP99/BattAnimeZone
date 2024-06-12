using BattAnimeZone.Authentication;
using BattAnimeZone.Components;
using BattAnimeZone.DatabaseInitializer;
using BattAnimeZone.DbContexts;
using BattAnimeZone.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(o => 
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( o=>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(JwtAuthenticationManager.JWT_SECURITY_KEY)),
        ValidateIssuer = false,
        ValidateAudience = false,

    };
});
//dont read/write from database, yet
builder.Services.AddSingleton<AnimeService>();
builder.Services.AddSingleton<UserAccountService>();


//databasecontexts
//builder.Services.AddDbContext<AnimeDbContext>(options =>
  //options.UseSqlite(builder.Configuration.GetConnectionString("AnimeDatabase")));
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


using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    if (serviceScope == null) return;
    var contextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<AnimeDbContext>>();
	var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();

	var dbInitializer = serviceScope.ServiceProvider.GetRequiredService<DbInitializer>();
    dbInitializer.Initialize(configuration, contextFactory);
}


var animeService = app.Services.GetRequiredService<AnimeService>();
await animeService.Initialize();

app.MapControllers();

GC.Collect();
app.Run();
