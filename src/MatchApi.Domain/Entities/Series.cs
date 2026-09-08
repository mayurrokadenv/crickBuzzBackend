using System;
using System.Collections.Generic;
using System.Text;
using MatchApi.Domain.Common;

namespace MatchApi.Domain.Entities;

public class Series : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid SportId { get; set; }
    public Sport Sport { get; set; } = null!;

    public ICollection<SeriesTeam> SeriesTeams { get; set; }
        = new List<SeriesTeam>();

    public ICollection<Fixture> Fixtures { get; set; }
        = new List<Fixture>();
}
