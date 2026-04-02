using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Dto.ExceptionJournal;
using DevelopmentTask.Core.Models.Entities;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Core.Services
{
    internal sealed class ExceptionJournalService : IExceptionJournalService
    {
        private readonly IExceptionJournalRepository _exceptionJournalRepository;
        private readonly ISpecificationResolver _specificationResolver;

        public ExceptionJournalService(IExceptionJournalRepository exceptionJournalRepository, ISpecificationResolver specificationResolver)
        {
            _exceptionJournalRepository = exceptionJournalRepository;
            _specificationResolver = specificationResolver;
        }

        public async Task<ExceptionJournal?> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _exceptionJournalRepository.GetAsync(id, cancellationToken);
        }

        public async Task<ExceptionJournalInfo> GetAsync(ExceptionJournalFilter filter, CancellationToken cancellationToken = default)
        {
            var queryFilter = _specificationResolver.GetFilter<ExceptionJournal>(filter);
            var result = await _exceptionJournalRepository.GetAsync(
                filter: queryFilter,
                skipCount: filter.Skip,
                takeCount: filter.Take,
                cancellationToken: cancellationToken);

            return new ExceptionJournalInfo(
                filter.Skip,
                result?.Count ?? 0,
                (result ?? []).ConvertAll(x => new ExceptionJournalItem(x.Id, x.EventId, x.CreatedAt)));
            
        }
    }
}
