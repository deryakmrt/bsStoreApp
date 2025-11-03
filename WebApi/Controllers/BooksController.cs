using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;
//Bu dosyanın amacı, BooksController'ın (Sipariş Ofisi) veritabanına (Depo) doğrudan erişmesini engellemektir.
//Bunu yaparak BooksController, bir RepositoryContext'in nasıl yaratıldığını bilmek zorunda kalmaz. Sadece ona verilmesini talep eder.
//Bu "temin etme" (resolve) işlemini (yani Depo Müdürü'nü bulup kapıdan içeri sokma işini) arka planda ASP.NET Core'un kendisi,
//Program.cs'teki kayıtlara bakarak senin için otomatik yapar.

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //_context adlı depo müdürü istendi ve yeri sabit(readonly)
        private readonly RepositoryContext _context;

        /* Bu kısmın "en önemli" olmasının nedeni şudur:
          Talep Etme (Constructor Injection): public BooksController(RepositoryContext context) Bu satır, 
          BooksController'ın (Sipariş Ofisi) ASP.NET Core'a bir kural koymasıdır. Der ki: "Eğer beni 
          (bu BooksController'ı) yaratmak istiyorsan, bana kapıda mutlaka bir RepositoryContext (Depo Müdürü) 
          vermek zorundasın. O olmadan ben çalışamam."
         Bu iki satır sayesinde BooksController (Sipariş Ofisi) "gevşek bağlı" (loosely coupled) hale gelir:

        BooksController'ın, Depo Müdürü'nün nasıl işe alındığını (yani RepositoryContext'in nasıl oluşturulduğunu,
        veritabanı bağlantı cümlesinin ne olduğunu vs.) bilmesine gerek kalmaz.*/
        public BooksController(RepositoryContext context)// (context = Kapıdan giren müdür)
        {
            // Sipariş Ofisi personeli, kapıdan giren müdürü (context) alır
            // ve içeride onun için ayrılan özel koltuğa (_context) oturtur.
            /* Bu satır, kapıda sana verilen Depo Müdürü'nü (context) alıp, 
             * ofisin içindeki o özel, sabit "Müdür Koltuğu"na (_context) oturttuğun andır.*/
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }
    }
}
