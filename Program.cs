using TelefonRehberi.Data;               // DbContext
using Microsoft.EntityFrameworkCore;      // UseSqlite
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using TelefonRehberi.Models;
var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Db
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "telefonrehberi.db");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));


// 🔐 Cookie Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath        = "/Auth/Login";   // Girişe yönlendir
        opt.LogoutPath       = "/Auth/Logout";
        opt.AccessDeniedPath = "/Auth/Denied";
        opt.ExpireTimeSpan   = TimeSpan.FromDays(7);
        opt.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // 👈 mutlaka Authorization'dan önce
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Kisiler}/{action=Index}/{id?}");


void SeedAdmin(ApplicationDbContext context)
{
    if (!context.Kisiler.Any())
    {
        var admin = new Kisi
        {
            Ad = "Admin",
            Soyad = "User",
            Email = "admin@firma.com",
            Telefon = "0000000000",
            Departman = "Yönetim"
        };

        var hasher = new PasswordHasher<Kisi>();
        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

        context.Kisiler.Add(admin);
        context.SaveChanges();
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedAdmin(db);
}

app.Run();
