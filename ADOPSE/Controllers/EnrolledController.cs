﻿using System.Security.Claims;
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

    [Authorize]
    [HttpPost("{moduleId}")]
    public IActionResult AddEnrolement(int moduleId)
    {
        int studentId = GetClaimedStudentId();
        _enrolledService.AddEnrolment(studentId, moduleId);

        return Ok("Enrolment done");
    }

    [HttpGet("isEnrolled/{moduleId}")]
    public IActionResult IsEnrolled(int moduleId)
    {
        bool IsEnrolled = false;
        var response = new { IsEnrolled };
        int studentId = GetClaimedStudentId();
        if (studentId == -1)
        {

            return new JsonResult(response);
        }
        bool enrolled = _enrolledService.isEnrolled(studentId, moduleId);

        IsEnrolled = enrolled;

        response = new { IsEnrolled };
        return new JsonResult(response);
    }

    [HttpPost("getIsEnrolled")]
    public IActionResult GetIsEnrolled([FromBody] EnrolledRequest request)
    {
        int studentId = GetClaimedStudentId();
        int[] moduleIds = request.ModuleIds;
        _logger.LogInformation(moduleIds.ToString());
        if (studentId == -1 || moduleIds == null || moduleIds.Length == 0)
        {
            _logger.LogInformation("AAAAAAAAA");
            var authorized = false;
            var response = new { authorized };
            return new JsonResult(response);
            // return Json(new { authorized = false });
        }
        var IsEnrolled = _enrolledService.GetIsEnrolledById(studentId, moduleIds);

        return new JsonResult(IsEnrolled); ;
    }


    private int GetClaimedStudentId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null && identity.IsAuthenticated)
        {
            var userClaims = identity.Claims;
            var Id = Int32.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            return Id;
        }
        return -1;
    }
}