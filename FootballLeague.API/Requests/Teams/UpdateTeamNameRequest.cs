using System.ComponentModel.DataAnnotations;
using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Domain.Teams;

namespace FootballLeague.API.Requests.Teams
{
    public record UpdateTeamNameRequest(
        [Required]
        [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
        string Name,
        [Required]
        [StringLength(TeamConstraint.DisplayNameMaxLength, MinimumLength = TeamConstraint.DisplayNameMinLength)]
        string DisplayName)
    {
        public UpdateTeamNameDto ToDto() => new(Name, DisplayName);
    }
}