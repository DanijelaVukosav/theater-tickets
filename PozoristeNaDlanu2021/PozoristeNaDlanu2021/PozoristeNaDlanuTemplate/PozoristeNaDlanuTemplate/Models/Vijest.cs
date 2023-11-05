using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class Vijest
    {
        public int idVijesti { set; get; }

        public string naslov { set; get; }

        public DateTime datum { set; get; }

        public string opis { set; get; }

        public virtual ICollection<string> slikeVijesti { set; get; }
        public virtual ICollection<(string username,string sadrzaj,DateTime datum)> komentariVijesti { set; get; }
    }
}