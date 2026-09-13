using FluentValidation;
using WBS_Assessment.Application.Dto;

namespace WBS_Assessment.Application.Validators;

public class CreateVehicleBookingValidator : AbstractValidator<CreateVehicleBookingRequest>
{
    public CreateVehicleBookingValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.Dropoff).GreaterThan(x => x.Pickup);
    }
}
