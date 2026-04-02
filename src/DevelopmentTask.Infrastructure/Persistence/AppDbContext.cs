using Microsoft.EntityFrameworkCore;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Infrastructure.Persistence
{
    public sealed class AppDbContext(
        DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public const string ConnectionStringName = "DefaultConnection";

        public DbSet<ExceptionJournal> ExceptionJournal => Set<ExceptionJournal>();
        public DbSet<Tree> Trees => Set<Tree>();
        public DbSet<TreeNode> TreeNodes => Set<TreeNode>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
