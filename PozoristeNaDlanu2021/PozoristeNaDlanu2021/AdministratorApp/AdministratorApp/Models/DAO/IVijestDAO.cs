using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models.DAO
{
    interface IVijestDAO
    {
        List<Vijest> listaVijesti();
        Vijest GetVijestById(int? idVijesti);
        void UkloniSliku(int id, string putanja);
        void addComment(Vijest vijest, String komentar, String UserID);
        int dodajVijest(Vijest vijest);
        void detaljiVijesti(Vijest vijest, string komentar, string korisnik);
        void sacuvajIzmjene(Vijest vijest);
        void izbrisiVijest(Vijest vijest);
        List<string> GetSlikeVijesti(int id);
        int AddSlikaVijest(SlikaVijesti slika);
    }
}
