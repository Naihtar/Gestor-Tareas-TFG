using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG.Database {
    class ConnectionManager : IConnectionManager {
        // Método - Obtener el string para conectarse
        public string GetConnectionString() {
            UnprotectConnectionString();
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnectionString"].ConnectionString;
            ProtectConnectionString();
            return connectionString;
        }

        // Método - Encriptar la clave de acceso.
        public void ProtectConnectionString() {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = config.GetSection("connectionStrings") as ConnectionStringsSection;
            if (!section.SectionInformation.IsProtected) {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Full);
            }
        }

        // Método - Desencriptar la contraseña
        public void UnprotectConnectionString() {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = config.GetSection("connectionStrings") as ConnectionStringsSection;
            if (section.SectionInformation.IsProtected) {
                section.SectionInformation.UnprotectSection();
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Full);
            }
        }
    }
}

