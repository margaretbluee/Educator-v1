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
        return Enumerable.Range(1, 5).Select(index => new Lecturer
        {
            Id = 5,
            Name = "Eimai mia katigoria",
            Bio = "hellooooo",
            Email = "aaa@gmail.com",
            Website = "www.kati.gr"
        })
            .ToArray();
    }
}