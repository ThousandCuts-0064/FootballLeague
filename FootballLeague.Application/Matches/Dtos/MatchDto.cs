using System;

namespace FootballLeague.Application.Matches.Dtos
{
    public record MatchDto(
        Guid Key,
        string Team1Name,
        string Team2Name,
        DateTime StartedAt,
        string Team1DisplayName,
        string Team2DisplayName,
        int Team1Score,
        int Team2Score);
}