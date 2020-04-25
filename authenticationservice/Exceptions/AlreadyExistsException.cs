using System;

namespace authenticationservice.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base("The object already exists")
        {
            
        }

        public AlreadyExistsException(string message) : base(message)
        {
            
        }
    }
}