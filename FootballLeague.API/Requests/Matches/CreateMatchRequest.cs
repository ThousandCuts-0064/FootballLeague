using System;
using System.ComponentModel.DataAnnotations;
using FootballLeague.Application.Matches.Dtos;
using FootballLeague.Domain.Teams;

namespace FootballLeague.API.Requests.Matches
{
    public record CreateMatchRequest(
        [Required] DateTime? StartedAt,
        [Required]
        [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
        string Team1Name,
        [Required]
        [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
        string Team2Name,
        [Required, Range(0, int.MaxValue)] int? Team1Score,
        [Required, Range(0, int.MaxValue)] int? Team2Score)
    {
        public CreateMatchDto ToDto() => new(StartedAt.Value, Team1Name, Team2Name, Team1Score.Value, Team2Score.Value);
    }
}