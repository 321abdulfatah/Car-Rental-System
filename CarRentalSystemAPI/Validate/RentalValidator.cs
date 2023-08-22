using CarRentalSystemAPI.Dtos;
using FluentValidation;

namespace BusinessAccessLayer.Data.Validate
{
    public class RentalValidator : AbstractValidator<RentalDto>
    {
        public RentalValidator()
        {
            RuleFor(x => x.StartDateRent)
                 .Must(Date => Date >= DateTime.Now);
           
        }
    }
}
