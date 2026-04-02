namespace DevelopmentTask.Core.Models.Dto.ExceptionJournal
{
    public sealed class ExceptionJournalFilter
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public long? Id { get; set; }
        public long? EventId { get; set; }
        public Guid? CorrelationId { get; set; }
        public string? RequestPath { get; set; } = null!;
        public string? HttpMethod { get; set; } = null!;
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? QueryParams { get; set; }
        public string? BodyParams { get; set; }
    }
}
