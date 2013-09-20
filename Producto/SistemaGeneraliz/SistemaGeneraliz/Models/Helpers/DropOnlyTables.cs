using System;
using System.Data.Entity;

namespace SistemaGeneraliz.Models.Helpers
{
    public class DropOnlyTables<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        public void InitializeDatabase(TContext context)
        {
            try
            {
                //remove all tables manually
                /*string dropTables = "USE SGPlocal;" +
                                    "DROP TABLE [dbo].[webpages_UsersInRoles];" +
                                    "DROP TABLE [dbo].[webpages_OAuthMembership];" +
                                    "DROP TABLE [dbo].[webpages_Membership];" +
                                    "DROP TABLE [dbo].[webpages_Roles];" +
                                    "DROP TABLE [dbo].[UserProfile];" +
                                    "DROP TABLE [dbo].[Domain];";

                context.Database.ExecuteSqlCommand(dropTables);*/

                // remove all tables automatically
                this.DropAllTables(context);

                //var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
 
        private void DropAllTables(TContext context)
        {
            // disable all foreign keys
            context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? NOCHECK CONSTRAINT all'");
 
            bool tryAgain = true;
 
            // need to perform multiple drop attempts due to the possibility of linked foreign key data
            while (tryAgain)
            {
                try
                {
                    // drop tables
                    context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = 'DROP TABLE ?'");
 
                    // remove try again flag
                    tryAgain = false;
                }
                catch { } // ignore errors as these are expected due to linked foreign key data
            }
        } 
    }
}