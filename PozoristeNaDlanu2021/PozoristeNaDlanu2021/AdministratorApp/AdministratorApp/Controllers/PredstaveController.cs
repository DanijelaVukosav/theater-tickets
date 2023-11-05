using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdministratorApp.Models;
using AdministratorApp.Models.DAO;
using MySql.Data.MySqlClient;

namespace AdministratorApp.Controllers
{
    public class PredstaveController : Controller
    {
        // GET: Predstave
        public ActionResult Index()
        {
            List<Predstava> predstave = new DAOFactoryImpl().CreatePredstavaDAO().predstaveList();
            return View(predstave);
        }


        [HttpGet]
        public ActionResult Details(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Predstava predstava = new DAOFactoryImpl().CreatePredstavaDAO().GetPredstavaById(id);
            return View(predstava);
        }
        
        [HttpPost]
        public ActionResult Details(Predstava predstava)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            string komentar = Request["Message"].ToString();
            Debug.WriteLine("Pozvao ovu drugu");

            new DAOFactoryImpl().CreatePredstavaDAO().dodajKomentar(komentar, predstava.id, Session["UserID"].ToString());
          
           
            return View(new DAOFactoryImpl().CreatePredstavaDAO().GetPredstavaById(predstava.id));
        }


        [HttpGet]
        public ActionResult AddNew()
        {
            return View(new Predstava());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Predstava predstava)
        {

            if (new DAOFactoryImpl().CreatePredstavaDAO().addPredstavu(predstava) == true)
            {
                FileController.idPredstave = predstava.id;
                Debug.WriteLine(predstava.id);
                return RedirectToAction("UploadFiles", "File",new { id = predstava.id });
            }
            else
                return View(new Predstava());
        }

        [HttpGet]
        public ActionResult Edit(int?id)
        {
            Predstava predstava = new DAOFactoryImpl().CreatePredstavaDAO().GetPredstavaById(id);
            FileController.idPredstave = predstava.id;
            return View(predstava);
        }

        [HttpPost]
        public ActionResult Edit(Predstava predstava)
        {
            new DAOFactoryImpl().CreatePredstavaDAO().izmijeniPredstavu(predstava);
            return RedirectToAction("UploadFiles", "File",new { id=predstava.id});
        }

        public ActionResult Delete(int? id)
        {
            Debug.WriteLine("brisanje");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            new DAOFactoryImpl().CreatePredstavaDAO().obrisiPredstavu(id);
            return RedirectToAction("Index", "Predstave");
        }
   
        public ActionResult pictureDelete(int? id)
        {
            if (id == null)
                Debug.WriteLine("ooo");
            Debug.WriteLine("udje");
            int idPredstave = new DAOFactoryImpl().CreatePredstavaDAO().obrisiSliku(id);
            if(idPredstave != -1)
                return RedirectToAction("Edit", "Predstave", new { id = idPredstave});
            return RedirectToAction("Index", "Predstave");
        }
    
        public ActionResult obrisiKomentar(int?id)
        {
            Debug.WriteLine("brisanje komentara");
            int idPredstave = new DAOFactoryImpl().CreatePredstavaDAO().obrisiKomentar(id);
            if(idPredstave != -1)
                return RedirectToAction("Details", "Predstave", new { id = idPredstave });
            return RedirectToAction("Index", "Predstave");
        }  
    }
}