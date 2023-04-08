using System.ComponentModel.DataAnnotations.Schema;

namespace ADOPSE.Models;

public class Lecturer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string  Name { get; set; }
    public string  Bio { get; set; }
    public string  Website { get; set; }
    public string  Email { get; set; }
}