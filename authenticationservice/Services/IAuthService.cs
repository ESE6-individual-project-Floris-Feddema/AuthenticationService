using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace authenticationservice.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
        
        /// <summary>
        /// Fills the database with 3 pre defined users if they do not exists
        /// </summary>
        /// <returns>the async task to be awaited</returns>
        Task Fill();
        
        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns>the async task with the list of all users as a result</returns>
        Task<List<User>> Get();
    }
}