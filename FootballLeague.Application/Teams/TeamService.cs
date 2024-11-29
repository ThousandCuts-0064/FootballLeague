using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FootballLeague.Application.Common;
using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Application.Teams.Enums;
using FootballLeague.Application.Teams.Utils;
using FootballLeague.Domain.Teams;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Application.Teams
{
    internal class TeamService : ITeamService
    {
        private readonly IFootballLeagueDbContext _dbContext;

        public TeamService(IFootballLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<IReadOnlyCollection<TeamDto>>> GetPageAsync(GetTeamsPageDto dto)
        {
            if (dto.AfterName is not null && UrlEncoder.Default.Encode(dto.AfterName) != dto.AfterName)
                return Result.Error<IReadOnlyCollection<TeamDto>>(TeamErrors.NameCannotBeUrlEncoded(dto.AfterName));

            var dtos = await _dbContext.Teams
                .GetPageBy(dto, _dbContext.Teams)
                .Take(dto.PageSize)
                .Select(TeamMapper.ToDtoExpression)
                .ToListAsync();

            return Result.Success<IReadOnlyCollection<TeamDto>>(dtos);
        }

        public async Task<Result<TeamWithDetailsDto>> FindByNameAsync(string name)
        {
            if (UrlEncoder.Default.Encode(name) != name)
                return Result.Error<TeamWithDetailsDto>(TeamErrors.NameCannotBeUrlEncoded(name));

            var dto = await _dbContext.Teams
                .Where(x => x.Name == name)
                .Include(x => x.Matches)
                .Select(TeamMapper.ToDtoWithDetailsExpression)
                .FirstOrDefaultAsync();

            return dto is null
                ? Result.Error<TeamWithDetailsDto>(TeamErrors.NotFound(name))
                : Result.Success(dto);
        }

        public async Task<Result<TeamDto>> CreateAsync(CreateTeamDto dto)
        {
            if (await _dbContext.Teams.AnyAsync(x => x.Name == dto.Name))
                return Result.Error<TeamDto>(TeamErrors.AlreadyExists(dto.Name));

            if (UrlEncoder.Default.Encode(dto.Name) != dto.Name)
                return Result.Error<TeamDto>(TeamErrors.NameCannotBeUrlEncoded(dto.Name));

            var entity = new Team
            {
                Name = dto.Name,
                DisplayName = dto.DisplayName
            };

            _dbContext.Teams.Add(entity);

            await _dbContext.SaveChangesAsync();

            return Result.Success(TeamMapper.ToDto(entity));
        }

        public async Task<Result<TeamDto>> UpdateNameByNameAsync(string name, UpdateTeamNameDto dto)
        {
            if (UrlEncoder.Default.Encode(name) != name)
                return Result.Error<TeamDto>(TeamErrors.NameCannotBeUrlEncoded(name));

            if (UrlEncoder.Default.Encode(dto.Name) != dto.Name)
                return Result.Error<TeamDto>(TeamErrors.NameCannotBeUrlEncoded(dto.Name));

            var entities = await _dbContext.Teams
                .Where(x => x.Name == name || x.Name == dto.Name)
                .ToListAsync();

            if (entities.Count == 0)
                return Result.Error<TeamDto>(TeamErrors.NotFound(name));

            if (entities.Count == 1 && entities[0].Name != name)
                return Result.Error<TeamDto>(TeamErrors.NotFound(name));

            if (entities.Count == 2)
                return Result.Error<TeamDto>(TeamErrors.AlreadyExists(name));

            entities[0].Name = dto.Name;
            entities[0].DisplayName = dto.DisplayName;

            await _dbContext.SaveChangesAsync();

            return Result.Success(TeamMapper.ToDto(entities[0]));
        }

        public async Task<Result<TeamDto>> DeleteByNameAsync(string name)
        {
            if (UrlEncoder.Default.Encode(name) != name)
                return Result.Error<TeamDto>(TeamErrors.NameCannotBeUrlEncoded(name));

            var entity = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Name == name);

            if (entity is null)
                return Result.Error<TeamDto>(TeamErrors.NotFound(name));

            if (entity.Wins > 0 || entity.Draws > 0 || entity.Losses > 0)
                return Result.Error(TeamErrors.CannotDeleteTeamWhileExistingInMatches(name), TeamMapper.ToDto(entity));

            _dbContext.Teams.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Result.Success(TeamMapper.ToDto(entity));
        }
    }

    internal static class TeamSourceEx
    {
        public static IQueryable<Team> GetPageBy(
            this IQueryable<Team> source,
            GetTeamsPageDto dto,
            DbSet<Team> dbSet)
        {
            var filtered = source;

            switch (dto.Strategy)
            {
                case TeamPaginationStrategy.Score:
                {
                    var lastScore = dbSet
                        .Where(x => x.Name == dto.AfterName)
                        .Select(x => x.Score);

                    if (!string.IsNullOrEmpty(dto.AfterName))
                    {
                        filtered = dto.IsAscending
                            ? source.Where(x => x.Score >= lastScore.FirstOrDefault())
                            : source.Where(x => x.Score <= lastScore.FirstOrDefault());
                    }

                    if (dto.IsAscending)
                    {
                        filtered = filtered
                            .OrderBy(x => x.Score)
                            .ThenBy(x => x.Name)
                            .Where(x => x.Name.CompareTo(dto.AfterName) > 0 || x.Score > lastScore.FirstOrDefault());
                    }
                    else
                    {
                        filtered = filtered
                            .OrderByDescending(x => x.Score)
                            .ThenBy(x => x.Name)
                            .Where(x => x.Name.CompareTo(dto.AfterName) > 0 || x.Score < lastScore.FirstOrDefault());
                    }

                    break;
                }
                case TeamPaginationStrategy.Name:
                {
                    if (!string.IsNullOrEmpty(dto.AfterName))
                    {
                        filtered = dto.IsAscending
                            ? source.Where(x => x.Name.CompareTo(dto.AfterName) > 0)
                            : source.Where(x => x.Name.CompareTo(dto.AfterName) < 0);
                    }

                    filtered = dto.IsAscending
                        ? filtered.OrderBy(x => x.Name)
                        : filtered.OrderByDescending(x => x.Name);

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(dto.Strategy), dto.Strategy, null);
            }

            return filtered.Take(dto.PageSize);
        }
    }
}