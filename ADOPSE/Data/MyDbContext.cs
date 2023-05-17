using ADOPSE.Models;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Category { get; set; }
    public DbSet<SubCategory> SubCategory { get; set; }
    public DbSet<Difficulty> Difficulty { get; set; }
    public DbSet<Enrolled> Enrolled { get; set; }
    public DbSet<Event> Event { get; set; }
    public DbSet<Lecturer> Lecturer { get; set; }
    public DbSet<Module> Module { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<ModuleType> ModuleType { get; set; }
}