using System;
using System.Collections.Generic;
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
    }
}