using DevelopmentTask.Auth.Services.Interfaces;
using DevelopmentTask.Core.Models.Entities;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Core.Services
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IJwtGeneratorService _jwtGeneratorService;

        public AuthenticationService(IUserService userService, IJwtGeneratorService jwtGeneratorService)
        {
            _userService = userService;
            _jwtGeneratorService = jwtGeneratorService;
        }

        public async Task<string> AuthenticateAsync(string code, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetAsync(code, cancellationToken);

            if (user == null)
            {
                user = new User
                {
                    Code = code,
                    LastLogin = DateTimeOffset.UtcNow,
                };

                await _userService.AddAsync(user, cancellationToken);
            }
            else
            {
                user.LastLogin = DateTimeOffset.UtcNow;
                await _userService.UpdateAsync(user, cancellationToken);
            }

            return _jwtGeneratorService.Generate(user.Code);
        }
    }
}
