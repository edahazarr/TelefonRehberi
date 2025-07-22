using Microsoft.EntityFrameworkCore;
using TelefonRehberi.Models;

namespace TelefonRehberi.Data
{
    public class ApplicationDbContext : DbContext //DbContext sınıfından türemiştir.
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) //DbContext’e özel ayar içeren parametre
            : base(options)
        {
        }

        public DbSet<Kisi> Kisiler { get; set; }
    }
}
/*options içinde veritabanı türü, bağlantı bilgisi vb. yer alır.
base(options): Bu ayarları DbContext taban sınıfına aktarır.*/