using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PozoristeNaDlanuTemplate.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace PozoristeNaDlanuTemplate.Controllers
{
    public class ZanrController : Controller
    {
        // GET: Zanr
        List<Predstava> predstave = new List<Predstava>();
        [HttpGet]
        public ActionResult Index(string search)
        {
            predstave = new PredstavaDAO().predstaveList();
            Debug.WriteLine(search);
            if (search!=null && search!="")
                predstave=predstave.Where(x=>x.naziv.Contains(search) || search == null).ToList();


            return View(predstave);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            Predstava predstava = new PredstavaDAO().GetPredstavaById(id);
            return View(predstava);
        }
        [HttpPost]
        public ActionResult Details(Predstava predstava)
        {
            if(Session["UserID"]==null)
                return RedirectToAction("Index", "Login");
            string komentar = Request["Message"].ToString();
            string korisnik = Session["UserID"].ToString();
            Debug.WriteLine("Pozvao ovu drugu");

            new PredstavaDAO().dodajKomentar(predstava, komentar, korisnik);
            return View(new PredstavaDAO().GetPredstavaById(predstava.id));
        }
    }
}