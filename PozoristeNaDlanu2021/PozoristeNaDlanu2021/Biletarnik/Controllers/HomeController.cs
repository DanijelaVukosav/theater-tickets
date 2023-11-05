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
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            User korisnik = null;

            korisnik = new DAOFactoryImpl().CreateUserDAO().userByName(user.UserName);
           
                
            if (korisnik != null && korisnik.Password.Equals(user.Password) && "b".Equals(korisnik.uloga) == true)
            {
                    Debug.WriteLine("postavi");
                    Session["UserID"] = korisnik.UserId.ToString();
                    Session["UserName"] = korisnik.UserName.ToString();
                    return RedirectToAction("Index", "Repertoar");
             }
            
            if (korisnik == null)
                return View(new User());
            
            return View(korisnik);
        }
       
        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}