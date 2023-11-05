using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletarnik.Models.DAO
{
    interface IPosjetilacDAO
    {
        double GetStanjePosjetioca(int idKorisnik);
        bool UplataNaRacun(double iznos, int id);
    }
}
