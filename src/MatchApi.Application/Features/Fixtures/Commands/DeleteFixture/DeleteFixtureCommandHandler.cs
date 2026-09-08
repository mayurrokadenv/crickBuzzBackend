using System;
using System.Collections.Generic;
using System.Text;
using MatchApi.Application.Common.Interfaces;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.DeleteFixture;

public class DeleteFixtureCommandHandler
    : IRequestHandler<DeleteFixtureCommand, string>
{
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFixtureCommandHandler(
        IFixtureRepository fixtureRepository,
        IUnitOfWork unitOfWork)
    {
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(
        DeleteFixtureCommand request,
        CancellationToken cancellationToken)
    {
        var fixture = await _fixtureRepository.GetByIdAsync(
            request.FixtureId,
            cancellationToken);

        if (fixture is null)
        {
            throw new InvalidOperationException("Fixture not found.");
        }

        _fixtureRepository.Delete(fixture);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Fixture deleted successfully.";
    }
}
