﻿using FluentValidation;

namespace Demo.DTO
{
    public class CreateUserRequest : BaseUserRequest
    {
    }

    public class UserValidator : AbstractValidator<CreateUserRequest>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotNull().WithMessage("Usernam is required");
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email is not valid");
            RuleFor(user => user.Birtday).Must(BeAValidDate)
            .WithMessage("Age must be more than 18");
            RuleFor(user => user.FullName).NotNull().WithMessage("Usernam is required");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Usernam is required")
                .MinimumLength(8).WithMessage("Password is at least 6 charracter")
                .Matches(@"[A-Z]").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Your password must contain at least one number.")
                .Matches(@"[""!@$%^&*(){}:;<>,.?/+\-_=|'[\]~\\]").WithMessage("{PropertyName} must contain at least one special character");
        }



        private bool BeAValidDate(DateTime date)
        {
            var currentDay = DateTime.Now;
            if ((date.Day >= currentDay.Day) && (date.Month>= currentDay.Month) && ((currentDay.Year -date.Year) >= 18))
                return true;
            return false;
        }
    }
   
}