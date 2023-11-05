using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Biletarnik.Models.DAO
{
    public class SalaDAO:Connection, ISalaDAO
    {
        public SalaDAO():base(){}
        public Sala getById(int? id)
        {
            List<Sjediste> sjedista = new List<Sjediste>();
            Debug.WriteLine(id + " kao id predstave na repertoaru");

            Sala sala;
            try
            {
                int idSale = 0;
                //predstava na repertoaru
                cmd = new MySqlCommand("select * from predstavanarepertoaru where idPredstavaNaRepertoaru=@idPredstave", con);
                cmd.Parameters.AddWithValue("@idPredstave", id);
                int idPredstave = 0;
                con.Open();
                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    idPredstave = (int)reader2[1];
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
                ////ovdjeee
                sala = new Sala { idPredstave = (int)id, idSale = idSale, sjedista = sjedista, imeSale = imesale, brojKolona = brojKolona, brojVrsta = brojVrsta };
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
                return new Sala();
            }
            return sala;
        }
    }
}