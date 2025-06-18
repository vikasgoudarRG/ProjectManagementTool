using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskItemChangeLog> TaskItemChangeLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One User <-> Many TaskItems
            modelBuilder.Entity<User>()
                .HasMany(u => u.AssignedTasks)
                .WithOne(t => t.AssignedUser)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // One Project <-> Many TaskItems
            modelBuilder.Entity<Project>()
                .HasMany(p => p.TaskItems)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);

            // Many Developers <-> Many Projects
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Developers)
                .WithMany(u => u.Projects);

            // One Manager <-> Many Projects
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany(u => u.ManagedProjects)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many TaskItems <-> Many Tags
            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Tags)
                .WithMany(tg => tg.TaskItems);

            // One TaskItems <-> Many Comments
            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.TaskItem)
                .HasForeignKey(c => c.TaskItemId);

            //  One TaskItem <-> Many TaskItemChangeLogs
            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.ChangeLogs)
                .WithOne(cl => cl.TaskItem)
                .HasForeignKey(cl => cl.TaskItemId);

            // One User <-> Many Notifications
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);
        }       
    }
}