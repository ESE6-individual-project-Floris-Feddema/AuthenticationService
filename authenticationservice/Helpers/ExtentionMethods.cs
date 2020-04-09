using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            user.Salt = null;
            return user;
        }
    }
}