using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
    //"Benim 'Books' adında bir tablom olacak ve bu tablo Book nesnelerinden oluşacak"
    //gemini geçmişine bakıp kodun açıklamasını yaz!
    //github a bu projeyi ekle
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) 
            : base(options) { }
        public DbSet <Book> Books { get; set; }
    }
}
