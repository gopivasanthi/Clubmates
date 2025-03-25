using Clubmates.Web.AppDbContext;
using Clubmates.Web.Areas.Club.Services;
using Clubmates.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Blazor;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ClubmatesUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    options.Lockout = new LockoutOptions
    {
        AllowedForNewUsers = true,
        DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
        MaxFailedAccessAttempts = 5
    };
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IClubLayoutService, ClubLayoutService>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
   options
   .UseSqlServer(
        builder
        .Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
}).AddCookie();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("MustbeASuperAdmin", policy
                                        => policy
                                        .RequireClaim(ClaimTypes.Role, "SuperAdmin")
                                        .RequireClaim(ClaimTypes.Role, "Guest"))
    .AddPolicy("MustbeAGuest", policy => policy.RequireClaim(ClaimTypes.Role, "Guest"));


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Club",
    pattern: "Club/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
