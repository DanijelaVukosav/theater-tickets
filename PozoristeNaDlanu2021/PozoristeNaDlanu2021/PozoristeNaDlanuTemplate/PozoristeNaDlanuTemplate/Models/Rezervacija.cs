using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class Rezervacija
    {
        [Key]
        public int idRezervacija { get; set; }

        [DisplayName("Broj karata")]
        public int brojKarata { set; get; }
        public int idPredstaveNaRepertoaru { set; get;}

        [DisplayName("Naslov predstave")]
        public string naslovPredstave { set; get; }
        public int idKorisnik { set; get; }
       
        public List<Sjediste> sjedista = new List<Sjediste>();
        public string sjedistastring;

        [DisplayName("Placeno")]
        public string placeno { set; get; }


    }
}