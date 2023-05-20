// using ADOPSE.Models;

// using ADOPSE.Data;
// using ADOPSE.Repositories.IRepositories;
// using Microsoft.EntityFrameworkCore;
// using Lucene.Net.Index;
// using Lucene.Net.Search;
// using Lucene.Net.Store;
// using Lucene.Net.Analysis;
// using Lucene.Net.Documents;
// using Lucene.Net.QueryParsers;
// using Lucene.Net.Util;


// namespace ADOPSE.Repositories;

// public class LuceneRepository : ILuceneRepository
// {

//     public IEnumerable<Module> SearchModules(string searchQuery)
//     {
//         var indexDirectory = FSDirectory.Open("<path_to_index_directory>");

//         var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
//         var queryParser = new QueryParser(LuceneVersion.LUCENE_48, "Content", analyzer);

//         using (var reader = DirectoryReader.Open(indexDirectory))
//         using (var searcher = new IndexSearcher(reader))
//         {
//             var parsedQuery = queryParser.Parse(searchQuery);
//             var topDocs = searcher.Search(parsedQuery, 10); // Adjust the number of results as needed

//             foreach (var scoreDoc in topDocs.ScoreDocs)
//             {
//                 var document = searcher.Doc(scoreDoc.Doc);
//                 // Map the Lucene document to your Module model
//                 var module = new Module
//                 {
//                     // Set the properties based on the fields in the Lucene document
//                     Id = int.Parse(document.Get("Id")),
//                     Name = document.Get("Name"),
//                     // Set other properties accordingly
//                 };

//                 yield return module;
//             }
//         }
//     }
// }
