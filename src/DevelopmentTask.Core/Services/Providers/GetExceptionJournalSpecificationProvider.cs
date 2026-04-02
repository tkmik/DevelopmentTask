using System.Linq.Expressions;
using DevelopmentTask.Core.Models.Dto.ExceptionJournal;
using DevelopmentTask.Core.Models.Entities;
using DevelopmentTask.Core.Models.Specifications;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Core.Services.Providers
{
    internal class GetExceptionJournalSpecificationProvider : ISpecificationProvider<ExceptionJournalFilter, ExceptionJournal>
    {
        public Expression<Func<ExceptionJournal, bool>> GetFilter(ExceptionJournalFilter model)
        {
            return new GetExceptionJournalSpecification(model).ToExpression();
        }
    }
}
