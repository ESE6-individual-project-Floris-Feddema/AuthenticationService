using System;

namespace authenticationservice.DataTransferObjects
{
    public class PublicUserDto
    { 
        public Guid Id {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
    }
}