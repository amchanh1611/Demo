using Demo.DTO;
using FluentValidation;

namespace Demo.Helper.FluentValidation
{
    public class UserValidator : AbstractValidator<CreateUserRequest>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotNull().WithMessage("Usernam is required");
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email is not valid");
            RuleFor(user => user.Password).NotNull().WithMessage("Usernam is required")
                .MinimumLength(6).WithMessage("Password is at least 6 charracter");
            RuleFor(user => user.Age).GreaterThan(18).WithMessage("Age must be more than 18");
            RuleFor(user => user.FullName).NotNull().WithMessage("Usernam is required");
        }
    }
}