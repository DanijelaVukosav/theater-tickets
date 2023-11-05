using Biletarnik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biletarnik.Controllers
{
    public class PomController : Controller
    {
        // GET: Pom
        public ActionResult Index()
        {
            return View(new Predstava() { komentari=new List<(string username, string sadrzaj, DateTime datum)>()});
        }
    }
}