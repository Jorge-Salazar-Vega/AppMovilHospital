using System.ComponentModel.DataAnnotations;
using MedicosMovil.Api.Models;

namespace MedicosMovil.Api.DTOs;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Correo { get; set; } = null!;

    [Required, MinLength(6)]
    public string Contrasena { get; set; } = null!;

    [Required]
    public RolUsuario Rol { get; set; }

    public DatosPacienteDto? Paciente { get; set; }
    public DatosMedicoDto? Medico { get; set; }
}

public class DatosPacienteDto
{
    [Required] public string Matricula { get; set; } = null!;
    [Required] public string Nombre { get; set; } = null!;
    [Required] public string ApellidoPaterno { get; set; } = null!;
    public string? ApellidoMaterno { get; set; }
    [Required] public DateOnly FechaNacimiento { get; set; }
    [Required] public string Sexo { get; set; } = null!;
    public string? TipoSangre { get; set; }
    public string? Telefono { get; set; }
    [Required] public string Carrera { get; set; } = null!;
    [Range(1, 14)] public short Semestre { get; set; }
}

public class DatosMedicoDto
{
    [Required] public int IdEspecialidad { get; set; }
    [Required] public string CedulaProfesional { get; set; } = null!;
    [Required] public string Nombre { get; set; } = null!;
    [Required] public string ApellidoPaterno { get; set; } = null!;
    public string? ApellidoMaterno { get; set; }
    [Required] public string Telefono { get; set; } = null!;
}

public class LoginDto
{
    [Required, EmailAddress]
    public string Correo { get; set; } = null!;

    [Required]
    public string Contrasena { get; set; } = null!;
}

public class AuthResponseDto
{
    public long IdUsuario { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expira { get; set; }
    public string Correo { get; set; } = null!;
    public string Rol { get; set; } = null!;
}

public class JwtSettings
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpireMinutes { get; set; }
}