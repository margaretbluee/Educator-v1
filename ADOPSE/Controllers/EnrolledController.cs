using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADOPSE.Controllers;

[ApiController]
[Route("[controller]")]
public class EnrolledController : ControllerBase
{
    private readonly ILogger<EnrolledController> _logger;
    private readonly IEnrolledService _enrolledService;

    public EnrolledController(ILogger<EnrolledController> logger, IEnrolledService enrolledService)
    {
        _logger = logger;
        _enrolledService = enrolledService;
    }

    [HttpGet]
    public IEnumerable<Module> Get()
    {
        return _enrolledService.GetEnrolmentsById(300);
    }
}