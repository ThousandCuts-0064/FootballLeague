using System;
using FootballLeague.Domain.Teams;

namespace FootballLeague.Domain.Matches
{
    public class Match
    {
        public Guid Key { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public DateTime StartedAt { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
    }
}