using FootballLeague.Application.Common.Errors;

namespace FootballLeague.Application.Teams.Utils
{
    internal static class TeamErrors
    {
        public static NotFoundError NotFound(string name) => new($"Team '{name}' was not found.");

        public static ValidationError NameCannotBeUrlEncoded(string name) =>
            new($"Team name '{name}' cannot be URL-encoded.");

        public static ConflictError AlreadyExists(string name) => new($"Team '{name}' already exists.");

        public static ConflictError CannotDeleteTeamWhileExistingInMatches(string name) =>
            new($"Cannot delete team '{name}' while existing in matches.");
    }
}