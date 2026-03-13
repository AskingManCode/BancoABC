using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Entities;
using MongoDB.Driver;
using MongoDB.Bson;

namespace WS.DataAccess
{
    public class DataBaseMongoDB
    {

        public DataBaseMongoDB()
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("BancoABC");
            var collection = database.GetCollection<BsonDocument>("Usuarios");

            // Buscar como conectarse a MongoDB desde C#
        }

        public bool VerificarUsuario(string user, string password)
        {
            if (this.CompararUsuario(user))
            {
                return false;
            }

            return true;
        }

        public bool GuardarDatos(Usuarios usuarios)
        {
            //this.collection

            return true;
        }

        public bool ModificarUsuario(Usuarios usuarios)
        {

            return true;
        }

        public bool ModificarEstadoUsuario(string identificacion, bool estado)
        {

            return true;
        }

        public bool CompararID(string identificacion)
        {
            // Buscar si existe un usuario con Ientificación igual

            return true;
        }

        public bool CompararUsuario(string user)
        {
            // Buscar si existe un nombre de usuario igual

            return true;
        }
        
        public string ObtenerRolUsuario(string identificacion)
        {

            return "";
        }

    }
}
