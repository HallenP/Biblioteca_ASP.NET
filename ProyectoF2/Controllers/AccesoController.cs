using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoF2.Models;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using ProyectoF2.Logica;

namespace ProyectoF2.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /*==========================================*/
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {

            Usuario us = new LO_Usuario().EncontrarUsuarios(correo, clave);
            if (correo == us.Correo &&
                clave == us.Clave && us.IdRol == 1)
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,us.Correo),
                    new Claim(ClaimTypes.Role,"Administrador")
                };
                //nombre claims el mismo en los dos
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                   CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    //IsPersistent = modelLogin.keepLoggedIn
                };
                //nombre claimsIdentity el mismo en los dos
                //nombre properties el mismo en los dos
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Index", "Home");
            }

            //----USUARIO--//
            if (correo == us.Correo &&
                clave == us.Clave && us.IdRol == 2)
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,us.Correo),
                    new Claim(ClaimTypes.Role,"Lector")
                };
                //nombre claims el mismo en los dos
                //ClaimIdentity permite una autenticación basada en notificaciones
                //La identidad del usuario se representa como un conjunto de notificaciones
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                   CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    //IsPersistent = modelLogin.keepLoggedIn
                };
                //nombre claimsIdentity el mismo en los dos
                //nombre properties el mismo en los dos
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Index", "Home");
            }
            else
                if (correo == null && clave == null)
            {
                ViewData["ValidateMessage"] = "Usuario no encontrado";

            }

            SessionAuthentication.Default.Equals(us);
            return RedirectToAction("Index", "Acceso");
        }
    }
}
