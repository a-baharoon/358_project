// Controllers/DashboardController.cs
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
    public class DashboardController : Controller
    {
        private readonly TeamSyncContext _context;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(TeamSyncContext context, ILogger<DashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get current user ID from claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID claim not found");
                    return RedirectToAction("Login", "Account");
                }

                int userId = int.Parse(userIdClaim.Value);

                // Get user from database with eager loading
                var user = await _context.Users
                    .Include(u => u.UserTeams)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found");
                    return RedirectToAction("Login", "Account");
                }

                // Get user's teams with separate query for better control
                var userTeams = await _context.UserTeams
                    .Where(ut => ut.UserId == userId)
                    .Include(ut => ut.Team)
                    .ToListAsync();

                var teams = userTeams.Select(ut => ut.Team).ToList();

                // Get user's work sessions
                var recentSessions = await _context.WorkSessions
                    .Where(ws => ws.UserId == userId)
                    .Include(ws => ws.Team)
                    .OrderByDescending(ws => ws.StartTime)
                    .Take(5)
                    .ToListAsync();

                // Create dashboard view model
                var dashboardViewModel = new DashboardViewModel
                {
                    UserProfile = user,
                    Teams = teams ?? new List<Team>(),
                    RecentSessions = recentSessions ?? new List<WorkSession>(),
                    TotalHoursWorked = recentSessions?.Sum(s => s.DurationMinutes ?? 0) / 60.0 ?? 0.0,
                    TasksCompleted = recentSessions?.Sum(s => s.TasksCompleted) ?? 0
                };

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Dashboard/Index: {ex.Message}");
                _logger.LogError(ex.StackTrace);
                // You could add more specific logging about the nature of the exception
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult StartTimer()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Get user's teams for dropdown
                var userTeams = _context.UserTeams
                    .Where(ut => ut.UserId == userId)
                    .Include(ut => ut.Team)
                    .Select(ut => ut.Team)
                    .ToList();

                ViewBag.UserTeams = userTeams;

                // Store session start time in session
                HttpContext.Session.SetString("TimerStartTime", DateTime.Now.ToString("o"));

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in StartTimer: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> StopTimer(int? teamId, string? reflectionNotes, int tasksCompleted)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Get start time from session
                if (!HttpContext.Session.TryGetValue("TimerStartTime", out var startTimeBytes))
                {
                    return RedirectToAction(nameof(Index));
                }

                var startTimeStr = System.Text.Encoding.UTF8.GetString(startTimeBytes);
                var startTime = DateTime.Parse(startTimeStr);
                var endTime = DateTime.Now;

                // Calculate duration
                var durationMinutes = (int)(endTime - startTime).TotalMinutes;

                // Create new work session
                var workSession = new WorkSession
                {
                    UserId = userId,
                    TeamId = teamId,
                    StartTime = startTime,
                    EndTime = endTime,
                    DurationMinutes = durationMinutes,
                    ReflectionNotes = reflectionNotes,
                    TasksCompleted = tasksCompleted
                };

                _context.Add(workSession);
                await _context.SaveChangesAsync();

                // Clear session timer data
                HttpContext.Session.Remove("TimerStartTime");

                // Set temporary success message in TempData
                TempData["TimerSuccess"] = "Work session recorded successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in StopTimer: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
    }
}