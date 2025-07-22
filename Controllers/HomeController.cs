using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TelefonRehberi.Models;

namespace TelefonRehberi.Controllers
{
    public class HomeController : Controller // controller sınıfından türemiştir.
    {
        private readonly ILogger<HomeController> _logger; // homecontroller'a özel loglama(kayıt tutma) yapmak için Ilogger<T>

        public HomeController(ILogger<HomeController> logger) 
        {
            /* HomeController sınıfından bir nesne oluşturulduğunda bu metod çalışır.
            Dependency Injection (bağımlılık enjeksiyonu) yoluyla sağlanır. */
            _logger = logger; //parametre olarak gelen logger nesnesi sınıfın private alanı olan _logger’a atanır.
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy() 
         //Bu bir attribute.ASP.NET Core’da yanıtların cache’lenmesini (önbelleğe alınmasını) kontrol eder.
         //Yanıtın önbelleğe alınmasını tamamen devre dışı bırakır.

        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // sayfayı sıfır saniye bile önbellekte tutmasın, önbelleğe alma, kesinlikle hiçbir yerde saklanmamasını zorunlu kıl.

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });   //Hata sayfasını döndürür, RequestId ile hangi istekte hata olduğunu gösterir.

        }
    }
}
