using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Enums;
using SGE.Dominio.Usuarios;
using System.Collections.Generic;

namespace SGE.Infraestructura.Datos;

public class SgeContext : DbContext
{
    public SgeContext(DbContextOptions<SgeContext> options) : base(options)
    {
    }

    public DbSet<Expediente> Expedientes { get; set; } = null!;
    public DbSet<Tramite> Tramites { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Mapeo de Expediente con su Value Object (Caratula)
        modelBuilder.Entity<Expediente>(builder =>
        {
            builder.ToTable("Expedientes");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever(); // Bloqueo de ID en BD (Pura Arquitectura Limpia)
            
            builder.ComplexProperty(e => e.Caratula, caratula =>
            {
                caratula.Property(valor => valor.Texto)
                    .HasColumnName("Caratula")
                    .IsRequired();
            });
        });

        // 2. Mapeo de Tramite con su Value Object (Contenido) y Relación
        modelBuilder.Entity<Tramite>(builder =>
        {
            builder.ToTable("Tramites");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.ComplexProperty(t => t.Contenido, contenido =>
            {
                contenido.Property(valor => valor.Texto)
                    .HasColumnName("Contenido")
                    .IsRequired();
            });

            builder.HasOne<Expediente>()
                .WithMany()
                .HasForeignKey(t => t.ExpedienteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 3. Mapeo de Tu Usuario Impecable
        modelBuilder.Entity<Usuario>(builder =>
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedNever();
            builder.HasIndex(u => u.CorreoElectronico).IsUnique();

            // Magia de EF Core: Mapeamos tu campo privado _permisos a un JSON en la columna "Permisos"
            builder.Property<HashSet<Permiso>>("_permisos")
                .HasColumnName("Permisos")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<HashSet<Permiso>>(v, (JsonSerializerOptions?)null) ?? new HashSet<Permiso>()
                );
        });
    }
}
