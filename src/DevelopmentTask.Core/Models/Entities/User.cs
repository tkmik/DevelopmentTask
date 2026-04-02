namespace DevelopmentTask.Core.Models.Entities
{
    public sealed class User : Entity<Guid>
    {
        public required string Code { get; set; }
        public DateTimeOffset LastLogin { get; set; }
    }
}
