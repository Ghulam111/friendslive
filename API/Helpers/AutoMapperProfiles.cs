using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.photos.FirstOrDefault(u => u.IsMain).Url));
            CreateMap<Photo,PhotoDTO>();

            CreateMap<MemberUpdateDTO,AppUser>();

            CreateMap<RegisterDTO,AppUser>();

            CreateMap<Message,MessageDto>().ForMember( dest => dest.SenderPhotoUrl, 
            opt => opt.MapFrom(src => src.Sender.photos.FirstOrDefault(u => u.IsMain).Url))
            
            .ForMember(dest => dest.RecipientPhotoUrl,
             u=> u.MapFrom(src => src.Recipient.photos.FirstOrDefault(u=>u.IsMain).Url));
        }
    }
}