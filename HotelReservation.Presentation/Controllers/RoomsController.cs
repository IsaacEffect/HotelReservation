using HotelReservation.Application.Services;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomService _service;

        public RoomsController(RoomService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequest req)
        {
            try
            {
                var dto = await _service.CreateRoomAsync(req.Number, req.CategoryId, req.Price);
                return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
            }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dto = await _service.GetRoomAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPut("{id:guid}/price")]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] UpdatePriceRequest req)
        {
            try
            {
                await _service.UpdatePriceAsync(id, req.NewPrice);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest req)
        {
            try
            {
                await _service.ChangeStatusAsync(id, req.Status);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }
    }

    public record CreateRoomRequest(int Number, Guid CategoryId, decimal Price);
    public record UpdatePriceRequest(decimal NewPrice);
    public record ChangeStatusRequest(RoomStatus Status);
}
