using Blazor.Interfaces;
using Datos.Interfaces;
using Datos.Repositorios;
using Modelos;

namespace Blazor.Servicios
{
    public class LoginServicio : ILoginServicio
    {
        private readonly Config Configuracion;
        private ILoginRepositorio loginRepositorio;
        public LoginServicio(Config config)
        {
            Configuracion = config;
            loginRepositorio = new LoginRepositorio(config.CadenaConexion);
        }

        public async Task<bool> ValidarUsuario(Login login)
        {
            return await loginRepositorio.ValidarUsuario(login);
        }
    }
}
