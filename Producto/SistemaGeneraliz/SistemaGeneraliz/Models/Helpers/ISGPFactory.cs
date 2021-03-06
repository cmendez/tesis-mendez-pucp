﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SistemaGeneraliz.Models.Entities;

namespace SistemaGeneraliz.Models.Helpers
{
    public interface ISGPFactory
    {
        void Dispose(bool disposing);
        void SetEntryState(Object res, System.Data.EntityState entityState);

        #region Personas
        void AgregarPersona(Persona persona);
        Persona GetPersonaLoggeada(int currentUserId);
        List<Distrito> GetDistritos();
        void AgregarUbicacion(UbicacionPersona ubicacion);
        bool ExisteDNIRUC(string dni, string ruc);
        Distrito GetDistritoPorId(int distritoId);
        UbicacionPersona GetPrimeraUbicacionPersona(int idPersona);
        void HabilitarDeshabilitarUsuario(string tipoUsuario, int idUsuario, string nuevoEstado);
        void CambiarEstadoUsuario(int usuarioId, string nuevoEstado);
        Imagen GetImagenPorId(int imagenId);
        Persona GetPersonaPorUsername(string userName);
        void AgregarImagen(Imagen imagen);
        List<UbicacionPersona> GetUbicacionesPersona(int personaId);
        void ActualizarPersona(Persona persona);
        List<Persona> GetTodasLasPersonasSistema();
        List<Configuracion> GetConfiguraciones();
        void ActualizarConfiguraciones(int[] valores);
        #endregion

        #region Proveedores
        void AgregarProveedor(Proveedor proveedor);
        List<TipoServicio> GetTipoServicios();
        TipoServicio GetTipoServicioPorId(int tipoServicioId);
        Proveedor GetProveedorPorDocumento(long documento, int opcionDocumento);
        List<TipoServicio> GetServiciosPorIds(int[] serviciosIds);
        List<Proveedor> GetProveedoresPorServicio(TipoServicio servicio, int cantidadMaxima, int puntajeMinimo);
        int GetCantidadMaximaProveedoresConfiguracion();
        int GetPuntajeMinimoConfiguracion();
        void ConsumirLeadsProveedor(int proveedorId, int cantidad);
        List<Proveedor> GetProveedoresServicios(int[] servicios);
        Proveedor GetProveedorPorPersonaId(int idPersona);
        List<TrabajoProveedor> GetHistorialTrabajos(int proveedorId);
        TrabajoProveedor GetTrabajoProveedor(int trabajoProveedorId);
        void ActualizarDetallesTrabajoProveedor(TrabajoProveedor trabajoProveedor);
        void ActualizarProveedor(Proveedor proveedor);
        int GetLeadsGratisRegistro();
        int GetPuntuacionPromedioInicial();
        void ActualizarUbicacion(UbicacionPersona ubicacion);
        List<TrabajoProveedor> GetHistoricoTrabajos(string fechaInicio, string fechaFin);
        List<Proveedor> GetProveedores();
        void ActualizarListaProveedores(List<Proveedor> proveedores);
        int GetNroLeadsRecompensa();
        int GetNroProveedoresRecompensa();
        void ActualizarLeadsProveedor(int idProveedor, int monto);
        #endregion

        #region Clientes
        Cliente GetClientePorId(int clienteId);
        void AgregarCliente(Cliente cliente);
        Cliente GetClientePorPersonaId(int idPersona);
        void AgregarTrabajo(Trabajo trabajo);
        void AgregarTrabajoProveedor(TrabajoProveedor trabajoProveedor);
        void AgregarEncuestaCliente(EncuestaCliente encuesta);
        void ActualizarEncuestaIdTrabajoProveedor(TrabajoProveedor trabajoProveedor);
        List<TrabajoProveedor> GetTrabajosConEncuestasPendientes(int clienteId);
        List<CriterioCalificacion> GetCriteriosEncuestas();
        void AgregarRespuestasEncuesta(List<RespuestaPorCriterio> listaRespuestas);
        EncuestaCliente GetEncuestaCliente(int encuestaId);
        void ActualizarEncuestaCompletada(EncuestaCliente encuesta);
        int CantidadEncuestasPendientesCliente(int clienteId);
        #endregion

        #region Suministradores
        void AgregarSuministrador(Suministrador suministrador);
        Suministrador GetSuministradorPorPersonaId(int personaId);
        List<RecargaLeads> GetListaRecargasSuministrador(int suministradorId);
        void AgregarRecarga(RecargaLeads recarga);
        Suministrador GetSuministrador(int idSuministrador);
        void ActualizarSuministrador(Suministrador suministrador);
        List<Producto> GetProductosCatalogo(string nombreProducto, int categoriaId, int distritoId, int suministradorId);
        List<CategoriaProducto> GetCategoriasProducto();
        Producto GetProducto(int productoId);
        List<Producto> GetProductosSuministradorCatalogo(int suministradorId);
        void AgregarProducto(Producto producto);
        void ModificarProducto(Producto producto);
        List<Suministrador> GetSuministradores();
        List<OfertaPromoDscto> GetOfertasPromosDsctosCatalogo();
        OfertaPromoDscto GetOfertaPromoDscto(int ofertaPromoDsctoId);
        void AgregarCompraVirtual(CompraVirtual compraVirtual);
        void ActualizarOfertaPromoDscto(OfertaPromoDscto ofertaPromoDscto);
        List<OfertaPromoDscto> GetOfertasPromosDsctosSuministradorCatalogo(int suministradorId);
        void AgregarOfertaPromoDscto(OfertaPromoDscto ofertaPromoDscto);
        void ModificarOfertaPromoDscto(OfertaPromoDscto ofertaPromoDscto);
        List<OfertaPromoDscto> Demanda_OfertasPromosDsctos();
        List<Suministrador> GetTodosSuministradores();
        int MontoComprasLogradasSuministrador(int suministradorId, int month, int year);
        void ActualizarListaProductos(List<Producto> productos);
        void ActualizarProducto(Producto producto);
        List<Producto> Demanda_Productos();
        List<CompraVirtual> GetComprasVirtualesSuministrador(int suministradorId);
        #endregion
    }
}
