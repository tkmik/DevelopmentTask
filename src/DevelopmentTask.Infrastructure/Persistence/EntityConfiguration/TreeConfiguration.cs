using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Infrastructure.Persistence.EntityConfiguration
{
    internal sealed class TreeConfiguration : IEntityTypeConfiguration<Tree>
    {
        public void Configure(EntityTypeBuilder<Tree> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.HasMany(x => x.Nodes)
                .WithOne()
                .HasForeignKey(x => x.TreeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
