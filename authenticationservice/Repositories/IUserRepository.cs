using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace authenticationservice.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> Get();
        Task<User> Get(string email);
        Task<User> Get(Guid id);
        Task<User> Create(User user);
        Task Update(Guid id, User userIn);
        void Remove(User userIn);
        Task Remove(Guid id);
    }
}