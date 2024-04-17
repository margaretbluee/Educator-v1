using ADOPSE.Services;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class OpenAiController : ControllerBase
{ 

    private readonly ILogger<OpenAiController> _logger;
    private readonly IOpenAiService _openAiService;

    public OpenAiController(ILogger<OpenAiController> logger, IOpenAiService openAiService)
    {
                _logger = logger;
                _openAiService = openAiService;
    }

    [HttpPost()]
    [Route("test")]
    public async Task<IActionResult> test()
    {
        var result = await _openAiService.test();
        return Ok(result);
    }

     [HttpPost()]
    [Route("CompleteSentence")]
    public async Task<IActionResult> CompleteSentence(string text)
    {
        var result = await _openAiService.CompleteSentence(text);
        return Ok(result);
    }



    
}