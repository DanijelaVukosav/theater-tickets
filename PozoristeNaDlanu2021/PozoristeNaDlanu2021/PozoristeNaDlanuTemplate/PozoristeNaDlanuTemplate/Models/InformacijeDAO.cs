using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class InformacijeDAO :DAO
    {

        public InformacijeDAO() : base()
        {

        }

        public Informacije listaInformacija()
        {
            
            Informacije info = new Informacije { informacije = new List<(string naslov, string sadrzaj)>(), glumci = new List<PozoristeNaDlanuTemplate.Models.Glumac>() };
            try
            {
                cmd = new MySqlCommand("select * from informacije", con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    info.informacije.Add(((string)reader[1], (string)reader[2]));
                }
                info.glumci = new GlumacDAO().listaGlumaca();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                //return View();S
            }
            return info;

        }

    }
}