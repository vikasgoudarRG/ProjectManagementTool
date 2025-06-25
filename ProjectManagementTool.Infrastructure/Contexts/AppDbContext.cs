using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;

namespace ProjectManagementTool.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<TaskItemComment> TaskItemComments => Set<TaskItemComment>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<UserNotification> UserNotifications => Set<UserNotification>();
        public DbSet<ProjectChangeLog> ProjectChangeLogs => Set<ProjectChangeLog>();
        public DbSet<TeamChangeLog> TeamChangeLogs => Set<TeamChangeLog>();
        public DbSet<TaskItemChangeLog> TaskItemChangeLogs => Set<TaskItemChangeLog>();


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectLead)
                .WithMany()
                .HasForeignKey(p => p.ProjectLeadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Teams)
                .HasForeignKey(t => t.ProjectId);


            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.TeamId, tm.UserId });

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId);


            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany()
                .HasForeignKey(tm => tm.UserId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Team)
                .WithMany()
                .HasForeignKey(t => t.TeamId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskItemComment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId);

            modelBuilder.Entity<TaskItemComment>()
                .HasOne(c => c.TaskItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskItemId);

            modelBuilder.Entity<UserNotification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<ProjectChangeLog>()
            .HasOne(pcl => pcl.Project)
            .WithMany()
            .HasForeignKey(pcl => pcl.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Tags)
                .WithMany(t => t.Tasks)
                .UsingEntity<Dictionary<string, object>>(  
                    "TaskItemTag",                         
                    j => j.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<TaskItem>()
                        .WithMany()
                        .HasForeignKey("TaskItemId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("TaskItemId", "TagId");
                    });
        }
    }
}
