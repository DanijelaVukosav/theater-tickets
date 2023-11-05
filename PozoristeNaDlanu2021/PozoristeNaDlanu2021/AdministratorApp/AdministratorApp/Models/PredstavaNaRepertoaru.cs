using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class PredstavaNaRepertoaru
    {
        public int idPredstave { get; set; }
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        public String naziv { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime vrijemeOdrzavanja { get; set; }
        [Required(ErrorMessage = "*")]
        public string nazivSale { get; set; }
        public float cijena { set; get; }
        public int idSale { get; set; }
    }
}