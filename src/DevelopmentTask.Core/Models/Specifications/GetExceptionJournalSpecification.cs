using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using DevelopmentTask.Core.Models.Dto.ExceptionJournal;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Core.Models.Specifications
{
    internal class GetExceptionJournalSpecification : Specification<ExceptionJournal>
    {
        private readonly ExceptionJournalFilter _filter;

        public GetExceptionJournalSpecification(ExceptionJournalFilter filter)
        {
            _filter = filter;
        }

        public override Expression<Func<ExceptionJournal, bool>> ToExpression()
        {
            bool qpIsJson = IsValidJson(_filter.QueryParams);
            bool bpIsJson = IsValidJson(_filter.BodyParams);

            return x => (_filter.Id == null || x.Id == _filter.Id)
                    && (_filter.EventId == null || x.EventId == _filter.EventId)
                    && (_filter.CorrelationId == null || x.CorrelationId == _filter.CorrelationId)
                    && (_filter.RequestPath == null || x.RequestPath.Contains(_filter.RequestPath!))
                    && (_filter.HttpMethod == null || x.HttpMethod.Contains(_filter.HttpMethod!))
                    && (_filter.StackTrace == null || (x.StackTrace != null && x.StackTrace.Contains(_filter.StackTrace!)))
                    && (!qpIsJson || EF.Functions.JsonContains(x.QueryParams!, _filter.QueryParams!))
                    && (!bpIsJson || EF.Functions.JsonContains(x.BodyParams!, _filter.BodyParams!));
        }

        private static bool IsValidJson(string? input)
        {
            try
            {
                JsonDocument.Parse(input ?? string.Empty);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
