// Data/TeamSyncContext.cs
using Microsoft.EntityFrameworkCore;
using TeamSync.Models;

namespace TeamSync.Data
{
    public class TeamSyncContext : DbContext
    {
        public TeamSyncContext(DbContextOptions<TeamSyncContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<UserTeam> UserTeams { get; set; } = null!;
        public DbSet<WorkSession> WorkSessions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTeams)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.TeamLeader)
                .WithMany()
                .HasForeignKey(t => t.TeamLeaderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.Team)
                .WithMany(t => t.UserTeams)
                .HasForeignKey(ut => ut.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkSession>()
                .HasOne(ws => ws.User)
                .WithMany(u => u.WorkSessions)
                .HasForeignKey(ws => ws.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkSession>()
                .HasOne(ws => ws.Team)
                .WithMany(t => t.WorkSessions)
                .HasForeignKey(ws => ws.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}