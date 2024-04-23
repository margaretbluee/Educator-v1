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
using Lucene.Net.Util;
using Microsoft.Extensions.Caching.Memory;
using Nest;
using Field = Lucene.Net.Documents.Field;
 
using StandardAnalyzer = Lucene.Net.Analysis.Standard.StandardAnalyzer;
using Term = Lucene.Net.Index.Term;
using WildcardQuery = Lucene.Net.Search.WildcardQuery;

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

        public IEnumerable<Module> SearchModulesLucene(string searchQuery,  int searchType)
        { 
    
            // var basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string indexPath = Path.Combine(Environment.CurrentDirectory, IndexName);
            if (!System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.CreateDirectory(indexPath);
            }

            using var indexDir = FSDirectory.Open(indexPath);

    // Re-use the writer to get real-time updates
    using var reader = DirectoryReader.Open(indexDir);
    var searcher = new IndexSearcher(reader);

    var LuceneVersion = Lucene.Net.Util.LuceneVersion.LUCENE_48;

    // Analyzer Setup:
    using var analyzer = new StandardAnalyzer(LuceneVersion); // Use StandardAnalyzer for text fields

    if (!searchQuery.Contains(':'))
    {
        if(searchType == 0){
           searchQuery = $"Id:{searchQuery} OR Name:{searchQuery}"; 
        }
        else if(searchType == 1){
        searchQuery = $"Id:{searchQuery} OR Description:{searchQuery}";
        }
        else if(searchType == 2){
            searchQuery = $"Id:{searchQuery} OR Name:{searchQuery} OR Description:{searchQuery}";
        }
    }

    // MultiFieldQueryParser Setup:
    var multiFieldQueryParser = new Lucene.Net.QueryParsers.Classic.MultiFieldQueryParser(LuceneVersion, new[] {"Id", "Name", "Description" }, analyzer);

    // Query Parsing:
    Query query = multiFieldQueryParser.Parse(searchQuery);
///
    BooleanQuery aggregateQuery = new() {
        { query, Occur.MUST }
    };

    // Search Execution:
    var topDocs = searcher.Search(aggregateQuery, int.MaxValue);
// Document Retrieval:
foreach (ScoreDoc scoreDoc in topDocs.ScoreDocs)
{
    int docId = scoreDoc.Doc;
    Document document = searcher.Doc(docId);
///
    // Retrieve the stored field as a string and parse it to an integer
    int id;
    if (int.TryParse(document.Get("Id"), out id))
    {
        var module = new Module
        {
            Id = id,
            Name = document.Get("Name"),
            Description = document.Get("Description")
        };

        yield return module;
    }
}}
  
    
        public void CreateIndexLucene()
        {
        // Ensures index backward compatibility
const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

// Construct a machine-independent path for the index
//var basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
 string indexPath = Path.Combine(Environment.CurrentDirectory, IndexName);
    Console.WriteLine($"Index Path : {indexPath}");
using var dir = FSDirectory.Open(indexPath);

// Create an analyzer to process the text
var analyzer = new StandardAnalyzer(AppLuceneVersion);

// Create an index writer
var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
using var writer = new IndexWriter(dir, indexConfig);

    // Get data from the database
            var modules = GetModulesFromDatabase(); 
            foreach (var module in modules)
            {
                var document = new Document
                {
                    // Add fields to the document
               new Int32Field("Id", module.Id, Field.Store.YES) ,
                    new TextField("Name", module.Name, Field.Store.YES),
                    new TextField("Description", module.Description, Field.Store.YES)
                
                };

                // Add the document to the index
                writer.AddDocument(document);
               //  Console.WriteLine($"Document is added : {document}");
            }

            // Commit the changes and close the writer
            writer.Commit();
            writer.Dispose();
        
        }
public void CreateIndexElastic()
{
    // connection settings for Elasticsearch
    var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200"))
        .DefaultIndex("module")  
        .RequestTimeout(TimeSpan.FromSeconds(10));

    // instance of ElasticClient
    var client = new ElasticClient(connectionSettings);

   // index and mapping
    var createIndexResponse =  client.Indices.Create(IndexName, c => c
            .Map<Module>(m => m
                .AutoMap()
            )
        );

    if (!createIndexResponse.IsValid)
    {
        // Handle index creation failure
        Console.WriteLine("Failed to create index: " + createIndexResponse.DebugInformation);
        return;
    }

    // Retrieve documents from the database (Assuming GetModulesFromDatabase() returns modules)
    var modules = GetModulesFromDatabase();

    // Index documents into Elasticsearch
    foreach (var module in modules)
    {
        var indexResponse = client.IndexDocument(module);

        if (!indexResponse.IsValid)
        {
            // Handle document indexing failure
            Console.WriteLine("Failed to index document with Id: " + module.Id);
        }
    }}




     public IEnumerable<Module> SearchModulesElastic(string searchQuery)
   {
    // Define connection settings for Elasticsearch
    var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200"))
        .DefaultIndex("module") // Set the default index to "module" (it's a good practice to use lowercase index names)
        .RequestTimeout(TimeSpan.FromSeconds(10));

    // Create an instance of ElasticClient
    var client = new ElasticClient(connectionSettings);

    // Construct the search query dynamically based on the provided search query
    var searchResponse = client.Search<Module>(s => s
        .Query(q => q
               .MultiMatch(m => m
                    .Query(searchQuery)
                    .Fields(f => f
                        .Field(f => f.Id)
                        .Field(f => f.Name)
                        .Field(f => f.Description)
       )
                )
            )
        );

        foreach (var hit in searchResponse.Hits)
        {
            yield return hit.Source;
        }
   // return searchResults;
}

        private IEnumerable<Module> GetModulesFromDatabase()
        {

            var modules = _aspNetCoreNTierDbContext.Module.ToList();
            return modules;
        }

    }
}
