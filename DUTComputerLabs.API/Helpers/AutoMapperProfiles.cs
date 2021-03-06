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
                    opt => opt.Ignore())
                .ForMember(dest => dest.PhotoPublicId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PhotoPublicId,
                    opt => opt.Ignore());

            CreateMap<ComputerLab, ComputerLabForList>()
                .ForMember(dest => dest.EditMode,
                    opt => opt.Ignore());

            CreateMap<ComputerLab, ComputerLabForDetailed>()
                .ForMember(dest => dest.Owner,
                    opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.OwnerPhoneNumber,
                    opt => opt.MapFrom(src => src.Owner.PhoneNumber))
                .ForMember(dest => dest.OwnerEmail,
                    opt => opt.MapFrom(src => src.Owner.Email));

            CreateMap<ComputerLabForInsert, ComputerLab>();

            CreateMap<Booking, BookingForDetailed>()
                .ForMember(dest => dest.BookerName,
                    opt => opt.MapFrom(src => src.User.Name));

            CreateMap<BookingForInsert, Booking>()
                .ForMember(dest => dest.User,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Lab,
                    opt => opt.Ignore());

            CreateMap<Notification, NotificationForDetailed>()
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => GenerateNoticeContent(src)));

            CreateMap<NotificationForInsert, Notification>()
                .ForMember(dest => dest.Booking,
                    opt => opt.Ignore());

            CreateMap<Feedback, FeedbackForDetailed>()
                .ForMember(dest => dest.User,
                    opt => opt.MapFrom(src => src.Booking.User));

            CreateMap<FeedbackForInsert, Feedback>();
        }

        private string GenerateNoticeContent(Notification notification)
        {
            var content = $"Thông báo đối với lịch đặt phòng ngày {notification.Booking.BookingDate.ToString("dd/MM/yyyy")}, tiết {notification.Booking.StartAt}-{notification.Booking.EndAt} tại phòng {notification.Booking.Lab.Name}: {notification.Content}. Vui lòng cập nhật hoặc hủy lịch đặt phòng này.";
            return content;
        }
        
    }
}