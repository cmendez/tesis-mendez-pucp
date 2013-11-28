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
        private List<Proveedor> lista_Candidatos;
        private List<Proveedor> vecindario;
        private Proveedor proveedor;
        private Proveedor mejorCandidato;
        private int M;
        private int nroTipoServicios;
        private int maxIteraciones;
        private int maxTamañoTabu;
        private int iteracion;
        private int k;
        private double sumatoriaOriginal;
        private double sumatoriaIntercambio;

        public AlgoritmoTabu(int[] serviciosIds, double latitudCliente, double longitudCliente)
        {
            //Inicialización
            //cliente = C;
            lista_Servicios = GenerarListaServicios(serviciosIds);
            lista_Proveedores = GenerarListaProveedores(ref lista_Servicios); //[][]
            lista_Inicial = GenerarListaInicial(lista_Proveedores, latitudCliente, longitudCliente);
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
            //Cuerpo Principal
            while (iteracion < maxIteraciones)
            {
                //Buscar a proveedor candidato para intercambio según servicio en turno (pos)
                lista_Candidatos = new List<Proveedor>();
                k = iteracion % nroTipoServicios;
                proveedor = lista_Mejores[k];

                //Buscar vecinos y agregar si no están marcados como Tabú
                vecindario = ObtenerVecindario(lista_Proveedores, proveedor, k);
                foreach (var candidato in vecindario)
                {
                    if (!EsTabu(lista_Tabu, candidato))
                    {
                        AgregarCandidato(lista_Candidatos, candidato);
                    }
                }

                //Encontrar óptimo local
                mejorCandidato = BuscarOptimoLocal(lista_Candidatos);

                //Calculamos el factor total obtenido para ambas listas (original y con reemplazo)
                sumatoriaOriginal = CalcularSumaFactores(lista_Mejores, null, k);
                sumatoriaIntercambio = CalcularSumaFactores(lista_Mejores, mejorCandidato, k);

                //Actualizar Mejores
                if (sumatoriaIntercambio > sumatoriaOriginal)
                {
                    MarcarTabu(lista_Tabu, mejorCandidato);
                    ListaMejorada(lista_Mejores, mejorCandidato, k);

                    //Desmarcar elementos de lista Tabú
                    while (TamañoActual(lista_Tabu) > maxTamañoTabu)
                        DesmarcarTabu(lista_Tabu);
                }

                iteracion++;
            }

            return GenerarListaMejores(lista_Mejores, lista_Servicios);
        }

        private List<ProveedorBusquedaViewModel> GenerarListaMejores(List<Proveedor> listaMejores, List<TipoServicio> listaServicios)
        {
            List<ProveedorBusquedaViewModel> lista = new List<ProveedorBusquedaViewModel>();
            int i = 0;
            foreach (var prov in listaMejores)
            {
                var pro = lista.Find(p => p.ProveedorId == prov.ProveedorId);

                if (pro == null)
                {
                    string nombre = "";
                    string tipoDocumento = "";

                    switch (prov.Persona.TipoPersona)
                    {
                        case "Natural":
                            nombre = prov.Persona.PrimerNombre + " " + prov.Persona.ApellidoPaterno;
                            tipoDocumento = "DNI";
                            break;

                        case "Juridica":
                            nombre = prov.Persona.RazonSocial;
                            tipoDocumento = "RUC";
                            break;
                    }

                    ProveedorBusquedaViewModel proveedorViewModel = new ProveedorBusquedaViewModel
                    {
                        ProveedorId = prov.ProveedorId,
                        Puntaje = prov.PuntuacionPromedio,
                        FotoId = (int)prov.Persona.ImagenId,
                        NombreCompleto = nombre,
                        TipoDocumento = tipoDocumento,
                        Documento = prov.Persona.UserName,
                        Servicio = listaServicios[i].NombreServicio,
                        ServicioId = listaServicios[i].TipoServicioId.ToString(),
                        Descripcion = prov.AcercaDeMi,
                        Telefono1 = prov.Persona.Telefono1 ?? "",
                        Telefono2 = prov.Persona.Telefono2 ?? "",
                        Telefono3 = prov.Persona.Telefono3 ?? "",
                        Email1 = prov.Persona.Email1 ?? "",
                        Email2 = prov.Persona.Email2 ?? "",
                        NroRecomendaciones = prov.NroRecomendaciones + "/" + prov.TrabajosProveedores.Count,
                        NroVolveriaContratarlo = prov.NroVolveriaContratarlo + "/" + prov.TrabajosProveedores.Count,
                    };
                    lista.Add(proveedorViewModel);
                }
                else
                {
                    pro.Servicio += " - " + listaServicios[i].NombreServicio;
                    pro.ServicioId += "-" + listaServicios[i].TipoServicioId.ToString();
                }

                i++;
            }
            return lista;
        }

        private List<TipoServicio> GenerarListaServicios(int[] serviciosIds)
        {
            List<TipoServicio> servicios = _logicaProveedores.GetServiciosPorIds(serviciosIds); //new List<TipoServicio>();
            return servicios;
        }

        private List<List<Proveedor>> GenerarListaProveedores(ref List<TipoServicio> listaServicios)
        {
            int puntajeMinimo = _logicaProveedores.GetPuntajeMinimoConfiguracion();
            int cantidadMaxima = _logicaProveedores.GetCantidadMaximaProveedoresConfiguracion();

            List<List<Proveedor>> listaProveedores = new List<List<Proveedor>>();
            List<TipoServicio> listaServiciosConProveedores = new List<TipoServicio>(listaServicios);
            //Para cada servicio requerido
            foreach (var servicio in listaServicios)
            {
                //Obtenemos proveedores directamente de la BD ordenados por puntaje de mayor a menor
                List<Proveedor> listaInterna = _logicaProveedores.GetProveedoresPorServicio(servicio, cantidadMaxima,
                                                                                            puntajeMinimo);
                if ((listaInterna == null) || (listaInterna.Count == 0))
                    listaServiciosConProveedores.Remove(servicio);
                else
                    listaProveedores.Add(listaInterna);
            }
            listaServicios = listaServiciosConProveedores;
            return listaProveedores;
        }

        private List<Proveedor> GenerarListaInicial(List<List<Proveedor>> listaProveedores, double latitudCliente, double longitudCliente)
        {
            List<Proveedor> lista_Inicial = new List<Proveedor>();

            UbicacionPersona ubicacionCliente = new UbicacionPersona
            {
                Latitud = latitudCliente,
                Longitud = longitudCliente
            };

            //Para cada sublista de proveedores
            foreach (var sublista in listaProveedores)
            {
                foreach (var prov in sublista)
                {
                    //Calculamos el distanciamiento de cada proveedor respecto del cliente
                    double distancia = Calcular_Distancia_GPS(prov.Persona.UbicacionesPersonas.ElementAt(0), ubicacionCliente);
                    prov.Distancia = distancia;
                    prov.Factor = prov.PuntuacionPromedio / prov.Distancia;
                }
                //Reordenamos la sublista por mejor Factor p/d
                sublista.Sort((x, y) => y.Factor.CompareTo(x.Factor));
                //sublista = sublista.OrderByDescending(p => p.Factor);

                //Obtener el primer proveedor y almacenarlo
                Proveedor p = sublista[0];
                lista_Inicial.Add(p);
            }

            return lista_Inicial;
        }

        private double Calcular_Distancia_GPS(UbicacionPersona ubicacionProveedor, UbicacionPersona ubicacionFuente)
        {
            double distancia, d, c, a, dLat, dLon, lat1, lat2;
            int R = 6371; // radio en km

            dLat = (ubicacionProveedor.Latitud - ubicacionFuente.Latitud) * (Math.PI / 180);
            dLon = (ubicacionProveedor.Longitud - ubicacionFuente.Longitud) * (Math.PI / 180);
            lat1 = ubicacionFuente.Latitud * (Math.PI / 180);
            lat2 = ubicacionProveedor.Latitud * (Math.PI / 180);

            a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            d = R * c;

            distancia = d;
            return distancia;
        }

        private int ObtenerTamañoMaximo(List<List<Proveedor>> listaProveedores)
        {
            int mayor = 0;
            //Para cada sublista de proveedores
            foreach (var sublista in listaProveedores)
            {
                //Obtener cantidad de proveedores en sublista
                int cantidad = sublista.Count();

                if (cantidad > mayor)
                    mayor = cantidad;
            }
            return mayor;
        }

        private List<Proveedor> ObtenerVecindario(List<List<Proveedor>> listaProveedores, Proveedor proveedor, int pos)
        {
            List<Proveedor> vecindarioProveedor = new List<Proveedor>();
            List<Proveedor> lista = listaProveedores[pos];	//la lista viene preordenada por puntaje
            int np = lista.Count;

            //Para cada proveedor
            for (int i = 0; i < np; i++)
            {
                if (lista[i] != proveedor)
                {
                    //Calculamos el distanciamiento de cada potencial vecino respecto del proveedor de turno
                    double distancia = Calcular_Distancia_GPS(lista[i].Persona.UbicacionesPersonas.ElementAt(0), proveedor.Persona.UbicacionesPersonas.ElementAt(0));

                    if (Convert.ToInt32(distancia) != 0) //los seeds a veces fallan y 2 proveedores tendran la misma Lat y Lon
                    {
                        vecindarioProveedor.Add(lista[i]);
                        vecindarioProveedor.ElementAt(vecindarioProveedor.Count - 1).Distancia = distancia;    
                    }
                }
            }

            if (vecindarioProveedor.Count == 0) //por si en la lista solo habia un proveedor (el mismo)
                vecindarioProveedor.Add(proveedor);

            return vecindarioProveedor;
        }

        private bool EsTabu(List<Proveedor> listaTabu, Proveedor candidato)
        {
            return listaTabu.Contains(candidato);
        }

        private void AgregarCandidato(List<Proveedor> listaCandidatos, Proveedor candidato)
        {
            listaCandidatos.Add(candidato);
        }

        private Proveedor BuscarOptimoLocal(List<Proveedor> listaCandidatos)
        {
            Proveedor mejor = null;

            if ((listaCandidatos != null) && (listaCandidatos.Count > 0)) //pueden que todos sus vecinos esten marcados como Tabú
            {
                foreach (var candidato in listaCandidatos)
                {
                    candidato.Factor = (candidato.PuntuacionPromedio/candidato.Distancia);
                }

                //Ordenar lista por factor P/D y tomar primer elemento
                //lista_Candidatos = QS_Proveedores(lista_Candidatos, 0, n - 1);
                listaCandidatos.Sort((x, y) => y.Factor.CompareTo(x.Factor));
                mejor = listaCandidatos[0];
            }

            return mejor;
        }

        private double CalcularSumaFactores(List<Proveedor> listaMejores, Proveedor prov, int pos)
        {
            double sumatoria, suma, factor;
            suma = 0;
            int i = 0;

            foreach (var p in listaMejores)
            {
                //Solo para proveedor en posición 'pos' y si no es 'null'
                if ((pos == i) && (prov != null))
                {
                    factor = prov.Factor;
                }
                else
                {
                    factor = p.Factor;
                }

                suma = suma + factor;
                i++;
            }

            sumatoria = suma;
            return sumatoria;
        }

        private void MarcarTabu(List<Proveedor> listaTabu, Proveedor mejor)
        {
            listaTabu.Add(mejor);
        }

        private void ListaMejorada(List<Proveedor> listaMejores, Proveedor mejor, int pos)
        {
            listaMejores[pos] = mejor;
        }

        private int TamañoActual(List<Proveedor> listaTabu)
        {
            return listaTabu.Count;
        }

        private void DesmarcarTabu(List<Proveedor> listaTabu)
        {
            listaTabu.RemoveAt(0);
        }
    }
}