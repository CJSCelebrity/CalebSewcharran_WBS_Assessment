using FluentValidation;
using WBS_Assessment.Application.Dto;

namespace WBS_Assessment.Application.Validators;

public class CreateApartmentBookingValidator : AbstractValidator<CreateApartmentBookingRequest>
{
    public CreateApartmentBookingValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.CheckOut).GreaterThan(x => x.CheckIn);
    }
}