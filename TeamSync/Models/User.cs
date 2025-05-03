// Models/User.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamSync.Models
{
    public enum UserType
    {
        Student,
        TeamLeader
    }

    public class User
    {
        public User()
        {
            UserTeams = new HashSet<UserTeam>();
            WorkSessions = new HashSet<WorkSession>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string University { get; set; } = string.Empty;

        public UserType AccountType { get; set; }

        public string? ProfilePicturePath { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<UserTeam> UserTeams { get; set; }
        public virtual ICollection<WorkSession> WorkSessions { get; set; }
    }
}