using ADOPSE.Models;
using Microsoft.EntityFrameworkCore;

namespace ADOPSE.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }

    public DbSet<Categorie> Categorie { get; set; }
}