using MedicosMovil.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicosMovil.Api.Repositories;

public class EspecialidadRepository
{
    private readonly AppDbContext _db;

    public EspecialidadRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsAsync(int id)
        => _db.Especialidades.AnyAsync(e => e.IdEspecialidad == id);

    public Task<List<Models.Especialidad>> GetAllAsync()
        => _db.Especialidades.OrderBy(e => e.NombreEspecialidad).ToListAsync();
}