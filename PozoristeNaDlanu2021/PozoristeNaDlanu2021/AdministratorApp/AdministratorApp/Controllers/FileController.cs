using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AdministratorApp.Models;
using AdministratorApp.Models.DAO;


namespace AdministratorApp.Controllers
{
    public class FileController : Controller
    {
        public static int idPredstave;
        public ActionResult UploadFiles(int id)
        {
            idPredstave = id;
            return View(new FileModel());
        }
        
        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] files)
        {
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        String josjedan = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName;
                        String konacno = Directory.GetParent(josjedan).FullName + "\\PozoristeNaDlanuTemplate\\PozoristeNaDlanuTemplate\\SlikePredstava\\";

                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/SlikePredstava/") + InputFileName);
                        new DAOFactoryImpl().CreatePredstavaDAO().dodajSlikuPredstave(idPredstave, "/SlikePredstava/" + InputFileName);
                        //Save file to server folder  
                        string putanjaZaKorisnika = konacno + InputFileName;

                        file.SaveAs(ServerSavePath);
                        file.SaveAs(putanjaZaKorisnika);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }

                }
            }
            return RedirectToAction("UploadFiles", "File");
        }
        
        public ActionResult AddGlumci()
        {
            List<Glumac> igraju = new DAOFactoryImpl().CreatePredstavaDAO().getGlumciPredstave(idPredstave);
            List<Glumac> neIgraju =new DAOFactoryImpl().CreatePredstavaDAO().GlumciKojiNeIgrajuUPredstavi(idPredstave);
            return View(new DodavanjeGlumacaUPredstavu { idPredstave = idPredstave, neIgrajuUPredstavi = neIgraju, glumciUPredstavi = igraju });
           
        }
        
        [HttpGet]
        public ActionResult DodajPredstavuGlumcu(int idGlumac)
        {
            Debug.WriteLine("Usao da doda predstavu u bazu" + idGlumac);
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            dfi.CreateGlumacDAO().DodajPredstavuGlumcu(idGlumac, idPredstave);
            //GlumacDAO.DodajPredstavuGlumcu(idGlumac, idPredstave);
            return RedirectToAction("AddGlumci");
        }
        
        [HttpGet]
        public ActionResult IzbrisiPredstavuGlumcu(int idGlumac)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Debug.WriteLine("Usao da izbrise predstavu u bazu" + idGlumac);
            dfi.CreateGlumacDAO().IzbrisiIgranuPredstavu(idGlumac, idPredstave);
            //GlumacDAO.IzbrisiIgranuPredstavu(idGlumac, idPredstave);
            return RedirectToAction("AddGlumci");
        }
    }
}