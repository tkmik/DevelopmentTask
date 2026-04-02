using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Core.Database.Repositories.Interfaces
{
    public interface IUserRepository : IEntityFrameworkRepository<User, Guid>
    {
    }
}
