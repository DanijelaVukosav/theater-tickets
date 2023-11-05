using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Biletarnik.Models;
using MySql.Data.MySqlClient;
namespace Biletarnik.Models.DAO
{
    public class RezervacijaDAO:Connection, IRezervacijaDAO
    {
       
        public RezervacijaDAO() : base() { }
        public bool PotvrdiRezervaciju(int?id)
        {
            try
            {
                cmd = new MySqlCommand("update rezervacija set placeno=@Stanje where idRezervacija=@idRezervacija", con);
                cmd.Parameters.AddWithValue("@Stanje", 1);
                cmd.Parameters.AddWithValue("@idRezervacija", id);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
                Debug.WriteLine("greska");
                return false;
            }
        }

        public bool ObrisiRezervaciju(int? id)
        {
            try
            {
                cmd = new MySqlCommand("delete from rezervacija where idRezervacija=@idRezervacija", con);
                cmd.Parameters.AddWithValue("@idRezervacija", id);
                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine("greska");
                return false;
            }
        }

        public List<Rezervacija> rezervacijeOdredjenogKorisnika(int id,string search)
        {
            List<Rezervacija> rezervacije = new List<Rezervacija>();
            try
            {
                if (id < 0)
                    cmd = new MySqlCommand("select idPredstavaNaRepertoaru,idRezervacija,brojKarata,idKorisnik,sjedista,placeno,username from (select * from rezervacija natural join predstavanarepertoaru) as pom natural join korisnik", con);
                else
                {
                    cmd = new MySqlCommand("select idPredstavaNaRepertoaru,idRezervacija,brojKarata,idKorisnik,sjedista,placeno,username from (select * from rezervacija natural join predstavanarepertoaru) as pom natural join korisnik where idPredstavaNaRepertoaru=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                }
                con.Open();

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
                    string username = (string)reader[6];
                    if (username.Contains(search))
                    {
                        Rezervacija r = new Rezervacija { username=username,idRezervacija = (int)reader[1], brojKarata = (int)reader[2], idPredstaveNaRepertoaru = (int)reader[0], idKorisnik = (int)reader[3], sjedista = (string)reader[4], placeno = pl };
                        rezervacije.Add(r);
                    }
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
                    //rezervacije = FiltrirajPoPaternu(rezervacije, search);
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

        private List<Rezervacija> FiltrirajPoPaternu(List<Rezervacija> rezervacije, string search)
        {
            return rezervacije;
        }

        public int DodajRezervaciju(int?Number, int?idPredstave, int?idKorisnik, string Seats, Boolean?kupovina)
        {
            if (Number == null || idPredstave == null || Seats == null)
                return -1;
            try
            {
                cmd = new MySqlCommand("insert into rezervacija(brojKarata, idPredstavaNaRepertoaru, idKorisnik, sjedista, placeno) values(@Broj, @Predstava, @Korisnik, @Sjedista, @Kupovina);", con);

                cmd.Parameters.AddWithValue("@Broj", Number);
                cmd.Parameters.AddWithValue("@Predstava", idPredstave);
                cmd.Parameters.AddWithValue("@Korisnik", idKorisnik);
                cmd.Parameters.AddWithValue("@Sjedista", Seats);
                cmd.Parameters.AddWithValue("@Kupovina", kupovina);

                con.Open();

                int result = cmd.ExecuteNonQuery();

                con.Close();
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return -1;
            }
        }

        public List<Rezervacija> rezervacijePredstave(int? id)
        {

            List<Rezervacija> rezervacije = new List<Rezervacija>();
            try
            {
                if (id < 0)
                    cmd = new MySqlCommand("select idPredstavaNaRepertoaru,idRezervacija,brojKarata,idKorisnik,sjedista,placeno,username from (select * from rezervacija natural join predstavanarepertoaru) as pom natural join korisnik", con);
                else
                {
                    cmd = new MySqlCommand("select idPredstavaNaRepertoaru,idRezervacija,brojKarata,idKorisnik,sjedista,placeno,username from (select * from rezervacija natural join predstavanarepertoaru) as pom natural join korisnik where idPredstavaNaRepertoaru=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                }
                con.Open();

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

                    Rezervacija r = new Rezervacija { username=(string)reader[6],idRezervacija = (int)reader[1], brojKarata = (int)reader[2], idPredstaveNaRepertoaru = (int)reader[0], idKorisnik = (int)reader[3], sjedista = (string)reader[4], placeno = pl };
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
    }
}