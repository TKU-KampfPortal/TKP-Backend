using FluentValidation;
using TKP.Server.Application.Features.Roles.Shared.Dtos;

namespace TKP.Server.Application.Features.Roles.Shared.Validators
{
    class RoleRequestBodyValidator : AbstractValidator<RoleRequestBody>
    {
        public RoleRequestBodyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Role name is required.")
                .MaximumLength(50)
                .WithMessage("Role name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Role description is required.")
                .MaximumLength(200)
                .WithMessage("Role description must not exceed 200 characters.");

            RuleFor(x => x.PermissionKeys)
                .NotEmpty()
                .WithMessage("Permissions are required.")
                .Must(permissions => permissions.All(permission => permission.StartsWith("Permission.")))
                .WithMessage("All permissions must start with 'Permission.' prefix.");
        }
    }
}
