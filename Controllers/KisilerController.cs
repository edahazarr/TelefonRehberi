using Microsoft.AspNetCore.Mvc;
using TelefonRehberi.Models;
using TelefonRehberi.Data;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TelefonRehberi.Controllers
{
    public class KisilerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KisilerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string adSoyadAra, string emailAra, string departmanAra, int sayfa = 1)
        {
            var kisiler = _context.Kisiler.AsQueryable();

            if (!string.IsNullOrEmpty(adSoyadAra))
            {
                var aranan = adSoyadAra.ToLower();
                kisiler = kisiler.Where(k =>
                    k.Ad.ToLower().Contains(aranan) ||
                    k.Soyad.ToLower().Contains(aranan));
            }

            if (!string.IsNullOrEmpty(emailAra))
            {
                var aranan = emailAra.ToLower();
                kisiler = kisiler.Where(k => k.Email.ToLower().Contains(aranan));
            }

            if (!string.IsNullOrEmpty(departmanAra))
            {
                var aranan = departmanAra.ToLower();
                kisiler = kisiler.Where(k => k.Departman.ToLower().Contains(aranan));
            }

            int sayfaBoyutu = 10;
            var sayfalananKisiler = kisiler.OrderBy(k => k.Id).ToPagedList(sayfa, sayfaBoyutu);
            return View(sayfalananKisiler);
        }

        // GET: Create
        public IActionResult Create()
        {
            var departmanlar = _context.Departmanlar.ToList();
            ViewBag.Departmanlar = new SelectList(departmanlar, "Ad", "Ad");
            return View();
        }

        // POST: Create
        [HttpPost]
        public IActionResult Create(Kisi kisi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kisi);
                _context.SaveChanges();
                TempData["Mesaj"] = $"✅ {kisi.Ad} {kisi.Soyad} kişisi başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departmanlar = new SelectList(_context.Departmanlar.ToList(), "Ad", "Ad");
            return View(kisi);
        }

        public IActionResult Edit(int id)
        {
            var kisi = _context.Kisiler.Find(id);
            if (kisi == null)
                return NotFound();

            ViewBag.Departmanlar = new SelectList(_context.Departmanlar.ToList(), "Ad", "Ad");
            return View(kisi);
        }

        [HttpPost]
        public IActionResult Edit(Kisi kisi)
        {
            if (ModelState.IsValid)
            {
                _context.Kisiler.Update(kisi);
                _context.SaveChanges();
                TempData["Mesaj"] = $"✏️ {kisi.Ad} {kisi.Soyad} kişisi başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            ViewBag.Departmanlar = new SelectList(_context.Departmanlar.ToList(), "Ad", "Ad");
            return View(kisi);
        }

        public IActionResult Delete(int id)
        {
            var kisi = _context.Kisiler.Find(id);
            if (kisi == null)
                return NotFound();

            return View(kisi);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var kisi = _context.Kisiler.Find(id);
            if (kisi != null)
            {
                _context.Kisiler.Remove(kisi);
                _context.SaveChanges();
                TempData["Mesaj"] = $"🗑️ {kisi.Ad} {kisi.Soyad} kişisi başarıyla silindi.";
            }

            return RedirectToAction("Index");
        }
    }
}
