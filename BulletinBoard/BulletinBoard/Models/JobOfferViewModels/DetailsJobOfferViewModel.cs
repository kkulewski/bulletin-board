using System;

namespace BulletinBoard.Models.JobOfferViewModels
{
    public class DetailsJobOfferViewModel
    {
        public string JobOfferId { get; set; }

        public ApplicationUser Author { get; set; }

        public JobCategory JobCategory { get; set; }

        public JobType JobType { get; set; }

        public DateTime Submitted { get; set; }

        public DateTime LastEdit { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Wage { get; set; }

        public bool Active { get; set; }
    }
}
