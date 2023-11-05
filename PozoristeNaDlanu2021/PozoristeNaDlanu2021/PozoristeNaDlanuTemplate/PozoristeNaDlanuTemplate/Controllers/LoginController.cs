using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.Web.Mvc;
using PozoristeNaDlanuTemplate.Models;

using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace PozoristeNaDlanuTemplate.Controllers
{
    //ovo je vise kao user kontroler
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new User());
        }

        [HttpPost]
        // GET: Login
        public ActionResult Index(User user)
        {
            Debug.WriteLine(user == null);
            //if (ModelState.IsValid)
            // {
            User korisnik = new UserDAO().GetUserByUsername(user.UserName);
            try
            {
                

                if (korisnik != null && korisnik.Password.Equals(user.Password) && korisnik.uloga.Equals("p"))
                {
                    Debug.WriteLine("postavi");
                    Session["UserID"] = korisnik.UserId.ToString();
                    Session["UserName"] = korisnik.UserName.ToString();
                   // return Redirect(Request.UrlReferrer.ToString());
                    return RedirectToAction("Index", "Home");
                    //treba nekako da se zna da li se dolazi sa rezervacije ili ne
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                //con.Close();
                return View();
            }
            if (korisnik == null)
                return View(new User());
            //}
            return View(korisnik);
        }
        [HttpGet]
        
        public ActionResult CreateNewUser()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewUser(User user)
        {
            Debug.WriteLine("udje");
            UserDAO dao = new UserDAO();
            if (ModelState.IsValid)
            {
                Debug.WriteLine("validan");
                if (dao.GetUserByUsername(user.UserName)==null)
                {
                    Debug.WriteLine("prije unosenja");
                    dao.AddUser(user);
                    Debug.WriteLine("unese");
                    Session["UserID"] = user.UserId.ToString();
                    Session["UserName"] = user.UserName.ToString();
                    return RedirectToAction("Index", "Home");

                }
            }
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignInForUser(User user)
        {
            if (ModelState.IsValid)
            {
                List<User> korisnici = new UserDAO().SignIn();

                try
                {
                    
                    //con.Close();

                    var obj = korisnici.Where(a => a.UserName.Equals(user.UserName) && a.Password.Equals(user.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserId.ToString();
                        Session["UserName"] = obj.UserName.ToString();
                        return RedirectToAction("UserDashBoard");
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("Greskaaa");
                    //con.Close();
                    return View();
                }

            }
            return View();
        }


       public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}