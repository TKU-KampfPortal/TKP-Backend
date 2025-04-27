using FluentValidation;
using TKP.Server.Application.Features.Roles.Shared.Validators;

namespace TKP.Server.Application.Features.Roles.Commands.UpdateRole
{
    class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.Body).SetValidator(new RoleRequestBodyValidator());
        }

    }
}
