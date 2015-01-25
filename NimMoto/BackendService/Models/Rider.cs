using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackendService.Models
{
    public class Rider : IdentityUser
    {
        public Rider()
        {

        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Rider> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        [MaxLength(6)]
        public String CustomEmailConfirmationToken { get; set; }
        public Int32 IsEmailConfirmedViaCustomToken { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Int32 Age { get; set; }
        public String ZipCode { get; set; }
    }
}
