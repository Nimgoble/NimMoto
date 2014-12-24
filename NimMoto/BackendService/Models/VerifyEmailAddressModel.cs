using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BackendService.Models
{
    public class VerifyEmailAddressModel
    {
        [Required]
        public String UserId { get; set; }
        [Required]
        public String Code { get; set; }
    }
}
