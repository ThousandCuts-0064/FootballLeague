namespace FootballLeague.Application.Common.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(string description) : base(description) { }
    }
}