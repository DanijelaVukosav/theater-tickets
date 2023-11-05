using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class UserDAO:Connection, IUsedDAO
    {
        public UserDAO() : base() { }
        public List<User> biletarnici()
        {
            List<User> biletarnici = new List<User>();

            try
            {
                cmd = new MySqlCommand("select * from korisnik where uloga=@uloga", con);
                cmd.Parameters.AddWithValue("@uloga", "b");
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int UserId = (int)reader[0];
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
                        adresa = (string)reader[9];
                    }
                    catch (InvalidCastException)
                    { }
                    User korisnik = new User { UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };
                    biletarnici.Add(korisnik);
                }
                con.Close();


            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaa");
                return new List<User>();
            }
            return biletarnici;
        }
        public bool AddBiletarnik(User user)
        {
            Debug.WriteLine(user.UserName + "  " + user.Password);
            try
            {
                cmd = new MySqlCommand("insert into korisnik(uloga,username,password,ime,prezime,email,pol,telefon,adresa) values('b',@username,@password,@ime,@prezime,@email,@pol,@telefon,@adresa)", con);

                cmd.Parameters.AddWithValue("@ime", user.FirstName);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@prezime", user.LastName);
                cmd.Parameters.AddWithValue("@username", user.UserName);
                cmd.Parameters.AddWithValue("@pol", user.Gender);

                cmd.Parameters.AddWithValue("@telefon", user.MobileNo);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@adresa", user.Address);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
                con.Close();

            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return false;
            }

            return true;
        }
        public List<User> korisnici()
        {
            List<User> korisnici = new List<User>();

            try
            {
                cmd = new MySqlCommand("select * from korisnik where uloga=@uloga", con);
                cmd.Parameters.AddWithValue("@uloga", "p");
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int UserId = (int)reader[0];
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
                        adresa = (string)reader[9];
                    }
                    catch (InvalidCastException)
                    { }
                    User korisnik = new User { UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };
                    korisnici.Add(korisnik);
                }
                con.Close();


            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaa");
                return new List<User>();
            }
            return korisnici;
        }
        public bool DeleteUser(int? id)
        {
            try
            {
                cmd = new MySqlCommand("delete from korisnik  where idKorisnik=@id", con);
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
                return false;
            }
            return true;

        }
        public User GetUserByUsername(string username)
        {
            List<User> korisnici = new List<User>();
            User korisnik = null;

            try
            {
                cmd = new MySqlCommand("select * from korisnik where username=@username", con);
                cmd.Parameters.AddWithValue("@username", username);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int UserId = (int)reader[0];
                    string uloga = (string)reader[1];
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
                        adresa = (string)reader[9];
                    }
                    catch (InvalidCastException)
                    { }

                    User k = korisnik = new User { uloga = uloga, UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };
                    korisnici.Add(k);
                }
                con.Close();


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Greskaaa");
                Debug.WriteLine(ex.StackTrace);
                con.Close();
                return null;
            }

            return korisnik;

        }
        public User updateUser(User user)
        {

            if (user.Password != null)
            {

                try
                {
                    cmd = new MySqlCommand("update korisnik set password=@password where idKorisnik=@id", con);
                    cmd.Parameters.AddWithValue("@id", user.UserId);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                catch (Exception)
                {
                    Debug.WriteLine("Greskaaa");
                    con.Close();
                    return new User();
                }
            }

            try
            {
                cmd = new MySqlCommand("update korisnik set ime=@ime,prezime=@prezime,username=@username,pol=@pol,telefon=@telefon,email=@email,adresa=@adresa where idKorisnik=@id", con);
                cmd.Parameters.AddWithValue("@id", user.UserId);
                cmd.Parameters.AddWithValue("@ime", user.FirstName);
                cmd.Parameters.AddWithValue("@prezime", user.LastName);
                cmd.Parameters.AddWithValue("@username", user.UserName);
                cmd.Parameters.AddWithValue("@pol", user.Gender);

                cmd.Parameters.AddWithValue("@telefon", user.MobileNo);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@adresa", user.Address);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return new User();
            }

            return user;
        }
        public bool AddUser(User user)
        {
            Debug.WriteLine(user.UserName + "  " + user.Password);
            try
            {
                cmd = new MySqlCommand("insert into korisnik(uloga,username,password,ime,prezime,email,pol,telefon,adresa) values('p',@username,@password,@ime,@prezime,@email,@pol,@telefon,@adresa)", con);

                cmd.Parameters.AddWithValue("@ime", user.FirstName);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@prezime", user.LastName);
                cmd.Parameters.AddWithValue("@username", user.UserName);
                cmd.Parameters.AddWithValue("@pol", user.Gender);

                cmd.Parameters.AddWithValue("@telefon", user.MobileNo);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@adresa", user.Address);
                Debug.WriteLine("prije otvaranja");
                con.Open();
                Debug.WriteLine("posle");
                cmd.ExecuteNonQuery();
                Debug.WriteLine("prije izvrsavanja");
                con.Close();

            }
            catch (Exception)
            {
                Debug.WriteLine("Greskaaa");
                con.Close();
                return false;
            }

            return true;
        }

        public List<User> sviKorisnici()
        {
            List<User> korisnici = new List<User>();
            try
            {
                cmd = new MySqlCommand("select * from korisnik", con);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int UserId = (int)reader[0];
                    string uloga = (string)reader[1];
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
                    User k = new User { uloga= uloga, UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };
                    korisnici.Add(k);
                }
                con.Close();
            }
            catch (Exception)
            {
                con.Close();
            }
            return korisnici;
        }

        public User korisnikByKorisnickoIme(String username)
        {
            List<User> korisnici = new List<User>();
            User korisnik = null;
            try
            {
                cmd = new MySqlCommand("select * from korisnik where username=@username", con);
                cmd.Parameters.AddWithValue("@username", username);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                string uloga = "";
                while (reader.Read())
                {

                    int UserId = (int)reader[0];
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
                    uloga = (string)reader[1];
                    User k = korisnik = new User {uloga=uloga, UserId = UserId, FirstName = (string)reader[4], LastName = (string)reader[5], UserName = UserName, Password = Password, Gender = (string)reader[7], MobileNo = telefon, Email = Email, Address = adresa };
                    korisnici.Add(k);
                }
                con.Close();
            }
            catch (Exception) 
            {
                con.Close();
            }
            return korisnik;
        }
    }
}