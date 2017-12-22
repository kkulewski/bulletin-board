namespace BulletinBoard.Models.JobOfferViewModels
{
    public class CreateJobOfferViewModel
    {
        public string JobOfferId { get; set; }

        public JobCategory JobCategory { get; set; }

        public JobType JobType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Wage { get; set; }
    }
}
