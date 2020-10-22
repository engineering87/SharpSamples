// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using MongoDB.Driver;

namespace SampleMongoDbRepository
{
    public class MongoDbContext : IMongoDbContext
    {
        private IMongoDatabase MongoDatabase { get; set; }
        private MongoClient MongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }

        public MongoDbContext(string connection, string database)
        {
            MongoClient = new MongoClient(connection);
            MongoDatabase = MongoClient.GetDatabase(database);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return string.IsNullOrEmpty(name) ? null : MongoDatabase.GetCollection<T>(name);
        }
    }
}
