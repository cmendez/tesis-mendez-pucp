using System;
using System.Collections.Generic;
using System.Globalization;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaClientes
    {
        private readonly ISGPFactory _sgpFactory;

        public LogicaClientes()
        {
            _sgpFactory = new SGPFactory();
        }

        public LogicaClientes(ISGPFactory sgpFactory)
        {
            _sgpFactory = sgpFactory;
        }

        internal void AgregarCliente(Cliente Cliente)
        {
            _sgpFactory.AgregarCliente(Cliente);
        }

        internal Cliente CrearObjetoClienteNatural(ClienteNaturalViewModel cliente)
        {
            return new Cliente
            {
            };
        }

        internal Cliente CrearObjetoClienteJuridico(ClienteJuridicoViewModel cliente)
        {
            return new Cliente
            {
            };
        }

        public Cliente GetClientePorPersonaId(int idPersona)
        {
            return _sgpFactory.GetClientePorPersonaId(idPersona);
        }

        public List<ProveedorBusquedaViewModel> EjecutarAlgoritmoTabu(string valueServicios, double latitud, double longitud)
        {
            try
            {
                string[] serviciosIds = valueServicios.Split(',');
                int[] servicios = new int[serviciosIds.Length];
                for (int i = 0; i < serviciosIds.Length; i++)
                {
                    servicios[i] = Int32.Parse(serviciosIds[i]);
                }

                //IMPLEMENTAR AQUI LLAMADA AL ALGORITMO TABU
                AlgoritmoTabu algoritmo = new AlgoritmoTabu(servicios, latitud, longitud);
                return algoritmo.EjecutarAlgoritmoTabu();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void AgregarTrabajo(int clienteId, string proveedoresIds, string serviciosIds, string fecha, string ubicacion, string desc)
        {
            int i = 0;

            Trabajo trabajo = new Trabajo
            {
                ClienteId = clienteId,
                Fecha = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Direccion = ubicacion,
                DescripcionCliente = desc,
                IsTerminado = 0
            };
            
            _sgpFactory.AgregarTrabajo(trabajo);

            //Agregar TrabajoProveedor
            string[] provIds = proveedoresIds.Split(',');
            string[] servicios = serviciosIds.Split(',');
            foreach (var proveedor in provIds)
            {
                if (proveedor != "")
                {
                    TrabajoProveedor trabajoProveedor = new TrabajoProveedor
                    {
                        ProveedorId = Int32.Parse(proveedor),
                        TrabajoId = trabajo.TrabajoId,
                        TiposServicios = new List<TipoServicio>()
                    };

                    //Agregar TiposServiciosXTrabajosProveedores
                    string serviciosProveedor = servicios[i];
                    if (serviciosProveedor.IndexOf("-") > 0) //el proveedor ejecutará más de 1 servicio
                    {
                        string[] idsServicios = serviciosProveedor.Split('-');
                        foreach (var idServ in idsServicios)
                        {
                            if (idServ != "")
                            {
                                TipoServicio tipo = _sgpFactory.GetTipoServicioPorId(Convert.ToInt32(idServ));
                                trabajoProveedor.TiposServicios.Add(tipo);
                            }
                        }
                    }
                    else
                    {
                        if (serviciosProveedor != "")
                        {
                            TipoServicio tipo = _sgpFactory.GetTipoServicioPorId(Convert.ToInt32(serviciosProveedor));
                            trabajoProveedor.TiposServicios.Add(tipo);
                        }
                    }

                    _sgpFactory.AgregarTrabajoProveedor(trabajoProveedor);
                    _sgpFactory.ConsumirLeadsProveedor(Int32.Parse(proveedor), 1);
                }
                i++;
            }
        }
    }
}