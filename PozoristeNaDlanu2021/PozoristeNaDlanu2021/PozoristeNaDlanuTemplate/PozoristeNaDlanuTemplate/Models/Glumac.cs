using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class Glumac
    {
        public int id { set; get; }

        public string ime { set; get; }
        
        public string prezime { set; get; }

        public string biografija { set; get; }

        public virtual ICollection<string> slikeGlumca { get; set; }

        public virtual ICollection<Predstava> igranePredstave { get; set; }
    }
}