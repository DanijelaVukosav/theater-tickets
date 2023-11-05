using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biletarnik.Models.DAO
{
    public abstract class DAOFactory
    {
        public abstract PosjetilacDAO CreatePosjetilacDAO();
        public abstract PredstavaNaRepertoaruDAO CreatePredstavaNaRepertoaruDAO();
        public abstract RezervacijaDAO CreateRezervacijaDAO();
        public abstract SalaDAO CreateSalaDAO();
        public abstract UserDAO CreateUserDAO();
    }
}