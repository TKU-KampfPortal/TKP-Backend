using AutoMapper;
using TKP.Server.Application.Models.Dtos.Auth;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.MappingProfile.Auth
{
    public class AuthUserProfile : Profile
    {
        public AuthUserProfile()
        {
            CreateMap<ApplicationUser, AuthUserDto>();
        }
    }
}
