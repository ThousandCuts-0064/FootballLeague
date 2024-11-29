using System;
using System.Linq.Expressions;
using FootballLeague.Application.Matches.Dtos;
using FootballLeague.Application.Teams.Utils;
using FootballLeague.Domain.Matches;
using LinqKit;

namespace FootballLeague.Application.Matches.Utils
{
    internal static class MatchMapper
    {
        public static Expression<Func<Match, MatchDto>> ToDtoExpression { get; } = x => new MatchDto(
            x.Key,
            x.Team1.Name,
            x.Team2.Name,
            x.StartedAt,
            x.Team1.DisplayName,
            x.Team2.DisplayName,
            x.Team1Score,
            x.Team2Score);

        public static Expression<Func<Match, MatchWithDetailsDto>> ToDtoWithDetailsExpression { get; } = x =>
            new MatchWithDetailsDto(
                x.Key,
                x.StartedAt,
                x.Team1Score,
                x.Team2Score,
                TeamMapper.ToDtoExpression.Invoke(x.Team1),
                TeamMapper.ToDtoExpression.Invoke(x.Team2));

        public static MatchDto ToDto(Match entity) => new(
            entity.Key,
            entity.Team1.Name,
            entity.Team2.Name,
            entity.StartedAt,
            entity.Team1.DisplayName,
            entity.Team2.DisplayName,
            entity.Team1Score,
            entity.Team2Score);
    }
}