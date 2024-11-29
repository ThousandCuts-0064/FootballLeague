using FootballLeague.Application.Matches;
using FootballLeague.Application.Scoring;
using FootballLeague.Application.Teams;
using Microsoft.Extensions.DependencyInjection;

namespace FootballLeague.Application
{
    public static class ServicesEx
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddSingleton<IScoringService, ScoringService>()
                .AddScoped<ITeamService, TeamService>()
                .AddScoped<IMatchService, MatchService>();
        }
    }
}