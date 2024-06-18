using Biblioteka.Api.Indices;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace Biblioteka.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class Books_WithIdAndTitleAndAuthorResultController : Controller
    {
        private readonly IDocumentStore _store;

        public Books_WithIdAndTitleAndAuthorResultController(IDocumentStore store)
        {
            _store = store;
        }
        [HttpGet]
        public IActionResult Get()
        {
            using (var session = _store.OpenSession())
            {
                var results = session
                    .Query<Books_WithIdAndTitleAndAuthor.Result, Books_WithIdAndTitleAndAuthor>()
                    .ToList();
                return Ok(results);
            }
        }
    }
}
