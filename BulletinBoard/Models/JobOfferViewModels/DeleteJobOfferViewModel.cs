namespace BulletinBoard.Models.JobOfferViewModels
{
    public class DeleteJobOfferViewModel
    {
        public string JobOfferId { get; set; }

        public ApplicationUser Author { get; set; }

        public JobCategory JobCategory { get; set; }

        public JobType JobType { get; set; }

        public string Title { get; set; }
    }
}
