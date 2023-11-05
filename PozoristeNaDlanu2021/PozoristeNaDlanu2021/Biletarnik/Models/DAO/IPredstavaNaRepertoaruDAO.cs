using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletarnik.Models.DAO
{
    interface IPredstavaNaRepertoaruDAO
    {
        double getCijenuPredstave(int? idPredstave);
        List<PredstavaNaRepertoaru> getAll();
    }
}
