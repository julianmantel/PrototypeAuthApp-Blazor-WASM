using System;
using System.Collections.Generic;
using AuthRoleApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace AuthRoleApp.Data;

public partial class AutentificacionContext : DbContext
{
    public AutentificacionContext()
    {
    }

    public AutentificacionContext(DbContextOptions<AutentificacionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Correo)
                .HasMaxLength(200)
                .HasColumnName("correo");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(200)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
            entity.Property(e => e.Rol)
                .HasMaxLength(200)
                .HasColumnName("rol");
            entity.Property(e => e.Token)
                .HasColumnType("character varying")
                .HasColumnName("token");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
