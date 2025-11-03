using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Repositories.Config
{
    // Bu, uzmanın "Ben bir entity yapılandırma uzmanıyım" 
    // diyen kimlik kartıdır (IEntityTypeConfiguration).
    // <Book> ise "Ben SADECE 'Book' rafından sorumluyum" demektir.
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        // Bu metot, uzmanın talimat defteridir.
        // Depo Müdürü, "Kitap rafını kur" dediğinde bu talimatları uygular.
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            // TALİMAT 1: "HasData" (Veri Tohumlama)
            // Bu, uzmanın verdiği en önemli talimattır:
            // "Depo ilk kez açıldığında ve bu rafı SIFIRDAN kurduğumuzda,
            // rafın boş görünmemesi için üzerine BU ÜÇ KİTABI başlangıç verisi olarak yerleştirin."
            builder.HasData(
                new Book { Id = 1, Title = "Karagöz ve Hacivat", Price = 75 },
                new Book { Id = 2, Title = "Mesnevi", Price = 175 },
                new Book { Id = 3, Title = "Devlet", Price = 375 }
            );

            // Gelecekte Depo Müdürü bu uzmana yeni görevler de verebilir. Örn:
            // builder.Property(b => b.Title).IsRequired();
            // (Talimat 2: "Bu rafa 'Başlığı' olmayan bir kitap koymak yasaktır!")
        }
    }
}