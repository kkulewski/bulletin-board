using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BulletinBoard.Helpers.CustomValidators;

namespace BulletinBoard.Models.JobOfferViewModels
{
    public class EditJobOfferViewModel
    {
        public string JobOfferId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string JobCategoryId { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string JobTypeId { get; set; }

        [Required]
        [PostalCode]
        [Display(Name = "Postal code", Prompt = "xx-xxx")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, 1_000_000.0, ErrorMessage = "Wage must be between 0.0 and 1000000.0")]
        [Display(Name = "Wage")]
        public decimal Wage { get; set; }
        
        public IEnumerable<JobCategory> JobCategories { get; set; }

        public IEnumerable<JobType> JobTypes { get; set; }
    }
}
