using MySql.Data.MySqlClient;
using PozoristeNaDlanuTemplate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PozoristeNaDlanuTemplate.Controllers
{
    public class ProfilController : Controller
    {
        // GET: Profil
        public ActionResult Index()
        {
            string username = "";
            if (Session["UserName"]!=null)
            {
                username = Session["UserName"].ToString();
            }
            User korisnik=new UserDAO().GetUserByUsername(username);
            return View(korisnik);

        }
        public ActionResult Edit(int? id)
        {
            string username = "";
            if (Session["UserName"] != null)
            {
                username = Session["UserName"].ToString();
            }
            User korisnik = new UserDAO().GetUserByUsername(username);
            return View(korisnik);
        }
        [HttpPost]
        public  ActionResult Edit(User user)
        {
            User provjera = new UserDAO().GetUserByUsername(user.UserName);
            if(provjera==null || user.UserId==provjera.UserId)
            {
                Debug.WriteLine("UPDATEEe");
                if (user.Password == null)
                    Debug.WriteLine("pass    "+user.Password);
                Debug.WriteLine(user.UserId);
                new UserDAO().updateUser(user);
            }
            else
            {
                ViewBag.Message = String.Format("Username {0} vec postoji. Izaberite drugi username.", user.UserName);
            }
            
            return View(user);
        }
    
    }
}