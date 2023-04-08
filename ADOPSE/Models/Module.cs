using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Models;

public class Module
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string  Name { get; set; }
    public string  Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime Completed { get; set; }
    public Lecturer Leader { get; set; }
    public string  GoogleCalendarID { get; set; }
    public int Price { get; set; }
    public int Rating { get; set; }
    public SubCategory SubCategory { get; set; }
    public Difficulty Difficulty { get; set; }
    public ModuleType ModuleType { get; set; }
    
}