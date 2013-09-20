using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace SistemaGeneraliz.Models.Helpers
{
    public class DropCreateDatabaseTables<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        public void InitializeDatabase(TContext context)
        {
            try
            {
                if (!context.Database.Exists())
                {
                    // Create the SimpleMembership database without Entity Framework migration schema
                    ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                }
                
                // if the context database exists
                if (context.Database.Exists())
                {
                    // if model has changed
                    if (!context.Database.CompatibleWithModel(false))
                    {
                        // remove all tables
                        this.DropAllTables(context);

                        // create all model tables
                        var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                        context.Database.ExecuteSqlCommand(dbCreationScript);

                        // create meta data table
                        this.CreateMetaDataTable(context);

                        // seed initialization data
                        //Seed(context);

                        // save
                        context.SaveChanges();
                    }
                }
                else
                {
                    string dbName = context.Database.Connection.Database;
                    string error = String.Format("Database {0} does not exist!", dbName);

                    
                }
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
        
        /// This assumes you are using Entity Framework Code-First 4.3.x or greater.
        /// This uses the __MigrationHistory table NOT the old EdmMetaData table.        
        
        private void CreateMetaDataTable(TContext context)
        {
            // create meta data table DDL
            string sql = @"CREATE TABLE dbo.__MigrationHistory (
                                MigrationId NVARCHAR(255) NOT NULL,
                                CreatedOn DATETIME NOT NULL,
                                Model VARBINARY(MAX) NOT NULL,
                                ProductVersion NVARCHAR(32) NOT NULL);
  
                            ALTER TABLE dbo.__MigrationHistory ADD PRIMARY KEY (MigrationId);
  
                            INSERT INTO dbo.__MigrationHistory (MigrationId, CreatedOn, Model, ProductVersion)
                            VALUES ('InitialCreate', GetDate(), @p0, @p1);";
 
            // execute SQL command
            context.Database.ExecuteSqlCommand(sql, GetModel(context), GetProductVersion());
        }
 
        private byte[] GetModel(TContext context)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    using (var xmlWriter = XmlWriter.Create(gzipStream, new XmlWriterSettings { Indent = true }))
                    {
                        EdmxWriter.WriteEdmx(context, xmlWriter);
                    }
                }
 
                return memoryStream.ToArray();
            }
        }
 
        private string GetProductVersion()
        {
            return typeof(DbContext).Assembly
                                    .GetCustomAttributes(false)
                                    .OfType<AssemblyInformationalVersionAttribute>()
                                    .Single()
                                    .InformationalVersion;
        }
 
        protected virtual void Seed(TContext context)
        {
            /// Let your Database Initializer class handle the seeding of data
        }
    }
}