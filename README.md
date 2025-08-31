# 📒 TelefonRehberi (ASP.NET Core MVC)

Kurum içi telefon rehberi uygulaması. Kişi ve departman yönetimi, arama/filtreleme, sayfalama ve Excel’e aktarma özellikleri içerir. **.NET 8 + EF Core (Code-First, Migrations) + SQLite** ile geliştirilmiştir.


---

## ✨ Özellikler

- 👤 **Kişi Yönetimi**: Ekle / Güncelle / Sil / Listele  
- 🏢 **Departman Yönetimi**: Ekle / Sil / Listele  
- 🔎 **Arama & Filtreleme**: Ad-soyad, e-posta ve departman bazlı filtreleme  
- 📄 **Sayfalama**: Uzun listelerde hızlı gezinme  
- 📤 **Excel’e Aktarma**: Listelenen kişileri tek tıkla Excel’e aktar (ClosedXML)  
- 🔐 **Giriş/Çıkış (Cookie Auth)**: Basit kimlik doğrulama ve oturum yönetimi  
- 🎨 **Responsive Arayüz**: Bootstrap 5

---

## 🧰 Teknolojiler

- **.NET 8**, **ASP.NET Core MVC**
- **Entity Framework Core 8** (Code-First, Migrations)
- **SQLite** (varsayılan) / SQL Server (opsiyonel)
- **ClosedXML** (Excel export)
- **Bootstrap 5**

---

## 📂 Proje Yapısı

```
TelefonRehberi/
| **Controllers/**                     | MVC Controller katmanı |
| ├── AuthController.cs                 | Kimlik doğrulama (Login/Logout) |
| ├── DepartmanController.cs            | Departman CRUD işlemleri |
| └── KisilerController.cs              | Kişi CRUD işlemleri |
| **Data/**                            | Veritabanı bağlamı |
| └── ApplicationDbContext.cs           | EF Core DbContext |
| **Migrations/**                       | EF Core migration dosyaları |
| **Models/**                           | Veri modelleri |
| ├── Departman.cs                      | Departman modeli |
| ├── ErrorViewModel.cs                 | Hata modeli |
| └── Kisi.cs                           | Kişi modeli |
| **Views/**                            | Razor View’lar |
| ├── Auth/                             | Giriş-çıkış sayfaları |
| ├── Departman/                        | Departman yönetim sayfaları |
| ├── Kisiler/                          | Kişi yönetim sayfaları |
| ├── Shared/                           | Ortak layout ve partial view’lar |
| ├── _ViewImports.cshtml               | Razor view import ayarları |
| └── _ViewStart.cshtml                 | Razor view başlangıç ayarları |
| **wwwroot/**                          | Statik dosyalar (css, js, bootstrap) |
| **Properties/**                       | Proje yapılandırma dosyaları |
| Program.cs                            | Uygulama başlangıç noktası |
| appsettings.Development.json          | Konfigürasyon dosyası (connection string vs.) |
| TelefonRehberi.csproj                 | Proje dosyası |
```
