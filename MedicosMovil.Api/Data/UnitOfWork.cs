namespace MedicosMovil.Api.Data;

public class UnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}