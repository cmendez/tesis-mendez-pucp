using System;
using System.Collections.Generic;
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
        //public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<Persona> Personas { get; set; }
        public IDbSet<Cliente> Clientes { get; set; }
        public IDbSet<Proveedor> Proveedores { get; set; }
        public IDbSet<Suministrador> Suministradores { get; set; }
        public IDbSet<TipoServicio> TipoServicios { get; set; }
        public IDbSet<PaisCiudad> PaisesCiudades { get; set; }
        public IDbSet<Distrito> Distritos { get; set; }
        public IDbSet<UbicacionPersona> UbicacionesPersonas { get; set; }
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
            SeedsTiposServicios();
            var distritos = SeedPaisesCiudadesDistritos();
            var personas = SeedPersonasNaturales(10);
            //SeedProveedores(personas);
            //SeedUbicacionesPersonas(personas, distritos);
        }

        private void SeedUbicacionesPersonas(List<Persona> personas, List<Distrito> distritos)
        {
            int r1;
            double r2;
            foreach (var persona in personas)
            {
                Random random = new Random();
                r1 = random.Next(0, distritos.Count());
                r2 = random.NextDouble();
                Distrito distrito = distritos[r1];

                /* punto 1
                 -12.08611459617
                -77.0022940635681
                punto 2
                -12.10389890780546
                 -76.99587821960449

                diferencia de dos puntos
                0.01778431163546
                0.00641584396361
                  
                podemos sumar o restar 0.01778431163546 a latitud y 0.00641584396361 a longitud
                 usar otra variable random para saber si debemos sumar o restar
                 */

                UbicacionPersona ubicacion = new UbicacionPersona
                {
                    PersonaId = persona.PersonaId,
                    DistritoId = distrito.DistritoId,
                    Direccion = "",
                    Referencia = "",
                    Latitud = distrito.LatitudDefault,
                    Longitud = distrito.LongitudDefault,
                    IsVisible = 1,
                    IsEliminado = 0
                };
            }
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
            string[] tipoPersona = { "Cliente", "Proveedor", "Suministrador" };
            List<long> docs = new List<long>();
            long d1;
            int r1, r2, r3;

            for (int i = 0; i < n; i++)
            {
                do
                {
                    Random random = new Random();
                    r1 = random.Next(0, 12);
                    r2 = random.Next(0, 12);
                    r3 = random.Next(0, 3);

                    d1 = (Int64.Parse(documentos[r1]) + Int64.Parse(documentos[r2])) / 2;
                } while (docs.Contains(d1));

                docs.Add(d1);

                Persona p = new Persona
                {
                    UserName = d1.ToString(),
                    TipoPersona = "Natural",
                    TipoUsuario = "Proveedor", //tipoPersona[r3],
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

        /* 
        private void SeedProveedores()
        {
            var listPersonas = new List<Persona>()
            {
                new TipoServicio { NombreServicio = "Carpintería", DescripcionServicio = "Servicio de Carpintería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Cerrajería", DescripcionServicio = "Servicio de Cerrajería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Drywall", DescripcionServicio = "Servicio de Drywall", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Electricidad",DescripcionServicio = "Servicio de Electricidad", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Gasfitería", DescripcionServicio = "Servicio de Gasfitería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Melamina", DescripcionServicio = "Servicio de Melamina", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Pintura", DescripcionServicio = "Servicio de Pintura", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Vidriería", DescripcionServicio = "Servicio de Vidriería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Carpintería", DescripcionServicio = "Servicio de Carpintería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Cerrajería", DescripcionServicio = "Servicio de Cerrajería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Drywall", DescripcionServicio = "Servicio de Drywall", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Electricidad",DescripcionServicio = "Servicio de Electricidad", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Gasfitería", DescripcionServicio = "Servicio de Gasfitería", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Melamina", DescripcionServicio = "Servicio de Melamina", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Pintura", DescripcionServicio = "Servicio de Pintura", IsEliminado = 0},
                new TipoServicio { NombreServicio = "Vidriería", DescripcionServicio = "Servicio de Vidriería", IsEliminado = 0}
            };

            var listProveedores = new List<Proveedor>()
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
            listProveedores.ForEach(s => this.Proveedores.Add(s));
            this.SaveChanges();
        }*/

        private void SeedsTiposServicios()
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