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
    }
}