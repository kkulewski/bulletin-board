using System;

namespace BulletinBoard.Models
{
    public class JobOffer
    {
        public string JobOfferId { get; set; }

        public ApplicationUser Author { get; set; }

        public string AuthorId { get; set; }

        public JobCategory JobCategory { get; set; }

        public string JobCategoryId { get; set; }

        public JobType JobType { get; set; }

        public string JobTypeId { get; set; }

        public DateTime Submitted { get; set; }

        public DateTime LastEdit { get; set; }

        public string PostalCode { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Wage { get; set; }

        public int Visits { get; set; }
    }
}
