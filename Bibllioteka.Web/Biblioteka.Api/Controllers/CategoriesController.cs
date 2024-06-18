using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Net.Http;
using Raven.Client.Documents;
using Microsoft.AspNetCore.Http;

namespace Biblioteka.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CategoriesController : Controller
    {
        private readonly IDocumentStore _store;

        public CategoriesController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public IActionResult Get()
        {
            using(var session = _store.OpenSession())
            {
                var categories = session.Query<Category>().ToList();
                return Ok(categories);
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            using(var session = _store.OpenSession())
            {
                var category = session.Load<Category>("categories/" + id);
                if (category == null)
                    return NotFound();

                return Ok(category);
            }
            
        }

        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            using(var session = _store.OpenSession())
            {
                session.Store(category);
                session.SaveChanges();
                return Ok(category);
            }
            
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Category category)
        {
            using(var session = _store.OpenSession())
            {
                var existingCategory = session.Load<Category>("categories/" + id);
                if (existingCategory == null)
                    return NotFound();

                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;

                session.SaveChanges();
                return Ok(existingCategory);
            }
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            using( var session = _store.OpenSession())
            {
                var category = session.Load<Category>("categories/" + id);
                if (category == null)
                    return NotFound();

                session.Delete(category);
                session.SaveChanges();
                return Ok();
            }
            
        }
    }
}

