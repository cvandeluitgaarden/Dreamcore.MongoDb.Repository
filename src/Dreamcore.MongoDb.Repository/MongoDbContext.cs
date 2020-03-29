using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dreamcore.MongoDb.Repository
{
    public abstract class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoClient client;
        private readonly IMongoDatabase database;

        protected MongoDbContext(string connectionString) : this(new MongoUrl(connectionString))
        {
        }

        protected MongoDbContext(MongoUrl mongoUrl)
        {
            this.client = new MongoClient(mongoUrl);
            this.database = client.GetDatabase(mongoUrl.DatabaseName);
            InitMongoDbContext(this.database);
        }

        private void InitMongoDbContext(IMongoDatabase database)
        {
            foreach(var property in GetTableProperties(this))
            {
                var collection = CreateMongoCollectionInstance(property, database);
                var table = CreateTableInstance(property, collection);
                property.SetValue(this, table);
            }
        }

        private object CreateTableInstance(PropertyInfo property, object collection)
        {
            return Activator.CreateInstance(property.PropertyType, new object[] { collection });
        }
        
        private object  CreateMongoCollectionInstance(PropertyInfo property, IMongoDatabase database)
        {
            var entityTypeArg = property.PropertyType.GetGenericArguments()[0];
            var dbType = database.GetType();
            var method = dbType.GetMethod("GetCollection")
                .MakeGenericMethod(new Type[] { entityTypeArg });
            return method.Invoke(database, new object[] { property.Name });
        }

        private IEnumerable<PropertyInfo> GetTableProperties(MongoDbContext dbContext)
        {
            return dbContext.GetType().GetProperties()
                .Where(x => x.PropertyType.IsGenericType &&
               x.PropertyType.GetGenericTypeDefinition() == typeof(Table<>));
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
