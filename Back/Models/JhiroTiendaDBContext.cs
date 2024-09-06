using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Back.Models
{
    public partial class JhiroTiendaDBContext : DbContext
    {
        public JhiroTiendaDBContext()
        {
        }

        public JhiroTiendaDBContext(DbContextOptions<JhiroTiendaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categorias { get; set; } = null!;
        public virtual DbSet<Inventario> Inventarios { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.Nombre).HasMaxLength(100);
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.ToTable("Inventario");

                entity.Property(e => e.FechaActualizacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.ProductoId)
                    .HasConstraintName("FK__Inventari__Produ__2A4B4B5E");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.ImagenUrl).HasMaxLength(255);

                entity.Property(e => e.Nombre).HasMaxLength(150);

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK__Productos__Categ__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
