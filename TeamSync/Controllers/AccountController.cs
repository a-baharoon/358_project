using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamSync.Data;
using TeamSync.Models;
using TeamSync.ViewModels;

namespace TeamSync.Controllers
{
    public class AccountController : Controller
    {
        private readonly TeamSyncContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AccountController> _logger;

        public AccountController(TeamSyncContext context, IWebHostEnvironment environment, ILogger<AccountController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? profilePicture = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Simple validation - check for existing email
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View(model);
                }

                // Create user
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    University = model.University,
                    AccountType = model.AccountType == "TeamLeader" ? UserType.TeamLeader : UserType.Student,
                    CreatedAt = DateTime.Now
                };

                // Handle file upload
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + profilePicture.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(fileStream);
                    }

                    user.ProfilePicturePath = "uploads/profiles/" + uniqueFileName;
                }

                // Save to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Success cookie
                Response.Cookies.Append("RegistrationSuccess", "true", new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(2)
                });

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration error: {ex.Message}");
                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View(model);
            }
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey("RegistrationSuccess"))
            {
                ViewBag.RegistrationSuccess = true;
                Response.Cookies.Delete("RegistrationSuccess");
            }

            if (Request.Cookies.ContainsKey("UserEmail"))
            {
                ViewBag.RememberedEmail = Request.Cookies["UserEmail"];
            }

            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Find user
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                    return View(model);
                }

                // Create authentication cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.AccountType.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
                    });

                // Session data
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.FullName);

                // Remember me cookie
                if (model.RememberMe)
                {
                    Response.Cookies.Append("UserEmail", user.Email, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30),
                        HttpOnly = true
                    });
                }
                else
                {
                    Response.Cookies.Delete("UserEmail");
                }

                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                ModelState.AddModelError("", "Login failed. Please try again.");
                return View(model);
            }
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}