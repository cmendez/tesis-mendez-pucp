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
                using (var context = new SGPContext())
                {
                    if (false)
                    {
                        //IF DATABASE ALREADY EXISTED, ONLY DROP TABLES AND RECREATE THEM
                        if (context.Database.Exists())
                        {
                            Database.SetInitializer<SGPContext>(new DropOnlyTables<SGPContext>());
                            var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                            string script = dbCreationScript.Replace("on delete cascade", "");
                            context.Database.ExecuteSqlCommand(script);
                            //context.Database.ExecuteSqlCommand("ALTER TABLE TrabajosProveedores ADD CONSTRAINT uc_EncuestaCliente UNIQUE(EncuestaClienteId)");
                            WebSecurity.InitializeDatabaseConnection("SGPContext", "Personas", "PersonaId", "UserName", autoCreateTables: true);
                            //if (WebSecurity.IsAuthenticated)
                            //    WebSecurity.Logout();
                        }
                        else //IF DATABASE DIDN'T EXIST, CREATE DATABASE AND TABLES
                        {
                            Database.SetInitializer<SGPContext>(new CreateDatabaseIfNotExists<SGPContext>());
                            var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                            WebSecurity.InitializeDatabaseConnection("SGPContext", "Personas", "PersonaId", "UserName", autoCreateTables: true);
                            //if (WebSecurity.IsAuthenticated) 
                            //    WebSecurity.Logout();
                        }

                        context.Seed();
                        //context.SaveChanges();
                    }
                    else
                    {
                        Database.SetInitializer<SGPContext>(null);
                        WebSecurity.InitializeDatabaseConnection("SGPContext", "Personas", "PersonaId", "UserName", autoCreateTables: true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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