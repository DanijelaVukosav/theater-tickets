using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Biletarnik.Models.DAO
{
    public class UserDAO:Connection, IUserDAO
    {
        
        
        public UserDAO() : base() { }
        public int getUserID(string name)
        {
            try
            {
                cmd = new MySqlCommand("select * from korisnik where username=@username", con);
                cmd.Parameters.AddWithValue("@username", name);
                int idKorisnika = 0;
                con.Open();
                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    idKorisnika = (int)reader2[0];
                }
                con.Close();

                return idKorisnika;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return -1;
            }
        }

        public User userByName(string name)
        {
            try
            {
                User korisnik = null;
                cmd = new MySqlCommand("select * from korisnik where username=@username", con);
                cmd.Parameters.AddWithValue("@username", name);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                string uloga = null;
                while (reader.Read())
                {

                    int UserId = (int)reader[0];
                    uloga = (string)reader[1];
                    string UserName = (string)reader[2];
                    string Password = (string)reader[3];
                    string Email = (string)reader[6];
                    string telefon = "";
                    try
                    {
                        telefon = (string)reader[8];
                    }
                    catch (InvalidCastException)
                    { }
                    string adresa = "";
                    try
                    {
                        telefon = (string)reader[9];
                    }
                    catch (InvalidCastException)
                    { }
                    korisnik = new User { uloga=uloga, UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };

                }
                con.Close();
                return korisnik;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}