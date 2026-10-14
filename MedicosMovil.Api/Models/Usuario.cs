namespace MedicosMovil.Api.Models;

public class Usuario
{
    public long IdUsuario { get; set; }
    public string Correo { get; set; } = null!;
    public string ContrasenaHash { get; set; } = null!;
    public RolUsuario Rol { get; set; }
    public DateTime FechaRegistro { get; set; }
    public EstadoUsuario Estado { get; set; }

    public Paciente? Paciente { get; set; }
    public Medico? Medico { get; set; }
}