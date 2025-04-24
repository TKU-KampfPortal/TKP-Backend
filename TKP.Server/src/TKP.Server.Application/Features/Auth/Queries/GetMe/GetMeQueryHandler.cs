using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Application.Models.Dtos.Auth;
using TKP.Server.Domain.Entites;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Application.Features.Auth.Queries.GetMe
{
    public sealed class GetMeQueryHandler : BaseQueryHandler<GetMeQuery, AuthUserDto>
    {
        private readonly ICacheService<AuthUserDto> _authUserCacheService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;

        public GetMeQueryHandler(ICacheFactory cacheFactory
            , IClaimService claimService
            , UserManager<ApplicationUser> userManager
            , IMapper mapper)
        {
            _authUserCacheService = cacheFactory.GetCacheService<AuthUserDto>();
            _claimService = claimService;
            _userManager = userManager;
            _mapper = mapper;
        }

        protected override async Task<AuthUserDto> HandleAsync(GetMeQuery request, CancellationToken cancellationToken = default)
        {
            var userId = _claimService.GetUserId();
            var userResponse = await _authUserCacheService.GetValueAsync(PrefixCacheKey.AuthUser, userId);
            if (userResponse is null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    throw new UnauthorizeException("User is not found");
                }
                userResponse = _mapper.Map<ApplicationUser, AuthUserDto>(user);
                await _authUserCacheService.SetValueAsync(PrefixCacheKey.AuthUser, userId, userResponse, TimeSpan.FromHours(1));
            }

            //TODO: Add permissions to userResponse
            userResponse.Permissions = new List<string>();

            return userResponse;

        }
    }
}
