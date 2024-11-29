namespace FootballLeague.Application.Common.Errors
{
    public abstract class Error
    {
        public string Name => GetType().Name;
        public string Description { get; }

        protected Error(string description)
        {
            Description = description;
        }
    }
}