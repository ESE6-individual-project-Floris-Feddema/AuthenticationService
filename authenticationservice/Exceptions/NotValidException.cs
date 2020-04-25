using System;

namespace authenticationservice.Exceptions
{
    public class NotValidException : Exception
    {
        public NotValidException() : base("The given input is not valid")
        {
            
        }

        public NotValidException(string message) : base(message)
        {
            
        }
    }
}