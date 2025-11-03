using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repositories.Config;

namespace WebApi.Repositories
{
    //"Benim 'Books' adında bir tablom olacak ve bu tablo Book nesnelerinden oluşacak"
    /*
    İş Akışı Nasıl Oluyor?
    Bir Controller'ınız var ve veritabanına erişmesi gerekiyor.

    ASP.NET Core, Controller'a bir RepositoryContext(Depo müüdürü) nesnesi vermesi/ataması gerektiğini anlıyor.

    Hemen Program.cs'deki talimatlara bakıyor.

    İçinde UseSqlServer ve bağlantı cümlenizin olduğu bir DbContextOptions nesnesi yaratıyor.

    Sonra sizin yazdığınız o constructor'ı çağırıyor: new RepositoryContext(yeni_olusturulan_options).

    RepositoryContext bu options'ı alıp base'e (yani ana DbContext'e) iletiyor.

    Sonuç: Elinizde, doğru veritabanına bağlanacak şekilde tamamen yapılandırılmış, kullanıma hazır bir RepositoryContext nesnesi oluyor.*/
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) 
            : base(options) { }
        public DbSet <Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }
    }
}
