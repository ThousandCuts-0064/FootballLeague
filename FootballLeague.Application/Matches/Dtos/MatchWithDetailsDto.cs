using System;
using FootballLeague.Application.Teams.Dtos;

namespace FootballLeague.Application.Matches.Dtos
{
    public record MatchWithDetailsDto(
        Guid Key,
        DateTime StartedAt,
        int Team1Score,
        int Team2Score,
        TeamDto Team1,
        TeamDto Team2)
        : MatchDto(
            Key,
            Team1.Name,
            Team2.Name,
            StartedAt,
            Team1.DisplayName,
            Team2.DisplayName,
            Team1Score,
            Team2Score);
}