using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using authenticationservice.DatastoreSettings;
using authenticationservice.Domain;
using MongoDB.Driver;

namespace authenticationservice.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IUserstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UserCollectionName);
        }

        public async Task<List<User>> Get() => 
            await _users.Find(user => true).ToListAsync();

        public async Task<User> Get(string email) => 
            await _users.Find(user => user.Email == email).FirstOrDefaultAsync();

        public async Task<User> Get(Guid id) =>
            await _users.Find(book => book.Id == id).FirstOrDefaultAsync();

        public async Task<User> Create(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }
        
        public async Task Update(Guid id, User userIn) =>
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);

        public async Task Remove(User userIn) =>
            await _users.DeleteOneAsync(user => user.Id == userIn.Id);

        public async Task Remove(Guid id) => 
            await _users.DeleteOneAsync(user => user.Id == id); 
    }
}