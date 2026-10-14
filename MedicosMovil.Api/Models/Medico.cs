namespace MedicosMovil.Api.Models;

public class Medico
{
    public long IdMedico { get; set; }
    public long? IdUsuario { get; set; }
    public Usuario? Usuario { get; set; }
    public int IdEspecialidad { get; set; }
    public Especialidad Especialidad { get; set; } = null!;
    public string CedulaProfesional { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string ApellidoPaterno { get; set; } = null!;
    public string? ApellidoMaterno { get; set; }
    public string Telefono { get; set; } = null!;
}