using System;

namespace authenticationservice.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message)
        {
            
        }
    }
}