// Models/UserTeam.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamSync.Models
{
    public class UserTeam
    {
        [Key]
        public int UserTeamId { get; set; }

        public int UserId { get; set; }

        public int TeamId { get; set; }

        public DateTime JoinedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; } = null!;
    }
}