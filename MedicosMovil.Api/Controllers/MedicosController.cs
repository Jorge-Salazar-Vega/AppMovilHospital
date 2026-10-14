using MedicosMovil.Api.DTOs;
using MedicosMovil.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicosMovil.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // todo el controller exige token válido
public class MedicosController : ControllerBase
{
    private readonly MedicoService _medicoService;

    public MedicosController(MedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    // Cualquier usuario autenticado
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<MedicoDto>>>> GetMedicos()
    {
        var medicos = await _medicoService.GetAllAsync();
        return Ok(ApiResponse<List<MedicoDto>>.Ok(medicos, "Médicos consultados correctamente."));
    }

    // Cualquier usuario autenticado
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<MedicoDto>>> GetMedico(long id)
    {
        var medico = await _medicoService.GetByIdAsync(id);
        if (medico is null)
        {
            return NotFound(ApiResponse<MedicoDto>.Error("No existe el médico."));
        }

        return Ok(ApiResponse<MedicoDto>.Ok(medico, "Médico consultado correctamente."));
    }

    // Solo administrador
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<MedicoDto>>> Registrar(CrearMedicoDto dto)
    {
        var result = await _medicoService.RegistrarAsync(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponse<MedicoDto>.Error(result.ErrorMessage!));
        }

        return CreatedAtAction(
            nameof(GetMedico),
            new { id = result.Data!.IdMedico },
            ApiResponse<MedicoDto>.Ok(result.Data, "Médico registrado correctamente."));
    }

    // Solo administrador
    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<MedicoDto>>> Actualizar(long id, ActualizarMedicoDto dto)
    {
        var result = await _medicoService.ActualizarAsync(id, dto);
        if (result.NotFound)
        {
            return NotFound(ApiResponse<MedicoDto>.Error("No existe el médico."));
        }

        if (!result.IsSuccess)
        {
            return Conflict(ApiResponse<MedicoDto>.Error(result.ErrorMessage!));
        }

        return Ok(ApiResponse<MedicoDto>.Ok(result.Data!, "Médico actualizado correctamente."));
    }
}