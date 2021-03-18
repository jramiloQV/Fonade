using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using Fonade.Clases;

namespace Fonade.PlanDeNegocioV2.Formulacion.PlanOperativo
{
    public partial class PlanOperativo : Negocio.Base_Page
    {
        public int txtTab = Constantes.CONST_PlanOperativoV2Hijo;
        public int CantidadMeses = 12;
        public string txtNomProyecto = "";
        public bool bNuevo = true;

        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }

        /// <summary>
        /// Diego Quiñonez - 29 de Diciembre de 2014
        /// </summary>
        private string codConvocatoria
        {
            get { return HttpContext.Current.Session["codConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codConvocatoria"].ToString()) ? HttpContext.Current.Session["codConvocatoria"].ToString() : ""; }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 29 de Diciembre de 2014
        /// </summary>
        public Boolean esMiembro
        {
            get { return fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString()); }
        }

        /// <summary>
        /// Diego Quiñonez - 29 de Diciembre de 2014
        /// </summary>
        public Boolean bRealizado
        {
            get { return esRealizado(txtTab.ToString(), CodigoProyecto.ToString(), codConvocatoria); }
        }

        /// <summary>
        /// Muestra u oculta el postit
        /// </summary>
        public Boolean PostitVisible
        {
            get
            {
                return esMiembro && !bRealizado;
            }
            set { }
        }

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"]); } } }

        public bool ejecucion
        {
            get
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                return
                    new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(CodigoProyecto).Count > 0
                    && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Encabezado.CodigoProyecto = CodigoProyecto;
            Encabezado.CodigoTab = txtTab;

            SetPostIt();

            inicioEncabezado(CodigoProyecto.ToString(), codConvocatoria, txtTab);
            
            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_AdministradorFonade)
            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)//!chk_realizado.Checked)
            { this.div_Post_It_1.Visible = true; }
            else
            {
                div_Post_It_1.Visible = false;
            }

            if ((miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false) || bNuevo == false)
                pnlAdicionarActividadPlanOperativo.Visible = true;

            #region Comentarios.
            /*
            if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && codEstado == Constantes.CONST_Inscripcion)
                pnlAdicionarAnexos.Visible = true;

            if (codEstado == Constantes.CONST_Evaluacion)
            {
                pnlDocumentosDeEvaluacion.Visible = true;
                if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    pnlAdicionarDocumentoEvaluacion.Visible = true;
                }
            }
            */

            #endregion

            CargarProrroga();
            CargarbNuevo();
            CargarNombreProyecto();

            if (!IsPostBack)
            {
                CargarGridActividades();
                ObtenerDatosUltimaActualizacion();
            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_PlanOperativoV2;
        }

        protected void Recargar()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", "window.parent.reload()", true);
        }

        protected void CargarProrroga()
        {
            try
            {
                var query = (from p in consultas.Db.ProyectoProrrogas
                             where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             select new { p.Prorroga }).FirstOrDefault();
                CantidadMeses += query.Prorroga;
            }
            catch
            {
                CantidadMeses = 12;
            }
        }

        protected void CargarbNuevo()
        {
            if (codEstado >= Constantes.CONST_Evaluacion && codConvocatoria == "1" && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                bNuevo = false;
            }
        }

        protected void CargarNombreProyecto()
        {
            try
            {
                var query = (from p in consultas.Db.Proyecto
                             where p.Id_Proyecto == Convert.ToInt32(CodigoProyecto)
                             select new { p.NomProyecto }).FirstOrDefault();
                txtNomProyecto = query.NomProyecto;
            }
            catch
            {
                txtNomProyecto = "";
            }
        }

        protected void CargarGridActividades()
        {

            var query = (from p in consultas.Db.ProyectoActividadPOs
                         where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                         orderby p.Item ascending
                         select new { p.Id_Actividad, p.Item, p.NomActividad });

            string consultaDetalle = "select id_actividad as CodActividad, mes,codtipofinanciacion,valor ";
            consultaDetalle += "from proyectoactividadpomes LEFT OUTER JOIN proyectoactividadPO ";
            consultaDetalle += "on id_actividad=codactividad where codproyecto={0}";
            consultaDetalle += " order by item, codactividad,mes,codtipofinanciacion";
            IEnumerable<ProyectoActividadPOMe> respuestaDetalle = consultas.Db.ExecuteQuery<ProyectoActividadPOMe>(consultaDetalle, Convert.ToInt32(CodigoProyecto));


            DataTable datos = new DataTable();
            DataTable detalle = new DataTable();
            datos.Columns.Add("CodProyecto");
            datos.Columns.Add("Id_Actividad");
            datos.Columns.Add("Item");
            datos.Columns.Add("Actividad");
            for (int i = 1; i <= 12; i++)
            {
                detalle.Columns.Add("fondo" + i);
                detalle.Columns.Add("emprendedor" + i);
            }
            detalle.Columns.Add("fondoTotal");
            detalle.Columns.Add("emprendedorTotal");

            foreach (var item in query)
            {
                DataRow dr = datos.NewRow();

                dr["CodProyecto"] = CodigoProyecto;
                dr["Id_Actividad"] = item.Id_Actividad;
                dr["Item"] = item.Item;
                dr["Actividad"] = item.NomActividad;
                datos.Rows.Add(dr);
            }
            int actividadActual = 0;
            DataRow drDet = detalle.NewRow();

            decimal totalFondo = 0;
            decimal totalEmprendedor = 0;

            foreach (ProyectoActividadPOMe registro in respuestaDetalle)
            {
                if (actividadActual != registro.CodActividad)
                {
                    if (actividadActual != 0)
                    {

                        drDet["fondoTotal"] = Clases.FieldValidate.moneyFormat(totalFondo, true); // "$" + String.Format("{0:0.00}", totalFondo);
                        //"$" + totalFondo.ToString("0,0.00", CultureInfo.InvariantCulture);
                        drDet["emprendedorTotal"] = Clases.FieldValidate.moneyFormat(totalEmprendedor, true); // "$" + String.Format("{0:0.00}", totalEmprendedor);
                        //totalEmprendedor.ToString("0,0.00", CultureInfo.InvariantCulture);
                        totalFondo = 0;
                        totalEmprendedor = 0;
                        detalle.Rows.Add(drDet);
                    }
                    drDet = detalle.NewRow();
                    actividadActual = registro.CodActividad;
                }

                if (registro.CodTipoFinanciacion == 1)
                {
                    drDet["fondo" + registro.Mes] = Clases.FieldValidate.moneyFormat(registro.Valor, true); //"$" + String.Format("{0:0.00}", registro.Valor);                    
                    totalFondo += registro.Valor;
                }
                else if (registro.CodTipoFinanciacion == 2)
                {
                    drDet["emprendedor" + registro.Mes] = Clases.FieldValidate.moneyFormat(registro.Valor, true); // "$" + String.Format("{0:0.00}", registro.Valor);                    
                    totalEmprendedor += registro.Valor;
                }
            }

            drDet["fondoTotal"] = Clases.FieldValidate.moneyFormat(totalFondo, true); // "$" + totalFondo.ToString("0,0.00", CultureInfo.InvariantCulture);
            drDet["emprendedorTotal"] = Clases.FieldValidate.moneyFormat(totalEmprendedor,true); // "$" + totalEmprendedor.ToString("0,0.00", CultureInfo.InvariantCulture);
            detalle.Rows.Add(drDet);

            gw_Anexos.DataSource = datos;
            gw_Anexos.DataBind();

            gw_AnexosActividad.DataSource = detalle;
            gw_AnexosActividad.DataBind();

            for (int i = 0; i < gw_Anexos.Rows.Count; i++)
            {
                if ((miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && !realizado) || bNuevo == false)
                {
                    ((ImageButton)gw_Anexos.Rows[i].Cells[0].FindControl("btn_Borrar")).Visible = true;
                    ((LinkButton)gw_Anexos.Rows[i].Cells[2].FindControl("btnEditar")).Enabled = true;
                    //((Label)gw_Anexos.Rows[i].Cells[2].FindControl("lblEditar")).Visible = false;
                }
                else
                {
                    ((ImageButton)gw_Anexos.Rows[i].Cells[0].FindControl("btn_Borrar")).Visible = false;
                    ((LinkButton)gw_Anexos.Rows[i].Cells[2].FindControl("btnEditar")).Enabled = false;
                }
            }
        }

        protected void gw_Anexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string Id_Actividad = e.CommandArgument.ToString(); ;

            switch (e.CommandName.ToString())
            {
                case "Editar":
                    CargarFormularioEdicion(Id_Actividad);
                    break;
                case "Borrar":
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        BorrarDatosActividadInterventor(Id_Actividad);
                    }
                    else
                    {
                        BorrarDatosActividad(Id_Actividad);
                    }
                    consultas.Db.SubmitChanges();
                    ObtenerDatosUltimaActualizacion();
                    CargarGridActividades();
                    break;
            }

        }

        protected void BorrarDatosActividadInterventor(string Id_Actividad)
        {
            try
            {
                var query = (from p in consultas.Db.Interventors
                             where p.CodContacto == usuario.IdContacto
                             select new { p.CodCoordinador }).FirstOrDefault();

                ProyectoActividadPOInterventorTMP datoNuevo = new ProyectoActividadPOInterventorTMP();
                datoNuevo.Id_Actividad = Convert.ToInt32(Id_Actividad);
                datoNuevo.CodProyecto = Convert.ToInt32(CodigoProyecto);
                datoNuevo.Tarea = "Borrar";
                consultas.Db.ProyectoActividadPOInterventorTMPs.InsertOnSubmit(datoNuevo);

                string sentenciaSQL = "INSERT INTO proyectoactividadPOMesInterventorTMP(CodActividad) ";
                sentenciaSQL += "VALUES({0},{1},{2},{3})";
                consultas.Db.ExecuteCommand(sentenciaSQL, Id_Actividad);
                ObtenerDatosUltimaActualizacion();
            }
            catch
            {
                //Alert1.Ver("No tiene ningún coordinador asignado.", true);
            }
        }

        protected void BorrarDatosActividad(string Id_Actividad)
        {

            string sentenciaSQL = "Delete ProyectoactividadPOMes where CodActividad={0}";
            consultas.Db.ExecuteCommand(sentenciaSQL, Convert.ToInt32(Id_Actividad));

            string sentenciaSQL2 = "Delete ProyectoactividadPO where Id_Actividad={0}";
            consultas.Db.ExecuteCommand(sentenciaSQL2, Convert.ToInt32(Id_Actividad));

            ObtenerDatosUltimaActualizacion();

        }

        protected void btnAdicionarActividadPlan_Click(object sender, EventArgs e)
        {

            pnlPrincipal.Visible = false;
            pnlCrearActividad.Visible = true;
            btnCrearActividad.Text = "Crear";
            lblTitulo.Text = "NUEVA ACTIVIDAD";
            LimpiarFormularioCreacion();
        }

        protected void LimpiarFormularioCreacion()
        {
            foreach (Control c in pnlCrearActividad.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
            }
        }

        private void CargarFormularioEdicion(string idActividad)
        {
            bool habilitar = true;
            HttpContext.Current.Session["tarea"] = "";
            bool chequeoCoordinador = false;
            bool chequeoGerente = false;
            string txtSQL = "";

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                habilitar = false;
                try
                {
                    var query = (from p in consultas.Db.ProyectoActividadPOInterventorTMPs
                                 where p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                                 p.Id_Actividad == Convert.ToInt64(idActividad)
                                 select new { p.Tarea, p.ChequeoCoordinador, p.ChequeoGerente }).FirstOrDefault();

                    HttpContext.Current.Session["tarea"] = query.Tarea;
                    chequeoCoordinador = (bool)query.ChequeoCoordinador;
                    chequeoGerente = (bool)query.ChequeoGerente;
                }
                catch
                {
                    //Alert1.Ver("Tarea ya Aprobada", true);
                    return;
                }
            }
            else if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
                txtSQL = "SELECT cast(Id_Actividad as int) asIdActividad, cast(NomActividad as varchar(100)) as NombreActividad, cast(CodProyecto as int) as CodProyecto, cast(Item as int ) as Item, cast(Metas as varchar(100)) as Metas FROM proyectoactividadPOInterventor  WHERE id_Actividad ={0}";
            }
            else
            {
                txtSQL = "SELECT cast(Id_Actividad as int) asIdActividad, cast(NomActividad as varchar(100)) as NombreActividad, cast(CodProyecto as int) as CodProyecto, cast(Item as int ) as Item, cast(Metas as varchar(100)) as Metas FROM proyectoactividadPO  WHERE id_Actividad ={0}";
            }
            if ((usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor) && HttpContext.Current.Session["tarea"].ToString() == "Adicionar")
            {
                lblTitulo.Text = "ADICIONAR ACTIVIDAD";
            }
            else if ((usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor) && HttpContext.Current.Session["tarea"].ToString() == "Modificar")
            {
                lblTitulo.Text = "MODIFICAR ACTIVIDAD";
            }
            else if ((usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor) && HttpContext.Current.Session["tarea"].ToString() == "Borrar")
            {
                lblTitulo.Text = "BORRAR ACTIVIDAD";
                txtSQL = "SELECT cast(Id_Actividad as int) asIdActividad, cast(NomActividad as varchar(100)) as NombreActividad, cast(CodProyecto as int) as CodProyecto, cast(Item as int ) as Item, cast(Metas as varchar(100)) as Metas FROM proyectoactividadPOInterventor where id_Actividad = {0}";
            }
            else
            {
                lblTitulo.Text = "Editar";
            }

            txtItem.Enabled = false;
            txtNombreActividad.Enabled = habilitar;
            txtMetas.Enabled = habilitar;
            for (int i = 1; i <= 12; i++)
            {
                ((TextBox)this.FindControl("txtAporte" + i)).Enabled = habilitar;
                ((TextBox)this.FindControl("txtFondo" + i)).Enabled = habilitar;
            }

            RespuestaProyectoactividadPO resultadoConsulta = consultas.Db.ExecuteQuery<RespuestaProyectoactividadPO>(txtSQL, Convert.ToInt32(idActividad)).FirstOrDefault();

            txtItem.Text = resultadoConsulta.Item.ToString();
            txtNombreActividad.Text = resultadoConsulta.NombreActividad.htmlDecode();
            txtMetas.Text = resultadoConsulta.Metas.htmlDecode();

            pnlAprobacion.Visible = false;
            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                pnlAprobacion.Visible = true;
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor && chequeoCoordinador)
                    ddlAprobado.SelectedValue = "Si";
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor && chequeoGerente)
                    ddlAprobado.SelectedValue = "Si";
            }

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                if (HttpContext.Current.Session["tarea"].ToString() == "Borrar")
                {
                    txtSQL = "select distinct CodActividad, CodTipoFinanciacion,mes , valor from proyectoactividadPOmesInterventor where CodActividad = {0} order by CodTipoFinanciacion,Mes";
                }
                else
                {
                    txtSQL = "select distinct CodActividad, CodTipoFinanciacion,mes , valor from proyectoactividadPOmesInterventorTMP where CodActividad = {0} and codtipofinanciacion is not null order by CodTipoFinanciacion,Mes";
                }
            }
            else if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
                txtSQL = "select distinct CodActividad, CodTipoFinanciacion ,mes , valor from proyectoactividadPOmesInterventor where CodActividad = {0} order by CodTipoFinanciacion,Mes";
            }
            else
            {
                txtSQL = "select distinct CodActividad, CodTipoFinanciacion,mes, valor from proyectoactividadPOmes where CodActividad = {0} order by CodTipoFinanciacion,Mes";
            }
            

            var resultadoValores = (from pa in consultas.Db.ProyectoActividadPOMes
                                    where pa.CodActividad == int.Parse(idActividad)
                                    select pa).ToList();

            Int64 totalFondo = 0;
            Int64 totalAporte = 0;
            Int64[] totalMeses = new Int64[13];

            if (resultadoValores.Count > 0)
            {
                foreach (ProyectoActividadPOMe registro in resultadoValores)
                {
                    if (registro.CodTipoFinanciacion == 1)
                    {
                        ((TextBox)this.FindControl("txtFondo" + registro.Mes)).Text = registro.Valor.ToString("###.00").Replace(",", ".");
                        totalFondo += Convert.ToInt64(registro.Valor);
                    }
                    else
                    {
                        ((TextBox)this.FindControl("txtAporte" + registro.Mes)).Text = registro.Valor.ToString("###.00").Replace(",", ".");
                        totalAporte += Convert.ToInt64(registro.Valor);
                    }
                    totalMeses[registro.Mes] += Convert.ToInt64(registro.Valor);
                }
            }

            txtAporteTotal.InnerText = "$" + totalAporte.ToString("0,0.00", CultureInfo.InvariantCulture); ; ;
            txtFondoTotal.InnerHtml = "$" + totalFondo.ToString("0,0.00", CultureInfo.InvariantCulture); ; ;

            for (int i = 1; i <= 12; i++)
            {
                ((HtmlTableCell)this.FindControl("TotalMes" + i)).InnerHtml = "$" + totalMeses[i].ToString("0,0.00", CultureInfo.InvariantCulture); ; ;
            }

            pnlPrincipal.Visible = false;
            pnlCrearActividad.Visible = true;
            hddIdActividad.Value = idActividad;
            btnCrearActividad.Text = "Actualizar";
        }

        protected void btnCrearActividad_Click(object sender, EventArgs e)
        {

            if (hddIdActividad.Value == "")
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    InsertarDatosActividadInterventor();
                }
                else
                {
                    InsertarDatosActividad();
                }
                consultas.Db.SubmitChanges();
                ObtenerDatosUltimaActualizacion();
            }
            else
            {

                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor && HttpContext.Current.Session["tarea"].ToString() == "Adicionar")
                {
                    EditarGerenteInterventorAdiciona();
                }

                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor && HttpContext.Current.Session["tarea"].ToString() == "Modificar")
                {
                    EditarGerenteInterventorModificar();
                }

                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor && HttpContext.Current.Session["tarea"].ToString() == "Borrar")
                {
                    EditarGerenteInterventorBorrar();
                }

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    EditarCoordinadorInterventor();
                }

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    EditarInterventor();
                }

                if (usuario.CodGrupo == Constantes.CONST_Emprendedor || usuario.CodGrupo == Constantes.CONST_Evaluador)
                {
                    EditarEmpleador_Evaluador();
                }
                HttpContext.Current.Session["tarea"] = "";
                hddIdActividad.Value = "";
            }

            consultas.Db.SubmitChanges();
            ObtenerDatosUltimaActualizacion();
            CargarGridActividades();

            pnlPrincipal.Visible = true;
            pnlCrearActividad.Visible = false;

            btnCerrar_Click(sender, e);

        }

        protected void EditarGerenteInterventorAdiciona()
        {
            if (ddlAprobado.SelectedValue == "Si")
            {
                var queryCosulta = (from p in consultas.Db.ProyectoActividadPOInterventorTMPs
                                    where p.CodProyecto == Convert.ToInt64(CodigoProyecto) &&
                                 p.Id_Actividad == Convert.ToInt64(hddIdActividad.Value)
                                    select p).FirstOrDefault();

                ProyectoActividadPOInterventor nuevoDato = new ProyectoActividadPOInterventor();
                nuevoDato.CodProyecto = Convert.ToInt32(CodigoProyecto);
                nuevoDato.Item = (short)queryCosulta.Item;
                nuevoDato.NomActividad = queryCosulta.NomActividad;
                nuevoDato.Metas = queryCosulta.Metas;

                consultas.Db.ProyectoActividadPOInterventors.InsertOnSubmit(nuevoDato);

                consultas.Db.ProyectoActividadPOInterventorTMPs.DeleteOnSubmit(queryCosulta);

                string codigoActividadNueva = nuevoDato.Id_Actividad.ToString();

                var queryMeses = (from p in consultas.Db.ProyectoActividadPOMesInterventorTMPs
                                  where p.CodActividad == Convert.ToInt64(hddIdActividad.Value)
                                  select p).Distinct();

                foreach (ProyectoActividadPOMesInterventorTMP registro in queryMeses)
                {
                    try
                    {
                        var queryExistente = (from p in consultas.Db.ProyectoActividadPOMesInterventors
                                              where p.CodActividad == Convert.ToInt64(hddIdActividad.Value) &&
                                              p.Mes == registro.Mes
                                              select p).FirstOrDefault();

                        queryExistente.CodTipoFinanciacion = (byte)registro.CodTipoFinanciacion;
                        queryExistente.Valor = (decimal)registro.Valor;

                    }
                    catch
                    {
                        ProyectoActividadPOMesInterventor datoNuevo = new ProyectoActividadPOMesInterventor();
                        datoNuevo.CodActividad = Convert.ToInt32(codigoActividadNueva);
                        datoNuevo.Mes = (byte)registro.Mes;
                        datoNuevo.CodTipoFinanciacion = (byte)registro.CodTipoFinanciacion;
                        datoNuevo.Valor = (decimal)registro.Valor;
                        consultas.Db.ProyectoActividadPOMesInterventors.InsertOnSubmit(datoNuevo);
                    }
                }

                ///COMENTADO: Genera error de PK.
                //consultas.Db.ExecuteCommand("DELETE ProyectoActividadPOMesInterventorTMP where CodActividad={0}", Convert.ToInt32(hddIdActividad.Value));
                //consultas.Db.SubmitChanges();

                //Ejecutar consulta.
                ejecutaReader("DELETE ProyectoActividadPOMesInterventorTMP where CodActividad=" + hddIdActividad.Value, 2);

                ObtenerDatosUltimaActualizacion();
            }
            else if (ddlAprobado.SelectedValue == "No")
            {
                var queryCodContacto = (from ei in consultas.Db.EmpresaInterventors
                                        from e in consultas.Db.Empresas
                                        where ei.CodEmpresa == e.id_empresa &&
                                        ei.Inactivo == Convert.ToBoolean(0) &&
                                        ei.Rol == Constantes.CONST_RolInterventorLider &&
                                 e.codproyecto == Convert.ToInt32(CodigoProyecto)
                                        select new { ei.CodContacto }).FirstOrDefault();

                ///COMENTADO: Genera error de PK.
                //consultas.Db.ExecuteCommand("DELETE FROM proyectoactividadPOInterventorTMP  where CodProyecto={0} and Id_Actividad={1}", Convert.ToInt32(codProyecto), Convert.ToInt32(hddIdActividad.Value));
                //consultas.Db.ExecuteCommand("DELETE FROM ProyectoactividadPOMesInterventorTMP  where Id_Actividad={1}", Convert.ToInt32(hddIdActividad.Value));

                //Ejecutar consulta.
                ejecutaReader("DELETE FROM proyectoactividadPOInterventorTMP  where CodProyecto=" + CodigoProyecto + " and Id_Actividad=" + hddIdActividad.Value, 2);

                //Ejecutar consulta.
                ejecutaReader("DELETE FROM ProyectoactividadPOMesInterventorTMP  where Id_Actividad=" + hddIdActividad.Value, 2);

                string nomActividad = "";

                AgendarTarea agenda = new AgendarTarea(
                                    Convert.ToInt32(queryCodContacto.CodContacto),
                                    "Actividad del Plan Operativo Rechazada por Coordinador de Interventoria",
                                "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + nomActividad + "<BR><BR>Observaciones:<BR>" + txtObservaciones.Text,
                                CodigoProyecto.ToString(), 2, "0", false, 1, true, false, usuario.IdContacto, ("CodProyecto=" + CodigoProyecto), "", "Catálogo Actividad Plan Operativo");
                agenda.Agendar();
                ObtenerDatosUltimaActualizacion();
            }
        }

        protected void EditarGerenteInterventorModificar()
        {
            if (ddlAprobado.SelectedValue == "Si")
            {
                var queryCosultaTMP = (from p in consultas.Db.ProyectoActividadPOInterventorTMPs
                                       where p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                                    p.Id_Actividad == Convert.ToInt64(hddIdActividad.Value)
                                       select p).FirstOrDefault();

                var registroActualPOInterventor = (from p in consultas.Db.ProyectoActividadPOInterventors
                                                   where p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                                                   p.Id_Actividad == Convert.ToInt64(hddIdActividad.Value)
                                                   select p).First();

                registroActualPOInterventor.Item = (short)queryCosultaTMP.Item; ;
                registroActualPOInterventor.NomActividad = queryCosultaTMP.NomActividad;
                registroActualPOInterventor.Metas = queryCosultaTMP.Metas;

                consultas.Db.ProyectoActividadPOInterventorTMPs.DeleteOnSubmit(queryCosultaTMP);

                string codigoActividadNueva = registroActualPOInterventor.Id_Actividad.ToString();

                var queryMeses = (from p in consultas.Db.ProyectoActividadPOMesInterventorTMPs
                                  where p.CodActividad == Convert.ToInt64(hddIdActividad.Value) &&
                                  p.Valor != null
                                  select p).Distinct();

                ///COMENTADO: Genera error de PK.
                //consultas.Db.ExecuteCommand("DELETE ProyectoActividadPOMesInterventor  where CodActividad={0}", Convert.ToInt32(hddIdActividad.Value));

                //Ejecutar consulta.
                ejecutaReader("DELETE ProyectoActividadPOMesInterventor  where CodActividad=" + hddIdActividad.Value, 2);


                foreach (ProyectoActividadPOMesInterventorTMP registro in queryMeses)
                {
                    try
                    {
                        var queryExistente = (from p in consultas.Db.ProyectoActividadPOMesInterventors
                                              where p.CodActividad == Convert.ToInt64(hddIdActividad.Value) &&
                                              p.Mes == registro.Mes &&
                                              p.CodTipoFinanciacion == registro.CodTipoFinanciacion
                                              select p).FirstOrDefault();

                        queryExistente.Valor = (decimal)registro.Valor;

                    }
                    catch
                    {
                        ProyectoActividadPOMesInterventor datoNuevo = new ProyectoActividadPOMesInterventor();
                        datoNuevo.CodActividad = Convert.ToInt32(codigoActividadNueva);
                        datoNuevo.Mes = (byte)registro.Mes;
                        datoNuevo.CodTipoFinanciacion = (byte)registro.CodTipoFinanciacion;
                        datoNuevo.Valor = (decimal)registro.Valor;
                        consultas.Db.ProyectoActividadPOMesInterventors.InsertOnSubmit(datoNuevo);
                    }
                }

                ///COMENTADO: Genera error de PK.
                //consultas.Db.ExecuteCommand("DELETE ProyectoActividadPOMesInterventorTMP where CodActividad={0}", Convert.ToInt32(hddIdActividad.Value));
                //consultas.Db.SubmitChanges();

                //Ejecutar consulta.
                ejecutaReader("DELETE ProyectoActividadPOMesInterventorTMP where CodActividad=" + hddIdActividad.Value, 2);

                ObtenerDatosUltimaActualizacion();
            }
        }

        protected void EditarGerenteInterventorBorrar()
        {
            List<string> Consultas_SQL = new List<string>();

            if (ddlAprobado.SelectedValue == "Si")
            {
                ///COMENTADO: Genera error por PK.
                //consultas.Db.ExecuteCommand("DELETE proyectoactividadPOInterventor  where CodProyecto={0} and Id_Actividad= {1}", Convert.ToInt32(codProyecto), Convert.ToInt32(hddIdActividad.Value));
                //consultas.Db.ExecuteCommand("DELETE proyectoactividadPOInterventorTMP  where CodProyecto={0} and Id_Actividad= {1}", Convert.ToInt32(codProyecto), Convert.ToInt32(hddIdActividad.Value));
                //consultas.Db.ExecuteCommand("DELETE ProyectoActividadPOMesInterventor  where CodActividad={0}", Convert.ToInt32(hddIdActividad.Value));
                //consultas.Db.ExecuteCommand("DELETE ProyectoActividadPOMesInterventorTMP  where CodActividad={0}", Convert.ToInt32(hddIdActividad.Value));
                //consultas.Db.SubmitChanges();

                //Elementos con consultas SQL a ejecutar.
                string txtSQL_1 = "DELETE proyectoactividadPOInterventor where CodProyecto=" + CodigoProyecto + " and Id_Actividad=" + hddIdActividad.Value;
                string txtSQL_2 = "DELETE proyectoactividadPOInterventorTMP where CodProyecto=" + CodigoProyecto + " and Id_Actividad=" + hddIdActividad.Value;
                string txtSQL_3 = "DELETE ProyectoActividadPOMesInterventor where CodActividad=" + hddIdActividad.Value;
                string txtSQL_4 = "DELETE ProyectoActividadPOMesInterventorTMP where CodActividad=" + hddIdActividad.Value;

                //Se agregan los elementos a la lista.
                Consultas_SQL.Add(txtSQL_1);
                Consultas_SQL.Add(txtSQL_2);
                Consultas_SQL.Add(txtSQL_3);
                Consultas_SQL.Add(txtSQL_4);

                //Se recorre la lista para ejecutar las consulas SQL almacenadas en ella.
                foreach (string sql_delete_query in Consultas_SQL)
                {
                    //Ejecutar consulta.
                    ejecutaReader(sql_delete_query, 2);
                }

                //Destruir la lista con sus elementos.
                Consultas_SQL.Clear();
                Consultas_SQL = null;

                ObtenerDatosUltimaActualizacion();
            }
        }

        protected void EditarCoordinadorInterventor()
        {
            if (ddlAprobado.SelectedValue == "Si")
            {
                var queryCosulta = (from p in consultas.Db.ProyectoActividadPOInterventorTMPs
                                    where p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                                 p.Id_Actividad == Convert.ToInt32(hddIdActividad.Value)
                                    select p).FirstOrDefault();


                queryCosulta.ChequeoCoordinador = true;
                consultas.Db.SubmitChanges();
            }
            else if (ddlAprobado.SelectedValue == "No")
            {
                var queryCodContacto = (from ei in consultas.Db.EmpresaInterventors
                                        from e in consultas.Db.Empresas
                                        where ei.CodEmpresa == e.id_empresa &&
                                        ei.Inactivo == false &&
                                        ei.Rol == Constantes.CONST_RolInterventorLider &&
                                        e.codproyecto == Convert.ToInt32(CodigoProyecto)
                                        select new { ei.CodContacto }).FirstOrDefault();
                                                
                //Ejecutar consulta.
                ejecutaReader("DELETE FROM proyectoactividadPOInterventorTMP where CodProyecto=" + CodigoProyecto + " and Id_Actividad=" + hddIdActividad.Value, 2);

                //Ejecutar consulta.
                ejecutaReader("DELETE FROM ProyectoactividadPOMesInterventorTMP  where Id_Actividad=" + hddIdActividad.Value, 2);

                string nomActividad = "";

                AgendarTarea agenda = new AgendarTarea(
                                    Convert.ToInt32(queryCodContacto.CodContacto),
                                    "Actividad del Plan Operativo Rechazada por Coordinador de Interventoria",
                                "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + nomActividad + "<BR><BR>Observaciones:<BR>" + txtObservaciones.Text,
                                CodigoProyecto.ToString(), 2, "0", false, 1, true, false, usuario.IdContacto, ("CodProyecto=" + CodigoProyecto), "", "Catálogo Actividad Plan Operativo");
                agenda.Agendar();
            }

            ObtenerDatosUltimaActualizacion();
        }

        protected void EditarInterventor()
        {
            try
            {
                var query = (from p in consultas.Db.Interventors
                             where p.CodContacto == usuario.IdContacto
                             select new { p.CodCoordinador }).FirstOrDefault();

                ProyectoActividadPOInterventorTMP datoNuevo = new ProyectoActividadPOInterventorTMP();
                datoNuevo.Id_Actividad = Convert.ToInt32(hddIdActividad.Value);
                datoNuevo.CodProyecto = Convert.ToInt32(CodigoProyecto);
                datoNuevo.Item = Convert.ToInt16(txtItem.Text.ToString());
                datoNuevo.NomActividad = txtNombreActividad.Text.htmlEncode();
                datoNuevo.Metas = txtMetas.Text.htmlEncode();
                datoNuevo.Tarea = "Modificar";

                consultas.Db.ProyectoActividadPOInterventorTMPs.InsertOnSubmit(datoNuevo);

                consultas.Db.ExecuteCommand("DELETE FROM ProyectoactividadPOMesInterventorTMP  where Id_Actividad={1}", Convert.ToInt64(hddIdActividad.Value));
                ObtenerDatosUltimaActualizacion();

                for (int i = 1; i <= 12; i++)
                {
                    CrearNuevoRegistroActividadInterventor(Convert.ToInt32(hddIdActividad.Value), "txtFondo", i, 1);
                    CrearNuevoRegistroActividadInterventor(Convert.ToInt32(hddIdActividad.Value), "txtAporte", i, 2);
                }
            }
            catch
            {
                //Alert1.Ver("No tiene ningún coordinador asignado.", true);
                //Mensaje de error...
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.opener.location.reload();window.close();", true);
                //return;
            }
        }

        protected void EditarEmpleador_Evaluador()
        {
            try
            {
                var query = (from p in consultas.Db.ProyectoActividadPOs
                             where p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                             p.Id_Actividad == Convert.ToInt32(hddIdActividad.Value)
                             select new { p.Id_Actividad }).FirstOrDefault();

                var consultaUpdate = (from p in consultas.Db.ProyectoActividadPOs
                                      where p.Id_Actividad == Convert.ToInt32(hddIdActividad.Value)
                                      select p).First();

                consultaUpdate.NomActividad = txtNombreActividad.Text.htmlEncode();
                consultaUpdate.Item = Convert.ToInt16(txtItem.Text);
                consultaUpdate.Metas = txtMetas.Text.htmlEncode();
                consultas.Db.SubmitChanges();

                //COMENTADO: Generar error por PK.
                //consultas.Db.ExecuteCommand("DELETE FROM ProyectoactividadPOMes  where CodActividad={1}", Convert.ToInt32(hddIdActividad.Value));

                //Ejecutar consulta.

                ejecutaReader("DELETE FROM ProyectoactividadPOMes where CodActividad=" + hddIdActividad.Value, 2);

                ObtenerDatosUltimaActualizacion();
                //Esta bien que borre pero debe preguntar quien lo esta pidiendo para saber si lo debe modificar o no
                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    InsertarDatosActividad();
                }
                if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                {

                    for (int i = 1; i <= 12; i++)
                    {
                        CrearNuevoRegistroActividadInterventor(Convert.ToInt32(hddIdActividad.Value), "txtFondo", i, 1);
                        CrearNuevoRegistroActividadInterventor(Convert.ToInt32(hddIdActividad.Value), "txtAporte", i, 2);
                    }

                }

            }
            catch { /*bRepetido = true;*/ }
        }

        protected void InsertarDatosActividadInterventor()
        {
            int idActividadTPM = 0;

            try
            {
                var query = (from p in consultas.Db.Interventors
                             where p.CodContacto == usuario.IdContacto
                             select new { p.CodCoordinador }).FirstOrDefault();
                try
                {
                    var queryActividadInterventor = (from p in consultas.Db.ProyectoActividadPOInterventorTMPs
                                                     select new { p.Id_Actividad }).FirstOrDefault();
                    idActividadTPM = queryActividadInterventor.Id_Actividad + 1 + Convert.ToInt32(CodigoProyecto);
                }
                catch { idActividadTPM = 0; }

                ProyectoActividadPOInterventorTMP datoNuevo = new ProyectoActividadPOInterventorTMP();
                datoNuevo.Id_Actividad = idActividadTPM;
                datoNuevo.CodProyecto = Convert.ToInt32(CodigoProyecto);
                datoNuevo.Item = Convert.ToInt16(txtItem.Text.ToString());
                datoNuevo.NomActividad = txtNombreActividad.Text.htmlEncode();
                datoNuevo.Metas = txtMetas.Text.htmlEncode();

                consultas.Db.ProyectoActividadPOInterventorTMPs.InsertOnSubmit(datoNuevo);
                ObtenerDatosUltimaActualizacion();

                for (int i = 1; i <= 12; i++)
                {
                    CrearNuevoRegistroActividadInterventor(idActividadTPM, "txtFondo", i, 1);
                    CrearNuevoRegistroActividadInterventor(idActividadTPM, "txtAporte", i, 2);
                }
            }
            catch { /*Alert1.Ver("No tiene ningún coordinador asignado.", true);*/ }
        }

        protected void InsertarDatosActividad()
        {
            bool bRepetido = true;

            try
            {
                ejecutaReader("DELETE FROM ProyectoactividadPO where CodProyecto = " + CodigoProyecto + " AND Item = " + txtItem.Text.ToString() + " AND NomActividad = '" + txtNombreActividad.Text.htmlEncode() + "'", 2);

                ProyectoActividadPO datoNuevo = new ProyectoActividadPO();
                datoNuevo.CodProyecto = Convert.ToInt32(CodigoProyecto);
                datoNuevo.Item = Convert.ToInt16(txtItem.Text.ToString());
                datoNuevo.NomActividad = txtNombreActividad.Text;
                datoNuevo.Metas = txtMetas.Text.htmlEncode();
                consultas.Db.ProyectoActividadPOs.InsertOnSubmit(datoNuevo);
                consultas.Db.SubmitChanges();
                ObtenerDatosUltimaActualizacion();

                #region Consultar el ID de la actividad recién creada para realizar las siguientes inserciones.

                //No cargar bien el ID de la actividad RECIÉN creada, por lo que
                //se debe consultar el ID usando SQL directamente.
                //var rt = consultas.ObtenerDataTable("SELECT Id_Actividad FROM ProyectoActividadPO WHERE NomActividad = '" + txtNombreActividad.Text.htmlEncode() + "' AND CodProyecto = " + codProyecto, "text");
                int Id_Actividad = datoNuevo.Id_Actividad;
                //if (rt.Rows.Count > 0) { Id_Actividad = Int32.Parse(rt.Rows[0]["Id_Actividad"].ToString()); }
                //rt = null;

                #endregion

                if (datoNuevo.Id_Actividad > 0)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        CrearNuevoRegistroActividad(Id_Actividad, "txtFondo", i, 1);  //dato.Id_Actividad
                        CrearNuevoRegistroActividad(Id_Actividad, "txtAporte", i, 2); //dato.Id_Actividad
                    }
                    bRepetido = false;
                }
                else
                    bRepetido = true;
            }
            catch { /*Alert1.Ver("No tiene ningún coordinador asignado.", true);*/ }
        }

        protected void CrearNuevoRegistroActividadInterventor(int idActividadTPM, string nombreCampo, int mes, int financiacion)
        {
            if (((TextBox)this.FindControl(nombreCampo + mes)).Text != "" && ((TextBox)this.FindControl(nombreCampo + mes)).Text != "0")
            {
                decimal valor = Convert.ToDecimal(((TextBox)this.FindControl(nombreCampo + mes)).Text);
                //string sentenciaSQL = "INSERT INTO ProyectoactividadPOMesInterventorTMP(CodActividad,Mes,CodTipoFinanciacion,Valor) ";
                //sentenciaSQL += "VALUES({0},{1},{2},{3})";
                //COMENTADO: Genera error de PK.
                //consultas.Db.ExecuteCommand(sentenciaSQL, idActividadTPM, (byte)mes, (byte)financiacion, valor);

                string sentenciaSQL = "";
                sentenciaSQL = "INSERT INTO ProyectoactividadPOMesInterventorTMP(CodActividad,Mes,CodTipoFinanciacion,Valor) " +
                               "VALUES(" + idActividadTPM + ", " + mes + ", " + financiacion + ", " + valor + ")";

                //Ejecutar consulta.
                ejecutaReader(sentenciaSQL, 2);
            }
        }

        protected void CrearNuevoRegistroActividad(int idActividad, string nombreCampo, int mes, int financiacion)
        {
            if (((TextBox)this.FindControl(nombreCampo + mes)).Text != "" && ((TextBox)this.FindControl(nombreCampo + mes)).Text != "0")
            {
                TextBox txtValorUnidad = (TextBox)this.FindControl(nombreCampo + mes);

                decimal valor = Decimal.Parse(txtValorUnidad.Text.Replace(",",string.Empty).Replace(".", ","));
                //string sentenciaSQL = "INSERT INTO ProyectoactividadPOMes(CodActividad,Mes,CodTipoFinanciacion,Valor)  ";
                //sentenciaSQL += "VALUES({0},{1},{2},{3})";
                //COMENTADO: Generaba error de PK.
                //consultas.Db.ExecuteCommand(sentenciaSQL, idActividad, (byte)mes, (byte)financiacion, valor);

                string sentenciaSQL = "";
                sentenciaSQL = "INSERT INTO ProyectoactividadPOMes(CodActividad,Mes,CodTipoFinanciacion,Valor)  " +
                               "VALUES(" + idActividad + ", " + mes + ", " + financiacion + ", " + valor.ToString().Replace(",", ".") + ")";

                //Ejecutar consulta.
                ejecutaReader(sentenciaSQL, 2);
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlCrearActividad.Visible = false;
            hddIdActividad.Value = "";
            Response.Redirect(Request.RawUrl);
        }

        #region Métodos de .

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>

        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            String txtSQL;
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), CodigoProyecto.ToString(), ""); //codConvocatoria);

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(CodigoProyecto, txtTab);

                var act = usuActualizo.ToList();

                if (act.Count() > 0)
                {
                    lbl_nombre_user_ult_act.Text = usuActualizo.SingleOrDefault().nombres.ToUpper();

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(usuActualizo.SingleOrDefault().fecha); }
                    catch { fecha = DateTime.Today; }
                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    //Obtener la hora en minúscula.
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();
                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); }
                    if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }
                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    //Realizado
                    bRealizado = usuActualizo.SingleOrDefault().realizado;
                }

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(CodigoProyecto.ToString())))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                if (EsMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                {
                    btm_guardarCambios.Visible = true;
                }
                else
                {
                    btm_guardarCambios.Visible = false;
                }

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(CodigoProyecto.ToString())))
                {
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                    {
                        visibleGuardar = false;
                    }
                    else
                    {
                        visibleGuardar = true;
                    }
                }

                //Mostrar los enlaces para adjuntar documentos.
                if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEmprendedor.ToString() && !bRealizado)
                {
                    //tabla_docs.Visible = true;
                }
                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                //tabla = null;
                //txtSQL = null;
                //return;
                lblMensajeError.Text = ex.Message;
            }
        }

        /// <summary>

        /// 06/06/2014.
        /// Obtener el número "numPostIt" usado en la condicional de "obtener última actualización".
        /// El código se encuentra en "Base_Page" línea "116", método "inicioEncabezado".
        /// Ya se le están enviado por parámetro en el método el código del proyecto y la constante "CONST_PostIt".
        /// </summary>
        /// <returns>numPostIt.</returns>
        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;

            //Hallar numero de post it por tab
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(CodigoProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }

        /// <summary>

        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            prActualizarTab(txtTab.ToString(), CodigoProyecto.ToString());
            Marcar(txtTab.ToString(), CodigoProyecto.ToString(), "", chk_realizado.Checked);
            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "PlanOperOper";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "PlanOperOper";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion

        public void GenerarTabla(String Cod_Actividad, String Cod_Proyecto)
        {
            //Inicializar variables.
            String txtSQL = "";
            String nomCargo;
            Double TotalFE = 0;
            Double TotalEmp = 0;
            DataTable rsActividad = new DataTable();
            DataTable contador = new DataTable();
            DataTable rsTipo1 = new DataTable();
            DataTable rsTipo2 = new DataTable();
            DataTable rsPagoActividad = new DataTable();
            int ejecutar = 0;
            Table t_anexos = new Table();
            Double prorroga = 0;
            double prorrogaTotal = 0;
            #region Obtener el valor de la prórroga para sumarla a la constante de meses generar la tabla.
            prorroga = 0;
            prorrogaTotal = 0;
            prorroga = ObtenerProrroga(CodigoProyecto.ToString());
            if (prorroga == 0)
            { prorrogaTotal = prorroga + Constantes.CONST_Meses; /*El +1 es para evitar modificar aún mas el for...*/ }
            else { prorrogaTotal = prorroga + Constantes.CONST_Meses; }
            /*int prorroga = 0;
            prorroga = ObtenerProrroga(CodProyecto.ToString());
            int prorrogaTotal = prorroga + Constantes.CONST_Meses; */
            //El +1 es paar evitar modificar aún mas el for...
            #endregion


            //Inicializar tabla.
            t_anexos.Rows.Clear();

            //Inicializar la fila.
            TableRow fila = new TableRow();
            fila.Style.Add("text-align", "center");

            #region Generar la primera fila con los meses que tiene la nómina seleccionada.
            for (int i = 1; i <= prorrogaTotal; i++)
            {
                TableHeaderCell celda = new TableHeaderCell();
                celda.Style.Add("text-align", "center");
                celda.ColumnSpan = 2;
                celda.Text = "Mes " + i;
                fila.Cells.Add(celda);
                t_anexos.Rows.Add(fila);
                celda = null;
            }
            #endregion

            #region Crear una nueva celda que contiene el valor "Costo Total".
            TableHeaderCell celdaCostoTotal = new TableHeaderCell();
            celdaCostoTotal.Text = "Costo Total";
            celdaCostoTotal.Style.Add("text-align", "center");
            celdaCostoTotal.ColumnSpan = 2;
            fila.Cells.Add(celdaCostoTotal);
            t_anexos.Rows.Add(fila);
            celdaCostoTotal = null;
            #endregion

            #region Agregar nueva fila (para adicionar las celdas "Sueldo" y "Prestaciones").
            //Se obtiene la cantidad de celdas que tiene la primera fila para generar los Sueldos y las Prestaciones.
            int conteo_celdas = fila.Cells.Count + 1; //El +1 es para contar también la celda "Costo Total".
            //Se inicializa la variable para generar una nueva fila.
            fila = new TableRow();

            //Generar las celdas "Sueldo" y "Prestaciones".
            for (int i = 1; i < conteo_celdas; i++)
            {
                //Celdas "Sueldo" y "Prestaciones Sociales".
                TableHeaderCell celdaSueldo = new TableHeaderCell();
                celdaSueldo.Style.Add("text-align", "left");
                TableHeaderCell celdaPrestaciones = new TableHeaderCell();
                celdaPrestaciones.Style.Add("text-align", "left");

                //Agregar datos a la celda de Sueldo.
                celdaSueldo.Text = "fondo";
                fila.Cells.Add(celdaSueldo);
                t_anexos.Rows.Add(fila);
                celdaSueldo = null;

                //Agregar datos a la celda de Prestaciones Sociales.
                celdaPrestaciones.Text = "Emprendedor";
                fila.Cells.Add(celdaPrestaciones);
                t_anexos.Rows.Add(fila);
                celdaPrestaciones = null;
            }
            #endregion

            #region Personal calificado - Cargos.
            //Personal calificado - Cargos.
            txtSQL = " SELECT DISTINCT ISNULL(pm.CodActividad,0) CodActividad,ISNULL(pm.Mes,0) Mes,ISNULL(pm.CodTipoFinanciacion,0) as CodTipoFinanciacion,ISNULL(CONVERT(VARCHAR(2000),CAST(pm.Valor AS DECIMAL),1) ,0.0) Valor,pin.Id_Actividad,pin.NomActividad,pin.CodProyecto,pin.Item,pin.Metas " +
                                      " FROM ProyectoActividadPOMesInterventor pm RIGHT OUTER JOIN proyectoactividadPOInterventor pin On pin.id_actividad= pm.CodActividad " +
                                      " Where pin.codproyecto=" + Cod_Proyecto + "  AND pin.Id_actividad = " + Cod_Actividad +
                                      " ORDER BY item, mes,codtipofinanciacion ";


            /* " WHERE a.Tipo='Cargo' AND a.Id_Nomina = b.CodCargo AND a.CodProyecto = " + Cod_Proyecto +
               " AND b.Mes <> 0 and a.Id_Nomina = " + codActividad + " " +
               " ORDER BY a.Id_Nomina, b.Mes, b.Tipo";*/


            //Asignar resultados de la consulta anterior a variable DataTable.
            rsActividad = consultas.ObtenerDataTable(txtSQL, "text");

            #endregion

            #region Contador...
            //Contador...
            /*  txtSQL = "SELECT count(*) AS contador " +
                          "FROM InterventorNomina " +
                          "WHERE tipo='Cargo'";

              //Asignar resultados de la consulta anterior a variable DataTable.
              contador = consultas.ObtenerDataTable(txtSQL, "text");*/
            contador = consultas.ObtenerDataTable(txtSQL, "text");
            #endregion

            //Crear variable temporal que contiene el código del cargo "asignable mas adelante".
            string CodCargo = "";

            if (rsActividad.Rows.Count > 0)
            {
                #region Agregar nueva fila con espacio separador "igual como lo deja FONADE clásico.

                //Inicializar la fila.
                fila = new TableRow();
                TableCell celdaEspacio = new TableCell();
                celdaEspacio.Text = "&nbsp;";
                fila.Cells.Add(celdaEspacio);
                t_anexos.Rows.Add(fila);

                #endregion


                //TableRow fila1 = new TableRow();
                //fila.Attributes.Add("align", "left");
                //fila.Attributes.Add("valign", "top");
                //Mientras que la consulta del personal calificado - Cargo - NO esté vacío.
                foreach (DataRow row in rsActividad.Rows)
                {

                    //Asinar nombre del cargo a variable.
                    nomCargo = row["NomActividad"].ToString();

                    if (CodCargo != row["CodActividad"].ToString())
                    {
                        #region Asigno la primera fila.
                        //Asignar valor de la fila a variable.
                        CodCargo = row["CodActividad"].ToString();
                    }
                    //Inicializar la nueva fila.

                    fila = new TableRow();
                    fila.Attributes.Add("align", "left");
                    fila.Attributes.Add("valign", "top");

                    if (row["Mes"].ToString() != null)
                    {
                        #region Crear la primera fila que contiene valores numéricos.
                        /*for (int j = 1; j < Constantes.CONST_Meses + prorrogaTotal; j++) //Se coloca -1 para que descuente una celda.
                                    {*/
                        if (row != null)
                        {
                            if (row["Mes"].ToString() != null)
                            {
                                #region Formatear el valor y agregarlo a la fila.
                                //Formatear el valor.
                                decimal valor = 0;
                                string valor_formateado = "";

                                valor = decimal.Parse(row["Valor"].ToString());
                                valor_formateado = "$" + valor.ToString("0,0.00", CultureInfo.InvariantCulture);



                                //Agregar celda con valor formateado.
                                TableCell celdaValor = new TableCell();
                                celdaValor.Text = valor_formateado;
                                celdaValor.Attributes.Add("align", "right");
                                fila.Cells.Add(celdaValor);
                                t_anexos.Rows.Add(fila);

                                #region NO BORRAR Código comentado, se reemplaza por funcionalidad multiplicada.
                                //switch (row["Tipo1"].ToString())
                                //{
                                //    case "1":
                                //        //TotalFE = TotalFE + Double.Parse(row["Valor"].ToString());
                                //        TotalFE = int.Parse(row["Mes"].ToString()) * double.Parse(row["Valor"].ToString());
                                //        break;
                                //    case "2":
                                //        TotalEmp = TotalEmp + Double.Parse(row["Valor"].ToString());
                                //        break;
                                //    default:
                                //        break;
                                //} 
                                #endregion

                                #region Generar celda vacía.
                                TableCell celdaVacia = new TableCell();
                                celdaVacia.Text = "&nbsp;";
                                celdaValor.Attributes.Add("align", "right");
                                fila.Cells.Add(celdaVacia);
                                t_anexos.Rows.Add(fila);
                                #endregion
                                #endregion
                            }
                            else
                            {
                                #region Generar celda vacía.
                                //TableCell celdaVacia = new TableCell();
                                //celdaVacia.Text = "&nbsp;";
                                //fila.Cells.Add(celdaVacia);
                                t_anexos.Rows.Add(fila);
                                #endregion
                            }

                        }
                        #region Costo total y cerrar fila.
                        //Formatear el valor de Fondo Emprender.
                        decimal valor_FE = 0;
                        string valor_formateado_FE = "";
                        DataRow lastRow = rsActividad.Rows[rsActividad.Rows.Count - 1];
                        #region Leer comentarios.
                        ////Porqué se tendrá que restar dos números???, si algo, toca cambiarlo por la consulta ya creada.
                        //CAMBIADO; se consulta aquí http://stackoverflow.com/questions/18528736/how-to-retrieve-values-from-the-last-row-in-a-datatable
                        //y se realiza el siguiente código:
                        //TotalFE = TotalFE - Double.Parse(rsCargo.Rows[0]["Valor"].ToString()) - Double.Parse(rsCargo.Rows[0]["Valor"].ToString()); 
                        #endregion
                        //Obtener el Total Emprendedor.
                        double multiplicar = Double.Parse(lastRow["Mes"].ToString()) * Double.Parse(lastRow["Valor"].ToString());
                        TotalFE = multiplicar;
                        valor_FE = decimal.Parse(TotalFE.ToString());
                        valor_formateado_FE = "$" + valor_FE.ToString("0,0.00", CultureInfo.InvariantCulture);

                        //Formatear el valor de Emprendimiento.
                        decimal valor_Emp = 0;
                        string valor_formateado_Emp = "";
                        valor_Emp = decimal.Parse(TotalEmp.ToString());
                        valor_formateado_Emp = "$" + valor_Emp.ToString("0,0.00", CultureInfo.InvariantCulture);


                        TableCell celdaCostoTotal_FE = new TableCell();
                        celdaCostoTotal_FE.Text = valor_formateado_FE;
                        celdaCostoTotal_FE.Attributes.Add("align", "right");
                        fila.Cells.Add(celdaCostoTotal_FE);
                        t_anexos.Rows.Add(fila);

                        //Emprendimiento.
                        TableCell celdaCostoTotal_Emp = new TableCell();
                        celdaCostoTotal_Emp.Text = valor_formateado_Emp;
                        celdaCostoTotal_Emp.Attributes.Add("align", "right");
                        fila.Cells.Add(celdaCostoTotal_Emp);
                        t_anexos.Rows.Add(fila);
                        #endregion

                        #endregion

                    }
                    else
                    {
                        #region prorrogaTotal * Constantes.CONST_Fuentes
                        //for (int j = 0; j < prorrogaTotal * Constantes.CONST_Fuentes; j++)
                        //{
                        //    #region Generar celda vacía.
                        //    TableCell celdaVacia = new TableCell();
                        //    celdaVacia.Attributes.Add("align", "right");
                        //    celdaVacia.Text = "&nbsp;";
                        //    fila.Cells.Add(celdaVacia);
                        //t_anexos.Rows.Add(fila);
                        //    #endregion
                        //}

                        #region Costo total y cerrar fila.
                        //Formatear el valor de Fondo Emprender.
                        decimal valor_FE = 0;
                        string valor_formateado_FE = "";
                        valor_FE = decimal.Parse(TotalFE.ToString());
                        valor_formateado_FE = "$" + valor_FE.ToString("0,0.00", CultureInfo.InvariantCulture);

                        //Formatear el valor de Emprendimiento.
                        decimal valor_Emp = 0;
                        string valor_formateado_Emp = "";
                        valor_Emp = decimal.Parse(TotalEmp.ToString());
                        valor_formateado_Emp = "$" + valor_Emp.ToString("0,0.00", CultureInfo.InvariantCulture);

                        //Fondo Emprender.
                        TableCell celdaCostoTotal_FE = new TableCell();
                        celdaCostoTotal_FE.Attributes.Add("align", "right");
                        celdaCostoTotal_FE.Text = valor_formateado_FE;
                        fila.Cells.Add(celdaCostoTotal_FE);
                        t_anexos.Rows.Add(fila);

                        //Emprendimiento.
                        TableCell celdaCostoTotal_Emp = new TableCell();
                        celdaCostoTotal_Emp.Text = valor_formateado_Emp;
                        celdaCostoTotal_Emp.Attributes.Add("align", "right");
                        fila.Cells.Add(celdaCostoTotal_Emp);
                        t_anexos.Rows.Add(fila);
                        #endregion
                        #endregion
                    }
                    #endregion

                    #region Asigno la segunda fila.

                    //Avances reportados.

                    //Inicializar la nueva fila.
                    //fila = new TableRow();
                    fila.Attributes.Add("bgcolor", "#EDF1F8");

                    //Inicializar variables internas.
                    double TotalTipo1 = 0;
                    double TotalTipo2 = 0;
                    double d_Tipo1 = 0;
                    double d_Tipo2 = 0;
                    string Tipo1 = "";
                    string Tipo2 = "";

                    for (int j = 1; j < conteo_celdas - 1; j++) //Se coloca -1 para que descuente una celda.
                    {
                        #region Tipo 1.
                        //Consulta de tipo 1.
                        txtSQL = " select * " +
                                 " from AvanceActividadPOMes " +
                                 " where codactividad = " + Cod_Actividad +
                                 " and Mes = " + j + " and codtipofinanciacion = 1 ";


                        //Asignar resultados de la consulta de tipo 1.
                        rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");

                        //Si la consulta tiene datos.
                        if (rsTipo1.Rows.Count > 0)
                        {
                            d_Tipo1 = Double.Parse(rsTipo1.Rows[0]["Valor"].ToString());
                            Tipo1 = "$" + d_Tipo1.ToString("0,0.00", CultureInfo.InvariantCulture);
                            TotalTipo1 = TotalTipo1 + d_Tipo1;
                        }
                        else
                        {
                            d_Tipo1 = 0;
                            Tipo1 = "&nbsp;";
                        }

                        //Generar la celda.
                        TableCell celdaInterna_Tipo1 = new TableCell();
                        celdaInterna_Tipo1.Attributes.Add("align", "right");
                        celdaInterna_Tipo1.Text = "<font color=\"#CC0000\">" + Tipo1 + "</font>";
                        fila.Cells.Add(celdaInterna_Tipo1);

                        #endregion

                        #region Tipo 2.
                        //Consulta de Tipo 2.
                        txtSQL = " select * " +
                                 " from AvanceActividadPOMes " +
                                 " where codactividad = " + Cod_Actividad +
                                 " and Mes = " + j + " and codtipofinanciacion = 2 ";

                        //Asignar resultados de la consulta de Tipo 2.
                        rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");

                        //Si la consulta tiene datos.
                        if (rsTipo2.Rows.Count > 0)
                        {
                            d_Tipo2 = Double.Parse(rsTipo2.Rows[0]["Valor"].ToString());
                            Tipo2 = "$" + d_Tipo2.ToString("0,0.00", CultureInfo.InvariantCulture);
                            TotalTipo2 = TotalTipo2 + d_Tipo2;
                        }
                        else
                        {
                            d_Tipo2 = 0;
                            Tipo2 = "&nbsp;";
                        }

                        //Generar la celda.
                        TableCell celdaInterna_Tipo2 = new TableCell();
                        celdaInterna_Tipo2.Attributes.Add("align", "right");
                        celdaInterna_Tipo2.Text = "<font color=\"#CC0000\">" + Tipo2 + "</font>";
                        fila.Cells.Add(celdaInterna_Tipo2);

                        #endregion
                    }

                    #region Costo Total de Avances reportados.

                    //Formatear el valor de Avance de Tipo 1.
                    decimal valor_Tipo1 = 0;
                    string valor_formateado_Tipo1 = "";
                    //TotalTipo1 = TotalTipo1 - Double.Parse(rsTipo1.Rows[0]["Valor"].ToString());
                    valor_Tipo1 = decimal.Parse(TotalTipo1.ToString());
                    valor_formateado_Tipo1 = "$" + valor_Tipo1.ToString("0,0.00", CultureInfo.InvariantCulture);

                    //Formatear el valor de Avance de Tipo 2.
                    decimal valor_Tipo2 = 0;
                    string valor_formateado_Tipo2 = "";
                    valor_Tipo2 = decimal.Parse(TotalTipo2.ToString());
                    valor_formateado_Tipo2 = "$" + valor_Tipo2.ToString("0,0.00", CultureInfo.InvariantCulture);

                    //Tipo 1.
                    TableCell celdaCostoTotal_Tipo1 = new TableCell();
                    celdaCostoTotal_Tipo1.Attributes.Add("align", "right");
                    celdaCostoTotal_Tipo1.Text = "<font color=\"#CC0000\">" + valor_formateado_Tipo1 + "</font>";
                    fila.Cells.Add(celdaCostoTotal_Tipo1);
                    t_anexos.Rows.Add(fila);

                    //Tipo 2.
                    TableCell celdaCostoTotal_Tipo2 = new TableCell();
                    celdaCostoTotal_Tipo2.Attributes.Add("align", "right");
                    celdaCostoTotal_Tipo2.Text = "<font color=\"#CC0000\">" + valor_formateado_Tipo2 + "</font>";
                    fila.Cells.Add(celdaCostoTotal_Tipo2);
                    t_anexos.Rows.Add(fila);
                    #endregion

                    //Añadir la fila a la tabla.
                    t_anexos.Rows.Add(fila);

                    #endregion

                    #region Asigno la tercera fila.

                    //Reportar avances.

                    //Inicializar la nueva fila.
                    //fila = new TableRow();
                    fila.Attributes.Add("bgcolor", "#EDF1F8");

                    int mes = 0;

                    for (int j = 1; j < conteo_celdas - 1; j++) //Se coloca -1 para que descuente una celda.
                    {
                        #region Consultar AvanceCargoPOMes
                        mes = j;

                        txtSQL = " SELECT * " +
                                 " FROM AvanceActividadPOMes " +
                                 " WHERE codactividad=" + Cod_Actividad +
                                 " AND mes=" + j;

                        rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");
                        #endregion

                        #region Establecer si se puede o no ejecutar.
                        if (rsTipo2.Rows.Count > 0)
                        {
                            ejecutar = 1; //Si existe se coloca la opción de editar y borrar.
                        }
                        else
                        {
                            ejecutar = 2; //Si NO existe se coloca la opción de adicionar.
                        }
                        #endregion

                        //Agregar celda.
                        TableCell celdaCentrada = new TableCell();
                        celdaCentrada.Attributes.Add("colspan", "2");
                        celdaCentrada.Attributes.Add("align", "center");

                        try { nomCargo = nomCargo.Replace("+", "$"); }
                        catch { }

                        if (ejecutar == 1)
                        {
                            #region Condición de "ObservacionesInterventor". Aquí es donde se agrega los botones de "Ver Avance".
                            if (!String.IsNullOrEmpty(rsTipo2.Rows[0]["ObservacionesInterventor"].ToString()))
                            {
                                #region Reportar avance.//Ver avance.
                                ImageButton img_VerAvance = new ImageButton();
                                LinkButton lnk_VerAvance = new LinkButton();

                                //ImageButton.
                                img_VerAvance.ID = "img_VerAvance_" + j.ToString();
                                img_VerAvance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                img_VerAvance.AlternateText = "Avance";
                                img_VerAvance.CommandName = "VerAvance";
                                img_VerAvance.CommandArgument = "Editar" + ";" + Cod_Proyecto + ";" + Cod_Actividad + ";" + j + ";" + nomCargo;
                                //img_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                { img_VerAvance = null; }
                                else
                                { celdaCentrada.Controls.Add(img_VerAvance); }

                                //LinkButton.
                                lnk_VerAvance.ID = "lnk_VerAvance_" + j.ToString();
                                lnk_VerAvance.Text = "<b>&nbsp;Ver Avance</b>";
                                lnk_VerAvance.Style.Add("text-decoration", "none");
                                lnk_VerAvance.CommandName = "VerAvance";
                                lnk_VerAvance.CommandArgument = "Editar" + ";" + Cod_Proyecto + ";" + Cod_Actividad + ";" + j + ";" + nomCargo;
                                //lnk_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                { lnk_VerAvance = null; }
                                else
                                { celdaCentrada.Controls.Add(lnk_VerAvance); }
                                #endregion
                            }

                            #endregion
                        }
                        if (celdaCentrada.Controls.Count == 0) { celdaCentrada.Text = "&nbsp;"; }
                        fila.Cells.Add(celdaCentrada);
                        t_anexos.Rows.Add(fila);
                        celdaCentrada = null;
                    }

                    //Al terminar el for...
                    //Agregar celda con espacio.
                    TableCell celda_Espacio = new TableCell();
                    celda_Espacio.Attributes.Add("colspan", "2");
                    celda_Espacio.Text = "&nbsp;";
                    fila.Cells.Add(celda_Espacio);
                    t_anexos.Rows.Add(fila);
                    celda_Espacio = null;

                    //Final.

                    #endregion
                }
            }
        }
        //}

        //            //Bindear finalmente la grilla.
        //            t_anexos.DataBind();
        //    } 
        //}

        public class RespuestaProyectoactividadPO
        {
            public int IdActividad { get; set; }
            public string NombreActividad { get; set; }
            public int CodProyecto { get; set; }
            public int Item { get; set; }
            public string Metas { get; set; }
        }

        public class RespuestaProyectoactividadPOMes
        {
            public int CodTipoFinanciacion { get; set; }
            public int Mes { get; set; }
            public decimal Valor { get; set; }
        }

        protected void gw_Anexos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                //{
                //    ((ImageButton)e.Row.Cells[0].FindControl("btn_Borrar")).Visible = false;
                //}

                //Diego Quiñonez - 29 de Diciembre de 2014
                if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado))
                {
                    ((ImageButton)e.Row.Cells[0].FindControl("btn_Borrar")).Visible = false;
                    ((LinkButton)e.Row.Cells[0].FindControl("btnEditar")).Enabled = false;
                }
            }
        }

        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {
            int flag = 0;
            prActualizarTab(txtTab.ToString(), CodigoProyecto.ToString());
            flag = Marcar(Constantes.CONST_PlanOperativoV2Hijo.ToString(), CodigoProyecto.ToString(), codConvocatoria, chk_realizado.Checked);
            ObtenerDatosUltimaActualizacion();

            if (flag == 1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}