using HotelReservation.Application.Base.Result;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Extensions
{
    public static class OperationResultExtensions
    {
        public static IActionResult ToActionResult(this OperationResult result)
        {
            var response = new
            {
                success = result.Success,
                message = result.Message
            };

            return result.Success
                ? new OkObjectResult(response)
                : new BadRequestObjectResult(response);
        }

        public static IActionResult ToActionResult<T>(this OperationResult<T> result)
        {
            var response = new
            {
                success = result.Success,
                message = result.Message,
                data = result.Data
            };

            if (result.Success)
                return new OkObjectResult(response);

            if (!string.IsNullOrEmpty(result.Message) &&
                result.Message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase))
            {
                return new NotFoundObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }
    }
}
