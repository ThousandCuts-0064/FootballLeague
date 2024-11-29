using System.Collections.Generic;
using FootballLeague.Domain.Matches;

namespace FootballLeague.Domain.Teams
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int Score { get; set; }

        public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
    }
}