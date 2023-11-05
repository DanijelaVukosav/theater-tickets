using AdministratorApp.Models;
using AdministratorApp.Models.DAO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministratorApp.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            
            return View(new User());
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            Debug.WriteLine(user == null);
            //if (ModelState.IsValid)
            // {
            List<User> korisnici = new List<User>();
            User korisnik = new DAOFactoryImpl().CreateUserDAO().GetUserByUsername(user.UserName);
            /*string mycon = "server =localhost; Uid=root; password = Vukosav99; persistsecurityinfo = True; database =pozoriste; SslMode = none";

            MySqlConnection con = new MySqlConnection(mycon);
            MySqlCommand cmd = null;
            try
            {
                cmd = new MySqlCommand("select * from korisnik where username=@username", con);
                cmd.Parameters.AddWithValue("@username", user.UserName);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                string uloga = "";
                while (reader.Read())
                {

                    int UserId = (int)reader[0];
                    string UserName = (string)reader[2];
                    string Password = (string)reader[3];
                    string Email = (string)reader[6];
                    string telefon = "";
                    try
                    {
                        telefon = (string)reader[8];
                    }
                    catch (InvalidCastException)
                    { }
                    string adresa = "";
                    try
                    {
                        telefon = (string)reader[9];
                    }
                    catch (InvalidCastException)
                    { }
                    uloga= (string)reader[1];
                    User k = korisnik = new User { UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };
                    korisnici.Add(k);
                }
                con.Close();*/
            if (korisnik != null && korisnik.Password.Equals(user.Password) && korisnik.uloga.Equals("a"))
            {
                Debug.WriteLine("postavi");
                Session["UserID"] = korisnik.UserId.ToString();
                Session["UserName"] = korisnik.UserName.ToString();
                    // return Redirect(Request.UrlReferrer.ToString());
                return RedirectToAction("Index", "Home");
                    //treba nekako da se zna da li se dolazi sa rezervacije ili ne
            }
            
            if (korisnik == null)
                return View(new User());
            
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
            UserDAO dao = new UserDAO();
            if (ModelState.IsValid)
            {
                if (new DAOFactoryImpl().CreateUserDAO().GetUserByUsername(user.UserName) == null)
                {
                    new DAOFactoryImpl().CreateUserDAO().AddUser(user);
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
                //List<User> korisnici = new List<User>();

                /*string mycon = "server =localhost; Uid=root; password = Vukosav99; persistsecurityinfo = True; database =pozoriste; SslMode = none";

                MySqlConnection con = new MySqlConnection(mycon);
                MySqlCommand cmd = null;
                try
                {
                    cmd = new MySqlCommand("select * from korisnik", con);

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int UserId = (int)reader[0];
                        string UserName = (string)reader[2];
                        string Password = (string)reader[3];
                        string Email = (string)reader[6];
                        string telefon = "";
                        try
                        {
                            telefon = (string)reader[8];
                        }
                        catch (InvalidCastException)
                        { }
                        string adresa = "";
                        try
                        {
                            telefon = (string)reader[9];
                        }
                        catch (InvalidCastException)
                        { }
                        User k = new User { UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };
                        korisnici.Add(k);
                    }
                    con.Close();*/
                List<User> korisnici = new DAOFactoryImpl().CreateUserDAO().sviKorisnici();

                var obj = korisnici.Where(a => a.UserName.Equals(user.UserName) && a.Password.Equals(user.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserID"] = obj.UserId.ToString();
                    Session["UserName"] = obj.UserName.ToString();
                    return RedirectToAction("UserDashBoard");
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}