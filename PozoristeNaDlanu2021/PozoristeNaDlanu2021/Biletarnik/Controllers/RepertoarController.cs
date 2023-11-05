using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biletarnik.Models;
using Biletarnik.Models.DAO;
using MySql.Data.MySqlClient;
namespace Biletarnik.Controllers
{
    public class RepertoarController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            
            if (Session["UserID"] != null)
            {
                Debug.WriteLine(Session["UserID"].ToString());
            }
            else
            {
                Debug.WriteLine("null");
            }
            List<PredstavaNaRepertoaru> predstave = new DAOFactoryImpl().CreatePredstavaNaRepertoaruDAO().getAll();

            return View(predstave);
        }
        public ActionResult RezervacijePredstave(int? id)
        {
            Debug.WriteLine(id + " kao ide prssssssss");
            List<Rezervacija> rezervacije= new DAOFactoryImpl().CreateRezervacijaDAO().rezervacijePredstave(id);
            return View(rezervacije);
        }
        public ActionResult SveBuduceRezervacije(string search)
        {
            List<Rezervacija> rezervacije = new List<Rezervacija>();
            if (search == null || search.Equals(""))
            {
                rezervacije = new DAOFactoryImpl().CreateRezervacijaDAO().rezervacijePredstave(-1);
            }
            else
            {
                rezervacije = new DAOFactoryImpl().CreateRezervacijaDAO().rezervacijeOdredjenogKorisnika(-1,search);
            }
            return View(rezervacije);
        }
    }
}