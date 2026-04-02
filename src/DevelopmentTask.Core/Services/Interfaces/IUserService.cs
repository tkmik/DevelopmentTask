using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetAsync(string code, CancellationToken cancellationToken = default);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
    }
}
