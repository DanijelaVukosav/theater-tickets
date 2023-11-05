using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web;
using PozoristeNaDlanuTemplate.Models;
using System.Diagnostics;

namespace PozoristeNaDlanuTemplate.Models
{
    public class GlumacDAO : DAO
    {

        public GlumacDAO() : base()
        {
            
        }

        public List<Glumac> listaGlumaca()
        {
            List<Glumac> glumci = new List<Glumac>();
            List<(Glumac glumac, int idPredstave)> pomocnaLista = new List<(Glumac glumac, int idPredstave)>();
            try
            {
                cmd = new MySqlCommand("select glumac.idGlumac,ime,prezime,biografija,idPredstave from glumac left join glumciupredstavi on glumac.idGlumac=glumciupredstavi.idGlumac", con);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int idGlumca = (int)reader[0];
                    string imeGlumca = (string)reader[1];
                    string prezimeGlumca = (string)reader[2];
                    string biografijaGlumca = (string)reader[3];
                    object idPredstave = reader[4];

                    Glumac glumac = new Glumac { id = idGlumca, ime = imeGlumca, prezime = prezimeGlumca, biografija = biografijaGlumca, slikeGlumca = new List<string>(), igranePredstave = new List<Predstava>() };

                    if (!glumci.Exists(x => x.id == glumac.id))
                    {
                        glumci.Add(glumac);
                    }
                    try
                    {
                        pomocnaLista.Add((glumac, (int)idPredstave));
                    }
                    catch (InvalidCastException)
                    {
                        pomocnaLista.Add((glumac, -1));
                    }
                }
                con.Close();
                foreach (var g in pomocnaLista)
                {
                    if (g.idPredstave > -1)
                    {
                        con.Open();
                        cmd = new MySqlCommand("select * from predstava where idPredstave=@IDp", con);
                        cmd.Parameters.AddWithValue("@IDp", g.idPredstave);

                        MySqlDataReader reader2 = cmd.ExecuteReader();
                        while (reader2.Read())
                        {
                            Predstava predstava = new Predstava { id = (int)reader2[0], naziv = (string)reader2[1], datumPremijere = (DateTime)reader2[2], rezija = (string)reader2[3], scenografija = (string)reader2[4], opis = (string)reader2[5], tekstopisac = (string)reader2[6] };
                            glumci.Find(x => x.id == g.glumac.id).igranePredstave.Add(predstava);

                        }
                        con.Close();
                    }
                }
                foreach (var g in glumci)
                {
                    con.Open();
                    cmd = new MySqlCommand("select * from slikeglumaca where idGlumac=@IDgl", con);
                    cmd.Parameters.AddWithValue("@IDgl", g.id);

                    MySqlDataReader reader2 = cmd.ExecuteReader();
                    while (reader2.Read())
                    {
                        glumci.Find(x => x.id == g.id).slikeGlumca.Add((string)reader2[1]);

                    }
                    con.Close();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
            return glumci;
        }

        public Glumac getGlumac(int? id)
        {
            Glumac glumac = null;

            try
            {
                cmd = new MySqlCommand("select pom.idGlumac,ime,prezime,biografija,pom.idPredstave,naziv,datumPremijere,rezija,scenografija,opis,tekstopisac from ( select glumac.idGlumac,ime,prezime,biografija,idPredstave from glumac left join glumciupredstavi on glumac.idGlumac=glumciupredstavi.idGlumac)as pom left join predstava on pom.idPredstave=predstava.idPredstave where idGlumac=@IDg", con);
                cmd.Parameters.AddWithValue("@IDg", id);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int idGlumca = (int)reader[0];
                    string imeGlumca = (string)reader[1];
                    string prezimeGlumca = (string)reader[2];
                    string biografijaGlumca = (string)reader[3];
                    if (glumac is null)
                        glumac = new Glumac { id = idGlumca, ime = imeGlumca, prezime = prezimeGlumca, biografija = biografijaGlumca, slikeGlumca = new List<string>(), igranePredstave = new List<Predstava>() };
                    Predstava predstava = new Predstava { id = (int)reader[4], naziv = (string)reader[5], datumPremijere = (DateTime)reader[6], rezija = (string)reader[7], scenografija = (string)reader[8], opis = (string)reader[9], tekstopisac = (string)reader[10] };
                    glumac.igranePredstave.Add(predstava);


                }
                con.Close();
                con.Open();
                cmd = new MySqlCommand("select * from slikeglumaca where idGlumac=@IDglumca", con);
                cmd.Parameters.AddWithValue("@IDglumca", id);
                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    glumac.slikeGlumca.Add((string)reader2[1]);

                }
                con.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
            return glumac;
        }
    }
}