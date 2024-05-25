using System.ComponentModel.DataAnnotations.Schema;

namespace ADOPSE.Models;

public class Enrolled
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UsersId { get; set; }
    public Users USERS { get; set; }
    public int ModuleId { get; set; }
    public Module Module { get; set; }
    public string? EventId { get; set; } = string.Empty;
    public bool IsChecked { get; set; } = true;
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Users)}: {USERS}, {nameof(Module)}: {Module}";
    }
}