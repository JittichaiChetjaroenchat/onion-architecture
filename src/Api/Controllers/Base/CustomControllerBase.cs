using Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Base
{
    [CustomExceptionFilter]
    public class CustomControllerBase : ControllerBase
    {
    }
}