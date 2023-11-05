using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class InformacijePomocna
    {
        [Key]
        public int id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Naslov je obavezan.")]
        [DisplayName("Naslov")]
        public string naslov { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Sadrzaj je obavezan")]
        [DisplayName("Sadrzaj")]
        public string sadrzaj { get; set; }
    }
}