using FluentValidation;
using TKP.Server.Application.Features.Roles.Shared.Validators;

namespace TKP.Server.Application.Features.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Body).SetValidator(new RoleRequestBodyValidator());
        }
    }
}
