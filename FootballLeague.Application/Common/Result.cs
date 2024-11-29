using FootballLeague.Application.Common.Errors;

namespace FootballLeague.Application.Common
{
    public static class Result
    {
        public static Result<T> Success<T>(T value) => new(true, value, null);
        public static Result<T> Error<T>(Error error, T value = default) => new(false, value, error);
    }

    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public Error Error { get; }

        internal Result(bool isSuccess, T value, Error error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }
    }
}