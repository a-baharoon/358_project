// ViewModels/TeamDetailsViewModel.cs
using System.Collections.Generic;
using TeamSync.Models;

namespace TeamSync.ViewModels
{
    public class TeamDetailsViewModel
    {
        public Team Team { get; set; } = null!;
        public List<WorkSession> WorkSessions { get; set; } = new List<WorkSession>();
        public bool IsUserTeamLeader { get; set; }
        public bool IsUserMember { get; set; }
    }
}