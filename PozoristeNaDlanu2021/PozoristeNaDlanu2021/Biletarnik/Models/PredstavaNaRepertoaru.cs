using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biletarnik.Models
{
    public class PredstavaNaRepertoaru
    {
        public int id { get; set; }
        public int idPredstave { get; set; }
        public String naziv { get; set; }

        public double cijena { get; set; }
       public DateTime vrijemeOdrzavanja { get; set; }
    }
}