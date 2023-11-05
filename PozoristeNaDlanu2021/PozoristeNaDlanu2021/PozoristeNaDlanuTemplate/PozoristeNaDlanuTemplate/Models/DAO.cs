using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PozoristeNaDlanuTemplate.Models
{
    public class DAO
    {
        public static string  mycon;
        public static MySqlConnection con;
        public static MySqlCommand cmd = null;
        public DAO()
        {
            mycon = "server =localhost; Uid=root; password = Vukosav99; persistsecurityinfo = True; database =pozoriste; SslMode = none";
            con = new MySqlConnection(mycon);
        }
        
    }
}