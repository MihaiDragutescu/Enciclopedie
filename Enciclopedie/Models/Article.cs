using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enciclopedie.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campul titlu este obligatoriu")]
        public string Title { get; set; }

        [MinLength(10, ErrorMessage = "Continutul trebuie sa fie format din minim 10 caractere")][Required(ErrorMessage = "Campul continut este obligatoriu")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Nu ati selectat o categorie")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ApplicationUser User { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}