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


        public List<ProductosViewModel> GetProductosCatalogo(string nombreProducto, int categoriaId, int distritoId)
        {
            List<ProductosViewModel> listaProductosViewModel = new List<ProductosViewModel>();
            List<Producto> productos = _sgpFactory.GetProductosCatalogo(nombreProducto, categoriaId, distritoId);
            
            if ((productos != null) && (productos.Count > 0))
            {
                foreach (var producto in productos)
                {
                    ProductosViewModel productoViewModel = new ProductosViewModel
                    {
                        ProductoId = producto.ProductoId,
                        NombreProducto = producto.NombreProducto,
                        DescripcionCorta = producto.DescripcionCorta,
                        DescripcionDetalle = producto.DescripcionDetalle,
                        CategoriaProductoId = producto.CategoriaProductoId,
                        ImagenId = (int) producto.ImagenId,
                        Precio = "S/. " + producto.Precio,
                        SuministradorId = producto.SuministradorId,
                        NroClicksVisita = producto.NroClicksVisita,
                        NroBusquedas = producto.NroBusquedas,
                        FechaRegistro = producto.FechaRegistro.ToString("dd/MM/yyyy"),
                        IsVisible = producto.IsVisible,
                        IsEliminado = producto.IsEliminado
                    };
                    listaProductosViewModel.Add(productoViewModel);
                }
            }

            return listaProductosViewModel;
        }

        public List<CategoriaProducto> GetCategoriasProducto()
        {
            List<CategoriaProducto> categorias = new List<CategoriaProducto>();
            CategoriaProducto categoria = new CategoriaProducto
            {
                CategoriaProductoId = -1,
                NombreCategoria = "- Todas las categorías -",
            };
            categorias.Add(categoria);
            categorias.AddRange(_sgpFactory.GetCategoriasProducto());
            return categorias;
        }

        public List<Distrito> GetDistritos()
        {
            List<Distrito> distritos = new List<Distrito>();
            Distrito distrito = new Distrito
            {
                DistritoId = -1,
                NombreDistrito = "- Todos los distritos -",
            };
            distritos.Add(distrito);
            distritos.AddRange(_sgpFactory.GetDistritos());
            return distritos;
        }
    }
}