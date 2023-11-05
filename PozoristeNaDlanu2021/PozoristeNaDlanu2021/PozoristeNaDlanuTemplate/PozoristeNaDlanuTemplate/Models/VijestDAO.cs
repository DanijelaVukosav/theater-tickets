using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class VijestDAO : DAO
    {

        public VijestDAO() : base()
        {
        }

        public Vijest GetVijestById(int? idVijesti)
        {
            Vijest vijest = null;
            try
            {
                cmd = new MySqlCommand("select * from vijest where idVijest=@ID", con);
                cmd.Parameters.AddWithValue("@ID", idVijesti);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    vijest = new Vijest { idVijesti = (int)reader[0], naslov = (string)reader[1], datum = (DateTime)reader[2], opis = (string)reader[3], slikeVijesti = new List<string>(), komentariVijesti = new List<(string username, string sadrzaj, DateTime datum)>() };
                }
                con.Close();
                con.Open();
                cmd = new MySqlCommand("select * from slikevijesti where idVijest=@IDv", con);
                cmd.Parameters.AddWithValue("@IDv", idVijesti);
                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    vijest.slikeVijesti.Add((string)reader2[2]);
                }
                con.Close();
                Debug.WriteLine("Procita slikeee");
                con.Open();
                cmd = new MySqlCommand("select username,sadrzaj,vrijeme from vijestkomentar natural join korisnik where idVijest = @IDvijest order by vrijeme", con);
                cmd.Parameters.AddWithValue("@IDvijest", idVijesti);
                MySqlDataReader reader3 = cmd.ExecuteReader();
                Debug.WriteLine("Izvrsi");
                while (reader3.Read())
                {

                    vijest.komentariVijesti.Add(((string)reader3[0], (string)reader3[1], (DateTime)reader3[2]));
                }
                con.Close();


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
            return vijest;
        }

        public List<Vijest> listaVijesti()
        {
            List<Vijest> vijesti = new List<Vijest>();
            List<(Vijest vijest, int idPredstave)> pomocnaLista = new List<(Vijest vijest, int idPredstave)>();


            try
            {
                cmd = new MySqlCommand("select * from vijest order by idVijest desc", con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idVijest = (int)reader[0];
                    string naslov = (string)reader[1];
                    DateTime datum = (DateTime)reader[2];
                    string opis = (string)reader[3];

                    Vijest vijest = new Vijest { idVijesti = idVijest, naslov = naslov, datum = datum, opis = opis, slikeVijesti = new List<string>() };

                    vijesti.Add(vijest);
                }
                con.Close();

                foreach (var vijest in vijesti)
                {
                    con.Open();
                    cmd = new MySqlCommand("select * from slikevijesti where idVijest=@ID", con);
                    cmd.Parameters.AddWithValue("@ID", vijest.idVijesti);

                    MySqlDataReader reader2 = cmd.ExecuteReader();
                    while (reader2.Read())
                    {
                        vijest.slikeVijesti.Add((string)reader2[2]);
                    }
                    con.Close();

                }

                return vijesti;
            }
            catch (Exception)
            {
                Debug.WriteLine("Greska");
                return new List<Vijest>();
            }

        }

        public static void detaljiVijesti(Vijest vijest, string komentar, string korisnik)
        {
            //if (korisnik == null)
            //return RedirectToAction("Index", "Login"); ova metoda kod mene ne radi

            try
            {
                cmd = new MySqlCommand("insert into vijestkomentar(sadrzaj,vrijeme,idVijest,idKorisnik) values(@sadrzaj,now(),@idvijest,@idkorisnik)", con);
                cmd.Parameters.AddWithValue("@sadrzaj", komentar);
                cmd.Parameters.AddWithValue("@idvijest", vijest.idVijesti);
                cmd.Parameters.AddWithValue("@idkorisnik", korisnik);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                Debug.WriteLine(System.DateTime.Now);
                if (vijest.komentariVijesti == null)
                {

                    Debug.WriteLine(vijest.idVijesti + "    id");
                    Debug.WriteLine(vijest.naslov + "    naslov");
                    Debug.WriteLine(vijest.slikeVijesti + "    slike");
                    vijest.komentariVijesti = new List<(string username, string sadrzaj, DateTime datum)>();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
        }



    }


}
