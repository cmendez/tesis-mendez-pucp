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
        IDbSet<RecargaLeads> RecargasLeads { get; set; }
        IDbSet<Configuracion> Configuraciones { get; set; }
        IDbSet<Trabajo> Trabajos { get; set; }
        IDbSet<TrabajoProveedor> TrabajosProveedores { get; set; }
        IDbSet<EncuestaCliente> EncuestasClientes { get; set; }
        IDbSet<RespuestaPorCriterio> RespuestasPorCriterio { get; set; }
        IDbSet<CriterioCalificacion> CriteriosCalificacion { get; set; }
        IDbSet<PuntajePromedioCriterio> PuntajesPromedioCriterio { get; set; }
        IDbSet<Imagen> Imagenes { get; set; }
        IDbSet<Producto> Productos { get; set; }
        IDbSet<CategoriaProducto> CategoriasProducto { get; set; }
        //IDbSet<SubcategoriaProducto> SubcategoriasProducto { get; set; }
        int SaveChanges();
        DbEntityEntry Entry(object entity);
    }
}
