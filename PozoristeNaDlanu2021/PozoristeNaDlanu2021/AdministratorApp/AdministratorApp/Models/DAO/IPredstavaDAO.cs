using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models.DAO
{
    interface IPredstavaDAO
    {
        List<Predstava> predstaveList();
        List<Glumac> getGlumciPredstave(object id);
        List<Glumac> GlumciKojiNeIgrajuUPredstavi(int idPredstave);
        Predstava GetPredstavaById(int? idp);
        Predstava DohvatiGlumcePredstave(Predstava predstava);
        Boolean addPredstavu(Predstava predstava);
        void dodajSlikuPredstave(int idPredstave, String putanja);
        void dodajKomentar(String komentar, int idPredstave, String idKorisnika);
        void izmijeniPredstavu(Predstava predstava);
        void obrisiPredstavu(int? id);
        int obrisiSliku(int? id);
        int obrisiKomentar(int? id);
        int getIdByName(String naziv);
    }
}
