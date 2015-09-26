using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Battle.Domain.Concrete;
using WebMatrix.WebData;

namespace Battle.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            logger.Info("Application Start");
            logger.Info("WebSecurity InitializeDatabaseConnection");
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: false);
            logger.Info("Area Registration");
            AreaRegistration.RegisterAllAreas();
            logger.Info("WebApiConfig");
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            logger.Info("FilterConfig");
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            logger.Info("RouteConfig");
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            logger.Info("BundleConfig");
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            logger.Info("AuthConfig");
            AuthConfig.RegisterAuth();

            // Снимаем ошибку возникающую при изменении модели или структуры базы
            logger.Info("Database SetInitializer");
            Database.SetInitializer<Entities>(null);

            //Database.SetInitializer<CountryContext>(null);
            //Database.SetInitializer<ManufactureContext>(null);
            //Database.SetInitializer<ArmyContext>(null);
            //Database.SetInitializer<BattleMechContext>(null);
            //Database.SetInitializer<DesignContext>(null);
            //Database.SetInitializer<GamerContext>(null);
            //Database.SetInitializer<UsersContext>(null);            
            //Database.SetInitializer<CountryContext>(new DropCreateDatabaseIfModelChanges<CountryContext>());
        }

        public void Application_Error() {
            logger.Info("Application Error");
        }

        public void Application_End() {
            logger.Info("Application End");
        }

        //protected void Application_EndRequest()
        //{
        //    if (Context.Response.StatusCode == 302 &&
        //        Context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    {
        //        Context.Response.Clear();
        //        Context.Response.StatusCode = 401;
        //    }
        //}
    }
}