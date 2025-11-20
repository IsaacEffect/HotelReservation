using HotelReservation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Application.Contracts
{
    public interface IPdfService
    {
        byte[] GenerarFacturaPdf(FacturaDto factura);
    }
}
