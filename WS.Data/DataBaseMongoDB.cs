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
        public DataBaseMongoDB() { 
            
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("UsuariosABC");
            var collection = database.GetCollection<BsonDocument>("Usuarios");

            // Buscar como conectarse a MongoDB desde C#
        }

        public static bool GuardarDatos(Usuarios user)
        {
            //this.collection

            return true;
        }

    }
}
