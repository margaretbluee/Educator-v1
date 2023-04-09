using System.ComponentModel.DataAnnotations.Schema;

namespace ADOPSE.Models;

public class SubCategory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public Category parent { get; set; }
}