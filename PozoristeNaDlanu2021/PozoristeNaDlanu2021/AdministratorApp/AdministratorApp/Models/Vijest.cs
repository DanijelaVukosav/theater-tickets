using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class Vijest
    {
        [Key]
        public int idVijesti { set; get; }

        public string naslov { set; get; }

        public DateTime datum { set; get; }

        public string opis { set; get; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase file { get; set; }
        public virtual ICollection<string> slikeVijesti { set; get; }
        public virtual ICollection<(string username, string sadrzaj, DateTime datum)> komentariVijesti { set; get; }
    }
}