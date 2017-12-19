using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.Models.JobCategoryViewModels
{
    public class CreateJobCategoryViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
