namespace Domain.RequestFeatures;

public class UserParameters : RequestParameters
{
    public UserParameters() => OrderBy = "Email";
    public string? searchName { get; set; }
}