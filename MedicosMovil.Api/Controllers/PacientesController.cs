using MedicosMovil.Api.DTOs;
using MedicosMovil.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicosMovil.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // todo el controller exige token válido
public class PacientesController : ControllerBase
{
    private readonly PacienteService _pacienteService;

    public PacientesController(PacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    // Médico o administrador
    [HttpGet]
    [Authorize(Roles = "Medico,Admin")]
    public async Task<ActionResult<ApiResponse<List<PacienteDto>>>> GetPacientes()
    {
        var pacientes = await _pacienteService.GetAllAsync();
        return Ok(ApiResponse<List<PacienteDto>>.Ok(pacientes, "Pacientes consultados correctamente."));
    }

    // Médico o administrador
    [HttpGet("{id:long}")]
    [Authorize(Roles = "Medico,Admin")]
    public async Task<ActionResult<ApiResponse<PacienteDto>>> GetPaciente(long id)
    {
        var paciente = await _pacienteService.GetByIdAsync(id);
        if (paciente is null)
        {
            return NotFound(ApiResponse<PacienteDto>.Error("No existe el paciente."));
        }

        return Ok(ApiResponse<PacienteDto>.Ok(paciente, "Paciente consultado correctamente."));
    }

    // Solo administrador
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<PacienteDto>>> Registrar(CrearPacienteDto dto)
    {
        var result = await _pacienteService.RegistrarAsync(dto);
        if (!result.IsSuccess)
        {
            return Conflict(ApiResponse<PacienteDto>.Error(result.ErrorMessage!));
        }

        return CreatedAtAction(
            nameof(GetPaciente),
            new { id = result.Data!.IdPaciente },
            ApiResponse<PacienteDto>.Ok(result.Data, "Paciente registrado correctamente."));
    }

    // Solo administrador
    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<PacienteDto>>> Actualizar(long id, ActualizarPacienteDto dto)
    {
        var result = await _pacienteService.ActualizarAsync(id, dto);
        if (result.NotFound)
        {
            return NotFound(ApiResponse<PacienteDto>.Error("No existe el paciente."));
        }

        if (!result.IsSuccess)
        {
            return Conflict(ApiResponse<PacienteDto>.Error(result.ErrorMessage!));
        }

        return Ok(ApiResponse<PacienteDto>.Ok(result.Data!, "Paciente actualizado correctamente."));
    }
}