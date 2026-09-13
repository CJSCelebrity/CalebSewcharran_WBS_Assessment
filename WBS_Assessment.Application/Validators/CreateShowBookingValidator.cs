using FluentValidation;
using WBS_Assessment.Application.Dto;

namespace WBS_Assessment.Application.Validators;

public class CreateShowBookingValidator : AbstractValidator<CreateShowBookingRequest>
{
    public CreateShowBookingValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.PerformanceTime).NotEmpty();
    }
}
