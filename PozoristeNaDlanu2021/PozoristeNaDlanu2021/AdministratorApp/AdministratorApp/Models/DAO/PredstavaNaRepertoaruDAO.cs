using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class PredstavaNaRepertoaruDAO:Connection, IPredstavaNaRepertoaruDAO
    {
        public PredstavaNaRepertoaruDAO() : base() { }

        public  List<PredstavaNaRepertoaru> predstaveNaRepertoaru()
        {
            List<PredstavaNaRepertoaru> predstave = new List<PredstavaNaRepertoaru>();

            try
            {
                cmd = new MySqlCommand("select idPredstavaNaRepertoaru,naziv,idPredstave,vrijeme,cijena from predstavanarepertoaru natural join predstava where vrijeme>now() order by vrijeme", con);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string naziv = (string)reader[1];
                    int idPredstaveKojojOdgovara = (int)reader[2];
                    DateTime vrijemeOdrzavanja = (DateTime)reader[3];
                    float cijena = (float)reader[4];
                    PredstavaNaRepertoaru predstava = new PredstavaNaRepertoaru { id = id, naziv = naziv, cijena = cijena, idPredstave = idPredstaveKojojOdgovara, vrijemeOdrzavanja = vrijemeOdrzavanja };
                    predstave.Add(predstava);
                }
                con.Close();
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return predstave;
            }
            return predstave;
        }

        public int getIdSaleByNAme(String naziv)
        {
            try
            {
                cmd = new MySqlCommand("select * from sala where (imeSale=@naziv)", con);
                cmd.Parameters.AddWithValue("@naziv", naziv);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                int idSale = -1;
                while (reader.Read())
                {
                    idSale = (int)reader[0];
                }
                con.Close();
                return idSale;
            }
            catch (Exception)
            {
                con.Close();
                return -1;
            }
        }

        public bool dodajPredstavuNaRepertoar(PredstavaNaRepertoaru predstava)
        {
            try
            {
                cmd = new MySqlCommand("insert into predstavanarepertoaru(idPredstave, vrijeme, idSala,cijena) values (@id, @vrijeme, @idSale,@cijena)", con);
                cmd.Parameters.AddWithValue("@id", predstava.idPredstave);
                cmd.Parameters.AddWithValue("@vrijeme", predstava.vrijemeOdrzavanja);
                cmd.Parameters.AddWithValue("@idSale", predstava.idSale);
                cmd.Parameters.AddWithValue("@cijena", predstava.cijena);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
                con.Close();


                cmd = new MySqlCommand("select * from predstava where naziv=@NazivP", con);

                cmd.Parameters.AddWithValue("@NazivP", predstava.naziv);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idPredstave = (int)reader[0];
                    predstava.id = idPredstave;
                }
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

        public void izbrisiPredstavuNaRepertoaru(int id)
        {
            Debug.WriteLine("brisanje");
            try
            {
                cmd = new MySqlCommand("delete from predstavanarepertoaru where (idPredstavaNaRepertoaru=@id)", con);
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

        public List<string> getEmailRezervacijaZaPredstavu(int id)
        {
            List<string> mailovi = new List<string>();
            try
            {
                cmd = new MySqlCommand("select distinct email from rezervacija left join  korisnik on rezervacija.idKorisnik=korisnik.idKorisnik where idPredstavaNaRepertoaru=@idPR", con);
                cmd.Parameters.AddWithValue("@idPR", id);
                con.Open();
                MySqlDataReader reader4 = cmd.ExecuteReader();
                while (reader4.Read())
                {
                    string mail = (string)reader4[0];
                    mailovi.Add(mail);
                }

                con.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
            }
            return mailovi;
        }

        public  PredstavaNaRepertoaru dohvatiPredstavu(int idP)
        {
            PredstavaNaRepertoaru predstava = new PredstavaNaRepertoaru();

            try
            {
                cmd = new MySqlCommand("select idPredstavaNaRepertoaru,naziv,idPredstave,vrijeme from predstavanarepertoaru natural join predstava where idPredstavaNaRepertoaru=@id", con);
                cmd.Parameters.AddWithValue("@id", idP);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string naziv = (string)reader[1];
                    int idPredstaveKojojOdgovara = (int)reader[2];
                    DateTime vrijemeOdrzavanja = (DateTime)reader[3];
                    predstava = new PredstavaNaRepertoaru { id = id, naziv = naziv, idPredstave = idPredstaveKojojOdgovara, vrijemeOdrzavanja = vrijemeOdrzavanja };

                }
                con.Close();
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
            return predstava;
        }

        public  bool dodajSalu(Sala sala)
        {
            try
            {
                cmd = new MySqlCommand("insert into sala(imeSale, brojKolona, brojVrsta) values (@imeSale, @brojKolona, @brojVrsta)", con);
                cmd.Parameters.AddWithValue("@imeSale", sala.imeSale);
                cmd.Parameters.AddWithValue("@brojKolona", sala.brojKolona);
                cmd.Parameters.AddWithValue("@brojVrsta", sala.brojVrsta);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
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

        public bool dodajSjedistaSale(Sala sala)
        {
            try
            {
                foreach (var sjediste in sala.sjedista)
                {
                    cmd = new MySqlCommand("insert into sjediste(vrsta, kolona, idSala, status) values (@vrsta, @kolona, @idSala, @status)", con);
                    cmd.Parameters.AddWithValue("@vrsta", sjediste.vrsta);
                    cmd.Parameters.AddWithValue("@kolona", sjediste.kolona);
                    cmd.Parameters.AddWithValue("@idSala", sala.idSale);
                    cmd.Parameters.AddWithValue("@status", sjediste.status);

                    Debug.WriteLine("prije otvaranja");
                    con.Open();
                    Debug.WriteLine("posle");
                    cmd.ExecuteNonQuery();
                    Debug.WriteLine("prije izvrsavanja");
                    con.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
                return false;
            }
        }
        public Sala dohvatiSalu(int? id)
        {
            Sala sala = new Sala();
            try
            {
                cmd = new MySqlCommand("select * from sala where idSala=@idSale", con);
                cmd.Parameters.AddWithValue("@idSale", id);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idSala = (int)reader[0];
                    string imeSale = (string)reader[1];
                    int brojKolona = (int)reader[2];
                    int brojVrsta = (int)reader[3];
                    sala = new Sala { idSale = idSala, imeSale = imeSale, brojKolona = brojKolona, brojVrsta = brojVrsta, sjedista = new List<Sjediste>() };
                }
                con.Close();


                cmd = new MySqlCommand("select * from sjediste where idSala=@idSala", con);
                cmd.Parameters.AddWithValue("@idSala", id);
                con.Open();
                MySqlDataReader reader1 = cmd.ExecuteReader();
                while (reader1.Read())
                {
                    int idSjediste = (int)reader1[0];
                    string vrsta = (string)reader1[1];
                    int kolona = (int)reader1[2];
                    int idSale = (int)reader1[3];
                    string status = (string)reader1[4];
                    Sjediste sjediste = new Sjediste { idSale = idSale, idSjediste = idSjediste, kolona = kolona, status = status, vrsta = vrsta };
                    sala.sjedista.Add(sjediste);
                }
                con.Close();
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
            return sala;
        }

        public bool azurirajSjedista(Sala sala)
        {
            try
            {
                foreach (var sjediste in sala.sjedista)
                {
                    //UPDATE `pozoriste`.`sjediste` SET `status` = 'prolaz' WHERE (`idSjediste` = '341');

                    cmd = new MySqlCommand("update sjediste set status=@Status where idSala=@idSale and kolona=@Kolona and vrsta=@Vrsta", con);
                    cmd.Parameters.AddWithValue("@Status", sjediste.status);
                    cmd.Parameters.AddWithValue("@idSale", sala.idSale);
                    cmd.Parameters.AddWithValue("@Kolona", sjediste.kolona);
                    cmd.Parameters.AddWithValue("@Vrsta", sjediste.vrsta);
                    Debug.WriteLine("prije otvaranja");
                    con.Open();
                    Debug.WriteLine("posle");
                    cmd.ExecuteNonQuery();
                    Debug.WriteLine("prije izvrsavanja");
                    con.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                con.Close();
                return false;
            }
        }
    }
}