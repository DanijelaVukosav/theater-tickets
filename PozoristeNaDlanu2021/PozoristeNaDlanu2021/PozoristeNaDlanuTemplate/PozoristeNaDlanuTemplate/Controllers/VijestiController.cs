using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PozoristeNaDlanuTemplate.Controllers
{
    public class VijestiController : Controller
    {
        // GET: Vijesti
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListaVijesti()
        {
            return View();
        }
    }
}