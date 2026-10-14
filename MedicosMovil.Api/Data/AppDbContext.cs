using MedicosMovil.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicosMovil.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Especialidad> Especialidades => Set<Especialidad>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Medico> Medicos => Set<Medico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuario");
            entity.HasKey(u => u.IdUsuario);
            entity.Property(u => u.IdUsuario).HasColumnName("id_usuario");
            entity.Property(u => u.Correo).HasColumnName("correo").HasMaxLength(100).IsRequired();
            entity.Property(u => u.ContrasenaHash).HasColumnName("contrasena_hash").HasMaxLength(255).IsRequired();
            entity.Property(u => u.Rol).HasColumnName("rol");
            entity.Property(u => u.FechaRegistro).HasColumnName("fecha_registro").HasDefaultValueSql("now()");
            entity.Property(u => u.Estado).HasColumnName("estado");
            entity.HasIndex(u => u.Correo).IsUnique();
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.ToTable("especialidad");
            entity.HasKey(e => e.IdEspecialidad);
            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.NombreEspecialidad).HasColumnName("nombre_especialidad").HasMaxLength(60).IsRequired();
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
            entity.HasIndex(e => e.NombreEspecialidad).IsUnique();
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("paciente");
            entity.HasKey(p => p.IdPaciente);
            entity.Property(p => p.IdPaciente).HasColumnName("id_paciente");
            entity.Property(p => p.IdUsuario).HasColumnName("id_usuario");
            entity.Property(p => p.Matricula).HasColumnName("matricula").HasMaxLength(15).IsRequired();
            entity.Property(p => p.Nombre).HasColumnName("nombre").HasMaxLength(60).IsRequired();
            entity.Property(p => p.ApellidoPaterno).HasColumnName("apellido_paterno").HasMaxLength(60).IsRequired();
            entity.Property(p => p.ApellidoMaterno).HasColumnName("apellido_materno").HasMaxLength(60);
            entity.Property(p => p.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(p => p.Sexo).HasColumnName("sexo").HasColumnType("char(1)").IsRequired();
            entity.Property(p => p.TipoSangre).HasColumnName("tipo_sangre").HasMaxLength(3);
            entity.Property(p => p.Telefono).HasColumnName("telefono").HasMaxLength(15);
            entity.Property(p => p.Carrera).HasColumnName("carrera").HasMaxLength(80).IsRequired();
            entity.Property(p => p.Semestre).HasColumnName("semestre");
            entity.HasIndex(p => p.Matricula).IsUnique();

            entity.HasOne(p => p.Usuario)
                .WithOne(u => u.Paciente)
                .HasForeignKey<Paciente>(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.ToTable("medico");
            entity.HasKey(m => m.IdMedico);
            entity.Property(m => m.IdMedico).HasColumnName("id_medico");
            entity.Property(m => m.IdUsuario).HasColumnName("id_usuario");
            entity.Property(m => m.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(m => m.CedulaProfesional).HasColumnName("cedula_profesional").HasMaxLength(20).IsRequired();
            entity.Property(m => m.Nombre).HasColumnName("nombre").HasMaxLength(60).IsRequired();
            entity.Property(m => m.ApellidoPaterno).HasColumnName("apellido_paterno").HasMaxLength(60).IsRequired();
            entity.Property(m => m.ApellidoMaterno).HasColumnName("apellido_materno").HasMaxLength(60);
            entity.Property(m => m.Telefono).HasColumnName("telefono").HasMaxLength(15).IsRequired();
            entity.HasIndex(m => m.CedulaProfesional).IsUnique();
            entity.HasIndex(m => m.IdEspecialidad);

            entity.HasOne(m => m.Usuario)
                .WithOne(u => u.Medico)
                .HasForeignKey<Medico>(m => m.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(m => m.Especialidad)
                .WithMany(e => e.Medicos)
                .HasForeignKey(m => m.IdEspecialidad)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}