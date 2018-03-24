using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Models.RoleViewModels
{
    public class RoleViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }
        
        public string RoleId { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }

        /// <summary>
        /// Determines if role is change-able.
        /// </summary>
        public bool Disabled { get; set; }
    }
}
