using Biblioteka.Model.Entities;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace Biblioteka.Api.Indices
{
    public class Books_WithIdAndTitleAndAuthor : AbstractIndexCreationTask<Book, Books_WithIdAndTitleAndAuthor.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public List<string> Authors { get; set; }
        }

        public Books_WithIdAndTitleAndAuthor()
        {
            Map = books => from book in books
                           select new
                           {
                               Id = book.Id,
                               Title = book.Title,
                               Authors = book.Authors
                           };
        }
    }
}

