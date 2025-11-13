using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.DTOs;

namespace HotelReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _service;
        public RoomsController(IRoomService service) => _service = service;

        [HttpGet] public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => await _service.GetByIdAsync(id) is { } r ? Ok(r) : NotFound();
        [HttpPost] public async Task<IActionResult> Post([FromBody] RoomDto dto) => CreatedAtAction(nameof(Get), new { id = (await _service.AddAsync(dto)).Id }, dto);
        [HttpPut("{id}")] public async Task<IActionResult> Put(int id, [FromBody] RoomDto dto) { if (id != dto.Id) return BadRequest(); await _service.UpdateAsync(dto); return NoContent(); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { await _service.DeleteAsync(id); return NoContent(); }
    }
}