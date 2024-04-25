using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ADOPSE.Data;
using ADOPSE.Models;
using Lucene.Net.Index;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public IActionResult GetCurrentUser()
    {
        var currentUser = GetMe();
        return Ok($"Hi you are an {currentUser}");
    }

    // for debugging purposes
    private Users GetMe()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            var Id = Int32.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            return _aspNetCoreNTierDbContext.USERS.Where(x => x.Id == Id).FirstOrDefault();
        }
        return null;
    }
   







    [AllowAnonymous]
    [HttpGet("getUsers")]
    public IEnumerable<Users> GetAllUsers()
    {
        var users = _aspNetCoreNTierDbContext.USERS.ToList();
        return users;
        
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult Login([FromBody] Users userLogin)
    {
        var user = Authenticate(userLogin);
        if (user != null)
        {
            var token = GenerateToken(user);
            var json = new JsonResult(new { token,user.Role });
            return json;
        }

        return NotFound("user not found");
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] Users newUser)
    {
        var user = ExistsAlreadyUser(newUser);
        if (user == null)
        {
            
            _aspNetCoreNTierDbContext.USERS.Add(newUser);
            _aspNetCoreNTierDbContext.SaveChanges();
            var createdUser = $"User {newUser} created successfully";
            return new JsonResult(new { createdUser });

        }
        return BadRequest("aaaaaaaaaaaaaaaaaaaaa");
    }


    // To generate token
    private string GenerateToken(Users user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Role,user.Role)
        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15), // token longs 15 mins
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    private Users ExistsAlreadyUser(Users userToCheck)
    {
        var currentUser = _aspNetCoreNTierDbContext.USERS.FirstOrDefault(x => x.Username.ToLower() ==
            userToCheck.Username.ToLower() || x.Email == userToCheck.Email);
        if (currentUser != null)
        {
            return currentUser;
        }
        return null;
    }
   
    

    //To authenticate user
    private Users Authenticate(Users userLogin)
    {
        var currentUser = _aspNetCoreNTierDbContext.USERS.FirstOrDefault(x => x.Username.ToLower()==
        userLogin.Username.ToLower() && x.Password == userLogin.Password  && x.Suspend == false);
        if (currentUser != null)
        {
            return currentUser;
        }
        return null;
    }

    
    public class UpdateUserRoleModel
    {
        public int userId { get; set; }
        public string newRole { get; set; }
    }

    
    [HttpPost("updateUserRole")]
    public IActionResult UpdateUserRole([FromBody] UpdateUserRoleModel model)
    {
        var user = _aspNetCoreNTierDbContext.USERS.FirstOrDefault(u => u.Id == model.userId);

       // Update User Role
        user.Role = model.newRole; 
         _aspNetCoreNTierDbContext.SaveChanges();
        var updatedMessage = $"User {user.Id} role updated successfully"; 
        return new JsonResult(new { updatedMessage });
       
    } 


    public class suspendModel
    {
        public int userId { get; set; }
        public bool suspend { get; set; }
    }

    [HttpPost("suspendUser")]
    public IActionResult suspendUser([FromBody] suspendModel susp)
    {
        Console.WriteLine($"Received UserId: {susp.userId}, Suspend: {susp.suspend}");

        var user = _aspNetCoreNTierDbContext.USERS.FirstOrDefault(u => u.Id == susp.userId);

        // Update the suspend status
        user.Suspend = susp.suspend;
        _aspNetCoreNTierDbContext.SaveChanges();

        var updatedMessage = $"User {user.Id} is {(user.Suspend ? "Suspended" : "Unsuspended")}";
        return new JsonResult(new { updatedMessage });
    }

     
    [Authorize]
    [HttpGet("getCurrentUserDetails")]
    public IActionResult GetCurrentUserData()
    {
        var currentUserData = GetMe();
        if (currentUserData != null)
        {
            return Ok(new
            {
                Id = currentUserData.Id,
                Username = currentUserData.Username,
                Password = currentUserData.Password,
                Email = currentUserData.Email
            });
        }
        return NotFound("User not found");
    }

    
    
    public class UpdateUserInfo
    {
        public int Id { get; set;}
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    
    
    
    
    [HttpPost("updateUser")]
    public IActionResult UpdateUser([FromBody] UpdateUserInfo updatedUser)
    {
        var user = _aspNetCoreNTierDbContext.USERS.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (updatedUser == null)
        {
            return BadRequest("The updated user object cannot be null.");
        }
        if (user == null)
        {
            return NotFound($"User with ID {updatedUser.Id} not found.");
        }
        // Update User data
        user.Username = updatedUser.Username;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password; 
        _aspNetCoreNTierDbContext.SaveChanges();

        var updatedMessage = $"User {user.Id} data updated successfully";
        return new JsonResult(user);

    }


}
