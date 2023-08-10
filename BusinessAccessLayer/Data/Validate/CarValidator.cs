using FluentValidation;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Data.Validate
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            
            // Type - MinLength 3, MaxLength 20
            RuleFor(x => x.Type)
                .MinimumLength(3)
                .WithMessage("The 'Name' should have at least 3 characters.")
                .MaximumLength(20)
                .WithMessage("The 'Name' should have not more than 20 characters.");

            // Color - MinLength 3, MaxLength 10
            RuleFor(x => x.Color)
                .MinimumLength(3)
                .WithMessage("The 'Name' should have at least 3 characters.")
                .MaximumLength(10)
                .WithMessage("The 'Name' should have not more than 10 characters.");


            RuleFor(x => x.EngineCapacity)
                .NotNull()
                .WithMessage("The 'Car' collection should have Engine Capacity.");

            RuleFor(x => x.DailyFare)
                .NotNull()
                .WithMessage("The 'Car' collection should have Daily Fare.");

           
        }
    }
}
