using System.ComponentModel.DataAnnotations.Schema;

namespace ADOPSE.Models;

public class ModuleType
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
}