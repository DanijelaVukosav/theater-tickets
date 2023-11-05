using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using PozoristeNaDlanuTemplate.Models;

namespace PozoristeNaDlanuTemplate.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new VijestDAO().listaVijesti());
        }
        [HttpGet]
        public ActionResult VijestDetails(int? idVijesti)
        {

            Vijest vijest = new VijestDAO().GetVijestById(idVijesti);
            return View(vijest);
        }
        [HttpPost]
        public ActionResult VijestDetails(Vijest vijest)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            string komentar = Request["Message"].ToString();
            string korisnik = Session["UserID"].ToString();
            VijestDAO.detaljiVijesti(vijest, komentar, korisnik);
            return View(new VijestDAO().GetVijestById(vijest.idVijesti));
            //Debug.WriteLine("Pozvao ovu drugu");

        }
    }

}