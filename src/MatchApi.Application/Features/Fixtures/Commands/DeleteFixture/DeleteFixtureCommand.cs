using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.DeleteFixture;

public record DeleteFixtureCommand(Guid FixtureId) : IRequest<string>;
