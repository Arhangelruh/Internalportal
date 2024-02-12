using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Services;
using InternalPortal.Infrastructure.LDAP.Interfaces;
using InternalPortal.Infrastructure.LDAP.Services;
using InternalPortal.Infrastucture.Data.Context;
using InternalPortal.Infrastucture.Data.Repository;
using InternalPortal.Web.Interfaces;
using InternalPortal.Web.Models;
using InternalPortal.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("InternalPortalDatabase");
builder.Services.AddDbContext<InternalPortalContext>(options =>
 options.UseNpgsql(connection));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ILDAPUserService), typeof(LDAPUserService));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(typeof(ISignInManager), typeof(SignInManager));
builder.Services.AddScoped(typeof(IProfileService), typeof(ProfileService));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(
            options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(11);                
                options.SlidingExpiration = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/access-denied";
            }
        );

//builder.Services.AddAuthorization(options =>
//{    
//    options.FallbackPolicy = options.DefaultPolicy;
//});
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddRazorPages();

builder.Services.Configure<ConfigurationAD>(
    c =>
    {
        c.Port = builder.Configuration.GetSection("AD:port").Get<int>();
        c.Zone = builder.Configuration.GetSection("AD:zone").Value;
        c.Domain = builder.Configuration.GetSection("AD:domain").Value;
        c.Subdomain = builder.Configuration.GetSection("AD:subdomain").Value;
        c.Username = builder.Configuration.GetSection("AD:username").Value;
        c.Password = builder.Configuration.GetSection("AD:password").Value;
        c.LDAPserver = $"{c.Subdomain}.{c.Domain}.{c.Zone}";
        c.LDAPQueryBase = $"DC={c.Domain},DC={c.Zone}";
        c.Users = new StringBuilder()
            .Append($"CN={builder.Configuration.GetSection("AD:users").Value},")
            .Append($"CN=Users,{c.LDAPQueryBase}")
            .ToString();
        c.Managers = new StringBuilder()
            .Append($"CN={builder.Configuration.GetSection("AD:managers").Value},")            
            .ToString();
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
