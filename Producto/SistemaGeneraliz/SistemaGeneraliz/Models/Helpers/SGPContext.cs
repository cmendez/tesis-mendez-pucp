using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Security;
using SGP.Models.Helpers;
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

        //public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<Persona> Personas { get; set; }
        /*
        //REGION OF DBSET's
        #region DbSet's
        public IDbSet<DomainModels> Domains { get; set; }
        
        public IDbSet<LocationModels> Locations { get; set; }
        public IDbSet<RoomModels> Rooms { get; set; }
        public IDbSet<ResourceModels> Resources { get; set; }
        public IDbSet<ReservationModels> Reservations { get; set; }
        #endregion

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

        internal void seed()
        {
            //Seed of Roles & Permissions
            //Administrator
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");

            if (!WebSecurity.UserExists("admin"))
                WebSecurity.CreateUserAndAccount(
                    "admin",
                    "admin", propertyValues: new
                                                 {
                                                     GivenName = "Administrator",
                                                     DisplayName = "Default Administrator"
                                                 });

            if (!Roles.GetRolesForUser("admin").Contains("Administrator"))
                Roles.AddUsersToRoles(new[] {"admin"}, new[] {"Administrator"});

            //Regular User
            if (!Roles.RoleExists("User"))
                Roles.CreateRole("User");
        }

        //    //Seed of Domains
        //    List<DomainModels> listDomains = Domains.ToList();
        //    if ((listDomains == null) || (listDomains.Count == 0))
        //    {

        //        DomainModels domain = new DomainModels
        //        {
        //            DomaindId = 1,
        //            DomainName = "belatrix.com",
        //            DomainIP = "192.168.6.3"
        //        };
        //        Domains.Add(domain);

        //        domain = new DomainModels
        //        {
        //            DomaindId = 2,
        //            DomainName = "belatrixperu.com",
        //            DomainIP = "192.168.1.2"
        //        };
        //        Domains.Add(domain);

        //        SaveChanges();
        //    }

        //    //Seed of Locations
        //    List<LocationModels> listLocations = Locations.ToList();
        //    if ((listLocations == null) || (listLocations.Count == 0))
        //    {

        //        LocationModels location = new LocationModels
        //        {
        //            LocationId = 1,
        //            CountryName = "Argentina",
        //            CityName = "Mendoza",
        //            DistrictName = "Centro",
        //            isDeleted = 0
        //        };
        //        Locations.Add(location);

        //        location = new LocationModels
        //        {
        //            LocationId = 2,
        //            CountryName = "Argentina",
        //            CityName = "Mendoza",
        //            DistrictName = "Chacras",
        //            isDeleted = 0
        //        };
        //        Locations.Add(location);

        //        location = new LocationModels
        //        {
        //            LocationId = 3,
        //            CountryName = "Perú",
        //            CityName = "Lima",
        //            DistrictName = "San Isidro",
        //            isDeleted = 0
        //        };
        //        Locations.Add(location);

        //        SaveChanges();
        //    }

        //    //Seed of Rooms
        //    var listRooms = new List<RoomModels>()
        //    {
        //        new RoomModels { RoomName = "Sala Amazonas", LocationId = 3, Description = "Sala de 8 personas", isDeleted = 0, RoomFullName = "Lima - Sala Amazonas" },
        //        new RoomModels { RoomName = "Sala Nazca", LocationId = 3, Description = "Sala de 7 personas", isDeleted = 0, RoomFullName = "Lima - Sala Nazca" },
        //        new RoomModels { RoomName = "Sala Cuzco", LocationId = 3, Description = "Sala de 8 personas", isDeleted = 0, RoomFullName = "Lima - Sala cuzco" },
        //        new RoomModels { RoomName = "Sala Centro1", LocationId = 1, Description = "Sala de 6 personas", isDeleted = 0, RoomFullName = "Centro - Sala Centro1" },
        //        new RoomModels { RoomName = "Sala Centro2", LocationId = 1, Description = "Sala de 8 personas", isDeleted = 0, RoomFullName = "Centro - Sala Centro2" },
        //        new RoomModels { RoomName = "Sala Centro3", LocationId = 1, Description = "Sala de 8 por personas", isDeleted = 0, RoomFullName = "Centro - Sala Centro3" },
        //        new RoomModels { RoomName = "Sala Chacras1", LocationId = 2, Description = "Sala de 6 personas", isDeleted = 0, RoomFullName = "Chacras - Sala Chacras1" },
        //        new RoomModels { RoomName = "Sala Chacras2", LocationId = 2, Description = "Sala de 5 personas", isDeleted = 0, RoomFullName = "Chacras - Sala Chacras2" },
        //        new RoomModels { RoomName = "Sala Chacras3", LocationId = 2, Description = "Sala de 7 personas", isDeleted = 0, RoomFullName = "Chacras - Sala Chacras3" }             
        //    };
        //    listRooms.ForEach(s => this.Rooms.Add(s));
        //    this.SaveChanges();

        //    //Seed of Resources
        //    var listResources = new List<ResourceModels>()
        //    {
        //        new ResourceModels { ResourceName = "Proyector Sala Amazonas", LocationId = 3, Description = "Proyector Sala de 8 personas", isDeleted = 0, ResourceFullName = "Lima - Proyector Sala Amazonas" },
        //        new ResourceModels { ResourceName = "Proyector Sala Nazca", LocationId = 3, Description = "Proyector Sala de 7 personas", isDeleted = 0, ResourceFullName = "Lima - Proyector Sala Nazca" },
        //        new ResourceModels { ResourceName = "Proyector Sala Centro1", LocationId = 1, Description = "Proyector Sala de 6 personas", isDeleted = 0, ResourceFullName = "Centro - Proyector Sala Centro1" },
        //        new ResourceModels { ResourceName = "Proyector Sala Centro2", LocationId = 1, Description = "Proyector Sala de 8 personas", isDeleted = 0, ResourceFullName = "Centro - Proyector Sala Centro2" },
        //        new ResourceModels { ResourceName = "Proyector Sala Chacras1", LocationId = 2, Description = "Proyector Sala de 6 personas", isDeleted = 0, ResourceFullName = "Chacras - Proyector Sala Chacras1" },
        //        new ResourceModels { ResourceName = "Proyector Sala Chacras2", LocationId = 2, Description = "Proyector Sala de 5 personas", isDeleted = 0, ResourceFullName = "Chacras - Proyector Sala Chacras2" }

        //    };
        //    listResources.ForEach(s => this.Resources.Add(s));
        //    this.SaveChanges();
        //}
        //



        public class SGPContextInitializer : DropOnlyTables<SGPContext>
        {
            /*protected override void Seed(SGPContext context)
            {
                base.Seed(context);
                //context.seed();
            }*/
        }

        //public class SGPContextInitializer : DropCreateDatabaseIfModelChanges<SGPContext>
        //public class SGPContextInitializer : DropCreateDatabaseTables<SGPContext>
        //public class SGPContextInitializer : DropCreateDatabaseAlways<SGPContext>
        //public class SGPContextInitializer : DropOnlyTables<SGPContext>
        //{
        //    /*protected override void Seed(SGPContext context)
        //    {
        //        base.Seed(context);
        //        //context.seed();
        //    }*/
        //}
    }
}