using System.Collections.Generic;
using System.Threading.Tasks;
using authenticationservice.Controllers;
using authenticationservice.Domain;

namespace authenticationservice.Services
{
    public interface IUserService
    {
       Task<User> Insert(string viewName, string viewEmail, string viewPassword);
       Task<User> Login(string viewEmail, string viewPassword);
       Task<User> InsertGoogle(string viewTokenId);
       Task<User> LoginGoogle(string viewTokenId);
    }
}