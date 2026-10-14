using MedicosMovil.Api.Data;
using MedicosMovil.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicosMovil.Api.Repositories;

public class PacienteRepository
{
    private readonly AppDbContext _db;

    public PacienteRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Paciente>> GetAllAsync()
        => _db.Pacientes
            .Include(p => p.Usuario)
            .OrderBy(p => p.ApellidoPaterno)
            .ThenBy(p => p.ApellidoMaterno)
            .ThenBy(p => p.Nombre)
            .ToListAsync();

    public Task<Paciente?> GetByIdAsync(long id)
        => _db.Pacientes
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.IdPaciente == id);

    public Task<bool> MatriculaExistsAsync(string matricula)
        => _db.Pacientes.AnyAsync(p => p.Matricula == matricula);

    public Task<bool> MatriculaExistsExceptAsync(string matricula, long id)
        => _db.Pacientes.AnyAsync(p => p.Matricula == matricula && p.IdPaciente != id);
}