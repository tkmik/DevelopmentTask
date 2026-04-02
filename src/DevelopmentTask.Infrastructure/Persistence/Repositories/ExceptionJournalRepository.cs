using DevelopmentTask.Core.Database.Repositories;
using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Infrastructure.Persistence.Repositories
{
    internal sealed class ExceptionJournalRepository(AppDbContext ctx)
        : EntityFrameworkRepository<AppDbContext, ExceptionJournal, long>(ctx), IExceptionJournalRepository
    {
    }
}
