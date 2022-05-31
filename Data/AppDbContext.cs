using Microsoft.EntityFrameworkCore;
using sadad.Models;

namespace sadad.Data;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
    {
        
    }

    public DbSet<Config>? Configs { get; set; }

}
