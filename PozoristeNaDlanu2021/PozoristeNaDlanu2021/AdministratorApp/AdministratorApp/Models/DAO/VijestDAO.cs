using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class VijestDAO:Connection, IVijestDAO
    {
        public VijestDAO() : base() { }

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
            catch (Exception ex)
            {
                Debug.WriteLine("Greska");
                Debug.WriteLine(ex.StackTrace);
                return new List<Vijest>();
            }

        }

        public Vijest GetVijestById(int? idVijesti)
        {
            Vijest vijest = null;
            try
            {
                Debug.WriteLine("USAOOOOOO U GetVijestBy************************************************************");
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
            }
            return vijest;
        }

        public void UkloniSliku(int id, string putanja)
        {
            try
            {
                con.Open();
                cmd = new MySqlCommand("delete  from slikevijesti where idVijest=@ID and putanja=@putanja", con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@putanja", putanja);

                cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
        }

        public void addComment(Vijest vijest, String komentar, String UserID)
        {
            try
            {
                cmd = new MySqlCommand("insert into vijestkomentar(sadrzaj,vrijeme,idVijest,idKorisnik) values(@sadrzaj,now(),@idvijest,@idkorisnik)", con);
                cmd.Parameters.AddWithValue("@sadrzaj", komentar);
                cmd.Parameters.AddWithValue("@idvijest", vijest.idVijesti);
                cmd.Parameters.AddWithValue("@idkorisnik", UserID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //Debug.WriteLine(Session["UserName"].ToString());
                Debug.WriteLine(System.DateTime.Now);
                if (vijest.komentariVijesti == null)
                {
                    Debug.WriteLine("JEste null");

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

        public int dodajVijest(Vijest vijest)
        {
            try
            {

                cmd = new MySqlCommand("insert into vijest (naslov, datum, opis) values(@naslov,@datum,@opis)", con);

                cmd.Parameters.AddWithValue("@naslov", vijest.naslov);
                cmd.Parameters.AddWithValue("@datum", vijest.datum);
                cmd.Parameters.AddWithValue("@opis", vijest.opis);
                con.Open();
                cmd.ExecuteNonQuery();
                int idVijesti = (int)cmd.LastInsertedId;
                con.Close();

                return idVijesti;
                /* 
                 Debug.WriteLine(vijest.slikeVijesti.Count() + " slika");
                 for (int i = 0; i < vijest.slikeVijesti.Count(); i++) 
                 {
                     con.Open();
                     Debug.WriteLine(vijest.slikeVijesti.ElementAt(i) + "   U            BAZI");
                     cmd = new MySqlCommand("insert into slikevijesti (idVijest, putanja) values (@idVijest,@putanja)", con);
                     cmd.Parameters.AddWithValue("@idVijest", idVijesti);
                     cmd.Parameters.AddWithValue("@putanja", vijest.slikeVijesti.ElementAt(i));
                     cmd.ExecuteNonQuery();
                     con.Close();
                 }
                 */

            }
            catch (Exception ex)
            {
                con.Close();
                return -1;
            }
        }

        public void detaljiVijesti(Vijest vijest, string komentar, string korisnik)
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

        public void sacuvajIzmjene(Vijest vijest)
        {

            try
            {
                cmd = new MySqlCommand("update vijest set naslov=@naslov,datum=now(),opis=@opis where idVijest=@id", con);
                //cmd.Parameters.AddWithValue("@id", vijest.idVijesti);
                cmd.Parameters.AddWithValue("@naslov", vijest.naslov);
                //cmd.Parameters.AddWithValue("@datum", DateTime.Now);
                cmd.Parameters.AddWithValue("@opis", vijest.opis);
                cmd.Parameters.AddWithValue("@id", vijest.idVijesti);
                con.Open();
                int rez = cmd.ExecuteNonQuery();
                con.Close();
                Debug.WriteLine("REZZZZ " + rez);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GRESKAAAAAA");
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine(ex.Message);

                con.Close();
            }
        }

        public void izbrisiVijest(Vijest vijest)
        {
            try
            {
                con.Open();
                //for (int i = 0; i < vijest.slikeVijesti.Count(); i++)
                {
                    cmd = new MySqlCommand("delete from slikevijesti where idVijest=@idVijest", con);
                    cmd.Parameters.AddWithValue("@idVijest", vijest.idVijesti);
                    cmd.ExecuteNonQuery();
                }
                con.Close();

                con.Open();
                cmd = new MySqlCommand("delete from vijestkomentar where idVijest=@idVijest", con);
                cmd.Parameters.AddWithValue("@idVijest", vijest.idVijesti);
                cmd.ExecuteNonQuery();
                con.Close();

                cmd = new MySqlCommand("delete from vijest where idVijest=@id", con);
                cmd.Parameters.AddWithValue("@id", vijest.idVijesti);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {

            }
        }

        public List<string> GetSlikeVijesti(int id)
        {
            List<string> slike = new List<string>();
            try
            {
                con.Open();
                cmd = new MySqlCommand("select * from slikevijesti where idVijest=@ID", con);
                cmd.Parameters.AddWithValue("@ID", id);

                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    slike.Add((string)reader2[2]);

                }
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
            return slike;
        }

        public int AddSlikaVijest(SlikaVijesti slika)
        {
            try
            {
                cmd = new MySqlCommand("insert into slikevijesti(idVijest,putanja) values(@id,@putanja)", con);
                cmd.Parameters.AddWithValue("@id", slika.idVijesti);
                cmd.Parameters.AddWithValue("@putanja", slika.putanja);
                con.Open();
                cmd.ExecuteNonQuery();
                long id = cmd.LastInsertedId;
                con.Close();
                return (int)id;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return -1;
            }
        }
    }
}