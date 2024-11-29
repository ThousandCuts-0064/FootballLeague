using FootballLeague.Application;
using FootballLeague.Domain.Matches;
using FootballLeague.Domain.Teams;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure
{
    internal class FootballLeagueDbContext : DbContext, IFootballLeagueDbContext
    {
        public DbSet<Team> Teams { get; init; }
        public DbSet<Match> Matches { get; init; }

        public FootballLeagueDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FootballLeagueDbContext).Assembly);
        }
    }
}