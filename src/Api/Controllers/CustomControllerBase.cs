using Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [CustomExceptionFilter]
    public class CustomControllerBase : ControllerBase
    {
    }
}