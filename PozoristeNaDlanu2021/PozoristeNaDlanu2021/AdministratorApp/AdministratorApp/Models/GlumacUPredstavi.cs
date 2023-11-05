using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class GlumacUPredstavi
    {
        public int idGlumac { set; get; }
        public List<Predstava> igranePredstave { set; get; }
        public List<Predstava> ostalePredstave { set; get; }
    }
}