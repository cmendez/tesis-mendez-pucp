using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.Helpers
{
    public interface ISGPContext
    {
        IDbSet<Persona> Personas { get; set; }
        IDbSet<Cliente> Clientes { get; set; }
        IDbSet<Proveedor> Proveedores { get; set; }
        IDbSet<Suministrador> Suministradores { get; set; }
        IDbSet<TipoServicio> TipoServicios { get; set; }
        IDbSet<PaisCiudad> PaisesCiudades { get; set; }
        IDbSet<Distrito> Distritos { get; set; }
        IDbSet<UbicacionPersona> UbicacionesPersonas { get; set; }

        int SaveChanges();
        DbEntityEntry Entry(object entity);
    }
}
