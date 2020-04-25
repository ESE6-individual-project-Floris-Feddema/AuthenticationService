using System;
using System.Linq.Expressions;

namespace authenticationservice.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("The object is not found")
        {
            
        }

        public NotFoundException(string message) : base(message)
        {
            
        }
    }
}