using AdministratorApp.Models;
using AdministratorApp.Models.DAO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministratorApp.Controllers
{
    public class BiletarnikController : Controller
    {
        static List<User> biletarnici = new List<User>();

        public ActionResult Index()
        {
            biletarnici = new DAOFactoryImpl().CreateUserDAO().biletarnici();
            return View(biletarnici);
        }
        
        public ActionResult Edit(string id)
        {
            Debug.WriteLine(id);
            User biletarnik = new DAOFactoryImpl().CreateUserDAO().GetUserByUsername(id);
            
            return View(biletarnik);
        }
        
        [HttpPost]
        public ActionResult Edit(User user)
        {
            User provjera = new DAOFactoryImpl().CreateUserDAO().GetUserByUsername(user.UserName);
            if (provjera == null || user.UserId == provjera.UserId)
            {
                Debug.WriteLine("UPDATEEe");
                if (user.Password == null)
                    Debug.WriteLine("pass    " + user.Password);
                Debug.WriteLine(user.UserId);
                new DAOFactoryImpl().CreateUserDAO().updateUser(user);
            }
            else
            {
                ViewBag.Message = String.Format("Username {0} vec postoji. Izaberite drugi username.", user.UserName);
            }

            return RedirectToAction("Index");
        }
        
        public ActionResult Delete(int? id)
        {
            Debug.WriteLine(id);
            User korisnik = biletarnici.Find(x => x.UserId == id);
            if (korisnik != null && new DAOFactoryImpl().CreateUserDAO().DeleteUser(id))
            {
                biletarnici.Remove(korisnik);

            }
            return View("Index", biletarnici);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View(new User());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            Debug.WriteLine("UDJEEE da kreira");
            if (ModelState.IsValid)
            {
                Debug.WriteLine("UDJEEE validan");
                if (new DAOFactoryImpl().CreateUserDAO().GetUserByUsername(user.UserName) == null && new DAOFactoryImpl().CreateUserDAO().AddBiletarnik(user))
                {
                    Debug.WriteLine("UDJEEE u if");
                    biletarnici.Add(user);
                    Debug.WriteLine("unese");
                    return View("Index",biletarnici);

                }
            }
            return View(new User());
        }

    }
}