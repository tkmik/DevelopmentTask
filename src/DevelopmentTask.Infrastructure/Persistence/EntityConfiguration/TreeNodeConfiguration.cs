using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Infrastructure.Persistence.EntityConfiguration
{
    internal sealed class TreeNodeConfiguration : IEntityTypeConfiguration<TreeNode>
    {
        public void Configure(EntityTypeBuilder<TreeNode> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TreeId)
                .IsRequired();
            builder.Property(x => x.Name)                
                .IsRequired();

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.TreeId, x.ParentId, x.Name });
        }
    }
}
