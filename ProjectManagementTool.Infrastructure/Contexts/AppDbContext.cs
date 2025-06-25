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
            // Project - ProjectLead (User)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectLead)
                .WithMany(u => u.LeadProjects)
                .HasForeignKey(p => p.ProjectLeadId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project - Teams (1-to-many)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Teams)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project - Developers (many-to-many)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Developers)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectDeveloper",
                    j => j.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade), 
                    j => j.HasOne<Project>()
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("ProjectId", "UserId");
                        j.ToTable("ProjectDevelopers"); 
                    }
                );

            // Team - TeamMembers (1-to-many)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.TeamMembers)
                .WithOne(tm => tm.Team)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // TeamMember - User (many-to-1)
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany(u => u.TeamMemberships)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // TeamMember composite key
            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.TeamId, tm.UserId });

            // TaskItem - Team (many-to-1)
            modelBuilder.Entity<TaskItem>()
                .HasOne(ti => ti.Team)
                .WithMany(t => t.TaskItems)
                .HasForeignKey(ti => ti.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskItem - AssignedUser (many-to-1)
            modelBuilder.Entity<TaskItem>()
                .HasOne(ti => ti.AssignedUser)
                .WithMany()
                .HasForeignKey(ti => ti.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // TaskItem - Comments (1-to-many)
            modelBuilder.Entity<TaskItem>()
                .HasMany(ti => ti.Comments)
                .WithOne(tc => tc.TaskItem)
                .HasForeignKey(tc => tc.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskItem - Tags (many-to-many)
            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Tags)
                .WithMany(tg => tg.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskItemTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<TaskItem>().WithMany().HasForeignKey("TaskItemId").OnDelete(DeleteBehavior.Cascade),
                    j => { j.HasKey("TaskItemId", "TagId"); }
                );

            // TaskItemComment - Author
            modelBuilder.Entity<TaskItemComment>()
                .HasOne(tc => tc.Author)
                .WithMany()
                .HasForeignKey(tc => tc.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserNotification - User
            modelBuilder.Entity<UserNotification>()
                .HasOne(un => un.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProjectChangeLog - Project
            modelBuilder.Entity<ProjectChangeLog>()
                .HasOne(pcl => pcl.Project)
                .WithMany(p => p.ChangeLogs)
                .HasForeignKey(pcl => pcl.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // TeamChangeLog - Team
            modelBuilder.Entity<TeamChangeLog>()
                .HasOne(tcl => tcl.Team)
                .WithMany(t => t.ChangeLogs)
                .HasForeignKey(tcl => tcl.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskItemChangeLog - TaskItem
            modelBuilder.Entity<TaskItemChangeLog>()
                .HasOne(ticl => ticl.TaskItem)
                .WithMany(ti => ti.ChangeLogs)
                .HasForeignKey(ticl => ticl.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
