//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Graph.Models.Security;
using Task11_crud_.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<RegisterDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<RegisterDbContext>();
builder.Services.AddHostedService<BackgroundTask>();
//// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
//builder.Services.ConfigureApplicationCookie(o=>o.LoginPath="/Login");

//new code
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           
           options.Cookie.HttpOnly = true;
           options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
           options.LoginPath = "/Login";
           options.SlidingExpiration = true;
       });
//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//new code end
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
//app.UseSession();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
