using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DevelopmentTask.Auth.Common.Options;
using DevelopmentTask.Auth.Services.Interfaces;

namespace DevelopmentTask.Auth.Services
{
    internal sealed class JwtGeneratorService : IJwtGeneratorService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtGeneratorService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }


        public string Generate(string code)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, code),
                new(JwtRegisteredClaimNames.Iat, TimeProvider.System.GetUtcNow().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes),
                signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
