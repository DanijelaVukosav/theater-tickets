using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class GlumacDAO:Connection, IGlumacDAO
    {
        public GlumacDAO() : base() { }

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
                    int idPredstave = -1;
                    if (reader.IsDBNull(4))
                        Debug.WriteLine("NULLLLLLLLLL");
                    else
                        idPredstave = (int)reader[4];
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
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
            return glumci;
        }

        public List<string> getSlikeGlumca(int idGlumca)
        {
            List<string> slike = new List<string>();
            try
            {
                con.Open();
                cmd = new MySqlCommand("select * from slikeglumaca where idGlumac=@IDgl", con);
                cmd.Parameters.AddWithValue("@IDgl", idGlumca);

                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    slike.Add((string)reader2[1]);

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

        public List<Predstava> IgranePredstaveGlumca(int idGlumac)
        {
            List<Predstava> predstave = new List<Predstava>();
            try
            {
                con.Open();
                cmd = new MySqlCommand("select * from glumciupredstavi natural join predstava where idGlumac=@IDgl", con);
                cmd.Parameters.AddWithValue("@IDgl", idGlumac);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string naziv = (string)reader[2];
                    DateTime datumPremijere = (DateTime)reader[3];
                    string rezija = (string)reader[4];
                    string scenografija = (string)reader[5];
                    string opis = (string)reader[6];
                    string tekstopisac = (string)reader[7];
                    Predstava predstava = new Predstava { id = id, naziv = naziv, datumPremijere = datumPremijere, rezija = rezija, scenografija = scenografija, opis = opis, tekstopisac = tekstopisac };
                    predstave.Add(predstava);

                }
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa u igranim");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
            return predstave;
        }

        public void updateGlumac(Glumac glumac)
        {
            try
            {
                cmd = new MySqlCommand("update glumac set ime=@ime,prezime=@prezime,biografija=@biografija where idGlumac=@id", con);
                cmd.Parameters.AddWithValue("@id", glumac.id);
                cmd.Parameters.AddWithValue("@ime", glumac.ime);
                cmd.Parameters.AddWithValue("@prezime", glumac.prezime);
                cmd.Parameters.AddWithValue("@biografija", glumac.biografija);
                con.Open();
                int rez = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
        }

        public void IzbrisiGlumca(int id)
        {
            try
            {
                con.Open();
                cmd = new MySqlCommand("delete  from glumac where idGlumac=@IDgl", con);
                cmd.Parameters.AddWithValue("@IDgl", id);

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

        public List<Predstava> getNeigranePredstave(int idGlumac)
        {
            List<Predstava> predstave = new List<Predstava>();
            try
            {
                con.Open();
                cmd = new MySqlCommand("SELECT distinct * FROM predstava WHERE idPredstave NOT IN(select distinct idPredstave from glumciupredstavi where IdGlumac = @IDgl)", con);
                cmd.Parameters.AddWithValue("@IDgl", idGlumac);

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
                    Predstava predstava = new Predstava { id = id, naziv = naziv, datumPremijere = datumPremijere, rezija = rezija, scenografija = scenografija, opis = opis, tekstopisac = tekstopisac };
                    predstave.Add(predstava);

                }
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa u neigranim");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
            return predstave;
        }

        public void DodajPredstavuGlumcu(int idGlumac, int idPredstava)
        {
            try
            {
                con.Open();
                cmd = new MySqlCommand("insert into glumciupredstavi values(@IDgl,@idPredstave)", con);
                cmd.Parameters.AddWithValue("@IDgl", idGlumac);
                cmd.Parameters.AddWithValue("@idPredstave", idPredstava);

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

        public void IzbrisiIgranuPredstavu(int idGlumac, int idPredstava)
        {
            try
            {
                con.Open();
                cmd = new MySqlCommand("delete  from glumciupredstavi where idGlumac=@IDgl and idPredstave=@idPredstave", con);
                cmd.Parameters.AddWithValue("@IDgl", idGlumac);
                cmd.Parameters.AddWithValue("@idPredstave", idPredstava);

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

        public void UkloniSliku(int id, string putanja)
        {
            try
            {
                con.Open();
                cmd = new MySqlCommand("delete  from slikeglumaca where idGlumac=@IDgl and putanja=@putanja", con);
                cmd.Parameters.AddWithValue("@IDgl", id);
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

        public  int AddGlumac(Glumac glumac)
        {
            try
            {
                cmd = new MySqlCommand("insert into glumac(ime,prezime,biografija) values(@ime,@prezime,@biografija)", con);
                cmd.Parameters.AddWithValue("@ime", glumac.ime);
                cmd.Parameters.AddWithValue("@prezime", glumac.prezime);
                cmd.Parameters.AddWithValue("@biografija", glumac.biografija);
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
        public int AddSlikaGlumca(SlikaGlumca slika)
        {
            try
            {
                cmd = new MySqlCommand("insert into slikeglumaca(idGlumac,putanja) values(@id,@putanja)", con);
                cmd.Parameters.AddWithValue("@id", slika.idGlumca);
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
                    if (reader.IsDBNull(4))
                        Debug.WriteLine("is null");
                    else
                    {
                        Debug.WriteLine("udje u esle");
                        Predstava predstava = new Predstava { id = (int)reader[4], naziv = (string)reader[5], datumPremijere = (DateTime)reader[6], rezija = (string)reader[7], scenografija = (string)reader[8], opis = (string)reader[9], tekstopisac = (string)reader[10] };
                        glumac.igranePredstave.Add(predstava);
                    }


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
                Debug.WriteLine(ex.StackTrace);
                con.Close();
            }
            return glumac;
        }
    }
}