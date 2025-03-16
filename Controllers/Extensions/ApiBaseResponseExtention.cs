using Domain.Responses;

namespace Controllers.Extensions;

public static class ApiBaseResponseExtention
{
    public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse)
        => ((ApiOkResponse<TResultType>)apiBaseResponse).Result;
}