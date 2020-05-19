using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Models;

namespace DUTComputerLabs.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForList>();

            CreateMap<User, UserForDetailed>()
                .ForMember(dest => dest.Faculty,
                    opt => opt.MapFrom(src => src.Faculty.Name))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender == false ? "Nam" : "Nữ"))
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<UserForInsert, User>()
                .ForMember(dest => dest.Password,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Role,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Faculty,
                    opt => opt.Ignore());

            CreateMap<ComputerLab, ComputerLabForList>();

            CreateMap<ComputerLabForInsert, ComputerLab>();

            CreateMap<Booking, BookingForDetailed>()
                .ForMember(dest => dest.LabName,
                    opt => opt.MapFrom(src => src.Lab.Name))
                .ForMember(dest => dest.BookerName,
                    opt => opt.MapFrom(src => src.User.Name));

            CreateMap<BookingForInsert, Booking>();
        }
        
    }
}