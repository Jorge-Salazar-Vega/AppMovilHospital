using MedicosMovil.Api.Data;
using MedicosMovil.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicosMovil.Api.Services;

public class UsuarioService
{
    private readonly AppDbContext _db;

    public UsuarioService(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExisteCorreoAsync(string correo)
        => _db.Usuarios.AnyAsync(u => u.Correo == correo);

    public Usuario CrearUsuario(string correo, string contrasena, RolUsuario rol)
        => new()
        {
            Correo = correo.Trim(),
            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(contrasena),
            Rol = rol,
            Estado = EstadoUsuario.Activo,
            FechaRegistro = DateTime.UtcNow
        };
}