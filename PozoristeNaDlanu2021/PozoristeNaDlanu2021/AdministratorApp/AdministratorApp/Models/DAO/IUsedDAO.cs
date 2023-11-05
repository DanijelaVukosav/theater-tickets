using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models.DAO
{
    interface IUsedDAO
    {
        List<User> biletarnici();
        bool AddBiletarnik(User user);
        List<User> korisnici();
        bool DeleteUser(int? id);
        User GetUserByUsername(string username);
        User updateUser(User user);
        bool AddUser(User user);
        List<User> sviKorisnici();

    }
}
