using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;

namespace ProjectManagementTool.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        // DbSets
        public DbSet<User> Users => Set<User>();
        public DbSet<UserNotification> UserNotifications => Set<UserNotification>();

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectChangeLog> ProjectChangeLogs => Set<ProjectChangeLog>();

        public DbSet<Team> Teams => Set<Team>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<TeamChangeLog> TeamChangeLogs => Set<TeamChangeLog>();

        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<TaskItemChangeLog> TaskItemChangeLogs => Set<TaskItemChangeLog>();
        public DbSet<TaskItemComment> TaskItemComments => Set<TaskItemComment>();

        public DbSet<Tag> Tags => Set<Tag>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key for TeamMember
            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.TeamId, tm.UserId });

            // Relationships
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectLead)
                .WithMany()
                .HasForeignKey(p => p.ProjectLeadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany()
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany()
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Team)
                .WithMany()
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Tags)
                .WithMany()
                .UsingEntity(j => j.ToTable("TaskItemTags"));

            modelBuilder.Entity<TaskItemComment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItemComment>()
                .HasOne(c => c.TaskItem)
                .WithMany()
                .HasForeignKey(c => c.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItemChangeLog>()
                .HasOne(c => c.TaskItem)
                .WithMany()
                .HasForeignKey(c => c.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItemChangeLog>()
                .HasOne(c => c.ChangedByUser)
                .WithMany()
                .HasForeignKey(c => c.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectChangeLog>()
                .HasOne(c => c.Project)
                .WithMany()
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectChangeLog>()
                .HasOne(c => c.ChangedByUser)
                .WithMany()
                .HasForeignKey(c => c.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamChangeLog>()
                .HasOne(c => c.Team)
                .WithMany()
                .HasForeignKey(c => c.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamChangeLog>()
                .HasOne(c => c.ChangedByUser)
                .WithMany()
                .HasForeignKey(c => c.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserNotification>()
                .HasKey(n => n.Id);
        }
    }
}