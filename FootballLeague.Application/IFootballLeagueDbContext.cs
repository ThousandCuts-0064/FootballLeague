using System.Threading.Tasks;
using System.Threading;
using FootballLeague.Domain.Matches;
using FootballLeague.Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FootballLeague.Application
{
    public interface IFootballLeagueDbContext
    {
        public DatabaseFacade Database { get; }

        public DbSet<Team> Teams { get; init; }
        public DbSet<Match> Matches { get; init; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}