using System.Configuration;
using System.Reflection;
using Nest;
using ADOPSE.Models;
using Module = ADOPSE.Models.Module;

namespace ADOPSE.Extensions
{
    public static class ElasticSearchExtensions
    {
 //initiallization of elasticsearch with services in order to pass the instance with dependency injection
      public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration
        )
        {
           var url = configuration["ElasticConfiguration:Uri"];
           var defaultIndex = configuration["ElasticConfiguration:index"];
//need credentials in case its not running locally
            var settings = new ConnectionSettings(new Uri(url))
            .PrettyJson()
            .DefaultIndex(defaultIndex);

           AddDefaultMappings(settings);
        
//CREATE CLIENT (singleton : type of dependency injection, same instance available in the app)
          
          //CREATE ELASTIC SEARCH CLIENT
           var client = new ElasticClient(settings);
           services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);

        }
    

    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        //ignore list 
        settings.DefaultMappingFor<Module>(p=>
        p.Ignore(x => x.Id)
            .Ignore(x => x.Completed)
            .Ignore(x => x.Created)
            .Ignore(x => x.DifficultyId)
            .Ignore(x => x.DifficultyName)
            .Ignore(x => x.leaderId)
          // .Ignore(x => x.GoogleCalendarID
            .Ignore(x => x.Lecturer)
            .Ignore(x => x.LecturerName)
            .Ignore(x => x.ModuleType)
            .Ignore(x => x.ModuleTypeId)
            .Ignore(x => x.ModuleTypeName)
            .Ignore(x => x.Price)
            .Ignore(x => x.Rating)
            .Ignore(x => x.SubCategoryId)
            );
    }


//whenever the app starts is going to check if it has the index created
    private static void CreateIndex(IElasticClient client, string indexName)
    {
        //Create: put request to elasticSearchAPI to create the indices
        client.Indices.Create(indexName, i => i.Map<Module>(x => x.AutoMap()));
    }
}
}