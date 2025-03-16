namespace Domain.Responses;

public abstract class ApiBaseResponse
{
    public bool Suссess {  get; set; }
    protected ApiBaseResponse(bool suссess) => Suссess = suссess;
}