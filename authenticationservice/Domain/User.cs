using System;
using MongoDB.Bson.Serialization.Attributes;

namespace authenticationservice
{
    public class User
    {
        [BsonId]
        public Guid Id {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public string OauthSubject {get; set;}
        public string OauthIssuer {get; set;}
    }
}