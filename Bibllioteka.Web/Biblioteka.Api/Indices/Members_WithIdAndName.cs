using Biblioteka.Model.Entities;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace Biblioteka.Api.Indices
{
    public class Members_WithIdAndName : AbstractIndexCreationTask<Member, Members_WithIdAndName.Result>
    {
        public class Result
        {
            public string? Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        public Members_WithIdAndName()
        {
            Map = members => from member in members
                             select new Result
                             {
                                 Id = member.Id,
                                 FirstName = member.FirstName,
                                 LastName = member.LastName
                             };
        }
    }
}

