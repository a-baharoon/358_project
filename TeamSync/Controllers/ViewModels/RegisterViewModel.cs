// ViewModels/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace TeamSync.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "University is required")]
        public string University { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type is required")]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; } = "Student";

        [Required(ErrorMessage = "You must accept the terms and conditions")]
        public bool TermsAccepted { get; set; }
    }
}