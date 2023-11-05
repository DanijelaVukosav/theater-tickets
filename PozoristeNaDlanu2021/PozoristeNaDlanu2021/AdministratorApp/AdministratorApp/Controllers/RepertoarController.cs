using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AdministratorApp.Models;
using AdministratorApp.Models.DAO;
namespace AdministratorApp.Controllers
{
    public class RepertoarController : Controller
    {
        public ActionResult Index()
        {
            List<PredstavaNaRepertoaru> predstave = new DAOFactoryImpl().CreatePredstavaNaRepertoaruDAO().predstaveNaRepertoaru();
            return View(predstave);
        }

        [HttpGet]
        public ActionResult AddNew()
        {
            return View(new PredstavaNaRepertoaru());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(PredstavaNaRepertoaru predstava)
        {
            int idPredstave = new DAOFactoryImpl().CreatePredstavaDAO().getIdByName(predstava.naziv);
            if (idPredstave == -1)
                return RedirectToAction("AddNew", "Repertoar");
            int idSale = new DAOFactoryImpl().CreatePredstavaNaRepertoaruDAO().getIdSaleByNAme(predstava.nazivSale);
            if (idSale == -1)
                return RedirectToAction("AddNew", "Repertoar");
            predstava.idSale = idSale;
            predstava.idPredstave = idPredstave;
            new DAOFactoryImpl().CreatePredstavaNaRepertoaruDAO().dodajPredstavuNaRepertoar(predstava);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult AddSalu()
        {
            return View(new Sala());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSalu(Sala sala)
        {
            sala.sjedista = new List<Sjediste>();
            char ch = 'A';
            for (int i = 1; i <= sala.brojVrsta; i++)
            {
                for (int j = 1; j <= sala.brojKolona; j++)
                {
                    Sjediste sjediste = new Sjediste { idSale = sala.idSale, kolona = j, vrsta = ch.ToString(), status = "slobodna" };
                    sala.sjedista.Add(sjediste);
                }
                Debug.WriteLine(ch + " ");
                ch++;
            }
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            bool rezultat = dfi.CreatePredstavaNaRepertoaruDAO().dodajSalu(sala);
            int idSale = dfi.CreatePredstavaNaRepertoaruDAO().getIdSaleByNAme(sala.imeSale);
            sala.idSale = idSale;
            rezultat &= dfi.CreatePredstavaNaRepertoaruDAO().dodajSjedistaSale(sala);
            if (rezultat == true)
                return RedirectToAction("AddSjedista", "Repertoar", new { id = sala.idSale });
            return View();
        }

        [HttpGet]
        public ActionResult AddSjedista(int? id)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Sala sala = dfi.CreatePredstavaNaRepertoaruDAO().dohvatiSalu(id);
            return View(sala);
        }

        [HttpPost]
        public ActionResult AddSjedista(string Seats, int? Number, int? idSale, Boolean kupovina)
        {
            Debug.WriteLine("dodje");
            Debug.WriteLine("id sale " + idSale);
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            Sala sala = dfi.CreatePredstavaNaRepertoaruDAO().dohvatiSalu(idSale);
            String[] split = Seats.Split(",".ToCharArray());
            foreach (var s in sala.sjedista)
            {
                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i].Equals(s.vrsta + "" + s.kolona) == true)
                    {
                        s.status = "prolaz";
                    }
                }
            }

            bool rezultat = dfi.CreatePredstavaNaRepertoaruDAO().azurirajSjedista(sala);
            if (rezultat == true)
                return RedirectToAction("AddNew", "Repertoar");
            else
                return View(sala);
        }
        public ActionResult OtkaziPredstavu(int id)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            PosaljiMailZaOtkazPredstave(id);
            dfi.CreatePredstavaNaRepertoaruDAO().izbrisiPredstavuNaRepertoaru(id);
            PosaljiMailZaOtkazPredstave(id);
            return RedirectToAction("Index");
        }

        private void PosaljiMailZaOtkazPredstave(int id)
        {
            DAOFactoryImpl dfi = new DAOFactoryImpl();
            List<string> mailovi = dfi.CreatePredstavaNaRepertoaruDAO().getEmailRezervacijaZaPredstavu(id);
            PredstavaNaRepertoaru predstava = dfi.CreatePredstavaNaRepertoaruDAO().dohvatiPredstavu(id);
            foreach (var address in mailovi)
            {
                try
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("mm2023012@gmail.com");
                        mail.To.Add(address);
                        mail.Subject = "Otkazana predstava " + predstava.naziv;
                        mail.Body = "<h3>Predstava " + predstava.naziv + " je okazana, u terminu " + predstava.vrijemeOdrzavanja + "</h3>";
                        mail.IsBodyHtml = true;
                        // mail.Attachments.Add(new Attachment("C:\\file.zip"));

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("mm2023012@gmail.com", "marija2020");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("GRESKA   "+e.ToString());
                }
            }
        }
    }
}