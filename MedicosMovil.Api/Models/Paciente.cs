namespace MedicosMovil.Api.Models;

public class Paciente
{
    public long IdPaciente { get; set; }
    public long? IdUsuario { get; set; }
    public Usuario? Usuario { get; set; }
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
}