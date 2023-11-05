using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models.DAO
{
    interface IGlumacDAO
    {
        List<Glumac> listaGlumaca();
        List<string> getSlikeGlumca(int idGlumca);
        List<Predstava> IgranePredstaveGlumca(int idGlumac);
        void updateGlumac(Glumac glumac);
        void IzbrisiGlumca(int id);
        List<Predstava> getNeigranePredstave(int idGlumac);
        void DodajPredstavuGlumcu(int idGlumac, int idPredstava);
        void IzbrisiIgranuPredstavu(int idGlumac, int idPredstava);
        void UkloniSliku(int id, string putanja);
        int AddGlumac(Glumac glumac);
        int AddSlikaGlumca(SlikaGlumca slika);
        Glumac getGlumac(int? id);
    }
}
