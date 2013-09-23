using System;
using System.Collections.Generic;
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
        #endregion

        #region Proveedores
        public void AgregarProveedor(Proveedor proveedor)
        {
            _db.Proveedores.Add(proveedor);
            _db.SaveChanges();
        }

        public List<TipoServicio> GetTipoServicios()
        {
            return _db.TipoServicios.Where(t => t.IsEliminado == 0).ToList();
        }

        public TipoServicio GetTipoServicioPorId(int tipoServicioId)
        {
            return _db.TipoServicios.First(t => t.TipoServicioId == tipoServicioId);
        }

        #endregion

        #region Clientes
        public void AgregarCliente(Cliente cliente)
        {
            _db.Clientes.Add(cliente);
            _db.SaveChanges();
        }
        #endregion

        #region Suministradores
        public void AgregarSuministrador(Suministrador suministrador)
        {
            _db.Suministradores.Add(suministrador);
            _db.SaveChanges();
        }
        #endregion

        /*
        #region Reservations
            public ReservationModels GetLastReservation()
            {
                return _db.Reservations.OrderByDescending(r => r.GrupalId).FirstOrDefault();
            }
            public IList<ReservationModels> GetReservationsByRange(DateTime startDate, DateTime endDate)
            {
                return (from l in _db.Reservations
                        where l.StartTime >= startDate
                        && l.EndTime <= endDate
                        && l.IsDeleted == 0
                        orderby l.StartTime ascending, l.EndTime ascending 
                        select l).ToList();
            }            
            
            public ReservationModels GetReservationByCalendarId(int calendarId)
            {
                return _db.Reservations.Find(calendarId);
            }
            public void AddReservation(ReservationModels reservationModels)
            {
                _db.Reservations.Add(reservationModels);
                _db.SaveChanges();
            }
            public ReservationModels GetReservationById(int id)
            {
                return _db.Reservations.Find(id);
            }

            public ReservationModels GetFirstReservationOnSeries(int grupalId, DateTime now)
            {
                return _db.Reservations.Where(r => r.IsDeleted == 0).Where(r => r.GrupalId == grupalId).Where(r => r.StartTime >= now).FirstOrDefault();
            }

            public List<ReservationModels> CheckDoubleBooking(DateTime pStartTime, DateTime pEndTime, int[] roomsIds, int[] resourcesIds, int grupalIdDoubleBooking)
            {
                if (resourcesIds[0] != -1)
                {
                    return _db.Reservations.Where(r => ((r.StartTime <= pStartTime) && (pStartTime < r.EndTime))
                                                   || ((r.StartTime < pEndTime) && (pEndTime <= r.EndTime))
                                                   || ((pStartTime < r.StartTime) && (r.EndTime < pEndTime))
                                                   || ((r.StartTime < pStartTime) && (pEndTime < r.EndTime))).Where(x => (x.Rooms.Any(r => roomsIds.Contains(r.RoomId)))
                                                   || (x.Resources.Any(r => resourcesIds.Contains(r.ResourceId)))).Where(r => r.IsDeleted == 0).Where(r => r.GrupalId != grupalIdDoubleBooking).ToList();

                }
                else
                {
                    return _db.Reservations.Where(r => ((r.StartTime <= pStartTime) && (pStartTime < r.EndTime))
                                                   || ((r.StartTime < pEndTime) && (pEndTime <= r.EndTime))
                                                   || ((pStartTime < r.StartTime) && (r.EndTime < pEndTime))
                                                   || ((r.StartTime < pStartTime) && (pEndTime < r.EndTime))).Where(x => x.Rooms.Any(r => roomsIds.Contains(r.RoomId))).Where(r => r.IsDeleted == 0).Where(r => r.GrupalId != grupalIdDoubleBooking).ToList();
                }
            }

            public IQueryable<ReservationModels> GetReservationsByGrupalId(int grupalId)
            {
                return _db.Reservations.Where(r => r.GrupalId == grupalId);
            }

            public int CountActiveReservationsInSeries(int grupalId)
            {
                return _db.Reservations.Where(r => r.GrupalId == grupalId).Where(r => r.IsDeleted == 0).Count();
            }
        #endregion

        #region Locations
            
            public IQueryable<LocationModels> GetAllActiveLocations()
            {
                return _db.Locations.Where(x => x.isDeleted == 0);
            }
            public IQueryable<LocationModels> GetAllLocations()
            {
                return _db.Locations;
            }
            public LocationModels GetLocationById(int locationId)
            {
                return _db.Locations.Single(x => x.LocationId == locationId);
            }
            public void AddLocation(LocationModels locationModels)
            {
                _db.Locations.Add(locationModels);
                _db.SaveChanges();
            }
        #endregion

        #region Rooms
            public IList<RoomModels> GetRoomModelsByCalendarId(int calendarId)
            {
                return _db.Rooms.Where(r => r.Reservations.Any(t => t.CalendarId == calendarId)).OrderByDescending(r => r.RoomFullName).ToList();
            }
            public IQueryable<RoomModels> GetAllActiveRooms()
            {
                return _db.Rooms.Where(x => x.isDeleted == 0);
            }
            public IQueryable<RoomModels> GetAllRooms()
            {
                return _db.Rooms.Include(r => r.Location);
            }
            public RoomModels GetRoomById(int roomId)
            {
                return _db.Rooms.Single(r => r.RoomId == roomId);
            }
            public void AddRoom(RoomModels roomModels)
            {
                _db.Rooms.Add(roomModels);
                _db.SaveChanges();
            }
            public IQueryable<RoomModels> GetAllActiveRoomsByLocationId(int id)
            {
                return _db.Rooms.Where(x => x.LocationId == id && x.isDeleted == 0);
            }
        #endregion

        #region Resources
            public IList<ResourceModels> GetResourcesByCalendarId(int calendarId)
            {
                return _db.Resources.Where(r => r.Reservations.Any(t => t.CalendarId == calendarId)).OrderByDescending(r => r.ResourceFullName).ToList();
            }
            public IQueryable<ResourceModels> GetAllActiveResources()
            {
                return _db.Resources.Where(x => x.isDeleted == 0);
            }
            public IQueryable<ResourceModels> GetAllResources()
            {
                return _db.Resources.Include(r => r.Location);
            }
            public ResourceModels GetResourceById(int resourceId)
            {
                return _db.Resources.Single(r => r.ResourceId == resourceId);
            }
            public void AddResource(ResourceModels resourceModels)
            {
                _db.Resources.Add(resourceModels);
                _db.SaveChanges();
            }
            public IQueryable<ResourceModels> GetAllActiveResourcesByLocationId(int id)
            {
                return _db.Resources.Where(x => x.LocationId == id && x.isDeleted == 0);
            }
        #endregion

        #region UserProfile
            public UserProfile GetUserById(int userId)
            {
                return _db.UserProfiles.Single(u => u.UserId == userId);
            }
            public IQueryable<UserProfile> GetAllUsers()
            {
                return (from u in _db.UserProfiles orderby u.GivenName ascending select u);
            }
        #endregion

        #region Domains
            public IQueryable<DomainModels> GetAllDomains()
            {
                return _db.Domains;
            }
            public DomainModels GetDomainById(int domainId)
            {
                return _db.Domains.Single(x => x.DomaindId == domainId);
            }
            public void AddDomain(DomainModels domainModels)
            {
                _db.Domains.Add(domainModels);
                _db.SaveChanges();
            }
            public int CountDomainsSameNameInDB(int domainId, string domainName)
            {
                return _db.Domains.Where(d => d.DomaindId != domainId).Count(d => d.DomainName.Equals(domainName));
            }
            public void DeleteDomain(DomainModels domainmodels)
            {
                _db.Domains.Remove(domainmodels);
                _db.SaveChanges();
            }
        #endregion*/
    }
}