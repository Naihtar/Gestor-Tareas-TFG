using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG.Database {
    public interface IConnectionManager {

        // Obtener la clave de acceso.
         string GetConnectionString();

        // Encriptar la clave de acceso.
         void ProtectConnectionString();

        // Desencriptar la contraseña
         void UnprotectConnectionString();
    }
}
