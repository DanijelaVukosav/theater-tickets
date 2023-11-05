using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class Predstava
    {
        [Required(ErrorMessage = "*")]
        public int id { set; get; }
        [Required(ErrorMessage = "*")]
        public string naziv { set; get; }
        [Required(ErrorMessage = "*")]
        public DateTime datumPremijere { set; get; }
        [Required(ErrorMessage = "*")]
        public string rezija { set; get; }
        [Required(ErrorMessage = "*")]
        public string scenografija { set; get; }
        [Required(ErrorMessage = "*")]
        public string opis { set; get; }
        [Required(ErrorMessage = "*")]
        public string tekstopisac { set; get; }
        public virtual List<Glumac> glumci { set; get; }
        public List<String> putanje { set; get; }
        public string putanja { get; set; }
        public virtual ICollection<(int id, string username, string sadrzaj, DateTime datum)> komentari { set; get; }

        public Dictionary<int, string> idIputanje { get; set; }


    }
}