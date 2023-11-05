using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PozoristeNaDlanuTemplate.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace PozoristeNaDlanuTemplate.Controllers
{
    public class RepertoarController : Controller
    {
        // GET: Repertoar
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                Debug.WriteLine(Session["UserID"].ToString());
            }
            else
            {
                Debug.WriteLine("null");
            }
            List<PredstavaNaRepertoaru> predstave = new PredstavaNaRepertoaruDAO().listaPredstava();

            

            return View(predstave);
        }
    }
}