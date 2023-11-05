using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletarnik.Models.DAO
{
    interface IRezervacijaDAO
    {
        bool PotvrdiRezervaciju(int? id);
        bool ObrisiRezervaciju(int? id);
        int DodajRezervaciju(int? Number, int? idPredstave, int? idKorisnik, string Seats, Boolean? kupovina);
        List<Rezervacija> rezervacijePredstave(int? id);

    }
}
