using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
