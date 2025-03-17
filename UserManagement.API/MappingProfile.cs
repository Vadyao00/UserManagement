using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace UserManagement.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegistrationDto, User>();
        CreateMap<UserForAuthenticationDto, User>();
    }
}