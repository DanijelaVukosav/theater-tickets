using AdministratorApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdministratorApp.Models.DAO;
namespace AdministratorApp.Controllers
{
    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Informacije info = dfi.CreateInformacijeDAO().GetInformacije();
            //Informacije info = InformacijeDAO.GetInformacije();
            return View(info);
        }

        public ActionResult GlumacDetails(int? id)
        {
            if (id == null)
            {
                throw new HttpException(404, "Doslo je do greske!");
            }
            return View(new DAOFactoryImpl().CreateGlumacDAO().getGlumac(id));//  GlumacDAO.getGlumac(id));
        }
       
        [HttpGet]
        public ActionResult CreateInfo()
        {
            return View(new InformacijePomocna());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInfo(InformacijePomocna info)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Debug.WriteLine("UDJEEE da kreira");
            if (ModelState.IsValid)
            {
                Debug.WriteLine("UDJEEE validan");
                if (dfi.CreateInformacijeDAO().AddInfo(info))
                {
                    return RedirectToAction("Index");

                }
            }
            return View(new InformacijePomocna());
        }

        public ActionResult EditInfo(int? id)
        {
            Debug.WriteLine(id);
            InformacijePomocna info = new DAOFactoryImpl().CreateInformacijeDAO().GetInfoById(id);
            return View(info);
        }
       
        [HttpPost]
        public ActionResult EditInfo(InformacijePomocna info)
        {
                if(new DAOFactoryImpl().CreateInformacijeDAO().updateInfo(info))
                     return RedirectToAction("Index");
                else
                    return View(info);

        }

        public ActionResult DeleteInfo(int? id)
        {
            new DAOFactoryImpl().CreateInformacijeDAO().deleteInfo(id);// InformacijeDAO.deleteInfo(id);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult CreateGlumac()
        {
            return View(new Glumac() { slikeGlumca = new List<string>() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGlumac(Glumac glumac)
        {
            Debug.WriteLine("UDJEEE da kreira " +glumac.id);
            if (ModelState.IsValid)
            {
                Debug.WriteLine("UDJEEE validan");
                int id = new DAOFactoryImpl().CreateGlumacDAO().AddGlumac(glumac);// GlumacDAO.AddGlumac(glumac);
                if (id>0)
                {
                    Debug.WriteLine("idd " +id);
                    glumac.id = id;
                    return RedirectToAction("DodajSliku",new { glumac = glumac.id });
                }
            }
            return View(new Glumac());
        }
       
        [HttpGet]
        public ActionResult DodajSliku(int glumac)
        {
            Debug.WriteLine("Usao da doda sliku "+glumac);
            SlikaGlumca slika = new SlikaGlumca { idGlumca = glumac};
            slika.slike =new DAOFactoryImpl().CreateGlumacDAO().getSlikeGlumca(slika.idGlumca);
            return View(slika);
        }

        [HttpPost]
        public ActionResult DodajSliku(SlikaGlumca slika)
        {
            String josjedan = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName;
            String konacno = Directory.GetParent(josjedan).FullName + "\\PozoristeNaDlanuTemplate\\PozoristeNaDlanuTemplate\\SlikeGlumaca\\";

            Debug.WriteLine("Usao da doda sliku u bazu " + slika.idGlumca);
            string filename = Path.GetFileNameWithoutExtension(slika.ImageFile.FileName);
            string ekstenzija = Path.GetExtension(slika.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yyyymmssfff") + ekstenzija;
            slika.putanja = "~/SlikeGlumaca/" + filename;
            string putanjaZaKorisnika = konacno + filename;
            filename = Path.Combine(Server.MapPath("~/SlikeGlumaca/"), filename);
            
            Debug.WriteLine(slika.putanja);
            Debug.WriteLine(filename);
            slika.ImageFile.SaveAs(filename);
            slika.ImageFile.SaveAs(putanjaZaKorisnika);
            new DAOFactoryImpl().CreateGlumacDAO().AddSlikaGlumca(slika);
            //GlumacDAO.AddSlikaGlumca(slika);
            slika.slike = new DAOFactoryImpl().CreateGlumacDAO().getSlikeGlumca(slika.idGlumca);
            return View(slika);
        }
       
        public ActionResult IzbrisiSliku(int id,string putanja)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Debug.WriteLine("Usao da ukloni sliku;");
            dfi.CreateGlumacDAO().UkloniSliku(id, putanja);
            //GlumacDAO.UkloniSliku(id, putanja);
            return RedirectToAction("DodajSliku", new { glumac = id });
        }
        
        [HttpGet]
        public ActionResult DodajIgranuPredstavu(int id)
        {
            DAOFactoryImpl dfi=new DAOFactoryImpl();
            Debug.WriteLine("Usao da doda predstavu " + id);
            List<Predstava> neigranepredstave = dfi.CreateGlumacDAO().getNeigranePredstave(id);
            List<Predstava> igranepredstave = dfi.CreateGlumacDAO().IgranePredstaveGlumca(id);
            return View(new GlumacUPredstavi { idGlumac=id,ostalePredstave= neigranepredstave, igranePredstave=igranepredstave});
        }
        
        [HttpGet]
        public ActionResult DodajPredstavGlumcu(int idGlumac,int idPredstava)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Debug.WriteLine("Usao da doda predstavu u bazu" + idGlumac);
            dfi.CreateGlumacDAO().DodajPredstavuGlumcu(idGlumac, idPredstava);
            //GlumacDAO.DodajPredstavuGlumcu(idGlumac, idPredstava);
            return RedirectToAction("DodajIgranuPredstavu", new { id = idGlumac }); 
        }
        
        [HttpGet]
        public ActionResult IzbrisiPredstavuGlumcu(int idGlumac, int idPredstava)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Debug.WriteLine("Usao da doda predstavu u bazu" + idGlumac);
            dfi.CreateGlumacDAO().IzbrisiIgranuPredstavu(idGlumac, idPredstava);
            //GlumacDAO.IzbrisiIgranuPredstavu(idGlumac, idPredstava);
            return RedirectToAction("DodajIgranuPredstavu", new { id = idGlumac });
        }
        
        public ActionResult IzbrisiGlumca(int id)
        {
            new DAOFactoryImpl().CreateGlumacDAO().IzbrisiGlumca(id);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult EditGlumac(int id)
        {
            Glumac glumac = new DAOFactoryImpl().CreateGlumacDAO().getGlumac(id);//GlumacDAO.getGlumac(id);
            return View(glumac);
        }
        
        [HttpPost]
        public ActionResult EditGlumac(Glumac glumac)
        {
            new DAOFactoryImpl().CreateGlumacDAO().updateGlumac(glumac);
            return RedirectToAction("DodajSliku",new { glumac = glumac.id});
        }
    }
}