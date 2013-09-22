using System;
using SistemaGeneraliz.Models.Entities;
using SistemaGeneraliz.Models.Helpers;

namespace SistemaGeneraliz.Models.BusinessLogic
{
    public class LogicaPersonas
    {
        private readonly ISGPFactory _sgpFactory;

        public LogicaPersonas()
        {
            _sgpFactory = new SGPFactory();
        }

        public LogicaPersonas(ISGPFactory sgpFactory)
        {
            _sgpFactory = sgpFactory;
        }

        internal Persona CrearObjetoPersonaNatural(ProveedorNaturalViewModel proveedor)
        {
            proveedor.Sexo = (proveedor.SexoId == 1) ? "Masculino" : "Femenino";

            return new Persona
            {
                TipoPersona = "Natural",
                TipoUsuario = "Proveedor",
                DNI = Int32.Parse(proveedor.DNI),
                RUC = Int32.Parse(proveedor.RUC),
                PrimerNombre = proveedor.PrimerNombre,
                SegundoNombre = proveedor.SegundoNombre,
                ApellidoMaterno = proveedor.ApellidoMaterno,
                ApellidoPaterno = proveedor.ApellidoPaterno,
                FechaNacimiento = proveedor.FechaNacimiento,
                Sexo = proveedor.Sexo,
                //DireccionCompleta = proveedor.DireccionCompleta,
                Email1 = proveedor.Email1,
                Email2 = proveedor.Email2,
                Telefono1 = proveedor.Telefono1,
                Telefono2 = proveedor.Telefono2,
                Telefono3 = proveedor.Telefono3,
                //ImagenPrincipal = proveedor.ImagenPrincipal,
                UltimaActualizacionPersonal = DateTime.Now,
                IsHabilitado = 1, //true
                IsEliminado = 0 //false
            };
        }

        internal void AgregarPersona(Persona persona)
        {
            _sgpFactory.AgregarPersona(persona);
        }
    }
}