using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.Helpers
{
    public interface ISGPContext
    {
        //IDbSet<UserProfile> UserProfiles { get; set; }
        IDbSet<Persona> Personas { get; set; }
        IDbSet<Cliente> Clientes { get; set; }
        IDbSet<Proveedor> Proveedores { get; set; }
        IDbSet<Suministrador> Suministradores { get; set; }

        /*IDbSet<DomainModels> Domains { get; set; }
       
       IDbSet<LocationModels> Locations { get; set; }
       IDbSet<RoomModels> Rooms { get; set; }
       IDbSet<ResourceModels> Resources { get; set; }
       IDbSet<ReservationModels> Reservations { get; set; }*/
        int SaveChanges();
        DbEntityEntry Entry(object entity);
    }
}
