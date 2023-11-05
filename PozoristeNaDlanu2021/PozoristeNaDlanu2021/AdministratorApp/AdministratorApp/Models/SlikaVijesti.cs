using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class SlikaVijesti
    {
        public int idVijesti { set; get; }
        [DisplayName("Izaberiste sliku")]
        public string putanja { set; get; }
        public HttpPostedFileBase ImageFile
        {
            set; get;
        }
        public List<string> slike { set; get; }
    }
}