using System.ComponentModel.DataAnnotations.Schema;
using ADOPSE.Data;
using ADOPSE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Models;

public class Module
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    private readonly MyDbContext _aspNetCoreNTierDbContext;

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime Completed { get; set; }

    public int leaderId { get; set; }

    // public Lecturer Leader { get; set; }

    public string? LeaderName
    {
        get
        {
            var lecturer = _aspNetCoreNTierDbContext.Lecturer.Find(leaderId);
            return lecturer?.Name;
        }
    }


    public string GoogleCalendarID { get; set; }
    public int Price { get; set; }
    public int Rating { get; set; }
    public int SubCategoryId { get; set; }
    public int DifficultyId { get; set; }

    public int ModuleTypeId { get; set; }

    // public ModuleType ModuleType { get; set; }



}