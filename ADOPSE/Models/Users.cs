using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADOPSE.Models;

public class Users
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Role { get; set; }="Student";
    public bool Suspend { get; set; }=false;
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id},{nameof(Username)}: {Username}, {nameof(Email)}: {Email},{nameof(Password)}: {Password}, {nameof(Role)}: {Role}, {nameof(Suspend)}: {Suspend}";
    }
}