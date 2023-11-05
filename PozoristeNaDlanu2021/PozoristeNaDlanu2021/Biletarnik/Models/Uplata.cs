using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biletarnik.Models
{
    public class Uplata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ime je obavezno.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Nevalidan tekst.")]
        public string korisnickoIme{get;set;}
        [Required(AllowEmptyStrings = false, ErrorMessage = "Iznos je obavezan.")]
        [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Nevalidan tekst.")]
        public double iznos {get;set;}
    }
}