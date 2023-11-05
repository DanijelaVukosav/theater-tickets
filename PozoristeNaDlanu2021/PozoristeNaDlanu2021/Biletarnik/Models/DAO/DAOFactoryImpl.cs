using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biletarnik.Models.DAO
{
    public class DAOFactoryImpl:DAOFactory
    {
        public override PosjetilacDAO CreatePosjetilacDAO()
        {
            return new PosjetilacDAO();
        }
        public override PredstavaNaRepertoaruDAO CreatePredstavaNaRepertoaruDAO() 
        {
            return new PredstavaNaRepertoaruDAO();
        }
        public override RezervacijaDAO CreateRezervacijaDAO()
        {
            return new RezervacijaDAO();
        }
        public override SalaDAO CreateSalaDAO()
        {
            return new SalaDAO();
        }
        public override UserDAO CreateUserDAO()
        {
            return new UserDAO();
        }
    }
}