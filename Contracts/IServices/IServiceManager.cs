﻿namespace Contracts.IServices;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
    IUserService UserService { get; }
}