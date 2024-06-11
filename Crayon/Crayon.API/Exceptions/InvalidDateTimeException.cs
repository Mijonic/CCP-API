namespace Crayon.API.Exceptions
{
    public class InvalidDateTimeException : Exception, ICustomException
    {
        public InvalidDateTimeException() { }
        public InvalidDateTimeException(string message) : base(message) { }
        public InvalidDateTimeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
