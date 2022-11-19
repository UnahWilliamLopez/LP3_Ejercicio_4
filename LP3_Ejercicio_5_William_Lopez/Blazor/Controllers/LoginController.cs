using Datos.Interfaces;
using Datos.Repositorios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using System.Security.Claims;

namespace Blazor.Controllers
{
    public class LoginController : Controller
    {
        private readonly Config configuracion;
        private ILoginRepositorio loginRepositorio;
        private IUsuarioRepositorio usuarioRepositorio;

        public LoginController(Config config)
        {
            configuracion = config;
            loginRepositorio = new LoginRepositorio(config.CadenaConexion);
            usuarioRepositorio = new UsuarioRepositorio(config.CadenaConexion);
        }

        [HttpPost("/account/login")]
        public async Task<IActionResult> Login(Login login)
        {
            string rol;
            try
            {
                bool usuarioValido = await loginRepositorio.ValidarUsuario(login);
                
                if (usuarioValido)
                {
                    Usuario usuario = await usuarioRepositorio.GetPorCodigo(login.Usuario);
                    if (usuario.Estado == 1)
                    {
                        rol = usuario.Rol.ToString();
                        var claims = new[] {
                            new Claim(ClaimTypes.Name, usuario.Usuario_),
                            new Claim(ClaimTypes.Role, rol)
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(5) });
                    }
                    else { return LocalRedirect("/login/El usuario no esta activo."); }
                }
                else { return LocalRedirect("/login/Datos de usuario invalidos."); }
            }
            catch (Exception Ex)
            {               
            }
            return LocalRedirect("/");
        }

        [HttpGet("/account/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/login");
        }
    }
}
