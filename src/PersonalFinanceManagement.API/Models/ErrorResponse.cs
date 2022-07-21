using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PersonalFinanceManagement.API.Models;

public class ErrorResponse
{
    public Error Error { get; private set; }
    public ErrorResponse(string? code, string? message)
    {
        Error = new Error(code, message);
    }

    public static ErrorResponse From(Exception exception)
    {
        return new ErrorResponse(exception.GetType().ToString(), exception.Message);
    }

    public static ErrorResponse FromModelState(ModelStateDictionary modelStateDictionary)
    {
        var errors = modelStateDictionary.Values.SelectMany(m => m.Errors);
        return new ErrorResponse("Invalid model state.", errors?.Select(e => e?.ErrorMessage)?.FirstOrDefault());
    }
}
