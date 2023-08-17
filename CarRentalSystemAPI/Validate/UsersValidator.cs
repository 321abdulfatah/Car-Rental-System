using CarRentalSystemAPI.Dtos;
using FluentValidation;

namespace BusinessAccessLayer.Data.Validate
{
    public class UsersValidator : AbstractValidator<UsersDto>
    {
        public UsersValidator()
        {
            
            // Type - MinLength 3, MaxLength 20
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .WithMessage("The 'Name' should have at least 3 characters.")
                .MaximumLength(10)
                .WithMessage("The 'Name' should have not more than 10 characters.");

            // Color - MinLength 3, MaxLength 10
            RuleFor(x => x.Password)
                .MinimumLength(3)
                .WithMessage("The 'Password' should have at least 3 characters.")
                .MaximumLength(10)
                .WithMessage("The 'Password' should have not more than 10 characters.")
                .NotNull()
                .WithMessage("The 'Password' should have not Null Value");

        }
    }
}
