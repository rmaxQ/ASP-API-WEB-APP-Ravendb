using Biblioteka.Api.Indices;
using Biblioteka.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace Biblioteka.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class Members_WithIdAndNameResultController : Controller
    {
        
        private readonly IDocumentStore _store;

        public Members_WithIdAndNameResultController(IDocumentStore store)
        {
            _store = store;
        }
        [HttpGet]
        public IActionResult Get()
        {
            using (var session = _store.OpenSession())
            {
                List<Members_WithIdAndName.Result> results = session
                    .Query<Members_WithIdAndName.Result, Members_WithIdAndName>()
                    .As<Members_WithIdAndName.Result>()
                    .ToList();
                return Ok(results);
            }
        }
    }
}
