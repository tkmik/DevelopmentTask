using DevelopmentTask.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevelopmentTask.Infrastructure.Persistence.EntityConfiguration
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired();

            builder.Property(x => x.LastLogin)
                .IsRequired();

            builder.HasIndex(x => x.Code);
        }
    }
}
