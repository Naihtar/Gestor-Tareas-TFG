using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG.Database {
    public interface IConnectionManager {

         string GetConnectionString();

        // Método - Encriptar la clave de acceso.
         void ProtectConnectionString();

        // Método - Desencriptar la contraseña
         void UnprotectConnectionString();
    }
}
