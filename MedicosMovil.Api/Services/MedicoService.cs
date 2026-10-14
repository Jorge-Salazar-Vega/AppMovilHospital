using MedicosMovil.Api.Data;
using MedicosMovil.Api.DTOs;
using MedicosMovil.Api.Models;
using MedicosMovil.Api.Repositories;

namespace MedicosMovil.Api.Services;

public class MedicoService
{
    private readonly AppDbContext _db;
    private readonly MedicoRepository _medicoRepository;
    private readonly EspecialidadRepository _especialidadRepository;
    private readonly UsuarioService _usuarioService;
    private readonly UnitOfWork _unitOfWork;

    public MedicoService(
        AppDbContext db,
        MedicoRepository medicoRepository,
        EspecialidadRepository especialidadRepository,
        UsuarioService usuarioService,
        UnitOfWork unitOfWork)
    {
        _db = db;
        _medicoRepository = medicoRepository;
        _especialidadRepository = especialidadRepository;
        _usuarioService = usuarioService;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<MedicoDto>> GetAllAsync()
    {
        var medicos = await _medicoRepository.GetAllAsync();
        return medicos.Select(ToDto).ToList();
    }

    public async Task<MedicoDto?> GetByIdAsync(long id)
    {
        var medico = await _medicoRepository.GetByIdAsync(id);
        return medico is null ? null : ToDto(medico);
    }

    public async Task<ServiceResult<MedicoDto>> RegistrarAsync(CrearMedicoDto dto)
    {        if (!await _especialidadRepository.ExistsAsync(dto.IdEspecialidad))
        {
            return ServiceResult<MedicoDto>.Error("La especialidad indicada no existe.");
        }

        if (await _medicoRepository.CedulaExistsAsync(dto.CedulaProfesional))
        {
            return ServiceResult<MedicoDto>.Error("Ya existe un médico con esa cédula profesional.");
        }

        if (await _usuarioService.ExisteCorreoAsync(dto.Correo))
        {
            return ServiceResult<MedicoDto>.Error("El correo ya está registrado.");
        }

        var usuario = _usuarioService.CrearUsuario(dto.Correo, dto.Contrasena, RolUsuario.Medico);

        var medico = new Medico
        {
            IdEspecialidad = dto.IdEspecialidad,
            CedulaProfesional = dto.CedulaProfesional.Trim(),
            Nombre = dto.Nombre.Trim(),
            ApellidoPaterno = dto.ApellidoPaterno.Trim(),
            ApellidoMaterno = string.IsNullOrWhiteSpace(dto.ApellidoMaterno) ? null : dto.ApellidoMaterno.Trim(),
            Telefono = dto.Telefono.Trim(),
            Usuario = usuario
        };

        _db.Medicos.Add(medico);
        await _unitOfWork.SaveChangesAsync();

        var creado = await _medicoRepository.GetByIdAsync(medico.IdMedico);
        return ServiceResult<MedicoDto>.Success(ToDto(creado!));
    }

    public async Task<ServiceResult<MedicoDto>> ActualizarAsync(long id, ActualizarMedicoDto dto)
    {
        var medico = await _medicoRepository.GetByIdAsync(id);
        if (medico is null)
        {
            return ServiceResult<MedicoDto>.NotFount();
        }

        if (!await _especialidadRepository.ExistsAsync(dto.IdEspecialidad))
        {
            return ServiceResult<MedicoDto>.Error("La especialidad indicada no existe.");
        }

        if (await _medicoRepository.CedulaExistsExceptAsync(dto.CedulaProfesional, id))
        {
            return ServiceResult<MedicoDto>.Error("Ya existe un médico con esa cédula profesional.");
        }

        medico.IdEspecialidad = dto.IdEspecialidad;
        medico.CedulaProfesional = dto.CedulaProfesional.Trim();
        medico.Nombre = dto.Nombre.Trim();
        medico.ApellidoPaterno = dto.ApellidoPaterno.Trim();
        medico.ApellidoMaterno = string.IsNullOrWhiteSpace(dto.ApellidoMaterno) ? null : dto.ApellidoMaterno.Trim();
        medico.Telefono = dto.Telefono.Trim();

        await _unitOfWork.SaveChangesAsync();

        var actualizado = await _medicoRepository.GetByIdAsync(id);
        return ServiceResult<MedicoDto>.Success(ToDto(actualizado!));
    }

    private static MedicoDto ToDto(Medico m) => new()
    {
        IdMedico = m.IdMedico,
        IdUsuario = m.IdUsuario,
        IdEspecialidad = m.IdEspecialidad,
        Especialidad = m.Especialidad?.NombreEspecialidad ?? string.Empty,
        CedulaProfesional = m.CedulaProfesional,
        Nombre = m.Nombre,
        ApellidoPaterno = m.ApellidoPaterno,
        ApellidoMaterno = m.ApellidoMaterno,
        Telefono = m.Telefono,
        Correo = m.Usuario?.Correo,
        Estado = (m.Usuario?.Estado ?? EstadoUsuario.Activo).ToString().ToLowerInvariant()
    };
}