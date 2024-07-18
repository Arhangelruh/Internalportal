using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Core.Services;
using InternalPortal.Infrastructure.LDAP.Interfaces;
using InternalPortal.Infrastructure.LDAP.Services;
using InternalPortal.Infrastucture.Data.Context;
using InternalPortal.Infrastucture.Data.Repository;
using InternalPortal.Web.Constants;
using InternalPortal.Web.Interfaces;
using InternalPortal.Web.Models;
using InternalPortal.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("InternalPortalDatabase");
string filesDirectory = builder.Configuration.GetSection("FilesPath:files").Value;
var Logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

builder.Services.AddDbContext<InternalPortalContext>(options =>
 options.UseNpgsql(connection));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ILDAPUserService), typeof(LDAPUserService));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(typeof(ISignInManager), typeof(SignInManager));
builder.Services.AddScoped(typeof(IProfileService), typeof(ProfileService));
builder.Services.AddScoped(typeof(ITestTopicService), typeof(TestTopicService));
builder.Services.AddScoped(typeof(ITestQuestionService), typeof(TestQuestionService));
builder.Services.AddScoped(typeof(ITestAnswerService), typeof(TestAnswerService));
builder.Services.AddScoped(typeof(ITestScoreService), typeof(TestScoreService));
builder.Services.AddScoped(typeof(ITestService), typeof(TestService));
builder.Services.AddScoped(typeof(ICashTestService), typeof(CashTestService));
builder.Services.AddScoped(typeof(IUploadFileService), typeof(UploadFileService));

var physicalProvider = new PhysicalFileProvider(filesDirectory);
builder.Services.AddSingleton<IFileProvider>(physicalProvider);

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


builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions
        .AddPageApplicationModelConvention("/Education",
            model =>
            {
                model.Filters.Add(
                    new GenerateAntiforgeryTokenCookieAttribute());
                model.Filters.Add(
                    new DisableFormValueModelBindingAttribute());
            });
});

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
            .Append($"CN={builder.Configuration.GetSection($"AD:{UserConstants.ManagerRole}").Value},")
            .Append($"CN=Users,{c.LDAPQueryBase}")
            .ToString();
        c.Managers = new StringBuilder()
            .Append($"CN={builder.Configuration.GetSection($"AD:{UserConstants.ManagerRole}").Value},")
            .ToString();
    }
);

builder.Services.Configure<ConfigurationTest>(
    c =>
    {
        try { c.Repeat = builder.Configuration.GetSection("Test:repeat").Get<int>(); } catch { c.Repeat = ConfigurationConstant.repeat; }
    });

builder.Services.Configure<ConfigurationFiles>(
    c =>
    {
        c.Files = filesDirectory;
        c.FileSizeLimit = builder.Configuration.GetSection("FilesPath:sizefile").Get<long>();
    });

builder.Host.UseNLog();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
