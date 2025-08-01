using Microsoft.AspNetCore.Mvc;
using TelefonRehberi.Data;
using TelefonRehberi.Models;
using X.PagedList;
namespace TelefonRehberi.Controllers
{
    public class DepartmanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            int sayfaBoyutu = 10; // Her sayfada 10 departman göster
            var departmanlar = _context.Departmanlar
                .OrderBy(d => d.Ad)
                .ToPagedList(page, sayfaBoyutu);

            return View(departmanlar);
        }
        public IActionResult Create()
{
    return View();
}

[HttpPost]
public IActionResult Create(Departman departman)
{
    if (ModelState.IsValid)
    {
        _context.Departmanlar.Add(departman);
        _context.SaveChanges();
        TempData["Mesaj"] = "Departman eklendi.";
        return RedirectToAction("Index");
    }

    return View(departman);
}
        public IActionResult Delete(int id)
        {
            var departman = _context.Departmanlar.Find(id);
            if (departman != null)
            {
                _context.Departmanlar.Remove(departman);
                _context.SaveChanges();
                TempData["Mesaj"] = "🗑️ Departman silindi.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
