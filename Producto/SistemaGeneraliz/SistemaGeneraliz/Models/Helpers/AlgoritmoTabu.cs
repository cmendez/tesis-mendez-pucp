using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.BusinessLogic;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Models.Helpers
{
    public class AlgoritmoTabu
    {
        private readonly LogicaProveedores _logicaProveedores = new LogicaProveedores();
        private List<TipoServicio> lista_Servicios;
        private List<List<Proveedor>> lista_Proveedores;
        private List<Proveedor> lista_Inicial;
        private List<Proveedor> lista_Mejores;
        private List<Proveedor> lista_Tabu;
        private int M;
        private int nroTipoServicios;
        private int maxIteraciones;
        private int maxTamañoTabu;
        private int iteracion;

        public AlgoritmoTabu(int[] serviciosIds, double latitudCliente, double longitudCliente)
        {
            //Inicialización
            //cliente = C;
            lista_Servicios = GenerarListaServicios(serviciosIds);
            lista_Proveedores = GenerarListaProveedores(lista_Servicios); //[][]
            lista_Inicial = GenerarListaInicial(lista_Proveedores);
            lista_Mejores = lista_Inicial;
            lista_Tabu = new List<Proveedor>();
            M = ObtenerTamañoMaximo(lista_Proveedores);
            nroTipoServicios = lista_Servicios.Count;
            maxIteraciones = nroTipoServicios * M;
            maxTamañoTabu = M * 2;
            iteracion = 0;
        }

        public List<ProveedorBusquedaViewModel> EjecutarAlgoritmoTabu()
        {

            return GenerarListaMejores(lista_Mejores, lista_Servicios);
        }

        private List<ProveedorBusquedaViewModel> GenerarListaMejores(List<Proveedor> lista_Mejores, List<TipoServicio> lista_Servicios)
        {
            List<ProveedorBusquedaViewModel> lista = new List<ProveedorBusquedaViewModel>();
            int i = 0;
            foreach (var proveedor in lista_Mejores)
            {
                string nombre = "";
                string tipoDocumento = "";

                switch (proveedor.Persona.TipoPersona)
                {
                    case "Natural":
                        nombre = proveedor.Persona.PrimerNombre + " " + proveedor.Persona.ApellidoPaterno;
                        tipoDocumento = "DNI";
                        break;

                    case "Juridica":
                        nombre = proveedor.Persona.RazonSocial;
                        tipoDocumento = "RUC";
                        break;
                }

                ProveedorBusquedaViewModel proveedorViewModel = new ProveedorBusquedaViewModel
                {
                    ProveedorId = proveedor.ProveedorId,
                    Puntaje = ((int)proveedor.PuntuacionPromedio).ToString(),
                    RutaFoto = "", //AQUI IRA LA URL DE LA FOTO
                    NombreCompleto = nombre,
                    TipoDocumento = tipoDocumento,
                    Documento = proveedor.Persona.UserName,
                    Servicio = lista_Servicios[i].NombreServicio,
                    Descripcion = proveedor.AcercaDeMi,
                    VerTrabajos = "", //AQUI IRA LINK DE TRABAJOS 
                    VerComentarios = "" //AQUI IRA LINK DE COMENTARIOS 
                };
                lista.Add(proveedorViewModel);
                i++;
            }
            return lista;
        }

        private List<TipoServicio> GenerarListaServicios(int[] serviciosIds)
        {
            List<TipoServicio> servicios = _logicaProveedores.GetServiciosPorIds(serviciosIds); //new List<TipoServicio>();
            return servicios;
        }

        private List<List<Proveedor>> GenerarListaProveedores(List<TipoServicio> listaServicios)
        {
            int cantidadMaxima = 100; //podría ser configurable
            int puntajeMinimo = 0;//14; //podría ser configurable
            int i = 0;
            List<List<Proveedor>> listaProveedores = new List<List<Proveedor>>();

            //Para cada servicio requerido
            foreach (var servicio in listaServicios)
            {
                //Obtenemos proveedores directamente de la BD 
                //ordenados por puntaje de mayor a menor
                List<Proveedor> listaInterna = _logicaProveedores.GetProveedoresPorServicio(servicio, cantidadMaxima,
                                                                                            puntajeMinimo);
                //lista_Proveedores[i] <- ObtenerProveedoresBD(servicio, cantidadMaxima, puntajeMinimo);
                listaProveedores.Add(listaInterna);
                i++;
            }

            return listaProveedores;
        }

        private List<Proveedor> GenerarListaInicial(List<List<Proveedor>> lista_Proveedores)
        {
            List<Proveedor> lista_Inicial = new List<Proveedor>();
            //i <- 0;

            //Para cada sublista de proveedores
            foreach (var sublista in lista_Proveedores)
            {
                //Obtener el primer proveedor y almacenarlo
                Proveedor proveedor = sublista[0];
                lista_Inicial.Add(proveedor);
                //i++
            }

            return lista_Inicial;
        }

        private int ObtenerTamañoMaximo(List<List<Proveedor>> lista_Proveedores)
        {
            int mayor = 0;
            //Para cada sublista de proveedores
            foreach (var sublista in lista_Proveedores)
            {
                //Obtener cantidad de proveedores en sublista
                int cantidad = sublista.Count();

                if (cantidad > mayor)
                    mayor = cantidad;
            }
            return mayor;
        }
    }
}