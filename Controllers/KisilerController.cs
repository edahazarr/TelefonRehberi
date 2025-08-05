using Microsoft.AspNetCore.Mvc;
using TelefonRehberi.Models;
using TelefonRehberi.Data;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClosedXML.Excel;
using System.IO;


namespace TelefonRehberi.Controllers
{
    public class KisilerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KisilerController(ApplicationDbContext context)
        // ApplicationDbContext nesnesini dependency injection ile alır ve veritabanı işlemleri için _context değişkenine atar.
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

    ViewBag.Departmanlar = new SelectList(
        _context.Kisiler
            .Where(k => k.Departman != null)
            .Select(k => k.Departman)
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

        // POST: Create
        [HttpPost]
        public IActionResult Create(Kisi kisi)
        {
            if (ModelState.IsValid)//formdan gelen verilerin Kisi modelindeki kurallara uyup uymadığını kontrol eder.
            {
                _context.Add(kisi);
                _context.SaveChanges();
                TempData["Mesaj"] = $"✅ {kisi.Ad} {kisi.Soyad} kişisi başarıyla eklendi.";
                return RedirectToAction(nameof(Index)); // kullanıcı ekleme,silme gibi işlem yaptıktan sonra başka sayfaya yönlendirilir.
            }

            ViewBag.Departmanlar = new SelectList(_context.Departmanlar.ToList(), "Ad", "Ad");
            return View(kisi);
        }
        public IActionResult ExportToExcel(string? adSoyadAra, string? emailAra, string? departmanAra)
{
    var kisiler = _context.Kisiler.AsQueryable();

    if (!string.IsNullOrEmpty(adSoyadAra))
        kisiler = kisiler.Where(k => k.Ad.Contains(adSoyadAra) || k.Soyad.Contains(adSoyadAra));

    if (!string.IsNullOrEmpty(emailAra))
        kisiler = kisiler.Where(k => k.Email.Contains(emailAra));

    if (!string.IsNullOrEmpty(departmanAra))
        kisiler = kisiler.Where(k => k.Departman.Contains(departmanAra));

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



        public IActionResult Edit(int id) // id birincil anahtar değeri
        {
            var kisi = _context.Kisiler.Find(id); // birincil anahtara göre kişi bulunur.
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
