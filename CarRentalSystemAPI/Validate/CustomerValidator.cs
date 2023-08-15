using CarRentalSystemAPI.Dtos;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BusinessAccessLayer.Data.Validate
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {

            RuleFor(x => x.Name)
                .MinimumLength(3)
                .WithMessage("The 'Name' should have at least 3 characters.")
                .MaximumLength(20)
                .WithMessage("The 'Name' should have not more than 20 characters.");

            RuleFor(x => x.Address)
                .MinimumLength(5)
                .WithMessage("The 'Address' should have at least 5 characters.")
                .MaximumLength(50)
                .WithMessage("The 'Address' should have not more than 50 characters.");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("The 'Email' collection should have Engine Capacity.")
                .EmailAddress();

            RuleFor(x => x.Age)
                .NotNull()
                .WithMessage("The 'Age' collection should have Daily Fare.");

            RuleFor(x => x.Phone)
                .NotNull()
                .WithMessage("The 'Age' collection should have Daily Fare.")
                .Matches(new Regex(@"09\d{2}-\d{3}-\d{3}$"))
                .WithMessage("PhoneNumber not valid");

            RuleFor(x => x.Gender)
               .NotNull()
               .WithMessage("The 'Gender' collection should have Daily Fare.")
               .Matches(new Regex("Male|Female"))
               .WithMessage("The 'Gender' collection should have either Male or Female");

        }
    }
}
