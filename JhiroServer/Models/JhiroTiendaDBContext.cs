using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JhiroServer.Models
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

        public virtual DbSet<Carrito> Carritos { get; set; } = null!;
        public virtual DbSet<Categoria> Categorias { get; set; } = null!;
        public virtual DbSet<Inventario> Inventarios { get; set; } = null!;
        public virtual DbSet<Orden> Ordens { get; set; } = null!;
        public virtual DbSet<OrdenProducto> OrdenProductos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        public virtual DbSet<CarritoSPResult>CarritoSPResults { get; set; } = null!;

         public virtual DbSet<EliminarProductoCarritoResult>EliminarProductoCarritoResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EliminarProductoCarritoResult>().HasNoKey();
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.ToTable("Carrito");

                entity.HasIndex(e => new { e.UsuarioId, e.ProductoId }, "UQ_Carrito")
                    .IsUnique();

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Carrito__Product__5165187F");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Carrito__Usuario__5070F446");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.Publicidad)
                    .HasMaxLength(255)
                    .IsUnicode(false);
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
                    .HasConstraintName("FK__Inventari__Produ__3E52440B");
            });

            modelBuilder.Entity<Orden>(entity =>
            {
                entity.ToTable("Orden");

                entity.Property(e => e.CorreoElectronico).HasMaxLength(255);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrdenProducto>(entity =>
            {
                entity.ToTable("OrdenProducto");

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Orden)
                    .WithMany(p => p.OrdenProductos)
                    .HasForeignKey(d => d.OrdenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrdenProd__Orden__59FA5E80");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.ImagenUrl).HasMaxLength(255);

                entity.Property(e => e.Nombre).HasMaxLength(150);

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK__Productos__Categ__3F466844");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Correo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
