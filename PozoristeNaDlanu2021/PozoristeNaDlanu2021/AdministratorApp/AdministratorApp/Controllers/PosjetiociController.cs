using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdministratorApp.Models;
using AdministratorApp.Models.DAO;

namespace AdministratorApp.Controllers
{
    public class PosjetiociController : Controller
    {
        // GET: Posjetioci
        static List<User> korisnici = new List<User>();
        
        public ActionResult Index()
        {
            korisnici = new DAOFactoryImpl().CreateUserDAO().korisnici();
            return View(korisnici);
        }
        
        public ActionResult Delete(int? id)
        {
            Debug.WriteLine(id);
            User korisnik = korisnici.Find(x => x.UserId == id);
            if(korisnik!=null && new DAOFactoryImpl().CreateUserDAO().DeleteUser(id))
            {
                korisnici.Remove(korisnik);
                
            }
            return View("Index",korisnici);
        }
    }
}