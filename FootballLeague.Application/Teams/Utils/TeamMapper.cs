using System;
using System.Linq;
using System.Linq.Expressions;
using FootballLeague.Application.Matches.Utils;
using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Domain.Teams;

namespace FootballLeague.Application.Teams.Utils
{
    internal static class TeamMapper
    {
        public static Expression<Func<Team, TeamDto>> ToDtoExpression { get; } = x =>
            new TeamDto(x.Name, x.DisplayName, x.Wins, x.Draws, x.Losses, x.Score);

        public static Expression<Func<Team, TeamWithDetailsDto>> ToDtoWithDetailsExpression { get; } = x =>
            new TeamWithDetailsDto(
                x.Name,
                x.DisplayName,
                x.Wins,
                x.Draws,
                x.Losses,
                x.Score,
                x.Matches.AsQueryable().Select(MatchMapper.ToDtoExpression).ToList());

        public static TeamDto ToDto(Team entity) =>
            new(entity.Name, entity.DisplayName, entity.Wins, entity.Draws, entity.Losses, entity.Score);
    }
}