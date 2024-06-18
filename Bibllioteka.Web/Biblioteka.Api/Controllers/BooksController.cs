using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Linq;
using Raven.Client.Documents;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;

namespace Biblioteka.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BooksController : ControllerBase
    {
        private readonly IDocumentStore _store;

        public BooksController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using(var session = _store.OpenSession()) 
            {
                var books = session.Query<Book>().ToList();
                return Ok(books);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            using( var session = _store.OpenSession()) 
            {
                var book = session.Load<Book>("books/" + id);
                if (book == null)
                    return NotFound();

                return Ok(book);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            using(var session = _store.OpenSession())
            {
                session.Store(book);
                session.SaveChanges();
                return Ok(book);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Book book)
        {
            using(var session = _store.OpenSession())
            {
                var existingBook = session.Load<Book>("books/" + id);
                if (existingBook == null)
                    return NotFound();

                existingBook.Title = book.Title;
                existingBook.Authors = book.Authors;
                existingBook.YearPublished = book.YearPublished;
                existingBook.ISBN = book.ISBN;
                existingBook.CategoryId = book.CategoryId;
                existingBook.Summary = book.Summary;

                session.SaveChanges();
                return Ok(existingBook);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using(var session = _store.OpenSession())
            {
                var book = session.Load<Book>("books/"+id);
                if (book == null)
                    return NotFound();

                session.Delete(book);
                session.SaveChanges();
                return NoContent();
            }
            
        }
    }
}

