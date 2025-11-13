using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Base.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ROL
            CreateMap<Rol, ObtenerRolDto>().ReverseMap();

            CreateMap<InsertarRolDto, Rol>()
                .ForMember(dest => dest.RolId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ReverseMap();

            // CLIENTE
            CreateMap<Cliente, ObtenerClienteDto>().ReverseMap();

            CreateMap<InsertarClienteDto, Cliente>()
                .ForMember(dest => dest.IdCliente, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ReverseMap();

            CreateMap<ActualizarClienteDto, Cliente>().ReverseMap();

            // USUARIO

            // GET
            CreateMap<Usuario, ObtenerUsuarioDto>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src =>
                    src.Rol != null
                        ? new ObtenerRolDto
                        {
                            RolId = src.Rol.RolId,
                            NombreRol = src.Rol.NombreRol
                        }
                        : null
                ))
                .ReverseMap();

            // INSERT
            CreateMap<InsertarUsuarioDto, Usuario>()
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Rol, opt => opt.Ignore()) // evitar loop con rol
                .ReverseMap();

            // UPDATE
            CreateMap<ActualizarUsuarioDto, Usuario>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore()) // no se toca contraseña aquí
                .ForMember(dest => dest.Rol, opt => opt.Ignore())         // se asigna por ID, no objeto completo
                .ReverseMap();

            // CATEGORIA HABITACION
            CreateMap<CategoriaHabitacion, ObtenerCategoriaDto>().ReverseMap();
            
            CreateMap<InsertarCategoriaDto, CategoriaHabitacion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
            
            CreateMap<ActualizarCategoriaDto, CategoriaHabitacion>();

            // -----------------------------------------------
        }
    }
}
