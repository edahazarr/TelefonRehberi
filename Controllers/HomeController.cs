using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TelefonRehberi.Models;
using TelefonRehberi.Data;


namespace TelefonRehberi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // ✅ Ekledik

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) // ✅ context parametresi eklendi
        {
            _logger = logger;
            _context = context; // ✅ DI'dan gelen context burada atanıyor
        }

        public IActionResult Index()
        {
             var kisiler = _context.Kisiler.ToList();
            return View(kisiler);
        
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
