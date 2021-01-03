using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Enciclopedie.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Campul nume este obligatoriu!")]
        [MinLength(5, ErrorMessage = "Numele categoriei trebuie sa aiba minim 5 de caractere!"),
         MaxLength(100, ErrorMessage = "Numele categoriei trebuie sa aiba maxim 100 de caractere!"),
         RegularExpression(@"^[A-Z].*", ErrorMessage = "Numele categoriei trebuie sa inceapa cu litera mare!")]
        public string CategoryName { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}