using MedicosMovil.Api.Data;
using MedicosMovil.Api.DTOs;
using MedicosMovil.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicosMovil.Api.Services;

public interface IAuthService
{
    Task<(AuthResponseDto? Respuesta, string? Error)> RegistrarAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokens;

    public AuthService(AppDbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    public async Task<(AuthResponseDto?, string?)> RegistrarAsync(RegisterDto dto)
    {
       
        if (dto.Rol == RolUsuario.Admin)
            return (null, "No se puede registrar un administrador desde este endpoint.");

      
        if (dto.Rol == RolUsuario.Paciente && dto.Paciente is null)
            return (null, "Para registrar un paciente debes enviar sus datos en 'paciente'.");

        if (dto.Rol == RolUsuario.Medico && dto.Medico is null)
            return (null, "Para registrar un médico debes enviar sus datos en 'medico'.");

        var correo = dto.Correo.Trim().ToLower();

        if (await _db.Set<Usuario>().AnyAsync(u => u.Correo == correo))
            return (null, "El correo ya está registrado.");

        var usuario = new Usuario
        {
            Correo = correo,
            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
            Rol = dto.Rol,
            FechaRegistro = DateTime.UtcNow,
            Estado = EstadoUsuario.Activo
        };

        if (dto.Rol == RolUsuario.Paciente)
        {
            var p = dto.Paciente!;

            if (await _db.Set<Paciente>().AnyAsync(x => x.Matricula == p.Matricula))
                return (null, "La matrícula ya está registrada.");

            usuario.Paciente = new Paciente
            {
                Matricula = p.Matricula,
                Nombre = p.Nombre,
                ApellidoPaterno = p.ApellidoPaterno,
                ApellidoMaterno = p.ApellidoMaterno,
                FechaNacimiento = p.FechaNacimiento,
                Sexo = p.Sexo,
                TipoSangre = p.TipoSangre,
                Telefono = p.Telefono,
                Carrera = p.Carrera,
                Semestre = p.Semestre
            };
        }
        else if (dto.Rol == RolUsuario.Medico)
        {
            var m = dto.Medico!;

            if (!await _db.Set<Especialidad>().AnyAsync(e => e.IdEspecialidad == m.IdEspecialidad))
                return (null, "La especialidad indicada no existe.");

            if (await _db.Set<Medico>().AnyAsync(x => x.CedulaProfesional == m.CedulaProfesional))
                return (null, "La cédula profesional ya está registrada.");

            usuario.Medico = new Medico
            {
                IdEspecialidad = m.IdEspecialidad,
                CedulaProfesional = m.CedulaProfesional,
                Nombre = m.Nombre,
                ApellidoPaterno = m.ApellidoPaterno,
                ApellidoMaterno = m.ApellidoMaterno,
                Telefono = m.Telefono
            };
        }

        _db.Add(usuario);              
        await _db.SaveChangesAsync();   

        return (CrearRespuesta(usuario), null);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLower();
        var usuario = await _db.Set<Usuario>().FirstOrDefaultAsync(u => u.Correo == correo);

        if (usuario is null ||
            usuario.Estado != EstadoUsuario.Activo ||
            !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.ContrasenaHash))
            return null;

        return CrearRespuesta(usuario);
    }

    private AuthResponseDto CrearRespuesta(Usuario u)
    {
        var (token, expira) = _tokens.Generar(u);
        return new AuthResponseDto
        {
            IdUsuario = u.IdUsuario,
            Token = token,
            Expira = expira,
            Correo = u.Correo,
            Rol = u.Rol.ToString()
        };
    }
}
