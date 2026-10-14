using MedicosMovil.Api.Data;
using MedicosMovil.Api.DTOs;
using MedicosMovil.Api.Models;
using MedicosMovil.Api.Repositories;

namespace MedicosMovil.Api.Services;

public class PacienteService
{
    private readonly AppDbContext _db;
    private readonly PacienteRepository _pacienteRepository;
    private readonly UsuarioService _usuarioService;
    private readonly UnitOfWork _unitOfWork;

    public PacienteService(
        AppDbContext db,
        PacienteRepository pacienteRepository,
        UsuarioService usuarioService,
        UnitOfWork unitOfWork)
    {
        _db = db;
        _pacienteRepository = pacienteRepository;
        _usuarioService = usuarioService;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PacienteDto>> GetAllAsync()
    {
        var pacientes = await _pacienteRepository.GetAllAsync();
        return pacientes.Select(ToDto).ToList();
    }

    public async Task<PacienteDto?> GetByIdAsync(long id)
    {
        var paciente = await _pacienteRepository.GetByIdAsync(id);
        return paciente is null ? null : ToDto(paciente);
    }

    public async Task<ServiceResult<PacienteDto>> RegistrarAsync(CrearPacienteDto dto)
    {
        var error = await ValidarRegistroAsync(dto);
        if (error is not null)
        {
            return ServiceResult<PacienteDto>.Error(error);
        }

        var usuario = _usuarioService.CrearUsuario(dto.Correo, dto.Contrasena, RolUsuario.Paciente);

        var paciente = new Paciente
        {
            Matricula = dto.Matricula.Trim(),
            Nombre = dto.Nombre.Trim(),
            ApellidoPaterno = dto.ApellidoPaterno.Trim(),
            ApellidoMaterno = string.IsNullOrWhiteSpace(dto.ApellidoMaterno) ? null : dto.ApellidoMaterno.Trim(),
            FechaNacimiento = dto.FechaNacimiento,
            Sexo = dto.Sexo.ToUpperInvariant(),
            TipoSangre = string.IsNullOrWhiteSpace(dto.TipoSangre) ? null : dto.TipoSangre.Trim().ToUpperInvariant(),
            Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim(),
            Carrera = dto.Carrera.Trim(),
            Semestre = dto.Semestre,
            Usuario = usuario
        };

        _db.Pacientes.Add(paciente);
        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<PacienteDto>.Success(ToDto(paciente));
    }

    public async Task<ServiceResult<PacienteDto>> ActualizarAsync(long id, ActualizarPacienteDto dto)
    {
        var paciente = await _pacienteRepository.GetByIdAsync(id);
        if (paciente is null)
        {
            return ServiceResult<PacienteDto>.NotFount();
        }

        if (dto.Sexo is not ("M" or "F"))
        {
            return ServiceResult<PacienteDto>.Error("El sexo debe ser 'M' o 'F'.");
        }

        if (await _pacienteRepository.MatriculaExistsExceptAsync(dto.Matricula, id))
        {
            return ServiceResult<PacienteDto>.Error("Ya existe un paciente con esa matrícula.");
        }

        paciente.Matricula = dto.Matricula.Trim();
        paciente.Nombre = dto.Nombre.Trim();
        paciente.ApellidoPaterno = dto.ApellidoPaterno.Trim();
        paciente.ApellidoMaterno = string.IsNullOrWhiteSpace(dto.ApellidoMaterno) ? null : dto.ApellidoMaterno.Trim();
        paciente.FechaNacimiento = dto.FechaNacimiento;
        paciente.Sexo = dto.Sexo.ToUpperInvariant();
        paciente.TipoSangre = string.IsNullOrWhiteSpace(dto.TipoSangre) ? null : dto.TipoSangre.Trim().ToUpperInvariant();
        paciente.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
        paciente.Carrera = dto.Carrera.Trim();
        paciente.Semestre = dto.Semestre;

        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<PacienteDto>.Success(ToDto(paciente));
    }

    private async Task<string?> ValidarRegistroAsync(CrearPacienteDto dto)
    {
        if (dto.Sexo is not ("M" or "F"))
        {
            return "El sexo debe ser 'M' o 'F'.";
        }

        if (await _pacienteRepository.MatriculaExistsAsync(dto.Matricula))
        {
            return "Ya existe un paciente con esa matrícula.";
        }

        if (await _usuarioService.ExisteCorreoAsync(dto.Correo))
        {
            return "El correo ya está registrado.";
        }

        return null;
    }

    private static PacienteDto ToDto(Paciente p) => new()
    {
        IdPaciente = p.IdPaciente,
        IdUsuario = p.IdUsuario,
        Matricula = p.Matricula,
        Nombre = p.Nombre,
        ApellidoPaterno = p.ApellidoPaterno,
        ApellidoMaterno = p.ApellidoMaterno,
        FechaNacimiento = p.FechaNacimiento,
        Sexo = p.Sexo,
        TipoSangre = p.TipoSangre,
        Telefono = p.Telefono,
        Carrera = p.Carrera,
        Semestre = p.Semestre,
        Correo = p.Usuario?.Correo,
        Estado = (p.Usuario?.Estado ?? EstadoUsuario.Activo).ToString().ToLowerInvariant()
    };
}