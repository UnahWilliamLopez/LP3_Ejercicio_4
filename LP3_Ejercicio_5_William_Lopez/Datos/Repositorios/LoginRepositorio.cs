using Datos.Interfaces;
using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Datos.Repositorios
{
    public class LoginRepositorio : ILoginRepositorio
    {
        private string CadenaConexion;
        public LoginRepositorio(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }
        private MySqlConnection Conexion()
        {
            return new MySqlConnection(CadenaConexion);
        }
        public async Task<bool> ValidarUsuario(Login login)
        {
            bool valido = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "SELECT 1 FROM usuarios WHERE (Usuario_ = @Usuario AND Contraseña = @Contraseña);";
                valido = await conexion.ExecuteScalarAsync<bool>(sql, new { login.Usuario, login.Contraseña });
            }
            catch (Exception Ex)
            {
            }
            return valido;
        }
    }
}
