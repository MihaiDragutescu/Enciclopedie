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

        [RegularExpression(@"^07(\d{8})$", ErrorMessage = "Numarul de telefon nu este valid!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Nu ati selectat un rol!")]
        public string RoleName { get; set; }

        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Formatul datei nu este valid!")]
        [Display(Name = "Data nasterii (dd/mm/yyyy)")]
        public string DateOfBirth { get; set; }
    }
}