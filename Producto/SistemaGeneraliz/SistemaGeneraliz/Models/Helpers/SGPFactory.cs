using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;

namespace SistemaGeneraliz.Models.Helpers
{
    public class SGPFactory : ISGPFactory
    {
        private readonly ISGPContext _db;
        public SGPFactory()
        {
            _db = new SGPContext();
        }

        public void SetEntryState(Object obj, System.Data.EntityState entityState)
        {
            _db.Entry(obj).State = entityState;
            _db.SaveChanges();
        }

        void ISGPFactory.Dispose(bool disposing)
        {
            if (_db is IDisposable)
            {
                ((IDisposable)_db).Dispose();
            }
        }

        #region Personas
        public void AgregarPersona(Persona persona)
        {
            _db.Personas.Add(persona);
            _db.SaveChanges();
        }

        public Persona GetPersonaLoggeada(int currentUserId)
        {
            return _db.Personas.First(p => p.PersonaId == currentUserId);
        }

        public List<Distrito> GetDistritos()
        {
            return _db.Distritos.Where(d => d.IsVisible == 1 && d.IsEliminado == 0).ToList();
        }

        public void AgregarUbicacion(UbicacionPersona ubicacion)
        {
            _db.UbicacionesPersonas.Add(ubicacion);
            _db.SaveChanges();
        }

        public bool ExisteDNIRUC(string dni, string ruc)
        {
            Persona p1, p2;
            bool b1, b2;
            p1 = p2 = null;
            b1 = b2 = false;

            if (!String.IsNullOrEmpty(dni))
            {
                int dniInt = Int32.Parse(dni);
                p1 = _db.Personas.FirstOrDefault(p => p.DNI == dniInt);
                if (p1 != null)
                    return true;
            }
            if (!String.IsNullOrEmpty(ruc))
            {
                long rucLong = Int64.Parse(ruc);
                p2 = _db.Personas.FirstOrDefault(p => p.RUC == rucLong);
                if (p2 != null)
                    return true;
            }
            return false;
        }

        public Distrito GetDistritoPorId(int distritoId)
        {
            return _db.Distritos.Find(distritoId);
        }

        public UbicacionPersona GetPrimeraUbicacionPersona(int idPersona)
        {
            return _db.UbicacionesPersonas.FirstOrDefault(u => u.PersonaId == idPersona);
        }

        public void HabilitarDeshabilitarUsuario(string tipoUsuario, int idUsuario, string nuevoEstado)
        {
            Object usuario = null;
            int isHabilitado = (nuevoEstado == "Habilitado") ? 1 : 0;

            switch (tipoUsuario)
            {
                case "Proveedor":
                    {
                        Proveedor proveedor = _db.Proveedores.Find(idUsuario);
                        proveedor.Persona.IsHabilitado = isHabilitado;
                        usuario = (Proveedor)proveedor;
                        _db.Proveedores.Attach(proveedor);
                        break;
                    }
                case "Cliente":
                    {
                        Cliente cliente = _db.Clientes.Find(idUsuario);
                        cliente.Persona.IsHabilitado = isHabilitado;
                        usuario = (Cliente)cliente;
                        _db.Clientes.Attach(cliente);
                        break;
                    }
                case "Suministrador":
                    {
                        Suministrador suministrador = _db.Suministradores.Find(idUsuario);
                        suministrador.Persona.IsHabilitado = isHabilitado;
                        usuario = (Suministrador)suministrador;
                        _db.Suministradores.Attach(suministrador);
                        break;
                    }
            }

            _db.Entry(usuario).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public Imagen GetImagenPorId(int imagenId)
        {
            Imagen imagen = _db.Imagenes.Find(imagenId);
            //((DbContext)_db).Dispose();
            return imagen;
        }

        public Persona GetPersonaPorUsername(string userName)
        {
            return _db.Personas.Where(p => p.UserName == userName).ToList().ElementAt(0);
        }

        public void AgregarImagen(Imagen imagen)
        {
            _db.Imagenes.Add(imagen);
            _db.SaveChanges();
        }

        public List<UbicacionPersona> GetUbicacionesPersona(int personaId)
        {
            return _db.UbicacionesPersonas.Where(u => u.PersonaId == personaId).ToList();
        }

        public void ActualizarPersona(Persona persona)
        {
            _db.Entry(persona).State = EntityState.Modified;
            _db.SaveChanges();
        }

        #endregion

        #region Proveedores
        public void AgregarProveedor(Proveedor proveedor)
        {
            _db.Proveedores.Add(proveedor);
            _db.SaveChanges();
        }

        public List<TipoServicio> GetTipoServicios()
        {
            return _db.TipoServicios.Where(t => t.IsEliminado == 0).Where(x => x.Proveedores.Count > 0).ToList(); //<-- SOLO OBTENER SERVICIOS QUE TENGAN PROVEEDORES
            //return _db.TipoServicios.Where(t => t.IsEliminado == 0).ToList();
        }

        public TipoServicio GetTipoServicioPorId(int tipoServicioId)
        {
            return _db.TipoServicios.First(t => t.TipoServicioId == tipoServicioId);
        }

        public Proveedor GetProveedorPorDocumento(long documento, int opcionDocumento)
        {
            Proveedor proveedor = null;
            if (opcionDocumento == 1) //DNI
                proveedor = _db.Proveedores.SingleOrDefault(p => p.Persona.DNI == documento);
            //{
            //    var singleOrDefault = _db.Personas.SingleOrDefault(p => p.DNI == documento);
            //    if (singleOrDefault != null)
            //        proveedor = singleOrDefault.Proveedores.SingleOrDefault();
            //}
            else if (opcionDocumento == 2) //RUC
                proveedor = _db.Proveedores.SingleOrDefault(p => p.Persona.RUC == documento);
            return proveedor;
        }

        public List<TipoServicio> GetServiciosPorIds(int[] serviciosIds)
        {
            return _db.TipoServicios.Where(s => serviciosIds.Contains(s.TipoServicioId)).Where(x => x.Proveedores.Count > 0).OrderBy(s => s.NombreServicio).ToList();
        }

        public List<Proveedor> GetProveedoresPorServicio(TipoServicio servicio, int cantidadMaxima, int puntajeMinimo)
        {
            return _db.Proveedores.Where(x => (x.TiposServicios.Any(r => servicio.TipoServicioId.Equals(r.TipoServicioId))))
                                    .Where(p => p.PuntuacionPromedio >= puntajeMinimo).Where(p => (p.Persona.IsHabilitado == 1))
                                    .Where(p => (p.LeadsDisponibles > 0))
                                    .OrderByDescending(p => p.PuntuacionPromedio).Take(cantidadMaxima).ToList();
        }

        public int GetCantidadMaximaProveedoresConfiguracion()
        {
            return _db.Configuraciones.Find(2).ValorNumerico;
        }

        public int GetPuntajeMinimoConfiguracion()
        {
            return _db.Configuraciones.Find(1).ValorNumerico;
        }

        public void ConsumirLeadsProveedor(int proveedorId, int cantidad)
        {
            Proveedor proveedor = _db.Proveedores.Find(proveedorId);
            if (proveedor.LeadsDisponibles >= cantidad)
            {
                proveedor.LeadsDisponibles = proveedor.LeadsDisponibles - cantidad;
                if (proveedor.LeadsDisponibles == 0)
                {
                    proveedor.Persona.IsHabilitado = 0;
                }
                //_db.Proveedores.Attach(proveedor);
                _db.Entry(proveedor).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public List<Proveedor> GetProveedoresServicios(int[] servicios)
        {
            return _db.Proveedores.Where(p => p.TiposServicios.Any(s => servicios.Contains(s.TipoServicioId))).Where(p => p.Persona.IsHabilitado == 1).ToList();
        }

        public Proveedor GetProveedorPorPersonaId(int idPersona)
        {
            return _db.Proveedores.FirstOrDefault(s => s.PersonaId == idPersona);
        }

        public List<TrabajoProveedor> GetHistorialTrabajos(int proveedorId)
        {
            return _db.TrabajosProveedores.Where(t => t.ProveedorId == proveedorId).OrderByDescending(t => t.Trabajo.Fecha).ToList();
        }

        public TrabajoProveedor GetTrabajoProveedor(int trabajoProveedorId)
        {
            return _db.TrabajosProveedores.Find(trabajoProveedorId);
        }

        public void ActualizarDetallesTrabajoProveedor(TrabajoProveedor trabajoProveedor)
        {
            //_db.TrabajosProveedores.Attach(trabajoProveedor);
            _db.Entry(trabajoProveedor).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            //_db.Proveedores.Attach(proveedor);
            _db.Entry(proveedor).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public int GetLeadsGratisRegistro()
        {
            return _db.Configuraciones.Find(3).ValorNumerico;
        }

        public int GetPuntuacionPromedioInicial()
        {
            return _db.Configuraciones.Find(4).ValorNumerico;
        }

        public void ActualizarUbicacion(UbicacionPersona ubicacion)
        {
            _db.Entry(ubicacion).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<TrabajoProveedor> GetHistoricoTrabajos(string fechaInicio, string fechaFin)
        {
            return _db.TrabajosProveedores.OrderByDescending(t => t.Trabajo.Fecha).ToList();
        }

        public Cliente GetClientePorId(int clienteId)
        {
            return _db.Clientes.Find(clienteId);
        }

        #endregion

        #region Clientes
        public void AgregarCliente(Cliente cliente)
        {
            _db.Clientes.Add(cliente);
            _db.SaveChanges();
        }

        public Cliente GetClientePorPersonaId(int idPersona)
        {
            return _db.Clientes.FirstOrDefault(s => s.PersonaId == idPersona);
        }

        public void AgregarTrabajo(Trabajo trabajo)
        {
            _db.Trabajos.Add(trabajo);
            _db.SaveChanges();
        }

        public void AgregarTrabajoProveedor(TrabajoProveedor trabajoProveedor)
        {
            _db.TrabajosProveedores.Add(trabajoProveedor);
            _db.SaveChanges();
        }

        public void AgregarEncuestaCliente(EncuestaCliente encuesta)
        {
            _db.EncuestasClientes.Add(encuesta);
            _db.SaveChanges();
        }

        public void ActualizarEncuestaIdTrabajoProveedor(TrabajoProveedor trabajoProveedor)
        {
            //_db.TrabajosProveedores.Attach(trabajoProveedor);
            _db.Entry(trabajoProveedor).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<TrabajoProveedor> GetTrabajosConEncuestasPendientes(int clienteId)
        {
            //b.Proveedores.Where(x => (x.TiposServicios.Any(r => servicio.TipoServicioId.Equals(r.TipoServicioId))))
            //_db.TrabajosProveedores.Where(t => t.Trabajo.ClienteId == clienteId)..ToList();
            return _db.TrabajosProveedores.Where(t => (t.EncuestaCliente.IsCompletada == 0) && (t.Trabajo.ClienteId == clienteId)).ToList();
        }

        public List<CriterioCalificacion> GetCriteriosEncuestas()
        {
            return _db.CriteriosCalificacion.Where(c => c.IsEliminado == 0).ToList();
        }

        public void AgregarRespuestasEncuesta(List<RespuestaPorCriterio> listaRespuestas)
        {
            foreach (var respuesta in listaRespuestas)
            {
                _db.RespuestasPorCriterio.Add(respuesta);
            }
            _db.SaveChanges();
        }

        public EncuestaCliente GetEncuestaCliente(int encuestaId)
        {
            return _db.EncuestasClientes.Find(encuestaId);
        }

        public void ActualizarEncuestaCompletada(EncuestaCliente encuesta)
        {
            //_db.EncuestasClientes.Attach(encuesta);
            _db.Entry(encuesta).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public int CantidadEncuestasPendientesCliente(int clienteId)
        {
            return _db.TrabajosProveedores.Count(t => (t.EncuestaCliente.IsCompletada == 0) && (t.Trabajo.ClienteId == clienteId));
        }

        #endregion

        #region Suministradores
        public void AgregarSuministrador(Suministrador suministrador)
        {
            _db.Suministradores.Add(suministrador);
            _db.SaveChanges();
        }

        public Suministrador GetSuministradorPorPersonaId(int personaId)
        {
            return _db.Suministradores.FirstOrDefault(s => s.PersonaId == personaId);
        }

        public List<RecargaLeads> GetListaRecargasSuministrador(int suministradorId)
        {
            return _db.RecargasLeads.Where(r => r.SuministradorId == suministradorId).OrderByDescending(r => r.FechaRecarga).ToList();
        }

        public void AgregarRecarga(RecargaLeads recarga)
        {
            _db.RecargasLeads.Add(recarga);
            _db.SaveChanges();
        }

        public Suministrador GetSuministrador(int idSuministrador)
        {
            return _db.Suministradores.SingleOrDefault(s => s.SuministradorId == idSuministrador);
        }

        public void ActualizarSuministrador(Suministrador suministrador)
        {
            _db.Entry(suministrador).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<Producto> GetProductosCatalogo(string nombreProducto, int categoriaId, int distritoId, int suministradorId)
        {
            IQueryable<Producto> query = _db.Productos.Where(p => p.IsEliminado == 0 && p.IsVisible == 1);
            if (!String.IsNullOrEmpty(nombreProducto))
            {
                query = query.Where(p => p.NombreCompleto.ToUpper().Contains(nombreProducto.ToUpper()));
            }
            if (categoriaId != -1)
            {
                query = query.Where(p => p.CategoriaProductoId == categoriaId);
            }
            if (distritoId != -1)
            {
                //(x => (x.TiposServicios.Any(r => servicio.TipoServicioId.Equals(r.TipoServicioId))))
                query = query.Where(p => p.Suministrador.Persona.UbicacionesPersonas.Any(r => distritoId.Equals(r.DistritoId)));
            }
            if (suministradorId != -1)
            {
                //(x => (x.TiposServicios.Any(r => servicio.TipoServicioId.Equals(r.TipoServicioId))))
                query = query.Where(p => p.Suministrador.SuministradorId == suministradorId);
            }
            //return query.OrderBy(r => Guid.NewGuid()).Take(15).ToList();
            return query.OrderBy(r => Guid.NewGuid()).ToList();
        }

        public List<CategoriaProducto> GetCategoriasProducto()
        {
            return _db.CategoriasProducto.Where(c => c.IsEliminado == 0).OrderBy(c => c.NombreCategoria).ToList();
        }

        public Producto GetProducto(int productoId)
        {
            return _db.Productos.Find(productoId);
        }

        public List<Producto> GetProductosSuministradorCatalogo(int suministradorId)
        {
            return _db.Productos.Where(p => p.SuministradorId == suministradorId && p.IsEliminado == 0).ToList();
        }

        public void AgregarProducto(Producto producto)
        {
            _db.Productos.Add(producto);
            _db.SaveChanges();
        }

        public void ModificarProducto(Producto producto)
        {
            //_db.Productos.Attach(producto);
            _db.Entry(producto).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<Suministrador> GetSuministradores()
        {
            return _db.Suministradores.Where(s => s.Persona.IsHabilitado == 1 && s.Persona.IsEliminado == 0).ToList();
        }

        public List<OfertaPromoDscto> GetOfertasPromosDsctosCatalogo()
        {
            IQueryable<OfertaPromoDscto> query = _db.OfertasPromosDsctos.Where(p => (p.CantidadDisponible > 0) && (p.IsEliminado == 0) && (p.IsVisible == 1));
            //return query.OrderBy(r => Guid.NewGuid()).ToList();
            return query.ToList();
        }

        public OfertaPromoDscto GetOfertaPromoDscto(int ofertaPromoDsctoId)
        {
            return _db.OfertasPromosDsctos.Find(ofertaPromoDsctoId);
        }

        public void AgregarCompraVirtual(CompraVirtual compraVirtual)
        {
            _db.ComprasVirtuales.Add(compraVirtual);
            _db.SaveChanges();
        }

        public void ActualizarOfertaPromoDscto(OfertaPromoDscto ofertaPromoDscto)
        {
            _db.Entry(ofertaPromoDscto).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<OfertaPromoDscto> GetOfertasPromosDsctosSuministradorCatalogo(int suministradorId)
        {
            return _db.OfertasPromosDsctos.Where(p => p.SuministradorId == suministradorId && p.IsEliminado == 0).ToList();
        }

        public void AgregarOfertaPromoDscto(OfertaPromoDscto ofertaPromoDscto)
        {
            _db.OfertasPromosDsctos.Add(ofertaPromoDscto);
            _db.SaveChanges();
        }

        public void ModificarOfertaPromoDscto(OfertaPromoDscto ofertaPromoDscto)
        {
            _db.Entry(ofertaPromoDscto).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<OfertaPromoDscto> Demanda_OfertasPromosDsctos()
        {
            IQueryable<OfertaPromoDscto> query = _db.OfertasPromosDsctos;
            return query.ToList();
        }

        public List<Suministrador> GetTodosSuministradores()
        {
            return _db.Suministradores.ToList();
        }

        public int MontoComprasLogradasSuministrador(int suministradorId, int month, int year)
        {
            var lista = _db.ComprasVirtuales.Where(c => (c.OfertaPromoDscto.SuministradorId == suministradorId) && (c.FechaCompra.Year == year) &&
                         (c.FechaCompra.Month == month)).ToList();
            int suma = 0;
            if ((lista != null) && (lista.Count>0))
                suma = lista.Sum(s=>s.LeadsPagados);
            return suma;
        }

        #endregion

    }
}