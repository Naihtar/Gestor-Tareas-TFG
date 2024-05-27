using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows;
using TFG.Database;
using TFG.Models;

namespace TFG.Services.DatabaseServices {
    public class DatabaseService(IDatabaseConnection connectionDataBase) : IDatabaseService {

        private readonly IDatabaseConnection _dataBase = connectionDataBase; //Dependencia de la conexión a la base de datos.

        //Obtener una colección desde la base de datos.
        public async Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionDatabaseName) {
            return await Task.FromResult(_dataBase.GetCollection<T>(collectionDatabaseName));
        }

        // AppUser Methods

        //Obtener el un usuario por el ID.
        public async Task<AppUser?> GetUserByIDAsync(ObjectId appUserID) {
            try {
                var users = await GetCollectionAsync<AppUser>("usuarios"); //Colección de usuarios
                return await users.Find(appUserSearchByID => appUserSearchByID.AppUserID == appUserID).FirstOrDefaultAsync(); //Operación en la base de datos
            } catch (Exception) {
                return null;
            }
        }

        //Obtener el usuario por el email.
        public async Task<AppUser?> GetUserByEmailAsync(string appUserEmail) {

            try {
                var users = await GetCollectionAsync<AppUser>("usuarios"); //Colección de usuarios
                return await users.Find(appUserSearchByEmail => appUserSearchByEmail.AppUserEmail == appUserEmail).FirstOrDefaultAsync(); //Operación en la base de datos
            } catch (Exception) {
                return null;
            }
        }

        //Actualizar los valores del usuario.
        public async Task<bool> UpdateUserAsync(AppUser appUserUpdate) {
            try {
                var users = await GetCollectionAsync<AppUser>("usuarios"); //Colección de usuarios
                var filter = Builders<AppUser>.Filter.Eq(appUserSearchUpdate => appUserSearchUpdate.AppUserID, appUserUpdate.AppUserID); //Filtro de búsqueda por ID
                var result = await users.ReplaceOneAsync(filter, appUserUpdate); //Operación en la base de datos
                return result.IsAcknowledged; //Confirmación de los cambios
            } catch (Exception) {
                return false;
            }
        }

        //Comprobar si el usuario existe en la base de datos, por su "username"
        public async Task<bool> CheckUserByUsernameAsync(string appUserUsernameCheck) {
            try {
                var users = await GetCollectionAsync<AppUser>("usuarios"); //Colección de usuarios
                var filter = Builders<AppUser>.Filter.Eq("aliasUsuario", appUserUsernameCheck); //Filtro de búsqueda por username
                return await users.Find(filter).AnyAsync(); //Resultados de la búsqueda
            } catch (Exception) {
                return true;
            }
        }
        //Comprobar si el Username existe y esta ligado a un usuario que no sea el actual.
        public async Task<bool> CheckUserByUsernameAsync(ObjectId appUserID, string appUserUsernameCheck) {
            try {
                var collection = await GetCollectionAsync<AppUser>("usuarios");//Coleción de usuarios.

                //Filtro de búsqueda por Username e ID del usuario.
                var filter = Builders<AppUser>.Filter.And(
                    Builders<AppUser>.Filter.Eq("aliasUsuario", appUserUsernameCheck),
                    Builders<AppUser>.Filter.Ne("_id", appUserID) // Usamos el Not Equal (NE) para evitar que coja el ID de nuestro usuario.
                );
                return await collection.Find(filter).AnyAsync(); //Resultados de la búsqueda
            } catch (Exception) {
                return true;
            }
        }

        //Comprobar si el correo existe en la base de datos a la hora de crear una cuenta nueva
        public async Task<bool> CheckUserByEmailAsync(string appUserEmailCheck) {
            try {
                var users = await GetCollectionAsync<AppUser>("usuarios"); //Colección de usuarios
                var filter = Builders<AppUser>.Filter.Eq("email", appUserEmailCheck); //Filtro de búsqueda por email
                return await users.Find(filter).AnyAsync(); //Resultados de la búsqueda
            } catch (Exception) {
                return true;
            }
        }

        //Comprobar si el correo existe y esta ligado a un usuario que no sea el actual.
        public async Task<bool> CheckUserByEmailAsync(ObjectId appUserID, string appUserEmailCheck) {
            try {
                var users = await GetCollectionAsync<AppUser>("usuarios"); //Coleción de usuarios.
                //Filtro de búsqueda por Email e ID del usuario.
                var filter = Builders<AppUser>.Filter.And(
                    Builders<AppUser>.Filter.Eq("email", appUserEmailCheck),
                    Builders<AppUser>.Filter.Ne("_id", appUserID) // Usamos el Not Equal (NE) para evitar que coja el ID de nuestro usuario.
                );
                return await users.Find(filter).AnyAsync(); //Resultados de la búsqueda
            } catch (Exception) {
                return true;
            }
        }

        //Comprobar si la contraseña introducida coincide con la contraseña del usuario.
        public async Task<bool> VerifyPasswordByUserIDAsync(ObjectId appUserID, string appUserPasswordCheck) {
            try {
                var users = await GetCollectionAsync<AppUser>("usuarios"); //Colección de usuarios
                var user = await users.Find(appUserCheck => appUserCheck.AppUserID == appUserID).FirstOrDefaultAsync(); //Busqueda del usuario por ID
                // Resultado de la busqueda, comprobando que el usuario exista y la contraseña coincida.
                return user != null && BCrypt.Net.BCrypt.Verify(appUserPasswordCheck, user.AppUserPassword);
            } catch (Exception) {
                return false;

            }
        }

        //Obtener la lista de contenedores, usando el ID del usuario.
        public async Task<List<ObjectId>> GetContainerListByUserIDAsync(ObjectId appUserID) {
            try {
                var collection = await GetCollectionAsync<AppUser>("usuarios"); //Coleción de usuarios 
                var user = await collection.Find(appUserCheck => appUserCheck.AppUserID == appUserID).FirstOrDefaultAsync();  //Busqueda del usuario por ID,
                return user?.AppUserAppContainerList ?? []; //Resultados de la búsqueda, en caso de ser null retorna una lista vácia

            } catch (Exception) {
                return [];
            }
        }

        //Agregar un nuevo usuario en la base de datos.
        public async Task<bool> CreateUserAsync(AppUser appUserCreate) {
            var collection = await GetCollectionAsync<AppUser>("usuarios"); //Colección de usuarios
            try {
                await collection.InsertOneAsync(appUserCreate); //Guardar en la base de datos el nuevo usuario
                return true; //Resultado de la operación
            } catch (Exception) {
                return false;
            }
        }

        //Eliminar un usuario de la base de datos y su contenido.
        public async Task<bool> DeleteUserAsync(AppUser appUserDelete) {
            try {

                //Colecciones
                var userCollection = await GetCollectionAsync<AppUser>("usuarios");
                var containerCollection = await GetCollectionAsync<AppContainer>("contenedores");
                var taskCollection = await GetCollectionAsync<AppTask>("tareas");

                //Filtro de búsqueda de los contenedores del usuario
                var containerFilter = Builders<AppContainer>.Filter.Eq(appContainerFilter => appContainerFilter.AppUserID, appUserDelete.AppUserID);
                var containers = await containerCollection.Find(containerFilter).ToListAsync(); //Resultado de la búsqueda

                // Borrado de tareas y contenedores
                foreach (var container in containers) {

                    //Filtro de búsqueda de las tareas vinculadas a ese contendor.
                    var taskFilter = Builders<AppTask>.Filter.Eq(appTaskFilter => appTaskFilter.AppContainerID, container.AppContainerID);

                    // Borramos las tareas vinculadas al contenedor
                    await taskCollection.DeleteManyAsync(taskFilter);

                    // Borramos el contenedor
                    await containerCollection.DeleteOneAsync(Builders<AppContainer>.Filter.Eq(appContainerFilter => appContainerFilter.AppContainerID, container.AppContainerID));
                }

                //Filtro de busqueda del usuario.
                var userFilter = Builders<AppUser>.Filter.Eq(appUserDelete => appUserDelete.AppUserID, appUserDelete.AppUserID);
                var userResult = await userCollection.DeleteOneAsync(userFilter); //Operación de borrado
                return userResult.DeletedCount > 0; //Resultado de la operación.
            } catch (Exception) {
                return false;
            }
        }
        // AppContainer Methods

        //Obtener el AppContainer através del ID
        public async Task<AppContainer?> GetContainerByContainerIDAsync(ObjectId appContainerID) {
            try {

                var collection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores
                var filter = Builders<AppContainer>.Filter.Eq(appContainerFilter => appContainerFilter.AppContainerID, appContainerID); //Filtro de búsqueda usando el ID
                return await collection.Find(filter).FirstOrDefaultAsync(); //Resultados de la busqueda.
            } catch (Exception) {
                return null;
            }
        }
        //Actualizar el AppContainer en la base de datos.
        public async Task<bool> UpdateContainerAsync(AppContainer appContainerUpdate) {
            try {

                var collection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores
                var filter = Builders<AppContainer>.Filter.Eq(appContainerFilter => appContainerFilter.AppContainerID, appContainerUpdate.AppContainerID); //Filtro de la búsqueda usando el ID
                var result = await collection.ReplaceOneAsync(filter, appContainerUpdate); // Resultado de la búsqueda
                return result.IsAcknowledged; //Confirmando la búsqueda
            } catch (Exception) {
                return false;
            }
        }

        //Comprobar si el nombre existe en la base de datos.
        public async Task<bool> CheckContainerByTitleAndUserIDAsync(string appContainerTitle, ObjectId appUserID) {
            try {

                var collection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores

                //Filtro de búsqueda por nombre del contenedor y el ID del usuario.
                var filter = Builders<AppContainer>.Filter.And(
                    Builders<AppContainer>.Filter.Eq(appContainerFilterTitle => appContainerFilterTitle.AppContainerTitle, appContainerTitle),
                    Builders<AppContainer>.Filter.Eq(appContainerFilterAppUserID => appContainerFilterAppUserID.AppUserID, appUserID)
                );
                return await collection.Find(filter).AnyAsync(); //Resultados de la búsqueda
            } catch (Exception) {
                return true;
            }
        }
        //Comprobar si un contenedor cuenta con tareas
        public async Task<bool> VerifyTaskInContainerAsync(ObjectId appContainerIDCheck) {
            try {
                var collection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores
                var filter = Builders<AppContainer>.Filter.Eq(appContaineFilter => appContaineFilter.AppContainerID, appContainerIDCheck); //Filtro de búsqueda por ID
                var container = await collection.Find(filter).FirstOrDefaultAsync(); //Resultado de la búsqueda
                return container != null && container.AppContainerAppTasksList.Count > 0; //Valor devuelto
            } catch (Exception ex) {
                return false;
            }
        }

        //Eliminar el contenedor y su contenido de forma recursiva.
        public async Task<bool> DeleteContainerRecursiveAsync(ObjectId appContainerIDDelete, ObjectId appUserID) {
            try {
                var containerCollection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores
                var containerFilter = Builders<AppContainer>.Filter.Eq(appContainerFilter => appContainerFilter.AppContainerID, appContainerIDDelete); //Filtro de búsqueda por ID
                var container = await containerCollection.Find(containerFilter).FirstOrDefaultAsync(); //Resultado de la búsqueda

                if (container != null) {
                    var taskCollection = await GetCollectionAsync<AppTask>("tareas"); // Colección de tareas
                    var taskFilter = Builders<AppTask>.Filter.Eq(appTaskFilter => appTaskFilter.AppContainerID, appContainerIDDelete); //Filtro de búsqueda por ID
                    var taskResult = await taskCollection.DeleteManyAsync(taskFilter); //Resultado de la búsqueda

                    if (taskResult.DeletedCount >= 0) {
                        var containerResult = await containerCollection.DeleteOneAsync(containerFilter); //Eliminar el contenedor
                        if (containerResult.DeletedCount > 0) {
                            var userCollection = await GetCollectionAsync<AppUser>("usuarios"); // Colección de usuarios
                            var userFilter = Builders<AppUser>.Filter.Eq(appUserFilter => appUserFilter.AppUserID, appUserID); //Filtro de búsqueda por ID
                            var userUpdate = Builders<AppUser>.Update.Pull(appUserUpdate => appUserUpdate.AppUserAppContainerList, appContainerIDDelete); //Creamos el objeto a actualizar.
                        }
                        return containerResult.DeletedCount > 0; //Resultado de la operación.
                    }
                }
                return false; //Ante cualquier incidente retornara falso.
            } catch (Exception ex) {
                return false;
            }
        }

        //Insertar un contenedor en la base de datos
        public async Task<bool> AddContainerAsync(AppContainer appContainerCreate, ObjectId appUserID) {
            var collection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores
            var userCollection = await GetCollectionAsync<AppUser>("usuarios"); // Colección de usuarios
            try {
                await collection.InsertOneAsync(appContainerCreate); //Insertar en la DB el contenedor.
                var user = await userCollection.Find(appUserFilter => appUserFilter.AppUserID == appUserID).FirstOrDefaultAsync(); //Buscar el Usuario por ID.

                if (user != null) {
                    user.AppUserAppContainerList.Add(appContainerCreate.AppContainerID); //Agregar el contenedor a la lista de contenedores del usuario
                    var update = Builders<AppUser>.Update.Set(appUserUpdate => appUserUpdate.AppUserAppContainerList, user.AppUserAppContainerList); //Creamos el objeto a actualizar
                    var result = await userCollection.UpdateOneAsync(u => u.AppUserID == appUserID, update); //Realizamos la opreación
                    return result.IsAcknowledged; // Confirmación de la operación.
                }
                return false; //Ante cualquier incidente retornara falso.
            } catch (Exception ex) {
                return false;
            }
        }



        // AppTask Methods

        //Obtener la AppTask por ID
        public async Task<AppTask?> GetTastkByIdAsync(ObjectId appTaskID) {
            try {
                var collection = await GetCollectionAsync<AppTask>("tareas"); // Colección de tareas
                var filter = Builders<AppTask>.Filter.Eq(appTaskFilter => appTaskFilter.AppTaskID, appTaskID); //Filtro de búsqueda por ID
                return await collection.Find(filter).FirstOrDefaultAsync(); //Resultado de la búsqueda
            } catch (Exception) {
                return null;
            }
        }

        //Obtener una lista de tareas, por el ID del contenedor
        public async Task<List<AppTask>> GetTasksByContainerIdAsync(ObjectId appContainerID) {
            try {
                var collection = await GetCollectionAsync<AppTask>("tareas"); // Colección de tareas
                var filter = Builders<AppTask>.Filter.Eq(appTaskFilter => appTaskFilter.AppContainerID, appContainerID); //Filtro de búsqueda por ID
                return await collection.Find(filter).ToListAsync(); //Resultado de la búsqueda
            } catch (Exception) {
                return [];
            }
        }

        //Actualizar una tarea
        public async Task<bool> UpdateTaskAsync(AppTask appTaskUpdate) {
            try {

                var tasks = await GetCollectionAsync<AppTask>("tareas"); // Colección de tareas
                var filter = Builders<AppTask>.Filter.Eq(appTaskFilter => appTaskFilter.AppTaskID, appTaskUpdate.AppTaskID); //Filtro de búsqueda por ID
                var result = await tasks.ReplaceOneAsync(filter, appTaskUpdate); //Resultado de la operación
                return result.IsAcknowledged; //Confirmación de la búsqueda
            } catch (Exception) {
                return false;
            }
        }

        //Eliminar las tareas de la base de datos y de los contenedores
        public async Task<bool> DeleteTaskAsync(ObjectId appTaskID) {
            try {
                var taskCollection = await GetCollectionAsync<AppTask>("tareas"); // Colección de tareas
                var taskFilter = Builders<AppTask>.Filter.Eq(appTaskFilter => appTaskFilter.AppTaskID, appTaskID);
                var task = await taskCollection.Find(taskFilter).FirstOrDefaultAsync();

                if (task != null) {
                    var containerCollection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores
                    var containerFilter = Builders<AppContainer>.Filter.AnyEq(appContainerFilter => appContainerFilter.AppContainerAppTasksList, appTaskID); //Filtro de búsqueda por ID
                    var containers = await containerCollection.Find(containerFilter).ToListAsync();
                    var containerUpdate = Builders<AppContainer>.Update.Pull(appContainerUpdate => appContainerUpdate.AppContainerAppTasksList, appTaskID); //Creación del objeto
                    foreach (var container in containers) {
                        await containerCollection.UpdateOneAsync(containerFilter, containerUpdate); //Resultado de la operación
                    }
                    var taskResult = await taskCollection.DeleteOneAsync(taskFilter); //Resultado de la operación
                    return taskResult.DeletedCount > 0;//Resultado de la operación
                }
                return false;
            } catch (Exception ex) {
                return false;
            }
        }

        //Agregar una tarea, a la base de datos y el contenedor respectivo
        public async Task<bool> AddTaskAsync(AppTask appTaskCreate, ObjectId appContainerID) {
            var collection = await GetCollectionAsync<AppTask>("tareas"); // Colección de tareas
            var containerCollection = await GetCollectionAsync<AppContainer>("contenedores"); // Colección de contenedores

            try {
                await collection.InsertOneAsync(appTaskCreate); //Insertar en la base de datos
                var container = await containerCollection.Find(appContaierFilter => appContaierFilter.AppContainerID == appContainerID).FirstOrDefaultAsync(); //Busacr el contenedor por ID

                if (container != null) {
                    container.AppContainerAppTasksList.Add(appTaskCreate.AppTaskID); //Agregar la tarea, a la lista de tareas del contenedor
                    var update = Builders<AppContainer>.Update.Set(appContainerUpdate => appContainerUpdate.AppContainerAppTasksList, container.AppContainerAppTasksList); //Creación del objeto
                    var result = await containerCollection.UpdateOneAsync(appContainerUpdate => appContainerUpdate.AppContainerID == appContainerID, update);  //Resultado de la operación
                    return result.IsAcknowledged; // Confirmación de la opereación 
                }
                return false;
            } catch (Exception ex) {
                return false;
            }
        }
    }
}
