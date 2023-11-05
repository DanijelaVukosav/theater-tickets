using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biletarnik.Models.DAO
{
    public class Connection
    {
        protected string mycon;

        protected MySqlConnection con;
        protected MySqlCommand cmd;

        public Connection()
        {
            mycon = "server =localhost; Uid=root; password = Vukosav99; persistsecurityinfo = True; database =pozoriste; SslMode = none";
            con = new MySqlConnection(mycon);
        }
    }
}