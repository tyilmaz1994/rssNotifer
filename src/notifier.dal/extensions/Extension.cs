using MongoDB.Driver;
using notifier.dal.attributes;
using notifier.dal.context;
using notifier.dal.entities;
using System.Reflection;

namespace notifier.dal.extensions
{
    public static class Extension
    {

        /// <summary>
        /// Opens connection between project and mongoDb database in order to perform Db manuplations
        /// </summary>
        /// <typeparam name="T">type of entity in order to get collection name</typeparam>
        /// <param name="dbContext">context of mongoDB database</param>
        /// <returns>return new collection related with entity collection</returns>
        public static IMongoCollection<T> OpenConnection<T>(this INotifierDbContext dbContext) where T: BaseEntity
        {
            var client = new MongoClient(dbContext.ConnectionString);
            var database = client.GetDatabase(dbContext.DatabaseName);
            var collectionName = typeof(T).GetCustomAttribute<MongoCollectionAttribute>().Name;
            return database.GetCollection<T>(collectionName);
        }
    }
}
