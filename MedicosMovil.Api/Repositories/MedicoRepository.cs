using MedicosMovil.Api.Data;
using MedicosMovil.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicosMovil.Api.Repositories;

public class MedicoRepository
{
    private readonly AppDbContext _db;

    public MedicoRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Medico>> GetAllAsync()
        => _db.Medicos
            .Include(m => m.Especialidad)
            .Include(m => m.Usuario)
            .OrderBy(m => m.ApellidoPaterno)
            .ThenBy(m => m.ApellidoMaterno)
            .ThenBy(m => m.Nombre)
            .ToListAsync();

    public Task<Medico?> GetByIdAsync(long id)
        => _db.Medicos
            .Include(m => m.Especialidad)
            .Include(m => m.Usuario)
            .FirstOrDefaultAsync(m => m.IdMedico == id);

    public Task<bool> CedulaExistsAsync(string cedula)
        => _db.Medicos.AnyAsync(m => m.CedulaProfesional == cedula);

    public Task<bool> CedulaExistsExceptAsync(string cedula, long id)
        => _db.Medicos.AnyAsync(m => m.CedulaProfesional == cedula && m.IdMedico != id);
}