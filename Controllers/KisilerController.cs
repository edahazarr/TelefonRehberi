using Microsoft.AspNetCore.Mvc;
using TelefonRehberi.Models;
using TelefonRehberi.Data;
using X.PagedList;  // Sayfalama (ToPagedList) için gerekli kütüphane

namespace TelefonRehberi.Controllers
{
    public class KisilerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KisilerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kişi listesini getir (sayfalama + arama destekli)
        public IActionResult Index(string arama, int sayfa = 1) 
        // IActionResult, farklı türde yanıtlar döndürebilmemizi sağlar (View, Redirect, Json, vs.)
{
    var kisiler = from k in _context.Kisiler // Kisiler tablosundaki tüm kişileri seçer ve kisiler adlı sorgu değişkenine atar.
                  select k;

    if (!string.IsNullOrEmpty(arama))//arama kutusu boş mu kontrolü
    {
        arama = arama.ToLower();//büyük/küçük harf farkı ortadan kaldırılır.girilen arama küçük harfe çevrilir.
        kisiler = kisiler.Where(k => //kişiler sorgusuna filtre eklenir.belli alanlar seçilir;
            k.Ad.ToLower().Contains(arama) ||
            k.Soyad.ToLower().Contains(arama) ||
            k.Telefon.ToLower().Contains(arama) ||
            k.Email.ToLower().Contains(arama) ||
            k.Departman.ToLower().Contains(arama));
    }

    int sayfaBoyutu = 10; //sayfadaki max kayıt sayısı
    var sayfalananKisiler = kisiler.OrderBy(k => k.Id).ToPagedList(sayfa, sayfaBoyutu);
//kisiler Id'e göre sıralanır.
    return View(sayfalananKisiler);
}

        // Kişi ekleme formu (GET)
        public IActionResult Create()
        {
            ViewBag.Departmanlar = new List<string>
            {
                "Yazılım Geliştirme",
                "Sistem ve Ağ Yönetimi",
                "Bilgi Güvenliği",
                "Veri Tabanı Yönetimi",
                "Proje Yönetimi",
                "Teknik Destek",
                "Satış ve Pazarlama",
                "İnsan Kaynakları",
                "Finans ve Muhasebe",
                "İdari İşler",
                "Ürün Yönetimi",
                "Kalite ve Test",
                "Müşteri Hizmetleri",
                "Eğitim ve Dökümantasyon",
                "Donanım Destek",
                "İç Denetim",
                "Ar-Ge (Araştırma ve Geliştirme)"
            };
            return View();
        }

        // Kişi ekleme işlemi (POST)
      [HttpPost]
public IActionResult Create(Kisi kisi)
{
      if (ModelState.IsValid) //Gerekli alanlar dolu mu, geçerli mi kontrol edilir (model validation).
    {
        _context.Add(kisi);
        _context.SaveChanges();
        TempData["Mesaj"] = $"✅ {kisi.Ad} {kisi.Soyad} kişisi başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    // Eğer form hatalıysa departman listesini tekrar ver
    ViewBag.Departmanlar = new List<string>
    {
        "Yazılım Geliştirme",
        "Sistem ve Ağ Yönetimi",
        "Bilgi Güvenliği",
        "Veri Tabanı Yönetimi",
        "Proje Yönetimi",
        "Teknik Destek",
        "Satış ve Pazarlama",
        "İnsan Kaynakları",
        "Finans ve Muhasebe",
        "İdari İşler",
        "Ürün Yönetimi",
        "Kalite ve Test",
        "Müşteri Hizmetleri",
        "Eğitim ve Dökümantasyon",
        "Donanım Destek",
        "İç Denetim",
        "Ar-Ge (Araştırma ve Geliştirme)"
    };
    return View(kisi);
}

        // Güncelleme formunu getir (GET)
        public IActionResult Edit(int id)
        {
            var kisi = _context.Kisiler.Find(id);
            if (kisi == null)
                return NotFound();

            return View(kisi);
        }

        // Güncelleme işlemi (POST)
        [HttpPost]
        public IActionResult Edit(Kisi kisi)
        {
      if (ModelState.IsValid)//burada veri doğrulaması yapılıyor.
    {
        _context.Kisiler.Update(kisi);
        _context.SaveChanges();
        TempData["Mesaj"] = $"✏️ {kisi.Ad} {kisi.Soyad} kişisi başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    return View(kisi);
        }

        // Silme onay sayfası (GET)
        public IActionResult Delete(int id)
        {
            var kisi = _context.Kisiler.Find(id);
            if (kisi == null)
                return NotFound();

            return View(kisi);
        }

        // Silme işlemi (POST)
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
