using FluentValidation;
using MatchApi.Domain.Enums;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixtureScore;

public class UpdateFixtureScoreCommandValidator : AbstractValidator<UpdateFixtureScoreCommand>
{
    public UpdateFixtureScoreCommandValidator()
    {
        RuleFor(x => x.FixtureId)
            .NotEmpty().WithMessage("Fixture is required.");

        RuleFor(x => x.Side)
            .IsInEnum().WithMessage("Side must be either Home or Away.");


    }
}
