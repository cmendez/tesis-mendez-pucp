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

        #endregion

        #region Proveedores
        public void AgregarProveedor(Proveedor proveedor)
        {
            _db.Proveedores.Add(proveedor);
            _db.SaveChanges();
        }

        public List<TipoServicio> GetTipoServicios()
        {
            //return _db.TipoServicios.Where(t => t.IsEliminado == 0).Where(x => x.Proveedores.Count > 0).ToList(); <-- SOLO OBTENER SERVICIOS QUE TENGAN PROVEEDORES
            return _db.TipoServicios.Where(t => t.IsEliminado == 0).ToList();
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
            return _db.TipoServicios.Where(s => serviciosIds.Contains(s.TipoServicioId)).OrderBy(s => s.NombreServicio).ToList();
        }

        public List<Proveedor> GetProveedoresPorServicio(TipoServicio servicio, int cantidadMaxima, int puntajeMinimo)
        {
            return _db.Proveedores.Where(x => (x.TiposServicios.Any(r => servicio.TipoServicioId.Equals(r.TipoServicioId))))
                                    .Where(p => p.PuntuacionPromedio >= puntajeMinimo)
                                    .Where(p => (p.Persona.IsHabilitado == 1))
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

        #endregion

    }
}