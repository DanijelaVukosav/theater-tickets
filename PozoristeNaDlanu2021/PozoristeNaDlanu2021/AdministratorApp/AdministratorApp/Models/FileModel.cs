using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models
{
    public class FileModel
    {
        public int idPredstave { set; get; }
        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        
        public  HttpPostedFileBase[] files { get; set; }
    }
}