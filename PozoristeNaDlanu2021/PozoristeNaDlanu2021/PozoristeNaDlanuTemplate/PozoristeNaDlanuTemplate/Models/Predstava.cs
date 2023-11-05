using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
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
        public List<String> putanje { set; get; }
        public Dictionary<int, string> idIputanje { get; set; }
        public virtual List<Glumac> glumci { set; get; }
        public virtual ICollection<(int id, string username, string sadrzaj, DateTime datum)> komentari { set; get; }


    }
}