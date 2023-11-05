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
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
           
            String path = Server.MapPath("~/slikevijesti/");
            String[] imageFiles = Directory.GetFiles(path);
            ViewBag.images = imageFiles;
            return View(new DAOFactoryImpl().CreateVijestDAO().listaVijesti());
        }

        [HttpGet]
        public ActionResult VijestDetails(int? idVijesti)
        {

            Vijest vijest = new DAOFactoryImpl().CreateVijestDAO().GetVijestById(idVijesti);
            return View(vijest);
        }
        
        [HttpPost]
        public ActionResult VijestDetails(Vijest vijest)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            string komentar = Request["Message"].ToString();
            string korisnik = Session["UserID"].ToString();
            dfi.CreateVijestDAO().detaljiVijesti(vijest, komentar, korisnik);
            return View(dfi.CreateVijestDAO().GetVijestById(vijest.idVijesti));
        }

        public ActionResult VijestCreate()
        {
            return View(new Vijest() { slikeVijesti=new List<string>()});
        }

        
        [HttpPost]
        public ActionResult VijestCreate(Vijest vijest)
        {
            vijest.datum = DateTime.Now;
            try
            { 
                
                int id=new DAOFactoryImpl().CreateVijestDAO().dodajVijest(vijest);
                if(id>0)
                    return RedirectToAction("DodajSliku",new { idVijest=id });
                else
                    return View(new Vijest() { slikeVijesti = new List<string>() });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        
        [HttpGet]
        public ActionResult DodajSliku(int idVijest)
        {
            Debug.WriteLine("Usao da doda sliku " + idVijest);
            SlikaVijesti slika = new SlikaVijesti { idVijesti = idVijest };
            slika.slike = new DAOFactoryImpl().CreateVijestDAO().GetSlikeVijesti(idVijest);
            return View(slika);
        }

        [HttpPost]
        public ActionResult DodajSliku(SlikaVijesti slika)
        {
            String josjedan = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName;
            String konacno = Directory.GetParent(josjedan).FullName + "\\PozoristeNaDlanuTemplate\\PozoristeNaDlanuTemplate\\slikevijesti\\";

            Debug.WriteLine("Usao da doda sliku u bazu " + slika.idVijesti);
            string filename = Path.GetFileNameWithoutExtension(slika.ImageFile.FileName);
            string ekstenzija = Path.GetExtension(slika.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yyyymmssfff") + ekstenzija;
            slika.putanja = "~/slikevijesti/" + filename;
            string putanjaZaKorisnika = konacno + filename;
            filename = Path.Combine(Server.MapPath("~/slikevijesti/"), filename);
           // string filename2= Path.Combine(Server.MapPath("C:/Users/pc/Downloads/ps/ps/PozoristeNaDlanuTemplate/PozoristeNaDlanuTemplate/"), filename);
            Debug.WriteLine(slika.putanja);
            Debug.WriteLine(filename);
            slika.ImageFile.SaveAs(filename);
            slika.ImageFile.SaveAs(putanjaZaKorisnika);
            new DAOFactoryImpl().CreateVijestDAO().AddSlikaVijest(slika);
            slika.slike = new DAOFactoryImpl().CreateVijestDAO().GetSlikeVijesti(slika.idVijesti);
            return View(slika);
        }

        public ActionResult IzbrisiSliku(int id, string putanja)
        {
            Debug.WriteLine("Usao da ukloni sliku;");
            new DAOFactoryImpl().CreateVijestDAO().UkloniSliku(id, putanja);
            return RedirectToAction("DodajSliku", new { idVijest = id });
        }

        [HttpGet]
        public ActionResult VijestEdit(int? id)
        {
            Debug.WriteLine("Poslato " + id);
            Vijest v = new DAOFactoryImpl().CreateVijestDAO().GetVijestById(id);
            Debug.WriteLine("dobijeno " + v.idVijesti);
            return View(v);
        }

        [HttpPost]
        public ActionResult VijestEdit(Vijest vijest)
        {

            try
            {
                Debug.WriteLine(vijest.idVijesti);
                Debug.WriteLine(vijest.naslov);
                Debug.WriteLine(vijest.opis);
                new DAOFactoryImpl().CreateVijestDAO().sacuvajIzmjene(vijest);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(vijest);
            }
        }

        public ActionResult VijestDelete(int id)
        {
            Vijest v = new DAOFactoryImpl().CreateVijestDAO().GetVijestById(id);

            if (v == null)
                return View("Not found");
            else
                return View(v);
        }

        [HttpPost]
        public ActionResult VijestDelete(int id, FormCollection collection)
        {
            try
            {
                DAOFactoryImpl dfi = new DAOFactoryImpl();
                // TODO: Add delete logic here
                Vijest v = dfi.CreateVijestDAO().GetVijestById(id);

                if (v == null)
                    return View("Not found");

                List<Vijest> lista = dfi.CreateVijestDAO().listaVijesti();
                lista.Remove(v);
                //lista.Save();
                dfi.CreateVijestDAO().izbrisiVijest(v);
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
    }
}