using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Biblioteka.Model.Entities;
using System.Linq;
using Raven.Client.Documents;
using Microsoft.AspNetCore.Http;
using Biblioteka.Model;

namespace Biblioteka.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MembersController : ControllerBase
    {
        private readonly IDocumentStore _store;

        public MembersController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int skip, int take)
        {
            using (var session = _store.OpenSession())
            {
                var members = session.Query<Member>().Skip((skip-1) * take).Take(take).ToList();
                var totalCount = session.Query<Member>().Count();

                var result = new PagedResult<Member>
                {
                    Items = members,
                    TotalCount = totalCount
                };

                return Ok(result);
            }
        }

        [HttpGet("{litera}")]
        public async Task<IActionResult> GetLastNameStartsWith(char litera)
        {
            using (var session = _store.OpenSession())
            {
                string query = $"{litera}*";
                var members = session.Query<Member>()
                                     .Search(m => m.LastName, query)
                                     .ToList();
                if (members == null)
                    return BadRequest();

                return Ok(members);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            using( var session = _store.OpenSession())
            {
                var member = session.Load<Member>("members/" + id);
                if (member == null)
                    return BadRequest();

                return Ok(member);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Member member)
        {
            using(var session = _store.OpenSession())
            {
                session.Store(member);
                session.SaveChanges();
                return Ok(member);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Member member)
        {
            using(var session= _store.OpenSession())
            {
                var existingMember = session.Load<Member>("members/"+id);
                if (existingMember == null)
                    return NotFound()
    ;
                existingMember.FirstName = member.FirstName;
                existingMember.LastName = member.LastName;
                existingMember.Email = member.Email;
                existingMember.PhoneNumber = member.PhoneNumber;
                existingMember.MembershipSince = member.MembershipSince;

                session.SaveChanges();
                return Ok(existingMember);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using( var session = _store.OpenSession())
            {
                var member = session.Load<Member>("members/" + id);
                if (member == null)
                    return NotFound();

                session.Delete(member);
                session.SaveChanges();
                return Ok();
            }
            
        }
    }
}

