using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Interfaces.Services;

namespace HotelReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInOutController : ControllerBase
    {
        private readonly ICheckInOutService _checkInOutService;

        public CheckInOutController(ICheckInOutService checkInOutService)
        {
            _checkInOutService = checkInOutService;
        }

        [HttpPost("checkin/{reservaId}")]
        public async Task<IActionResult> RegistrarCheckIn(Guid reservaId)
        {
            await _checkInOutService.RegistrarCheckInAsync(reservaId);
            return Ok(new { mensaje = "Check-In registrado correctamente." });
        }

        [HttpPost("checkout/{reservaId}")]
        public async Task<IActionResult> RegistrarCheckOut(Guid reservaId)
        {
            await _checkInOutService.RegistrarCheckOutAsync(reservaId);
            return Ok(new { mensaje = "Check-Out registrado y factura generada correctamente." });
        }
    }
}