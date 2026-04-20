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
        private readonly IMongoCollection<Personas> PersonasCollection; 

        public DataBaseMongoDB()
        {
            var Client = new MongoClient("mongodb://localhost:27017/");
            var DataBase = Client.GetDatabase("BancoABC");
            PersonasCollection = DataBase.GetCollection<Personas>("Personas");
        }

        public bool VerificarUsuario(string user, string password)
        {
            var usuarioCuenta = this.PersonasCollection.Find(
                pu => pu.Usuario.User == user 
                && pu.Usuario.Password == password)
            .FirstOrDefault();

            return usuarioCuenta != null;
        }

        public bool GuardarPersona(Personas usuario)
        {
            try
            {
                this.PersonasCollection.InsertOne(usuario);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool GuardarUsuario(Personas persona)
        {
            try
            {
                // A quién le voy a agregar la cuenta
                var filtro = Builders<Personas>.Filter.Eq(p => p.Identificacion, persona.Identificacion);

                // Agregar el Usuario a la persona
                var modificado = Builders<Personas>.Update
                    .Set(p => p.Usuario, persona.Usuario);

                // Update
                var resultado = this.PersonasCollection.UpdateOne(filtro, modificado);

                return resultado.ModifiedCount > 0; // Cantidad cambios realizados positivo
            }
            catch (Exception)
            {
                return false;
            }

        }


        /*public bool ModificarUsuario(Personas usuario)
        {
            try
            {
                // A quién voy a modificar
                var filtro = Builders<Personas>.Filter.Eq(u => u.Identificacion, usuario.Identificacion);

                // Lo que voy a modificar
                var modificado = Builders<Personas>.Update
                    .Set(u => u.Nombre, usuario.Nombre)
                    .Set(u => u.PrimerApellido, usuario.PrimerApellido)
                    .Set(u => u.SegundoApellido, usuario.SegundoApellido)
                    .Set(u => u.Correo, usuario.Correo)
                    .Set(u => u.Password, usuario.Password);

                // Update
                var resultado = this.UsuariosCollection.UpdateOne(filtro, modificado);

                return resultado.PersonasCollection > 0; // cambios realizados
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ModificarEstadoUsuario(Personas usuario)
        {
            try
            {
                // A quién voy a modificar
                var filtro = Builders<Personas>.Filter.Eq(u => u.Identificacion, usuario.Identificacion);

                // Lo que voy a modificar
                var modificado = Builders<Personas>.Update
                    .Set(u => u.Estado, usuario.Estado);

                // Update
                var resultado = this.UsuariosCollection.UpdateOne(filtro, modificado);

                return resultado.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }

        } */

        public bool ExisteIdentificacion(string identificacion)
        {
            // Busca si existe un usuario con Identificación igual
            var persona = this.PersonasCollection.Find(p => p.Identificacion == identificacion).FirstOrDefault();

            return persona != null;
        }

        public bool ExisteUsuario(string user)
        {
            // Busca si existe un nombre de usuario igual
            var persona = this.PersonasCollection.Find(pu => pu.Usuario.User == user).FirstOrDefault();

            return persona != null;
        }

        public bool PersonaTieneUsuario(string identificacion)
        {
            // Busca a la persona con una identificación igual
            var persona =  this.PersonasCollection.Find(p => p.Identificacion == identificacion).FirstOrDefault();

            // Luego verifica si tiene un usuario
            return persona.Usuario != null;
        }

        public bool UsuarioActivo(string user)
        {
            // Busca al usuario y verifica si está activo
            var usuarioEstado = this.PersonasCollection.Find(
                pu => pu.Usuario.User == user
                && pu.Usuario.Estado == true)
            .FirstOrDefault();

            return usuarioEstado != null;
        }

        public string ObtenerIdentificacion(string user, string password)
        {
            var persona = this.PersonasCollection.Find(
                pu => pu.Usuario.User == user 
                && pu.Usuario.Password == password)
            .FirstOrDefault();

            return persona != null ? persona.Identificacion : null;
        }

        public string ObtenerTipoUsuario(string user, string password)
        {
            var persona = this.PersonasCollection.Find(
                pu => pu.Usuario.User == user
                && pu.Usuario.Password == password)
            .FirstOrDefault();

            return persona != null ? persona.Usuario.TipoUsuario : null;
        }

        /*public Personas ObtenerDatosPersona(string user, string password)
        {
            var persona = this.PersonasCollection.Find(
                pu => pu.Usuario.User == user
                && pu.Usuario.Password == password)
            .FirstOrDefault();


            // Termianar función
            return persona;
        }*/

        /*
        public bool VerificarRolUsuario(string user, string tipoUsuario)
        {
            var usuarioRol = this.UsuariosCollection.Find(
                u => u.User == user
                && u.TipoUsuario == tipoUsuario)
            .FirstOrDefault();

            return usuarioRol != null;
        } */



    }
}
