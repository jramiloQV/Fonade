using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Datos;
using System.Data;
using System.Drawing;

namespace Fonade.PlanDeNegocioV2.Administracion.Tareas
{
    public partial class HistorialTareas : Negocio.Base_Page
    {
        public Boolean EmpresaOrProyecto
        {
            get
            {
                return !(usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor);
            }
            set
            {
            }
        }

        public string ProyectoTitle
        {
            get
            {
                if (EmpresaOrProyecto)
                    return "Plan de negocio";
                else
                    return "Empresa";
            }
            set
            {
            }
        }

        public int TamPaginacion
        {
            get
            {
                return 30;
            }
            set
            {
            }
        }

        public Boolean AllowUserProyecto
        {
            get
            {
                return !(usuario.CodGrupo == Constantes.CONST_GerenteAdministrador || usuario.CodGrupo == Constantes.CONST_AdministradorSistema || usuario.CodGrupo == Constantes.CONST_AdministradorSena);
            }
            set
            {
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarDDLProyecto(getTareas("", 0, 1000));
                //gvTareas.DataSource = getTareas("", 0, getTareasCount());
                BindData(0);
                                
                gvTareas.Columns[2].HeaderText = ProyectoTitle;
                gvTareas.Columns[2].Visible = AllowUserProyecto;
            }
        }

        public List<TareaUsuarioHistoria> getTareas(string orderBy, int startIndex, int maxRows)
        {

            if (startIndex < 0)
                startIndex = 0;

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tarea in db.TareaUsuarios
                                join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
                                join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
                                join contacto in db.Contacto on tarea.CodContactoAgendo equals contacto.Id_Contacto
                                join proyecto in db.Proyecto on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
                                join grupoContactoAgendo in db.GrupoContactos on contacto.Id_Contacto equals grupoContactoAgendo.CodContacto into GruposContacto
                                from proyectosTareas in proyectoTarea.DefaultIfEmpty()
                                from GruposContactoAgendo in GruposContacto.DefaultIfEmpty()
                                where (tarea.CodContacto.Equals(usuario.IdContacto) || tarea.CodContactoAgendo.Equals(usuario.IdContacto))
                                orderby repeticion.Fecha descending
                                select new TareaUsuarioHistoria
                                {
                                    Tipo = programa.NomTareaPrograma,
                                    Ejecutable = programa.Ejecutable,
                                    Icono = programa.Icono,
                                    Id = tarea.Id_TareaUsuario,
                                    Nombre = tarea.NomTareaUsuario,
                                    Descripcion = tarea.Descripcion,
                                    CodigoTarea = tarea.CodTareaPrograma,
                                    RecordatorioEmail = tarea.RecordatorioEmail,
                                    NivelUrgencia = tarea.NivelUrgencia,
                                    RecordatorioPantalla = tarea.RecordatorioPantalla,
                                    RequiereRespuesta = tarea.RequiereRespuesta,
                                    CodigoContactoAgendo = tarea.CodContactoAgendo,
                                    IdTareaUsuarioRepeticion = repeticion.Id_TareaUsuarioRepeticion,
                                    Parametros = repeticion.Parametros,
                                    Fecha = repeticion.Fecha,
                                    IdProyecto = proyectosTareas.Id_Proyecto,
                                    GrupoContactoAgendo = GruposContactoAgendo != null ? GruposContactoAgendo.CodGrupo : 0,
                                    NombreProyecto = proyectosTareas.NomProyecto,
                                    NombreContactoAgendo = contacto.Nombres,
                                    ApellidoContactoAgendo = contacto.Apellidos,
                                    Email = contacto.Email,
                                    GrupoContacto = usuario.CodGrupo
                                });


                entities = entities.Skip(startIndex).Take(maxRows);

                return entities.ToList();
            }
        }

        private void cargarDDLProyecto(List<TareaUsuarioHistoria> tareas)
        {
            List<ListItem> listItems = new List<ListItem>();

            List<ListItem> codProyectos = tareas.Where(x => x.IdProyecto != null)
                                                .Select(x => x.IdProyecto)
                                                .OrderByDescending(x => x.Value)
                                                .Distinct()
                                                .Select(p => new ListItem()
                                                {
                                                    Value = p.Value.ToString()
                                                                            ,
                                                    Text = p.Value.ToString()
                                                })
                                               .ToList();
            ListItem item = new ListItem
            {
                Value = "0",
                Text = "Seleccione..."
            };

            listItems.Add(item);
            listItems.AddRange(codProyectos);

            ddlProyecto.DataTextField = "Text";
            ddlProyecto.DataValueField = "Value";
            ddlProyecto.DataSource = listItems;
            ddlProyecto.DataBind();
        }

        public int getTareasCount()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tarea in db.TareaUsuarios
                                join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
                                join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
                                join contacto in db.Contacto on tarea.CodContactoAgendo equals contacto.Id_Contacto
                                join proyecto in db.Proyecto on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
                                from proyectosTareas in proyectoTarea.DefaultIfEmpty()
                                where (tarea.CodContacto.Equals(usuario.IdContacto) || tarea.CodContactoAgendo.Equals(usuario.IdContacto))
                                select tarea.CodContacto).Count();

                return entities;
            }
        }

        public int getTareasCountXProyecto(int _codProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tarea in db.TareaUsuarios
                                join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
                                join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
                                join contacto in db.Contacto on tarea.CodContactoAgendo equals contacto.Id_Contacto
                                join proyecto in db.Proyecto on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
                                from proyectosTareas in proyectoTarea.DefaultIfEmpty()
                                where (tarea.CodContacto.Equals(usuario.IdContacto)
                                        || tarea.CodContactoAgendo.Equals(usuario.IdContacto))
                                && tarea.CodProyecto == _codProyecto
                                select tarea.CodContacto).Count();

                return entities;
            }
        }

        protected void gwTareas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrarTarea")
            {
                HttpContext.Current.Session["Id_tareaRepeticion"] = e.CommandArgument.ToString();
                HttpContext.Current.Session["IdTarea"] = e.CommandArgument.ToString();

                Fonade.Proyecto.Proyectos.Redirect(Response, "~/Fonade/Tareas/Tarea.aspx", "_Blank", "width=1000,height=1000,top=0,left=0,scrollbars=yes,resizable=yes,toolbar=yes");
            }
            if (e.CommandName == "mostrarProyecto")
            {
                HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
                Response.Redirect("~/Fonade/Proyecto/ProyectoFrameSet.aspx", true);
            }
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(0);
        }

        protected void lbFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            BindData(CurrentPage);
        }

        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            BindData(CurrentPage);
        }



        private void BindData(int index)
        {
            if (ddlProyecto.SelectedValue.ToString()=="0")
            {
                ViewState["TotalPages"] = Math.Ceiling(Convert.ToDouble(getTareasCount()) 
                                                        / Convert.ToDouble(TamPaginacion));

                gvTareas.DataSource = getTareas("", index * TamPaginacion, TamPaginacion);
                gvTareas.DataBind();
            }
            else
            {
                ViewState["TotalPages"] = Math.Ceiling(Convert.ToDouble(getTareasCountXProyecto(Convert.ToInt32(ddlProyecto.SelectedValue))) 
                                                        / Convert.ToDouble(TamPaginacion));

                gvTareas.DataSource = getTareasXProyecto(Convert.ToInt32(ddlProyecto.SelectedValue)
                                                        , index * TamPaginacion, TamPaginacion);
                gvTareas.DataBind();
            }

            // Call the function to do paging
            HandlePaging();
        }


        int _firstIndex, _lastIndex;
        

        private void HandlePaging()
        {
            if (_codProyecto != Convert.ToInt32(ddlProyecto.SelectedValue))//primera vez que se ejecuta
            {
                _codProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                CurrentPage = 0;
            }                
           

            int maxIndices = 20;
            int midIndices = 10;

            var dt = new DataTable();
            dt.Columns.Add("PageIndex"); //Start from 0
            dt.Columns.Add("PageText"); //Start from 1

            _firstIndex = CurrentPage - midIndices;
            if (CurrentPage > midIndices)
                _lastIndex = CurrentPage + midIndices;
            else
                _lastIndex = maxIndices;

            //Check last page is greater than total page then reduced it to total no. of page is last index
            if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                _firstIndex = _lastIndex - maxIndices;
            }

            if (_firstIndex < 0)
                _firstIndex = 0;

            // Now creating page number based on above first and last page index
            for (var i = _firstIndex; i < _lastIndex; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaging.DataSource = dt;
            rptPaging.DataBind();
        }

        protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("newPage")) return;
            CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
            BindData(CurrentPage);
        }

        protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
            if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
            lnkPage.Enabled = false;
            lnkPage.BackColor = Color.FromName("#00468f");
            lnkPage.ForeColor = Color.FromName("white");
        }

        protected void lbNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            BindData(CurrentPage);
        }

        private int _codProyecto
        {
            get
            {
                if (ViewState["CodProyecto"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CodProyecto"]);
            }
            set
            {
                ViewState["CodProyecto"] = value;
            }
        }

        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPage"]);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        private List<TareaUsuarioHistoria> getTareasXProyecto(int codigoProyecto, int startIndex, int maxRows)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tarea in db.TareaUsuarios
                                join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
                                join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
                                join contacto in db.Contacto on tarea.CodContactoAgendo equals contacto.Id_Contacto
                                join proyecto in db.Proyecto on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
                                join grupoContactoAgendo in db.GrupoContactos on contacto.Id_Contacto equals grupoContactoAgendo.CodContacto into GruposContacto
                                from proyectosTareas in proyectoTarea.DefaultIfEmpty()
                                from GruposContactoAgendo in GruposContacto.DefaultIfEmpty()
                                where (tarea.CodContacto.Equals(usuario.IdContacto) || tarea.CodContactoAgendo.Equals(usuario.IdContacto))
                                && proyectosTareas.Id_Proyecto == codigoProyecto
                                orderby repeticion.Fecha descending
                                select new TareaUsuarioHistoria
                                {
                                    Tipo = programa.NomTareaPrograma,
                                    Ejecutable = programa.Ejecutable,
                                    Icono = programa.Icono,
                                    Id = tarea.Id_TareaUsuario,
                                    Nombre = tarea.NomTareaUsuario,
                                    Descripcion = tarea.Descripcion,
                                    CodigoTarea = tarea.CodTareaPrograma,
                                    RecordatorioEmail = tarea.RecordatorioEmail,
                                    NivelUrgencia = tarea.NivelUrgencia,
                                    RecordatorioPantalla = tarea.RecordatorioPantalla,
                                    RequiereRespuesta = tarea.RequiereRespuesta,
                                    CodigoContactoAgendo = tarea.CodContactoAgendo,
                                    IdTareaUsuarioRepeticion = repeticion.Id_TareaUsuarioRepeticion,
                                    Parametros = repeticion.Parametros,
                                    Fecha = repeticion.Fecha,
                                    IdProyecto = proyectosTareas.Id_Proyecto,
                                    GrupoContactoAgendo = GruposContactoAgendo != null ? GruposContactoAgendo.CodGrupo : 0,
                                    NombreProyecto = proyectosTareas.NomProyecto,
                                    NombreContactoAgendo = contacto.Nombres,
                                    ApellidoContactoAgendo = contacto.Apellidos,
                                    Email = contacto.Email,
                                    GrupoContacto = usuario.CodGrupo
                                });

                entities = entities.Skip(startIndex).Take(maxRows);


                return entities.ToList();
            }
        }
    }

    public class TareaUsuarioHistoria
    {
        public string Tipo { get; set; }
        public string Ejecutable { get; set; }
        public string Icono { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CodigoTarea { get; set; }
        public Boolean RecordatorioEmail { get; set; }
        public short NivelUrgencia { get; set; }
        public string Urgencia
        {
            get
            {
                if (NivelUrgencia.Equals(Constantes.CONST_PostIt))
                    return "/Images/IcoPost.gif";
                else
                    return "/Images/Tareas/Urgencia" + NivelUrgencia + ".gif";
            }
            set { }
        }
        public Boolean RequiereRespuesta { get; set; }
        public Boolean RecordatorioPantalla { get; set; }
        public int CodigoContactoAgendo { get; set; }
        public int IdTareaUsuarioRepeticion { get; set; }
        public string Parametros { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaFormated
        {
            get
            {
                return Fecha.getFechaConFormato(true);
            }
            set
            {
            }
        }
        public int? IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreContactoAgendo { get; set; }
        public string ApellidoContactoAgendo { get; set; }
        public string NombreCompletoContactoAgendo
        {
            get
            {
                return hideEvaluadorNameFromEmprendedor ? "Evaluador" : NombreContactoAgendo + ' ' + ApellidoContactoAgendo;
            }
            set { }
        }
        public string Email { get; set; }
        /// <summary>
        /// Verifica si es permitido ver tareas.
        /// </summary>
        public Boolean AllowTarea
        {
            get
            {
                return !GrupoContacto.Equals(Constantes.CONST_Perfil_Fiduciario);
            }
            set { }
        }
        /// <summary>
        /// Verifica si es permitido ver proyecto
        /// </summary>
        public Boolean AllowProyecto
        {
            get
            {
                return !GrupoContacto.Equals(Constantes.CONST_Perfil_Fiduciario);
            }
            set { }
        }
        public int GrupoContacto { get; set; }
        public int GrupoContactoAgendo { get; set; }

        public string UrlRouteTarea
        {
            get
            {
                if (AllowTarea)
                    return "~/Fonade/Tareas/TareasAgendar.aspx";
                else
                    return "~/Fonade/evaluacion/AccesoDenegado.aspx";
            }
            set { }
        }

        public string UrlRouteProyecto
        {
            get
            {
                if (AllowProyecto)
                    return "~/Fonade/Proyecto/ProyectoFrameSet.aspx";
                else
                    return "~/Fonade/evaluacion/AccesoDenegado.aspx";
            }
            set { }
        }

        public bool hideEvaluadorNameFromEmprendedor
        {
            get
            {
                return (GrupoContacto.Equals(Constantes.CONST_Emprendedor)
                   || GrupoContacto.Equals(Constantes.CONST_Asesor)
                   || GrupoContacto.Equals(Constantes.CONST_JefeUnidad)) && GrupoContactoAgendo.Equals(Constantes.CONST_Evaluador);
            }
            set
            {
            }
        }
    }

}