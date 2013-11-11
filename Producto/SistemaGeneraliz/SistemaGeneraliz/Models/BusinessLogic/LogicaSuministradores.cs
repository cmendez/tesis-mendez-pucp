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
                listaRecargas = listaRecargas.OrderByDescending(r => r.FechaRecarga).ToList();
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
                        DocumentoProveedor = d,
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
                    if (ofertaPromoDscto.FechaFin.CompareTo(DateTime.Now) >= 0)
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
            }

            return listaOfertasPromosDsctosViewModel;
        }

        public OfertaPromoDscto GetOfertaPromoDscto(int ofertaPromoDsctoId)
        {
            return _sgpFactory.GetOfertaPromoDscto(ofertaPromoDsctoId);
        }

        public void SepararOfertaPromoDscto(int proveedorId, int ofertaId, int costo)
        {
            OfertaPromoDscto ofertaPromoDscto = _sgpFactory.GetOfertaPromoDscto(ofertaId);
            ofertaPromoDscto.CantidadDisponible = ofertaPromoDscto.CantidadDisponible - 1;

            CompraVirtual compraVirtual = new CompraVirtual
            {
                ProveedorId = proveedorId,
                OfertaPromoDsctoId = ofertaId,
                FechaCompra = DateTime.Now,
                LeadsPagados = costo,
                IsEliminado = 0
            };

            _sgpFactory.AgregarCompraVirtual(compraVirtual);
            _sgpFactory.ConsumirLeadsProveedor(proveedorId, costo);
            _sgpFactory.ActualizarOfertaPromoDscto(ofertaPromoDscto);
        }

        public List<OfertasPromosDsctosViewModel> GetOfertasPromosDsctosSuministradorCatalogo(int suministradorId)
        {
            List<OfertasPromosDsctosViewModel> listaOfertasPromosDsctosViewModel = new List<OfertasPromosDsctosViewModel>();
            List<OfertaPromoDscto> ofertasPromosDsctos = _sgpFactory.GetOfertasPromosDsctosSuministradorCatalogo(suministradorId);

            if ((ofertasPromosDsctos != null) && (ofertasPromosDsctos.Count > 0))
            {
                foreach (var ofertaPromoDscto in ofertasPromosDsctos)
                {
                    string tipo = "Oferta";
                    if (ofertaPromoDscto.Tipo == "Promoción")
                        tipo = "Promo";
                    if (ofertaPromoDscto.Tipo == "Descuento")
                        tipo = "Dscto";

                    OfertasPromosDsctosViewModel ofertasPromosDsctosViewModel = new OfertasPromosDsctosViewModel
                    {
                        OfertaPromoDsctoId = ofertaPromoDscto.OfertaPromoDsctoId,
                        NombreCorto = ofertaPromoDscto.NombreCorto.Length > 19 ? ofertaPromoDscto.NombreCorto.Substring(0, 18) + "..." : ofertaPromoDscto.NombreCorto,
                        NombreCompleto = ofertaPromoDscto.NombreCompleto,
                        Descripcion = ofertaPromoDscto.Descripcion,
                        Tipo = tipo,
                        ImagenPrincipalId = (int)ofertaPromoDscto.ImagenPrincipalId,
                        ImagenBannerId = (int)ofertaPromoDscto.ImagenBannerId,
                        CostoEnLeads = ofertaPromoDscto.CostoEnLeads,
                        SuministradorId = ofertaPromoDscto.SuministradorId,
                        CantidadDisponible = ofertaPromoDscto.CantidadDisponible,
                        FechaRegistro = ofertaPromoDscto.FechaRegistro.ToString("dd/MM/yyyy"),
                        FechaInicio = ofertaPromoDscto.FechaInicio,
                        FechaFin = ofertaPromoDscto.FechaFin,
                        IsAdquiribleConLeads = ofertaPromoDscto.IsAdquiribleConLeads,
                        IsVisible = ofertaPromoDscto.IsVisible,
                        IsEliminado = ofertaPromoDscto.IsEliminado
                    };
                    listaOfertasPromosDsctosViewModel.Add(ofertasPromosDsctosViewModel);
                }
            }

            return listaOfertasPromosDsctosViewModel;
        }

        public void AgregarOfertaPromoDscto(OfertasPromosDsctosViewModel ofertasPromoDsctoViewModel)
        {
            OfertaPromoDscto ofertaPromoDscto = new OfertaPromoDscto
            {
                NombreCorto = ofertasPromoDsctoViewModel.NombreCorto,
                NombreCompleto = ofertasPromoDsctoViewModel.NombreCompleto,
                Descripcion = ofertasPromoDsctoViewModel.Descripcion,
                Tipo = ofertasPromoDsctoViewModel.Tipo,
                ImagenPrincipalId = ofertasPromoDsctoViewModel.ImagenPrincipalId,
                ImagenBannerId = ofertasPromoDsctoViewModel.ImagenBannerId,
                SuministradorId = ofertasPromoDsctoViewModel.SuministradorId,
                FechaRegistro = DateTime.Now,
                FechaInicio = ofertasPromoDsctoViewModel.FechaInicio,
                FechaFin = ofertasPromoDsctoViewModel.FechaFin,
                CantidadDisponible = ofertasPromoDsctoViewModel.CantidadDisponible,
                IsAdquiribleConLeads = ofertasPromoDsctoViewModel.IsAdquiribleConLeads,
                CostoEnLeads = ofertasPromoDsctoViewModel.CostoEnLeads,
                IsVisible = ofertasPromoDsctoViewModel.IsVisible,
                IsEliminado = 0
            };

            _sgpFactory.AgregarOfertaPromoDscto(ofertaPromoDscto);
        }

        public OfertasPromosDsctosViewModel GetOfertaPromoDsctoViewModel(int ofertaPromoDsctoId)
        {
            OfertaPromoDscto ofertaPromoDscto = _sgpFactory.GetOfertaPromoDscto(ofertaPromoDsctoId);
            OfertasPromosDsctosViewModel ofertaPromoDsctoViewModel = new OfertasPromosDsctosViewModel
            {
                OfertaPromoDsctoId = ofertaPromoDscto.OfertaPromoDsctoId,
                SuministradorId = ofertaPromoDscto.SuministradorId,
                NombreCorto = ofertaPromoDscto.NombreCorto,
                NombreCompleto = ofertaPromoDscto.NombreCompleto,
                Descripcion = ofertaPromoDscto.Descripcion,
                Tipo = ofertaPromoDscto.Tipo,
                ImagenPrincipalId = (int)ofertaPromoDscto.ImagenPrincipalId,
                ImagenBannerId = (int)ofertaPromoDscto.ImagenBannerId,
                FechaInicio = ofertaPromoDscto.FechaInicio,
                FechaFin = ofertaPromoDscto.FechaFin,
                CantidadDisponible = ofertaPromoDscto.CantidadDisponible,
                IsAdquiribleConLeads = ofertaPromoDscto.IsAdquiribleConLeads,
                CostoEnLeads = ofertaPromoDscto.CostoEnLeads,
                IsVisible = ofertaPromoDscto.IsVisible,
                IsEliminado = 0
            };
            return ofertaPromoDsctoViewModel;
        }

        public void ModificarOfertaPromoDscto(OfertasPromosDsctosViewModel ofertaPromoDsctoViewModel)
        {
            OfertaPromoDscto ofertaPromoDscto = _sgpFactory.GetOfertaPromoDscto(ofertaPromoDsctoViewModel.OfertaPromoDsctoId);
            //Modificamos los atributos segun el viewmodel
            ofertaPromoDscto.NombreCorto = ofertaPromoDsctoViewModel.NombreCorto;
            ofertaPromoDscto.NombreCompleto = ofertaPromoDsctoViewModel.NombreCompleto;
            ofertaPromoDscto.Descripcion = ofertaPromoDsctoViewModel.Descripcion;
            ofertaPromoDscto.Tipo = ofertaPromoDsctoViewModel.Tipo;
            ofertaPromoDscto.ImagenPrincipalId = ofertaPromoDsctoViewModel.ImagenPrincipalId;
            ofertaPromoDscto.ImagenBannerId = ofertaPromoDsctoViewModel.ImagenBannerId;
            ofertaPromoDscto.FechaInicio = ofertaPromoDsctoViewModel.FechaInicio;
            ofertaPromoDscto.FechaFin = ofertaPromoDsctoViewModel.FechaFin;
            ofertaPromoDscto.CostoEnLeads = ofertaPromoDsctoViewModel.CostoEnLeads;
            ofertaPromoDscto.CantidadDisponible = ofertaPromoDsctoViewModel.CantidadDisponible;
            ofertaPromoDscto.IsAdquiribleConLeads = ofertaPromoDsctoViewModel.IsAdquiribleConLeads;
            ofertaPromoDscto.IsVisible = ofertaPromoDsctoViewModel.IsVisible;
            ofertaPromoDscto.IsEliminado = ofertaPromoDsctoViewModel.IsEliminado;

            _sgpFactory.ModificarOfertaPromoDscto(ofertaPromoDscto);
        }

        public SuministradorJuridicoViewModel GetSuministradorViewModel(Suministrador suministrador)
        {
            UbicacionPersona ubicacion = suministrador.Persona.UbicacionesPersonas.First();
            SuministradorJuridicoViewModel suministradorJuridicoViewModel = new SuministradorJuridicoViewModel
            {
                SuministradorId = suministrador.SuministradorId,
                PersonaId = suministrador.PersonaId,
                RUC = suministrador.Persona.RUC.ToString(),
                RazonSocial = suministrador.Persona.RazonSocial,
                FechaCreacion = (DateTime)suministrador.Persona.FechaCreacion,
                DireccionCompleta = suministrador.Persona.DireccionCompleta,
                IdDistrito = ubicacion.DistritoId,
                IdCiudad = ubicacion.Distrito.PaisCiudadId,
                Direccion = ubicacion.Direccion,
                Referencia = ubicacion.Referencia,
                Latitud = ubicacion.Latitud,
                Longitud = ubicacion.Longitud,
                Email1 = suministrador.Persona.Email1,
                Email2 = suministrador.Persona.Email2,
                Telefono1 = suministrador.Persona.Telefono1,
                Telefono2 = suministrador.Persona.Telefono2,
                Telefono3 = suministrador.Persona.Telefono3,
                ImagenPrincipal = (int)suministrador.Persona.ImagenId,
                UltimaActualizacionPersonal = DateTime.Now,
                //OldPassword = "asdasdasd",
                Password = "password",
                ConfirmPassword = "password",
                IsHabilitado = 1, //true
                IsEliminado = 0, //false
                Facebook = suministrador.Facebook,
                PaginaWeb = suministrador.PaginaWeb,
                AcercaDeMi = suministrador.AcercaDeMi,
                LeadsDisponibles = suministrador.LeadsDisponibles,
                LeadsReserva = suministrador.LeadsReserva
            };
            return suministradorJuridicoViewModel;
        }

        public void ActualizarSuministrador(Suministrador suministrador)
        {
            _sgpFactory.ActualizarSuministrador(suministrador);
        }

        public Suministrador ModificarObjetoSuministradorJuridico(SuministradorJuridicoViewModel suministradorJuridicoViewModel)
        {
            Suministrador suministrador = _sgpFactory.GetSuministradorPorPersonaId(suministradorJuridicoViewModel.PersonaId);
            suministrador.Facebook = suministradorJuridicoViewModel.Facebook;
            suministrador.PaginaWeb = suministradorJuridicoViewModel.PaginaWeb;
            suministrador.AcercaDeMi = suministradorJuridicoViewModel.AcercaDeMi;
            return suministrador;
        }

        public List<OfertasPromosDsctosViewModel> Demanda_OfertasPromosDsctos(string fechaInicio, string fechaFin)
        {
            List<OfertasPromosDsctosViewModel> listaOfertasPromosDsctosViewModel = new List<OfertasPromosDsctosViewModel>();
            List<OfertaPromoDscto> ofertasPromosDsctos = _sgpFactory.Demanda_OfertasPromosDsctos();

            if ((ofertasPromosDsctos != null) && (ofertasPromosDsctos.Count > 0))
            {
                ofertasPromosDsctos = ofertasPromosDsctos.OrderByDescending(o => o.ComprasVirtuales.Count).ToList();
                if (!String.IsNullOrEmpty(fechaInicio))
                {
                    DateTime finicio = DateTime.Parse(fechaInicio);
                    ofertasPromosDsctos = ofertasPromosDsctos.Where(o => o.ComprasVirtuales.Any(c => c.FechaCompra.Date.CompareTo(finicio) >= 0)).ToList();
                }

                if (!String.IsNullOrEmpty(fechaFin))
                {
                    DateTime ffin = DateTime.Parse(fechaFin);
                    ofertasPromosDsctos = ofertasPromosDsctos.Where(o => o.ComprasVirtuales.Any(c => c.FechaCompra.Date.CompareTo(ffin) <= 0)).ToList();
                }
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
                        Suministrador = ofertaPromoDscto.Suministrador.Persona.RazonSocial,
                        CantidadDisponible = ofertaPromoDscto.CantidadDisponible,
                        IsAdquiribleConLeads = ofertaPromoDscto.IsAdquiribleConLeads,
                        FechaRegistro = ofertaPromoDscto.FechaRegistro.ToString("dd/MM/yyyy"),
                        FechaInicioString = ofertaPromoDscto.FechaInicio.ToString("dd/MM/yyyy"),
                        FechaFinString = ofertaPromoDscto.FechaFin.ToString("dd/MM/yyyy"),
                        IsVisible = ofertaPromoDscto.IsVisible,
                        IsEliminado = ofertaPromoDscto.IsEliminado,
                        CantidadComprada = ofertaPromoDscto.ComprasVirtuales.Count //ofertaPromoDscto.ComprasVirtuales.Count(o => o.FechaCompra.Date.CompareTo(ffin) <= 0)
                    };
                    listaOfertasPromosDsctosViewModel.Add(ofertaPromoDsctoViewModel);
                }
            }

            return listaOfertasPromosDsctosViewModel;
        }

        public List<ConversionLeadsViewModel> ReporteConversionLeads(string fechaInicio, string fechaFin)
        {
            List<ConversionLeadsViewModel> suministradorJuridicoViewModels = new List<ConversionLeadsViewModel>();
            List<Suministrador> suministradores = _sgpFactory.GetTodosSuministradores();

            if ((suministradores != null) && (suministradores.Count > 0))
            {
                DateTime finicio = DateTime.Now.AddMonths(-6);
                DateTime ffin = DateTime.Now.AddMonths(-1);
                if (!String.IsNullOrEmpty(fechaInicio))
                {
                    finicio = DateTime.Parse(fechaInicio);
                }

                if (!String.IsNullOrEmpty(fechaFin))
                {
                    ffin = DateTime.Parse(fechaFin);
                }
                int nroMeses = ((ffin.Year - finicio.Year) * 12) + ffin.Month - finicio.Month + 1;
                for (int i = 0; i < nroMeses; i++)
                {
                    DateTime d = finicio.AddMonths(i);
                    string fecha = d.Month + "/" + d.Year;
                    foreach (var suministradorJuridico in suministradores)
                    {
                        int recargas = suministradorJuridico.RecargasLeads.Where(r => (r.FechaRecarga.Year == d.Year) && (r.FechaRecarga.Month == d.Month)).Sum(r => r.MontoRecarga);
                        int compras = _sgpFactory.MontoComprasLogradasSuministrador(suministradorJuridico.SuministradorId, d.Month, d.Year);
                        double t = (((recargas + compras) * 100.0) / (suministradorJuridico.LeadsMensuales * 1.0));
                        string tasa = t.ToString() + "%";
                        ConversionLeadsViewModel conversionLeadsViewModel = new ConversionLeadsViewModel
                        {
                            Año = d.Year,
                            Mes = d.Month,
                            RazonSocial = suministradorJuridico.Persona.RazonSocial,
                            ImagenPrincipal = (int)suministradorJuridico.Persona.ImagenId,
                            LeadsMensuales = suministradorJuridico.LeadsMensuales,
                            MontoRecargasLogradas = recargas,
                            MontoComprasLogradas = compras,
                            TasaConversion = t
                        };
                        suministradorJuridicoViewModels.Add(conversionLeadsViewModel);
                    }
                }
            }

            return suministradorJuridicoViewModels;
        }
    }
}
