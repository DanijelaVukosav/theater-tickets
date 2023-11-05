using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class RezervacijaDAO : DAO
    {

        public RezervacijaDAO() : base()
        {

        }

        public List<Rezervacija> rezervacijePrijavljenogKorisnika(string idKorisnika)
        {

            List<Rezervacija> rezervacije = new List<Rezervacija>();
            try
            {
                int idKorisnik = Int32.Parse(idKorisnika);
                cmd = new MySqlCommand("select * from rezervacija natural join predstavanarepertoaru where idKorisnik=@id", con);
                cmd.Parameters.AddWithValue("@id", idKorisnik);
                con.Open();
                Debug.WriteLine("prije komanda " + idKorisnik);
                MySqlDataReader reader = cmd.ExecuteReader();
                Debug.WriteLine("posle komanda");

                while (reader.Read())
                {

                    Debug.WriteLine("bool je u pitanju");
                    string placeno = (string)reader[5];
                    Debug.WriteLine("nova linija");
                    Debug.WriteLine(placeno);
                    Debug.WriteLine("bool je u pitanju");
                    string pl = "NE";
                    if (placeno.Equals("1"))
                        pl = "DA";

                    Rezervacija r = new Rezervacija { idRezervacija = (int)reader[1], brojKarata = (int)reader[2], idPredstaveNaRepertoaru = (int)reader[0], idKorisnik = (int)reader[3], sjedistastring = (string)reader[4], placeno = pl };
                    rezervacije.Add(r);
                }

                con.Close();
                foreach (var r in rezervacije)
                {
                    con.Open();
                    cmd = new MySqlCommand("select naziv from predstavanarepertoaru natural join predstava where idPredstavaNaRepertoaru=@ID", con);
                    cmd.Parameters.AddWithValue("@ID", r.idPredstaveNaRepertoaru);

                    MySqlDataReader reader2 = cmd.ExecuteReader();
                    while (reader2.Read())
                    {
                        r.naslovPredstave = (string)reader2[0];

                    }
                    con.Close();
                }


            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa iz rezervacija");
                con.Close();
                return null;
            }

            return rezervacije;
        }

        public Sala vratiSalu(int? id)
        {
            Sala sala = new Sala();

            List<Sjediste> sjedista = new List<Sjediste>();
            Debug.WriteLine(id + " kao id predstave na repertoaru");

            try
            {
                int idSale = 0;
                //predstava na repertoaru
                cmd = new MySqlCommand("select * from predstavanarepertoaru where idPredstavaNaRepertoaru=@idSale", con);
                cmd.Parameters.AddWithValue("@idSale", id);
                int idPredstave = 0;
                con.Open();
                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    idPredstave = (int)reader2[0];
                    idSale = (int)reader2[3];
                }
                con.Close();

                //sala u kojoj se odrzava kao i sjedista
                cmd = new MySqlCommand("select * from sala where idSala=@idSale", con);
                cmd.Parameters.AddWithValue("@idSale", idSale);
                Debug.WriteLine(idSale + " kao id sale");
                con.Open();
                MySqlDataReader reader1 = cmd.ExecuteReader();
                string imesale = "";
                int brojKolona = 0;
                int brojVrsta = 0;
                while (reader1.Read())
                {
                    imesale = (string)reader1[1];
                    brojKolona = (int)reader1[2];
                    brojVrsta = (int)reader1[3];
                }
                con.Close();
                //sjedista
                cmd = new MySqlCommand("select * from sjediste where idSala=@idSale order by vrsta, kolona", con);
                cmd.Parameters.AddWithValue("@idSale", idSale);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idSjediste = (int)reader[0];
                    string vrsta = (string)reader[1];
                    int kolona = (int)reader[2];
                    Debug.WriteLine(vrsta + " kao vrsta");
                    Debug.WriteLine(kolona + " kao kolona");
                    // Debug.WriteLine(vrsta );
                    // Debug.WriteLine(kolona );
                    string status = (string)reader[4];
                    Debug.WriteLine(status + " kao status");
                    Sjediste sjediste = new Sjediste { idSjediste = idSjediste, vrsta = vrsta, kolona = kolona, idSale = idSale, status = status };
                    sjedista.Add(sjediste);
                }
                sala = new Sala { idPredstave = idPredstave, idSale = idSale, sjedista = sjedista, imeSale = imesale, brojKolona = brojKolona, brojVrsta = brojVrsta };
                con.Close();

                //rezervaacije 
                cmd = new MySqlCommand("select * from rezervacija where idPredstavaNaRepertoaru=@idPR", con);
                cmd.Parameters.AddWithValue("@idPR", id);
                List<String> rezervacije = new List<String>();
                con.Open();
                MySqlDataReader reader4 = cmd.ExecuteReader();
                while (reader4.Read())
                {
                    string rezervacija = (string)reader4[4];
                    rezervacije.Add(rezervacija);
                }

                con.Close();

                Debug.WriteLine("eee izvan foreach ");
                //sada inicijalizacija rezervacija

                foreach (var s in sjedista)
                {
                    Debug.WriteLine("u jednom");
                    foreach (var r in rezervacije)
                    {
                        //Debug.WriteLine("aaa");
                        String[] split = r.Split(",".ToCharArray());
                        for (int i = 0; i < split.Length; i++)
                        {
                            if (split[i].Equals(s.vrsta + "" + s.kolona) == true)
                            {
                                //Debug.WriteLine("oooo");
                                s.status = "rezervisana";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
                //return View();
            }

            return sala;
        }

        public int rezervisi(string Seats, int? Number, int? idPredstave, Boolean kupovina, int idKorisnik)
        {
            cmd = new MySqlCommand("select stanje,ukupnoKarata from posjetilac where idKorisnik=@idKorisnik", con);
            cmd.Parameters.AddWithValue("@idKorisnik", idKorisnik);
            con.Open();
            int ukupnoKarata = 0;
            MySqlDataReader reader = cmd.ExecuteReader();
            float stanje = 0;
            while (reader.Read())
            {
                stanje = (float)reader[0];
                ukupnoKarata = (int)reader[1];

            }
            con.Close();

            if (kupovina == true)
            {
                Debug.WriteLine("kupovicna");
                
                cmd = new MySqlCommand("select * from predstavanarepertoaru where idPredstavaNaRepertoaru=@idPredstave", con);
                cmd.Parameters.AddWithValue("@idPredstave", idPredstave);
                con.Open();
                MySqlDataReader reader1 = cmd.ExecuteReader();
                float cijena = 0;
                while (reader1.Read())
                {
                    cijena = (float)reader1[4];

                }
                con.Close();
                Debug.WriteLine(cijena);
                if (cijena > stanje)
                    return 2;


                stanje -= cijena;
                ukupnoKarata += !(Number is null) ? (int)Number : 0;
                cmd = new MySqlCommand("update posjetilac set stanje=@Stanje,ukupnoKarata=@brojKarata where idKorisnik=@idKorisnik", con);
                cmd.Parameters.AddWithValue("@Stanje", stanje);
                cmd.Parameters.AddWithValue("@brojKarata", ukupnoKarata);
                cmd.Parameters.AddWithValue("@idKorisnik", idKorisnik);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
                //UPDATE `mydb`.`posjetilac` SET `stanje` = '5' WHERE (`Korisnik_idKorisnik` = '3');

            }
            else
            {
                ukupnoKarata += !(Number is null) ? (int)Number : 0;
                cmd = new MySqlCommand("update posjetilac set ukupnoKarata=@brojKarata where idKorisnik=@idKorisnik", con);
                
                cmd.Parameters.AddWithValue("@brojKarata", ukupnoKarata);
                cmd.Parameters.AddWithValue("@idKorisnik", idKorisnik);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

            }



            cmd = new MySqlCommand("insert into rezervacija(brojKarata, idPredstavaNaRepertoaru, idKorisnik, sjedista, placeno) values(@Broj, @Predstava, @Korisnik, @Sjedista, @Kupovina);", con);

            cmd.Parameters.AddWithValue("@Broj", Number);
            cmd.Parameters.AddWithValue("@Predstava", idPredstave);
            cmd.Parameters.AddWithValue("@Korisnik", idKorisnik);
            cmd.Parameters.AddWithValue("@Sjedista", Seats);
            cmd.Parameters.AddWithValue("@Kupovina", kupovina);

            con.Open();

            int result = cmd.ExecuteNonQuery();

            con.Close();

            return 0;
        }
    }
}
