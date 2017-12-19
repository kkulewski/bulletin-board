namespace BulletinBoard.Models
{
    public class JobOffer
    {
        public string Id { get; set; }

        public ApplicationUser Author { get; set; }

        public JobCategory JobCategory { get; set; }

        public JobType JobType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Wage { get; set; }
    }
}
