using System.Threading.Tasks;

namespace Dreamcore.MongoDb.Repository
{
    public interface IMongoDbContext
    {
        Task SaveChangesAsync();
    }
}
