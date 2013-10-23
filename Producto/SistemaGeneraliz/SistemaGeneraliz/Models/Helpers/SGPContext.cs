using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Data.Entity;
using System.Web.Hosting;
using System.Web.Security;
using SistemaGeneraliz.Models.Entities;
using WebMatrix.WebData;

namespace SistemaGeneraliz.Models.Helpers
{
    public class SGPContext : DbContext, ISGPContext
    {
        public SGPContext()
            : base("SGPContext")
        {
        }

        #region DbSet's
        public IDbSet<Persona> Personas { get; set; }
        public IDbSet<Cliente> Clientes { get; set; }
        public IDbSet<Proveedor> Proveedores { get; set; }
        public IDbSet<Suministrador> Suministradores { get; set; }
        public IDbSet<TipoServicio> TipoServicios { get; set; }
        public IDbSet<PaisCiudad> PaisesCiudades { get; set; }
        public IDbSet<Distrito> Distritos { get; set; }
        public IDbSet<UbicacionPersona> UbicacionesPersonas { get; set; }
        public IDbSet<RecargaLeads> RecargasLeads { get; set; }
        public IDbSet<Configuracion> Configuraciones { get; set; }
        public IDbSet<Trabajo> Trabajos { get; set; }
        public IDbSet<TrabajoProveedor> TrabajosProveedores { get; set; }
        public IDbSet<EncuestaCliente> EncuestasClientes { get; set; }
        public IDbSet<RespuestaPorCriterio> RespuestasPorCriterio { get; set; }
        public IDbSet<CriterioCalificacion> CriteriosCalificacion { get; set; }
        public IDbSet<PuntajePromedioCriterio> PuntajesPromedioCriterio { get; set; }
        public IDbSet<Imagen> Imagenes { get; set; }
        public IDbSet<Producto> Productos { get; set; }
        public IDbSet<CategoriaProducto> CategoriasProducto { get; set; }
        //public IDbSet<SubcategoriaProducto> SubcategoriasProducto { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Proveedor>().HasMany(r => r.TiposServicios).WithMany(t => t.Proveedores)
                .Map(t => t.MapLeftKey("ProveedorId").MapRightKey("TipoServicioId").ToTable("TiposServiciosPorProveedor"));
            modelBuilder.Entity<TrabajoProveedor>().HasMany(r => r.TiposServicios).WithMany(t => t.TrabajosProveedores)
                .Map(t => t.MapLeftKey("TrabajoProveedorId").MapRightKey("TipoServicioId").ToTable("TiposServiciosPorTrabajoProveedor"));
            //modelBuilder.Entity<TrabajoProveedor>().HasRequired(t => t.EncuestaCliente).WithRequiredPrincipal(t => t.TrabajoProveedor);
            //modelBuilder.Entity<EncuestaCliente>().HasRequired(a => a.TrabajoProveedor).WithOptional(u => u.EncuestaCliente).Map(m => m.MapKey("TrabajoProveedorId"));
            //modelBuilder.Entity<EncuestaCliente>().HasRequired(a => a.TrabajoProveedor).WithOptional(u => u.EncuestaCliente);
            //modelBuilder.Entity<TrabajoProveedor>().HasRequired(a => a.EncuestaCliente).WithMany().HasForeignKey(u => u.EncuestaClienteId);
        }
        #endregion

        #region Seeds
        internal void Seed()
        {
            SeedConfiguraciones();
            SeedsRoles();
            var tiposServicios = SeedsTiposServicios();
            var distritos = SeedPaisesCiudadesDistritos();
            var personasNaturales = SeedPersonasNaturales(20); // n personas en la BD
            var personasJuridicas = SeedPersonasJuridicas();
            var personas = personasNaturales.Concat(personasJuridicas).ToList();
            SeedUbicacionesPersonas(personas, distritos);
            var proveedores = SeedProveedores(personas, tiposServicios);
            var suministradores = SeedSuministradores(personasJuridicas);
            var clientes = SeedClientes(personas);
            SeedRecargaLeads(suministradores, proveedores);
            var trabajos = SeedTrabajos(clientes, 3); //nro clientes * factorMultiplicativo
            var trabajosProveedores = SeedTrabajosProveedores(proveedores, trabajos, tiposServicios);
            var criteriosCalificacion = SeedCriteriosCalificacion();
            var encuestas = SeedEncuestasCliente(trabajosProveedores);
            var respuestasXcriterio = SeedRespuestasPorCriterio(encuestas, criteriosCalificacion);
            ActualizarIndicesEstadísticosProveedor();
            //faltaria puntajepromediocriterio
            //faltaria actualizar campos tabla proveedor
            SeedImagenesPersonas(personas);
            var categoriasProductos = SeedCategoriasProductos();
            SeedProductos(suministradores, categoriasProductos, 2);
        }

        private void SeedProductos(List<Suministrador> suministradores, List<CategoriaProducto> categoriasProductos, int nroproductosXsuministrador)
        {
            int r1, r2, r3, r4, r5, r6;
            Random random = new Random();
            //string[] letras = { "1A", "1B", "1C", "2A", "2B", "2C", "3A", "3B", "3C", "4A", "4B", "4C", "5A", "5B", "5C" };
            string[] letras = { "A", "B", "C", "D", "E", "F" };
            string[] numeros = { "1", "2", "3", "4", "5", "6" };
            List<Producto> listaProductos = new List<Producto>();
            List<Imagen> listaImagenes = new List<Imagen>();
            foreach (var suministrador in suministradores)
            {
                //nroproductosXsuministrador = (nroproductosXsuministrador > 3) ? 3 : nroproductosXsuministrador;
                //cada suministrador tendra N productos por cada categoria
                for (int i = 0; i < nroproductosXsuministrador; i++)
                {
                    foreach (var categoria in categoriasProductos)
                    {
                        r1 = random.Next(0, letras.Count());
                        r2 = random.Next(0, numeros.Count());
                        r3 = random.Next(0, 21);
                        r4 = random.Next(0, 21);
                        r5 = random.Next(2, 15);
                        //asumiremos que las imagenes van del 1 al 3, p. ej: tablero1.jpg, tablero2.jpg, tablero3.jpg
                        r6 = random.Next(1, 4);
                        string filename = categoria.DescripcionCategoria.ToLower() + r6;
                        //string filename = "tablero1";

                        string path = HostingEnvironment.ApplicationPhysicalPath + "Images\\Productos\\" + filename + ".jpg";
                        byte[] bytes = System.IO.File.ReadAllBytes(path);
                        Imagen imagen = new Imagen { Data = bytes, Nombre = filename, Mime = "image/jpg" };
                        listaImagenes.Add(imagen);

                        Producto producto = new Producto
                        {
                            NombreCorto = categoria.DescripcionCategoria + " " + letras[r1] + numeros[r2],
                            SuministradorId = suministrador.SuministradorId,
                            CategoriaProductoId = categoria.CategoriaProductoId,
                            NombreCompleto = categoria.DescripcionCategoria + " " + letras[r1] + numeros[r2],
                            Descripcion = "Producto de la categoría '" + categoria.NombreCategoria + "'",
                            Precio = categoria.PrecioPromedio + r5,
                            NroClicksVisita = r3,
                            NroBusquedas = r4,
                            FechaRegistro = DateTime.Now,
                            IsVisible = 1,
                            IsEliminado = 0
                        };
                        listaProductos.Add(producto);
                    }
                }
            }

            listaImagenes.ForEach(s => this.Imagenes.Add(s));
            this.SaveChanges();

            //Sabemos que hay una imagen por cada uno de los productos
            int k = 0;
            foreach (var imagen in listaImagenes)
            {
                //Producto producto = listaProductos[k];
                listaProductos[k].ImagenId = imagen.ImagenId;
                this.Productos.Add(listaProductos[k]);
                k++;
            }
            this.SaveChanges();
        }

        private List<CategoriaProducto> SeedCategoriasProductos()
        {
            var listaCategoriaProducto = new List<CategoriaProducto>()
            {
                new CategoriaProducto { NombreCategoria = "Madera y Tablas", DescripcionCategoria = "Tablero", IsEliminado = 0, PrecioPromedio = 10.90},
                new CategoriaProducto { NombreCategoria = "Fierro/Hierro", DescripcionCategoria = "Fierro", IsEliminado = 0, PrecioPromedio = 15.50},
                new CategoriaProducto { NombreCategoria = "Herramientas y Maquinarias", DescripcionCategoria = "Taladro", IsEliminado = 0, PrecioPromedio = 250.00},
                new CategoriaProducto { NombreCategoria = "Plomería/Gasfitería", DescripcionCategoria = "Tubo", IsEliminado = 0, PrecioPromedio = 8.50},
                new CategoriaProducto { NombreCategoria = "Ventanas", DescripcionCategoria = "Vidrio", IsEliminado = 0, PrecioPromedio = 12.30},
                new CategoriaProducto { NombreCategoria = "Electricidad", DescripcionCategoria = "Cable", IsEliminado = 0, PrecioPromedio = 30.50},
                new CategoriaProducto { NombreCategoria = "Cerrajería", DescripcionCategoria = "Cerradura", IsEliminado = 0, PrecioPromedio = 38.50},
                new CategoriaProducto { NombreCategoria = "Pintura", DescripcionCategoria = "Pintura", IsEliminado = 0, PrecioPromedio = 30.10},
                new CategoriaProducto { NombreCategoria = "Pisos", DescripcionCategoria = "Baldosa", IsEliminado = 0, PrecioPromedio = 23.40},
            };
            listaCategoriaProducto.ForEach(s => this.CategoriasProducto.Add(s));
            this.SaveChanges();
            return listaCategoriaProducto;
        }

        private void SeedConfiguraciones()
        {
            var listaConfiguraciones = new List<Configuracion>()
            {
                new Configuracion { Nombre = "PuntuacionMinimaAlgoritmo", Descripcion = "Puntuación mínima requerida del proveedor para ser considerado en la lógica del algoritmo", ValorNumerico = 12},
                new Configuracion { Nombre = "CantidadMaximaProveedoresAlgoritmo", Descripcion = "Cantidad máxima de proveedores que se devuelven al buscar proveedores dado un servicio, para la lógica del algoritmo", ValorNumerico = 100},
                new Configuracion { Nombre = "LeadsGratisRegistro", Descripcion = "Leads gratis al registrarse", ValorNumerico = 2},
                new Configuracion { Nombre = "PuntajePromedioInicialProveedores", Descripcion = "Puntaje Promedio Inicial Proveedores", ValorNumerico = 14}
            };
            listaConfiguraciones.ForEach(s => this.Configuraciones.Add(s));
            this.SaveChanges();
        }

        private void SeedsRoles()
        {
            //Seed of Roles & Permissions
            //Administrator
            if (!Roles.RoleExists("Administrador"))
                Roles.CreateRole("Administrador");

            if (!WebSecurity.UserExists("46394691"))
            {
                WebSecurity.CreateUserAndAccount("46394691", "admin", propertyValues: new
                {
                    TipoPersona = "Natural",
                    TipoUsuario = "Administrador",
                    DNI = 46394691,
                    PrimerNombre = "Christian",
                    ApellidoPaterno = "Mendez",
                    ApellidoMaterno = "Anchante",
                    Sexo = "Masculino",
                    FechaNacimiento = DateTime.Parse("01/12/1989"),
                    Email1 = "c.mendez@pucp.pe",
                    Telefono1 = "998560870",
                    UltimaActualizacionPersonal = DateTime.Now,
                    IsHabilitado = 1,
                    IsEliminado = 0
                });
            }

            if (!Roles.GetRolesForUser("46394691").Contains("Administrador"))
                Roles.AddUsersToRoles(new[] { "46394691" }, new[] { "Administrador" });

            if (!Roles.RoleExists("Cliente"))
                Roles.CreateRole("Cliente");
            if (!Roles.RoleExists("Proveedor"))
                Roles.CreateRole("Proveedor");
            if (!Roles.RoleExists("Suministrador"))
                Roles.CreateRole("Suministrador");
        }

        private List<TipoServicio> SeedsTiposServicios()
        {
            var listTiposServicios = new List<TipoServicio>()
            {
                new TipoServicio { NombreServicio = "Carpintería", DescripcionServicio = "Servicio de Carpintería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Cerrajería", DescripcionServicio = "Servicio de Cerrajería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Drywall", DescripcionServicio = "Servicio de Drywall", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Electricidad",DescripcionServicio = "Servicio de Electricidad", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Gasfitería", DescripcionServicio = "Servicio de Gasfitería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Melamina", DescripcionServicio = "Servicio de Melamina", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Pintura", DescripcionServicio = "Servicio de Pintura", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Vidriería", DescripcionServicio = "Servicio de Vidriería", IsEliminado = 0}
            };
            listTiposServicios.ForEach(s => this.TipoServicios.Add(s));
            this.SaveChanges();
            return listTiposServicios;
        }

        private List<Distrito> SeedPaisesCiudadesDistritos()
        {
            var listaPaisesCiudades = new List<PaisCiudad>()
            {
                new PaisCiudad { NombrePais = "Perú", NombreCiudad = "Lima", IsVisible = 1, IsEliminado = 0}
            };
            listaPaisesCiudades.ForEach(s => this.PaisesCiudades.Add(s));
            this.SaveChanges();

            var ciudadId = listaPaisesCiudades[0].PaisCiudadId;
            var listaDistritos = new List<Distrito>()
            {
                new Distrito{NombreDistrito = "San Borja", PaisCiudadId = ciudadId, LatitudDefault = -12.08611459617003, LongitudDefault = -77.00229406356812, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Pueblo Libre", PaisCiudadId = ciudadId, LatitudDefault = -12.072673294033422, LongitudDefault = -77.0689845085144, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "San Miguel", PaisCiudadId = ciudadId, LatitudDefault = -12.076534155680132, LongitudDefault = -77.084219455719, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "La Molina", PaisCiudadId = ciudadId, LatitudDefault = -12.084003925605176, LongitudDefault = -76.9336295127868, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Los Olivos", PaisCiudadId = ciudadId, LatitudDefault = -11.972271408716203, LongitudDefault =  -77.0740485191345, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Lince", PaisCiudadId = ciudadId, LatitudDefault = -12.084423569416773, LongitudDefault = -77.040274143219, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Ate", PaisCiudadId = ciudadId, LatitudDefault = -12.057426674697998, LongitudDefault = -76.9339513778686, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Chorrillos", PaisCiudadId = ciudadId, LatitudDefault = -12.17801716046097, LongitudDefault = -77.01849460601807, IsVisible = 1, IsEliminado = 0}
            };
            listaDistritos.Sort((x, y) => string.Compare(x.NombreDistrito, y.NombreDistrito));
            listaDistritos.ForEach(s => this.Distritos.Add(s));
            this.SaveChanges();

            return listaDistritos;
        }

        private List<Persona> SeedPersonasNaturales(int n)
        {
            var listPersonas = new List<Persona>();
            string[] nombres = { "Juan", "Alberto", "Pedro", "David", "Alfredo", "Renato", "Marcos", "Lucas", "Raúl", "Eduardo", "Cristopher", "Toribio" };
            string[] apellidos = { "Lopez", "Vidal", "Guerra", "Garcia", "Alvarez", "Dominguez", "Rodriguez", "Balcazar", "Quintana", "Taboada", "Córdova", "Suarez" };
            string[] documentos = { "46394691", "86735959", "34896582", "70688569", "42384465", "41774584", "26335963", "37855213", "58765115", "31669569", "33287845", "42542398" };
            string[] tipoUsuario = { "Cliente", "Cliente", "Proveedor", "Proveedor", "Proveedor", "Proveedor", "Proveedor", "Proveedor", "Proveedor" };
            List<long> docs = new List<long>();
            long d1;
            int r1, r2, r3, r4;
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                do
                {
                    r1 = random.Next(0, 12);
                    r2 = random.Next(0, 12);
                    r3 = random.Next(0, 3);
                    r4 = random.Next(0, tipoUsuario.Count());
                    d1 = (Int64.Parse(documentos[r1]) + Int64.Parse(documentos[r2])) / 2;
                } while (docs.Contains(d1));

                docs.Add(d1);

                Persona p = new Persona
                {
                    UserName = d1.ToString(),
                    TipoPersona = "Natural",
                    TipoUsuario = tipoUsuario[r4],
                    DNI = unchecked((int)d1),
                    PrimerNombre = nombres[r1],
                    ApellidoMaterno = apellidos[r1],
                    ApellidoPaterno = apellidos[r2],
                    FechaNacimiento = DateTime.Now.AddYears((r1 + 30 + r3 * 4) * -1),
                    Sexo = "Masculino",
                    //DireccionCompleta = persona.DireccionCompleta,
                    Email1 = nombres[r1].Substring(0, 1).ToLower() + "." + apellidos[r1].ToLower() + "@gmail.com",
                    Telefono1 = d1.ToString(),
                    //ImagenPrincipal = persona.ImagenPrincipal,
                    UltimaActualizacionPersonal = DateTime.Now,
                    IsHabilitado = 1, //true
                    IsEliminado = 0 //false                 
                };
                listPersonas.Add(p);
            }

            listPersonas.ForEach(s => this.Personas.Add(s));
            this.SaveChanges();

            foreach (var persona in listPersonas)
            {
                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { persona.TipoUsuario });
                WebSecurity.CreateAccount(persona.UserName, "asdasdasd");
            }
            var admin = this.Personas.Find(1);
            listPersonas.Add(admin);

            return listPersonas;
        }

        private List<Persona> SeedPersonasJuridicas()
        {
            var listPersonas = new List<Persona>();
            string[] razonesSociales = { "J&R", "ABC", "XYZ", "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Lambda", "Omega" };
            string[] documentos = { "20046394691", "20086735959", "20034896582", "20070688569", "20042384465", "20041774584", "20026335963", "20037855213", "20058765115", "20031669569" };
            string[] tipoUsuario = { "Cliente", "Proveedor", "Proveedor", "Proveedor", "Proveedor", "Suministrador", "Suministrador", "Suministrador", "Suministrador", "Suministrador" };
            List<long> docs = new List<long>();
            long d1;
            int r1, r2, r3, r4;
            Random random = new Random();
            for (int i = 0; i < razonesSociales.Count(); i++)
            {
                r1 = random.Next(0, 12);
                r2 = random.Next(0, 12);
                r3 = random.Next(0, 3);

                Persona p = new Persona
                {
                    UserName = documentos[i],
                    TipoPersona = "Juridica",
                    TipoUsuario = tipoUsuario[i],
                    RUC = Int64.Parse(documentos[i]),
                    RazonSocial = razonesSociales[i] + " S.A.C.",
                    FechaCreacion = DateTime.Now.AddYears((r1 + 5 + r3 * 4) * -1),
                    Email1 = razonesSociales[i].ToLower() + "@gmail.com",
                    Telefono1 = documentos[i].Substring(4, 7).ToString(),
                    //ImagenPrincipal = persona.ImagenPrincipal,
                    UltimaActualizacionPersonal = DateTime.Now,
                    IsHabilitado = 1, //true
                    IsEliminado = 0 //false                 
                };
                listPersonas.Add(p);
            }

            listPersonas.ForEach(s => this.Personas.Add(s));
            this.SaveChanges();

            foreach (var persona in listPersonas)
            {
                Roles.AddUsersToRoles(new[] { persona.UserName }, new[] { persona.TipoUsuario });
                WebSecurity.CreateAccount(persona.UserName, "asdasdasd");
            }

            return listPersonas;
        }

        private void SeedUbicacionesPersonas(List<Persona> personas, List<Distrito> distritos)
        {
            int r1, r2, r3, r4, r5, r6;
            var listaUbicaciones = new List<UbicacionPersona>();
            Random random = new Random();
            foreach (var persona in personas)
            {
                r1 = random.Next(0, distritos.Count());
                r2 = random.Next(0, 2);
                r3 = random.Next(0, 2);
                r4 = random.Next(1, 1);
                r5 = random.Next(1, 1);
                r6 = random.Next(201, 500);
                if (r6 < 100)
                    r6 *= 2;

                Distrito distrito = distritos[r1];
                string direccion = "";
                if (distrito.NombreDistrito == "Ate") direccion = "Av. Nicolás Ayllón " + r6.ToString();
                if (distrito.NombreDistrito == "Chorrillos") direccion = "Av. Huaylas " + r6.ToString();
                if (distrito.NombreDistrito == "La Molina") direccion = "Av. La Molina " + r6.ToString();
                if (distrito.NombreDistrito == "Lince") direccion = "Av. Arequipa " + r5.ToString();
                if (distrito.NombreDistrito == "Los Olivos") direccion = "Av. Universitaria " + r6.ToString();
                if (distrito.NombreDistrito == "Pueblo Libre") direccion = "Av. Bolivar " + r6.ToString();
                if (distrito.NombreDistrito == "San Borja") direccion = "Av. Aviación " + r6.ToString();
                if (distrito.NombreDistrito == "San Miguel") direccion = "Av. La Marina " + r6.ToString();

                int signo1 = ((int)r2 == 0) ? 1 : -1;
                int signo2 = ((int)r3 == 0) ? 1 : -1;

                double lat = 0.01778431163546 * signo1 * r4;
                double lon = 0.00641584396361 * signo2 * r5;

                UbicacionPersona ubicacion = new UbicacionPersona
                {
                    PersonaId = persona.PersonaId,
                    DistritoId = distrito.DistritoId,
                    Direccion = direccion,
                    Referencia = " ",
                    Latitud = distrito.LatitudDefault + lat,
                    Longitud = distrito.LongitudDefault + lon,
                    IsVisible = 1,
                    IsEliminado = 0
                };
                listaUbicaciones.Add(ubicacion);
                persona.DireccionCompleta = ubicacion.Direccion + " - " + distrito.NombreDistrito;
                this.Personas.Attach(persona);
                this.Entry(persona).State = EntityState.Modified;
            }
            listaUbicaciones.ForEach(s => this.UbicacionesPersonas.Add(s));

            this.SaveChanges();
        }

        private List<Proveedor> SeedProveedores(List<Persona> personas, List<TipoServicio> tiposServicios)
        {
            int r1, r2, r3, r4, r5, r6;
            var listaProveedores = new List<Proveedor>();
            Random random = new Random();
            foreach (var persona in personas)
            {
                r1 = random.Next(10, 26);
                r3 = random.Next(10, 21);

                if (persona.TipoUsuario == "Proveedor")
                {
                    string t = "";
                    if (persona.TipoPersona == "Natural") t = "Maestro";
                    if (persona.TipoPersona == "Juridica") t = "Empresa";

                    Proveedor proveedor = new Proveedor
                    {
                        PersonaId = persona.PersonaId,
                        LeadsDisponibles = 2,
                        PuntuacionPromedio = r3,
                        NroTrabajosTerminados = 0,
                        NroBusquedasCliente = r1,
                        NroClicksVisita = r3,
                        NroComentarios = 0,
                        NroCalificaciones = 0,
                        NroRecomendaciones = 0,
                        NroVolveriaContratarlo = 0,
                        PaginaWeb = "",
                        Facebook = "",
                        AcercaDeMi = t + " con " + r1.ToString() + " años de experiencia.",
                        IsDestacado = 0
                    };

                    proveedor.TiposServicios = new List<TipoServicio>();

                    while (proveedor.TiposServicios.Count < 2)
                    {
                        r2 = random.Next(0, tiposServicios.Count());
                        TipoServicio tipo = tiposServicios[r2];
                        if (!proveedor.TiposServicios.Contains(tipo))
                            proveedor.TiposServicios.Add(tipo);
                    }

                    listaProveedores.Add(proveedor);
                }
            }
            listaProveedores.ForEach(s => this.Proveedores.Add(s));
            this.SaveChanges();
            return listaProveedores;
        }

        private List<Suministrador> SeedSuministradores(List<Persona> personas)
        {
            int r1, r2;
            Random random = new Random();
            var listaSuministradores = new List<Suministrador>();

            foreach (var persona in personas)
            {
                if (persona.TipoUsuario == "Suministrador")
                {
                    r1 = random.Next(20, 81);
                    r2 = random.Next(5, 16);
                    Suministrador suministrador = new Suministrador
                    {
                        PersonaId = persona.PersonaId,
                        LeadsDisponibles = r1,
                        LeadsReserva = r2,
                        PaginaWeb = "",
                        Facebook = "",
                        AcercaDeMi = "",
                        IsDestacado = 0
                    };
                    listaSuministradores.Add(suministrador);
                }
            }
            listaSuministradores.ForEach(s => this.Suministradores.Add(s));
            this.SaveChanges();

            return listaSuministradores;
        }

        private List<Cliente> SeedClientes(List<Persona> personas)
        {
            var listaClientes = new List<Cliente>();

            foreach (var persona in personas)
            {
                if (persona.TipoUsuario == "Cliente")
                {
                    Cliente cliente = new Cliente
                    {
                        PersonaId = persona.PersonaId
                    };
                    listaClientes.Add(cliente);
                }
            }
            listaClientes.ForEach(s => this.Clientes.Add(s));
            this.SaveChanges();
            return listaClientes;
        }

        private void SeedRecargaLeads(List<Suministrador> suministradores, List<Proveedor> proveedores)
        {
            var listaRecargas = new List<RecargaLeads>();
            int r1, r2, r3;
            Random random = new Random();

            foreach (var suministrador in suministradores)
            {
                r1 = random.Next(1, 6);
                for (int i = 0; i < r1; i++)
                {
                    r2 = random.Next(0, proveedores.Count);
                    r3 = random.Next(5, 21);
                    RecargaLeads recarga = new RecargaLeads
                    {
                        SuministradorId = suministrador.SuministradorId,
                        ProveedorId = proveedores[r2].ProveedorId,
                        FechaRecarga = DateTime.Now.AddMonths((r1 + r2 / 2) * -1).AddDays(r1 + r2 / 2).AddHours(r1).AddMinutes(r1 + r3),
                        MontoRecarga = r3,
                        TipoMoneda = "Soles",
                        CantidadLeads = r3
                    };
                    listaRecargas.Add(recarga);
                }
            }
            listaRecargas.Sort((x, y) => DateTime.Compare(x.FechaRecarga, y.FechaRecarga));
            listaRecargas.ForEach(s => this.RecargasLeads.Add(s));
            this.SaveChanges();
        }

        private List<Trabajo> SeedTrabajos(List<Cliente> clientes, int nroTrabajos)
        {
            var listaTrabajos = new List<Trabajo>();
            int r1, r2, r3;
            Random random = new Random();

            for (int i = 0; i < nroTrabajos; i++)
            {
                foreach (var cliente in clientes)
                {
                    r1 = random.Next(1, 6);
                    r2 = random.Next(0, clientes.Count);
                    r3 = random.Next(5, 21);

                    Trabajo trabajo = new Trabajo
                    {
                        ClienteId = cliente.ClienteId,
                        DescripcionCliente = "Trabajo nro. " + r3,
                        Direccion = cliente.Persona.DireccionCompleta,
                        Fecha = DateTime.Now.AddMonths((r1 + r2 / 2) * -1).AddDays(r1 + r2 / 2).AddHours(r1).AddMinutes(r1 + r3),
                    };

                    listaTrabajos.Add(trabajo);
                }
            }

            listaTrabajos.ForEach(s => this.Trabajos.Add(s));
            this.SaveChanges();
            return listaTrabajos;
        }

        private List<TrabajoProveedor> SeedTrabajosProveedores(List<Proveedor> proveedores, List<Trabajo> trabajos, List<TipoServicio> tiposServicios)
        {
            var listaTrabajosProveedores = new List<TrabajoProveedor>();
            int r1, r2, r3, r4, r5;
            Random random = new Random();
            foreach (var trabajo in trabajos)
            {
                r1 = random.Next(1, 4); //nro de servicios requeridos en cada trabajo
                var auxServ = new List<TipoServicio>(tiposServicios);
                //auxServ.AddRange(tiposServicios);
                var listaServiciosTrabajo = new List<TipoServicio>();

                for (int i = 1; i <= r1; i++)
                {
                    r2 = random.Next(0, auxServ.Count);
                    var serv = auxServ[r2];
                    listaServiciosTrabajo.Add(serv);
                    auxServ.RemoveAt(r2);
                }

                var auxProv = new List<Proveedor>(proveedores);
                //auxProv.AddRange(proveedores);
                var listaProveedoresTrabajo = new List<Proveedor>();

                foreach (var tipoServicio in listaServiciosTrabajo)
                {
                    while (true)
                    {
                        r3 = random.Next(0, auxProv.Count);
                        var prov = auxProv[r3];
                        var servXprov = prov.TiposServicios.ToList();
                        var esta = servXprov.Exists(s => s.TipoServicioId == tipoServicio.TipoServicioId);
                        if (esta)
                        {
                            listaProveedoresTrabajo.Add(prov);
                            auxProv.RemoveAt(r3);
                            break;
                        }
                    }
                }

                for (int i = 0; i < r1; i++)
                {
                    r4 = random.Next(8, 35) * 10;
                    r5 = random.Next(222222, 999999);

                    TrabajoProveedor trabajoProveedor = new TrabajoProveedor
                    {
                        TrabajoId = trabajo.TrabajoId,
                        ProveedorId = listaProveedoresTrabajo[i].ProveedorId,
                        DescripcionProveedor = "",
                        MontoCobrado = r4.ToString(),
                        NroRpH_Factura = r5.ToString(),
                        TipoRpH_Factura = "Recibo por Honorarios",
                        TiposServicios = new List<TipoServicio>(),
                        IsTerminado = 1
                    };
                    trabajoProveedor.TiposServicios.Add(listaServiciosTrabajo[i]);
                    listaTrabajosProveedores.Add(trabajoProveedor);
                }
            }
            listaTrabajosProveedores.ForEach(s => this.TrabajosProveedores.Add(s));
            this.SaveChanges();
            return listaTrabajosProveedores;
        }

        private List<CriterioCalificacion> SeedCriteriosCalificacion()
        {
            var listaCriterios = new List<CriterioCalificacion>()
            {
                new CriterioCalificacion { NombreCriterio = "Calidad", TipoPregunta = "Estrellas",PreguntaAsociada = "Califique la calidad del servicio recibido", PuntajeMaximo = 5, IsEliminado = 0},
                new CriterioCalificacion { NombreCriterio = "Compromiso", TipoPregunta = "Estrellas", PreguntaAsociada = "Califique el compromiso con el trabajo", PuntajeMaximo = 5, IsEliminado = 0},
                new CriterioCalificacion { NombreCriterio = "Trato y Cortesía", TipoPregunta = "Estrellas", PreguntaAsociada = "Califique el trato y cortesía del proveedor ", PuntajeMaximo = 5, IsEliminado = 0},
                new CriterioCalificacion { NombreCriterio = "Puntualidad", TipoPregunta = "Estrellas", PreguntaAsociada = "Califique la puntualidad", PuntajeMaximo = 5, IsEliminado = 0},
                new CriterioCalificacion { NombreCriterio = "Precio cobrado", TipoPregunta = "Estrellas", PreguntaAsociada = "Califique el precio cobrado por el proveedor", PuntajeMaximo = 5, IsEliminado = 0},
                new CriterioCalificacion { NombreCriterio = "Volvería a contratarlo", TipoPregunta = "Si-No", PreguntaAsociada = "¿Volvería a contratar a este proveedor?", PuntajeMaximo = 1, IsEliminado = 0},
                new CriterioCalificacion { NombreCriterio = "Recomendaría", TipoPregunta = "Si-No", PreguntaAsociada = "¿Recomendaría a este proveedor?", PuntajeMaximo = 1, IsEliminado = 0}
                //new CriterioCalificacion { NombreCriterio = "Comentarios", PreguntaAsociada = "Comentarios", PuntajeMaximo = -1, IsEliminado = 0}
            };

            listaCriterios.ForEach(s => this.CriteriosCalificacion.Add(s));
            this.SaveChanges();
            return listaCriterios;
        }

        private List<EncuestaCliente> SeedEncuestasCliente(List<TrabajoProveedor> trabajosProveedores)
        {
            var listaEncuestas = new List<EncuestaCliente>();
            int r1, r2, r3;
            Random random = new Random();
            foreach (var trabajo in trabajosProveedores)
            {
                r1 = random.Next(3, 8);
                r2 = random.Next(0, trabajosProveedores.Count);
                r3 = random.Next(5, 21);

                EncuestaCliente encuesta = new EncuestaCliente
                {
                    //TrabajoProveedorId = trabajo.TrabajoProveedorId,
                    Fecha = trabajo.Trabajo.Fecha.AddDays(r1).AddHours(r1).AddMinutes(r1 + r3),
                    ComentariosProveedor = "Trabajo culminado al 100%",
                    PuntajeTotal = -1,
                    IsVisible = 1,
                    IsCompletada = 1,
                    IsEliminado = 0,
                };
                listaEncuestas.Add(encuesta);
            }
            listaEncuestas.ForEach(s => this.EncuestasClientes.Add(s));
            this.SaveChanges();
            //Update a TrabajosProveedores
            int i = 0;
            foreach (var encuestaCliente in listaEncuestas)
            {
                var trabajo = trabajosProveedores[i];
                trabajo.EncuestaClienteId = encuestaCliente.EncuestaClienteId;
                this.TrabajosProveedores.Attach(trabajo);
                //this.Entry(trabajo).State = EntityState.Modified;
                var entry = this.Entry(trabajo);
                entry.Property(e => e.EncuestaClienteId).IsModified = true; //otra forma de indicar el Update
                i++;
            }
            this.SaveChanges();
            //para hacer la relación 1 a 1
            this.Database.ExecuteSqlCommand("ALTER TABLE TrabajosProveedores ADD CONSTRAINT uc_EncuestaCliente UNIQUE(EncuestaClienteId)");
            return listaEncuestas;
        }

        private List<RespuestaPorCriterio> SeedRespuestasPorCriterio(List<EncuestaCliente> encuestas, List<CriterioCalificacion> criteriosCalificacion)
        {
            var listaRespuestas = new List<RespuestaPorCriterio>();
            string[] comentariosCliente = { "Excelente trabajo. Recomendado", "Buen proveedor. Recomendado.", "Servicio regular.", "No lo recomiendo." };
            int r1, r2, r3;
            Random random = new Random();

            foreach (var encuesta in encuestas)
            {
                int suma = 0; int total = 0;
                foreach (var criterio in criteriosCalificacion)
                {
                    r1 = random.Next(2, 6);
                    int puntaje;

                    if (criterio.TipoPregunta == "Estrellas") //preguntas del 1 al 5
                    {
                        puntaje = r1;
                        total += 5;
                    }
                    else //preguntas del 6 y 7
                    {
                        puntaje = (suma >= 11) ? 1 : 0;
                        total += 1;
                    }

                    suma += puntaje;
                    RespuestaPorCriterio respuesta = new RespuestaPorCriterio
                    {
                        EncuestaClienteId = encuesta.EncuestaClienteId,
                        CriterioCalificacionId = criterio.CriterioCalificacionId,
                        PuntajeOtorgado = puntaje,
                    };
                    listaRespuestas.Add(respuesta);
                }
                double puntuacion = (suma * 20.0) / total;
                encuesta.PuntajeTotal = Convert.ToInt32(Math.Round(puntuacion, MidpointRounding.ToEven));
                if ((encuesta.PuntajeTotal >= 18) && (encuesta.PuntajeTotal <= 20))
                    encuesta.ComentariosCliente = comentariosCliente[0];
                if ((encuesta.PuntajeTotal >= 15) && (encuesta.PuntajeTotal <= 17))
                    encuesta.ComentariosCliente = comentariosCliente[1];
                if ((encuesta.PuntajeTotal >= 11) && (encuesta.PuntajeTotal <= 14))
                    encuesta.ComentariosCliente = comentariosCliente[2];
                if ((encuesta.PuntajeTotal >= 0) && (encuesta.PuntajeTotal <= 11))
                    encuesta.ComentariosCliente = comentariosCliente[3];
                this.EncuestasClientes.Attach(encuesta);
                this.Entry(encuesta).State = EntityState.Modified;
            }
            listaRespuestas.ForEach(s => this.RespuestasPorCriterio.Add(s));
            this.SaveChanges();
            return listaRespuestas;
        }

        private void ActualizarIndicesEstadísticosProveedor()
        {
            List<Proveedor> listaProveedores = this.Proveedores.ToList();
            foreach (var proveedor in listaProveedores)
            {
                if (proveedor.TrabajosProveedores != null)
                {
                    //Actualizar NroTrabajosTerminados
                    List<TrabajoProveedor> listaTrabajosProveedor = proveedor.TrabajosProveedores.ToList();
                    int puntaje = 0, recomendaciones = 0, volveria = 0;
                    proveedor.NroTrabajosTerminados = listaTrabajosProveedor.Count;
                    proveedor.NroComentarios = listaTrabajosProveedor.Count;

                    foreach (var trabajo in listaTrabajosProveedor)
                    {
                        puntaje += trabajo.EncuestaCliente.PuntajeTotal;
                        recomendaciones +=
                            trabajo.EncuestaCliente.RespuestasPorCriterio.Count(
                                r => (r.CriterioCalificacionId == 7) && (r.PuntajeOtorgado == 1));
                        volveria +=
                            trabajo.EncuestaCliente.RespuestasPorCriterio.Count(
                                r => (r.CriterioCalificacionId == 6) && (r.PuntajeOtorgado == 1));
                    }
                    proveedor.NroRecomendaciones = recomendaciones;
                    proveedor.NroVolveriaContratarlo = volveria;
                    //Cálculo de la puntuación promedio: acá consideramos PuntajeTotal / NroTrabajos
                    double puntuacion = (puntaje * 1.0) / proveedor.NroTrabajosTerminados;
                    proveedor.PuntuacionPromedio = Convert.ToInt32(Math.Round(puntuacion, MidpointRounding.ToEven));

                    this.Proveedores.Attach(proveedor);
                    this.Entry(proveedor).State = EntityState.Modified;
                }
            }
            this.SaveChanges();
        }

        private void SeedImagenesPersonas(List<Persona> personas)
        {
            List<Imagen> listaImagenes = new List<Imagen>();
            string path;
            int r1 = 0;
            string[] imagenesNaturales = { "perfil_1", "perfil_2", "perfil_3", "perfil_4", "perfil_5" };
            string[] imagenesJuridicos = { "tienda_1", "tienda_2", "tienda_3" };
            Random random = new Random();
            foreach (var persona in personas)
            {
                string filename = "";
                if (persona.TipoPersona == "Natural")
                {
                    r1 = random.Next(0, imagenesNaturales.Count());
                    filename = imagenesNaturales[r1];
                }
                else
                {
                    r1 = random.Next(0, imagenesJuridicos.Count());
                    filename = imagenesJuridicos[r1];
                }

                path = HostingEnvironment.ApplicationPhysicalPath + "Images\\Personas\\" + filename + ".jpg";
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                Imagen imagen = new Imagen { Data = bytes, Nombre = filename, Mime = "image/jpg" };
                listaImagenes.Add(imagen);
                //this.Imagenes.Add(imagen);

                //persona.ImagenId = imagen.ImagenId;
                //this.Personas.Attach(persona);
                //this.Entry(persona).State = EntityState.Modified;
            }

            listaImagenes.ForEach(s => this.Imagenes.Add(s));
            this.SaveChanges();

            int i = 0;
            foreach (var imagen in listaImagenes)
            {
                Persona persona = personas[i];
                persona.ImagenId = imagen.ImagenId;
                this.Personas.Attach(persona);
                this.Entry(persona).State = EntityState.Modified;
                i++;
            }
            this.SaveChanges();
        }
        #endregion
    }

    #region Strategy
    //public class SGPContextInitializer : DropCreateDatabaseTables<SGPContext>
    //public class SGPContextInitializer : DropCreateDatabaseAlways<SGPContext>
    //public class SGPContextInitializer : DropOnlyTables<SGPContext>
    public class SGPContextInitializer : DropCreateDatabaseIfModelChanges<SGPContext>
    {
        /*protected override void Seed(SGPContext context)
        {
            base.Seed(context);
            //context.seed();
        }*/
    }
    #endregion
}