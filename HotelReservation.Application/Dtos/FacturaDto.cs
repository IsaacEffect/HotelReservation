
using HotelReservation.Application.DTOs;
using System;
using System.Collections.Generic;

namespace HotelReservation.Application.Dtos;

    public class FacturaDto
    {
        public Guid Id { get; set; }
        public Guid ReservaId { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal MontoTotal { get; set; }
        public string MetodoPago { get; set; } = string.Empty;

        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string HuespedNombre { get; set; } = string.Empty;

        public List<DetalleFacturaDto> Detalles { get; set; } = new();
    }


