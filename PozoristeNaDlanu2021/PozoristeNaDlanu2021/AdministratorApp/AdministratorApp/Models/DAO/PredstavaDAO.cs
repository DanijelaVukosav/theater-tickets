using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class PredstavaDAO:Connection, IPredstavaDAO
    {
        public PredstavaDAO() : base() { }

        public List<Predstava> predstaveList()
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
                        p.putanja = "/images/default.jpg";
                    con.Close();
                }
                return predstave;
            }
            catch (Exception)
            {
                con.Close();
                return new List<Predstava>();
            }
        }

        public List<Glumac> getGlumciPredstave(object id)
        {
            List<Glumac> glumci = new List<Glumac>();
            try
            {
                con.Open();
                cmd = new MySqlCommand("select * from glumciupredstavi natural join glumac where idPredstave=@ID", con);
                cmd.Parameters.AddWithValue("@ID", id);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idGlumac = (int)reader[0];
                    string ime = (string)reader[2];

                    string prezime = (string)reader[3];
                    string biografija = (string)reader[4];
                    Glumac glumac = new Glumac { id = idGlumac, ime = ime, prezime = prezime, biografija = biografija };
                    glumci.Add(glumac);

                }
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa u igranim");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
            return glumci;
        }

        public List<Glumac> GlumciKojiNeIgrajuUPredstavi(int idPredstave)
        {
            List<Glumac> glumci = new List<Glumac>();
            try
            {
                con.Open();
                cmd = new MySqlCommand("SELECT distinct * FROM glumac WHERE idGlumac NOT IN(select distinct idGlumac from glumciupredstavi where IdPredstave = @IDPredstave)", con);
                cmd.Parameters.AddWithValue("@IDPredstave", idPredstave);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string ime = (string)reader[1];
                    string prezime = (string)reader[2];
                    string biografija = (string)reader[3];
                    Glumac glumac = new Glumac { id = id, ime = ime, prezime = prezime, biografija = biografija };
                    glumci.Add(glumac);

                }
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa u neigranim");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
            return glumci;
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
                predstava = DohvatiGlumcePredstave(predstava);
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
            return predstava;
        }
        public Predstava DohvatiGlumcePredstave(Predstava predstava)
        {
            predstava.glumci = new List<Glumac>();
            cmd = new MySqlCommand("select * from glumciupredstavi natural join glumac where idPredstave=@IDpredstave", con);
            cmd.Parameters.AddWithValue("@IDpredstave", predstava.id);
            con.Open();
            MySqlDataReader reader1 = cmd.ExecuteReader();

            while (reader1.Read())
            {
                int idG = (int)reader1[0];
                string ime = (string)reader1[2];
                string prezime = (string)reader1[3];
                string biografija = (string)reader1[4];
                Glumac glumac = new Glumac { id = idG, ime = ime, prezime = prezime, biografija = biografija };
                predstava.glumci.Add(glumac);
            }
            con.Close();
            return predstava;
        }



        public Boolean addPredstavu(Predstava predstava)
        {
            try
            {
                cmd = new MySqlCommand("insert into predstava(naziv, datumPremijere,rezija,scenografija,opis,tekstopisac) values (@NazivP, @DatumP, @RezijaP, @ScenografijaP, @OpisP, @TekstopisacP)", con);

                cmd.Parameters.AddWithValue("@NazivP", predstava.naziv);
                cmd.Parameters.AddWithValue("@DatumP", predstava.datumPremijere);
                cmd.Parameters.AddWithValue("@RezijaP", predstava.rezija);
                cmd.Parameters.AddWithValue("@ScenografijaP", predstava.scenografija);
                cmd.Parameters.AddWithValue("@OpisP", predstava.opis);
                cmd.Parameters.AddWithValue("@TekstopisacP", predstava.tekstopisac);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                predstava.id = (int)cmd.LastInsertedId;
                Debug.WriteLine("prije izvrsavanja " + predstava.id);
                con.Close();

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
                return false;
            }
        }

        public void dodajSlikuPredstave(int idPredstave, String putanja)
        {
            cmd = new MySqlCommand("insert into slikepredstave(idPredstave, putanja) values (@idP,@PutanjaS)", con);
            cmd.Parameters.AddWithValue("@idP", idPredstave);
            cmd.Parameters.AddWithValue("@PutanjaS", putanja);

            Debug.WriteLine("prije otvaranja");
            con.Open();
            Debug.WriteLine("posle");
            cmd.ExecuteNonQuery();
            Debug.WriteLine("prije izvrsavanja");
            con.Close();
        }

        public void dodajKomentar(String komentar, int idPredstave, String idKorisnika)
        {
            try
            {
                cmd = new MySqlCommand("insert into komentar(sadrzaj,vrijeme,idPredstave,idKorisnik) values(@sadrzaj,now(),@idpredstave,@idkorisnik)", con);
                cmd.Parameters.AddWithValue("@sadrzaj", komentar);
                cmd.Parameters.AddWithValue("@idpredstave", idPredstave);
                cmd.Parameters.AddWithValue("@idkorisnik", idKorisnika);
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

        public void izmijeniPredstavu(Predstava predstava)
        {
            //UPDATE `pozoriste`.`predstava` SET `naziv` = 'pr857' WHERE(`idPredstave` = '1');
            Debug.WriteLine("izmjena");
            try
            {
                cmd = new MySqlCommand("update predstava set naziv=@NazivP, datumPremijere=@DatumP,rezija=@RezijaP,scenografija=@ScenografijaP,opis=@OpisP,tekstopisac=@TekstopisacP where (idPredstave=@id)", con);
                cmd.Parameters.AddWithValue("@NazivP", predstava.naziv);
                cmd.Parameters.AddWithValue("@DatumP", predstava.datumPremijere);
                cmd.Parameters.AddWithValue("@RezijaP", predstava.rezija);
                cmd.Parameters.AddWithValue("@ScenografijaP", predstava.scenografija);
                cmd.Parameters.AddWithValue("@OpisP", predstava.opis);
                cmd.Parameters.AddWithValue("@TekstopisacP", predstava.tekstopisac);
                cmd.Parameters.AddWithValue("@id", predstava.id);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
                con.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
            }
        }

        public void obrisiPredstavu(int? id)
        {
            Debug.WriteLine("brisanje");
            try
            {
                cmd = new MySqlCommand("delete from predstava where (idPredstave=@id)", con);
                cmd.Parameters.AddWithValue("@id", id);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
                con.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
            }
        }

        public int obrisiSliku(int? id)
        {
            Debug.WriteLine("brisanje slike");
            try
            {
                cmd = new MySqlCommand("select * from slikepredstave where (idSlike=@id)", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                int idPredstave = -1;
                while (reader.Read())
                {
                    idPredstave = (int)reader[2];
                }
                con.Close();

                cmd = new MySqlCommand("delete from slikepredstave where (idSlike=@id)", con);
                cmd.Parameters.AddWithValue("@id", id);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
                con.Close();
                return idPredstave;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
                return -1;
            }
        }

        public int obrisiKomentar(int? id)
        {
            Debug.WriteLine("brisanje komentara");
            try
            {
                cmd = new MySqlCommand("select * from komentar where (idKomentara=@id)", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                int idPredstave = -1;
                while (reader.Read())
                {
                    idPredstave = (int)reader[2];
                }
                con.Close();


                cmd = new MySqlCommand("delete from komentar where (idKomentara=@id)", con);
                cmd.Parameters.AddWithValue("@id", id);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
                con.Close();
                return idPredstave;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
                return -1;
            }
        }

        public int getIdByName(String naziv)
        {
            try
            {
                cmd = new MySqlCommand("select * from predstava where (naziv=@naziv)", con);
                cmd.Parameters.AddWithValue("@naziv", naziv);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                int idPredstave = -1;
                while (reader.Read())
                {
                    idPredstave = (int)reader[0];
                }
                con.Close();
                return idPredstave;
            }
            catch (Exception)
            {
                con.Close();
                return -1;
            }
        }
    }
}