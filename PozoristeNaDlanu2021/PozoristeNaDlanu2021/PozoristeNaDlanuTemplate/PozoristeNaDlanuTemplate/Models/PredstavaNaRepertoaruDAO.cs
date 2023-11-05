using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class PredstavaNaRepertoaruDAO : DAO
    {

        public PredstavaNaRepertoaruDAO() : base()
        {

        }

        public List<PredstavaNaRepertoaru> listaPredstava()
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
                    PredstavaNaRepertoaru predstava = new PredstavaNaRepertoaru { id = id, naziv = naziv, idPredstave = idPredstaveKojojOdgovara, vrijemeOdrzavanja = vrijemeOdrzavanja, cijena=cijena };
                    predstave.Add(predstava);
                }
                con.Close();
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                //return View();
            }
            return predstave;
        }

    }
}