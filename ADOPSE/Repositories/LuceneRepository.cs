using ADOPSE.Data;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
// using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.Extensions.Caching.Memory;

namespace ADOPSE.Repositories
{
    public class LuceneRepository : ILuceneRepository
    {
        private readonly MyDbContext _aspNetCoreNTierDbContext;

        private readonly ILogger<ModuleRepository> _logger;

        private readonly IMemoryCache _memoryCache;

        private const string IndexCreatedCacheKey = "IndexCreated";

        public LuceneRepository(MyDbContext aspNetCoreNTierDbContext, ILogger<ModuleRepository> logger)
        {
            _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
            _logger = logger;
        }
        private const string IndexName = "lucene";

        public IEnumerable<Module> SearchModules(string searchQuery)
        {
            string indexPath = Path.Combine(Environment.CurrentDirectory, IndexName);
            if (!System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.CreateDirectory(indexPath);
            }


            using var indexDir = FSDirectory.Open(indexPath);

            var LuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

            using var analyzer = new KeywordAnalyzer();
            var queryParser = new QueryParser(LuceneVersion, "Name", analyzer);

            using var reader = DirectoryReader.Open(indexDir, true);
            using var searcher = new IndexSearcher(reader);

            var queryTerms = searchQuery.Split(' ');

            // var phraseQuery = new PhraseQuery();
            // foreach (var term in queryTerms)
            // {
            //     phraseQuery.Add(new Term("Name", term));
            // }

            var booleanQuery = new BooleanQuery();
            foreach (var term in queryTerms)
            {
                var wildcardTerm = new WildcardQuery(new Term("Name", $"*{term}*"));
                var wildcardTerm2 = new WildcardQuery(new Term("Description", $"*{term}*"));

                booleanQuery.Add(wildcardTerm, Occur.SHOULD);
                booleanQuery.Add(wildcardTerm2, Occur.SHOULD);
           
            }


            // var parsedQuery = queryParser.Parse(searchQuery);
            var topDocs = searcher.Search(booleanQuery, int.MaxValue); // Adjust the number of results as needed

            foreach (var scoreDoc in topDocs.ScoreDocs)
            {
                var document = searcher.Doc(scoreDoc.Doc);
                // Map the Lucene document to your Module model
                var module = new Module
                {
                    // Set the properties based on the fields in the Lucene document
                    Id = int.Parse(document.Get("Id")),
                    Name = document.Get("Name"),
                };

                yield return module;
            }
        }
        public void CreateIndex()
        {
            // bool isIndexCreated = _memoryCache.TryGetValue<bool>(IndexCreatedCacheKey, out var indexCreated);
            string indexPath = Path.Combine(Environment.CurrentDirectory, IndexName);
            if (!System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.CreateDirectory(indexPath);
            }

            // var LuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

            using var indexDir = FSDirectory.Open(indexPath);
            using var analyzer = new KeywordAnalyzer();
            var writer = new IndexWriter(indexDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            // Get data from the database
            var modules = GetModulesFromDatabase(); // Replace this with your own logic to fetch modules from the database

            foreach (var module in modules)
            {
                var document = new Document();

                // Add fields to the document
                document.Add(new Field("Id", module.Id.ToString(), Field.Store.YES, Field.Index.NO));
                document.Add(new Field("Name", module.Name, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("Description", module.Description, Field.Store.YES, Field.Index.ANALYZED));

                // Add the document to the index
                writer.AddDocument(document);
            }

            // Commit the changes and close the writer
            writer.Commit();
            writer.Dispose();
        }

        private IEnumerable<Module> GetModulesFromDatabase()
        {

            var modules = _aspNetCoreNTierDbContext.Module.ToList();
            return modules;
        }

    }
}
