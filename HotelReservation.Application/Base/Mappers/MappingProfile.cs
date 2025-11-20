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
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ReverseMap();

            // UPDATE
            CreateMap<ActualizarUsuarioDto, Usuario>()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore())
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ReverseMap();


            // CATEGORIA HABITACION
            CreateMap<CategoriaHabitacion, ObtenerCategoriaDto>().ReverseMap();

            CreateMap<InsertarCategoriaDto, CategoriaHabitacion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<ActualizarCategoriaDto, CategoriaHabitacion>();

            // -----------------------------------------------

            // Entidad a DTO Básico (para GetById, GetAll)
            CreateMap<Reserva, ReservaDTO>().ReverseMap();

            // DTO de Creación -> Entidad
            CreateMap<CrearReservaDTO, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.FechaReserva, opt => opt.MapFrom(src => DateTime.UtcNow))
                // El estado ("Confirmada") lo asigna el Servicio, no el mapeador
                .ForMember(dest => dest.EstadoReserva, opt => opt.Ignore())
                // El Total lo calcula el Trigger de SQL
                .ForMember(dest => dest.Total, opt => opt.Ignore());

            // DTO de Actualización -> Entidad (Solo actualiza fechas)
            CreateMap<ActualizarReservaDTO, Reserva>()
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                // Ignora todos los demás campos para no sobrescribirlos
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaReserva, opt => opt.Ignore())
                // El estado ("Pendiente") lo asigna el Servicio
                .ForMember(dest => dest.EstadoReserva, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.HabitacionId, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore());

            // -----------------------------------------------
        }
    }
}
