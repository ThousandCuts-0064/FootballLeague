using System;

namespace FootballLeague.Application.Matches.Dtos
{
    public record GetMatchesPageDto(Guid AfterKey = default, int PageSize = 25, bool IsAscending = false);
}