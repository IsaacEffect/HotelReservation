using AutoMapper;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.DTOs
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>().ReverseMap();
        }
    }
}