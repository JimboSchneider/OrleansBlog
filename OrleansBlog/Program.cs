using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orleans.Configuration;
using OrleansBlog.Components;
using OrleansBlog.Components.Account;
using OrleansBlog.Data;
using OrleansBlog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add startup delay in development to ensure Silo is ready
if (builder.Environment.IsDevelopment() && 
    Environment.GetEnvironmentVariable("ORLEANS_STARTUP_DELAY") is { } delayStr && 
    int.TryParse(delayStr, out var delaySeconds))
{
    Console.WriteLine($"Waiting {delaySeconds} seconds for Orleans Silo to start...");
    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
}

builder.Host
	.UseOrleansClient(client =>
	{
		client
			.UseLocalhostClustering(gatewayPort: 30000);
	})
	.ConfigureLogging(logging => logging.AddConsole());

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Use SQLite if no connection string is provided
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "Data Source=orleans-blog.db";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}
// Override to use SQLite explicitly in development environments
else if (builder.Environment.IsDevelopment())
{
    connectionString = "Data Source=orleans-blog.db";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
