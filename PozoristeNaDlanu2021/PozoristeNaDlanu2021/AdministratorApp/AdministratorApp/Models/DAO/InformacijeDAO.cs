using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class InformacijeDAO:Connection,IInformacijeDAO
    {
        public InformacijeDAO() : base() { }
        public Informacije GetInformacije()
        {
            List<Informacije> informacije = new List<Informacije>();
            Informacije info = new Informacije { informacije = new List<InformacijePomocna>(), glumci = new List<Glumac>() };
            try
            {
                cmd = new MySqlCommand("select * from informacije", con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    info.informacije.Add(new InformacijePomocna { id = (int)reader[0], naslov = (string)reader[1], sadrzaj = (string)reader[2] });
                }
                info.glumci = new DAOFactoryImpl().CreateGlumacDAO().listaGlumaca();
                con.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return new Informacije();
            }
            return info;
        }
        public InformacijePomocna GetInfoById(int? id)
        {
            InformacijePomocna info = null;
            try
            {

                cmd = new MySqlCommand("select * from informacije where idInformacije=@ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    info = new InformacijePomocna { id = (int)reader[0], naslov = (string)reader[1], sadrzaj = (string)reader[2] };
                }
                con.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return null;
            }
            return info;
        }
        public void deleteInfo(int? id)
        {
            try
            {
                cmd = new MySqlCommand("delete from informacije  where idInformacije=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                int rez = cmd.ExecuteNonQuery();
                con.Close();
                Debug.WriteLine("izbrise " + rez);

            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
            }
        }
        public bool updateInfo(InformacijePomocna info)
        {
            try
            {
                cmd = new MySqlCommand("update informacije set naslov=@naslov,sadrzaj=@sadrzaj where idInformacije=@id", con);
                cmd.Parameters.AddWithValue("@id", info.id);
                cmd.Parameters.AddWithValue("@naslov", info.naslov);
                cmd.Parameters.AddWithValue("@sadrzaj", info.sadrzaj);
                con.Open();
                int rez = cmd.ExecuteNonQuery();
                con.Close();
                if (rez > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return false;
            }
        }
        public bool AddInfo(InformacijePomocna info)
        {
            try
            {
                cmd = new MySqlCommand("insert into informacije(naslov,sadrzaj) values(@naslov,@sadrzaj)", con);
                cmd.Parameters.AddWithValue("@sadrzaj", info.sadrzaj);
                cmd.Parameters.AddWithValue("@naslov", info.naslov);
                con.Open();
                int rez = cmd.ExecuteNonQuery();
                con.Close();
                if (rez > 0)
                    return true;
                else
                    return false;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
                return false;
            }
        }
    }
}