using System;

namespace authenticationservice.DataTransferObjects
{
    public class PrivateUserDto
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public string Token { get; set; }
    }
}