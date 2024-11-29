using FootballLeague.Application.Teams.Enums;

namespace FootballLeague.Application.Teams.Dtos
{
    public record GetTeamsPageDto(
        TeamPaginationStrategy Strategy = TeamPaginationStrategy.Score,
        string AfterName = "",
        int PageSize = 25,
        bool IsAscending = true);
}