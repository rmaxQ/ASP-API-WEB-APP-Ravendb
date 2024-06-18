using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Linq;
using Raven.Client.Documents;
using Microsoft.AspNetCore.Http;

namespace Biblioteka.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class LoansController : ControllerBase
    {
        private readonly IDocumentStore _store;

        public LoansController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using(var session = _store.OpenSession())
            {
                var loans = session.Query<Loan>().ToList();
                return Ok(loans);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            using (var session = _store.OpenSession())
            {
                var loan = session.Load<Loan>("loans/" + id);
                if (loan == null)
                    return NotFound();

                return Ok(loan); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Loan loan)
        {
            using(var session = _store.OpenSession())
            {
                session.Store(loan);
                session.SaveChanges();
                return Ok(loan);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Loan loan)
        {
            using( var session = _store.OpenSession())
            {
                var existingLoan = session.Load<Loan>("loans/" + id);
                if (existingLoan == null)
                    return NotFound();

                existingLoan.MemberId = loan.MemberId;
                existingLoan.BookId = loan.BookId;
                existingLoan.LoanedAt = loan.LoanedAt;
                existingLoan.ReturnedAt = loan.ReturnedAt;

                session.SaveChanges();
                return Ok(existingLoan);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using(var session= _store.OpenSession())
            {
                var loan = session.Load<Loan>("loans/"+id);
                if (loan == null)
                    return NotFound();

                session.Delete(loan);
                session.SaveChanges();
                return NoContent();
            }
            
        }
    }
}

