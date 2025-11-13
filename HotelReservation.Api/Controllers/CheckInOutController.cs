using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Services;
using HotelReservation.Application.DTOs;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInOutController : ControllerBase
    {
        private readonly ICheckInOutService _checkService;
        private readonly IHistorialService _histService;

        public CheckInOutController(ICheckInOutService checkService, IHistorialService histService)
        {
            _checkService = checkService;
            _histService = histService;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CreateCheckInRequest request)
        {
            if (request == null || request.ReservaId == Guid.Empty)
                return BadRequest("ReservaId inválido.");

            var result = await _checkService.RegisterCheckInAsync(request);
            return CreatedAtAction(nameof(GetByReserva), new { reservationId = result.ReservaId }, result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CreateCheckOutRequest request)
        {
            if (request == null || request.ReservaId == Guid.Empty)
                return BadRequest("ReservaId inválido.");

            var result = await _checkService.RegisterCheckOutAsync(request);
            return Ok(result);
        }

        [HttpGet("{reservationId:guid}")]
        public async Task<IActionResult> GetByReserva(Guid reservationId)
        {
            var res = await _checkService.GetByReservaIdAsync(reservationId);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var res = await _histService.GetAllAsync();
            return Ok(res);
        }

        [HttpGet("history/client/{clientId:guid}")]
        public async Task<IActionResult> GetHistoryByClient(Guid clientId)
        {
            var res = await _histService.GetByClienteIdAsync(clientId);
            return Ok(res);
        }

        [HttpGet("history/room/{roomId:guid}")]
        public async Task<IActionResult> GetHistoryByRoom(Guid roomId)
        {
            var res = await _histService.GetByHabitacionIdAsync(roomId);
            return Ok(res);
        }
    }
}