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

        public List<RecargasLeadsViewModel> GetListaRecargasSuministrador(int suministradorId)
        {
            var listaRecargasViewModel = new List<RecargasLeadsViewModel>();
            var listaRecargas = _sgpFactory.GetListaRecargasSuministrador(suministradorId);
            foreach (var recarga in listaRecargas)
            {
                string n = "";
                string d = "";

                if (recarga.Proveedor.Persona.TipoPersona == "Natural")
                {
                    n = recarga.Proveedor.Persona.PrimerNombre + " " + recarga.Proveedor.Persona.ApellidoPaterno;
                    d = recarga.Proveedor.Persona.DNI.ToString();
                }

                if (recarga.Proveedor.Persona.TipoPersona == "Juridica")
                {
                    n = recarga.Proveedor.Persona.RazonSocial;
                    d = recarga.Proveedor.Persona.RUC.ToString();
                }

                RecargasLeadsViewModel rec = new RecargasLeadsViewModel
                {
                    RecargaLeadsId = recarga.RecargaLeadsId,
                    FechaRecarga = recarga.FechaRecarga.ToString("dd/MM/yyyy HH:mm"),
                    NombreProveedor = n,
                    DocumentoProveedor = d + " " + recarga.Proveedor.Persona.UserName,
                    MontoRecarga = "S/. " + recarga.MontoRecarga.ToString(),
                    CantidadLeads = recarga.CantidadLeads
                };
                listaRecargasViewModel.Add(rec);
            }
            return listaRecargasViewModel;
        }

        public void AgregarRecarga(int idProveedor, int idSuministrador, int monto)
        {
            RecargaLeads recarga = new RecargaLeads
            {
                SuministradorId = idSuministrador,
                ProveedorId = idProveedor,
                FechaRecarga = DateTime.Now,
                MontoRecarga = monto,
                TipoMoneda = "Soles",
                CantidadLeads = monto
            };

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