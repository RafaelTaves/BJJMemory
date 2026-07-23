using BJJMemory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BJJMemory.Infrastructure.DataAccess;

public class BJJMemoryDbContext : DbContext
{
    public BJJMemoryDbContext(DbContextOptions<BJJMemoryDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasIndex(categoria => new { categoria.UsuarioId, categoria.Nome })
                .IsUnique();

            entity.Property(categoria => categoria.Nome)
                .IsRequired();

            entity.HasOne(categoria => categoria.Parent)
                .WithMany(categoria => categoria.Subcategorias)
                .HasForeignKey(categoria => categoria.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(categoria => categoria.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
