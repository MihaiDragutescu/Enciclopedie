using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enciclopedie.Models
{
    public class Image
    {
        [Key, ForeignKey("Article"), DisplayName("Articol:")]
        public int Id { get; set; }

        [DisplayName("Titlu imagine:")]
        [Required(ErrorMessage = "Campul titlu este obligatoriu")]
        [MinLength(5, ErrorMessage = "Titlul trebuie sa aiba minim 5 de caractere!"),
         MaxLength(100, ErrorMessage = "Titlul trebuie sa aiba maxim 100 de caractere!"),
         RegularExpression(@"^[A-Z].*", ErrorMessage = "Titlul trebuie sa inceapa cu litera mare!")]
        public string ImageTitle { get; set; }

        [DisplayName("Incarca imaginea:"),
         RegularExpression(@"^.*\.(jpg|JPG|gif|GIF|doc|DOC|pdf|PDF|png|jfif)$", ErrorMessage = "Fisierul incarcat nu are o extensie corespunzatoare!")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public virtual Article Article { get; set; }    

        public IEnumerable<SelectListItem> Articles { get; set; }
    }
}