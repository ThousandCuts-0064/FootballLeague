namespace FootballLeague.Application.Scoring
{
    internal class ScoringService : IScoringService
    {
        public int GetScore(int wins, int draws, int losses) => wins * 3 + draws;
    }
}
