using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballLeague.Application.Common;
using FootballLeague.Application.Matches.Dtos;

namespace FootballLeague.Application.Matches
{
    public interface IMatchService
    {
        public Task<Result<IReadOnlyCollection<MatchDto>>> GetPageAsync(GetMatchesPageDto dto);
        public Task<Result<MatchWithDetailsDto>> FindByKeyAsync(Guid key);
        public Task<Result<MatchDto>> CreateAsync(CreateMatchDto dto);
        public Task<Result<MatchDto>> UpdateScoresByKeyAsync(Guid key, UpdateMatchScoresDto dto);
        public Task<Result<MatchDto>> DeleteByKeyAsync(Guid key);
    }
}