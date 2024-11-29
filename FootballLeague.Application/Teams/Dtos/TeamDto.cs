namespace FootballLeague.Application.Teams.Dtos
{
    public record TeamDto(string Name, string DisplayName, int Wins, int Draws, int Losses, int Score);
}