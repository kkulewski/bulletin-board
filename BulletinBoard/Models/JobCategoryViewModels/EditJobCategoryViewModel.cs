using System.ComponentModel.DataAnnotations;
using BulletinBoard.Helpers.CustomValidators;

namespace BulletinBoard.Models.JobCategoryViewModels
{
    public class EditJobCategoryViewModel
    {
        public string JobCategoryId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Capitalized]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
