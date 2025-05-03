using System.Collections.Generic;
using TeamSync.Models;

namespace TeamSync.ViewModels
{
    public class DashboardViewModel
    {
        public User UserProfile { get; set; }
        public List<Team> Teams { get; set; } = new List<Team>();
        public List<WorkSession> RecentSessions { get; set; } = new List<WorkSession>();
        public double TotalHoursWorked { get; set; }
        public int TasksCompleted { get; set; }
    }
}