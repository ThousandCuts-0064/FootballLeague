using System.Collections.Generic;
using FootballLeague.Application.Matches.Dtos;

namespace FootballLeague.Application.Teams.Dtos
{
    public record TeamWithDetailsDto(
        string Name,
        string DisplayName,
        int Wins,
        int Draws,
        int Losses,
        int Score,
        IReadOnlyCollection<MatchDto> Matches)
        : TeamDto(Name, DisplayName, Wins, Draws, Losses, Score);
}