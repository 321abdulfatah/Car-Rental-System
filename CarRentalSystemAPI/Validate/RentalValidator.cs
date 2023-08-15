using CarRentalSystemAPI.Dtos;
using FluentValidation;

namespace BusinessAccessLayer.Data.Validate
{
    public class RentalValidator : AbstractValidator<RentalDto>
    {
        public RentalValidator()
        {
            
            RuleFor(x => x.Driver.IsAvailable)
                .Must(available => available == true)
                .WithMessage("The 'Driver' should have an available.");

           
           
        }
    }
}
