using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class LecturerController : ControllerBase
{
    private readonly ILogger<LecturerController> _logger;
    private readonly ILecturerService _lecturerService;

    public LecturerController(ILogger<LecturerController> logger, ILecturerService lecturerService)
    {
        _logger = logger;
        _lecturerService = lecturerService;
    }
    
    [HttpGet]
    public IEnumerable<Lecturer> Get()
    {
        return _lecturerService.GetLecturers();
    }
}