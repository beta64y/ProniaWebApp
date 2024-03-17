using Microsoft.EntityFrameworkCore;
using System;
using ProniaWebApp.Contexts;
using ProniaWebApp.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProniaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ProniaDbContext>();


var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );


app.Run();
