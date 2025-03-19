using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Contracts.IRepositories;
using Contracts.IServices;
using Domain.ConfigurationModels;
using Domain.Dtos;
using Domain.Entities;
using Domain.Responses;
using LoggerService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _manager;
    private readonly IOptions<JwtConfiguration> _configuration;
    private readonly JwtConfiguration _jwtConfiguration;

    private User? _user;

    public AuthenticationService(
        ILoggerManager logger,
        IMapper mapper,
        IRepositoryManager manager,
        IOptions<JwtConfiguration> configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _manager = manager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
    }
    
    public async Task<ApiBaseResponse> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userForRegistration.Password);
        
        var user = _mapper.Map<User>(userForRegistration);
        user.PasswordHash = passwordHash;
        user.RegistrationTime = DateTime.UtcNow;
        user.Status = "active";
        try
        {
            _manager.User.CreateUser(user);
            await _manager.SaveAsync();
        }
        catch (Exception e)
        {
            return new InvalidEmailBadRequestResponse();
        }

        return new ApiOkResponse<User>(user);
    }
    
    public async Task<ApiBaseResponse> ValidateUser(UserForAuthenticationDto userForAuth)
    {
        _user = await _manager.User.GetUserByEmailAsync(userForAuth.Email);

        if (_user == null || !BCrypt.Net.BCrypt.Verify(userForAuth.Password, _user.PasswordHash))
        {
            return new BadUserBadRequestResponse();
        }

        return new ApiOkResponse<User>(_user);
    }
    
    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;
        _user.LastLogin = DateTime.UtcNow;

        _manager.User.UpdateUser(_user);
        await _manager.SaveAsync();

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }
    
    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes("SECRETSECRETSECRETSECRETSECRETSECRETSECRET");
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.Name),
            new Claim(ClaimTypes.Email, _user.Email)
        };
        
        claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

        return claims;
    }
    
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
            ValidateLifetime = false,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
    
    public async Task<ApiBaseResponse> RefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _manager.User.GetUserByEmailAsync(userEmail);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return new RefreshTokenBadRequestResponse();
        }

        _user = user;

        var token = await CreateToken(populateExp: false);

        return new ApiOkResponse<TokenDto>(token);
    }
    
    public async Task<User> GetCurrentUserFromTokenAsync(string token)
    {
        var principal = GetPrincipalFromExpiredToken(token);
        var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return null;
        }

        var user = await _manager.User.GetUserByEmailAsync(userEmail);
        return user;
    }
}