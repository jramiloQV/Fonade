using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Datos;
using System.Activities.Statements;
using System.Data;
using System.EnterpriseServices;
using System.Drawing;
using System.Configuration;
using System.Web.Configuration;
using Fonade.Account;
using Fonade.Controles;

namespace Fonade.FONADE.MiPerfil
{
    public partial class Home : Negocio.Base_Page
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

        [Obsolete]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string expirePass = WebConfigurationManager.AppSettings["VigenciaInfo"];
                string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
                Clave clave = Negocio.PlanDeNegocioV2.Utilidad.User.getClave(usuario.IdContacto);
                if (clave.DebeCambiar == 1 && (DateTime.Now - usuario.LastPasswordChangedDate).TotalMinutes <= Double.Parse(tempPass))
                {
                    Response.Redirect("~/FONADE/MiPerfil/CambiarClave.aspx", false);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave temporal expiro, reintente en la opcion olvido su clave, o comuniquese con el administrador!')", true);
                }
                else
                if ((DateTime.Now - usuario.LastPasswordChangedDate).TotalDays > (Convert.ToInt32(expirePass) * 30))
                {
                    Response.Redirect("~/FONADE/MiPerfil/CambiarClave.aspx", false);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
                }
                else
                {
                    cargarDDLProyecto(getTareas("", 0, 1000));
                    //gvTareas.DataBind();

                    BindData(0);

                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_Interventor)
                        VerificarTareasEspecialesInterventoria();

                    gvTareas.Columns[2].HeaderText = ProyectoTitle;
                    gvTareas.Columns[2].Visible = AllowUserProyecto;
                }

            }
        }


        private void verLogin()
        {

            Response.Redirect("~/Account/Login.aspx");
            System.Runtime.Caching.MemoryCache.Default.Dispose();
            Session.Abandon();
        }

        private void cargarDDLProyecto(List<TareaUsuario> tareas)
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

        private void BindData(int index)
        {
            if (ddlProyecto.SelectedValue.ToString() == "0")
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

        private List<TareaUsuario> getTareasXProyecto(int codigoProyecto, int startIndex, int maxRows)
        {
            using (Datos.FonadeDBLightDataContext db = new FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var tareaUsers = (db.MD_ConsultarTareasXContactoProyecto(usuario.IdContacto, startIndex, maxRows, codigoProyecto)
                                 .Select(x => new TareaUsuario
                                 {
                                     Tipo = x.Tipo,
                                     Ejecutable = x.Ejecutable,
                                     Icono = x.Icono,
                                     Id = x.Id,
                                     Nombre = x.Nombre,
                                     Descripcion = x.Descripcion,
                                     CodigoTarea = x.CodigoTarea,
                                     RecordatorioEmail = x.RecordatorioEmail,
                                     NivelUrgencia = x.NivelUrgencia,
                                     RecordatorioPantalla = x.RecordatorioPantalla,
                                     RequiereRespuesta = x.RequiereRespuesta,
                                     CodigoContactoAgendo = x.CodigoContactoAgendo,
                                     IdTareaUsuarioRepeticion = x.IdTareaUsuarioRepeticion,
                                     Parametros = x.Parametros,
                                     Fecha = x.Fecha,
                                     IdProyecto = x.IdProyecto,
                                     GrupoContactoAgendo = x.GrupoContactoAgendo,
                                     NombreProyecto = x.NombreProyecto,
                                     NombreContactoAgendo = x.NombreContactoAgendo,
                                     ApellidoContactoAgendo = x.ApellidoContactoAgendo,
                                     Email = x.Email,
                                     GrupoContacto = x.GrupoContactoAgendo
                                 }).ToList());
                return tareaUsers;
            }
        }

        public int getTareasCountXProyecto(int _codProyecto)
        {
            using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int entities = db.MD_ConsultarCantTareasXContactoProyecto(usuario.IdContacto, _codProyecto)
                                        .FirstOrDefault().CantRegistros ?? 0;

                return entities;
            }

        }

        public void VerificarTareasEspecialesInterventoria()
        {
            if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.TieneTareasPendientes(usuario.IdContacto))
                divTareasEspeciales.Visible = true;
        }

        public List<TareaUsuario> getTareas(string orderBy, int startIndex, int maxRows)
        {
            //Se modifica consulta por procedimiento almacenado para mejorar desempeño.
            //09/Julio/2018

            if (startIndex < 0)
                startIndex = 0;

            using (Datos.FonadeDatosDataContext db = new FonadeDatosDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var tareaUsers = (db.MD_ConsultarTareasPendientesXContacto(usuario.IdContacto, startIndex, maxRows)
                                 .Select(x => new TareaUsuario
                                 {
                                     Tipo = x.Tipo,
                                     Ejecutable = x.Ejecutable,
                                     Icono = x.Icono,
                                     Id = x.Id,
                                     Nombre = x.Nombre,
                                     Descripcion = x.Descripcion,
                                     CodigoTarea = x.CodigoTarea,
                                     RecordatorioEmail = x.RecordatorioEmail,
                                     NivelUrgencia = x.NivelUrgencia,
                                     RecordatorioPantalla = x.RecordatorioPantalla,
                                     RequiereRespuesta = x.RequiereRespuesta,
                                     CodigoContactoAgendo = x.CodigoContactoAgendo,
                                     IdTareaUsuarioRepeticion = x.IdTareaUsuarioRepeticion,
                                     Parametros = x.Parametros,
                                     Fecha = x.Fecha,
                                     IdProyecto = x.IdProyecto,
                                     GrupoContactoAgendo = x.GrupoContactoAgendo,
                                     NombreProyecto = x.NombreProyecto,
                                     NombreContactoAgendo = x.NombreContactoAgendo,
                                     ApellidoContactoAgendo = x.ApellidoContactoAgendo,
                                     Email = x.Email,
                                     GrupoContacto = x.GrupoContactoAgendo
                                 }).ToList());
                return tareaUsers;
            }



            //using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            //{
            //    var entities = (from tarea in db.TareaUsuarios
            //                    join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
            //                    join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
            //                    join contacto in db.Contactos on tarea.CodContactoAgendo equals contacto.Id_Contacto
            //                    join proyecto in db.Proyectos on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
            //                    join grupoContactoAgendo in db.GrupoContactos on contacto.Id_Contacto equals grupoContactoAgendo.CodContacto into GruposContacto
            //                    from proyectosTareas in proyectoTarea.DefaultIfEmpty()
            //                    from GruposContactoAgendo in GruposContacto.DefaultIfEmpty()
            //                    where tarea.CodContacto.Equals(usuario.IdContacto)
            //                          && repeticion.FechaCierre == null
            //                    orderby repeticion.Fecha descending
            //                    select new TareaUsuario
            //                    {
            //                        Tipo = programa.NomTareaPrograma,
            //                        Ejecutable = programa.Ejecutable,
            //                        Icono = programa.Icono,
            //                        Id = tarea.Id_TareaUsuario,
            //                        Nombre = tarea.NomTareaUsuario,
            //                        Descripcion = tarea.Descripcion,
            //                        CodigoTarea = tarea.CodTareaPrograma,
            //                        RecordatorioEmail = tarea.RecordatorioEmail,
            //                        NivelUrgencia = tarea.NivelUrgencia,
            //                        RecordatorioPantalla = tarea.RecordatorioPantalla,
            //                        RequiereRespuesta = tarea.RequiereRespuesta,
            //                        CodigoContactoAgendo = tarea.CodContactoAgendo,
            //                        IdTareaUsuarioRepeticion = repeticion.Id_TareaUsuarioRepeticion,
            //                        Parametros = repeticion.Parametros,
            //                        Fecha = repeticion.Fecha,
            //                        IdProyecto = proyectosTareas.Id_Proyecto,
            //                        GrupoContactoAgendo = GruposContactoAgendo != null ? GruposContactoAgendo.CodGrupo : 0,
            //                        NombreProyecto = proyectosTareas.NomProyecto,
            //                        NombreContactoAgendo = contacto.Nombres,
            //                        ApellidoContactoAgendo = contacto.Apellidos,
            //                        Email = contacto.Email,
            //                        GrupoContacto = usuario.CodGrupo                                    
            //                    });


            //    entities = entities.Skip(startIndex).Take(maxRows);

            //    return entities.ToList();
            //}
        }

        public int getTareasCount()
        {
            using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int entities = db.MD_ConsultarCantTareasPendientesXContacto(usuario.IdContacto)
                                        .FirstOrDefault().CantRegistros ?? 0;

                return entities;
            }

            //using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            //{

            //    var entities = (from tarea in db.TareaUsuarios
            //                    join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
            //                    join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
            //                    join contacto in db.Contactos on tarea.CodContactoAgendo equals contacto.Id_Contacto
            //                    join proyecto in db.Proyectos on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
            //                    from proyectosTareas in proyectoTarea.DefaultIfEmpty()
            //                    where tarea.CodContacto.Equals(usuario.IdContacto)
            //                          && repeticion.FechaCierre == null
            //                    select tarea.CodContacto).Count();

            //    return entities;
            //}
        }

        protected void gwTareas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrarTarea")
            {
                HttpContext.Current.Session["Id_tareaRepeticion"] = e.CommandArgument.ToString();
                HttpContext.Current.Session["IdTarea"] = e.CommandArgument.ToString();
                Response.Redirect("~/Fonade/Tareas/Tarea.aspx", true);
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
    }

    public class TareaUsuario
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

                return (GrupoContacto.Equals(Constantes.CONST_Emprendedor) || GrupoContacto.Equals(Constantes.CONST_Asesor)) && GrupoContactoAgendo.Equals(Constantes.CONST_Evaluador);
            }
            set
            {
            }
        }
    }

    #region AntiguoCode
    //public partial class Home : Negocio.Base_Page
    //{

    //    public Boolean EmpresaOrProyecto
    //    {
    //        get
    //        {
    //            return !(usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor);
    //        }
    //        set
    //        {
    //        }
    //    }

    //    public int TamPaginacion
    //    {
    //        get
    //        {
    //            return 30;
    //        }
    //        set
    //        {
    //        }
    //    }

    //    private int _codProyecto
    //    {
    //        get
    //        {
    //            if (ViewState["CodProyecto"] == null)
    //            {
    //                return 0;
    //            }
    //            return ((int)ViewState["CodProyecto"]);
    //        }
    //        set
    //        {
    //            ViewState["CodProyecto"] = value;
    //        }
    //    }

    //    private int CurrentPage
    //    {
    //        get
    //        {
    //            if (ViewState["CurrentPage"] == null)
    //            {
    //                return 0;
    //            }
    //            return ((int)ViewState["CurrentPage"]);
    //        }
    //        set
    //        {
    //            ViewState["CurrentPage"] = value;
    //        }
    //    }

    //    public string ProyectoTitle
    //    {
    //        get
    //        {
    //            if (EmpresaOrProyecto)
    //                return "Plan de negocio";
    //            else
    //                return "Empresa";
    //        }
    //        set
    //        {
    //        }
    //    }

    //    public Boolean AllowUserProyecto
    //    {
    //        get
    //        {
    //            return !(usuario.CodGrupo == Constantes.CONST_GerenteAdministrador || usuario.CodGrupo == Constantes.CONST_AdministradorSistema || usuario.CodGrupo == Constantes.CONST_AdministradorSena);
    //        }
    //        set
    //        {
    //        }
    //    }

    //    [Obsolete]
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        if (!Page.IsPostBack)
    //        {
    //            string expirePass = WebConfigurationManager.AppSettings["VigenciaInfo"];
    //            string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
    //            Clave clave = Negocio.PlanDeNegocioV2.Utilidad.User.getClave(usuario.IdContacto);
    //            if (clave.DebeCambiar == 1 && (DateTime.Now - usuario.LastPasswordChangedDate).TotalMinutes <= Double.Parse(tempPass))
    //            {
    //                Response.Redirect("~/FONADE/MiPerfil/CambiarClave.aspx", false);
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave temporal expiro, reintente en la opcion olvido su clave, o comuniquese con el administrador!')", true);
    //            }
    //            else
    //            if ((DateTime.Now - usuario.LastPasswordChangedDate).TotalDays > (Convert.ToInt32(expirePass) * 30))
    //            {
    //                Response.Redirect("~/FONADE/MiPerfil/CambiarClave.aspx", false);
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
    //            }
    //            else
    //            {
    //                cargarDDLProyecto(getTareas("", 0, 1000));
    //                //gvTareas.DataBind();

    //                BindData(0);

    //                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_Interventor)
    //                    VerificarTareasEspecialesInterventoria();

    //                gvTareas.Columns[2].HeaderText = ProyectoTitle;
    //                gvTareas.Columns[2].Visible = AllowUserProyecto;
    //            }

    //        }
    //    }


    //    private void verLogin()
    //    {

    //        Response.Redirect("~/Account/Login.aspx");
    //        System.Runtime.Caching.MemoryCache.Default.Dispose();
    //        Session.Abandon();
    //    }

    //    private void cargarDDLProyecto(List<TareaUsuario> tareas)
    //    {
    //        List<ListItem> listItems = new List<ListItem>();

    //        List<ListItem> codProyectos = tareas.Where(x => x.IdProyecto != null)
    //                                            .Select(x => x.IdProyecto)
    //                                            .OrderByDescending(x => x.Value)
    //                                            .Distinct()
    //                                            .Select(p => new ListItem()
    //                                            {
    //                                                Value = p.Value.ToString()
    //                                                                        ,
    //                                                Text = p.Value.ToString()
    //                                            })
    //                                           .ToList();
    //        ListItem item = new ListItem
    //        {
    //            Value = "0",
    //            Text = "Seleccione..."
    //        };

    //        listItems.Add(item);
    //        listItems.AddRange(codProyectos);

    //        ddlProyecto.DataTextField = "Text";
    //        ddlProyecto.DataValueField = "Value";
    //        ddlProyecto.DataSource = listItems;
    //        ddlProyecto.DataBind();
    //    }

    //    private void BindData(int index)
    //    {
    //        if (ddlProyecto.SelectedValue.ToString() == "0")
    //        {
    //            ViewState["TotalPages"] = Math.Ceiling(Convert.ToDouble(getTareasCount())
    //                                                    / Convert.ToDouble(TamPaginacion));

    //            gvTareas.DataSource = getTareas("", index * TamPaginacion, TamPaginacion);
    //            gvTareas.DataBind();
    //        }
    //        else
    //        {
    //            ViewState["TotalPages"] = Math.Ceiling(Convert.ToDouble(getTareasCountXProyecto(Convert.ToInt32(ddlProyecto.SelectedValue)))
    //                                                    / Convert.ToDouble(TamPaginacion));

    //            gvTareas.DataSource = getTareasXProyecto(Convert.ToInt32(ddlProyecto.SelectedValue)
    //                                                    , index * TamPaginacion, TamPaginacion);
    //            gvTareas.DataBind();
    //        }

    //        // Call the function to do paging
    //        HandlePaging();
    //    }

    //    int _firstIndex, _lastIndex;

    //    private void HandlePaging()
    //    {
    //        if (_codProyecto != Convert.ToInt32(ddlProyecto.SelectedValue))//primera vez que se ejecuta
    //        {
    //            _codProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
    //            CurrentPage = 0;
    //        }


    //        int maxIndices = 20;
    //        int midIndices = 10;

    //        var dt = new DataTable();
    //        dt.Columns.Add("PageIndex"); //Start from 0
    //        dt.Columns.Add("PageText"); //Start from 1

    //        _firstIndex = CurrentPage - midIndices;
    //        if (CurrentPage > midIndices)
    //            _lastIndex = CurrentPage + midIndices;
    //        else
    //            _lastIndex = maxIndices;

    //        //Check last page is greater than total page then reduced it to total no. of page is last index
    //        if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
    //        {
    //            _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
    //            _firstIndex = _lastIndex - maxIndices;
    //        }

    //        if (_firstIndex < 0)
    //            _firstIndex = 0;

    //        // Now creating page number based on above first and last page index
    //        for (var i = _firstIndex; i < _lastIndex; i++)
    //        {
    //            var dr = dt.NewRow();
    //            dr[0] = i;
    //            dr[1] = i + 1;
    //            dt.Rows.Add(dr);
    //        }

    //        rptPaging.DataSource = dt;
    //        rptPaging.DataBind();
    //    }

    //    private List<TareaUsuario> getTareasXProyecto(int codigoProyecto, int startIndex, int maxRows)
    //    {
    //        using (Datos.FonadeDBLightDataContext db = new FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var tareaUsers = (db.MD_ConsultarTareasXContactoProyecto(usuario.IdContacto, startIndex, maxRows, codigoProyecto)
    //                             .Select(x => new TareaUsuario
    //                             {
    //                                 Tipo = x.Tipo,
    //                                 Ejecutable = x.Ejecutable,
    //                                 Icono = x.Icono,
    //                                 Id = x.Id,
    //                                 Nombre = x.Nombre,
    //                                 Descripcion = x.Descripcion,
    //                                 CodigoTarea = x.CodigoTarea,
    //                                 RecordatorioEmail = x.RecordatorioEmail,
    //                                 NivelUrgencia = x.NivelUrgencia,
    //                                 RecordatorioPantalla = x.RecordatorioPantalla,
    //                                 RequiereRespuesta = x.RequiereRespuesta,
    //                                 CodigoContactoAgendo = x.CodigoContactoAgendo,
    //                                 IdTareaUsuarioRepeticion = x.IdTareaUsuarioRepeticion,
    //                                 Parametros = x.Parametros,
    //                                 Fecha = x.Fecha,
    //                                 IdProyecto = x.IdProyecto,
    //                                 GrupoContactoAgendo = x.GrupoContactoAgendo,
    //                                 NombreProyecto = x.NombreProyecto,
    //                                 NombreContactoAgendo = x.NombreContactoAgendo,
    //                                 ApellidoContactoAgendo = x.ApellidoContactoAgendo,
    //                                 Email = x.Email,
    //                                 GrupoContacto = x.GrupoContactoAgendo
    //                             }).ToList());
    //            return tareaUsers;
    //        }
    //    }

    //    public int getTareasCountXProyecto(int _codProyecto)
    //    {
    //        using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            int entities = db.MD_ConsultarCantTareasXContactoProyecto(usuario.IdContacto, _codProyecto)
    //                                    .FirstOrDefault().CantRegistros ?? 0;

    //            return entities;
    //        }

    //    }

    //    public void VerificarTareasEspecialesInterventoria()
    //    {
    //        if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.TieneTareasPendientes(usuario.IdContacto))
    //            divTareasEspeciales.Visible = true;
    //    }

    //    public List<TareaUsuario> getTareas(string orderBy, int startIndex, int maxRows)
    //    {
    //        //Se modifica consulta por procedimiento almacenado para mejorar desempeño.
    //        //09/Julio/2018

    //        if (startIndex < 0)
    //            startIndex = 0;

    //        using (Datos.FonadeDatosDataContext db = new FonadeDatosDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var tareaUsers = (db.MD_ConsultarTareasPendientesXContacto(usuario.IdContacto, startIndex, maxRows)
    //                             .Select(x => new TareaUsuario
    //                             {
    //                                 Tipo = x.Tipo,
    //                                 Ejecutable = x.Ejecutable,
    //                                 Icono = x.Icono,
    //                                 Id = x.Id,
    //                                 Nombre = x.Nombre,
    //                                 Descripcion = x.Descripcion,
    //                                 CodigoTarea = x.CodigoTarea,
    //                                 RecordatorioEmail = x.RecordatorioEmail,
    //                                 NivelUrgencia = x.NivelUrgencia,
    //                                 RecordatorioPantalla = x.RecordatorioPantalla,
    //                                 RequiereRespuesta = x.RequiereRespuesta,
    //                                 CodigoContactoAgendo = x.CodigoContactoAgendo,
    //                                 IdTareaUsuarioRepeticion = x.IdTareaUsuarioRepeticion,
    //                                 Parametros = x.Parametros,
    //                                 Fecha = x.Fecha,
    //                                 IdProyecto = x.IdProyecto,
    //                                 GrupoContactoAgendo = x.GrupoContactoAgendo,
    //                                 NombreProyecto = x.NombreProyecto,
    //                                 NombreContactoAgendo = x.NombreContactoAgendo,
    //                                 ApellidoContactoAgendo = x.ApellidoContactoAgendo,
    //                                 Email = x.Email,
    //                                 GrupoContacto = x.GrupoContactoAgendo
    //                             }).ToList());
    //            return tareaUsers;
    //        }



    //        //using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        //{
    //        //    var entities = (from tarea in db.TareaUsuarios
    //        //                    join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
    //        //                    join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
    //        //                    join contacto in db.Contactos on tarea.CodContactoAgendo equals contacto.Id_Contacto
    //        //                    join proyecto in db.Proyectos on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
    //        //                    join grupoContactoAgendo in db.GrupoContactos on contacto.Id_Contacto equals grupoContactoAgendo.CodContacto into GruposContacto
    //        //                    from proyectosTareas in proyectoTarea.DefaultIfEmpty()
    //        //                    from GruposContactoAgendo in GruposContacto.DefaultIfEmpty()
    //        //                    where tarea.CodContacto.Equals(usuario.IdContacto)
    //        //                          && repeticion.FechaCierre == null
    //        //                    orderby repeticion.Fecha descending
    //        //                    select new TareaUsuario
    //        //                    {
    //        //                        Tipo = programa.NomTareaPrograma,
    //        //                        Ejecutable = programa.Ejecutable,
    //        //                        Icono = programa.Icono,
    //        //                        Id = tarea.Id_TareaUsuario,
    //        //                        Nombre = tarea.NomTareaUsuario,
    //        //                        Descripcion = tarea.Descripcion,
    //        //                        CodigoTarea = tarea.CodTareaPrograma,
    //        //                        RecordatorioEmail = tarea.RecordatorioEmail,
    //        //                        NivelUrgencia = tarea.NivelUrgencia,
    //        //                        RecordatorioPantalla = tarea.RecordatorioPantalla,
    //        //                        RequiereRespuesta = tarea.RequiereRespuesta,
    //        //                        CodigoContactoAgendo = tarea.CodContactoAgendo,
    //        //                        IdTareaUsuarioRepeticion = repeticion.Id_TareaUsuarioRepeticion,
    //        //                        Parametros = repeticion.Parametros,
    //        //                        Fecha = repeticion.Fecha,
    //        //                        IdProyecto = proyectosTareas.Id_Proyecto,
    //        //                        GrupoContactoAgendo = GruposContactoAgendo != null ? GruposContactoAgendo.CodGrupo : 0,
    //        //                        NombreProyecto = proyectosTareas.NomProyecto,
    //        //                        NombreContactoAgendo = contacto.Nombres,
    //        //                        ApellidoContactoAgendo = contacto.Apellidos,
    //        //                        Email = contacto.Email,
    //        //                        GrupoContacto = usuario.CodGrupo                                    
    //        //                    });


    //        //    entities = entities.Skip(startIndex).Take(maxRows);

    //        //    return entities.ToList();
    //        //}
    //    }

    //    public int getTareasCount()
    //    {
    //        using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            int entities = db.MD_ConsultarCantTareasPendientesXContacto(usuario.IdContacto)
    //                                    .FirstOrDefault().CantRegistros ?? 0;

    //            return entities;
    //        }

    //        //using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        //{

    //        //    var entities = (from tarea in db.TareaUsuarios
    //        //                    join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
    //        //                    join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
    //        //                    join contacto in db.Contactos on tarea.CodContactoAgendo equals contacto.Id_Contacto
    //        //                    join proyecto in db.Proyectos on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
    //        //                    from proyectosTareas in proyectoTarea.DefaultIfEmpty()
    //        //                    where tarea.CodContacto.Equals(usuario.IdContacto)
    //        //                          && repeticion.FechaCierre == null
    //        //                    select tarea.CodContacto).Count();

    //        //    return entities;
    //        //}
    //    }

    //    protected void gwTareas_RowCommand(object sender, GridViewCommandEventArgs e)
    //    {
    //        if (e.CommandName == "mostrarTarea")
    //        {
    //            HttpContext.Current.Session["Id_tareaRepeticion"] = e.CommandArgument.ToString();
    //            HttpContext.Current.Session["IdTarea"] = e.CommandArgument.ToString();
    //            Response.Redirect("~/Fonade/Tareas/Tarea.aspx", true);
    //        }
    //        if (e.CommandName == "mostrarProyecto")
    //        {
    //            HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
    //            Response.Redirect("~/Fonade/Proyecto/ProyectoFrameSet.aspx", true);
    //        }

    //    }

    //    protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        BindData(0);
    //    }

    //    protected void lbFirst_Click(object sender, EventArgs e)
    //    {
    //        CurrentPage = 0;
    //        BindData(CurrentPage);
    //    }

    //    protected void lbPrevious_Click(object sender, EventArgs e)
    //    {
    //        CurrentPage -= 1;
    //        BindData(CurrentPage);
    //    }

    //    protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
    //    {
    //        if (!e.CommandName.Equals("newPage")) return;
    //        CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
    //        BindData(CurrentPage);
    //    }

    //    protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    //    {
    //        var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
    //        if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
    //        lnkPage.Enabled = false;
    //        lnkPage.BackColor = Color.FromName("#00468f");
    //        lnkPage.ForeColor = Color.FromName("white");
    //    }
    //    protected void lbNext_Click(object sender, EventArgs e)
    //    {
    //        CurrentPage += 1;
    //        BindData(CurrentPage);
    //    }
    //}

    //public class TareaUsuario
    //{
    //    public string Tipo { get; set; }
    //    public string Ejecutable { get; set; }
    //    public string Icono { get; set; }
    //    public int Id { get; set; }
    //    public string Nombre { get; set; }
    //    public string Descripcion { get; set; }
    //    public int CodigoTarea { get; set; }
    //    public Boolean RecordatorioEmail { get; set; }
    //    public short NivelUrgencia { get; set; }
    //    public string Urgencia
    //    {
    //        get
    //        {
    //            if (NivelUrgencia.Equals(Constantes.CONST_PostIt))
    //                return "/Images/IcoPost.gif";
    //            else
    //                return "/Images/Tareas/Urgencia" + NivelUrgencia + ".gif";
    //        }
    //        set { }
    //    }
    //    public Boolean RequiereRespuesta { get; set; }
    //    public Boolean RecordatorioPantalla { get; set; }
    //    public int CodigoContactoAgendo { get; set; }
    //    public int IdTareaUsuarioRepeticion { get; set; }
    //    public string Parametros { get; set; }
    //    public DateTime Fecha { get; set; }
    //    public string FechaFormated
    //    {
    //        get
    //        {
    //            return Fecha.getFechaConFormato(true);
    //        }
    //        set
    //        {
    //        }
    //    }
    //    public int? IdProyecto { get; set; }
    //    public string NombreProyecto { get; set; }
    //    public string NombreContactoAgendo { get; set; }
    //    public string ApellidoContactoAgendo { get; set; }
    //    public string NombreCompletoContactoAgendo
    //    {
    //        get
    //        {
    //            return hideEvaluadorNameFromEmprendedor ? "Evaluador" : NombreContactoAgendo + ' ' + ApellidoContactoAgendo;
    //        }
    //        set { }
    //    }
    //    public string Email { get; set; }
    //    /// <summary>
    //    /// Verifica si es permitido ver tareas.
    //    /// </summary>
    //    public Boolean AllowTarea
    //    {
    //        get
    //        {
    //            return !GrupoContacto.Equals(Constantes.CONST_Perfil_Fiduciario);
    //        }
    //        set { }
    //    }
    //    /// <summary>
    //    /// Verifica si es permitido ver proyecto
    //    /// </summary>
    //    public Boolean AllowProyecto
    //    {
    //        get
    //        {
    //            return !GrupoContacto.Equals(Constantes.CONST_Perfil_Fiduciario);
    //        }
    //        set { }
    //    }
    //    public int GrupoContacto { get; set; }
    //    public int GrupoContactoAgendo { get; set; }

    //    public string UrlRouteTarea
    //    {
    //        get
    //        {
    //            if (AllowTarea)
    //                return "~/Fonade/Tareas/TareasAgendar.aspx";
    //            else
    //                return "~/Fonade/evaluacion/AccesoDenegado.aspx";
    //        }
    //        set { }
    //    }

    //    public string UrlRouteProyecto
    //    {
    //        get
    //        {
    //            if (AllowProyecto)
    //                return "~/Fonade/Proyecto/ProyectoFrameSet.aspx";
    //            else
    //                return "~/Fonade/evaluacion/AccesoDenegado.aspx";
    //        }
    //        set { }
    //    }

    //    public bool hideEvaluadorNameFromEmprendedor
    //    {
    //        get
    //        {

    //            return (GrupoContacto.Equals(Constantes.CONST_Emprendedor) || GrupoContacto.Equals(Constantes.CONST_Asesor)) && GrupoContactoAgendo.Equals(Constantes.CONST_Evaluador);
    //        }
    //        set
    //        {
    //        }
    //    }


    //}
    #endregion
}
