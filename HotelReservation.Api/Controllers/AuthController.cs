using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [Authorize(Roles = "Usuario")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
    }
}
