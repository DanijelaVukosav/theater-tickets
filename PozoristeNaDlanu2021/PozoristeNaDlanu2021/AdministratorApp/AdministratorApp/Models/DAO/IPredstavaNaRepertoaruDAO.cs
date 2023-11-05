using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models.DAO
{
    interface IPredstavaNaRepertoaruDAO
    {
        List<PredstavaNaRepertoaru> predstaveNaRepertoaru();
        int getIdSaleByNAme(String naziv);
        bool dodajPredstavuNaRepertoar(PredstavaNaRepertoaru predstava);
        void izbrisiPredstavuNaRepertoaru(int id);
        List<string> getEmailRezervacijaZaPredstavu(int id);
        PredstavaNaRepertoaru dohvatiPredstavu(int idP);
        bool dodajSalu(Sala sala);
        bool dodajSjedistaSale(Sala sala);
        Sala dohvatiSalu(int? id);
        bool azurirajSjedista(Sala sala);

    }
}
