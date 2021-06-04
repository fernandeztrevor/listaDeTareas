using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ListaDeTareas.Models
{
    public partial class ListaDeTareasCTX : DbContext
    {
        public ListaDeTareasCTX()
        {
        }

        public ListaDeTareasCTX(DbContextOptions<ListaDeTareasCTX> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Tarea> Tareas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-L5TBTJC\\SQLEXPRESS;Database=ListaDeTareas;Trusted_Connection=False;User ID=sa;Password=superadministrador;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Roles__3C872F76371DCC5F");
            });

            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(e => e.IdTarea)
                    .HasName("PK__Tareas__756A5402C3009A35");

                entity.HasOne(d => d.IdAsignadoNavigation)
                    .WithMany(p => p.TareaIdAsignadoNavigations)
                    .HasForeignKey(d => d.IdAsignado)
                    .HasConstraintName("FK__Tareas__idAsigna__3C69FB99");

                entity.HasOne(d => d.IdCreadorNavigation)
                    .WithMany(p => p.TareaIdCreadorNavigations)
                    .HasForeignKey(d => d.IdCreador)
                    .HasConstraintName("FK__Tareas__idCreado__3B75D760");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__645723A68D2EA511");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Usuarios__idRol__38996AB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
