using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class Sala
    {
        public int idSale { get; set; }
        public string imeSale { get; set; }
        public int brojKolona { get; set; }
        public int brojVrsta { get; set; }
        public List<Sjediste> sjedista { get; set; }

        public int idPredstave { get; set; }
    }
}