using System.ComponentModel.DataAnnotations;

namespace MedicosMovil.Api.DTOs;

public class CrearPacienteDto
{
    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    [MaxLength(15, ErrorMessage = "La matrícula no puede superar 15 caracteres.")]
    public string Matricula { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [MaxLength(100)]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Contrasena { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(60)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
    [MaxLength(60)]
    public string ApellidoPaterno { get; set; } = null!;

    [MaxLength(60)]
    public string? ApellidoMaterno { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    [MaxLength(1)]
    public string Sexo { get; set; } = null!;

    [MaxLength(3)]
    public string? TipoSangre { get; set; }

    [MaxLength(15)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La carrera es obligatoria.")]
    [MaxLength(80)]
    public string Carrera { get; set; } = null!;

    [Required(ErrorMessage = "El semestre es obligatorio.")]
    [Range(1, 12, ErrorMessage = "El semestre debe estar entre 1 y 12.")]
    public short Semestre { get; set; }
}

public class ActualizarPacienteDto
{
    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    [MaxLength(15)]
    public string Matricula { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(60)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
    [MaxLength(60)]
    public string ApellidoPaterno { get; set; } = null!;

    [MaxLength(60)]
    public string? ApellidoMaterno { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    [MaxLength(1)]
    public string Sexo { get; set; } = null!;

    [MaxLength(3)]
    public string? TipoSangre { get; set; }

    [MaxLength(15)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La carrera es obligatoria.")]
    [MaxLength(80)]
    public string Carrera { get; set; } = null!;

    [Required(ErrorMessage = "El semestre es obligatorio.")]
    [Range(1, 12, ErrorMessage = "El semestre debe estar entre 1 y 12.")]
    public short Semestre { get; set; }
}

public class PacienteDto
{
    public long IdPaciente { get; set; }
    public long? IdUsuario { get; set; }
    public string Matricula { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string ApellidoPaterno { get; set; } = null!;
    public string? ApellidoMaterno { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    public string Sexo { get; set; } = null!;
    public string? TipoSangre { get; set; }
    public string? Telefono { get; set; }
    public string Carrera { get; set; } = null!;
    public short Semestre { get; set; }
    public string? Correo { get; set; }
    public string Estado { get; set; } = null!;
}