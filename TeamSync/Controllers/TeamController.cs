using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamSync.Data;
using TeamSync.Models;
using TeamSync.ViewModels;

namespace TeamSync.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly TeamSyncContext _context;
        private readonly ILogger<TeamController> _logger;

        public TeamController(TeamSyncContext context, ILogger<TeamController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Team
        public async Task<IActionResult> Index()
        {
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Get user's teams
            var userTeams = await _context.UserTeams
                .Where(ut => ut.UserId == currentUserId)
                .Include(ut => ut.Team)
                .Select(ut => ut.Team)
                .ToListAsync();

            return View(userTeams);
        }

        // GET: Team/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var team = await _context.Teams
                .Include(t => t.TeamLeader)
                .Include(t => t.UserTeams)
                    .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            // Map UserTeams to TeamMembers for easier access in the view
            team.TeamMembers = team.UserTeams.Select(ut => ut.User).ToList();

            // Get work sessions for the team
            var workSessions = await _context.WorkSessions
                .Where(ws => ws.TeamId == id)
                .Include(ws => ws.User)
                .OrderByDescending(ws => ws.StartTime)
                .Take(10)
                .ToListAsync();

            // Check if current user is a member or leader
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var isTeamLeader = team.TeamLeaderId == currentUserId;
            var isMember = team.UserTeams.Any(ut => ut.UserId == currentUserId);

            var viewModel = new TeamDetailsViewModel
            {
                Team = team,
                WorkSessions = workSessions,
                IsUserTeamLeader = isTeamLeader,
                IsUserMember = isMember
            };

            return View(viewModel);
        }

        // Additional Team controller actions would go here
    }
}