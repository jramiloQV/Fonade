using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Clases;
using System.Globalization;

namespace Fonade.FONADE.interventoria
{
    public partial class PagosActividad : Negocio.Base_Page //System.Web.UI.Page    
    {
        #region Variables globales.
        String CodProyecto;
        String TipoPago;
        String txtSQL;
        Boolean esFechaFin;
        int prorroga;
        int prorrogaTotal;
        String CodPagoActividad;
        String CodEstado;

        public bool usrVldn { get { return usuario.CodGrupo != 14; }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
             CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
             TipoPago = HttpContext.Current.Session["TipoPago"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["TipoPago"].ToString()) ? HttpContext.Current.Session["TipoPago"].ToString() : "0";
             CodEstado = HttpContext.Current.Session["CodEstado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEstado"].ToString()) ? HttpContext.Current.Session["CodEstado"].ToString() : "0";
             //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); 

            if (!IsPostBack)
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                    //|| usuario.CodGrupo == Constantes.CONST_Interventor
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                {
                    lnkBtn_Add_PagosActividadSinConvenio.Visible = false;
                    imgBtn_Add_PagosActividadSinConvenio.Visible = false;

                    txtObservaInterventor.Enabled = false;
                    ddlAprobado.Enabled = false;
                    btn_accion.Visible = false;
                    btnEnviar.Visible = false;
                }

                if(usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    imgBtn_addDocumentoPago.Visible = false;
                    txtObservaInterventor.Enabled = false;
                    ddlAprobado.Enabled = false;
                    btn_accion.Visible = false;
                    btnEnviar.Visible = false;
                }

                CargarGrids();
                #region Obtener el valor de la prórroga para sumarla a la constante de meses generar la tabla.
                prorroga = 0;
                prorroga = ObtenerProrroga(CodProyecto.ToString());
                prorrogaTotal = prorroga + Constantes.CONST_Meses;
                #endregion

                #region Establecer el título y demás datos correspondientes.

                if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoActividad)
                {
                    lbl_titutlo.Text = "PAGOS POR ACTIVIDAD";
                    lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago por Actividad";
                }
                else if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoNomina)
                {
                    lbl_titutlo.Text = "PAGOS DE NOMINA";
                    lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago de Nómina";
                }

                #endregion
            }

            #region inhabilitar opcionesnes para el  interventor            

            if (usuario.CodGrupo == Constantes.CONST_Interventor 
                || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
            {
                ddl_Tipo.Enabled = false;
                ddl_NumSolicitudRechazada.Enabled = false;
                ddl_meses.Enabled = false;
                ddl_actividad_cargo.Enabled = false;
                ddl_Concepto.Enabled = false;
                ddl_CodPagoBeneficiario.Enabled = false;
                ddl_CodPagoForma.Enabled = false;
                CantidadDinero.Enabled = false;
                Observaciones.Enabled = false;
                if(usuario.CodGrupo == Constantes.CONST_Interventor)
                btn_accion.Visible = true;
            }
            else
            {
                ddl_Tipo.Enabled = true;
                ddl_NumSolicitudRechazada.Enabled = true;
                ddl_meses.Enabled = true;
                ddl_actividad_cargo.Enabled = true;
                ddl_Concepto.Enabled = true;
                ddl_CodPagoBeneficiario.Enabled = true;
                ddl_CodPagoForma.Enabled = true;
                CantidadDinero.Enabled = true;
                ddlAprobado.Enabled = false;
                txtObservaInterventor.Enabled = false;
            }
            #endregion
            if (Request.Form.Keys.Count > 6){
                //btn_accion.Text = "Adicionar";
                //return;
            }

            CargarPagosActividades();
            ValidarConvenios();


            if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
            {
                imgBtn_addDocumentoPago.Visible = false;
                imgBtnListaDocs.Visible = false;
                if (ddlAprobado.SelectedValue == "0" || ddlAprobado.SelectedValue == "")
                {
                    btnEnviar.Visible = true;
                }
                else
                {
                    btnEnviar.Visible = false;
                    pAviso.Visible = false;
                }

                if (string.IsNullOrEmpty(lbl_tipo_seleccionado.Text))
                {
                    btn_accion.Visible = true;
                    btnEnviar.Enabled = false;
                }
                else
                {
                    btn_accion.Visible = false;
                    btnEnviar.Enabled = true;
                }

            }
            
        }

        private void CargarGrids()
        {
            CargarActividadCargo();
            CargarBeneficiarios();
            CargarConceptos();
            CargarFomaDePago();
            CargarMeses();
            CargarNumSolicitudesRechazadas();
            CargrAprobado();

            if (string.IsNullOrEmpty(lbl_tipo_seleccionado.Text))
            {
                btn_accion.Visible = true;
                btnEnviar.Enabled = false;
            }
            else
            {
                btn_accion.Visible = false;
                btnEnviar.Enabled = true;
            }
        }

        /// <summary>
        /// Se inicia Validación de convenios relacionados con la convocatoria y si estos convenios están vigentes.
        /// </summary>
        private void ValidarConvenios()
        {
            //Inicializar variables.
            DataTable RSAux = new DataTable();
            DateTime FechaFin = new DateTime();


                txtSQL = " SELECT Max(codConvocatoria)as CodConvocatoria FROM ConvocatoriaProyecto WHERE viable=1 and CodProyecto = " + CodProyecto;

                RSAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (RSAux.Rows.Count > 0)
                {
                    txtSQL = " SELECT CodConvenio FROM Convocatoria WHERE Id_Convocatoria = " + RSAux.Rows[0]["CodConvocatoria"].ToString();
                    RSAux = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RSAux.Rows.Count > 0)
                    {
                        if (RSAux.Rows[0]["CodConvenio"].ToString() != "")
                        {
                            #region Si tiene convenio...

                            txtSQL = "SELECT FechaFin FROM Convenio WHERE Id_Convenio = " + RSAux.Rows[0]["CodConvenio"].ToString();
                            RSAux = consultas.ObtenerDataTable(txtSQL, "text");

                            if (RSAux.Rows.Count > 0)
                            {
                                try { FechaFin = DateTime.Parse(RSAux.Rows[0]["FechaFin"].ToString()); }
                                catch { }

                                if (FechaFin >= DateTime.Now)
                                {
                                    //Establecer variable en TRUE;
                                    esFechaFin = true;

                                    if (TipoPago == Constantes.CONST_TipoPagoActividad.ToString())
                                    {
                                        lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago por Actividad";
                                    }
                                    else if (TipoPago == Constantes.CONST_TipoPagoNomina.ToString())
                                    {
                                        lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago de Nómina";
                                    }
                                }
                                else
                                {
                                    //Establecer variable en FALSE;
                                    esFechaFin = false;

                                    if (TipoPago == Constantes.CONST_TipoPagoActividad.ToString())
                                    {
                                        lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago por Actividad";
                                    }
                                    else if (TipoPago == Constantes.CONST_TipoPagoNomina.ToString())
                                    {
                                        lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago de Nómina";
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region Se establece el MISMO valor que como si la "FechaFin" no fuera mayor o igual a la fecha actual.

                            //Establecer variable en FALSE;
                            esFechaFin = false;

                            if (TipoPago == Constantes.CONST_TipoPagoActividad.ToString())
                            {
                                lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago por Actividad";
                            }
                            else if (TipoPago == Constantes.CONST_TipoPagoNomina.ToString())
                            {
                                lnkBtn_Add_PagosActividadSinConvenio.Text = "Adicionar Pago de Nómina";
                            }

                            #endregion
                        }
                    }
                }

        }

        /// <summary>
        /// Cargar los Pagos de actividades.
        /// </summary>
        private void CargarPagosActividades()
        {
            //Inicializar variables.
            DataTable tabla = new DataTable();

            switch(usuario.CodGrupo)
            {
                case 6:
                    txtSQL = " SELECT Id_PagoActividad, NomPagoActividad, Estado FROM PagoActividad WHERE CodProyecto = " + CodProyecto + " AND TipoPago = " + TipoPago + " Order by 1";
                    break;
                case 14:
                    txtSQL = " SELECT Id_PagoActividad, NomPagoActividad, Estado FROM PagoActividad WHERE CodProyecto = " + CodProyecto + " AND TipoPago = " + TipoPago + " AND Estado = " + Constantes.CONST_EstadoInterventor + " Order by 1";
                    break;
                default:
                    txtSQL = " SELECT Id_PagoActividad, NomPagoActividad, Estado FROM PagoActividad WHERE CodProyecto = " + CodProyecto + " AND TipoPago = " + TipoPago + " Order by 1";
                    break;
            }

            
                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                HttpContext.Current.Session["tabla_actividades"] = tabla;

                gv_PagosActividades.DataSource = tabla;
                gv_PagosActividades.DataBind();

                if (string.IsNullOrEmpty(lbl_tipo_seleccionado.Text))
                {
                    btn_accion.Visible = true;
                    btnEnviar.Enabled = false;
                }
                else
                {
                    btn_accion.Visible = false;
                    btnEnviar.Enabled = true;
                }
            
            }

        /// <summary>
        /// Adicionar pago.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>mo
        protected void imgBtn_Add_PagosActividadSinConvenio_Click(object sender, ImageClickEventArgs e)
        {
            ValidarConvenios();
            if (esFechaFin)
            {
                LimpiarCampos(); //Redirect(null, "PagosActividad.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            else { Redirect(null, "PagosActividadSinConvenio.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100"); }
        }

        /// <summary>
        /// Adicionar pago.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBtn_Add_PagosActividadSinConvenio_Click(object sender, EventArgs e)
        {
            CargarGrids();
            ValidarConvenios();
            if (esFechaFin)
            {
                pnl_Datos.Visible = true;
                pnl_PagosActividad.Visible = false;
                btn_accion.Text = "Adicionar";
                btn_accion.Visible = true;

                if (string.IsNullOrEmpty(lbl_tipo_seleccionado.Text))
                {
                    btn_accion.Visible = true;
                    btnEnviar.Enabled = false;
                }
                else
                {
                    btn_accion.Visible = false;
                    btnEnviar.Enabled = true;
                }
                //LimpiarCampos(); //Redirect(null, "PagosActividad.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            else { Redirect(null, "PagosActividadSinConvenio.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100"); }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Limpiar los campos "permitir generar datos.".
        /// </summary>
        private void LimpiarCampos()
        {
            lbl_newOrEdit.Text = "NUEVO";
            pnl_PagosActividad.Visible = false;
            pnl_Datos.Visible = true;
            btn_accion.Text = "Adicionar";
            ddl_Tipo.SelectedValue = "0";
            lbl_tipo_seleccionado.Text = "";
            hdf_tipo.Value = "";
            ddl_NumSolicitudRechazada.SelectedValue = "0";
            hdf_numsolicitud.Value = "";
            ddl_meses.SelectedValue = "0";
            lbl_mes_seleccionado.Text = "";
            ddl_actividad_cargo.SelectedValue = "0";
            lbl_loaded_actividad_cargo.Text = "";
            ddl_Concepto.SelectedValue = "0";
            ddl_CodPagoBeneficiario.SelectedValue = "0";
            lblNombreBeneficiario.Text = "";
            ddl_CodPagoForma.SelectedValue = "0";
            lbl_FormaDePago.Text = "";
            Observaciones.Text = "";
            CantidadDinero.Text = "";
            btn_accion.Visible = true;
        }

        /// <summary>
        /// Se valida que si el pago ha estado en un acta, no se pueda borrar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_PagosActividades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                    var lbl_id = e.Row.FindControl("lbl_id") as Label;
                    var lnk_elim = e.Row.FindControl("lnk_eliminar") as LinkButton;
                    var imgEditar = e.Row.FindControl("imgeditar") as Image;
                    var lbl_Est = e.Row.FindControl("lbl_Estado") as Label;
                    var lnk_estado = e.Row.FindControl("lnk_nombre") as LinkButton;

                    if (lbl_id != null && imgEditar != null && lbl_Est != null)
                    {
                        //Establecer el valor a mostar en el Label "Estado".
                        lbl_Est.Text = EstadoPago(Int32.Parse(lbl_Est.Text));

                        txtSQL = " SELECT Aprobado FROM PagosActaSolicitudPagos WHERE CodPagoActividad = " + lbl_id.Text;
                        var rsPagoActa = consultas.ObtenerDataTable(txtSQL, "text");

                        if (rsPagoActa.Rows.Count == 0)
                        {
                            if (lnk_elim != null)
                            { lnk_elim.Visible = true; }
                            if (imgEditar != null)
                            { imgEditar.Visible = true; }
                            if(usuario.CodGrupo == Constantes.CONST_Interventor
                            || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                            || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                            || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                            {
                                lnk_elim.Visible = false;
                            }

                        }

                    }

                    if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        if(lbl_Est.Text == "Edición")
                        {
                            lnk_elim.Visible = true;
                        }
                        else
                        {
                            lnk_elim.Visible = false;
                        }
                    }
                }
            }

        /// <summary>
        /// Paginación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_PagosActividades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["tabla_actividades"] as DataTable;

            if (dt != null)
            {
                gv_PagosActividades.PageIndex = e.NewPageIndex;
                gv_PagosActividades.DataSource = dt;
                gv_PagosActividades.DataBind();
            }
        }

        /// <summary>
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_PagosActividades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"msg",string.Format("alert({0})",e.CommandArgument),true);
            if (e.CommandName == "eliminar")
            {
                var idPago = e.CommandArgument.ToString();
                var txtSQL = "DELETE FROM PagoActividadArchivo WHERE CodPagoActividad = " + idPago;
                ejecutaReader(txtSQL, 2);

                txtSQL = "DELETE FROM PagoActividad WHERE Estado = 0 and FechaRtaFA is null and Id_PagoActividad = " + idPago;
                ejecutaReader(txtSQL, 2);

                CargarPagosActividades();

            }
            if (e.CommandName == "editar")
            {
                btn_accion.Text = "Actualizar";
                Session["boton"] = "Actualizar";
                btnEnviar.Visible = false;
                pAviso.Visible = false;
                CargarGrids();
                string[] palabras = e.CommandArgument.ToString().Split(';');
                CodPagoActividad = palabras[0];
                Session["paramDoc"] = palabras;
                //Session["CodPagoActividad"] = CodPagoActividad;
                hdCodigoPago.Value = CodPagoActividad;

                Session["NomActividad"] = palabras[1];
                CargarDatosPagoSeleccionado(hdCodigoPago.Value);
                pnl_PagosActividad.Visible = false;
                pnl_Datos.Visible = true;

                if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    if (palabras[2] == Constantes.CONST_EstadoEdicion.ToString())
                    {
                        btn_accion.Visible = true;
                        btnEnviar.Visible = true;
                    }
                    else
                    {
                        btn_accion.Visible = false;
                        btnEnviar.Visible = false;
                        Session["boton"] = null;
                    }

                    if(palabras[2] == Constantes.CONST_EstadoInterventor.ToString())
                    {
                        btnEnviar.Visible = false;
                    }
                }
                
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Cargar las solicitudes rechazadas.
        /// </summary>
        private void CargarNumSolicitudesRechazadas()
        {
            //Inicializar variables.
            DataTable rs = new DataTable();
            ListItem item = new ListItem();

            try
            {}catch { }
                txtSQL = " SELECT Id_PagoActividad FROM PagoActividad WHERE CodProyecto = " + CodProyecto + " AND Estado = " + Constantes.CONST_EstadoRechazadoFA;
                rs = consultas.ObtenerDataTable(txtSQL, "text");

                ddl_NumSolicitudRechazada.Items.Clear();

                //Valor por defecto.
                item = new ListItem();
                item.Text = "Seleccione una solicitud";
                item.Value = "0";
                ddl_NumSolicitudRechazada.Items.Add(item);

                foreach (DataRow row in rs.Rows)
                {
                    item = new ListItem();
                    item.Text = row["Id_PagoActividad"].ToString();
                    item.Value = row["Id_PagoActividad"].ToString();
                    ddl_NumSolicitudRechazada.Items.Add(item);
                }
            
            }

        private void CargrAprobado()
        {
            ddlAprobado.Items.Clear();
            ddlAprobado.Items.Insert(0, new ListItem("No", "0"));
            ddlAprobado.Items.Insert(1, new ListItem("Si", "1"));
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Cargar los meses "con su prórroga ya obtenida".
        /// </summary>
        private void CargarMeses()
        {
            //Inicializar variables.
            DataTable rs = new DataTable();
            ListItem item = new ListItem();

            try
            {}catch { }
                txtSQL = " SELECT Id_PagoActividad FROM PagoActividad WHERE CodProyecto = " + CodProyecto + " AND Estado = " + Constantes.CONST_EstadoRechazadoFA;
                rs = consultas.ObtenerDataTable(txtSQL, "text");

                ddl_meses.Items.Clear();

                //Valor por defecto.
                item = new ListItem();
                item.Text = "Seleccione";
                item.Value = "0";
                ddl_meses.Items.Add(item);

                for (int i = 1; i < prorrogaTotal + 1; i++)
                {
                    item = new ListItem();
                    item.Text = i.ToString();
                    item.Value = i.ToString();
                    ddl_meses.Items.Add(item);
                }
            
            }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Dependiendo de la validación interna, se carga la "Actividad" o "Cargo" correspondiente.
        /// </summary>
        private void CargarActividadCargo()
        {
            //Inicializar variables.
            DataTable rsActividad = new DataTable();
            ListItem item = new ListItem();

            #region Obtener el valor de la prórroga para sumarla a la constante de meses generar la tabla.
            prorroga = 0;
            prorroga = ObtenerProrroga(CodProyecto.ToString());
            prorrogaTotal = prorroga + Constantes.CONST_Meses;
            #endregion

            if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoActividad)
            {
                    consultas.Parameters = new[]{
                                                    new SqlParameter
                                                       {
                                                           ParameterName = "@codProyecto",
                                                           Value = CodProyecto
                                                       }
                                               };
                    DataSet query = consultas.ObtenerDataSet("MD_ObtenerItems_Interventor");

                    lbl_Actividad_Cargo.Text = "Actividad";
                    ddl_actividad_cargo.Items.Clear();

                    if (query.Tables[0].Rows.Count != 0){
                        ddl_actividad_cargo.DataSource = query.Tables[0];
                        ddl_actividad_cargo.DataValueField = "id_Actividad";
                        ddl_actividad_cargo.DataTextField = "Nomactividad";
                        ddl_actividad_cargo.DataBind();
                        ddl_actividad_cargo.Items.Insert(0, new ListItem("Seleccione", "0"));
                    }                    
                }
                else if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoNomina)
                {
                    txtSQL = " SELECT Id_Nomina AS Id_Actividad, Cargo AS NomActividad " +
                             " FROM InterventorNomina WHERE CodProyecto = " + CodProyecto +
                             " ORDER BY Id_Nomina ";
                    lbl_Actividad_Cargo.Text = "Cargo";
                    ddl_actividad_cargo.Items.Clear();

                    rsActividad = consultas.ObtenerDataTable(txtSQL, "text");
                    ddl_actividad_cargo.DataSource = rsActividad;
                    ddl_actividad_cargo.DataValueField = "Id_Actividad";
                    ddl_actividad_cargo.DataTextField = "NomActividad";
                    ddl_actividad_cargo.DataBind();
                    ddl_actividad_cargo.Items.Insert(0, new ListItem("Seleccione", "0"));
                }                
            }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Cargar los conceptos.
        /// </summary>
        private void CargarConceptos()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            ListItem item = new ListItem();

            try
            {}catch { }
                txtSQL = " SELECT * FROM PagoConcepto order by nompagoconcepto ";

                ddl_Concepto.Items.Clear();

                //Valor por defecto.
                //item = new ListItem();
                //item.Text = "Seleccione el concepto";
                //item.Value = "0";
                //ddl_Concepto.Items.Add(item);

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                ddl_Concepto.DataSource = RS;
                ddl_Concepto.DataTextField = "NomPagoConcepto";
                ddl_Concepto.DataValueField = "Id_PagoConcepto";
                ddl_Concepto.DataBind();
                ddl_Concepto.Items.Insert(0, new ListItem("Seleccione el concepto", "0"));

                //foreach (DataRow row in RS.Rows)
                //{
                //    item = new ListItem();
                //    item.Text = row["NomPagoConcepto"].ToString();
                //    item.Value = row["Id_PagoConcepto"].ToString();
                //    ddl_Concepto.Items.Add(item);
                //}
            
            }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Cargar los beneficiarios.
        /// </summary>
        private void CargarBeneficiarios()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            ListItem item = new ListItem();

            try
            {}catch { }
                txtSQL = " SELECT Id_PagoBeneficiario, razonsocial, Nombre + ' '+ Apellido Nombres FROM PagoBeneficiario WHERE CodEmpresa in (SELECT id_empresa FROM empresa WHERE CodProyecto = " + CodProyecto + ") ORDER BY razonsocial asc ";

                ddl_CodPagoBeneficiario.Items.Clear();

                //Valor por defecto.
                //item = new ListItem();
                //item.Text = "Seleccione el nombre del beneficiario";
                //item.Value = "0";
                //ddl_CodPagoBeneficiario.Items.Add(item);

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                ddl_CodPagoBeneficiario.DataSource = RS;
                ddl_CodPagoBeneficiario.DataTextField = "Nombres";
                ddl_CodPagoBeneficiario.DataValueField = "Id_PagoBeneficiario";
                ddl_CodPagoBeneficiario.DataBind();
                ddl_CodPagoBeneficiario.Items.Insert(0, new ListItem("Seleccione el nombre del beneficiario", "0"));

                //foreach (DataRow row in RS.Rows)
                //{
                //    item = new ListItem();
                //    item.Text = row["Nombre"].ToString() + " " + row["Apellido"].ToString() + " " + row["razonsocial"].ToString();
                //    item.Value = row["Id_PagoBeneficiario"].ToString();
                //    ddl_CodPagoBeneficiario.Items.Add(item);
                //}
            
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Cargar las formas de pago.
        /// </summary>
        private void CargarFomaDePago()
        {
            
            var txtSQL = "SELECT * FROM PagoForma ";
            var dt = consultas.ObtenerDataTable(txtSQL, "text");
            ddl_CodPagoForma.DataSource = dt;
            ddl_CodPagoForma.DataValueField = "Id_PagoForma";
            ddl_CodPagoForma.DataTextField = "NomPagoForma";
            ddl_CodPagoForma.DataBind();
            ddl_CodPagoForma.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Validar la información antes de crearla/editarla.
        /// </summary>
        /// <returns>string vacío = puede continuar. // string con datos = campo requerido u ocurrió un error.</returns>
        private string Validar()
        {
            //Inicializar variables.
            String mensaje = "";

            try
            {}catch { mensaje = "Error inesperado"; return mensaje; }
                //Según el clásico, se bloquean los botones...
                btn_accion.Enabled = false;
                btnEnviar.Enabled = false;

                if (ddl_Tipo.SelectedValue == "0") { mensaje = "Debe seleccionar el tipo"; }

                if (ddl_Tipo.SelectedValue == "2" && ddl_NumSolicitudRechazada.SelectedValue == "0")
                { mensaje = "Debe ingresar el número de la solicitud de orden que se va a reemplazar con la nueva"; }

                if (ddl_meses.SelectedValue == "0")
                { mensaje = "Debe ingresar un mes"; }

                if (ddl_actividad_cargo.SelectedValue == "0")
                { mensaje = "Debe ingresar la actividad"; }

                if (ddl_Concepto.SelectedValue == "0")
                { mensaje = "Debe seleccionar el concepto"; }

                if (ddl_CodPagoBeneficiario.SelectedValue == "0")
                { mensaje = "Debe seleccionar el beneficiario"; }

                if (ddl_CodPagoForma.SelectedValue == "0")
                { mensaje = "Debe seleccionar la forma de pago"; }

                if (Observaciones.Text.Trim() == "")
                { mensaje = "Debe ingresar las observaciones"; }

               
            if(!string.IsNullOrEmpty(CantidadDinero.Text))
            {
                var valor = decimal.Parse(CantidadDinero.Text.Trim().Replace(',','.'));
                if (CantidadDinero.Text.Trim() == "")
                { mensaje = "Debe ingresar la cantidad de dinero solicitado al fondo emprender"; }
                try
                { }
                catch { mensaje = "Debe ingresar la cantidad de dinero solicitado al fondo emprender"; }
            }
                    
                return mensaje;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Cargar la información del pago seleccionado.
        /// </summary>
        /// <param name="CodPagoActividad">Pago seleccionado.</param>
        private void CargarDatosPagoSeleccionado(String CodPagoActividad)
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            Int32 numArchivos = 0;
            DataTable RsPagoActividad = new DataTable();
            //CargarGrids();

                //Establecer texto del botón
                btn_accion.Text = "Actualizar";
                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    btnEnviar.Visible = true;
                    btnEnviar.Enabled = true;
                }

                txtSQL = " SELECT COUNT(Id_PagoActividadArchivo) AS Cuantos FROM PagoActividadarchivo " +
                         " WHERE CodPagoActividad = " + CodPagoActividad;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                numArchivos = 0;

                try { if (RS.Rows.Count > 0) { numArchivos = Int32.Parse(RS.Rows[0]["Cuantos"].ToString()); } }
                catch { numArchivos = 0; }

                txtSQL = "SELECT * FROM PagoActividad WHERE Id_PagoActividad = " + CodPagoActividad;
                RsPagoActividad = consultas.ObtenerDataTable(txtSQL, "text");

                if (RsPagoActividad.Rows.Count > 0)
                {
                    lbl_newOrEdit.Text = "EDITAR";
                    Session["IdEstadoPago"] = RsPagoActividad.Rows[0]["Estado"].ToString();
                    //Nuevo control para mostrar Número de la solicitud de orden que se va a reemplazar con la nueva:
                    //NomPagoActividad = RsPagoActividad.Rows[0]["NomPagoActividad"].ToString();
                    ddl_actividad_cargo.Visible = false;
                    lbl_loaded_actividad_cargo.Visible = true;
                    lbl_loaded_actividad_cargo.Text = RsPagoActividad.Rows[0]["NomPagoActividad"].ToString();

                    if (RsPagoActividad.Rows[0]["Tipo"].ToString() == "1") 
                    { 
                        ddl_Tipo.Items[0].Selected = true; 
                    }// = "Nueva"; }
                    else
                    {
                        ddl_Tipo.Items[1].Selected = true;
                    }
                    //if (RsPagoActividad.Rows[0]["Tipo"].ToString() == "2") { ddl_Tipo.Items[1].Selected = true; }// = "Rechazada"; }

                    ddl_meses.Visible = false;
                    lbl_mes_seleccionado.Visible = true;
                    lbl_mes_seleccionado.Text = "Mes " + RsPagoActividad.Rows[0]["Mes"].ToString();
                    ddl_NumSolicitudRechazada.Visible = false;

                    ddl_Tipo.Visible = false;
                    ddl_Tipo.SelectedValue = RsPagoActividad.Rows[0]["Tipo"].ToString();
                    ddl_CodPagoBeneficiario.SelectedValue = RsPagoActividad.Rows[0]["CodPagoBeneficiario"].ToString();
                    lbl_tipo_seleccionado.Visible = true;
                    lbl_tipo_seleccionado.Text = ddl_Tipo.SelectedItem.Text;
                    hdf_tipo.Value = RsPagoActividad.Rows[0]["Tipo"].ToString();
                    txtObservaInterventor.Text = RsPagoActividad.Rows[0]["ObservaInterventor"].ToString();
                    if (RsPagoActividad.Rows[0]["Estado"].ToString() == "0" || RsPagoActividad.Rows[0]["Estado"].ToString() == "5" || RsPagoActividad.Rows[0]["Estado"].ToString() == "1" || RsPagoActividad.Rows[0]["Estado"].ToString() == "4")
                    {                        
                        if (usuario.CodGrupo == Constantes.CONST_Emprendedor  
                                    || RsPagoActividad.Rows[0]["Estado"].ToString() == "0")
                            imgBtn_addDocumentoPago.Visible = true;
                        else
                            imgBtn_addDocumentoPago.Visible = false;

                        imgBtnListaDocs.Visible = true;                        
                    }
                    else
                    {
                        ddlAprobado.SelectedValue = "1";
                        imgBtn_addDocumentoPago.Visible = false;
                    }

                    td_archivosAdjuntos.Visible = true;

                    //Cargar el número de la solicitud (sólo si es diferente de cero.).
                    if (Convert.ToInt32(RsPagoActividad.Rows[0]["CodPagoActividadRechazada"].ToString()) > 0)
                    {
                        lbl_NumSolic.Visible = true;
                        lbl_NumSolic.Text = RsPagoActividad.Rows[0]["Id_PagoActividad"].ToString();
                        hdf_numsolicitud.Value = RsPagoActividad.Rows[0]["CodPagoActividadRechazada"].ToString();
                    }
                    else { lbl_NumSolic.Visible = false; }

                    ddl_Concepto.SelectedValue = RsPagoActividad.Rows[0]["CodPagoConcepto"].ToString();
                    ddl_CodPagoBeneficiario.SelectedValue = RsPagoActividad.Rows[0]["CodPagoBeneficiario"].ToString();
                    ddl_CodPagoForma.SelectedValue = RsPagoActividad.Rows[0]["CodPagoForma"].ToString();
                    Observaciones.Text = RsPagoActividad.Rows[0]["Observaciones"].ToString();
                    CantidadDinero.Text = Convert.ToDecimal(RsPagoActividad.Rows[0]["CantidadDinero"]).ToString("0.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' }).Replace(".00", "");
                    hdf_estado.Value = RsPagoActividad.Rows[0]["Estado"].ToString();

                    if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        ddl_Concepto.Enabled = true;
                        ddl_CodPagoBeneficiario.Enabled = true;
                        ddl_CodPagoForma.Enabled = true;
                        CantidadDinero.Enabled = true;
                    }

                    if (CodEstado == "1") // Constantes.CONST_EstadoEdicion.ToString())
                    {
                        btn_accion.Text = "Actualizar";
                        btn_accion.Visible = true;
                        //btnEnviar.Visible = false;
                    }

                    if(RsPagoActividad.Rows[0]["Estado"].ToString() == "3")
                    {
                        btn_accion.Visible = false;
                        imgBtn_addDocumentoPago.Visible = false;
                    }

                    if (int.Parse(RsPagoActividad.Rows[0]["Estado"].ToString()) < 1)
                    {
                        btnEnviar.Visible = true;
                    }

                    if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        if (int.Parse(RsPagoActividad.Rows[0]["Estado"].ToString()) > 0)
                        {
                            imgBtn_addDocumentoPago.Visible = false;
                            DisableControls(Page, false);
                        }
                    }
                }
                //btnEnviar.Visible = false;
            }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Adicionar pago.
        /// </summary>
        private void Adicionar()
        {
            //Inicializar variables.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable RS = new DataTable();
            Boolean bolUnicaActividad = true;
            DataTable rsCuantasActividades = new DataTable();
            String NombreActividad = "";
            Int32 NumSolicitudRechazada = 0;
            Int32 Consecutivo = 1;

            // Se inhabilitan los objetos que no se permite modificar
            // Ajuste relacionado con el caso ERROR INT-50 
            // Diciembre 2 de 2014 - Alex Flautero
            ddl_Concepto.Enabled = true;
            ddl_CodPagoBeneficiario.Enabled = true;
            ddl_CodPagoForma.Enabled = true;
            CantidadDinero.Enabled = true;


            var ddl_TipoSelectedValue = !string.IsNullOrEmpty(ddl_Tipo.SelectedValue)?ddl_Tipo.SelectedValue : Request.Form.Get("ctl00$bodyContentPlace$ddl_Tipo");
            var ddl_NumSolicitudRechazadaSelectedValue = !string.IsNullOrEmpty(ddl_NumSolicitudRechazada.SelectedValue)?
                ddl_NumSolicitudRechazada.SelectedValue : Request.Form.Get("ctl00$bodyContentPlace$ddl_NumSolicitudRechazada");
            var ddl_mesesSelectedValue = !string.IsNullOrEmpty(ddl_meses.SelectedValue)?
                ddl_meses.SelectedValue : Request.Form.Get("ctl00$bodyContentPlace$ddl_meses");
            var ddl_actividad_cargoSelectedValue = !string.IsNullOrEmpty(ddl_actividad_cargo.SelectedValue)? 
                ddl_actividad_cargo.SelectedValue : Request.Form.Get("ctl00$bodyContentPlace$ddl_actividad_cargo");
            var ddl_ConceptoSelectedValue = !string.IsNullOrEmpty(ddl_Concepto.SelectedValue)? 
                ddl_Concepto.SelectedValue : Request.Form.Get("ctl00$bodyContentPlace$ddl_Concepto");
            var ddl_CodPagoBeneficiarioSelectedValue = !string.IsNullOrEmpty(ddl_CodPagoBeneficiario.SelectedValue)? ddl_CodPagoBeneficiario.SelectedValue : 
                Request.Form.Get("ctl00$bodyContentPlace$ddl_CodPagoBeneficiario");
            var ddl_CodPagoFormaSelectedValue = !string.IsNullOrEmpty(ddl_CodPagoForma.SelectedValue)? 
                ddl_CodPagoForma.SelectedValue: Request.Form.Get("ctl00$bodyContentPlace$ddl_CodPagoForma");
            var CantidadDineroText = !string.IsNullOrEmpty(CantidadDinero.Text)? CantidadDinero.Text : Request.Form.Get("ctl00$bodyContentPlace$CantidadDinero");
            var ObservacionesText = !string.IsNullOrEmpty(Observaciones.Text)? Observaciones.Text : Request.Form.Get("ctl00$bodyContentPlace$Observaciones");

            try
            {}catch { }
                //Se valida que tenga avance sobre el mes y actividad de la solicitud de pago que esta ingresando.
                if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoActividad)
                {
                    //Para plan operativo
                    txtSQL = " SELECT Valor FROM AvanceActividadPOMes WHERE CodActividad = " + ddl_actividad_cargoSelectedValue + " AND Mes = " + ddl_mesesSelectedValue;
                }
                else if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoNomina)
                {
                    //Para Nomina
                    txtSQL = " SELECT Valor FROM AvanceCargoPOMes WHERE CodCargo = " + ddl_actividad_cargoSelectedValue + " AND Mes = " + ddl_mesesSelectedValue;
                }

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    #region Para pagos de tipo Nomina se valida que solo tenga un pago por mes registrado.

                    bolUnicaActividad = true;
                    if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoNomina)
                    {
                        txtSQL = " SELECT COUNT(*) AS Cuantos FROM PagoActividad " +
                                 " WHERE TipoPago = " + Constantes.CONST_TipoPagoNomina +
                                 " AND Tipo = " + ddl_TipoSelectedValue +
                                 " AND Mes = " + ddl_mesesSelectedValue +
                                 " AND CodActividad = " + ddl_actividad_cargoSelectedValue +
                                 " AND codproyecto = " + CodProyecto +
                                 " AND Estado <> " + Constantes.CONST_EstadoRechazadoFA;

                        rsCuantasActividades = consultas.ObtenerDataTable(txtSQL, "text");

                        if (rsCuantasActividades.Rows.Count > 0)
                        {
                            if (Int32.Parse(rsCuantasActividades.Rows[0]["Cuantos"].ToString()) > 0) { bolUnicaActividad = false; }
                        }
                    }

                    #endregion

                    #region  Para pagos de tipo PO se valida que solo tenga un pago por mes registrado.

                    if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoActividad)
                    {
                        txtSQL = " SELECT COUNT(*) AS Cuantos FROM PagoActividad " +
                                 " WHERE TipoPago = " + Constantes.CONST_TipoPagoActividad +
                                 " AND Tipo = " + ddl_TipoSelectedValue +
                                 " AND Mes = " + ddl_mesesSelectedValue +
                                 " AND CodActividad = " + ddl_actividad_cargoSelectedValue +
                                 " AND codproyecto = " + CodProyecto +
                                 " AND Estado <> " + Constantes.CONST_EstadoRechazadoFA;

                        rsCuantasActividades = consultas.ObtenerDataTable(txtSQL, "text");

                        if (rsCuantasActividades.Rows.Count > 0)
                        {
                            if (Int32.Parse(rsCuantasActividades.Rows[0]["Cuantos"].ToString()) > 0) { bolUnicaActividad = false; }
                        }
                    }

                    #endregion

                    if (bolUnicaActividad)
                    {
                        if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoActividad)
                        {
                            txtSQL = " SELECT id_Actividad, NomActividad " +
                                     " FROM proyectoactividadPOInterventor " +
                                     " WHERE codproyecto = " + CodProyecto + " AND Id_actividad = " + ddl_actividad_cargoSelectedValue + " ORDER BY Item ";
                        }
                        else if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoNomina)
                        {
                            txtSQL = " SELECT Id_Nomina AS Id_Actividad, Cargo AS NomActividad " +
                                     " FROM InterventorNomina" +
                                     " WHERE CodProyecto = " + CodProyecto +
                                     " AND Id_Nomina = " + ddl_actividad_cargoSelectedValue;
                        }

                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        if (RS.Rows.Count > 0) { NombreActividad = RS.Rows[0]["NomActividad"].ToString(); }

                        NombreActividad = "Mes " + ddl_mesesSelectedValue + " - " + NombreActividad;

                        NumSolicitudRechazada = 0;
                        if (ddl_NumSolicitudRechazadaSelectedValue != "0") { NumSolicitudRechazada = Int32.Parse(ddl_NumSolicitudRechazadaSelectedValue); }

                        //Se calcula el valor del consecutivo de solicitudes por proyecto
                        Consecutivo = 1;

                        txtSQL = " SELECT Max(Consecutivo) AS Consecutivo FROM PagoActividad " +
                                 " WHERE CodProyecto = " + CodProyecto;
                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        if (RS.Rows.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(RS.Rows[0]["Consecutivo"].ToString())) { Consecutivo = Int32.Parse(RS.Rows[0]["Consecutivo"].ToString()) + 1; }
                            else { Consecutivo = 1; }
                        }

                        #region Finalmente la inserción!.
                        //Validar monto solicitado contra saldo
                        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                        var dt = new DataTable();
                        var command = new SqlCommand("Proc_SaldoProyecto", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idProyecto", CodProyecto);
                        var adp = new SqlDataAdapter(command);
                        adp.Fill(dt);
                        var saldo = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                        conn.Close();
                        var cantidadDineroDeciamal = CantidadDineroText.Trim().Replace(',', '.');
                        if (Decimal.Parse(CantidadDineroText.Trim().Replace('.', ',')) <= saldo)
                        {
                            txtSQL = " INSERT INTO PagoActividad (NomPagoActividad, Tipo, Mes, CodPagoConcepto, CodPagoBeneficiario, CodPagoForma, Observaciones, CantidadDinero, Consecutivo, CodActividad, CodProyecto, CodPagoActividadRechazada, Estado, FechaIngreso, FechaInterventor, FechaCoordinador, FechaRtaFA ,TipoPago, ObservaInterventor, RutaArchivoZIP, ObservacionesFA) " +
                                 " VALUES ('" + NombreActividad + "', " + ddl_TipoSelectedValue + ", " +
                                 " " + ddl_mesesSelectedValue + ", " + ddl_ConceptoSelectedValue + ", " + ddl_CodPagoBeneficiarioSelectedValue + ", " +
                                 " " + ddl_CodPagoFormaSelectedValue + ", '" + ObservacionesText.Trim() + "', Cast('" + cantidadDineroDeciamal + "' as decimal(38,2)) ," +
                                 " " + Consecutivo + ", " + ddl_actividad_cargoSelectedValue + "," + CodProyecto + "," + NumSolicitudRechazada +
                                 ",0,GETDATE(),NULL,NULL,NULL," + TipoPago + ",'','',''); Select @@IDENTITY;";

                            var idPago = int.Parse(consultas.RetornarEscalar(txtSQL, "text").ToString());
                            //ejecutaReader(txtSQL, 2);
                            Session["boton"] = null;
                            Response.Redirect("PagosActividad.aspx");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El valor solicitado es mayor al presupuesto disponible. La solicitud no puede ser enviada!');", true);
                            return;
                        }
                        
                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Las solicitudes de pagos deben ser sólo una por mes !!!');", true);
                        return;
                    }
                }
                else
                {
                    //Se debe mostrar mensaje de advertencia diciendole al emprendedor que no puede solicitar pagos sin hacer avance
                    if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoActividad)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No puede ingresar una solicitud de pago sin haber ingresado un avance sobre la misma actividad y mes en el Plan Operativo');", true);
                        return;
                    }
                    else if (Int32.Parse(TipoPago) == Constantes.CONST_TipoPagoNomina)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No puede ingresar una solicitud de pago sin haber ingresado un avance sobre el mismo cargo y mes en la pestaña de Nómina');", true);
                        return;
                    }
                }
            
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Eliminar...
        /// </summary>
        private void Eliminar(String CodPagoActividad)
        {
            //Inicializar variables.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {}catch { }
                //Se borran los archivos asociados a esa solicitud de pago
                txtSQL = " DELETE FROM PagoActividadArchivo WHERE CodPagoActividad = " + CodPagoActividad;

                try
                {
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                finally {
                    con.Close();
                    con.Dispose();
                }
                //Se borra la solicitud de pago
                txtSQL = " DELETE FROM PagoActividad WHERE Estado = 0 and FechaRtaFA is null and  Id_PagoActividad = " + CodPagoActividad;

                try
                {
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                finally {
                    con.Close();
                    con.Dispose();
                }
            
        }

        /// <summary>
        /// Regresar a la ventana "anterior".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CodProyecto"] = CodProyecto;
            HttpContext.Current.Session["TipoPago"] = TipoPago;
            //Redirect(null, "PagosActividad.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
            Session["boton"] = null;
            Response.Redirect("PagosActividad.aspx");
            pnl_PagosActividad.Visible = true;
            pnl_Datos.Visible = false;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Crear o actualizar pagos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_accion_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String validado = "";

            if (Session["boton"] != null)
            {
                btn_accion.Text = Session["boton"].ToString();

            }

            if(btn_accion.Text !="Actualizar")
            {
                validado = Validar();
            }
            

            if (validado == "")
            {
                if (btn_accion.Text == "Adicionar")
                {
                    if(ValidaCampos())
                    {
                        int codActividad = int.Parse(Request.Form.Get("ctl00$bodyContentPlace$ddl_actividad_cargo"));
                        int codMes = int.Parse(Request.Form.Get("ctl00$bodyContentPlace$ddl_meses"));
                        if (ValidaAvance(codActividad, codMes))
                        {
                            //Validar aprobacion por el interventor
                            if (ValidarAprobacion(codActividad, codMes))
                            {
                                Adicionar();
                            }                            
                        }                        
                    }
                    else
                    {
                        return;
                    }
                    
                }
                else
                {
                    #region Actualizar
                    if (btn_accion.Text == "Actualizar")
                    {
                        if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                        {                                                                                   
                            if (!string.IsNullOrEmpty(hdCodigoPago.Value))
                            {
                                if( Request.Form.Get("ctl00$bodyContentPlace$ddl_CodPagoBeneficiario").ToString() == "0" 
                                    || Request.Form.Get("ctl00$bodyContentPlace$ddl_Concepto").ToString() == "0")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El beneficiario y el concepto son obligatorios.')", true);
                                    return;
                                }

                                var presupuestoDisponible = Negocio.PlanDeNegocioV2.Interventoria.Interventoria.PresupuestoDisponible(int.Parse(Session["CodProyecto"].ToString()), int.Parse(hdCodigoPago.Value));

                                if (Convert.ToDouble(CantidadDinero.Text.Replace('.', ',')) > presupuestoDisponible)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El valor solicitado es mayor al presupuesto disponible. La solicitud no puede ser enviada!');", true);
                                    return;
                                }

                                var cantidadDecimal = CantidadDinero.Text.Replace(',', '.');
                                txtSQL = "UPDATE PagoActividad Set CantidadDinero = Cast('" + cantidadDecimal + "' as decimal(38,2)) , CodPagoBeneficiario = " + Request.Form.Get("ctl00$bodyContentPlace$ddl_CodPagoBeneficiario") + ", codpagoconcepto = " + Request.Form.Get("ctl00$bodyContentPlace$ddl_Concepto");
                                txtSQL += ", Observaciones = '" + Observaciones.Text + "' WHERE Estado not in(4,5) and Id_PagoActividad = " + hdCodigoPago.Value;
                                //txtSQL += ", Observaciones = '" + Observaciones.Text + "' WHERE Id_PagoActividad = " + Session["CodPagoActividad"].ToString();
                                ejecutaReader(txtSQL, 2);

                                var estado = (from pa in consultas.Db.PagoActividad
                                              where pa.Id_PagoActividad == int.Parse(hdCodigoPago.Value)
                                              //where pa.Id_PagoActividad == int.Parse(Session["CodPagoActividad"].ToString())
                                              select pa).FirstOrDefault();

                                //Agendar tarea a Interventor
                                // Seleccional id del Interventor
                                if(estado.Estado != 0)
                                {
                                    var datos = (from ei in consultas.Db.EmpresaInterventors
                                                 join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                                 join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                                 where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                                                 select new datosAgendar
                                                 {
                                                     idContacto = (int)ei.CodContacto,
                                                     idProyecto = (int)p.Id_Proyecto,
                                                     nombre = p.NomProyecto
                                                 }).ToList();
                                    //Agendar tarea.
                                    AgendarTarea agenda = new AgendarTarea
                                    (datos[0].idContacto,
                                    "Revisar Solicitud de Pago",
                                    //"Revisar Solicitud de Pago No. " + Session["CodPagoActividad"].ToString() + "  Observaciones: " + Observaciones.Text.Trim(),
                                    "Revisar Solicitud de Pago No. " + hdCodigoPago.Value + "  Observaciones: " + Observaciones.Text.Trim(),
                                    datos[0].idProyecto.ToString(),
                                    32,
                                    "0",
                                    true,
                                    1,
                                    true,
                                    false,
                                    usuario.IdContacto,
                                    "CodProyecto=" + datos[0].idProyecto,
                                    //"Accion=Editar&CodProyecto=" + datos[0].idProyecto.ToString() + "&CodPagoActividad=" + Session["CodPagoActividad"].ToString() + "&CodEstado=" + Constantes.CONST_EstadoInterventor + "&TipoPago=" + TipoPago,
                                    "Accion=Editar&CodProyecto=" + datos[0].idProyecto.ToString() + "&CodPagoActividad=" + hdCodigoPago.Value + "&CodEstado=" + Constantes.CONST_EstadoInterventor + "&TipoPago=" + TipoPago,
                                    "Pagos Actividad Interventoria");

                                    agenda.Agendar();
                                }
                                Session["boton"] = null;
                                Response.Redirect("PagosActividad.aspx");                                
                            }
                        }

                        if (usuario.CodGrupo == Constantes.CONST_Interventor)
                        {
                            var aprobado = Request.Form.Get("ctl00$bodyContentPlace$ddlAprobado");

                            if (aprobado == "0")
                            {
                                //if (!string.IsNullOrEmpty(Session["CodPagoActividad"].ToString()))
                                if (!string.IsNullOrEmpty(hdCodigoPago.Value))
                                {
                                    txtSQL = "UPDATE PagoActividad Set FechaInterventor=Getdate(), Estado = 0" + Constantes.CONST_EstadoEdicion;
                                    txtSQL += ", ObservaInterventor = '" + txtObservaInterventor.Text + "' WHERE Estado not in(4,5) and Id_PagoActividad = " + hdCodigoPago.Value;
                                    ejecutaReader(txtSQL, 2);

                                    //Agendar tarea a Emprendedor
                                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                                    {
                                        // Seleccional id del Emprendedor
                                        var datos = (from pc in consultas.Db.ProyectoContactos
                                                     join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                                     where pc.CodProyecto == int.Parse(Session["CodProyecto"].ToString()) && pc.CodRol == 3
                                                     select new datosAgendar
                                                     {
                                                         idContacto = pc.CodContacto,
                                                         idProyecto = p.Id_Proyecto,
                                                         nombre = p.NomProyecto
                                                     }).ToList();
                                        //Agendar tarea.
                                        AgendarTarea agenda = new AgendarTarea
                                        (datos[0].idContacto,
                                        "Revisar Solicitud de Pago",
                                        //"Revisar Solicitud de Pago No.. " + Session["CodPagoActividad"].ToString() + "  Observaciones: " + txtObservaInterventor.Text.Trim(),
                                        "Revisar Solicitud de Pago No.. " + hdCodigoPago.Value + "  Observaciones: " + txtObservaInterventor.Text.Trim(),
                                        datos[0].idProyecto.ToString(),
                                        32,
                                        "0",
                                        true,
                                        1,
                                        true,
                                        false,
                                        usuario.IdContacto,
                                        "CodProyecto=" + datos[0].idProyecto,
                                        //"Accion=Editar&CodProyecto=" + datos[0].idProyecto.ToString() + "&CodPagoActividad=" + Session["CodPagoActividad"].ToString() + "&CodEstado=" + Constantes.CONST_EstadoInterventor + "&TipoPago=" + TipoPago,
                                        "Accion=Editar&CodProyecto=" + datos[0].idProyecto.ToString() + "&CodPagoActividad=" + hdCodigoPago.Value + "&CodEstado=" + Constantes.CONST_EstadoInterventor + "&TipoPago=" + TipoPago,
                                        "Pagos Actividad Interventoria");

                                        agenda.Agendar();
                                    }
                                    Session["boton"] = null;
                                    Response.Redirect("PagosActividad.aspx");
                                }
                            }
                            if(aprobado == "1")
                            {
                                if (!string.IsNullOrEmpty(hdCodigoPago.Value))
                                {
                                    txtSQL = "UPDATE PagoActividad SET FechaInterventor=Getdate(), Estado = " + Constantes.CONST_EstadoCoordinador;
                                    txtSQL += ", ObservaInterventor = '" + txtObservaInterventor.Text + "' WHERE Estado not in(4,5) and Id_PagoActividad = " + hdCodigoPago.Value;
                                    ejecutaReader(txtSQL, 2);

                                    Session["boton"] = null;
                                    Response.Redirect("PagosActividad.aspx");
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + validado + "');", true);
                btn_accion.Enabled = true;
                btnEnviar.Enabled = true;
                //return;
            }
        }

        private bool ValidarAprobacion(int codActividad, int codMes)
        {
            var resp = true;
            //Plan Operativo
            if (TipoPago == "1")
            {
                var obj = (from po in consultas.Db.AvanceActividadPOMes
                           where po.CodActividad == codActividad && po.Mes == codMes
                           && po.Aprobada == true
                           select po).ToList();
                if (obj.Count() == 0)
                {
                    var mensaje = "El avance no ha sido aprobado por el interventor, por tal motivo no puede solicitar un pago de este avance.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                    resp = false;
                }
            }

            //Nomina
            if (TipoPago == "2")
            {
                var obj = (from no in consultas.Db.AvanceCargoPOMes
                           where no.CodCargo == codActividad && no.Mes == codMes
                           && no.Aprobada == true
                           select no).ToList();

                if (obj.Count() == 0)
                {
                    var mensaje = "El avance no ha sido aprobado por el interventor, por tal motivo no puede solicitar un pago de este avance.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                    resp = false;
                }
            }

            return resp;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Adicionar documento "en la página CatalogoDocumentoPagos.aspx".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtn_addDocumentoPago_Click(object sender, ImageClickEventArgs e)
        {
            //if (Session["CodPagoActividad"] != null)
            if (hdCodigoPago.Value != null)
            {
                HttpContext.Current.Session["codPagoactividad"] = hdCodigoPago.Value;
                //HttpContext.Current.Session["CodPagoActividad"] = HttpContext.Current.Session["CodPagoActividad"].ToString();
                Redirect(null, "CatalogoDocumentoPagos.aspx?Accion=Nuevo", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Primero debe grabar el pago de la actividad!')", true);
            }
           
        }

        protected void imgBtn_addDocumentoPago_Click2(object sender, ImageClickEventArgs e)
        {
            //if (Session["CodPagoActividad"] != null)
            if (hdCodigoPago.Value != null)
            {
                HttpContext.Current.Session["codPagoactividad"] = hdCodigoPago.Value;
                //HttpContext.Current.Session["CodPagoActividad"] = HttpContext.Current.Session["CodPagoActividad"].ToString();
                //Redirect(null, "CatalogoDocumentoPagos.aspx?Accion=Lista", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                Response.Redirect("CatalogoDocumentoPagos.aspx?Accion=Lista");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Primero debe grabar el pago de la actividad!')", true);
            }

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (hdCodigoPago.Value != null)
            {
                var archivosAdjuntos = consultas.Db.PagoActividadarchivo.Any(filter => filter.CodPagoActividad.Equals(hdCodigoPago.Value));

                if (!archivosAdjuntos)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Para enviar el pago al interventor debe tener mínimo un archivo adjunto.');", true);
                    return;
                }

                txtSQL = "UPDATE PagoActividad SET Estado = " + Constantes.CONST_EstadoInterventor;
                //txtSQL += ", Observaciones = '" + Observaciones.Text + "' WHERE Id_PagoActividad = " + Session["CodPagoActividad"].ToString();
                txtSQL += ", Observaciones = '" + Observaciones.Text + "',FechaEnvioAInterventor = Getdate() WHERE Estado not in(4,5) and Id_PagoActividad = " + hdCodigoPago.Value;
                ejecutaReader(txtSQL, 2);

                // Seleccional id del Interventor
                var datos = (from ei in consultas.Db.EmpresaInterventors
                             join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                             join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                             where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                             select new datosAgendar
                             {
                                 idContacto = (int)ei.CodContacto,
                                 idProyecto = (int)p.Id_Proyecto,
                                 nombre = p.NomProyecto
                             }).ToList();
                //Agendar tarea.
                AgendarTarea agenda = new AgendarTarea
                (datos[0].idContacto,
                "Aprobar Solicitud de Pago",
                //"Aprobación de Solicitud de Pago No. " + Session["CodPagoActividad"].ToString() + "  Observaciones: " + Observaciones.Text.Trim(),
                "Aprobación de Solicitud de Pago No. " + hdCodigoPago.Value + "  Observaciones: " + Observaciones.Text.Trim(),
                datos[0].idProyecto.ToString(),
                32,
                "0",
                true,
                1,
                true,
                false,
                usuario.IdContacto,
                "CodProyecto=" + datos[0].idProyecto,
                //"Accion=Editar&CodProyecto=" + datos[0].idProyecto.ToString() + "&CodPagoActividad=" + Session["CodPagoActividad"].ToString() + "&CodEstado=" + Constantes.CONST_EstadoInterventor + "&TipoPago=" + TipoPago,
                "Accion=Editar&CodProyecto=" + datos[0].idProyecto.ToString() + "&CodPagoActividad=" + hdCodigoPago.Value + "&CodEstado=" + Constantes.CONST_EstadoInterventor + "&TipoPago=" + TipoPago,
                "Pagos Actividad");

                agenda.Agendar();
                Session["boton"] = null;
                Response.Redirect("PagosActividad.aspx");
            }
            else
            {
                btn_accion.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Primero debe adicionar el pago, antes de enviarlo al Interventor!');", true);
                return;
            }
            
        }

        protected void ddl_Tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddl_Tipo.SelectedValue == "1")
            {
                btnEnviar.Enabled = false;
            }
        } 
       
        private bool ValidaCampos()
        {
            var resp = true;
            if (ddl_Tipo.SelectedValue == "0" || Request.Form.Get("ctl00$bodyContentPlace$ddl_Tipo").ToString() == "0")
            {
                ddl_Tipo.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un tipo de pago!');", true);
            }

            if (ddl_meses.SelectedValue == "0" || Request.Form.Get("ctl00$bodyContentPlace$ddl_meses").ToString() == "0")
            {
                ddl_meses.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar el mes!');", true);
            }

            if (ddl_actividad_cargo.SelectedValue == "0" || Request.Form.Get("ctl00$bodyContentPlace$ddl_actividad_cargo").ToString() == "0")
            {
                ddl_actividad_cargo.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar la actividad!');", true);
            }

            if (ddl_Concepto.SelectedValue == "0" || Request.Form.Get("ctl00$bodyContentPlace$ddl_Concepto").ToString() == "0")
            {
                ddl_Concepto.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un concepto!');", true);
            }

            if (ddl_CodPagoBeneficiario.SelectedValue == "0" || Request.Form.Get("ctl00$bodyContentPlace$ddl_CodPagoBeneficiario").ToString() == "0")
            {
                ddl_CodPagoBeneficiario.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un beneficiario!');", true);
            }

            if (ddl_CodPagoForma.SelectedValue == "0" || Request.Form.Get("ctl00$bodyContentPlace$ddl_CodPagoForma").ToString() == "0")
            {
                ddl_CodPagoForma.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar una forma de pago!');", true);
            }

            if (string.IsNullOrEmpty(Observaciones.Text))
            {
                Observaciones.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar una observacion!');", true);
            }

            if (string.IsNullOrEmpty(CantidadDinero.Text))
            {
                CantidadDinero.Focus();
                resp = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar el valor solicitado!');", true);
            }
            return resp;
        }

        private bool ValidaAvance(int codActividad, int mes)
        {
            var resp = true;
            //Plan Operativo
            if(TipoPago == "1")
            {
                var obj = (from po in consultas.Db.AvanceActividadPOMes
                           where po.CodActividad == codActividad && po.Mes == mes
                           select po).ToList();
                if (obj.Count() == 0)
                {
                    var mensaje = "No puede ingresar una solicitud de pago sin haber ingresado un avance sobre la misma actividad y mes en el Plan Operativo!";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                    resp = false;
                }
            }

            //Nomina
            if (TipoPago == "2")
            {
                var obj = (from no in consultas.Db.AvanceCargoPOMes
                           where no.CodCargo == codActividad && no.Mes == mes
                           select no).ToList();

                if (obj.Count() == 0)
                {
                    var mensaje = "No puede ingresar una solicitud de pago sin haber ingresado un avance sobre el mismo cargo y mes en la pestaña de Nómina!";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                    resp = false;
                }
            }

            return resp;
        }

        protected void DisableControls(Control parent, bool State)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is DropDownList)
                {
                    ((DropDownList)(c)).Enabled = State;
                }

                if (c is TextBox)
                {
                    ((TextBox)(c)).Enabled = State;
                }

                DisableControls(c, State);
            }
        }

    }
}