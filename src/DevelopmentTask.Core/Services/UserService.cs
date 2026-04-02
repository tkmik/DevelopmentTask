using System.Linq.Expressions;
using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Entities;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Core.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            return _userRepository.AddAsync(user, cancellationToken);
        }

        public async Task<User?> GetAsync(string code, CancellationToken cancellationToken = default)
        {
            Expression<Func<User, bool>> filter = x => x.Code == code;

            return await _userRepository.FirstOrDefaultAsync(filter, cancellationToken: cancellationToken);
        }

        public Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            return _userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}
