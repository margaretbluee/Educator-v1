 using ADOPSE.Models;
 using ADOPSE.Repositories.IRepositories;
 using ADOPSE.Services.IServices;
 using Microsoft.AspNetCore.Mvc;
 
using HtmlAgilityPack;
 
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using MistralSharp;
using MistralSharp.Models;




namespace ADOPSE.Services;

 
 public class ModuleService : IModuleService
 {
     private readonly IModuleRepository _moduleRepository;
     private readonly ILogger<ModuleService> _logger;
      
      private readonly string google_key = "AIzaSyB-XerdLLd5yPOrek3-oIuN2H9l7QwoLXQ";
 //adopse3@adopse3.iam.gserviceaccount.com 
//read hugginf face description token:  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
//mistral api key iDUVIKlLPIrn0uQmBjlR9an4j4WfVuGm
// WRITE TOKEN HF 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
 
private readonly string mistral_apiKey = "TEeDC4DUTia9azwiMHcecjWdcQQtC60m";


//Check if all Descriptions are filled
public bool FindAndWriteMissingIDsToFile(string folderPath, int startID, int endID)
{
    try
    {
        List<int> foundIDs = new List<int>();

        // Get all files in the folder
        string[] files = Directory.GetFiles(folderPath);

        // Extract IDs from filenames
        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            if (int.TryParse(fileName.Split('_')[0], out int id))
            {
                foundIDs.Add(id);
            }
        }

        // Find missing IDs within the specified range
        List<int> missingIDs = new List<int>();
        for (int id = startID; id <= endID; id++)
        {
            if (!foundIDs.Contains(id))
            {
                missingIDs.Add(id);
            }
        }

        // Write missing IDs to a single text file
        string missingIDsFilePath = Path.Combine(folderPath, "missing_ids.txt");
        using (StreamWriter writer = new StreamWriter(missingIDsFilePath, true)) // Append mode
        {
            foreach (int missingID in missingIDs)
            {
                writer.WriteLine(missingID);
                _logger.LogInformation($"Module with ID {missingID} added to the missing descriptions file.");
            }
        }

        Console.WriteLine("Missing IDs have been written to missing_ids.txt.");
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return false;
    }
}


//Find Descriptions without context
public bool Find_missing_files()
{
    string folderPath = @"C:\Users\marga\Desktop\εαρ23-24\AΔΟΠΣΕ\mistral_descriptions";
    int startID = 15415;
    int endID = 30305;

    bool finished = FindAndWriteMissingIDsToFile(folderPath, startID, endID);

    if (finished)
    {
        Console.WriteLine("Missing IDs have been written to missing_ids.txt.");
    return true;
    }
    else
    {
        Console.WriteLine("Failed to write missing IDs.");
        return false;
    }
}


//Update Descriptions using MISTRAL AI API
public async  Task<bool> Mistral (int start, int end){
  string folderPath = @"C:\Users\marga\Desktop\εαρ23-24\AΔΟΠΣΕ\mistral_descriptions";

 for (int id = start; id <= end; id++)
    {
        var module = _moduleRepository.GetModuleById(id);
        if (module == null)
        {
            return false;
        }
        
      module =  _moduleRepository.GetModuleById(id);
   
   // Create a new instance of MistralClient and pass your API key
var mistralClient = new MistralClient(mistral_apiKey);
// Prepare a chat request
        var chatRequest = new ChatRequest()
        {
            Model = ModelType.MistralMedium,
            Messages = new List<Message>
            {
                new Message()
                {
                    Role = "user",
           Content = $"Give me a description for module named {module.Name} telling me why the course is important.Please provide 4 sentences."       }
            },
          
            Temperature = 0.7
        };

        try
        {
            // Call the chat endpoint
            var chatResponse = await mistralClient.ChatAsync(chatRequest);
  // Create a file for each module's response
            string filePath = Path.Combine(folderPath, $"{id}_mistralAI.txt");
            using (StreamWriter writer = File.CreateText(filePath))
               {
            // Handle the response
            foreach (var choice in chatResponse.Choices)
            {
                // Print the generated response
                Console.WriteLine($"Mistral AI Response: {choice.Message.Content}");
             
                    // Write the generated response to the file
                    writer.WriteLine($"Mistral AI Response: {choice.Message.Content}");
//update db           
             module.Description = choice.Message.Content;
             _moduleRepository.Update(module);
           
            }
            }
              _logger.LogInformation($"Module with ID {id} processing complete");

        }
        
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
        }
    }
        return true;
    }
 

 //Update Descriptions using Google Search API  
 public async Task<bool> UpdateModulesWithGoogleDescription(int start, int end)
{
      string folderPath = @"C:\Users\marga\Desktop\εαρ23-24\AΔΟΠΣΕ\descriptions\";
    //check if module exists
    for (int id = start; id <= end; id++)
    {
        var module = _moduleRepository.GetModuleById(id);
        if (module == null)
        {
            return false;
        }

        string name = module.Name;
        string description = module.Description;
//check for problematic descriptions in order to refill them
        string[] descriptionsToCheck = {
            "No relevant content found on the webpage.",
            "Invalid URL.",
            "Failed to retrieve description from the webpage.",
            "Lorem Ipsum is simpl",
            "- Το μάθημα δεν διαθέτει περιγραφή -.",
            ".*?Φ. Π. Α.*?",
            ".*?cookies.*?",
            "Something went wrong. Wait a moment and try again.",
            ".*?�.*?",      // Match any description containing "�"
                    ".*?$.*?",      // Match any description ending with "$"
                    ".*?€.*?",      // Match any description containing "€"
                    ".*?euro.*?"  
        };
     bool isDescriptionError = descriptionsToCheck.Any(pattern => Regex.IsMatch(description, pattern));
      bool containsSymbol = description.Contains('�');
            int maxAttempts = 5;
        string url = null;

        // Attempt to fetch a new description until a non-error description is found or maxAttempts is reached
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            if(isDescriptionError||containsSymbol){
          
          //generate a search url and choose the first if attempt =1, second if attempt = 2 etc..
            url = await SpecifyURLForDescription(attempt, id);
            // Create a file path for the txt file
                string filePath = Path.Combine(folderPath, $"{id}_searchResult.txt");
            // Serialize the URL object to JSON
                string json = JsonConvert.SerializeObject(url, Formatting.Indented);

                // Write the JSON object and the number of attempts to a txt file
                File.WriteAllText(filePath, $"Attempts: {attempt}\n\n{json}");
            
            
            string newDescription = await GetDescriptionFromWebPage(url);

            // If the new description is not an error, update the module description and break the loop
            if (!descriptionsToCheck.Contains(newDescription))
            {
                module.Description = newDescription;
                bool updated = _moduleRepository.Update(module);
                if (!updated)
                {
                    return false; // Failed to update module
                }
                else
                {
                    break; // Exit the loop as a suitable description is found
                }
            }
        } 
        }
 _logger.LogInformation($"Module with ID {id} processing complete");
    }

    // If all modules are successfully updated, return true.
    return true;
}

//Search in the 6th URL for Description 
public async Task<bool> UpdateModuleWithFaultyDescription(int id)
{
    var module = _moduleRepository.GetModuleById(id);
    if (module == null)
    {
        return false;
    }
    //string name = module.Name;

 string  sixth_url = await SpecifyURLForDescription(6,id);
string description = await GetDescriptionFromWebPage(sixth_url);

    // Update module description
    module.Description = description;

    // Update module in repository
    return _moduleRepository.Update(module);
}

//Fix wrong google search descriptions
public async Task<bool> FixWrongDescriptions(int moduleId)
{
    try
    {
        string folderPath = @"C:\Users\marga\Desktop\εαρ23-24\AΔΟΠΣΕ\descriptions\";
        string searchResultFilePath = Path.Combine(folderPath, $"{moduleId}_searchResult.txt");
        string linksFilePath = Path.Combine(folderPath, $"{moduleId}_Links.txt");

        if (!File.Exists(searchResultFilePath) || !File.Exists(linksFilePath))
        {
            // Files not found, return false
            return false;
        }

        // Read the content of {id}_searchResult.txt
        string[] searchResultLines = File.ReadAllLines(searchResultFilePath);

        // Extract the attempts count from the first line
        if (int.TryParse(searchResultLines.FirstOrDefault()?.Split(':')[1]?.Trim(), out int attempts))
        {
            // Check if any link ends with ".pdf"
            bool hasPdfLink = searchResultLines.Any(line => line.Trim().EndsWith(".pdf"));

            // If there is a PDF link, proceed
            if (hasPdfLink)
            {
                // Read the content of {id}_Links.txt
                string[] links = File.ReadAllLines(linksFilePath);

                // Choose a link that is not a PDF
                string chosenLink = links.FirstOrDefault(link => !link.Trim().EndsWith(".pdf"));
                if (chosenLink == null)
                {
                    // No non-PDF links found, return false
                    return false;
                }

                int chosenIndex = Array.IndexOf(links, chosenLink) + 1; // Index starts from 1

                // Update the attempts count and the chosen link in {id}_searchResult.txt
                searchResultLines[0] = $"Attempts: {chosenIndex}";
                searchResultLines[1] = chosenLink;

                // Write the updated content back to {id}_searchResult.txt
                File.WriteAllLines(searchResultFilePath, searchResultLines);

                // Fetch new description using the chosen link
                string url = await SpecifyURLForDescription(chosenIndex, moduleId);
                string description = await GetDescriptionFromWebPage(url);

                // Get the module by ID
                var module = _moduleRepository.GetModuleById(moduleId);
                if (module == null)
                {
                    return false;
                }

                // Update module description
                module.Description = description;

                // Update module in repository
                return _moduleRepository.Update(module);
            }
        }
    }
    catch (Exception ex)
    {
        // Handle any exceptions by logging
        _logger.LogError(ex, "Error in FixWrongDescriptions for module ID {ModuleId}", moduleId);
    }

    return false; // Return false in case of any errors
}


  //Search for a description in a specified url 
public  async Task<string> SpecifyURLForDescription(int url_number, int id)
{
   
     try
        {
 var module = _moduleRepository.GetModuleById(id);
 string name = module.Name;
 
     string searchQuery = name;
//connect with google key in my google search engine
 string cx =  "05b900cddc2de4a83";
string apiKey = "AIzaSyB-XerdLLd5yPOrek3-oIuN2H9l7QwoLXQ";
 using (HttpClient client = new HttpClient())
        {
            string requestUrl = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cx}&q={searchQuery}";
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                dynamic jsonData = JsonConvert.DeserializeObject(responseString);

//CREATE FILES WITH LINKS
WriteJsonDataAndLinksToFile(id,responseString,jsonData);


//return the specified link 
                string  ResultLink = null;
               
                if (jsonData.items != null && jsonData.items.Count >= url_number)
                {
                     ResultLink = jsonData.items[url_number-1].link;
                    return ResultLink;
                }
  if (!string.IsNullOrEmpty(ResultLink))
    {
     return ResultLink;
    }
    else
    {
        return "No search results found in first URL.";
    }
            }
            else
            {
                // Handle unsuccessful response
                Console.WriteLine($"Error: {response.StatusCode}");
                return "PROBLEM";
            }
        }
    }
    catch (Exception ex)
    {
        // Handle any exceptions
        Console.WriteLine("Error: " + ex.Message);
        return "PROBLEM";
    }
}

//Keep record of Google Search Custom Search Functionality
public void WriteJsonDataAndLinksToFile(int id, string responseString, dynamic jsonData)
    {
        string folderPath = @"C:\Users\marga\Desktop\εαρ23-24\AΔΟΠΣΕ\descriptions\";
        string jsonFilePath = Path.Combine(folderPath, $"{id}_jsonSearchResponse.txt");
        string linksFilePath = Path.Combine(folderPath, $"{id}_Links.txt");

        // Append JSON data obtained from the Google Custom Search API
        if (!File.Exists(jsonFilePath) || new FileInfo(jsonFilePath).Length == 0)
        {
            using (StreamWriter writer = new StreamWriter(jsonFilePath))
            {
                // Write the JSON data to the file
                writer.WriteLine("JSON data obtained from the Google Custom Search API:");
                writer.WriteLine(responseString);
            }
        }

        // Append links to a txt file if it doesn't exist
        if (!File.Exists(linksFilePath) || new FileInfo(linksFilePath).Length == 0)
        {
            using (StreamWriter writer = new StreamWriter(linksFilePath))
            {
                // Write the header line
                writer.WriteLine("Links:");

                // Write the links
                for (int i = 0; i < 10 && i < jsonData.items.Count; i++)
                {
                    writer.WriteLine(jsonData.items[i].link);
                }
            }
        }
    }


//Search inside a page to find sentences inside a paragraph element
  static async Task<string> GetDescriptionFromWebPage(string url)
{
    try
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
        {
            return "Invalid URL.";
        }

        using (HttpClient client = new HttpClient())
        {
            string htmlContent = await client.GetStringAsync(uri);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            var nodes = htmlDocument.DocumentNode.SelectNodes("//p");

            if (nodes != null)
            {
                int sentenceCount = 0;
                StringBuilder descriptionBuilder = new StringBuilder();
                foreach (var node in nodes)
                {
                    string[] sentences = node.InnerText.Split('.', '!', '?');
                    foreach (var sentence in sentences)
                    {
                        if (!string.IsNullOrWhiteSpace(sentence))
                        {
                            descriptionBuilder.Append(sentence.Trim());
                            descriptionBuilder.Append(". ");
                            sentenceCount++;
                            if (sentenceCount >= 10)
                            {
                                break;
                            }
                        }
                    }
                    if (sentenceCount >= 5)
                    {
                        break;
                    }
                }
                string description = descriptionBuilder.ToString().Trim();
                return !string.IsNullOrEmpty(description) ? description : "No relevant content found on the webpage.";
            }
            else
            {
                return "No relevant content found on the webpage.";
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
        return "Failed to retrieve description from the webpage.";
    }
}


public class Result{
    public string Title {get; set;}
    public string Link {get; set;}
    public string Snippet {get;set;}
        public string Source {get; set;}
    public string Query {get; set;}
    public int Index {get;set;}

}
 public class module_link 
{ 
	public string? Url { get; set; } 
 
}



     public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger)
     {
         _moduleRepository = moduleRepository;
         _logger = logger;
     }
 
     public IEnumerable<Module> GetModules()
     {
         return _moduleRepository.GetModules();
     }
 
     public Module GetModuleById(int id)
     {
         return _moduleRepository.GetModuleById(id);
     }
 
     public IActionResult GetModuleStacks(int limit, int offset)
     {
         var modules = _moduleRepository.GetModuleStacks(limit, offset);
         var count = _moduleRepository.GetModuleCount();
         var response = new { count, modules };
         return new JsonResult(response);
     }
 
     public IActionResult GetModuleStacksByLecturerId(int limit, int offset, int id)
     {
         var modules = _moduleRepository.GetModuleStacksByLecturerId(limit, offset, id);
         var count = _moduleRepository.GetModuleCountByLecturerId(id);
         var response = new { count, modules };
         return new JsonResult(response);
     }
 
     public IActionResult GetFilteredModulesLucene(Dictionary<string, string> dic, int limit, int offset)
     {
         IEnumerable<Module> modules = _moduleRepository.GetFilteredModulesLucene(dic, limit, offset);
         var count = _moduleRepository.GetModuleCountFilteredLucene(dic);
         var response = new { count, modules };
         return new JsonResult(response);
     }
 
     public Module GetModuleByCalendarId(string id)
     {
         return _moduleRepository.GetModuleByCalendarId(id);
     }
 
     public void CreateIndexLucene()
     {
         _moduleRepository.CreateIndexLucene();
     }

  
}