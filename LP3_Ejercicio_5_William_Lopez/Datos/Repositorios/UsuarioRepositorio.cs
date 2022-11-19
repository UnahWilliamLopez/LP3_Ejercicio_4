using Dapper;
using Datos.Interfaces;
using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private string CadenaConexion;
        public UsuarioRepositorio(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }        

        private MySqlConnection Conexion()
        {
            return new MySqlConnection(CadenaConexion);
        }
        public async Task<Usuario> GetPorCodigo(string Usuario)
        {
            Usuario user = new Usuario();
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "SELECT * FROM usuarios WHERE Usuario_ = @Usuario;";
                user = await conexion.QueryFirstAsync<Usuario>(sql, new { Usuario });
            }
            catch (Exception Ex)
            {
            }
            return user;
        }
    }
}
