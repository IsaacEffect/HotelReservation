using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Services;
using HotelReservation.Application.Dtos;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _checkService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("registro/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _checkService.GetByIdAsync(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCheckInOutRequest request)
        {
            if (request == null)
                return BadRequest("Datos inválidos.");

            var updated = await _checkService.UpdateAsync(id, request);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _checkService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
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