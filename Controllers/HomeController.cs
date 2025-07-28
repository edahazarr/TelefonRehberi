using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TelefonRehberi.Models;
using TelefonRehberi.Data;
using  X.PagedList;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace TelefonRehberi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

       public IActionResult Index(string adSoyadAra, string emailAra, string departmanAra, int sayfa = 1)
{
    var kisiler = _context.Kisiler.AsQueryable();

    // Ad veya Soyad araması
    if (!string.IsNullOrEmpty(adSoyadAra))
    {
        var aranan = adSoyadAra.ToLower();
        kisiler = kisiler.Where(k =>
            (k.Ad + " " + k.Soyad).ToLower().Contains(aranan) || // ad + soyad birlikte arama
            k.Ad.ToLower().Contains(aranan) ||
            k.Soyad.ToLower().Contains(aranan));
    }

    // Email araması
    if (!string.IsNullOrEmpty(emailAra))
    {
        var aranan = emailAra.ToLower();
        kisiler = kisiler.Where(k => k.Email.ToLower().Contains(aranan));
    }

    // Departman araması
  if (!string.IsNullOrEmpty(departmanAra))
{
    var aranan = departmanAra.Trim().ToLower();

    kisiler = kisiler.Where(k =>
        k.Departman != null &&
        k.Departman.ToLower().Contains(aranan));
}

    int sayfaBoyutu = 10;
    var sayfalananKisiler = kisiler.OrderBy(k => k.Id).ToPagedList(sayfa, sayfaBoyutu);
    return View(sayfalananKisiler);
    
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
