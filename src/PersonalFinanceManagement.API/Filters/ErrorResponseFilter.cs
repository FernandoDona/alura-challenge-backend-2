using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonalFinanceManagement.API.Models;

namespace PersonalFinanceManagement.API.Filters;

public class ErrorResponseFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var errorResponse = ErrorResponse.From(context.Exception);
        context.Result = new ObjectResult(errorResponse) { StatusCode = 500 };
    }
}
