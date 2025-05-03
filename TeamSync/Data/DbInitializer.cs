// Data/DbInitializer.cs
using TeamSync.Models;

namespace TeamSync.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TeamSyncContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if there are any users
            if (context.Users.Any())
            {
                return;  // Database has been seeded
            }

            // Add default users
            var adminUser = new User
            {
                FullName = "Admin User",
                Email = "admin@teamsync.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                University = "King Abdulaziz University",
                AccountType = UserType.TeamLeader,
                CreatedAt = DateTime.Now
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}