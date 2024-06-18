using Biblioteka.Model.Entities;

namespace Biblioteka.Web.Models
{
    public class MemberViewModel
    {
        public List<Member> Members { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
