using PozoristeNaDlanuTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class Informacije
    {
        public virtual List<(string naslov,string sadrzaj)> informacije { set; get; }
        public virtual List<Glumac> glumci { set; get; }
    }
}