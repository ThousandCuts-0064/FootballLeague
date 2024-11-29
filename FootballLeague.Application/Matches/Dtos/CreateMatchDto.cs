using System;

namespace FootballLeague.Application.Matches.Dtos
{
    public record CreateMatchDto(DateTime StartedAt, string Team1Name, string Team2Name, int Team1Score, int Team2Score);
}
