// Models/Team.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamSync.Models
{
    public class Team
    {
        public Team()
        {
            UserTeams = new HashSet<UserTeam>();
            WorkSessions = new HashSet<WorkSession>();
        }

        [Key]
        public int TeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string TeamName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? TeamLeaderId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Add the missing TeamLogoPath property
        public string? TeamLogoPath { get; set; }

        // Navigation properties
        [ForeignKey("TeamLeaderId")]
        public virtual User? TeamLeader { get; set; }

        public virtual ICollection<UserTeam> UserTeams { get; set; }

        public virtual ICollection<WorkSession> WorkSessions { get; set; }

        [NotMapped]
        public List<User> TeamMembers { get; set; } = new List<User>();
    }
}