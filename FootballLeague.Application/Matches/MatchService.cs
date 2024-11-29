using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FootballLeague.Application.Common;
using FootballLeague.Application.Matches.Dtos;
using FootballLeague.Application.Matches.Utils;
using FootballLeague.Application.Scoring;
using FootballLeague.Application.Teams.Utils;
using FootballLeague.Domain.Matches;
using FootballLeague.Domain.Teams;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Application.Matches
{
    internal class MatchService : IMatchService
    {
        private readonly IFootballLeagueDbContext _dbContext;
        private readonly IScoringService _scoringService;

        public MatchService(IFootballLeagueDbContext context, IScoringService scoringService)
        {
            _dbContext = context;
            _scoringService = scoringService;
        }

        public async Task<Result<IReadOnlyCollection<MatchDto>>> GetPageAsync(GetMatchesPageDto dto)
        {
            var dtos = await _dbContext.Matches
                .GetPage(dto, _dbContext.Matches)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Select(MatchMapper.ToDtoExpression)
                .ToListAsync();

            return Result.Success<IReadOnlyCollection<MatchDto>>(dtos);
        }

        public async Task<Result<MatchWithDetailsDto>> FindByKeyAsync(Guid key)
        {
            var dto = await _dbContext.Matches
                .Where(x => x.Key == key)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .AsExpandable()
                .Select(MatchMapper.ToDtoWithDetailsExpression)
                .FirstOrDefaultAsync();

            return dto is null
                ? Result.Error<MatchWithDetailsDto>(MatchErrors.NotFound(key))
                : Result.Success(dto);
        }

        public async Task<Result<MatchDto>> CreateAsync(CreateMatchDto dto)
        {
            if (dto.Team1Name == dto.Team2Name)
                return Result.Error<MatchDto>(MatchErrors.BothTeamsAreTheSame(dto.Team1Name));

            if (dto.StartedAt > DateTime.UtcNow)
                return Result.Error<MatchDto>(MatchErrors.StartedAtWasNotInThePast(dto.StartedAt));

            if (UrlEncoder.Default.Encode(dto.Team1Name) != dto.Team1Name)
                return Result.Error<MatchDto>(TeamErrors.NameCannotBeUrlEncoded(dto.Team1Name));

            if (UrlEncoder.Default.Encode(dto.Team2Name) != dto.Team2Name)
                return Result.Error<MatchDto>(TeamErrors.NameCannotBeUrlEncoded(dto.Team2Name));

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var teams = await _dbContext.Teams
                .Where(x => x.Name == dto.Team1Name || x.Name == dto.Team2Name)
                .ToListAsync();

            var team1 = teams.FirstOrDefault(x => x.Name == dto.Team1Name);
            var team2 = teams.FirstOrDefault(x => x.Name == dto.Team2Name);

            if (team1 is null)
                return Result.Error<MatchDto>(TeamErrors.NotFound(dto.Team1Name));

            if (team2 is null)
                return Result.Error<MatchDto>(TeamErrors.NotFound(dto.Team2Name));

            var alreadyExists = await _dbContext.Matches.AnyAsync(x =>
                x.Team1Id == team1.Id &&
                x.Team2Id == team2.Id &&
                x.StartedAt == dto.StartedAt);

            if (alreadyExists)
                return Result.Error<MatchDto>(MatchErrors.AlreadyExists(dto.Team1Name, dto.Team2Name, dto.StartedAt));

            var entity1 = new Match
            {
                Key = Guid.NewGuid(),
                StartedAt = dto.StartedAt,
                Team1Score = dto.Team1Score,
                Team2Score = dto.Team2Score,
                Team1 = team1,
                Team2 = team2
            };

            var entity2 = new Match
            {
                Key = entity1.Key,
                StartedAt = dto.StartedAt,
                Team1Score = dto.Team2Score,
                Team2Score = dto.Team1Score,
                Team1 = team2,
                Team2 = team1
            };

            _dbContext.Matches.AddRange(entity1, entity2);

            UpdateStats(dto.Team1Score, dto.Team2Score, team1, team2, true);

            team1.Score = _scoringService.GetScore(team1.Wins, team1.Draws, team1.Losses);
            team2.Score = _scoringService.GetScore(team2.Wins, team2.Draws, team2.Losses);

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Result.Success(MatchMapper.ToDto(entity1));
        }

        public async Task<Result<MatchDto>> UpdateScoresByKeyAsync(Guid key, UpdateMatchScoresDto dto)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var matches = await _dbContext.Matches
                .Where(x => x.Key == key)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .ToListAsync();

            if (matches.Count == 0)
                return Result.Error<MatchDto>(MatchErrors.NotFound(key));

            var match = matches[0];
            var team1 = match.Team1;
            var team2 = match.Team2;

            UpdateStats(match.Team1Score, match.Team2Score, team1, team2, false);
            UpdateStats(dto.Team1Score, dto.Team2Score, team1, team2, true);

            match.Team1Score = dto.Team1Score;
            match.Team2Score = dto.Team2Score;

            team1.Score = _scoringService.GetScore(team1.Wins, team1.Draws, team1.Losses);
            team2.Score = _scoringService.GetScore(team2.Wins, team2.Draws, team2.Losses);

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Result.Success(MatchMapper.ToDto(matches[0]));
        }

        public async Task<Result<MatchDto>> DeleteByKeyAsync(Guid key)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var matches = await _dbContext.Matches
                .Where(x => x.Key == key)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .ToListAsync();

            if (matches.Count == 0)
                return Result.Error<MatchDto>(MatchErrors.NotFound(key));

            _dbContext.Matches.RemoveRange(matches);

            var match = matches[0];
            var team1 = match.Team1;
            var team2 = match.Team2;

            UpdateStats(match.Team1Score, match.Team2Score, team1, team2, false);

            team1.Score = _scoringService.GetScore(team1.Wins, team1.Draws, team1.Losses);
            team2.Score = _scoringService.GetScore(team2.Wins, team2.Draws, team2.Losses);

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Result.Success(MatchMapper.ToDto(matches[0]));
        }

        private static void UpdateStats(int team1Score, int team2Score, Team team1, Team team2, bool isAdd)
        {
            var modifier = isAdd ? 1 : -1;

            switch (team1Score.CompareTo(team2Score))
            {
                case > 0:
                    team1.Wins += modifier;
                    team2.Losses += modifier;

                    break;

                case 0:
                    team1.Draws += modifier;
                    team2.Draws += modifier;

                    break;


                case < 0:
                    team1.Losses += modifier;
                    team2.Wins += modifier;

                    break;
            }
        }
    }

    internal static class MatchSourceEx
    {
        public static IQueryable<Match> GetPage(
            this IQueryable<Match> source,
            GetMatchesPageDto dto,
            DbSet<Match> dbSet)
        {
            var lastStartedAt = dbSet
                .Where(x => x.Key == dto.AfterKey)
                .Select(x => x.StartedAt);

            var filtered = source;

            if (dto.AfterKey != default)
            {
                filtered = dto.IsAscending
                    ? source.Where(x => x.StartedAt >= lastStartedAt.FirstOrDefault())
                    : source.Where(x => x.StartedAt <= lastStartedAt.FirstOrDefault());
            }

            filtered = filtered.Where(x => x.Team1Id < x.Team2Id);

            if (dto.IsAscending)
            {
                filtered = filtered
                    .OrderBy(x => x.StartedAt)
                    .ThenBy(x => x.Key)
                    .Where(x => x.Key.CompareTo(dto.AfterKey) > 0 || x.StartedAt > lastStartedAt.FirstOrDefault());
            }
            else
            {
                filtered = filtered
                    .OrderByDescending(x => x.StartedAt)
                    .ThenBy(x => x.Key)
                    .Where(x => x.Key.CompareTo(dto.AfterKey) > 0 || x.StartedAt < lastStartedAt.FirstOrDefault());
            }

            return filtered.Take(dto.PageSize);
        }
    }
}