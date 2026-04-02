using DevelopmentTask.Core.Models.Dto.ExceptionJournal;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Core.Services
{
    public interface IExceptionJournalService
    {
        Task<ExceptionJournal?> GetAsync(long id, CancellationToken cancellationToken = default);
        Task<ExceptionJournalInfo> GetAsync(ExceptionJournalFilter filter, CancellationToken cancellationToken = default);
    }
}
