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
            //SeedProveedores();
            SeedPaisesCiudadesDistritos();
            //SeedDistritos();
            //SeedUbicacionesPersonas();
        }

        private void SeedPaisesCiudadesDistritos()
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
                new Distrito{NombreDistrito = "San Borja", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Pueblo Libre", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "San Miguel", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "La Molina", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Los Olivos", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Lince", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Ate", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0},
                new Distrito{NombreDistrito = "Chorrillos", PaisCiudadId = ciudadId, LatitudDefault = 0, LongitudDefault = 1, IsVisible = 1, IsEliminado = 0}
            };
            listaDistritos.Sort((x, y) => string.Compare(x.NombreDistrito, y.NombreDistrito));
            listaDistritos.ForEach(s => this.Distritos.Add(s));
            this.SaveChanges();
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