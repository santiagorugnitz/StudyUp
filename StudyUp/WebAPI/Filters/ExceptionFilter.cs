using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        private static readonly int SERVER_ERROR_STATUS_CODE = 500;
        private static readonly int BAD_REQUEST_STATUS_CODE = 400;
        //Not used yet
        //private static readonly int NOT_FOUND_STATUS_CODE = 404;


        public void OnException(ExceptionContext context)
        {
            int code = SERVER_ERROR_STATUS_CODE;
            if (context.Exception is AlreadyExistsException) code = BAD_REQUEST_STATUS_CODE;

            context.Result = new ContentResult()
            {
                StatusCode = code,
                Content = context.Exception.Message
            };
        }
    }
}
