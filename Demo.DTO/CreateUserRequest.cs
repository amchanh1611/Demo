using demo.Models;
using FluentValidation;

namespace Demo.DTO
{
    public class CreateUserRequest : BaseUserRequest
    {

        public CreateUserRequest()
        {
            
        }
    }

    public class UserValidator : AbstractValidator<CreateUserRequest>
    {
        
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Usernam is required");
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email is not valid");
            RuleFor(user => user.Birtday)
                .Must((_, date) => {
                    DateTime currentDay = DateTime.Now;
                    TimeSpan birtday = currentDay.Date - date.Date;
                    if ((int)Math.Floor(birtday.Days / (double)365) >= 18)
                        return true;
                    return false;
                })
            .WithMessage("Age must be more than 18");
            RuleFor(user => user.FullName).NotNull().WithMessage("Usernam is required");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Usernam is required")
                .MinimumLength(8).WithMessage("Password is at least 8 charracter")
                .Matches("[A-Z]").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Your password must contain at least one number.")
                .Matches(@"[""!@$%^&*(){}:;<>,.?/+\-_=|'[\]~\\]").WithMessage("{PropertyName} must contain at least one special character");
        }
    }
   
}