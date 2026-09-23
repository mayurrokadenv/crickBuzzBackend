using FluentValidation;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixture;

public class UpdateFixtureCommandValidator : AbstractValidator<UpdateFixtureCommand>
{
    public UpdateFixtureCommandValidator()
    {
        RuleFor(x => x.FixtureId)
            .NotEmpty().WithMessage("Fixture is required.");

        RuleFor(x => x)
            .Must(x => x.Status is not null || x.Phase is not null)
            .WithMessage("At least one of Status or Phase must be provided.");

        RuleFor(x => x.Status!.Value)
            .IsInEnum()
            .WithMessage("Status is not a recognized match status.")
            .When(x => x.Status is not null);

        RuleFor(x => x.Phase!.Value)
            .IsInEnum()
            .WithMessage("Phase is not a recognized match phase.")
            .When(x => x.Phase is not null);

       
    }
}