using MedicosMovil.Api.DTOs;
using MedicosMovil.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicosMovil.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var (respuesta, error) = await _auth.RegistrarAsync(dto);
        if (error is not null) return BadRequest(new { mensaje = error });
        return Ok(respuesta);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var respuesta = await _auth.LoginAsync(dto);
        if (respuesta is null) return Unauthorized(new { mensaje = "Credenciales inválidas o usuario inactivo." });
        return Ok(respuesta);
    }
}
