using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;
using SistemaGeneraliz.Models.ViewModels;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaSuministradores
    {
        private readonly ISGPFactory _sgpFactory;

        public LogicaSuministradores()
        {
            _sgpFactory = new SGPFactory();
        }

        public LogicaSuministradores(ISGPFactory sgpFactory)
        {
            _sgpFactory = sgpFactory;
        }

        internal void AgregarSuministrador(Suministrador suministrador)
        {
            _sgpFactory.AgregarSuministrador(suministrador);
        }


        internal Suministrador CrearObjetoSuministradorJuridico(SuministradorJuridicoViewModel suministrador)
        {
            return new Suministrador
            {
                LeadsDisponibles = suministrador.LeadsDisponibles,
                LeadsReserva = suministrador.LeadsReserva,
                //Especialidad = Suministrador.Especialidad, //setear esto desde la logica
                PaginaWeb = suministrador.PaginaWeb,
                Facebook = suministrador.Facebook,
                AcercaDeMi = suministrador.AcercaDeMi,
                IsDestacado = 0 //false
            };
        }

        public Suministrador GetSuministradorPorPersonaId(int personaId)
        {
            return _sgpFactory.GetSuministradorPorPersonaId(personaId);
        }

        public List<RecargaLeads> GetListaRecargasSuministrador(int suministradorId)
        {
            return _sgpFactory.GetListaRecargasSuministrador(suministradorId);
        }

        internal void AgregarRecarga(RecargaLeads recarga)
        {
            _sgpFactory.AgregarRecarga(recarga);
        }

        public void ActualizarLeads(int idSuministrador, int monto)
        {
            Suministrador suministrador = this.GetSuministrador(idSuministrador);

            if (suministrador.LeadsDisponibles >= monto)
            {
                suministrador.LeadsDisponibles -= monto;
            }
            else if (monto > suministrador.LeadsDisponibles)
            {
                suministrador.LeadsReserva -= monto - suministrador.LeadsDisponibles;
                suministrador.LeadsDisponibles = 0;
            }
            _sgpFactory.ActualizarSuministrador(suministrador);
        }

        private Suministrador GetSuministrador(int idSuministrador)
        {
            return _sgpFactory.GetSuministrador(idSuministrador);
        }
    }
}