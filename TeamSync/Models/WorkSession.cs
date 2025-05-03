// Models/WorkSession.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamSync.Models
{
    public class WorkSession
    {
        [Key]
        public int WorkSessionId { get; set; }

        public int UserId { get; set; }

        public int? TeamId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int? DurationMinutes { get; set; }

        public int TasksCompleted { get; set; }

        public string? ReflectionNotes { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("TeamId")]
        public virtual Team? Team { get; set; }
    }
}