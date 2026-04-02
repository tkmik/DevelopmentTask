namespace DevelopmentTask.Core.Models.Entities
{
    public interface IAuditableEntity
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset ModifiedAt { get; set; }
    }

    public class AuditableEntity<TId> : Entity<TId>, IAuditableEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
