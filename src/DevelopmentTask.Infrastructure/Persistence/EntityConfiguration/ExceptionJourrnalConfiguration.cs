using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Infrastructure.Persistence.EntityConfiguration
{
    internal sealed class ExceptionJourrnalConfiguration : IEntityTypeConfiguration<ExceptionJournal>
    {
        public void Configure(EntityTypeBuilder<ExceptionJournal> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventId)
                .IsRequired();
            builder.Property(x => x.CorrelationId)
                .IsRequired();
            builder.Property(x => x.Timestamp)
                .IsRequired();
            builder.Property(x => x.RequestPath)
                .IsRequired();
            builder.Property(x => x.HttpMethod)
                .IsRequired();
            builder.Property(x => x.ExceptionMessage)
                .HasColumnType(PgTypes.text)
                .IsRequired(false);
            builder.Property(x => x.StackTrace)
                .HasColumnType(PgTypes.text)
                .IsRequired(false);
            builder.Property(x => x.QueryParams)
                .HasColumnType(PgTypes.jsonb)
                .IsRequired(false);
            builder.Property(x => x.BodyParams)
               .HasColumnType(PgTypes.jsonb)
               .IsRequired(false);
        }
    }
}
