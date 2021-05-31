using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Exceptions;
using WebAPI.Filters;

namespace WebAPITest
{
    [TestClass]
    public class ExceptionFilterTest
    {
        ModelStateDictionary modelState;
        DefaultHttpContext httpContext;
        ExceptionContext context;
        ExceptionFilter filter;

        [TestInitialize]
        public void Setup()
        {
            filter = new ExceptionFilter();
            modelState = new ModelStateDictionary();
            httpContext = new DefaultHttpContext();

            context = new ExceptionContext(
                new ActionContext(httpContext: httpContext,
                                  routeData: new Microsoft.AspNetCore.Routing.RouteData(),
                                  actionDescriptor: new ActionDescriptor(),
                                  modelState: modelState),
                new List<IFilterMetadata>());
        }

        [TestMethod]
        public void Test500()
        {
            context.Exception = new Exception();
            filter.OnException(context);

            ContentResult response = context.Result as ContentResult;

            Assert.AreEqual(500, response.StatusCode);
        }
        
        [TestMethod]
        public void Test400()
        {
            context.Exception = new AlreadyExistsException("This is an already exists exception");
            filter.OnException(context);

            ContentResult response = context.Result as ContentResult;

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public void Test404()
        {
            context.Exception = new NotFoundException("This is a not found exception");
            filter.OnException(context);

            ContentResult response = context.Result as ContentResult;

            Assert.AreEqual(404, response.StatusCode);
        }

        [TestMethod]
        public void Test401()
        {
            context.Exception = new NotAuthenticatedException("This is a not authenticated exception");
            filter.OnException(context);

            ContentResult response = context.Result as ContentResult;

            Assert.AreEqual(401, response.StatusCode);
        }
    }
}
