using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Biletarnik.Models.DAO
{
    public class PosjetilacDAO: Connection, IPosjetilacDAO
    {
        public PosjetilacDAO():base(){}
        public  double GetStanjePosjetioca(int idKorisnik)
        {
            try
            {
                cmd = new MySqlCommand("select * from posjetilac where idKorisnik=@idKorisnik", con);
                cmd.Parameters.AddWithValue("@idKorisnik", idKorisnik);
                double stanje = 0;
                con.Open();
                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    stanje = (int)reader2[1];
                }
                con.Close();

                return stanje;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0;
            }
        }
      
        public bool UplataNaRacun(double iznos, int id)
        {
            try
            {
                cmd = new MySqlCommand("update posjetilac set stanje=@Stanje where idKorisnik=@idKorisnik", con);
                cmd.Parameters.AddWithValue("@Stanje", iznos);
                cmd.Parameters.AddWithValue("@idKorisnik", id);
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
    }
}