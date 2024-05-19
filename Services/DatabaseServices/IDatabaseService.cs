using MongoDB.Bson;
using MongoDB.Driver;
using TFG.Models;

namespace TFG.Services.DatabaseServices {
    public interface IDatabaseService {
        Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionName);
        Task<AppUser> GetUserByIdAsync(ObjectId userId);
        Task UpdateUserAsync(AppUser user);
        Task<bool> ExistEmailDB(ObjectId userId, string emailUsuario);
        Task<bool> ExistAliasDB(ObjectId userId, string aliasUsuario);
        Task<bool> VerifyPasswordAsync(ObjectId userId, string password);
        Task<AppContainer> GetContainerByIdAsync(ObjectId containerId);
        Task UpdateContainerAsync(AppContainer container);
        Task<bool> ExistContainerWithName(string nombreContenedor, ObjectId userId);
        Task<AppTask> GetTastkByIdAsync(ObjectId taskId);
        Task<List<AppTask>> GetTasksByContainerIdAsync(ObjectId containerId);
        Task<bool> DeleteContainerAndTasks(ObjectId containerId, ObjectId userId);
        Task<bool> AddContainer(AppContainer container, ObjectId userId);
        Task<List<ObjectId>> GetListContianerUser(ObjectId userId);
        Task UpdateTaskAsync(AppTask task);
        Task<bool> DeleteTask(ObjectId taskId);
        Task<bool> AddTask(AppTask task, ObjectId containerId);
    }
}
