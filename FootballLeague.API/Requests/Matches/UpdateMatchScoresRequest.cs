using System.ComponentModel.DataAnnotations;
using FootballLeague.Application.Matches.Dtos;

namespace FootballLeague.API.Requests.Matches
{
    public record UpdateMatchScoresRequest(
        [Range(0, int.MaxValue)] int? Team1Score,
        [Range(0, int.MaxValue)] int? Team2Score)
    {
        public UpdateMatchScoresDto ToDto() => new(Team1Score.Value, Team2Score.Value);
    }
}