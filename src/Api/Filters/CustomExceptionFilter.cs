using System;
using System.Net;
using Application.Exceptions;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ErrorsException ee)
            {
                var result = new OkObjectResult(ResponseHelper.Error<object>(ee.Errors));

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = result;

                return;
            }

            if (context.Exception is FluentValidation.ValidationException ve)
            {
                var errors = ResponseHelper.Build(ve);
                var result = new OkObjectResult(ResponseHelper.Error<object>(errors));

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = result;

                return;
            }

            if (context.Exception is DbUpdateException dbe)
            {
                var errors = ResponseHelper.Build(dbe);
                var result = new OkObjectResult(ResponseHelper.Error<object>(errors));

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = result;

                return;
            }

            if (context.Exception is Exception ex)
            {
                var errors = ResponseHelper.Build(ex);
                var result = new OkObjectResult(ResponseHelper.Error<object>(errors));

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = result;

                return;
            }

            base.OnException(context);
        }
    }
}