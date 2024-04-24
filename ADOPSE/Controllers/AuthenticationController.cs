using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ADOPSE.Data;
using ADOPSE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ADOPSE.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _config;
    private readonly MyDbContext _aspNetCoreNTierDbContext;

    public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration config, MyDbContext aspNetCoreNTierDbContext)
    {
        _logger = logger;
        _config = config;
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }

    // for debugging purposes
    [Authorize]
    [HttpGet("getme")]
    public IActionResult GetCurrentStudent()
    {
        var currentStudent = GetMe();
        return Ok($"Hi you are an {currentStudent}");
    }

    // for debugging purposes
    private Student GetMe()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            var Id = Int32.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            return _aspNetCoreNTierDbContext.Student.Where(x => x.Id == Id).FirstOrDefault();
        }
        return null;
    }

    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<Student> GetAllStudents()
    {
        return _aspNetCoreNTierDbContext.Student.ToList();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult Login([FromBody] Student userLogin)
    {
        var user = Authenticate(userLogin);
        if (user != null)
        {
            var token = GenerateToken(user);
            var json = new JsonResult(new { token });
            return json;
        }

        return NotFound("user not found");
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] Student newStudent)
    {
        var user = ExistsAlreadyStudent(newStudent);
        if (user == null)
        {
            _aspNetCoreNTierDbContext.Student.Add(newStudent);
            _aspNetCoreNTierDbContext.SaveChanges();
            var createdStudent = $"Student {newStudent} created successfully";
            return new JsonResult(new { createdStudent });
        }
        return BadRequest("aaaaaaaaaaaaaaaaaaaaa");
    }

    // To generate token
    private string GenerateToken(Student user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            // new Claim(ClaimTypes.Role,user.Role)
        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15), // token longs 15 mins
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    private Student ExistsAlreadyStudent(Student studentToCheck)
    {
        var currentUser = _aspNetCoreNTierDbContext.Student.FirstOrDefault(x => x.Username.ToLower() ==
            studentToCheck.Username.ToLower() || x.Email == studentToCheck.Email);
        if (currentUser != null)
        {
            return currentUser;
        }
        return null;
    }

    //To authenticate user
    private Student Authenticate(Student userLogin)
    {
        var currentUser = _aspNetCoreNTierDbContext.Student.FirstOrDefault(x => x.Username.ToLower() ==
            userLogin.Username.ToLower() && x.Password == userLogin.Password);
        if (currentUser != null)
        {
            return currentUser;
        }
        return null;
    }
}