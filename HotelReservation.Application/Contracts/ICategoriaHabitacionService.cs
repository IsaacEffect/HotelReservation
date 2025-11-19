<<<<<<< HEAD
using HotelReservation.Application.Base.Result;
=======
﻿using HotelReservation.Application.Base.Result;
>>>>>>> origin/develop
using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface ICategoriaHabitacionService
    {
        Task<OperationResult<IEnumerable<ObtenerCategoriaDto>>> GetAllAsync();
        Task<OperationResult<ObtenerCategoriaDto>> GetByIdAsync(Guid id);
        Task<OperationResult<Guid>> AddAsync(InsertarCategoriaDto dto);
        Task<OperationResult> UpdateAsync(Guid id, ActualizarCategoriaDto dto);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}