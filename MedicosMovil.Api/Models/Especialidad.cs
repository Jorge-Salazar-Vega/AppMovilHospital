namespace MedicosMovil.Api.Models;

public class Especialidad
{
    public int IdEspecialidad { get; set; }
    public string NombreEspecialidad { get; set; } = null!;
    public string? Descripcion { get; set; }

    public ICollection<Medico> Medicos { get; set; } = new List<Medico>();
}