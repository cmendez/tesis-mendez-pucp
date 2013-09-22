using System;
using System.Collections.Generic;
using System.Linq;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.Helpers
{
    public interface ISGPFactory
    {
        void Dispose(bool disposing);
        void SetEntryState(Object res, System.Data.EntityState entityState);
        /*
        #region Reservations
            ReservationModels GetLastReservation();
            IList<ReservationModels> GetReservationsByRange(DateTime startDate, DateTime endDate);
            ReservationModels GetReservationByCalendarId(int calendarId);
            void AddReservation(ReservationModels reservationModels);
            ReservationModels GetReservationById(int id);
            IQueryable<ReservationModels> GetReservationsByGrupalId(int grupalId);
            ReservationModels GetFirstReservationOnSeries(int grupalId, DateTime now);
            List<ReservationModels> CheckDoubleBooking(DateTime pStartTime, DateTime pEndTime, int[] roomsIds, int[] resourcesIds, int grupalIdDoubleBooking);
            int CountActiveReservationsInSeries(int grupalId);
        #endregion

        #region Locations
            IQueryable<LocationModels> GetAllActiveLocations();
            IQueryable<LocationModels> GetAllLocations();
            LocationModels GetLocationById(int locationId);    
            void AddLocation(LocationModels locationModels);
        #endregion
        
        #region Rooms
            IQueryable<RoomModels> GetAllRooms();
            void AddRoom(RoomModels roomModels);
            IList<RoomModels> GetRoomModelsByCalendarId(int calendarId);
            IQueryable<RoomModels> GetAllActiveRooms();
            RoomModels GetRoomById(int roomId);
            IQueryable<RoomModels> GetAllActiveRoomsByLocationId(int id);
        #endregion

        #region Resources
            IQueryable<ResourceModels> GetAllResources();
            void AddResource(ResourceModels resourceModels);
            IList<ResourceModels> GetResourcesByCalendarId(int calendarId);
            IQueryable<ResourceModels> GetAllActiveResources();
            ResourceModels GetResourceById(int resourceId);
            IQueryable<ResourceModels> GetAllActiveResourcesByLocationId(int id);
        #endregion

        #region UserProfile
            UserProfile GetUserById(int userId);
            IQueryable<UserProfile> GetAllUsers();
        #endregion

        #region Domains
            IQueryable<DomainModels> GetAllDomains();
            DomainModels GetDomainById(int domainId);
            void AddDomain(DomainModels domainModels);
            int CountDomainsSameNameInDB(int domainId, string domainName);
            void DeleteDomain(DomainModels domainmodels);
        #endregion*/

        #region Personas
        void AgregarPersona(Persona persona);
        Persona GetPersonaLoggeada(int currentUserId);
        #endregion

        #region Proveedores
        void AgregarProveedor(Proveedor proveedor);
        #endregion

        #region Clientes
        void AgregarCliente(Cliente Cliente);
        #endregion



        
    }
}
