using Generator.Areas.Identity.Data;
using Generator.Authorization;
using Generator.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Unless specified otherwise, fallback to require a user to be logged in to view a page.
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// Authorization handlers
builder.Services.AddSingleton<IAuthorizationHandler, AdministratorAuthHandler>();
/*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Identity/Account/login";
        options.LogoutPath = "/Identity/Account/logout";
        options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);
        options.Cookie.MaxAge = new TimeSpan(7, 0, 0, 0);
        options.Cookie.Name = "Generator_Access_Token";
        options.Cookie.HttpOnly = false;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Headers["Location"] = context.RedirectUri;
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    })*/
builder.Services.AddAuthentication()
    .AddDiscord(discordOptions =>
    {
        discordOptions.ClientId = configuration.GetValue<string>("DiscordClientId");
        discordOptions.ClientSecret = configuration.GetValue<string>("DiscordClientSecret");
        discordOptions.Scope.Add("identify");
        discordOptions.SaveTokens = true;
        discordOptions.Prompt = "none";
        discordOptions.SignInScheme = IdentityConstants.ExternalScheme; // This works for external logins instead of CookieAuthenticationDefaults.AuthenticationScheme
        discordOptions.AccessDeniedPath = "/oauthfailed";
        discordOptions.CorrelationCookie.SameSite = SameSiteMode.None; // Note: Lax isn't supported for external cookies
        discordOptions.CorrelationCookie.HttpOnly = false;
    });
builder.Services.AddRazorPages();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    // Set pw with Secret Manager tool
    // dotnet user-secrets set SeedUserPW passwordgoeshere
    var testUserPw = configuration.GetValue<string>("SeedUserPW");
    var adminEmail = configuration.GetValue<string>("AdminEmail");
    var normalEmail = configuration.GetValue<string>("NormalEmail");
    if (testUserPw != null)
    {
        await SeedData.Initialize(services, testUserPw, adminEmail, normalEmail);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
