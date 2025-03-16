namespace Domain.RequestFeatures;

public class UserParameters : RequestParameters
{
    public UserParameters() => OrderBy = "Name";
    public string? searchName { get; set; }
}