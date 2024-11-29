namespace FootballLeague.Application.Scoring
{
    public interface IScoringService
    {
        public int GetScore(int wins, int draws, int losses);
    }
}