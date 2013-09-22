using System;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Data.Entity;
using System.Web.Security;
using SistemaGeneraliz.Models.Entities;
using WebMatrix.WebData;

namespace SistemaGeneraliz.Models.Helpers
{
    public class SGPContext : DbContext, ISGPContext
    {
        public SGPContext()
            : base("SGPContext")
        {
        }

        #region DbSet's
        //public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<Persona> Personas { get; set; }
        public IDbSet<Cliente> Clientes { get; set; }
        public IDbSet<Proveedor> Proveedores { get; set; }
        public IDbSet<Suministrador> Suministradores { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        /*   
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReservationModels>()
                .HasMany(r => r.Rooms).WithMany(t => t.Reservations)
                .Map(t => t.MapLeftKey("CalendarId")
                    .MapRightKey("RoomId")
                    .ToTable("RoomsByReservation"));

            modelBuilder.Entity<ReservationModels>()
                .HasMany(r => r.Resources).WithMany(t => t.Reservations)
                .Map(t => t.MapLeftKey("CalendarId")
                    .MapRightKey("ResourceId")
                    .ToTable("ResourcesByReservation"));
        }
        */

        #region Seeds
        internal void seed()
        {
            //Seed of Roles & Permissions
            //Administrator
            if (!Roles.RoleExists("Administrador"))
                Roles.CreateRole("Administrador");

            if (!WebSecurity.UserExists("46394691"))
            {
                WebSecurity.CreateUserAndAccount("46394691", "admin", propertyValues: new
                {
                    TipoPersona = "Natural",
                    TipoUsuario = "Administrador",
                    PrimerNombre = "Christian",
                    ApellidoPaterno = "Mendez",
                    DNI = 46394691,
                    UltimaActualizacionPersonal = DateTime.Now,
                    IsHabilitado = 0,
                    IsEliminado = 0
                });
            }

            if (!Roles.GetRolesForUser("46394691").Contains("Administrador"))
                Roles.AddUsersToRoles(new[] { "46394691" }, new[] { "Administrador" });

            if (!Roles.RoleExists("Cliente"))
                Roles.CreateRole("Cliente");
            if (!Roles.RoleExists("Proveedor"))
                Roles.CreateRole("Proveedor");
            if (!Roles.RoleExists("Suministrador"))
                Roles.CreateRole("Suministrador");
        }
        #endregion


    }

    //public class SGPContextInitializer : DropCreateDatabaseTables<SGPContext>
    //public class SGPContextInitializer : DropCreateDatabaseAlways<SGPContext>
    //public class SGPContextInitializer : DropOnlyTables<SGPContext>
    public class SGPContextInitializer : DropCreateDatabaseIfModelChanges<SGPContext>
    {
        /*protected override void Seed(SGPContext context)
        {
            base.Seed(context);
            //context.seed();
        }*/
    }
}