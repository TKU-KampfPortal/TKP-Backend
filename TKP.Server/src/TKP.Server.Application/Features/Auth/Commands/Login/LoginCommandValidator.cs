using FluentValidation;
using System.Text.RegularExpressions;

namespace TKP.Server.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(p => p.Body.UserName)
                .NotEmpty()
                .WithMessage("UserName is required")
                .MaximumLength(50)
                .WithMessage("UserName must not exceed 50 characters")
                .Matches(@"^[a-zA-Z0-9-._@+]+$") // Check allowed characters 
                .WithMessage("UserName contains invalid characters.")
                .Must((command, userName) => IsValidEmailOrUserName(userName))  // Check if email or username vaid
                .WithMessage("UserName or Email is invalid.");

            RuleFor(p => p.Body.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)  // Check min password length
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]")  // Check at lease 1 uppercase letter
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")  // Check at lease 1 lowercase letter    
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]")  // Check at lease 1 digit   
                .WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]+")  // Check at lease 1 non-alphanumeric character
                .WithMessage("Password must contain at least one non-alphanumeric character.");
        }

        private bool IsValidEmailOrUserName(string userName)
        {
            // Kiểm tra xem userName có phải là email hợp lệ không
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (emailRegex.IsMatch(userName))
            {
                return true; // Là email hợp lệ
            }

            // Nếu không phải email, kiểm tra userName hợp lệ
            // (Trường hợp này có thể tùy chỉnh thêm nếu bạn có yêu cầu khác về tên người dùng)
            return !string.IsNullOrWhiteSpace(userName);
        }

    }
}
