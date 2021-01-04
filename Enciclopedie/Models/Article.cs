using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Enciclopedie.Models.Validations;

namespace Enciclopedie.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campul titlu este obligatoriu!")]
        [MinLength(2, ErrorMessage = "Titlul trebuie sa aiba minim 2 caractere!"),
         MaxLength(200, ErrorMessage = "Titlul trebuie sa aiba maxim 200 de caractere!"),
         RegularExpression(@"^[A-Z].*", ErrorMessage = "Titlul trebuie sa inceapa cu litera mare!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campul rezumat este obligatoriu!")]
        [MinLength(50, ErrorMessage = "Rezumatul trebuie sa aiba minim 50 de caractere!"),
         MaxLength(1000, ErrorMessage = "Rezumatul trebuie sa aiba maxim 1000 de caractere!")]
        public string Summary { get; set; }

        [ContentValidator]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Nu ati setat disponibilitatea articolului!")]
        public bool Available { get; set; }

        [Required(ErrorMessage = "Nu ati selectat o categorie!")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Image Image { get; set; }
        public virtual Category Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}