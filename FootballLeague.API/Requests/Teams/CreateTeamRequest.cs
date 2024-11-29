using System.ComponentModel.DataAnnotations;
using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Domain.Teams;

namespace FootballLeague.API.Requests.Teams
{
    public record CreateTeamRequest(
        [Required]
        [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
        string Name,
        [Required]
        [StringLength(TeamConstraint.DisplayNameMaxLength, MinimumLength = TeamConstraint.DisplayNameMinLength)]
        string DisplayName)
    {
        public CreateTeamDto ToDto() => new(Name, DisplayName);
    }
}