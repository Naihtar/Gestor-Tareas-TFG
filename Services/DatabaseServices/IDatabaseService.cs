using MongoDB.Bson;
using MongoDB.Driver;
using TFG.Models;

namespace TFG.Services.DatabaseServices {
    public interface IDatabaseService {
        // Funciones genéricas
        Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionName);

        // Funciones para AppUser
        Task<AppUser?> GetUserByIDAsync(ObjectId appUserID);
        Task<AppUser?> GetUserByEmailAsync(string appUserEmail);
        Task<bool> UpdateUserAsync(AppUser appUserUpdate);
        Task<bool> CheckUserByEmailAsync(string appUserEmailCheck);
        Task<bool> CheckUserByEmailAsync(ObjectId appUserID, string appUserEmailCheck);
        Task<bool> CheckUserByUsernameAsync(ObjectId appUserID, string appUserUsernameCheck);
        Task<bool> CheckUserByUsernameAsync(string appUserUsernameCheck);
        Task<bool> VerifyPasswordByUserIDAsync(ObjectId appUserID, string appUserPasswordCheck);
        Task<bool> CreateUserAsync(AppUser appUserCreate);
        Task<bool> DeleteUserAsync(AppUser appUserDelete);

        // Funciones para AppContainer
        Task<AppContainer?> GetContainerByContainerIDAsync(ObjectId appContainerID);
        Task<bool> UpdateContainerAsync(AppContainer appContainerUpdate);
        Task<bool> CheckContainerByTitleAndUserIDAsync(string appContainerTitle, ObjectId appUserID);
        Task<bool> DeleteContainerRecursiveAsync(ObjectId appContainerIDDelete, ObjectId appUserID);
        Task<bool> AddContainerAsync(AppContainer appContainerCreate, ObjectId appUserID);
        Task<List<ObjectId>> GetContainerListByUserIDAsync(ObjectId appUserID);
        Task<bool> VerifyTaskInContainerAsync(ObjectId appContainerIDCheck);
        // Funciones para AppTask
        Task<AppTask?> GetTastkByIdAsync(ObjectId appTaskID);
        Task<List<AppTask>> GetTasksByContainerIdAsync(ObjectId appContainerID);
        Task<bool> UpdateTaskAsync(AppTask appTaskUpdate);
        Task<bool> DeleteTaskAsync(ObjectId appTaskID);
        Task<bool> AddTaskAsync(AppTask appTaskCreate, ObjectId appContainerID);
    }
}