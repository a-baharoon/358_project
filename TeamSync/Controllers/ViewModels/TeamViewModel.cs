// ViewModels/TeamViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace TeamSync.ViewModels
{
    public class TeamViewModel
    {
        public int TeamId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}