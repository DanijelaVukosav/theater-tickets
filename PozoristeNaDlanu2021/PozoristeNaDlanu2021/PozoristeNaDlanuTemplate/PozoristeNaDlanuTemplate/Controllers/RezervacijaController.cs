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
    public class RezervacijaController : Controller
    {
        public ActionResult Index(int? id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            Sala sala = new RezervacijaDAO().vratiSalu(id);
            

            return View(sala);
        }

        [HttpPost]
        public ActionResult Index(string Seats, int? Number, int? idPredstave, Boolean kupovina)
        {
            Debug.WriteLine(Seats);
            Debug.WriteLine(Number);
            Debug.WriteLine(idPredstave);
            Debug.WriteLine(kupovina);
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if(new RezervacijaDAO().rezervisi(Seats, Number, idPredstave, kupovina, Int32.Parse(Session["UserID"].ToString())) == 2)
            {
                return PartialView("StanjeNaRacunu");
            }

            return RedirectToAction("ListaRezervacija", "Rezervacija");
        }
        [HttpGet]
        public ActionResult ListaRezervacija()
        {
            List<Rezervacija> rezervacije = new RezervacijaDAO().rezervacijePrijavljenogKorisnika(Session["UserID"].ToString());
            return View(rezervacije);
        }

    }
}
