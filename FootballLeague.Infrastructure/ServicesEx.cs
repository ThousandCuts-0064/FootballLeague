using FootballLeague.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballLeague.Infrastructure
{
    public static class ServicesEx
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddDbContext<IFootballLeagueDbContext, FootballLeagueDbContext>(x =>
                    x.UseSqlServer(configuration.GetConnectionString("Default")));
        }
    }
}