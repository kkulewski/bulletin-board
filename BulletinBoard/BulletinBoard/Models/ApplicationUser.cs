using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
