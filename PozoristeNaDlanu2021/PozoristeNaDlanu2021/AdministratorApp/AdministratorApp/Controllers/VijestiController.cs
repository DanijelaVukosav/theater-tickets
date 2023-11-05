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
    public class VijestiController : Controller //ne koristim ovaj sve mi je u homecontroller
    {
        public ActionResult Index()
        {
            String path = Server.MapPath("~/slikevijesti/");
            String[] imageFiles = Directory.GetFiles(path);
            ViewBag.images = imageFiles;
            return View(new DAOFactoryImpl().CreateVijestDAO().listaVijesti());
        }

        public ActionResult VijestDetails(int idVijesti)
        {
            return View(new DAOFactoryImpl().CreateVijestDAO().GetVijestById(idVijesti));
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Vijest vijest, HttpPostedFileBase[] files)
        {
            try
            {
                //Ensure model state is valid  
                if (ModelState.IsValid)
                {   //iterating through multiple file collection   
                    int i = 0;
                    foreach (HttpPostedFileBase file in files)
                    {

                        //Checking file is available to save.  
                        if (file != null)
                        {

                            //file.SaveAs(Path.Combine(Server.MapPath("/uploads"), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                            var InputFileName = Path.GetFileName(file.FileName);
                            var ServerSavePath = Path.Combine(Server.MapPath("/slikevijesti/"), InputFileName);

                            //Save file to server folder 
                            //file.SaveAs(ServerSavePath);
                            //assigning file uploaded status to ViewBag for showing message to user.  
                            //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                            //vijest.files[i++] = file;
                            vijest.slikeVijesti.Add(ServerSavePath);

                        }

                    }
                }

                new DAOFactoryImpl().CreateVijestDAO().dodajVijest(vijest);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {

            return View(new DAOFactoryImpl().CreateVijestDAO().GetVijestById(id));
        }

        [HttpPost]
        public ActionResult Edit(Vijest vijest)
        {

            try
            {

                /*if (ModelState.IsValid)
                {   //iterating through multiple file collection   
                    int i = 0;
                    foreach (HttpPostedFileBase file in files)
                    {

                        //Checking file is available to save.  
                        if (file != null)
                        {

                            //file.SaveAs(Path.Combine(Server.MapPath("/uploads"), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                            var InputFileName = Path.GetFileName(file.FileName);
                            var ServerSavePath = Path.Combine(Server.MapPath("/slikevijesti/"), InputFileName);

                            //Save file to server folder 
                            //file.SaveAs(ServerSavePath);
                            //assigning file uploaded status to ViewBag for showing message to user.  
                            //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                            //vijest.files[i++] = file;
                            vijest.slikeVijesti.Add(ServerSavePath);

                        }

                    }
                }*/

                new DAOFactoryImpl().CreateVijestDAO().sacuvajIzmjene(vijest);

                return RedirectToAction("Details", new { id = vijest.idVijesti });
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult Delete(int id)
        {
            Vijest v = new DAOFactoryImpl().CreateVijestDAO().GetVijestById(id);

            if (v == null)
                return View("Index");
            else
                return View(v);
        }

        
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                DAOFactoryImpl dfi = new DAOFactoryImpl();
                // TODO: Add delete logic here
                Vijest v = dfi.CreateVijestDAO().GetVijestById(id);

                if (v == null)
                    return View("Index");

                List<Vijest> lista = dfi.CreateVijestDAO().listaVijesti();
                lista.Remove(v);
                //lista.Save();
                dfi.CreateVijestDAO().izbrisiVijest(v);
                return View("Delete");

            }
            catch
            {
                return View();
            }
        }
    }
}
