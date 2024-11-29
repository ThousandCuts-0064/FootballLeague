using System.ComponentModel.DataAnnotations;
using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Application.Teams.Enums;
using FootballLeague.Domain.Teams;

namespace FootballLeague.API.Requests.Teams
{
    public record GetTeamsPageRequest(
        TeamPaginationStrategy Strategy = TeamPaginationStrategy.Score,
        [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
        string AfterName = null,
        [Range(1, 50)] int PageSize = 25,
        bool IsAscending = true)
    {
        public GetTeamsPageDto ToDto() => new(Strategy, AfterName, PageSize, IsAscending);
    }
}