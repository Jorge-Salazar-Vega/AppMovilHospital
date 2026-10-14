using System.ComponentModel.DataAnnotations;

namespace MedicosMovil.Api.DTOs;

public class CrearMedicoDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [MaxLength(100)]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Contrasena { get; set; } = null!;

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    public int IdEspecialidad { get; set; }

    [Required(ErrorMessage = "La cédula profesional es obligatoria.")]
    [MaxLength(20)]
    public string CedulaProfesional { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(60)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
    [MaxLength(60)]
    public string ApellidoPaterno { get; set; } = null!;

    [MaxLength(60)]
    public string? ApellidoMaterno { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [MaxLength(15)]
    public string Telefono { get; set; } = null!;
}

public class ActualizarMedicoDto
{
    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    public int IdEspecialidad { get; set; }

    [Required(ErrorMessage = "La cédula profesional es obligatoria.")]
    [MaxLength(20)]
    public string CedulaProfesional { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(60)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
    [MaxLength(60)]
    public string ApellidoPaterno { get; set; } = null!;

    [MaxLength(60)]
    public string? ApellidoMaterno { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [MaxLength(15)]
    public string Telefono { get; set; } = null!;
}

public class MedicoDto
{
    public long IdMedico { get; set; }
    public long? IdUsuario { get; set; }
    public int IdEspecialidad { get; set; }
    public string Especialidad { get; set; } = null!;
    public string CedulaProfesional { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string ApellidoPaterno { get; set; } = null!;
    public string? ApellidoMaterno { get; set; }
    public string Telefono { get; set; } = null!;
    public string? Correo { get; set; }
    public string Estado { get; set; } = null!;
}