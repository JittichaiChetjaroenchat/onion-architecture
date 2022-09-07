using System;
using System.Linq;
using Domain.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Helpers
{
    public class ResponseHelper
    {
        public static Response<T> Ok<T>(T data) where T : class
        {
            return new Response<T>()
            {
                Status = EnumResponseStatus.Ok,
                Data = data
            };
        }

        public static Response<T> Error<T>(string[] errors) where T : class
        {
            return new Response<T>()
            {
                Status = EnumResponseStatus.Error,
                Errors = errors
            };
        }

        public static string[] Build(string error)
        {
            return new string[] { error };
        }

        public static string[] Build(ValidationException ve)
        {
            return ve.Errors
                .Select(x => x.ErrorMessage)
                .ToArray();
        }

        public static string[] Build(DbUpdateException dbe)
        {
            return Build(dbe.Message);
        }

        public static string[] Build(Exception ex)
        {
            return Build(ex.Message);
        }
    }
}