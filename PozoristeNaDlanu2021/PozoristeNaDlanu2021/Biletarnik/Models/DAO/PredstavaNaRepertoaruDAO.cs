using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Biletarnik.Models.DAO
{
    public class PredstavaNaRepertoaruDAO: Connection, IPredstavaNaRepertoaruDAO
    {

        public PredstavaNaRepertoaruDAO():base(){}
        public double getCijenuPredstave(int?idPredstave)
        {
            if (idPredstave == null)
                return 0;
            try{
                cmd = new MySqlCommand("select * from predstavanarepertoaru where idPredstavaNaRepertoaru=@idPredstave", con);
                cmd.Parameters.AddWithValue("@idPredstave", idPredstave);
                con.Open();
                MySqlDataReader reader1 = cmd.ExecuteReader();
                double cijena = 0;
                while (reader1.Read())
                {
                    cijena = (int)reader1[4];

                }
                con.Close();
                return cijena;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0;
            }
        }
    
        public List<PredstavaNaRepertoaru> getAll()
        {
            List<PredstavaNaRepertoaru> predstave = new List<PredstavaNaRepertoaru>();

            try
            {
                cmd = new MySqlCommand("select idPredstavaNaRepertoaru,naziv,idPredstave,vrijeme, cijena from predstavanarepertoaru natural join predstava where vrijeme>now()", con);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string naziv = (string)reader[1];
                    int idPredstaveKojojOdgovara = (int)reader[2];
                    DateTime vrijemeOdrzavanja = (DateTime)reader[3];
                    double cijena = (float)reader[4];
                    PredstavaNaRepertoaru predstava = new PredstavaNaRepertoaru { cijena = cijena, id = id, naziv = naziv, idPredstave = idPredstaveKojojOdgovara, vrijemeOdrzavanja = vrijemeOdrzavanja };
                    predstave.Add(predstava);
                }
                con.Close();
                return predstave;
            }
            catch (Exception ex)
            {
                con.Close();
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine(ex.Message);
                return predstave;
            }
        }
    }
}