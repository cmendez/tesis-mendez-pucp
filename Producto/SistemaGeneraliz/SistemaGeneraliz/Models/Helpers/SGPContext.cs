using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Data.Entity;
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
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Proveedor>().HasMany(r => r.TiposServicios).WithMany(t => t.Proveedores)
                .Map(t => t.MapLeftKey("ProveedorId").MapRightKey("TipoServicioId").ToTable("TiposServiciosPorProveedor"));
        }

        internal void Seed()
        {
            SeedsRoles();
            var tiposServicios = SeedsTiposServicios();
            var distritos = SeedPaisesCiudadesDistritos();
            var personasNaturales = SeedPersonasNaturales(10);
            var personasJuridicas = SeedPersonasJuridicas();
            var personas = personasNaturales.Concat(personasJuridicas).ToList();
            SeedUbicacionesPersonas(personas, distritos);
            var proveedores = SeedProveedores(personas, tiposServicios);
            var suministradores = SeedSuministradores(personas);
            var clientes = SeedClientes(personas);
            SeedRecargaLeads(suministradores, proveedores);
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
                    PrimerNombre = "Christian",
                    ApellidoPaterno = "Mendez",
                    DNI = 46394691,
                    UltimaActualizacionPersonal = DateTime.Now,
                    IsHabilitado = 0,
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
            string[] tipoPersona = { "Cliente", "Cliente", "Proveedor", "Proveedor", "Proveedor", "Proveedor", "Proveedor", "Proveedor", "Proveedor" };
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
                    r4 = random.Next(0, tipoPersona.Count());
                    d1 = (Int64.Parse(documentos[r1]) + Int64.Parse(documentos[r2])) / 2;
                } while (docs.Contains(d1));

                docs.Add(d1);

                Persona p = new Persona
                {
                    UserName = d1.ToString(),
                    TipoPersona = "Natural",
                    TipoUsuario = tipoPersona[r4],
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

            return listPersonas;
        }

        private List<Persona> SeedPersonasJuridicas()
        {
            var listPersonas = new List<Persona>();
            string[] razonesSociales = { "J&R", "ABC", "XYZ", "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Lambda", "Omega" };
            string[] documentos = { "20046394691", "20086735959", "20034896582", "20070688569", "20042384465", "20041774584", "20026335963", "20037855213", "20058765115", "20031669569" };
            string[] tipoPersona = { "Cliente", "Proveedor", "Proveedor", "Suministrador", "Suministrador", "Suministrador", "Suministrador", "Suministrador", "Suministrador", "Suministrador" };
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
                    TipoUsuario = tipoPersona[i],
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
                r4 = random.Next(1, 6);
                r5 = random.Next(1, 6);
                r6 = random.Next(201, 500);

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

                if (persona.TipoUsuario == "Proveedor")
                {
                    string t = "";
                    if (persona.TipoPersona == "Natural") t = "Maestro";
                    if (persona.TipoPersona == "Juridica") t = "Empresa";

                    Proveedor proveedor = new Proveedor
                    {
                        PersonaId = persona.PersonaId,
                        LeadsDisponibles = 2,
                        PuntuacionPromedio = 0,
                        NroTrabajosTerminados = 0,
                        NroBusquedasCliente = 0,
                        NroClicksVisita = 0,
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
    }

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
}