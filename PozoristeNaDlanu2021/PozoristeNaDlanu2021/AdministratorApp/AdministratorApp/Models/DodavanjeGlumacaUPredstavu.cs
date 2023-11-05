using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class DodavanjeGlumacaUPredstavu
    {
        public int idPredstave { set; get; }
        public List<Glumac> glumciUPredstavi { set; get; }
        public List<Glumac> neIgrajuUPredstavi { set; get; }
    }
}