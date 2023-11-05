using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class Connection
    {
        protected string mycon;// = "server =localhost; Uid=root; password = Vukosav99; persistsecurityinfo = True; database =pozoriste; SslMode = none";

        protected MySqlConnection con;// = new MySqlConnection(mycon);
        protected MySqlCommand cmd = null;

        public Connection()
        {
            mycon = "server =localhost; Uid=root; password = Vukosav99; persistsecurityinfo = True; database =pozoriste; SslMode = none";
            con = new MySqlConnection(mycon);
        }
    }
}