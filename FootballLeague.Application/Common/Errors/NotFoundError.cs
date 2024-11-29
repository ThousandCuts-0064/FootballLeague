namespace FootballLeague.Application.Common.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string description) : base(description) { }
    }
}