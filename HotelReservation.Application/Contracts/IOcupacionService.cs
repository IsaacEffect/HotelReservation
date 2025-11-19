<<<<<<< HEAD
﻿using HotelReservation.Application.DTOs;
=======
﻿using HotelReservation.Application.Dtos;
>>>>>>> origin/develop

namespace HotelReservation.Application.Contracts
{
    public interface IOcupacionService
    {
        Task<OcupacionDiariaDto> ObtenerOcupacionDiariaAsync();
    }

}