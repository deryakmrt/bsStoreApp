using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
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
            try
            {
                var books = _context.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBooks([FromRoute(Name = "id")] int id)//parametreler karışmasın diye
        {
            try
            {
                var book = _context
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

                if (book is null)
                    return NotFound(); //error 404

                return Ok(book);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
        //====POST İSTEKLERİ====
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); //400

                _context.Books.Add(book);
                _context.SaveChanges();
                return StatusCode(201, book);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //====PUT İSTEKLERİ====
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] Book book)
        {

            try
            {
                //check book?
                var entity = _context
                    .Books
                    .Where(b => b.Id.Equals(id))
                    .SingleOrDefault();
                if (entity is null)
                    return NotFound();//404

                //check id
                if (id != book.Id)
                    return BadRequest();//400


                entity.Title = book.Title;
                entity.Price = book.Price;
                _context.SaveChanges();
                    return Ok(book);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        //====DELETE İSTEKLERİ====
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBooks([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _context
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

                if (entity is null) return NotFound(new
                {
                    statusCode = 404,
                    message = $"Book with id:{id} could not found."
                });//404

                _context.Books.Remove(entity);
                _context.SaveChanges();
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //====PATCH İSTEKLERİ====
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                //check entity
                var entity = _context
                    .Books
                    .Where(b => b.Id.Equals(id))
                    .SingleOrDefault();

                if (entity is null) return NotFound();//404

                bookPatch.ApplyTo(entity);
                _context.SaveChanges();
                return NoContent();//204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
