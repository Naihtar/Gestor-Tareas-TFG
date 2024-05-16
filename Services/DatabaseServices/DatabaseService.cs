using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows;
using TFG.Database;
using TFG.Models;
using TFGDesktopApp.Models;

namespace TFG.Services.DatabaseServices {
    public class DatabaseService(IDatabaseConnection connectionDB) : IDatabaseService {

        //Atributos
        private readonly IDatabaseConnection _dataBase = connectionDB;

        // Método genérico.
        public async Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionName) {
            return await Task.FromResult(_dataBase.GetCollection<T>(collectionName));
        }

        // Método usuario.
        public async Task<AppUser> GetUserByIdAsync(ObjectId userId) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await GetCollectionAsync<AppUser>("usuarios");

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(userSearch => userSearch.IdUsuario == userId).FirstOrDefaultAsync();

            return user;
        }

        public async Task UpdateUserAsync(AppUser user) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await GetCollectionAsync<AppUser>("usuarios");

            // Crea un filtro para encontrar al usuario por su ID
            var filter = Builders<AppUser>.Filter.Eq(userSearch => userSearch.IdUsuario, user.IdUsuario);

            // Actualiza el usuario en la colección de forma asíncrona
            await users.ReplaceOneAsync(filter, user);
        }

        public async Task<bool> ExistEmailDB(ObjectId userId, string emailUsuario) {
            var users = await GetCollectionAsync<AppUser>("usuarios");

            // Crea un filtro para buscar un usuario con el mismo email pero diferente al usuario actual
            var filter = Builders<AppUser>.Filter.And(
                Builders<AppUser>.Filter.Eq("email", emailUsuario),
                Builders<AppUser>.Filter.Ne("_id", userId)
            );

            // Verifica si existe algún usuario que coincida con el filtro
            return await users.Find(filter).AnyAsync();
        }
        public async Task<bool> ExistAliasDB(ObjectId userId, string aliasUsuario) {
            var collection = await GetCollectionAsync<AppUser>("usuarios");

            var filter = Builders<AppUser>.Filter.And(
                Builders<AppUser>.Filter.Eq("aliasUsuario", aliasUsuario),
                Builders<AppUser>.Filter.Ne("_id", userId)
            );

            return await collection.Find(filter).AnyAsync();
        }

        // Método usuario - Verificar password.
        public async Task<bool> VerifyPasswordAsync(ObjectId userId, string password) {

            var users = await GetCollectionAsync<AppUser>("usuarios");

            var user = await users.Find(user => user.IdUsuario == userId).FirstOrDefaultAsync();

            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordUsuario)) {
                return true;
            }
            return false;
        }

        public async Task<List<ObjectId>> GetListContianerUser(ObjectId userId) {

            var collection = await GetCollectionAsync<AppUser>("usuarios");

            var user = await collection.Find(appUser => appUser.IdUsuario == userId).FirstOrDefaultAsync();

            return user?.ListaContenedoresUsuario ?? [];
        }

        // Metodo Contenedor:

        public async Task<AppContainer> GetContainerByIdAsync(ObjectId containerId) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores");
            var filter = Builders<AppContainer>.Filter.Eq(c => c.IdContenedor, containerId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateContainerAsync(AppContainer container) {
            // Obtener la colección de contenedores.
            var collection = await GetCollectionAsync<AppContainer>("contenedores");

            // Crea un filtro para encontrar el contenedor deseado.
            var filter = Builders<AppContainer>.Filter.Eq(containerSearch => containerSearch.IdContenedor, container.IdContenedor);

            // Actualizar el contenedor.
            await collection.ReplaceOneAsync(filter, container);
        }

        public async Task<bool> ExistContainerWithName(string nombreContenedor, ObjectId userId) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores");

            var filter = Builders<AppContainer>.Filter.And(
                Builders<AppContainer>.Filter.Eq("nombre", nombreContenedor),
                Builders<AppContainer>.Filter.Eq("usuario_id", userId)
            );

            return await collection.Find(filter).AnyAsync();
        }

        public async Task<bool> DeleteContainerAndTasks(ObjectId idContenedor, ObjectId userId) {
            try {
                // Obtén la colección de contenedores
                var containerCollection = await GetCollectionAsync<AppContainer>("contenedores");

                // Crea un filtro para encontrar el contenedor
                var containerFilter = Builders<AppContainer>.Filter.Eq(c => c.IdContenedor, idContenedor);

                // Encuentra el contenedor
                var container = await containerCollection.Find(containerFilter).FirstOrDefaultAsync();

                if (container != null) {
                    // Obtén la colección de tareas
                    var taskCollection = await GetCollectionAsync<AppTask>("tareas");

                    // Crea un filtro para encontrar las tareas asociadas al contenedor
                    var taskFilter = Builders<AppTask>.Filter.Eq(t => t.ContenedorID, idContenedor);

                    // Elimina todas las tareas asociadas al contenedor
                    var taskResult = await taskCollection.DeleteManyAsync(taskFilter);

                    // Si se eliminaron las tareas o no había tareas asociadas, elimina el contenedor
                    if (taskResult.DeletedCount > 0 || taskResult.DeletedCount == 0) {
                        var containerResult = await containerCollection.DeleteOneAsync(containerFilter);

                        // Si se elimina el contenedor, actualiza la lista de contenedores del usuario
                        if (containerResult.DeletedCount > 0) {
                            var userCollection = await GetCollectionAsync<AppUser>("usuarios");
                            var userFilter = Builders<AppUser>.Filter.Eq(u => u.IdUsuario, userId);

                            // Remueve el ID del contenedor de la lista de contenedores del usuario
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


        // Metodo Tasks:

        public async Task<AppTask> GetTestkByIdAsync(ObjectId taskId) {
            var collection = await GetCollectionAsync<AppTask>("tareas");
            var filter = Builders<AppTask>.Filter.Eq(t => t.ContenedorID, taskId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<AppTask>> GetTasksByContainerIdAsync(ObjectId containerId) {
            var collection = await GetCollectionAsync<AppTask>("tareas");
            var filter = Builders<AppTask>.Filter.Eq(t => t.ContenedorID, containerId);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<bool> AddContainer(AppContainer container, ObjectId userId) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores");
            var userCollection = await GetCollectionAsync<AppUser>("usuarios");

            try {
                await collection.InsertOneAsync(container);

                // Obtén el usuario de la base de datos
                var user = await userCollection.Find(u => u.IdUsuario == userId).FirstOrDefaultAsync();

                if (user != null) {
                    // Agrega el ID del contenedor a la lista de contenedores del usuario
                    user.ListaContenedoresUsuario.Add(container.IdContenedor);

                    // Actualiza el usuario en la base de datos
                    var update = Builders<AppUser>.Update.Set(appUser => appUser.ListaContenedoresUsuario, user.ListaContenedoresUsuario);
                    await userCollection.UpdateOneAsync(appUser => appUser.IdUsuario == userId, update);
                }

                return true;
            } catch (Exception ex) {
                MessageBox.Show($"Error al intentar crear el contenedor. {ex.Message}");
                return false;
            }
        }


    }
}
