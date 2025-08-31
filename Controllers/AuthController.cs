using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TelefonRehberi.Data;
using TelefonRehberi.Models;
using Microsoft.AspNetCore.Authorization;

namespace TelefonRehberi.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Kisi> _hasher = new();

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            var user = _context.Kisiler.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı.");
                return View();
            }

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verify == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı.");
                return View();
            }

            // Claims = kullanıcıya ait bilgiler
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Ad} {user.Soyad}"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Oturum aç
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Kisiler");
        }

        // POST: /Auth/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Denied() => Content("Erişim engellendi.");
    }
}
