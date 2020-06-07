using System.Collections;
using System.Collections.Generic;
using System.Linq;
using authenticationservice.DataTransferObjects;
using authenticationservice.Domain;

namespace authenticationservice.Helpers
{
    public static class ExtentionMethods
    {
        public static List<User> WithoutPasswords(this List<User> users)
        {
            return users.Select(x => x.WithoutPassword()).ToList();
        }

        public static User WithoutPassword(this User user)
        {
            user.Password = null;
            user.OauthSubject = null;
            user.OauthIssuer = null;
            user.Salt = null;
            return user;
        }

        public static PrivateUserDto ToPrivateDto(this User user)
        {
            return new PrivateUserDto()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Token = user.Token
            };
        }
        
        public static PublicUserDto ToPublicDto(this User user)
        {
            return new PublicUserDto()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
            };
        }
    }
}