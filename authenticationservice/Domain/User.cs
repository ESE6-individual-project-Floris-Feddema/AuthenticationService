using System;
using MongoDB.Bson.Serialization.Attributes;

namespace authenticationservice.Domain
{
    public class User
    {
        [BsonId]
        public Guid Id {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public string OauthSubject {get; set;}
        public string OauthIssuer {get; set;}
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Token { get; set; }
    }
}