using System;
using System.Linq.Expressions;

namespace authenticationservice.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
            
        }
    }
}