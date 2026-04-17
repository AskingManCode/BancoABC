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
        private readonly IMongoCollection<Usuarios> UsuariosCollection; 

        public DataBaseMongoDB()
        {
            var Client = new MongoClient("mongodb://localhost:27017/");
            var DataBase = Client.GetDatabase("BancoABC");
            UsuariosCollection = DataBase.GetCollection<Usuarios>("Usuarios");
        }

        public bool VerificarUsuario(string user, string password)
        {
            var usuarioCuenta = this.UsuariosCollection.Find(
                u => u.User == user 
                && u.Password == password)
            .FirstOrDefault();

            return usuarioCuenta != null;
        }

        public bool GuardarDatos(Usuarios usuario)
        {
            try
            {
                this.UsuariosCollection.InsertOne(usuario);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ModificarUsuario(Usuarios usuario)
        {
            try
            {
                // A quién voy a modificar
                var filtro = Builders<Usuarios>.Filter.Eq(u => u.Identificacion, usuario.Identificacion);

                // Lo que voy a modificar
                var modificado = Builders<Usuarios>.Update
                    .Set(u => u.Nombre, usuario.Nombre)
                    .Set(u => u.PrimerApellido, usuario.PrimerApellido)
                    .Set(u => u.SegundoApellido, usuario.SegundoApellido)
                    .Set(u => u.Correo, usuario.Correo)
                    .Set(u => u.Password, usuario.Password);

                // Update
                var resultado = this.UsuariosCollection.UpdateOne(filtro, modificado);

                return resultado.ModifiedCount > 0; // cambios realizados
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ModificarEstadoUsuario(Usuarios usuario)
        {
            try
            {
                // A quién voy a modificar
                var filtro = Builders<Usuarios>.Filter.Eq(u => u.Identificacion, usuario.Identificacion);

                // Lo que voy a modificar
                var modificado = Builders<Usuarios>.Update
                    .Set(u => u.Estado, usuario.Estado);

                // Update
                var resultado = this.UsuariosCollection.UpdateOne(filtro, modificado);

                return resultado.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool CompararID(string identificacion)
        {
            // Busca si existe un usuario con Identificación igual
            var usuario = this.UsuariosCollection.Find(u => u.Identificacion == identificacion).FirstOrDefault();

            return usuario != null;
        }

        public bool CompararUsuario(string user)
        {
            // Busca si existe un nombre de usuario igual
            var usuario = this.UsuariosCollection.Find(u => u.User == user).FirstOrDefault();

            return usuario != null;
        }
        
        public bool VerificarRolUsuario(string user, string tipoUsuario)
        {
            var usuarioRol = this.UsuariosCollection.Find(
                u => u.User == user
                && u.TipoUsuario == tipoUsuario)
            .FirstOrDefault();

            return usuarioRol != null;
        }

        public bool VerificarUsuarioActivo(string user)
        {
            var usuarioEstado = this.UsuariosCollection.Find(
                u => u.User == user
                && u.Estado == true)
            .FirstOrDefault();

            return usuarioEstado != null;
        }

    }
}
