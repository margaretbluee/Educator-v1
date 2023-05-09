using System.Security.Claims;
using ADOPSE.Models;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet]
    public IEnumerable<Module> Get()
    {
        int studentId = GetClaimedStudentId();
        return _enrolledService.GetEnrolmentsById(studentId);
    }

    private int GetClaimedStudentId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            var Id = Int32.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            return Id;
        }
        return -1;
    }
}