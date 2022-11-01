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
        //private readonly DemoDbContext context;
        public UserValidator(/*DemoDbContext context*/)
        {
            //this.context = context;
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Usernam is required")
                /*.Must(AlreadyExistUser).WithMessage("UserName alreadt exists")*/;
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email is not valid");
            RuleFor(user => user.Birtday).Must(BeAValidDate)
            .WithMessage("Age must be more than 18");
            RuleFor(user => user.FullName).NotNull().WithMessage("Usernam is required");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Usernam is required")
                .MinimumLength(8).WithMessage("Password is at least 8 charracter")
                .Matches(@"[A-Z]").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Your password must contain at least one number.")
                .Matches(@"[""!@$%^&*(){}:;<>,.?/+\-_=|'[\]~\\]").WithMessage("{PropertyName} must contain at least one special character");
        }



        private bool BeAValidDate(DateTime date)
        {
            var currentDay = DateTime.Now;
            var birtday = currentDay.Date - date.Date;
            if ((birtday.Days / 365) >= 18)
                return true;
            return false;
        }
        //private bool AlreadyExistUser(string userName)
        //{
        //    var user = context.users.SingleOrDefault(s => s.UserName == userName);
        //    if (user != null)
        //        return true;
        //    return false;
        //}
    }
   
}