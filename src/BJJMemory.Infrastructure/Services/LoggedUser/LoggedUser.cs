using BJJMemory.Domain.Entities;
using BJJMemory.Domain.Security.Tokens;
using BJJMemory.Domain.Services.LoggedUser;
using BJJMemory.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BJJMemory.Infrastructure.Services.LoggedUser;

internal class LoggedUser : ILoggedUser
{
    private readonly BJJMemoryDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;
    public LoggedUser(BJJMemoryDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }
    public async Task<Usuario> Get()
    {
        string token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await _dbContext
            .Usuarios
            .AsNoTracking()
            .FirstAsync(user => user.Id == Guid.Parse(identifier));
    }
}
