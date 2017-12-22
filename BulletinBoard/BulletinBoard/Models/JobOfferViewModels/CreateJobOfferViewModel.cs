using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.Models.JobOfferViewModels
{
    public class CreateJobOfferViewModel
    {
        public string JobOfferId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public JobCategory JobCategory { get; set; }

        [Required]
        [Display(Name = "Type")]
        public JobType JobType { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 50)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Wage")]
        public decimal Wage { get; set; }
    }
}
