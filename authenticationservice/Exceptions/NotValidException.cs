using System;

namespace authenticationservice.Exceptions
{
    public class NotValidException : Exception
    {
        public NotValidException(string message) : base(message)
        {
            
        }
    }
}