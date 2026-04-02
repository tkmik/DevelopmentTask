namespace DevelopmentTask.Core.Models.Entities
{
    public sealed class ExceptionJournal : AuditableEntity<long>
    {
        public long EventId { get; set; }
        public Guid CorrelationId { get; set; }
        public long Timestamp { get; set; }
        public string RequestPath { get; set; } = null!;
        public string HttpMethod { get; set; } = null!;
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? QueryParams { get; set; }
        public string? BodyParams { get; set; }
    }
}
