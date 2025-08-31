using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TelefonRehberi.Models;
using TelefonRehberi.Data;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClosedXML.Excel;
using System.IO;
using System.Linq;

namespace TelefonRehberi.Controllers
{
    //[Authorize] // Rehber sayfalarını girişe kapat
    public class KisilerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Kisi> _hasher = new(); // Şifreler için

        public KisilerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string adSoyadAra, string emailAra, string departmanAra, int page = 1)
        {
            var kisiler = _context.Kisiler.AsQueryable();

            if (!string.IsNullOrEmpty(adSoyadAra))
            {
                var aranan = adSoyadAra.ToLower();
                kisiler = kisiler.Where(k =>
                    (k.Ad != null && k.Ad.ToLower().Contains(aranan)) ||
                    (k.Soyad != null && k.Soyad.ToLower().Contains(aranan)));
            }

            if (!string.IsNullOrEmpty(emailAra))
            {
                var aranan = emailAra.ToLower();
                kisiler = kisiler.Where(k => k.Email != null && k.Email.ToLower().Contains(aranan));
            }

            if (!string.IsNullOrEmpty(departmanAra))
            {
                var aranan = departmanAra.ToLower();
                kisiler = kisiler.Where(k => k.Departman != null && k.Departman.ToLower().Contains(aranan));
            }

            ViewBag.Departmanlar = new SelectList(
                _context.Kisiler
                    .Where(k => k.Departman != null)
                    .Select(k => k.Departman!)
                    .Distinct()
                    .ToList(),
                departmanAra
            );

            int sayfaBoyutu = 10;
            var sayfalananKisiler = kisiler.OrderBy(k => k.Id).ToPagedList(page, sayfaBoyutu);

            return View(sayfalananKisiler);
        }

        // GET: Create
        public IActionResult Create()
        {
            var departmanlar = _context.Departmanlar.ToList();
            ViewBag.Departmanlar = new SelectList(departmanlar, "Ad", "Ad");
            return View();
        }

        // POST: Create  —> Şifreyi hash’leyip kaydediyoruz
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kisi kisi, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Password", "Şifre alanı zorunludur.");
            }

            if (_context.Kisiler.Any(x => x.Email == kisi.Email))
            {
                ModelState.AddModelError("Email", "Bu e-posta zaten kayıtlı.");
            }

            if (ModelState.IsValid)
            {
                kisi.PasswordHash = _hasher.HashPassword(kisi, password);

                _context.Add(kisi);
                _context.SaveChanges();
                TempData["Mesaj"] = $"✅ {kisi.Ad} {kisi.Soyad} kişisi başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departmanlar = new SelectList(_context.Departmanlar.ToList(), "Ad", "Ad");
            return View(kisi);
        }

        public IActionResult ExportToExcel(string? adSoyadAra, string? emailAra, string? departmanAra)
        {
            var kisiler = _context.Kisiler.AsQueryable();

            if (!string.IsNullOrEmpty(adSoyadAra))
                kisiler = kisiler.Where(k => (k.Ad ?? "").Contains(adSoyadAra) || (k.Soyad ?? "").Contains(adSoyadAra));

            if (!string.IsNullOrEmpty(emailAra))
                kisiler = kisiler.Where(k => (k.Email ?? "").Contains(emailAra));

            if (!string.IsNullOrEmpty(departmanAra))
                kisiler = kisiler.Where(k => (k.Departman ?? "").Contains(departmanAra));

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Kisiler");
                worksheet.Cell(1, 1).Value = "Ad";
                worksheet.Cell(1, 2).Value = "Soyad";
                worksheet.Cell(1, 3).Value = "Telefon";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "Departman";

                int row = 2;
                foreach (var kisi in kisiler)
                {
                    worksheet.Cell(row, 1).Value = kisi.Ad;
                    worksheet.Cell(row, 2).Value = kisi.Soyad;
                    worksheet.Cell(row, 3).Value = kisi.Telefon;
                    worksheet.Cell(row, 4).Value = kisi.Email;
                    worksheet.Cell(row, 5).Value = kisi.Departman;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Kisiler.xlsx");
                }
            }
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var kisi = _context.Kisiler.Find(id);
            if (kisi == null)
                return NotFound();

            ViewBag.Departmanlar = new SelectList(_context.Departmanlar.ToList(), "Ad", "Ad");
            return View(kisi);
        }

        // POST: Edit  —> Şifre girilirse değiştir, girilmezse koru
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Kisi form, string? password)
        {
            var kisi = _context.Kisiler.FirstOrDefault(x => x.Id == form.Id);
            if (kisi == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Departmanlar = new SelectList(_context.Departmanlar.ToList(), "Ad", "Ad");
                return View(form);
            }

            // Alanları güncelle
            kisi.Ad = form.Ad;
            kisi.Soyad = form.Soyad;
            kisi.Telefon = form.Telefon;
            kisi.Email = form.Email;
            kisi.Departman = form.Departman;

            // Şifre değişimi (opsiyonel)
            if (!string.IsNullOrWhiteSpace(password))
            {
                kisi.PasswordHash = _hasher.HashPassword(kisi, password);
            }

            _context.SaveChanges();
            TempData["Mesaj"] = $"✏️ {kisi.Ad} {kisi.Soyad} kişisi başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var kisi = _context.Kisiler.Find(id);
            if (kisi == null)
                return NotFound();

            return View(kisi);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
