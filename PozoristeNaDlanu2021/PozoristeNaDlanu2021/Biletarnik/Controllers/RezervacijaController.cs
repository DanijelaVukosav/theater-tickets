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
    public class RezervacijaController : Controller
    {
        public ActionResult Index(int? id)
        {
            Sala sala = new DAOFactoryImpl().CreateSalaDAO().getById(id);
            return View(sala);
        }

        [HttpPost]
        public ActionResult Index(string Seats, int? Number, int? idPredstave, Boolean? kupovina)
        {
            if (Seats == null || Number == null || idPredstave == null || kupovina == null)
                return View();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int idKorisnik = Int32.Parse(Session["UserID"].ToString());

            /*if (kupovina == true)
            {
                double stanje = PosjetilacDAO.getStanjePosjetioca(idKorisnik);
               
                double cijena = PredstavaNaRepertoaruDAO.getCijenuPredstave(idPredstave);
               
                if (cijena > stanje)
                    return PartialView("StanjeNaRacunu");

                stanje -= cijena;
                //ovo je u stvari skidanje sa racuna ali sam koristila tu metodu koju vec imam
                PosjetilacDAO.UplataNaRacun(stanje, idKorisnik);

            }*/

            new DAOFactoryImpl().CreateRezervacijaDAO().DodajRezervaciju(Number, idPredstave, idKorisnik, Seats, kupovina);

            return RedirectToAction("Index", "Repertoar");
        }
    
        public ActionResult KupiRezervaciju(int?id)
        {
           new DAOFactoryImpl().CreateRezervacijaDAO().PotvrdiRezervaciju(id);
            return RedirectToAction("SveBuduceRezervacije", "Repertoar");
        }

        public ActionResult ObrisiRezervaciju(int?id)
        {
            new DAOFactoryImpl().CreateRezervacijaDAO().ObrisiRezervaciju(id);
            return RedirectToAction("SveBuduceRezervacije", "Repertoar");
        }
       
        [HttpGet]
        public ActionResult UplatiNaRacun() 
        {
            return View(new Uplata());
        }
     
        [HttpPost]
        public ActionResult UplatiNaRacun(Uplata uplata)
        {
            int userID = new DAOFactoryImpl().CreateUserDAO().getUserID(uplata.korisnickoIme);
            if(userID == -1)
                return View();
            double stanje =  new DAOFactoryImpl().CreatePosjetilacDAO().GetStanjePosjetioca(userID);
            stanje += uplata.iznos;
            new DAOFactoryImpl().CreatePosjetilacDAO().UplataNaRacun(stanje, userID);
            return RedirectToAction("SveBuduceRezervacije", "Repertoar");
        }
    }
}