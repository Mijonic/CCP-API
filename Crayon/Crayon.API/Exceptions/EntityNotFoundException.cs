namespace Crayon.API.Exceptions
{
    public class EntityNotFoundException : Exception, ICustomException
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
