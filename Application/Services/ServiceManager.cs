using AutoMapper;
using Contracts.IRepositories;
using Contracts.IServices;
using Domain.ConfigurationModels;
using LoggerService;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IUserService> _userService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IRepositoryManager manager, IOptions<JwtConfiguration> configuration)
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, manager, configuration));
        _userService = new Lazy<IUserService>(() => new UserService(manager));
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IUserService UserService => _userService.Value;
}