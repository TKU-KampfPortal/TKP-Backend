using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Models.Dtos.Roles;
using TKP.Server.Core.Entities;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.Features.Roles.Queries.GetAllRole
{
    public sealed class GetAllRoleQueryHandler : BaseQueryPaginationHandler<GetAllRoleQuery, RoleListDto>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public GetAllRoleQueryHandler(RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        protected override async Task<PaginationResponse<RoleListDto>> HandleAsync(GetAllRoleQuery request, CancellationToken cancellationToken = default)
        {
            var roles = await _roleManager.Roles.Skip(request.PageIndex)
                                                .Take(request.PageSize).ToListAsync();

            var roleCount = await _roleManager.Roles.CountAsync();

            return new PaginationResponse<RoleListDto>()
            {
                Items = _mapper.Map<List<RoleListDto>>(roles),
                PageIndex = request.PageIndex,
                PageSize = roles.Count,
                TotalCount = roleCount
            };
        }
    }
}
