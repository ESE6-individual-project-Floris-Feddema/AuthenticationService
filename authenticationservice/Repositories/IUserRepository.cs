using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace authenticationservice.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a list of all the users
        /// </summary>
        /// <returns>List of all users</returns>
        Task<List<User>> Get();
        
        /// <summary>
        /// Gets a single user by their email address
        /// </summary>
        /// <param name="email">the email address to be searched for</param>
        /// <returns>User with the correct email address</returns>
        Task<User> Get(string email);
        
        /// <summary>
        /// <param name="id">Guid to search for</para
        /// Gets a single user by their Guid
        /// </summary>m>
        /// <returns>User with the correct Guid</returns>
        Task<User> Get(Guid id);
        
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User to be saved</param>
        /// <returns></returns>
        Task<User> Create(User user);
        
        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <param name="userIn">User with updated fields</param>
        /// <returns>Updated user</returns>
        Task Update(Guid id, User userIn);
        
        /// <summary>
        /// Removes an user
        /// </summary>
        /// <param name="userIn">User to remove</param>
        void Remove(User userIn);
        
        /// <summary>
        /// Removes an user by their Guid
        /// </summary>
        /// <param name="id">Guid of the user te remove</param>
        /// <returns>Async tas to await</returns>
        Task Remove(Guid id);
    }
}