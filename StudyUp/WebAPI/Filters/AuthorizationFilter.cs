using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        /*private static readonly int UNAUTHORIZED_STATUS_CODE = 401;

        private readonly IAdministratorHandler handler;
        public AuthorizationFilter(IAdministratorHandler handler)
        {
            this.handler = handler;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["token"];
            if (token == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = UNAUTHORIZED_STATUS_CODE,
                    Content = "You must be logged as an administrator to access"
                };
                return;
            }
            if (!handler.IsLogged(token))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = UNAUTHORIZED_STATUS_CODE,
                    Content = "You must be logged as an administrator to access"
                };
                return;
            }
        }*/
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
