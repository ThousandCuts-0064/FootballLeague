using System;
using FootballLeague.Application.Common.Errors;

namespace FootballLeague.Application.Matches.Utils
{
    internal static class MatchErrors
    {
        public static NotFoundError NotFound(Guid key) => new($"Match '{key}' was not found.");

        public static ValidationError BothTeamsAreTheSame(string teamName) =>
            new($"Team '{teamName}' cannot play against itself.");

        public static ValidationError StartedAtWasNotInThePast(DateTime startedAt) =>
            new($"Match could not have started at '{startedAt}' which is in the future.");

        public static ConflictError AlreadyExists(string team1Name, string team2Name, DateTime startedAt) =>
            new($"Match between '{team1Name}' and '{team2Name}' at '{startedAt}' already exists.");
    }
}