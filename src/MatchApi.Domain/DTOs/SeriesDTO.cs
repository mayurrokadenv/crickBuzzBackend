using System;
using System.Collections.Generic;

namespace MatchApi.Domain.DTOs;

public class SeriesDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid SportId { get; set; }

    public string SportName { get; set; } = string.Empty;

    public List<SeriesTeamDto> Teams { get; set; } = new();
}

public class SeriesTeamDto
{
    public Guid TeamId { get; set; }

    public string TeamName { get; set; } = string.Empty;
}