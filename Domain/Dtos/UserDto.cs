using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public record UserDto
{
    public string? Id { get; set; }
    
    [Display(Name = "Логин")]
    public string? Name { get; set; }
    
    [Display(Name = "Время регистрации")]
    [DataType(DataType.DateTime)]
    public DateTimeOffset RegistrationTime { get; set; }
    
    [Display(Name = "Послдений раз онлайн")]
    [DataType(DataType.DateTime)]
    public DateTimeOffset? LastLogin { get; set; }
    
    [EmailAddress(ErrorMessage = "Некорректный адрес")]
    public string? Email { get; set; }
    
    [Display(Name = "Статус")]
    public string? Status { get; set; }
}