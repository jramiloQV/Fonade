using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Clases;


namespace Fonade.FONADE.interventoria
{
    public partial class PagosActividadInter : Negocio.Base_Page
    {
        string CodContatoFiduciaria;
        string TipoPago;
        String CodProyecto;
        String txtSQL;
        String CodPagoActividad;

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener valores de la variable de sesión.
            CodContatoFiduciaria = HttpContext.Current.Session["CodContatoFiduciaria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodContatoFiduciaria"].ToString()) ? HttpContext.Current.Session["CodContatoFiduciaria"].ToString() : "0";
            TipoPago = HttpContext.Current.Session["TipoPago"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["TipoPago"].ToString()) ? HttpContext.Current.Session["TipoPago"].ToString() : "2";
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodPagoActividad = HttpContext.Current.Session["CodPagoActividad"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodPagoActividad"].ToString()) ? HttpContext.Current.Session["CodPagoActividad"].ToString() : "0";

            var datat = new DataTable();

            //Columnas para la tabla.
            datat.Columns.Add("Id_PagoActividad");
            datat.Columns.Add("NomPagoActividad");
            datat.Columns.Add("Estado");

            //Establecer títulos...
            lblNombre.Text = usuario.Nombres + " " + usuario.Apellidos;
            if (TipoPago == Constantes.CONST_TipoPagoActividad.ToString())
            { lblTitulo.Text = "PAGOS POR ACTIVIDAD"; }
            else if (TipoPago == Constantes.CONST_TipoPagoNomina.ToString())
            { lblTitulo.Text = "PAGOS DE NOMINA"; }
            DateTime fecha = DateTime.Now;
            string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
            lbl_tiempo.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;

            if (!IsPostBack)
            {
                CargarDatos();
                CargarBeneficiarios();
                CargarConceptos();
                CargarFomaDePago();

                //CodContatoFiduciaria != "0" && 
                if (TipoPago != "0" && CodProyecto != "0" && CodPagoActividad != "0")
                {
                    CargarDatosPagoSeleccionado(CodPagoActividad);
                    /*Pedro V. Carreño - ERROR INT-01 - Error en tareas tipo Aprobar solicitud de pago, al revisar la tarea debe 
                     * tener un link que lleve directamente al pago en una ventana auxiliar (no funciona el link) - Inicio*/
                    //pnl_PagosActividad.Visible = false;
                    //pnl_Datos.Visible = true;
                    pnl_PagosActividad.Visible =  true;
                    pnl_Datos.Visible = false;
                    /*Pedro V. Carreño - ERROR INT-01 - Error en tareas tipo Aprobar solicitud de pago, al revisar la tarea debe 
                     * tener un link que lleve directamente al pago en una ventana auxiliar (no funciona el link) - Fin*/
                    if (HttpContext.Current.Session["Accion"].ToString() == "Editar")
                    {
                        //Bloquear campos.
                        ddl_Concepto.Enabled = false;
                        ddl_CodPagoBeneficiario.Enabled = false;
                        ddl_CodPagoForma.Enabled = false;
                        Observaciones.Enabled = false;
                        CantidadDinero.Enabled = false;
                    }
                    else { 
                    //Ocultar y mostrar ciertos campos.
                    lbl_numsolicitudRechazada.Visible = true;
                    lbl_tipo_seleccionado.Visible = true;
                    ddl_Tipo.Visible = false;
                    lbl_mes_seleccionado.Visible = true;
                    ddl_meses.Visible = false;
                    lbl_loaded_actividad_cargo.Visible = true;
                    ddl_actividad_cargo.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Cargar la información de los pagos, se ha movido el código de postback para mejorar
        /// la comprensión del código.
        /// </summary>
        private void CargarDatos()
        {
            #region Código LINQ...
            //var result = from f in consultas.Db.MD_Fiduciaria() select f;

            //if (result != null)
            //{
            //    if (!string.IsNullOrEmpty(CodContatoFiduciaria))
            //    {
            //        result = result.Where(f => f.codcontactofiduciaria == Convert.ToInt32(CodContatoFiduciaria));
            //    }

            //    result = result.OrderBy(f => f.Fecha);
            //    result = result.OrderBy(f => f.Id_PagoActividad);

            //    foreach (var res in result)
            //    {
            //        var result2 = (from i in consultas.Db.MD_InterventorFida(Constantes.CONST_RolInterventorLider, usuario.IdContacto, res.Id_Empresa)
            //                       select new
            //                       {
            //                           Intervemtor = i.Intervemtor
            //                       }).FirstOrDefault();

            //        if (result2 != null)
            //        {
            //            DataRow filadt = datat.NewRow();

            //            filadt["Id_PagoActividad"] = "" + res.Id_PagoActividad;
            //            filadt["Fecha"] = "" + res.Fecha;
            //            filadt["RazonSocial"] = "" + res.razonsocial;
            //            filadt["Intervemtor"] = "" + result2.Intervemtor;
            //            filadt["Valor"] = "" + res.Valor;
            //            filadt["ObservaInterventor"] = "" + res.ObservaInterventor;

            //            datat.Rows.Add(filadt);
            //        }
            //    }
            //    if (datat.Rows.Count != 0)
            //    {
            //        gv_pagosactividad.DataSource = datat;
            //        gv_pagosactividad.DataBind();
            //    }
            //    else
            //    {
            //        resTotal.Visible = false;
            //        resTotal.Enabled = false;
            //    }
            //}
            //else
            //{
            //    resTotal.Visible = false;
            //    resTotal.Enabled = false;
            //} 
            #endregion

            #region Código SQL.

            try
            {
                //Inicializar variables.
                DataTable rs = new DataTable();
                String txtSQL;

                txtSQL = " SELECT Id_PagoActividad, NomPagoActividad, Estado FROM PagoActividad " +
                         " WHERE TipoPago = " + TipoPago + " AND Estado = " + Constantes.CONST_EstadoInterventor +
                         " AND CodProyecto = " + CodProyecto;

                rs = consultas.ObtenerDataTable(txtSQL, "text");

                gv_pagosactividad.DataSource = rs;
                gv_pagosactividad.DataBind();
            }
            catch { }

            #endregion
        }

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
        /// Regresar a la ventana "anterior".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CodProyecto"] = CodProyecto;
            HttpContext.Current.Session["TipoPago"] = TipoPago;
          //  Redirect(null, "PagosActividadInter.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
            pnl_PagosActividad.Visible = true;
            pnl_Datos.Visible = false;
        }

        /// <summary>
        /// RowDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_pagosactividad_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var lbl_id = e.Row.FindControl("lbl_id") as Label;
                    var lnk_elim = e.Row.FindControl("lnk_eliminar") as LinkButton;
                    var imgEditar = e.Row.FindControl("imgeditar") as Image;
                    var lbl_Est = e.Row.FindControl("lbl_Estado") as Label;

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
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_pagosactividad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "eliminar")
            {

            }
            if (e.CommandName == "editar")
            {
                string[] palabras = e.CommandArgument.ToString().Split(';');
                CodPagoActividad = palabras[0];
                //HttpContext.Current.Session["CodPagoActividad"] = CodPagoActividad;
                hdCodigoPago.Value = CodPagoActividad;
                CargarDatosPagoSeleccionado(CodPagoActividad);
                pnl_PagosActividad.Visible = false;
                pnl_Datos.Visible = true;
            }
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
            DataTable RsPagoActividad = new DataTable();
            DataTable RsEstado = new DataTable();

            try
            {
                ////Establecer texto del botón
                //btn_accion.Text = "Actualizar";

                txtSQL = " SELECT * FROM PagoActividad WHERE Id_PagoActividad =" + CodPagoActividad + " AND CodProyecto = " + CodProyecto;

                RsPagoActividad = consultas.ObtenerDataTable(txtSQL, "text");

                if (RsPagoActividad.Rows.Count > 0)
                {
                    lbl_loaded_actividad_cargo.Text = RsPagoActividad.Rows[0]["NomPagoActividad"].ToString();

                    if (RsPagoActividad.Rows[0]["Tipo"].ToString() == "1") { lbl_tipo_seleccionado.Text = "Nueva"; }
                    if (RsPagoActividad.Rows[0]["Tipo"].ToString() == "2") { lbl_tipo_seleccionado.Text = "Rechazada"; }

                    lbl_numsolicitudRechazada.Text = RsPagoActividad.Rows[0]["Id_Pagoactividad"].ToString();
                    ddl_NumSolicitudRechazada.Visible = false;
                    td_archivosAdjuntos.Visible = true;

                    lbl_mes_seleccionado.Text = "Mes " + RsPagoActividad.Rows[0]["Mes"].ToString();

                    ddl_Concepto.SelectedValue = RsPagoActividad.Rows[0]["CodPagoConcepto"].ToString();
                    ddl_CodPagoBeneficiario.SelectedValue = RsPagoActividad.Rows[0]["CodPagoBeneficiario"].ToString();
                    ddl_CodPagoForma.SelectedValue = RsPagoActividad.Rows[0]["CodPagoForma"].ToString();
                    Observaciones.Text = RsPagoActividad.Rows[0]["Observaciones"].ToString();
                    CantidadDinero.Text = double.Parse(RsPagoActividad.Rows[0]["CantidadDinero"].ToString()).ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }

                txtSQL = "select Estado from PagoActividad where id_pagoactividad = " + CodPagoActividad;
                RsEstado = consultas.ObtenerDataTable(txtSQL, "text");

                if (RsEstado.Rows.Count > 0)
                {
                    if (RsEstado.Rows[0]["Estado"].ToString() == "1")
                    {
                        tr_1.Visible = true;
                        tr_2.Visible = true;

                        btnEnviar.Visible = true;
                        btnRegresar.Visible = true;
                    }
                }
            }
            catch { }
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
            {
                txtSQL = " SELECT * FROM PagoConcepto ";

                ddl_Concepto.Items.Clear();

                //Valor por defecto.
                item = new ListItem();
                item.Text = "Seleccione el concepto";
                item.Value = "0";
                ddl_Concepto.Items.Add(item);

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in RS.Rows)
                {
                    item = new ListItem();
                    item.Text = row["NomPagoConcepto"].ToString();
                    item.Value = row["Id_PagoConcepto"].ToString();
                    ddl_Concepto.Items.Add(item);
                }
            }
            catch { }
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
            {
                txtSQL = " SELECT Id_PagoBeneficiario, razonsocial + ' - ' + nombre + ' ' + apellido as razonsocial FROM PagoBeneficiario WHERE CodEmpresa in (SELECT id_empresa FROM empresa WHERE CodProyecto = " + CodProyecto + ")";

                ddl_CodPagoBeneficiario.Items.Clear();

                //Valor por defecto.
                item = new ListItem();
                item.Text = "Seleccione el nombre del beneficiario";
                item.Value = "0";
                ddl_CodPagoBeneficiario.Items.Add(item);

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in RS.Rows)
                {
                    item = new ListItem();
                    item.Text = row["razonsocial"].ToString();
                    item.Value = row["Id_PagoBeneficiario"].ToString();
                    ddl_CodPagoBeneficiario.Items.Add(item);
                }
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/07/2014.
        /// Cargar las formas de pago.
        /// </summary>
        private void CargarFomaDePago()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            ListItem item = new ListItem();

            try
            {
                txtSQL = " SELECT * FROM PagoForma ";

                ddl_CodPagoForma.Items.Clear();

                //Valor por defecto.
                item = new ListItem();
                item.Text = "Seleccione la forma de pago";
                item.Value = "0";
                ddl_CodPagoForma.Items.Add(item);

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in RS.Rows)
                {
                    item = new ListItem();
                    item.Text = row["NomPagoForma"].ToString();
                    item.Value = row["Id_PagoForma"].ToString();
                    ddl_CodPagoForma.Items.Add(item);
                }
            }
            catch { }
        }

        /// <summary>
        /// Según FONADE clásico, hace una acción y luego carga/dibuja nuevamente la información...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable RS = new DataTable();

            if (Aprobado.SelectedValue == "Si")
            {
                #region Si es aprobado.

                txtSQL = " UPDATE PagoActividad SET FechaInterventor=Getdate(), Estado = " + Constantes.CONST_EstadoCoordinador + ", " +
                         " ObservaInterventor = '" + ObservacionesInter.Text + "' WHERE Id_PagoActividad = " + CodPagoActividad;

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
                catch { }
                finally {
                    con.Close();
                    con.Dispose();
                }
                #endregion
            }
            else
            {
                #region Si no es aprobado, se le devuelve al emprendedor.

                txtSQL = " UPDATE PagoActividad SET FechaInterventor=Getdate(), Estado = " + Constantes.CONST_EstadoEdicion + ", " +
                         " ObservaInterventor = '" + ObservacionesInter.Text + "' WHERE Id_PagoActividad = " + CodPagoActividad;

                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                catch { }
                finally {
                    con.Close();
                    con.Dispose();
                }
                #endregion

                #region Se generan tareas para los emprendedores
                //Numero de la tarea
                txtSQL = "SELECT Id_TareaPrograma FROM TareaPrograma WHERE NomTareaPrograma = 'Revisión de Solicitud de Pago - Emprendedor'";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                int CodTareaPrograma = 32;

                if (RS.Rows.Count > 0) { CodTareaPrograma = Int32.Parse(RS.Rows[0]["Id_TareaPrograma"].ToString()); }
                #endregion

                #region Tipo de pago, parametro del programa

                txtSQL = " SELECT TipoPago FROM PagoActividad WHERE Id_PagoActividad = " + CodPagoActividad;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                int CodTipoPago = 1;
                if (RS.Rows.Count > 0) { CodTipoPago = Int32.Parse(RS.Rows[0]["TipoPago"].ToString()); }

                #endregion

                #region Emprendedores del proyecto

                txtSQL = " SELECT CodContacto FROM ProyectoContacto " +
                         " WHERE CodProyecto = " + CodProyecto +
                         " AND Inactivo = 0 AND CodRol = " + Constantes.CONST_RolEmprendedor;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in RS.Rows)
                {
                    string param = "";
                    param = "Accion=Editar&CodProyecto=" + CodProyecto + "&CodPagoActividad=" + CodPagoActividad + "&CodEstado=" + Constantes.CONST_EstadoEdicion + "&TipoPago=" + CodTipoPago;
                    AgendarTarea agenda = new AgendarTarea
                        (Int32.Parse(row["CodContacto"].ToString()),
                        "Revisar Solicitud de Pago",
                        "Revisar Solicitud de Pago No. " + CodPagoActividad + "<br/>" + ObservacionesInter.Text,
                        CodProyecto,
                        CodTareaPrograma,
                        "0",
                        true,
                        1,
                        true,
                        false,
                        usuario.IdContacto,
                        param,
                        "",
                        "Firma Coordinador");

                    agenda.Agendar();
                }

                #endregion
            }

            //Cargar la información.
            //Vaciar los campos.
            //Session[""] = null;
            lbl_loaded_actividad_cargo.Text = "";
            lbl_tipo_seleccionado.Text = "";
            lbl_mes_seleccionado.Text = "";
            ddl_Concepto.Items.Clear();
            ddl_CodPagoBeneficiario.Items.Clear();
            ddl_CodPagoForma.Items.Clear();
            Observaciones.Text = "";
            CantidadDinero.Text = "";
            pnl_PagosActividad.Visible = true;
            CargarDatos();
            pnl_Datos.Visible = false;
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
            HttpContext.Current.Session["CodPagoActividad"] = hdCodigoPago.Value;
            pnl_PagosActividad.Visible = true;
            pnl_Datos.Visible = false;
            Redirect(null, "CatalogoDocumentoPagos.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }
    }
}