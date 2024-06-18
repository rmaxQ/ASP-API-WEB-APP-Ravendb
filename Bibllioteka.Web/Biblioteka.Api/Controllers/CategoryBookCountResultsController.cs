using Biblioteka.Api.Indices;
using Biblioteka.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace Biblioteka.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CategoryBookCountResultsController : ControllerBase
    {
        private readonly IDocumentStore _store;

        public CategoryBookCountResultsController(IDocumentStore store)
        {
            _store = store;
        }
        [HttpGet]
        public IActionResult GetCategoryBookCounts()
        {
            using (var session = _store.OpenSession())
            {
                var results = session
                    .Query<Categories_WithBookCount.Result, Categories_WithBookCount>()
                    .ProjectInto<CategoryBookCountResult>()
                    .ToList();
                return Ok(results);
            }
        }
    }
}
