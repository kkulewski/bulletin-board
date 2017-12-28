using System;
using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.Models.JobOfferViewModels
{
    public class JobOfferViewModel
    {
        public string JobOfferId { get; set; }
        
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Display(Name = "Author")]
        public ApplicationUser Author { get; set; }

        [Display(Name = "Category")]
        public JobCategory JobCategory { get; set; }

        [Display(Name = "Type")]
        public JobType JobType { get; set; }

        public DateTime Submitted { get; set; }

        public DateTime LastEdit { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Wage { get; set; }

        public int Visits { get; set; }

        public bool CanEdit { get; set; }
    }
}
