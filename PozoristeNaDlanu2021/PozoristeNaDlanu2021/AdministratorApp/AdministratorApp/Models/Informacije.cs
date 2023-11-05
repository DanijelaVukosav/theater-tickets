
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class Informacije
    {
        public virtual List<InformacijePomocna> informacije { set; get; }
        public virtual List<Glumac> glumci { set; get; }
    }
}