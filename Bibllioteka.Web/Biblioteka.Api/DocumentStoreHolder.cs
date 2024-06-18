using System;
using Biblioteka.Api.Indices;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace Biblioteka.Web
{
    public class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
                var store = new DocumentStore
                {
                    Urls = new[] { "http://localhost:8080" },
                    Database = "Biblioteka"
                };

                var initializedStore = store.Initialize();
                new Categories_WithBookCount().Execute(initializedStore);
                new Books_WithIdAndTitleAndAuthor().Execute(initializedStore);
                new Members_WithIdAndName().Execute(initializedStore);

                // Tworzenie indeksów po inicjalizacji DocumentStore
                /*IndexCreation.CreateIndexes(typeof(Categories_WithBookCount).Assembly, initializedStore);
                IndexCreation.CreateIndexes(typeof(Books_WithIdAndTitleAndAuthor).Assembly, initializedStore);
                IndexCreation.CreateIndexes(typeof(Members_WithIdAndName).Assembly, initializedStore);*/

                return initializedStore;

            });

        public static IDocumentStore Store => LazyStore.Value;
    }
}