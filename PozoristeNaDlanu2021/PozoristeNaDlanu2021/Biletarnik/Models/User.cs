using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Biletarnik.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Ime je obavezno.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Nevalidan tekst.")]
        [DisplayName("Ime")]
        public string FirstName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Prezime je ob.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Nevalidan tekst.")]
        [DisplayName("Prezime")]
        public string LastName { get; set; }

        [Required]

        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lozinka je obavezna.")]
        [DisplayName("Lozinka")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Pol")]
        [RegularExpression(@"m|M|Z|z", ErrorMessage = "Pol moze biti 'm' ili 'z'.")]
        public string Gender { get; set; }

        [DisplayName("Mobile No")]
        public string MobileNo { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        public string Address { get; set; }

        public string uloga { get; set; }
       // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
       // public virtual ICollection<Booking> Bookings { get; set; }
    }
}