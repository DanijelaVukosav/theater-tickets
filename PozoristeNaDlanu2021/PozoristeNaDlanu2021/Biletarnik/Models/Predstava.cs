using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biletarnik.Models
{
    public class Predstava
    {
        public int id { set; get; }

        public string naziv { set; get; }

        public DateTime datumPremijere { set; get; }

        public string rezija { set; get; }
        public string scenografija { set; get; }
        public string opis { set; get; }
        public string tekstopisac { set; get; }
        public string putanja { get; set; }
        public virtual ICollection<(string username, string sadrzaj, DateTime datum)> komentari { set; get; }


    }
}