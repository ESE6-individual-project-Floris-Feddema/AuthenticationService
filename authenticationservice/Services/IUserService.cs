using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using authenticationservice.Controllers;
using authenticationservice.Domain;
using authenticationservice.Views;

namespace authenticationservice.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Inserts a new user with the given information
        /// </summary>
        /// <param name="viewName">The name of the user</param>
        /// <param name="viewEmail">The unique password of the user</param>
        /// <param name="viewPassword">The plain password of the user</param>
        /// <returns>The user including a Guid and without password</returns>
        Task<User> Insert(string viewName, string viewEmail, string viewPassword);

        /// <summary>
        /// Creates a jwt based on the users guid if the given credentials are correct
        /// </summary>
        /// <param name="viewEmail">The email of the user</param>
        /// <param name="viewPassword">The plain password of the user</param>
        /// <returns>The user including jwt and without password</returns>
        Task<User> Login(string viewEmail, string viewPassword);

        /// <summary>
        /// Inserts a new Google authenticated user
        /// </summary>
        /// <param name="viewTokenId">TokenId from google OAuth</param>
        /// <returns>A new user with Guid and without OAuth information</returns>
        Task<User> InsertGoogle(string viewTokenId);

        /// <summary>
        /// Creates a jwt based on the users guid if the tokenId is valid
        /// </summary>
        /// <param name="viewTokenId">The tokenId of a valid Google OAuth user</param>
        /// <returns>The user including jwt and without OAuth information</returns>
        Task<User> LoginGoogle(string viewTokenId);

        /// <summary>
        /// Updates a users name to a new value
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="name">New name</param>
        /// <returns>Updated user object</returns>
        Task<User> Update(Guid id, string name);
    }
}