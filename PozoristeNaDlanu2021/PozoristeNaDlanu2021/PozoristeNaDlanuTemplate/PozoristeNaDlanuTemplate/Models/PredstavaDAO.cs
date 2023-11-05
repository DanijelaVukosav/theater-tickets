using MySql.Data.MySqlClient;
using PozoristeNaDlanuTemplate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class PredstavaDAO : DAO
    {

        public PredstavaDAO() : base()
        {

        }

        public  Predstava GetPredstavaById(int? idp)
        {
            Predstava predstava = null;
            try
            {
                cmd = new MySqlCommand("select * from predstava where idPredstave=@IDpredstave", con);
                cmd.Parameters.AddWithValue("@IDpredstave", idp);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idPredstave = (int)reader[0];
                    string naziv = (string)reader[1];
                    DateTime datumPremijere = (DateTime)reader[2];
                    string rezija = (string)reader[3];
                    string scenografija = (string)reader[4];
                    string opis = (string)reader[5];
                    string tekstopisac = (string)reader[6];
                    predstava = new Predstava { id = idPredstave, naziv = naziv, datumPremijere = datumPremijere, rezija = rezija, scenografija = scenografija, opis = opis, tekstopisac = tekstopisac, komentari = new List<(int id, string username, string sadrzaj, DateTime datum)>(), putanje = new List<string>(), idIputanje = new Dictionary<int, string>() };
                }
                con.Close();


                cmd = new MySqlCommand("select * from slikepredstave where idPredstave=@IDpredstave", con);
                cmd.Parameters.AddWithValue("@IDpredstave", idp);
                con.Open();
                MySqlDataReader reader1 = cmd.ExecuteReader();
                int indikator = 0;
                while (reader1.Read())
                {
                    int idSlike = (int)reader1[0];
                    string putanja = (string)reader1[1];
                    if (indikator == 0)
                    {
                        predstava.putanja = putanja;
                        indikator = 1;
                    }
                    predstava.idIputanje.Add(idSlike, putanja);
                    predstava.putanje.Add(putanja);
                }
                con.Close();


                con.Open();
                cmd = new MySqlCommand("select idKomentara,username,sadrzaj,vrijeme from komentar natural join korisnik where idPredstave = @ID order by vrijeme", con);
                cmd.Parameters.AddWithValue("@ID", predstava.id);
                MySqlDataReader reader3 = cmd.ExecuteReader();
                while (reader3.Read())
                {

                    predstava.komentari.Add(((int)reader3[0], (string)reader3[1], (string)reader3[2], (DateTime)reader3[3]));
                }
                con.Close();
                DohvatiGlumcePredstave(predstava);
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
            return predstava;
        }
        private Predstava DohvatiGlumcePredstave(Predstava predstava)
        {
            predstava.glumci = new List<Glumac>();
            cmd = new MySqlCommand("select * from glumciupredstavi natural join glumac where idPredstave=@IDpredstave", con);
            cmd.Parameters.AddWithValue("@IDpredstave", predstava.id);
            con.Open();
            MySqlDataReader reader1 = cmd.ExecuteReader();

            while (reader1.Read())
            {
                int idG= (int)reader1[0];
                string ime= (string)reader1[2];
                string prezime= (string)reader1[3];
                string biografija= (string)reader1[4];
                Glumac glumac = new Glumac { id = idG, ime = ime, prezime = prezime, biografija = biografija };
                predstava.glumci.Add(glumac);
            }
            con.Close();
            return predstava;
        }

        public  List<Predstava> predstaveList()
        {
            List<Predstava> predstave = new List<Predstava>();
            try
            {
                cmd = new MySqlCommand("select * from predstava", con);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string naziv = (string)reader[1];
                    DateTime datumPremijere = (DateTime)reader[2];
                    string rezija = (string)reader[3];
                    string scenografija = (string)reader[4];
                    string opis = (string)reader[5];
                    string tekstopisac = (string)reader[6];
                    Predstava predstava = new Predstava { id = id, naziv = naziv, datumPremijere = datumPremijere, rezija = rezija, scenografija = scenografija, opis = opis, tekstopisac = tekstopisac, putanje = new List<string>(), idIputanje = new Dictionary<int, string>() };
                    predstave.Add(predstava);
                }

                con.Close();

                foreach (var p in predstave)
                {
                    cmd = new MySqlCommand("select * from slikepredstave where idPredstave=@IDpredstave", con);
                    cmd.Parameters.AddWithValue("@IDpredstave", p.id);
                    con.Open();
                    MySqlDataReader reader1 = cmd.ExecuteReader();
                    int indikator = 0;
                    int indikatorNaslovne = 0;
                    while (reader1.Read())
                    {
                        string putanja = (string)reader1[1];
                        int idSlike = (int)reader1[0];
                        if (indikatorNaslovne == 0)
                        {
                            p.putanja = putanja;
                            indikatorNaslovne = 1;
                        }
                        p.putanje.Add(putanja);
                        p.idIputanje.Add(idSlike, putanja);
                        indikator = 1;
                    }
                    if (indikator == 0)
                        p.putanja = "/images/logo.jfif";
                    con.Close();
                }
                return predstave;
            }
            catch (Exception)
            {
                return new List<Predstava>();
            }
        }


        public void dodajKomentar(Predstava predstava, string komentar, string korisnik)
        {
            
            try
            {
                cmd = new MySqlCommand("insert into komentar(sadrzaj,vrijeme,idPredstave,idKorisnik) values(@sadrzaj,now(),@idpredstave,@idkorisnik)", con);
                cmd.Parameters.AddWithValue("@sadrzaj", komentar);
                cmd.Parameters.AddWithValue("@idpredstave", predstava.id);
                cmd.Parameters.AddWithValue("@idkorisnik", korisnik);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();



            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
        }
    }
}