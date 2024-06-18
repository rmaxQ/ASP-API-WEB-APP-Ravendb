using Biblioteka.Model.Entities;
using Raven.Client.Documents.Indexes;

namespace Biblioteka.Api.Indices
{
    public class Categories_WithBookCount : AbstractMultiMapIndexCreationTask<Categories_WithBookCount.Result>
    {
        public class Result
        {
            public string CategoryId { get; set; }
            public string CategoryName { get; set; }
            public int BookCount { get; set; }
        }

        public Categories_WithBookCount()
        {
            AddMap<Category>(categories => from category in categories
                                           select new
                                           {
                                               CategoryId = category.Id,
                                               CategoryName = category.Name,
                                               BookCount = 0
                                           });

            AddMap<Book>(books => from book in books
                                  select new
                                  {
                                      CategoryId = book.CategoryId,
                                      CategoryName = (string)null,
                                      BookCount = 1
                                  });

            Reduce = results => from result in results
                                group result by result.CategoryId into g
                                select new
                                {
                                    CategoryId = g.Key,
                                    CategoryName = g.Select(x => x.CategoryName).FirstOrDefault(x => x != null),
                                    BookCount = g.Sum(x => x.BookCount)
                                };
        }
    }
}
