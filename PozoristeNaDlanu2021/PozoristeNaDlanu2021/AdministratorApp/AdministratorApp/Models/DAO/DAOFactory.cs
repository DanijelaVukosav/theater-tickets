using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministratorApp.Models.DAO
{
    public abstract class DAOFactory
    {
        public abstract GlumacDAO CreateGlumacDAO();
        public abstract InformacijeDAO CreateInformacijeDAO();
        public abstract PredstavaDAO CreatePredstavaDAO();
        public abstract PredstavaNaRepertoaruDAO CreatePredstavaNaRepertoaruDAO();
        public abstract UserDAO CreateUserDAO();
        public abstract VijestDAO CreateVijestDAO();
    }
}