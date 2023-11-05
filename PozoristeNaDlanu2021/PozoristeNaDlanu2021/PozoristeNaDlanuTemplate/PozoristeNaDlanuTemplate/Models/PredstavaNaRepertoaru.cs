using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class PredstavaNaRepertoaru
    {
        public int id { get; set; }
        public int idPredstave { get; set; }
        public String naziv { get; set; }
        public float cijena { set; get; }
       public DateTime vrijemeOdrzavanja { get; set; }
        public int idSale { get; set; }
    }
}