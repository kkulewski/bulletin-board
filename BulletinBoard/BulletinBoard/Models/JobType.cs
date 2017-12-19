using System.Collections.Generic;

namespace BulletinBoard.Models
{
    public class JobType
    {
        public string JobTypeId { get; set; }

        public string Name { get; set; }

        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
