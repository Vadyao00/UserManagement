using Domain.ErrorModels;
using Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

public class ApiControllerBase : Controller
{
    [HttpHead]
    public IActionResult ProccessError(ApiBaseResponse baseResponse)
    {
        return baseResponse switch
        {
            ApiBadRequestResponse response => BadRequest(new ErrorDetails
            {
                Message = response.Message,
                StatusCode = StatusCodes.Status400BadRequest
            }),
            _ => throw new NotImplementedException()
        };
    }
}