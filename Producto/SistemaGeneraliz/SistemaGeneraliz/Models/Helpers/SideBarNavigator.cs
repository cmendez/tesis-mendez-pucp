using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            // Evaluacion 360

            Opciones.Add(new SidebarOption("Evaluacion360", "Evaluación 360°", "icon-pencil", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Competencias", "Competencias", "Index", "icon-plus-sign"),
                new SidebarSuboption("Capacidades", "Capacidades", "Index", "icon-check"),
				//new SidebarSuboption("Evaluaciones", "Registrar Evaluaciones", "Index", "icon-check"),
                new SidebarSuboption("Evaluación de puestos de trabajo", "PuestosEvaluacion", "Index", "icon-ok-sign"),
                new SidebarSuboption("Procesos de evaluación", "ProcesoEvaluacion", "Index", "icon-road"),
                new SidebarSuboption("Mis evaluaciones", "ListarProcesosXEvaluado", "Index", "icon-ok-sign"),
            	new SidebarSuboption("Mis pendientes", "ListarProcesosXEvaluador", "Index", "icon-ok-sign"),
            	new SidebarSuboption("Mis colaboradores", "Subordinados", "Index", "icon-group"),

            })));


            // Objetivos
            Opciones.Add(new SidebarOption("Objetivos", "Objetivos", "icon-bookmark", new List<SidebarSuboption>(new SidebarSuboption[]{
               new SidebarSuboption("Objetivos de la empresa", "Objetivosempresa", "Index", "icon-building"),
               new SidebarSuboption("Monitoreo en mi equipo de trabajo", "Acordion", "Index", "icon-check"),
               new SidebarSuboption("Mis objetivos", "Misobjetivos", "Index", "icon-road"),
               new SidebarSuboption("Objetivos de subordinados", "Objetivossubordinados", "Index", "icon-group")
            })));

            // Configuracion
            Opciones.Add(new SidebarOption("Configuracion", "Configuración", "icon-wrench", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Períodos", "Periodos", "Index", "icon-time")
            })));

            // Segiuridad
            Opciones.Add(new SidebarOption("Seguridad", "Seguridad", "icon-lock", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Permisos a Web", "Usuarios", "Index", "icon-user-md"),
                new SidebarSuboption("Permisos a Móvil", "UsuariosMovil", "Index", "icon-user-md"),
                new SidebarSuboption("Nuevos Usuarios", "CrearUsuario", "Index", "icon-user"),
                new SidebarSuboption("Asignar Usuario", "AsignarCredenciales", "Index", "icon-user")
            })));

            // Organizacion
            Opciones.Add(new SidebarOption("Organizacion", "Organizacion", "icon-globe", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Organización","Organizaciones","Index","icon-cogs"),
                new SidebarSuboption("Organigrama", "Organigrama", "Index", "icon-sitemap"),
                new SidebarSuboption("Areas", "Areas", "Index", "icon-certificate"),
                new SidebarSuboption("Colaboradores", "Colaboradores", "Index", "icon-user"),
                new SidebarSuboption("Funciones", "Funciones", "Index", "icon-check"),
                new SidebarSuboption("Puestos", "Puestos", "Index", "icon-tag"),
                new SidebarSuboption("Linea de Carrera","Historial","Index","icon-signal"),
                new SidebarSuboption("Página Personal","Intranet","Index","icon-cloud")
            })));

            Opciones.Add(new SidebarOption("Reclutamiento", "Reclutamiento", "icon-group", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Ofertas Laborales","SolicitudOfertasLaborales","Index","icon-tag"),
                new SidebarSuboption("Solicitud de Promoción", "SolicitudPromociones","Index","icon-tag"),
                new SidebarSuboption("Administrar Ofertas Laborales Internas", "OfertasLaboralesInternas","Index","icon-book"),
                new SidebarSuboption("Administrar Ofertas Laborales Externas", "OfertasLaboralesExternas","Index","icon-book")
            })));

            Opciones.Add(new SidebarOption("BolsaTrabajo", "Bolsa de Trabajo", "icon-thumbs-up", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Convocatoria Interna","ConvocatoriasInternas","Index","icon-tag")
            })));

            Opciones.Add(new SidebarOption("Eventos", "Eventos", "icon-calendar", new List<SidebarSuboption>(new SidebarSuboption[]{
                new SidebarSuboption("Eventos","Eventos","Index","icon-calendar")
            })));
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
        public SidebarSuboption(string title, string controller, string method, string icon)
        {
            Title = title;
            Icon = icon;
            Controller = controller;
            Method = method;
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
                            }

                        }
                    }
                }
            }

            return salida;
        }
    }
}