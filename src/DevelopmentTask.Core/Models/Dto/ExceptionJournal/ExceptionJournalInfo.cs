namespace DevelopmentTask.Core.Models.Dto.ExceptionJournal
{
    public sealed record ExceptionJournalInfo(int Skip, int Count, ICollection<ExceptionJournalItem> Items);

    public sealed record ExceptionJournalItem(long Id, long EventId, DateTimeOffset CreatedAt);
}
