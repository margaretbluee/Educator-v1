using System.ComponentModel.DataAnnotations.Schema;
using ADOPSE.Data;
using ADOPSE.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ADOPSE.Models;

public class Module
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime Completed { get; set; }
    public int leaderId { get; set; }

    [ForeignKey("leaderId ")]
    [JsonIgnore]
    public virtual Lecturer Lecturer { get; set; }

    [NotMapped]
    public string? LecturerName
    {
        get { return Lecturer?.Name; }
        set { } // Empty setter to satisfy the entity framework
    }

    public string GoogleCalendarID { get; set; }
    public int Price { get; set; }
    public int Rating { get; set; }
    public int SubCategoryId { get; set; }
    public int DifficultyId { get; set; }
    public string? DifficultyName { get; set; }

    public int ModuleTypeId { get; set; }

    [ForeignKey("ModuleTypeId")]
    [JsonIgnore]
    public virtual ModuleType ModuleType { get; set; }

    [NotMapped]
    public string? ModuleTypeName
    {
        get { return ModuleType?.Name; }
        set { } // Empty setter to satisfy the entity framework
    }
    
    

}