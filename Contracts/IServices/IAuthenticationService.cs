﻿using Domain.Dtos;
using Domain.Entities;
using Domain.Responses;

namespace Contracts.IServices;

public interface IAuthenticationService
{
    Task<ApiBaseResponse> RegisterUser(UserForRegistrationDto userForRegistrationDto);
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<ApiBaseResponse> RefreshToken(TokenDto tokenDto);
    Task<User> GetCurrentUserFromTokenAsync(string token);
}