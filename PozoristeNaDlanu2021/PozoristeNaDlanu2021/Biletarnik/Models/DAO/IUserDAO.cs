using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletarnik.Models.DAO
{
    interface IUserDAO
    {
        int getUserID(string name);
        User userByName(string name);
    }
}
