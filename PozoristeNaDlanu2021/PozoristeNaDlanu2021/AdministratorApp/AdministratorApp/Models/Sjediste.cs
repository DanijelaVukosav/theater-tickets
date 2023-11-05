using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class Sjediste
    {
        public int idSjediste { get; set; }
        public int kolona { get; set; }
        public string vrsta { get; set; }
        public int idSale { get; set; }
        public string status { get; set; }
    }
}