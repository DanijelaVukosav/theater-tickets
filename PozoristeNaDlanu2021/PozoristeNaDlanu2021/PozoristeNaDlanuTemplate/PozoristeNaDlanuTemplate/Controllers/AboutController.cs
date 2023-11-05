using MySql.Data.MySqlClient;
using PozoristeNaDlanuTemplate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PozoristeNaDlanuTemplate.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            return View(new InformacijeDAO().listaInformacija());
        }

        public ActionResult GlumacDetails(int? id)
        {
            if(id==null)
            {
                throw new HttpException(404, "Doslo je do greske!");
            }
            return View(new GlumacDAO().getGlumac(id));
        }
    }
}