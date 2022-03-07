using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RegistroLogin.Models;
using RegistroLogin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RegistroLogin.Controllers
{
    public class ManageController : Controller
    {
        private RepositoryWeb repo;
        
        public ManageController(RepositoryWeb repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            Usuario usuario = this.repo.LogInUsuario(email, password);
            if (usuario == null)
            {
                ViewData["MENSAJE"] = "No tienes credenciales correctas";
                return View();
            }
            else
            {
                //DEBEMOS CREAR UNA IDENTIDAD (name y role)
                //Y UN PRINCIPAL
                //DICHA IDENTIDAD DEBEMOS COMBINARLA CON LA COOKIE DE 
                //AUTENTIFICACION
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
                //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
                //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
                Claim claimUserName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimRole = new Claim(ClaimTypes.Role, usuario.Tipo);
                Claim claimIdUsuario = new Claim("IdUsuario", usuario.IdUsuario.ToString());
                Claim claimEmail = new Claim("EmailUsuario", usuario.Email);

                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimIdUsuario);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(45)
                });

                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult ErrorAcceso()
        {
            ViewData["MENSAJE"] = "Error de acceso";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
