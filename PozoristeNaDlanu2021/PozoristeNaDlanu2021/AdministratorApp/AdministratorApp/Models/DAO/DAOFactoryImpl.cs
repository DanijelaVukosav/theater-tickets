using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public class DAOFactoryImpl:DAOFactory
    {
        public override GlumacDAO CreateGlumacDAO()
        {
            return new GlumacDAO();
        }
        public override InformacijeDAO CreateInformacijeDAO()
        {
            return new InformacijeDAO();
        }
        public override PredstavaDAO CreatePredstavaDAO()
        {
            return new PredstavaDAO();
        }
        public override PredstavaNaRepertoaruDAO CreatePredstavaNaRepertoaruDAO()
        {
            return new PredstavaNaRepertoaruDAO();
        }
        public override UserDAO CreateUserDAO()
        {
            return new UserDAO();
        }
        public override VijestDAO CreateVijestDAO()
        {
            return new VijestDAO();
        }
    }
}