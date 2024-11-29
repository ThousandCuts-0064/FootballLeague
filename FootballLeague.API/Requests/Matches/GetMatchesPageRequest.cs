using System;
using System.ComponentModel.DataAnnotations;
using FootballLeague.Application.Matches.Dtos;

namespace FootballLeague.API.Requests.Matches
{
    public record GetMatchesPageRequest(
        Guid AfterKey = default,
        [Range(1, 50)] int PageSize = 25,
        bool IsAscending = true)
    {
        public GetMatchesPageDto ToDto() => new(AfterKey, PageSize, IsAscending);
    }
}