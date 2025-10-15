using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Services;
using HotelReservation.Domain;


namespace HotelReservation.Backend.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
private readonly ReservaService _service;


public ReservasController(ReservaService service)
{
_service = service;
}


[HttpPost]
public IActionResult CrearReserva([FromBody] CrearReservaDTO dto)
{
try
{
var id = _service.CrearReserva(dto);
return Created($"/api/reservas/{id}", new { id });
}
catch (Exception ex)
{
return BadRequest(new { error = ex.Message });
}
}


[HttpGet]
public IActionResult ListarReservas()
{
var reservas = _service.ObtenerReservas();
return Ok(reservas);
}
}
}