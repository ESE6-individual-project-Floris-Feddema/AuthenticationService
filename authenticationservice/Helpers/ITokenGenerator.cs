using System;

namespace authenticationservice.Helpers
{
    public interface ITokenGenerator
    {
        string CreateToken(Guid id);
    }
}