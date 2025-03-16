using AutoMapper;
using Contracts.IRepositories;
using Contracts.IServices;
using Domain.ConfigurationModels;
using Domain.Entities;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IRepositoryManager manager, IOptions<JwtConfiguration> configuration)
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, manager, configuration));
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}