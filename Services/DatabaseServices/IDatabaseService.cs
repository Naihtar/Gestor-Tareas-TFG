using MongoDB.Bson;
using MongoDB.Driver;
using TFG.Models;

namespace TFG.Services.DatabaseServices {
    public interface IDatabaseService {
        // Funciones genéricas
        Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionName);

        // Funciones para AppUser
        Task<AppUser> GetUserByIdAsync(ObjectId userId);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(AppUser user);
        Task<bool> CheckEmailByUserIDAsync(ObjectId userId, string emailUsuario);
        Task<bool> CheckUsernameByUserIDAsync(ObjectId userId, string aliasUsuario);
        Task<bool> VerifyPasswordAsync(ObjectId userId, string password);
        Task<bool> CheckEmailAsync(string email);
        Task<bool> CheckUsernameAsync(string username);
        Task<bool> CreateAppUser(AppUser user);
        Task<bool> DeleteAppUser(AppUser appUser);

        // Funciones para AppContainer
        Task<AppContainer> GetContainerByIdAsync(ObjectId containerId);
        Task<bool> UpdateContainerAsync(AppContainer container);
        Task<bool> ExistContainerWithName(string nombreContenedor, ObjectId userId);
        Task<bool> DeleteContainerAndTasks(ObjectId containerId, ObjectId userId);
        Task<bool> AddContainer(AppContainer container, ObjectId userId);
        Task<List<ObjectId>> GetListContianerUser(ObjectId userId);
        Task<bool> HasTasksInContainerAsync(ObjectId containerID);
        // Funciones para AppTask
        Task<AppTask> GetTastkByIdAsync(ObjectId taskId);
        Task<List<AppTask>> GetTasksByContainerIdAsync(ObjectId containerId);
        Task<bool> UpdateTaskAsync(AppTask task);
        Task<bool> DeleteTask(ObjectId taskId);
        Task<bool> AddTask(AppTask task, ObjectId containerId);
    }
}

