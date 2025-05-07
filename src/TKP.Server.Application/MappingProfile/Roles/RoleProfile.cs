using AutoMapper;
using TKP.Server.Application.Features.Roles.Shared.Dtos;
using TKP.Server.Application.Models.Dtos.Roles;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.MappingProfile.Roles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {

            CreateMap<RoleRequestBody, ApplicationRole>();
            CreateMap<ApplicationRole, RoleDto>();
            CreateMap<ApplicationRole, RoleListDto>();
        }
    }
}
