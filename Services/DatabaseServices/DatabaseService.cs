using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows;
using TFG.Database;
using TFG.Models;

namespace TFG.Services.DatabaseServices {
    public class DatabaseService(IDatabaseConnection connectionDB) : IDatabaseService {

        private readonly IDatabaseConnection _dataBase = connectionDB;

        public async Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionName) {
            return await Task.FromResult(_dataBase.GetCollection<T>(collectionName));
        }

        // AppUser Methods
        public async Task<AppUser> GetUserByIdAsync(ObjectId userId) {
            var users = await GetCollectionAsync<AppUser>("usuarios");
            return await users.Find(userSearch => userSearch.IdUsuario == userId).FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByEmailAsync(string email) {
            var users = await GetCollectionAsync<AppUser>("usuarios");
            return await users.Find(userSearch => userSearch.EmailUsuario == email).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUserAsync(AppUser user) {
            var users = await GetCollectionAsync<AppUser>("usuarios");
            var filter = Builders<AppUser>.Filter.Eq(userSearch => userSearch.IdUsuario, user.IdUsuario);
            var result = await users.ReplaceOneAsync(filter, user);
            return result.IsAcknowledged;
        }

        public async Task<bool> CheckUsernameAsync(string userName) {
            var users = await GetCollectionAsync<AppUser>("usuarios");
            var filter = Builders<AppUser>.Filter.Eq("aliasUsuario", userName);
            return await users.Find(filter).AnyAsync();
        }

        public async Task<bool> CheckEmailAsync(string userEmail) {
            var users = await GetCollectionAsync<AppUser>("usuarios");
            var filter = Builders<AppUser>.Filter.Eq("email", userEmail);
            return await users.Find(filter).AnyAsync();
        }

        public async Task<bool> CheckEmailByUserIDAsync(ObjectId userId, string emailUsuario) {
            var users = await GetCollectionAsync<AppUser>("usuarios");
            var filter = Builders<AppUser>.Filter.And(
                Builders<AppUser>.Filter.Eq("email", emailUsuario),
                Builders<AppUser>.Filter.Ne("_id", userId)
            );
            return await users.Find(filter).AnyAsync();
        }

        public async Task<bool> CheckUsernameByUserIDAsync(ObjectId userId, string aliasUsuario) {
            var collection = await GetCollectionAsync<AppUser>("usuarios");
            var filter = Builders<AppUser>.Filter.And(
                Builders<AppUser>.Filter.Eq("aliasUsuario", aliasUsuario),
                Builders<AppUser>.Filter.Ne("_id", userId)
            );
            return await collection.Find(filter).AnyAsync();
        }

        public async Task<bool> VerifyPasswordAsync(ObjectId userId, string password) {
            var users = await GetCollectionAsync<AppUser>("usuarios");
            var user = await users.Find(user => user.IdUsuario == userId).FirstOrDefaultAsync();
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordUsuario);
        }

        public async Task<List<ObjectId>> GetListContianerUser(ObjectId userId) {
            var collection = await GetCollectionAsync<AppUser>("usuarios");
            var user = await collection.Find(appUser => appUser.IdUsuario == userId).FirstOrDefaultAsync();
            return user?.ListaContenedoresUsuario ?? new List<ObjectId>();
        }

        public async Task<bool> CreateAppUser(AppUser user) {
            var collection = await GetCollectionAsync<AppUser>("usuarios");
            try {
                await collection.InsertOneAsync(user);
                return true;
            } catch (Exception ex) {
                MessageBox.Show($"Error al intentar crear la tarea. {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteAppUser(AppUser appUser) {
            try {
                var userCollection = await GetCollectionAsync<AppUser>("usuarios");
                var containerCollection = await GetCollectionAsync<AppContainer>("contenedores");
                var taskCollection = await GetCollectionAsync<AppTask>("tareas");

                // Crear un filtro para encontrar todos los contenedores asociados al usuario
                var containerFilter = Builders<AppContainer>.Filter.Eq(c => c.UsuarioID, appUser.IdUsuario);
                var containers = await containerCollection.Find(containerFilter).ToListAsync();

                // Borrado de tareas y contenedores
                foreach (var container in containers) {
                    var taskFilter = Builders<AppTask>.Filter.Eq(t => t.ContenedorID, container.IdContenedor);

                    // Borramos las tareas asociadas al contenedor
                    await taskCollection.DeleteManyAsync(taskFilter);

                    // Borramos el contenedor
                    await containerCollection.DeleteOneAsync(Builders<AppContainer>.Filter.Eq(c => c.IdContenedor, container.IdContenedor));
                }

                // Finalmente, borrar el usuario
                var userFilter = Builders<AppUser>.Filter.Eq(u => u.IdUsuario, appUser.IdUsuario);
                var userResult = await userCollection.DeleteOneAsync(userFilter);

                return userResult.DeletedCount > 0;
            } catch (Exception ex) {
                MessageBox.Show($"Error al intentar eliminar el usuario y sus contenedores y tareas asociadas. {ex.Message}");
                return false;
            }
        }

        // AppContainer Methods
        public async Task<AppContainer> GetContainerByIdAsync(ObjectId containerId) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores");
            var filter = Builders<AppContainer>.Filter.Eq(c => c.IdContenedor, containerId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateContainerAsync(AppContainer container) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores");
            var filter = Builders<AppContainer>.Filter.Eq(containerSearch => containerSearch.IdContenedor, container.IdContenedor);
            var result = await collection.ReplaceOneAsync(filter, container);
            return result.IsAcknowledged;
        }

        public async Task<bool> ExistContainerWithName(string nombreContenedor, ObjectId userId) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores");
            var filter = Builders<AppContainer>.Filter.And(
                Builders<AppContainer>.Filter.Eq("nombre", nombreContenedor),
                Builders<AppContainer>.Filter.Eq("usuario_id", userId)
            );
            return await collection.Find(filter).AnyAsync();
        }

        public async Task<bool> HasTasksInContainerAsync(ObjectId containerID) {
            try {
                var collection = await GetCollectionAsync<AppContainer>("contenedores");
                var filter = Builders<AppContainer>.Filter.Eq(c => c.IdContenedor, containerID);
                var container = await collection.Find(filter).FirstOrDefaultAsync();
                return container != null && container.ListaTareas.Count > 0;
            } catch (Exception ex) {
                MessageBox.Show($"Error obtaining tasks from container: {ex.Message}");
                return false;
            }
        }




        public async Task<bool> DeleteContainerAndTasks(ObjectId idContenedor, ObjectId userId) {
            try {
                var containerCollection = await GetCollectionAsync<AppContainer>("contenedores");
                var containerFilter = Builders<AppContainer>.Filter.Eq(c => c.IdContenedor, idContenedor);
                var container = await containerCollection.Find(containerFilter).FirstOrDefaultAsync();

                if (container != null) {
                    var taskCollection = await GetCollectionAsync<AppTask>("tareas");
                    var taskFilter = Builders<AppTask>.Filter.Eq(t => t.ContenedorID, idContenedor);
                    var taskResult = await taskCollection.DeleteManyAsync(taskFilter);

                    if (taskResult.DeletedCount > 0 || taskResult.DeletedCount == 0) {
                        var containerResult = await containerCollection.DeleteOneAsync(containerFilter);
                        if (containerResult.DeletedCount > 0) {
                            var userCollection = await GetCollectionAsync<AppUser>("usuarios");
                            var userFilter = Builders<AppUser>.Filter.Eq(u => u.IdUsuario, userId);
                            var userUpdate = Builders<AppUser>.Update.Pull(u => u.ListaContenedoresUsuario, idContenedor);
                            await userCollection.UpdateOneAsync(userFilter, userUpdate);
                        }
                        return containerResult.DeletedCount > 0;
                    }
                }
                return false;
            } catch (Exception ex) {
                MessageBox.Show($"Error al intentar eliminar el contenedor y sus tareas asociadas. {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddContainer(AppContainer container, ObjectId userId) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores");
            var userCollection = await GetCollectionAsync<AppUser>("usuarios");
            try {
                await collection.InsertOneAsync(container);
                var user = await userCollection.Find(u => u.IdUsuario == userId).FirstOrDefaultAsync();

                if (user != null) {
                    user.ListaContenedoresUsuario.Add(container.IdContenedor);
                    var update = Builders<AppUser>.Update.Set(appUser => appUser.ListaContenedoresUsuario, user.ListaContenedoresUsuario);
                    var result = await userCollection.UpdateOneAsync(appUser => appUser.IdUsuario == userId, update);
                    return result.IsAcknowledged;
                }

                return false;
            } catch (Exception ex) {
                MessageBox.Show($"Error al intentar crear el contenedor. {ex.Message}");
                return false;
            }
        }

        // AppTask Methods
        public async Task<AppTask> GetTastkByIdAsync(ObjectId taskId) {
            var collection = await GetCollectionAsync<AppTask>("tareas");
            var filter = Builders<AppTask>.Filter.Eq(t => t.IdTarea, taskId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<AppTask>> GetTasksByContainerIdAsync(ObjectId containerId) {
            var collection = await GetCollectionAsync<AppTask>("tareas");
            var filter = Builders<AppTask>.Filter.Eq(t => t.ContenedorID, containerId);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateTaskAsync(AppTask task) {
            var tasks = await GetCollectionAsync<AppTask>("tareas");
            var filter = Builders<AppTask>.Filter.Eq(tarea => tarea.IdTarea, task.IdTarea);
            var result = await tasks.ReplaceOneAsync(filter, task);
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteTask(ObjectId idTarea) {
            try {
                var taskCollection = await GetCollectionAsync<AppTask>("tareas");
                var taskFilter = Builders<AppTask>.Filter.Eq(t => t.IdTarea, idTarea);
                var task = await taskCollection.Find(taskFilter).FirstOrDefaultAsync();

                if (task != null) {
                    var containerCollection = await GetCollectionAsync<AppContainer>("contenedores");
                    var containerFilter = Builders<AppContainer>.Filter.AnyEq(container => container.ListaTareas, idTarea);
                    var containers = await containerCollection.Find(containerFilter).ToListAsync();
                    var containerUpdate = Builders<AppContainer>.Update.Pull(container => container.ListaTareas, idTarea);
                    foreach (var container in containers) {
                        await containerCollection.UpdateOneAsync(containerFilter, containerUpdate);
                    }
                    var taskResult = await taskCollection.DeleteOneAsync(taskFilter);
                    return taskResult.DeletedCount > 0;
                }
                return false;
            } catch (Exception ex) {
                MessageBox.Show($"Error al intentar eliminar la tarea y removerla de los contenedores. {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddTask(AppTask appTask, ObjectId containerId) {
            var collection = await GetCollectionAsync<AppTask>("tareas");
            var containerCollection = await GetCollectionAsync<AppContainer>("contenedores");

            try {
                await collection.InsertOneAsync(appTask);
                var container = await containerCollection.Find(container => container.IdContenedor == containerId).FirstOrDefaultAsync();

                if (container != null) {
                    container.ListaTareas.Add(appTask.IdTarea);
                    var update = Builders<AppContainer>.Update.Set(appContainer => appContainer.ListaTareas, container.ListaTareas);
                    var result = await containerCollection.UpdateOneAsync(appContainer => appContainer.IdContenedor == containerId, update);
                    return result.IsAcknowledged;
                }

                return false;
            } catch (Exception ex) {
                MessageBox.Show($"Error al intentar crear la tarea. {ex.Message}");
                return false;
            }
        }
    }
}
