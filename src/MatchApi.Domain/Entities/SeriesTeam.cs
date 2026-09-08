using System;
using System.Collections.Generic;
using System.Text;

using MatchApi.Domain.Common;

namespace MatchApi.Domain.Entities;

public class SeriesTeam : BaseEntity
{
    public Guid SeriesId { get; set; }
    public Series Series { get; set; } = null!;

    public string SeriesName { get; set; } = string.Empty;

    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
}