using BJJMemory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BJJMemory.Infrastructure.DataAccess;

public class BJJMemoryDbContext : DbContext
{
    public BJJMemoryDbContext(DbContextOptions<BJJMemoryDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(user => user.Email)
            .IsUnique();
    }
}
