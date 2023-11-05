using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models.DAO
{
    interface IInformacijeDAO
    {
        Informacije GetInformacije();
        InformacijePomocna GetInfoById(int? id);
        void deleteInfo(int? id);
        bool updateInfo(InformacijePomocna info);
        bool AddInfo(InformacijePomocna info);
    }
}
