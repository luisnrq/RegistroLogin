using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistroLogin.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {


            


            //LOS USUARIOS SON ALMACENADOS DENTRO DE HttpContext
            //Y DENTRO DE User
            //UN USUARIO ESTA COMPUESTO POR UNA IDENTIDAD Y UN PRINCIPAL
            //PODEMOS SABER EL NOMBRE DEL USUARIO O SI ESTA AUTENTICADO
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                //NECESITAMOS REALIZAR LA REDIRECCION PARA 
                //LLEVARNOS LA PETICION A LOGIN DE MANAGED
                RouteValueDictionary rutalogin = new RouteValueDictionary(new
                {
                    controller = "Manage",
                    action = "Login"
                });
                RedirectToRouteResult result = new RedirectToRouteResult(rutalogin);
                context.Result = result;
            }
        }
    }
}
