using BJJMemory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BJJMemory.Infrastructure.DataAccess;

public class BJJMemoryDbContext : DbContext
{
    public BJJMemoryDbContext(DbContextOptions<BJJMemoryDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Posicao> Posicoes { get; set; }

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

        modelBuilder.Entity<Posicao>(entity =>
        {
            entity.HasIndex(posicao => new { posicao.UsuarioId, posicao.Titulo });
            entity.HasIndex(posicao => posicao.CategoriaId);

            entity.Property(posicao => posicao.Titulo)
                .IsRequired();

            entity.Property(posicao => posicao.Descricao)
                .IsRequired();

            entity.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(posicao => posicao.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Categoria>()
                .WithMany()
                .HasForeignKey(posicao => posicao.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
