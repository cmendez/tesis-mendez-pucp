using System;
using System.Collections.Generic;
using System.IO;
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

        public List<RecargasLeadsViewModel> GetListaRecargasSuministrador(Suministrador suministrador)
        {
            var listaRecargasViewModel = new List<RecargasLeadsViewModel>();
            var listaRecargas = suministrador.RecargasLeads; //_sgpFactory.GetListaRecargasSuministrador(suministradorId);
            if (listaRecargas != null)
            {
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
                                                         FechaRecarga =
                                                             recarga.FechaRecarga.ToString("dd/MM/yyyy HH:mm"),
                                                         NombreProveedor = n,
                                                         DocumentoProveedor =
                                                             d + " " + recarga.Proveedor.Persona.UserName,
                                                         MontoRecarga = "S/. " + recarga.MontoRecarga.ToString(),
                                                         CantidadLeads = recarga.CantidadLeads
                                                     };
                    listaRecargasViewModel.Add(rec);
                }
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

        public Suministrador GetSuministrador(int idSuministrador)
        {
            return _sgpFactory.GetSuministrador(idSuministrador);
        }

        public List<ProductosViewModel> GetProductosCatalogo(string nombreProducto, int categoriaId, int distritoId, int suministradorId)
        {
            List<ProductosViewModel> listaProductosViewModel = new List<ProductosViewModel>();
            List<Producto> productos = _sgpFactory.GetProductosCatalogo(nombreProducto, categoriaId, distritoId, suministradorId);

            if ((productos != null) && (productos.Count > 0))
            {
                foreach (var producto in productos)
                {
                    ProductosViewModel productoViewModel = new ProductosViewModel
                    {
                        ProductoId = producto.ProductoId,
                        NombreCorto = producto.NombreCorto,
                        NombreCompleto = producto.NombreCompleto,
                        Descripcion = producto.Descripcion,
                        CategoriaProductoId = producto.CategoriaProductoId,
                        ImagenId = (int)producto.ImagenId,
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

        public Producto GetProducto(int productoId)
        {
            return _sgpFactory.GetProducto(productoId);
        }

        public List<UbicacionPersona> GetUbicacionesSuministrador(int suministradorId)
        {
            Suministrador s = this.GetSuministrador(suministradorId);
            return _sgpFactory.GetUbicacionesPersona(s.PersonaId);
        }

        public List<ProductosViewModel> GetProductosSuministradorCatalogo(int suministradorId)
        {
            List<ProductosViewModel> listaProductosViewModel = new List<ProductosViewModel>();
            List<Producto> productos = _sgpFactory.GetProductosSuministradorCatalogo(suministradorId);

            if ((productos != null) && (productos.Count > 0))
            {
                foreach (var producto in productos)
                {
                    ProductosViewModel productoViewModel = new ProductosViewModel
                    {
                        ProductoId = producto.ProductoId,
                        NombreCorto = producto.NombreCorto,
                        NombreCompleto = producto.NombreCompleto,
                        Descripcion = producto.Descripcion,
                        CategoriaProductoId = producto.CategoriaProductoId,
                        ImagenId = (int)producto.ImagenId,
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

        public Imagen AgregarImagenProducto(HttpPostedFileBase file)
        {
            Imagen imagen;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                imagen = new Imagen { Data = memoryStream.ToArray(), Nombre = file.FileName, Mime = file.ContentType };
            }
            _sgpFactory.AgregarImagen(imagen);
            return imagen;
        }

        public void AgregarProducto(ProductosViewModel productoViewModel)
        {
            Producto producto = new Producto
            {
                NombreCorto = productoViewModel.NombreCorto,
                NombreCompleto = productoViewModel.NombreCompleto,
                Descripcion = productoViewModel.Descripcion,
                CategoriaProductoId = productoViewModel.CategoriaProductoId,
                ImagenId = productoViewModel.ImagenId,
                Precio = productoViewModel.PrecioProducto,
                SuministradorId = productoViewModel.SuministradorId,
                NroClicksVisita = 0,
                NroBusquedas = 0,
                FechaRegistro = DateTime.Now,
                IsVisible = productoViewModel.IsVisible,
                IsEliminado = 0
            };

            _sgpFactory.AgregarProducto(producto);
        }

        public ProductosViewModel GetProductoViewModel(int productoId)
        {
            Producto producto = _sgpFactory.GetProducto(productoId);
            ProductosViewModel productoViewModel = new ProductosViewModel
            {
                ProductoId = producto.ProductoId,
                SuministradorId = producto.SuministradorId,
                NombreCorto = producto.NombreCorto,
                NombreCompleto = producto.NombreCompleto,
                Descripcion = producto.Descripcion,
                PrecioProducto = producto.Precio,
                ImagenId = (int)producto.ImagenId,
                CategoriaProductoId = producto.CategoriaProductoId,
                IsVisible = producto.IsVisible,
                IsEliminado = 0
            };
            return productoViewModel;
        }

        public void ModificarProducto(ProductosViewModel productoViewModel)
        {
            Producto producto = _sgpFactory.GetProducto(productoViewModel.ProductoId);

            producto.NombreCorto = productoViewModel.NombreCorto;
            producto.NombreCompleto = productoViewModel.NombreCompleto;
            producto.Descripcion = productoViewModel.Descripcion;
            producto.CategoriaProductoId = productoViewModel.CategoriaProductoId;
            producto.ImagenId = productoViewModel.ImagenId;
            producto.Precio = productoViewModel.PrecioProducto;
            //producto.SuministradorId = productoViewModel.SuministradorId;
            producto.IsVisible = productoViewModel.IsVisible;
            producto.IsEliminado = productoViewModel.IsEliminado;

            _sgpFactory.ModificarProducto(producto);
        }

        public List<Object> GetSuministradores()
        {
            List<Object> suministradores = new List<Object>();
            Object suministrador = new 
            {
                SuministradorId = -1,
                NombreSuministrador = "- Todos los suministradores -",
            };
            suministradores.Add(suministrador);
            List<Suministrador> lista = _sgpFactory.GetSuministradores();
            if ((lista != null) && (lista.Count > 0))
            {
                foreach (var sumin in lista)
                {
                    suministrador = new
                    {
                        SuministradorId = sumin.SuministradorId,
                        NombreSuministrador = sumin.Persona.RazonSocial
                    };
                    suministradores.Add(suministrador);
                }
            }
            return suministradores;
        }

        public List<OfertasPromosDsctosViewModel> GetOfertasPromosDsctosCatalogo()
        {
            List<OfertasPromosDsctosViewModel> listaOfertasPromosDsctosViewModel = new List<OfertasPromosDsctosViewModel>();
            List<OfertaPromoDscto> ofertasPromosDsctos = _sgpFactory.GetOfertasPromosDsctosCatalogo();

            if ((ofertasPromosDsctos != null) && (ofertasPromosDsctos.Count > 0))
            {
                foreach (var ofertaPromoDscto in ofertasPromosDsctos)
                {
                    OfertasPromosDsctosViewModel ofertaPromoDsctoViewModel = new OfertasPromosDsctosViewModel
                    {
                        OfertaPromoDsctoId = ofertaPromoDscto.OfertaPromoDsctoId,
                        Tipo = ofertaPromoDscto.Tipo,
                        NombreCorto = ofertaPromoDscto.NombreCorto.Length > 19 ? ofertaPromoDscto.NombreCorto.Substring(0, 18) + "..." : ofertaPromoDscto.NombreCorto,
                        NombreCompleto = ofertaPromoDscto.NombreCompleto,
                        Descripcion = ofertaPromoDscto.Descripcion,
                        ImagenPrincipalId = (int)ofertaPromoDscto.ImagenPrincipalId,
                        ImagenBannerId = (int)ofertaPromoDscto.ImagenBannerId,
                        CostoEnLeads = ofertaPromoDscto.CostoEnLeads,
                        SuministradorId = ofertaPromoDscto.SuministradorId,
                        CantidadDisponible = ofertaPromoDscto.CantidadDisponible,
                        IsAdquiribleConLeads = ofertaPromoDscto.IsAdquiribleConLeads,
                        FechaRegistro = ofertaPromoDscto.FechaRegistro.ToString("dd/MM/yyyy"),
                        FechaInicioString = ofertaPromoDscto.FechaInicio.ToString("dd/MM/yyyy"),
                        FechaFinString = ofertaPromoDscto.FechaFin.ToString("dd/MM/yyyy"),
                        IsVisible = ofertaPromoDscto.IsVisible,
                        IsEliminado = ofertaPromoDscto.IsEliminado
                    };
                    listaOfertasPromosDsctosViewModel.Add(ofertaPromoDsctoViewModel);
                }
            }

            return listaOfertasPromosDsctosViewModel;
        }

        public OfertaPromoDscto GetOfertaPromoDscto(int ofertaPromoDsctoId)
        {
            return _sgpFactory.GetOfertaPromoDscto(ofertaPromoDsctoId);
        }
    }
}