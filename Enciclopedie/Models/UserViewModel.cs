using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Enciclopedie.Models
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Nu ati selectat un rol!")]
        public string RoleName { get; set; }
    }
}