using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SistemaGeneraliz.Models.Helpers;
using WebMatrix.WebData;

namespace SistemaGeneraliz
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            try
            {
                /*solo si IfModelChanges intentará hacer esto, pero se caerá por lo que entrará al catch
                 * la idea de hacer esto es para evitar regenerar todas las tablas y los seeds cada vez que corra (solo se
                 * debe hacer eso si el modelo cambia), y así sea más rápida la ejecución.
                */
                Database.SetInitializer<SGPContext>(new SGPContextInitializer());
            }
            catch (Exception ex)
            {
                try
                {
                    using (var context = new SGPContext())
                    {
                        //IF DATABASE ALREADY EXISTED, ONLY DROP TABLES AND RECREATE THEM
                        if (context.Database.Exists())
                        {
                            Database.SetInitializer<SGPContext>(new DropOnlyTables<SGPContext>());
                            var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                            context.Database.ExecuteSqlCommand(dbCreationScript);
                            WebSecurity.InitializeDatabaseConnection("SGPContext", "Persona", "PersonaId", "UserName", autoCreateTables: true);
                            new SGPContext().Personas.Find(1);
                            //WebSecurity.Logout();
                        }
                        else //IF DATABASE DIDN'T EXIST, CREATE DATABASE AND TABLES
                        {
                            Database.SetInitializer<SGPContext>(new CreateDatabaseIfNotExists<SGPContext>());
                            var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                            WebSecurity.InitializeDatabaseConnection("SGPContext", "Persona", "PersonaId", "UserName", autoCreateTables: true);
                            //WebSecurity.Logout();
                        }

                        context.seed();
                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }


    }
}