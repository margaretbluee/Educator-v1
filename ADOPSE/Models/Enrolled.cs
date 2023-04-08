using System.ComponentModel.DataAnnotations.Schema;

namespace ADOPSE.Models;

public class Enrolled
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Student Student { get; set; }
    public Module Module { get; set; }
}