using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace SistemaGeneraliz.Models.Helpers
{
    public class SidebarNavigator
    {
        public List<SidebarOption> Opciones { get; set; }

        // Puedes encontar iconos en http://fortawesome.github.io/Font-Awesome/
        public SidebarNavigator()
        {

            Opciones = new List<SidebarOption>();
            // Agregue aqui  las opciones y subopciones del navegador de la barra de menu

            // Inicio
            Opciones.Add(new SidebarOption("", "Home", "Index", "Inicio", "icon-home"));

            // Clientes
            Opciones.Add(new SidebarOption("Clientes", "Clientes", "icon-user", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Buscar Proveedores", "Clientes", "MenuBuscarProveedores", "icon-search"),
                new SidebarSuboption("Buscar Productos", "Suministradores", "BuscarProductos", "icon-search"),
                new SidebarSuboption("Calificar Proveedores", "Clientes", "CalificarProveedores", "icon-star"),
                //new SidebarSuboption("Editar Mi Información", "Clientes", "Index", "icon-edit")
            })));

            // Proveedores
            Opciones.Add(new SidebarOption("Proveedores", "Proveedores", "icon-wrench", new List<SidebarSuboption>(new SidebarSuboption[]{
               new SidebarSuboption("Historial de Trabajos", "Proveedores", "HistorialTrabajos", "icon-list-ul"),
               //new SidebarSuboption("Historial de Recargas", "Proveedores", "Index", "icon-list-alt"), // evaluar si va o no va
               new SidebarSuboption("Buscar Productos", "Suministradores", "BuscarProductos", "icon-search"),
               new SidebarSuboption("Ofertas, Promociones y Descuentos", "Suministradores", "OfertasPromocionesDescuentos", "icon-tags"),
               //new SidebarSuboption("Comprar Ofertas, Promos y Dsctos", "Proveedores", "Index", "icon-shopping-cart"),
               new SidebarSuboption("Demanda de Servicios Generales", "Proveedores", "Demanda_ServiciosGenerales", "icon-bar-chart"),
               //new SidebarSuboption("Mi Calendario", "Proveedores", "Index", "icon-calendar"),
               //new SidebarSuboption("Editar Mi Información", "Proveedores", "Index", "icon-edit")
            })));

            // Suministradores
            Opciones.Add(new SidebarOption("Suministradores", "Suministradores", "icon-building", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Recargar Leads", "Suministradores", "RecargarLeads", "icon-bolt"),
                new SidebarSuboption("Historial de Ventas", "Suministradores", "HistorialVentasOfertasPromosDsctos", "icon-shopping-cart"),
                new SidebarSuboption("Demanda de Productos", "Suministradores", "Demanda_Productos", "icon-bar-chart"),
                new SidebarSuboption("Demanda de Ofertas, Promos y Dsctos", "Suministradores", "Demanda_OfertasPromosDsctos", "icon-bar-chart"),
                //new SidebarSuboption("Demanda de Servicios Generales", "Suministradores", "Index", "icon-bar-chart"),
                new SidebarSuboption("Editar Productos", "Suministradores", "EditarProductos", "icon-cog"),
                new SidebarSuboption("Editar Ofertas, Promos y Dsctos", "Suministradores", "EditarOfertasPromosDsctos", "icon-cog"),
                //new SidebarSuboption("Editar Mi Información", "Suministradores", "Index", "icon-edit")
            })));

            //if ((WebSecurity.IsAuthenticated) && (Roles.GetRolesForUser()[0] == "Administrador"))
            //{
                // Administración
                Opciones.Add(new SidebarOption("Administracion", "Administración", "icon-lock", new List<SidebarSuboption>(new SidebarSuboption[]{
                //new SidebarSuboption("Registrar Suministrador","Suministradores","RegistrarSuministradorJuridico","icon-building"),
                new SidebarSuboption("Administrar Usuarios","Administracion","AdministrarUsuarios","icon-group"),
                new SidebarSuboption("Configurar Parámetros", "Administracion", "ConfigurarParametros", "icon-cogs"),
                new SidebarSuboption("Conversión de Leads", "Administracion", "ConversionLeads", "icon-bar-chart"),
                //new SidebarSuboption("Administrar Usuarios","Usuarios","Index","icon-group", new List<SidebarSuboption>(new SidebarSuboption[]
                //                                                                                                            {
                //                                                                                                                new SidebarSuboption("Registrar Suministrador","Suministradores","RegistrarSuministradorJuridico","icon-building")
                //                                                                                                            })),
                //new SidebarSuboption("Configurar Ubicaciones", "Administracion", "Index", "icon-globe"),
                //new SidebarSuboption("Configurar Servicios", "Administracion", "Index", "icon-cogs"),
                new SidebarSuboption("Histórico de Trabajos", "Administracion", "HistoricoTrabajos", "icon-list-ul"),
                new SidebarSuboption("Proveedores Destacados", "Administracion", "ProveedoresDestacados", "icon-star"),
                //new SidebarSuboption("Gestión de Recompensas", "Administracion", "Index", "icon-gift"),
                
            })));
            //}
        }
    }

    public class SidebarOption
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<SidebarSuboption> Suboptions { get; set; }

        public SidebarOption() { }
        public SidebarOption(string area, string text, string icon, List<SidebarSuboption> suboptions) : this(area, null, null, text, icon, suboptions) { }
        public SidebarOption(string area, string controller, string method, string text, string icon) : this(area, controller, method, text, icon, new List<SidebarSuboption>()) { }
        private SidebarOption(string area, string controller, string method, string text, string icon, List<SidebarSuboption> suboptions)
        {
            Area = area;
            Title = text;
            Icon = icon;
            Suboptions = suboptions;
            Controller = controller;
            Method = method;
        }
    }

    public class SidebarSuboption
    {
        public string Title { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string Icon { get; set; }
        public List<SidebarSuboption> Suboptions { get; set; }
        public SidebarSuboption(string title, string controller, string method, string icon)
        {
            Title = title;
            Icon = icon;
            Controller = controller;
            Method = method;
        }
        public SidebarSuboption(string title, string controller, string method, string icon, List<SidebarSuboption> suboptions)
        {
            Title = title;
            Icon = icon;
            Controller = controller;
            Method = method;
            Suboptions = suboptions;
        }
    }

    public class ObtenerMenu
    {
        public SidebarNavigator menu { get; set; }

        public ObtenerMenu()
        {
            menu = new SidebarNavigator();
        }

        public SidebarNavigator MenuUsuario(string username)
        {
            SidebarNavigator salida = new SidebarNavigator();
            salida.Opciones.Clear();
            salida.Opciones = new List<SidebarOption>();

            //using (DP2Context context = new DP2Context())
            {
                //UsuarioDTO logeo = context.TablaUsuarios.One(i => i.Username == username).ToDTO();
                foreach (SidebarOption option in menu.Opciones)
                {
                    if (option.Suboptions.Count == 0)
                    {
                        //if (logeo.Roles.Where(c => c.Nombre == option.Controller).Where(c => c.Permiso == true).Count() == 1)
                        {
                            salida.Opciones.Add(new SidebarOption(option.Area, option.Controller, option.Method, option.Title, option.Icon));
                        }

                    }
                    else
                    {
                        salida.Opciones.Add(new SidebarOption(option.Area, option.Title, option.Icon, new List<SidebarSuboption>()));

                        foreach (SidebarSuboption subopt in option.Suboptions)
                        {
                            //if (logeo.Roles.Where(c => c.Nombre == subopt.Controller).Where(c => c.Permiso == true).Count() == 1)
                            {
                                SidebarSuboption aux = new SidebarSuboption(subopt.Title, subopt.Controller, subopt.Method, subopt.Icon);
                                salida.Opciones.Where(i => i.Area == option.Area).SingleOrDefault().Suboptions.Add(aux);
                                //if (subopt.Suboptions != null)
                                //{
                                //    foreach (SidebarSuboption subopt2 in subopt.Suboptions)
                                //    {
                                //        SidebarSuboption aux2 = new SidebarSuboption(subopt2.Title, subopt2.Controller, subopt2.Method, subopt2.Icon);
                                //        //subopt.Suboptions.Add(aux2);
                                //        salida.Opciones.Where(i => i.Area == option.Area).SingleOrDefault().Suboptions.Where(t => t.Title == "Administrar Usuarios").SingleOrDefault().Suboptions.Add(aux2);
                                //    }
                                //}
                            }

                        }
                    }
                }
            }

            return salida;
        }
    }
}