using System;
using System.Threading;
using System.Web.Mvc;

namespace SistemaGeneraliz.Models.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        //private SGPContext db = new SGPContext();
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                //Database.SetInitializer<SGPContext>(new SGPContextInitializer());

                /* try
                 {
                     using (var context = new UsersContext())
                     {
                         if (!context.Database.Exists())
                         {
                             // Create the SimpleMembership database without Entity Framework migration schema
                             ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                         }
                     }

                     WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex);
                     //throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                 }/*
                 try
                 {
                     WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex);
                 }*/
            }
        }
    }
}
