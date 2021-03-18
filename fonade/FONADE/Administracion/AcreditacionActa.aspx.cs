using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Configuration;
using System.Web.UI;
using Fonade.Negocio.Administracion;
using Fonade.Clases;
using System.Text;
using System.IO;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using Fonade.Account;
using Fonade.Error;
using ClosedXML.Excel;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// AcreditacionActa
    /// </summary>    
    public partial class AcreditacionActa : Base_Page
    {
        //private Int32 CodProyecto_Seleccionado;
        private Int32 CodActa_Seleccionado;

        /// <summary>
        /// CodActa que se obtiene al ejecutar el método "CrearActa"
        /// </summary>
        private Int32 CodActa_ConsultadoCreacion;

        /// <summary>
        /// Código de la convocatoria         
        /// </summary>
        private Int32 CodConvocatoria_Cargado;

        /// <summary>
        /// Indica si el acta esta repetida
        /// </summary>
        Boolean repetido;

        /// <summary>
        /// Boolean, Indica si el acta esta publicada o no.
        /// </summary>
        Boolean bPublicado;

        /// <summary>
        /// Valor que se concatena en el texto al seleccionar el acta de gv_ResultadosActas".
        /// </summary>
        String EsAcreditadaTitle;

        /// <summary>
        /// Determina si el acta está o no acreditada.
        /// </summary>
        Boolean bActaAcreditada;

        //Prueba Inicial

        ValidacionCuenta validacionCuenta = new ValidacionCuenta();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Title = "FONADE";
                if (!IsPostBack)
                {
                    //Recuperar la url
                    string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                    if (!validacionCuenta.ActaParcial(usuario.IdContacto)) //Menu Master Page
                    {
                        Response.Redirect(validacionCuenta.rutaHome(), true);
                    }
                    else
                    {
                        LLenarControlFechas();
                        LlenarActasAcreditacion();
                        LlenarConvocatorias();
                        AlternarVisibilidadPaneles();
                        DateTime fechaHoy = DateTime.Today;
                        dd_fecha_dias_Memorando.SelectedValue = fechaHoy.Day.ToString();
                        dd_fecha_mes_Memorando.SelectedValue = fechaHoy.Month.ToString();
                        dd_fecha_year_Memorando.SelectedValue = fechaHoy.Year.ToString();
                    }
                }
            }
            catch
            {
                Response.Redirect("~/Fonade/MiPerfil/Home.aspx");
            }
        }


        /// <summary>
        /// Se debe enviar la información de la tabla en uan variable se sesión
        /// para poder sortearlo.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            var sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;

                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private void LlenarActasAcreditacion(string added_sql = "")
        {
            try
            {
                String txtSQL = string.Empty;
                txtSQL = " SELECT Id_Acta, NumActa, NomActa, NomConvocatoria, FechaActa, e.publicado , ActaAcreditada " +
                         " FROM AcreditacionActa e, Convocatoria " +
                         " WHERE Id_Convocatoria = CodConvocatoria " + added_sql;
                         

                if (usuario.CodOperador != null)
                {
                    txtSQL += " and codOperador = " + usuario.CodOperador;
                }

                txtSQL += " ORDER BY 5 desc;";

                var tabla_inicial = consultas.ObtenerDataTable(txtSQL, "text");

                if (tabla_inicial.Rows.Count > 0)
                {
                    HttpContext.Current.Session["dtEmpresas"] = tabla_inicial;
                    gv_resultadosActas.DataSource = tabla_inicial;
                    gv_resultadosActas.DataBind();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al cargar las actas de acreditación.')", true);
            }
        }

        /// <summary>
        /// Oculta o no el panel principal
        /// </summary>
        private void AlternarVisibilidadPaneles()
        {
            if (pnlPrincipal.Visible == true)
            {
                pnl_detalles.Visible = false;
                lbl_enunciado.Text = "ACTAS PARCIALES DE ACREDITACIÓN";
            }
            if (pnl_detalles.Visible == true)
            {
                pnlPrincipal.Visible = false;
                lbl_enunciado.Text = "ACTA PARCIAL DE ACREDITACIÓN";
            }
        }

        private void LLenarControlFechas()
        {
            int currentYear = DateTime.Today.AddYears(-11).Year;
            int futureYear = DateTime.Today.AddYears(5).Year;

            for (int i = currentYear; i < futureYear; i++)
            {
                ListItem item = new ListItem();
                item.Text = i.ToString();
                item.Value = i.ToString();
                dd_fecha_year_Memorando.Items.Add(item);
            }
        }

        private void LlenarConvocatorias()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String txtSQL = string.Empty;

            try
            {
                txtSQL = " SELECT Id_Convocatoria, NomConvocatoria " +
                         " FROM Convocatoria WHERE Publicado = 1  ";
                         

                if (usuario.CodOperador != null)
                {
                    txtSQL += " and codOperador = " + usuario.CodOperador;
                }

                txtSQL += " ORDER BY NomConvocatoria ";

                var Listado = consultas.ObtenerDataTable(txtSQL, "text");

                if (Listado.Rows.Count > 0)
                {
                    for (int i = 0; i < Listado.Rows.Count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Value = Listado.Rows[i]["Id_Convocatoria"].ToString();
                        item.Text = Listado.Rows[i]["NomConvocatoria"].ToString();
                        dd_convocatoriasSeleccionables.Items.Add(item);
                    }
                }
            }
            catch
            {
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;

                AlternarVisibilidadPaneles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al cargar el listado de convocatorias.')", true);
                return;
            }
        }

        /// <summary>
        /// Activa los campos del formulario de creación o edición de un acta.
        /// </summary>
        /// <param name="estado"></param>
        private void ActivarCampos(bool estado)
        {
            txt_noActaSeleccionado.Enabled = estado;
            txt_NomActaSeleccionado.Enabled = estado;
            dd_fecha_dias_Memorando.Enabled = estado;
            dd_fecha_mes_Memorando.Enabled = estado;
            dd_fecha_year_Memorando.Enabled = estado;
            txt_observaciones.Enabled = estado;
            dd_convocatoriasSeleccionables.Enabled = estado;
        }

        private void MostrarCamposCrearActa(string TituloEnunciadoActa)
        {
            pnlPrincipal.Visible = false;
            pnl_detalles.Visible = true;
            lbl_enunciado_acta.Text = "CREAR ACTA";
            lbl_enunciado_acta.ForeColor = System.Drawing.Color.Red;
            lbl_enunciado_acta.Text = TituloEnunciadoActa;
            ActivarCampos(true);
            AlternarVisibilidadPaneles();
        }

        private void LLenarActaSeleccionadaImpresion(Int32 CodActa_Seleccionado)
        {
            String txtSQL;
            DateTime fecha_Detalle = new DateTime();

            try
            {
                if (CodActa_Seleccionado != 0)
                {
                    txtSQL = " SELECT NumActa, NomActa, FechaActa, Observaciones, CodConvocatoria, Publicado, ActaAcreditada " +
                             " FROM AcreditacionActa where Id_Acta = " + CodActa_Seleccionado;

                    var dtEmpresas = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dtEmpresas.Rows.Count > 0)
                    {

                        //ActivarCampos(false);

                        HttpContext.Current.Session["dtEmpresas"] = dtEmpresas;

                        //Asignar los resultados de la tabla a los campos de "solo-lectura".
                        txt_noActaSeleccionado.Text = dtEmpresas.Rows[0]["NumActa"].ToString();

                        //sp_noActa.InnerText = dtEmpresas.Rows[0]["NumActa"].ToString();
                        lblNoActa.Text = dtEmpresas.Rows[0]["NumActa"].ToString();

                        txt_NomActaSeleccionado.Text = dtEmpresas.Rows[0]["NomActa"].ToString();

                        //sp_Nombre.InnerText = dtEmpresas.Rows[0]["NomActa"].ToString();
                        lblNombre.Text = dtEmpresas.Rows[0]["NomActa"].ToString();

                        txt_observaciones.Text = dtEmpresas.Rows[0]["Observaciones"].ToString();
                        sp_observaciones.InnerText = dtEmpresas.Rows[0]["Observaciones"].ToString();
                        lblObservaciones.Text = dtEmpresas.Rows[0]["Observaciones"].ToString();

                        lblFecha.Text = Convert.ToDateTime(dtEmpresas.Rows[0]["FechaActa"]).ToString("dd/MM/yyyy");
                        bPublicado = Boolean.Parse(dtEmpresas.Rows[0]["Publicado"].ToString());
                        bActaAcreditada = Boolean.Parse(dtEmpresas.Rows[0]["ActaAcreditada"].ToString());
                        Session["idActaActreditada"] = dtEmpresas.Rows[0]["ActaAcreditada"].ToString();

                        if (bPublicado)
                        {
                            panel_AddPlanes.Visible = false;
                            txt_noActaSeleccionado.Enabled = false;
                            txt_NomActaSeleccionado.Enabled = false;
                            dd_fecha_dias_Memorando.Enabled = false;
                            dd_fecha_mes_Memorando.Enabled = false;
                            dd_fecha_year_Memorando.Enabled = false;
                            txt_observaciones.Enabled = false;
                            dd_convocatoriasSeleccionables.Enabled = false;

                            btn_imprimirMemorando.Visible = true;
                            pnlPublicar.Visible = false;

                            Publicar.Visible = false;
                            lblPublicar.Visible = false;
                            Btn_crearActa.Visible = false;
                        }
                        else
                        {
                            txt_noActaSeleccionado.Enabled = true;
                            txt_NomActaSeleccionado.Enabled = true;
                            dd_fecha_dias_Memorando.Enabled = true;
                            dd_fecha_mes_Memorando.Enabled = true;
                            dd_fecha_year_Memorando.Enabled = true;
                            txt_observaciones.Enabled = true;
                            dd_convocatoriasSeleccionables.Enabled = false;

                            btn_imprimirMemorando.Visible = false;
                            lblError.Visible = false;
                            panel_AddPlanes.Visible = true;
                            img_btn_addPlanes.Visible = true;
                            lnk_btn_addPlanes.Visible = true;

                            Btn_crearActa.Text = "Actualizar";
                            Btn_crearActa.Visible = true;
                        }

                        if (bActaAcreditada)
                        {
                            EsAcreditadaTitle = "";
                            HttpContext.Current.Session["bActaAcreditada"] = "1";
                        }
                        else
                        {
                            EsAcreditadaTitle = " NO ";
                            HttpContext.Current.Session["bActaAcreditada"] = "0";

                        }
                        fecha_Detalle = Convert.ToDateTime(dtEmpresas.Rows[0]["FechaActa"].ToString());
                        lbl_enunciado_acta.Text = "ACTA DE " + EsAcreditadaTitle + "ACREDITACIÓN";

                        //Asginando el valor seleccionado de acuerdo con la información de las variables anteriores.
                        dd_fecha_dias_Memorando.SelectedValue = fecha_Detalle.Day.ToString();
                        dd_fecha_mes_Memorando.SelectedValue = fecha_Detalle.Month.ToString();
                        dd_fecha_year_Memorando.SelectedValue = fecha_Detalle.Year.ToString();

                        sp_FechaFormateada.InnerText = dd_fecha_mes_Memorando.SelectedItem.Text + " " + dd_fecha_dias_Memorando.SelectedValue + " de " + dd_fecha_year_Memorando.SelectedValue;

                        CodConvocatoria_Cargado = Convert.ToInt32(dtEmpresas.Rows[0]["CodConvocatoria"].ToString());
                        HttpContext.Current.Session["CodConvocatoria_Acta"] = CodConvocatoria_Cargado;
                        HttpContext.Current.Session["CODCONVOCATORIAAcreditar"] = CodConvocatoria_Cargado;

                        dd_convocatoriasSeleccionables.SelectedValue = dtEmpresas.Rows[0]["CodConvocatoria"].ToString();

                        sp_convocatoria.InnerText = "  " + dd_convocatoriasSeleccionables.SelectedItem.Text;

                        lblConvocatoria.Text = !String.IsNullOrEmpty(dd_convocatoriasSeleccionables.SelectedItem.Text) ? dd_convocatoriasSeleccionables.SelectedItem.Text : String.Empty;
                    }
                }
                else
                {
                    throw new Exception("Error al cargar el codigo del acta");
                }
            }
            catch (Exception)
            {
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                AlternarVisibilidadPaneles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al obtener el codigo del acta, intentelo de nuevo )", true);
            }
        }

        private void LLenarPlanesActa(Int32 CodActa_Seleccionado)
        {
            try
            {
                consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@CodActa", Value = CodActa_Seleccionado
                                                   },
                                                   new SqlParameter
                                                   {
                                                       ParameterName = "@codConvocatoria", Value = int.Parse(Session["CodConvocatoria_Acta"].ToString())
                                                   }
                                           };

                DataTable dtEmpresas = consultas.ObtenerDataTable("pr_ProyectosAcreditados");

                HttpContext.Current.Session["dtEmpresas"] = dtEmpresas;
                gv_imprimir_planesNegocio.DataSource = dtEmpresas;
                gv_imprimir_planesNegocio.DataBind();
                gv_DetallesActa.DataSource = dtEmpresas;
                gv_DetallesActa.DataBind();

                consultas.Parameters = null;
            }
            catch (Exception ex)
            {
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                AlternarVisibilidadPaneles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al obtener la información del acta, intentelo de nuevo.)", true);
            }
        }

        /// <summary>
        /// Evento para filtras por tipos de acta
        /// </summary>
        protected void dd_tiposActa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_tiposActa.SelectedValue == "")
            {
                LlenarActasAcreditacion("");
            }
            if (dd_tiposActa.SelectedValue == "1")
            {
                LlenarActasAcreditacion(" AND ActaAcreditada = 1 ");
            }
            if (dd_tiposActa.SelectedValue == "0")
            {
                LlenarActasAcreditacion(" AND ActaAcreditada = 0 ");
            }
        }

        /// <summary>
        /// Handles the Click event of the img_btn_AddActaAcreditacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void img_btn_AddActaAcreditacion_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["EsAcreditada_Valor"] = "1";

            HttpContext.Current.Session["CodActa_Seleccionado"] = null;
            HttpContext.Current.Session["CodConvocatoria_Acta"] = null;
            HttpContext.Current.Session["bActaAcreditada"] = null;

            pnlPrincipal.Visible = false;
            pnl_detalles.Visible = true;
            lbl_enunciado_acta.Text = "CREAR ACTA";
            lbl_enunciado_acta.ForeColor = System.Drawing.Color.Red;
            lbl_enunciado_acta.Text = "ACTA DE ACREDITACIÓN";

            txt_noActaSeleccionado.Enabled = true;
            txt_noActaSeleccionado.Text = string.Empty;
            txt_NomActaSeleccionado.Enabled = true;
            txt_NomActaSeleccionado.Text = string.Empty;
            dd_fecha_dias_Memorando.Enabled = true;
            dd_fecha_mes_Memorando.Enabled = true;
            dd_fecha_year_Memorando.Enabled = true;
            txt_observaciones.Enabled = true;
            txt_observaciones.Text = string.Empty;
            dd_convocatoriasSeleccionables.Enabled = true;
            pnlPublicar.Visible = false;
            btn_imprimirMemorando.Visible = false;
            Btn_crearActa.Visible = true;
            panel_AddPlanes.Visible = false;
            gv_DetallesActa.Visible = false;
            Btn_crearActa.Text = "Crear";

            AlternarVisibilidadPaneles();
        }

        /// <summary>
        /// Handles the Click event of the lnk_btn_AddActaAcreditacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnk_btn_AddActaAcreditacion_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["EsAcreditada_Valor"] = "1";

            HttpContext.Current.Session["CodActa_Seleccionado"] = null;
            HttpContext.Current.Session["CodConvocatoria_Acta"] = null;
            HttpContext.Current.Session["bActaAcreditada"] = null;

            pnlPrincipal.Visible = false;
            pnl_detalles.Visible = true;
            lbl_enunciado_acta.Text = "CREAR ACTA";
            lbl_enunciado_acta.ForeColor = System.Drawing.Color.Red;
            lbl_enunciado_acta.Text = "ACTA DE ACREDITACIÓN";

            txt_noActaSeleccionado.Enabled = true;
            txt_noActaSeleccionado.Text = string.Empty;
            txt_NomActaSeleccionado.Enabled = true;
            txt_NomActaSeleccionado.Text = string.Empty;
            dd_fecha_dias_Memorando.Enabled = true;
            dd_fecha_mes_Memorando.Enabled = true;
            dd_fecha_year_Memorando.Enabled = true;
            txt_observaciones.Enabled = true;
            txt_observaciones.Text = string.Empty;
            dd_convocatoriasSeleccionables.Enabled = true;
            pnlPublicar.Visible = false;
            btn_imprimirMemorando.Visible = false;
            Btn_crearActa.Visible = true;
            panel_AddPlanes.Visible = false;
            gv_DetallesActa.Visible = false;
            Btn_crearActa.Text = "Crear";

            AlternarVisibilidadPaneles();
        }

        /// <summary>
        /// Handles the Click event of the img_btn_AddActa_NO_Acreditacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void img_btn_AddActa_NO_Acreditacion_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["EsAcreditada_Valor"] = "0";

            HttpContext.Current.Session["CodActa_Seleccionado"] = null;
            HttpContext.Current.Session["CodConvocatoria_Acta"] = null;
            HttpContext.Current.Session["bActaAcreditada"] = null;

            pnlPrincipal.Visible = false;
            pnl_detalles.Visible = true;
            lbl_enunciado_acta.Text = "CREAR ACTA";
            lbl_enunciado_acta.ForeColor = System.Drawing.Color.Red;
            lbl_enunciado_acta.Text = "ACTA DE NO ACREDITACIÓN";

            txt_noActaSeleccionado.Enabled = true;
            txt_noActaSeleccionado.Text = string.Empty;
            txt_NomActaSeleccionado.Enabled = true;
            txt_NomActaSeleccionado.Text = string.Empty;
            dd_fecha_dias_Memorando.Enabled = true;
            dd_fecha_mes_Memorando.Enabled = true;
            dd_fecha_year_Memorando.Enabled = true;
            txt_observaciones.Enabled = true;
            txt_observaciones.Text = string.Empty;
            dd_convocatoriasSeleccionables.Enabled = true;
            pnlPublicar.Visible = false;
            btn_imprimirMemorando.Visible = false;
            Btn_crearActa.Visible = true;
            panel_AddPlanes.Visible = false;
            gv_DetallesActa.Visible = false;
            Btn_crearActa.Text = "Crear";

            AlternarVisibilidadPaneles();
        }

        /// <summary>
        /// Handles the Click event of the lnk_btn_AddActa_NO_Acreditacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnk_btn_AddActa_NO_Acreditacion_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["EsAcreditada_Valor"] = "0";

            HttpContext.Current.Session["CodActa_Seleccionado"] = null;
            HttpContext.Current.Session["CodConvocatoria_Acta"] = null;
            HttpContext.Current.Session["bActaAcreditada"] = null;

            pnlPrincipal.Visible = false;
            pnl_detalles.Visible = true;
            lbl_enunciado_acta.Text = "CREAR ACTA";
            lbl_enunciado_acta.ForeColor = System.Drawing.Color.Red;
            lbl_enunciado_acta.Text = "ACTA DE NO ACREDITACIÓN";

            txt_noActaSeleccionado.Enabled = true;
            txt_noActaSeleccionado.Text = string.Empty;
            txt_NomActaSeleccionado.Enabled = true;
            txt_NomActaSeleccionado.Text = string.Empty;
            dd_fecha_dias_Memorando.Enabled = true;
            dd_fecha_mes_Memorando.Enabled = true;
            dd_fecha_year_Memorando.Enabled = true;
            txt_observaciones.Enabled = true;
            txt_observaciones.Text = string.Empty;
            dd_convocatoriasSeleccionables.Enabled = true;
            pnlPublicar.Visible = false;
            btn_imprimirMemorando.Visible = false;
            Btn_crearActa.Visible = true;
            panel_AddPlanes.Visible = false;
            gv_DetallesActa.Visible = false;
            Btn_crearActa.Text = "Crear";

            AlternarVisibilidadPaneles();
        }

        private void CrearActa(Int32 EsAcreditada)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand();
                String txtSQL = "";
                DateTime strtxtFecha = new DateTime();
                DataTable RSActa = new DataTable();
                DataTable RS = new DataTable();
                String fecha = string.Empty;
                String dato = dd_fecha_dias_Memorando.SelectedValue + "/" + dd_fecha_mes_Memorando.SelectedValue + "/" + dd_fecha_year_Memorando.SelectedValue;

                try
                {
                    strtxtFecha = new DateTime(Convert.ToInt32(dd_fecha_year_Memorando.SelectedValue), Convert.ToInt32(dd_fecha_mes_Memorando.SelectedValue), Convert.ToInt32(dd_fecha_dias_Memorando.SelectedValue));
                    fecha = dd_fecha_year_Memorando.SelectedValue + (dd_fecha_mes_Memorando.SelectedValue.Length < 2 ? "0" : "") + dd_fecha_mes_Memorando.SelectedValue + (dd_fecha_dias_Memorando.SelectedValue.Length < 2 ? "0" : "") + dd_fecha_dias_Memorando.SelectedValue;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La fecha seleccionada es invalida, seleccionela de nuevo.')", true);
                    return;
                }

                txtSQL = "SELECT Id_Acta FROM AcreditacionActa WHERE NomActa='" + txt_NomActaSeleccionado.Text + "'";
                RSActa = consultas.ObtenerDataTable(txtSQL, "text");

                //Verificamos que no exista otra acta con nombre repetido
                if (RSActa.Rows.Count != 0)
                {
                    repetido = true;
                    throw new ApplicationException("Ya existe acta con el mismo nombre, rectifique");
                }

                txtSQL = "INSERT INTO AcreditacionActa (NumActa, NomActa, FechaActa, Observaciones, CodConvocatoria, Publicado, ActaAcreditada)" +
                         " VALUES ('" + txt_noActaSeleccionado.Text.Trim() + "','" +
                                      txt_NomActaSeleccionado.Text.Trim() + "','" +
                                      fecha + "','" +
                                      txt_observaciones.Text.Trim() + "'," +
                                      dd_convocatoriasSeleccionables.SelectedItem.Value +
                                      ",0,'" +
                                      EsAcreditada + "')";

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    cmd = new SqlCommand(txtSQL, con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                repetido = false;

                txtSQL = "Select id_Acta AS ValorPK from AcreditacionActa where NomActa = '" + txt_NomActaSeleccionado.Text.Trim() + "' and NumActa = '" + txt_noActaSeleccionado.Text.Trim() + "'";

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    CodActa_ConsultadoCreacion = Convert.ToInt32(RS.Rows[0]["ValorPK"].ToString());
                    Session["idActaAcreditacion"] = RS.Rows[0]["ValorPK"].ToString();

                    dd_convocatoriasSeleccionables.Enabled = false;
                    lnk_btn_addPlanes.Visible = true;
                    img_btn_addPlanes.Visible = true;
                    panel_publicar.Visible = true;

                    Btn_crearActa.Text = "Actualizar";

                    Btn_crearActa.CommandArgument = "Crear";
                    panel_AddPlanes.Visible = true;

                    HttpContext.Current.Session["CodActa_Seleccionado"] = CodActa_Seleccionado;
                    HttpContext.Current.Session["CodConvocatoria_Acta"] = dd_convocatoriasSeleccionables.SelectedItem.Value;
                    HttpContext.Current.Session["bActaAcreditada"] = CodActa_ConsultadoCreacion;
                }
                RSActa = null;
                RS = null;
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia al generar el acta detalle : " + ex.Message + " ')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al generar el acta, intentelo de nuevo. detalle : " + ex.Message + "')", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the Btn_crearActa control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception">Error al obtener el tipo de acta.</exception>
        protected void Btn_crearActa_Click(object sender, EventArgs e)
        {
            if (Btn_crearActa.Text == "Crear")
            {
                try
                {
                    Int32 EsAcreditada_Valor = Convert.ToInt32(HttpContext.Current.Session["EsAcreditada_Valor"].ToString());

                    if (EsAcreditada_Valor == 1 || EsAcreditada_Valor == 0)
                    {
                        FieldValidate.ValidateString("Numero acta", txt_noActaSeleccionado.Text, true, 10);
                        FieldValidate.ValidateString("Nombre acta", txt_NomActaSeleccionado.Text, true, 80);
                        FieldValidate.ValidateString("Observaciones", txt_observaciones.Text, true, 1500);

                        CrearActa(EsAcreditada_Valor);
                    }
                    else
                    {
                        throw new Exception("Error al obtener el tipo de acta.");
                    }
                }
                catch (ApplicationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia al crear el acta detalle ex :" + ex.Message + ".')", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al crear el acta, intentelo de nuevo')", true);
                }
            }
            if (Btn_crearActa.Text == "Actualizar")
            {
                try
                {
                    FieldValidate.ValidateString("Numero acta", txt_noActaSeleccionado.Text, true, 10);
                    FieldValidate.ValidateString("Nombre acta", txt_NomActaSeleccionado.Text, true, 80);
                    FieldValidate.ValidateString("Observaciones", txt_observaciones.Text, true, 1500);

                    CodActa_ConsultadoCreacion = Convert.ToInt32(HttpContext.Current.Session["CodActa_Seleccionado"]);

                    ActualizarActa();

                    if (Publicar.Checked)
                    {
                        Correo email;
                        AcreditacionActaNegocio acredActaNeg = new AcreditacionActaNegocio();
                        List<Contacto> lstContactos = new List<Contacto>();

                        var ruta = ConfigurationManager.AppSettings["RutaHttpArchivosExcelActas"] + Session["nombreActaAxcel"].ToString();
                        var imgLogo = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

                        var cuerpo = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>";
                        cuerpo += "<html xmlns='http://www.w3.org/1999/xhtml'>";
                        cuerpo += "<head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' />";
                        cuerpo += "<title></title><style type='text/css'>body {margin: 0; padding: 0; min-width: 100%!important;} .content {width: 100%; max-width: 600px; font-family: arial; font-size:}  </style></head>";
                        cuerpo += "<body yahoo bgcolor='#f6f8f1'>";
                        cuerpo += "<table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr><td>";
                        cuerpo += "<table class='content' align='center' cellpadding='0' cellspacing='0' border='0'>";
                        cuerpo += "<tr><td>" + DateTime.Now.Date + ", <br><br>Estimado Nombress<br>Cordial Saludo.<br><br>Realice la descarga de los proyectos que fueron acreditados a través del siguiente enlace<br>";
                        cuerpo += "<b><a href='" + ruta + "'>Descargar</a></b>";
                        cuerpo += "<br><br> Cordialmente, <br> Fondo Emprender - SENA<br>";
                        cuerpo += "<img src=\'" + imgLogo + "\' /> </td></tr></table></td></tr></table></body></html>";


                        lstContactos = acredActaNeg.TraerUsuariosCreaActaParcialAcreditacion(usuario.CodOperador);
                        foreach (var item in lstContactos)
                        {
                            string nombres = item.Nombres + " " + item.Apellidos;

                            email = new Correo(
                                            ConfigurationManager.AppSettings["Email"].ToString(),
                                            "Fondo Emprender",
                                            item.Email,
                                            item.Nombres + " " + item.Apellidos,
                                            "Descarga Acta de Acreditacion ",
                                            cuerpo.Replace("Nombress", nombres)
                                    );
                            email.Enviar();
                        }
                    }

                    LlenarActasAcreditacion();
                    pnl_detalles.Visible = false;
                    pnlPrincipal.Visible = true;

                }
                catch (ApplicationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia al crear el acta detalle ex :" + ex.Message + ".')", true);
                }
                //catch (Exception ex)
                //{
                //    string url = Request.Url.ToString();

                //    string mensaje = ex.Message.ToString();
                //    string data = ex.Data.ToString();
                //    string stackTrace = ex.StackTrace.ToString();
                //    string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                //    // Log the error
                //    ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al crear el acta detalle ex :" + ex.Message + ".')", true);
                //}
            }
        }

        private string CrearArchivoActa()
        {
            StringBuilder sbActa = new StringBuilder();

            try
            {
                sbActa.AppendLine("<!DOCTYPE html> ");
                sbActa.AppendLine(@"<html xmlns=\""http://www.w3.org/1999/xhtml\""> ");
                sbActa.AppendLine("<head> ");
                sbActa.AppendLine("    <title></title>");
                sbActa.AppendLine("</head>");
                sbActa.AppendLine("<body> ");
                sbActa.AppendLine("<table style=\"width: 95%; border: 0\" >");
                sbActa.AppendLine("    <tbody>");
                sbActa.AppendLine("        <tr>");
                sbActa.AppendLine("            <td style=\"width: 50%; text-align: center; vertical-align: baseline; background-color: #000000;\" class=\"Blanca\">");
                sbActa.AppendLine("                <b>");
                sbActa.AppendLine("                    ACTA DE ACREDITACIÓN</b>");
                sbActa.AppendLine("            </td>");
                sbActa.AppendLine("            <td style=\"width: 30%; text-align: right;\" class=\"titulo\">&nbsp; </td>");
                sbActa.AppendLine("            <td style=\"width: 20%; text-align: right;\" class=\"titulo\">&nbsp; </td>");
                sbActa.AppendLine("        </tr>");
                sbActa.AppendLine("    </tbody>");
                sbActa.AppendLine("</table>");
                sbActa.AppendLine("<table style=\"width: 100%; border: 0\">");
                sbActa.AppendLine("    <tbody>");
                sbActa.AppendLine("        <tr>");
                sbActa.AppendLine("            <td style=\"width: 30%\">");
                sbActa.AppendLine("                <b>No Acta:</b> </td>");
                sbActa.AppendLine("            <td>");
                sbActa.AppendLine(lblNoActa.Text);
                sbActa.AppendLine("            </td>");
                sbActa.AppendLine("        </tr>");
                sbActa.AppendLine("<tr>");
                sbActa.AppendLine("    <td>");
                sbActa.AppendLine("        <b>Nombre:</b> </td>");
                sbActa.AppendLine("    <td>");
                sbActa.AppendLine(lblNombre.Text);
                sbActa.AppendLine("    </td>");
                sbActa.AppendLine("</tr>");
                sbActa.AppendLine("<tr>");
                sbActa.AppendLine("    <td colspan=\"2\">");
                sbActa.AppendLine("Planes de Negocio Incluidos");
                sbActa.AppendLine("    </td>");
                sbActa.AppendLine("</tr>");
                sbActa.AppendLine("                                            ");
                sbActa.AppendLine("</body> ");
                sbActa.AppendLine("</html> ");

                sbActa.AppendLine("<tr>");
                sbActa.AppendLine("    <td colspan=\"2\">");
                sbActa.AppendLine("");
                sbActa.AppendLine("<table style=\"width: 95%; border: 0\" >");
                sbActa.AppendLine("     <tr>");
                sbActa.AppendLine("         <td style=\"width: 50%; border: 0\">");
                sbActa.AppendLine("         <td>");
                sbActa.AppendLine("         <td style=\"width: 35%; border: 0\">");
                sbActa.AppendLine("         <td>");
                sbActa.AppendLine("         <td style=\"width: 15%; border: 0\">");
                sbActa.AppendLine("         <td>");
                sbActa.AppendLine("     </tr>");

                sbActa.AppendLine("     <tr>");
                sbActa.AppendLine("         <td style=\"width: 50%; border: 0\">");
                sbActa.AppendLine("Plan de Negocio");
                sbActa.AppendLine("         <td>");
                sbActa.AppendLine("         <td style=\"width: 35%; border: 0\">");
                sbActa.AppendLine("Acreditador");
                sbActa.AppendLine("         <td>");
                sbActa.AppendLine("         <td style=\"width: 15%; border: 0\">");
                sbActa.AppendLine("Viable Acreditación");
                sbActa.AppendLine("         <td>");
                sbActa.AppendLine("     </tr>");
                var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sbActa.AppendLine("     <tr>");
                        sbActa.AppendLine("         <td style=\"width: 50%; border: 0\">");
                        sbActa.AppendLine(dt.Rows[i]["NomProyecto"].ToString());
                        sbActa.AppendLine("         <td>");
                        sbActa.AppendLine("         <td style=\"width: 35%; border: 0\">");
                        sbActa.AppendLine(dt.Rows[i]["Acreditador"].ToString());
                        sbActa.AppendLine("         <td>");
                        sbActa.AppendLine("         <td style=\"width: 15%; border: 0\">");
                        sbActa.AppendLine(Convert.ToBoolean(dt.Rows[i]["viable"]) ? "Si" : "No");
                        sbActa.AppendLine("         <td>");
                        sbActa.AppendLine("     </tr>");
                    }
                }
                sbActa.AppendLine("     <tr>");
                sbActa.AppendLine("         <td colspan=\"3\">");
                sbActa.AppendLine("<br /><br /><br />");
                sbActa.AppendLine("<p>");
                sbActa.AppendLine("    Aprobó:");
                sbActa.AppendLine("</p>");
                sbActa.AppendLine("<br />");
                sbActa.AppendLine("<br />");
                sbActa.AppendLine("______________________________________<br />");
                sbActa.AppendLine("Gerente de Convenio Fondo Emprender<br />");
                sbActa.AppendLine("<br />");
                sbActa.AppendLine("<br />");
                sbActa.AppendLine("<br />");
                sbActa.AppendLine("         <td>");
                sbActa.AppendLine("     </tr>");

                sbActa.AppendLine("</table>");
                sbActa.AppendLine("    </td>");
                sbActa.AppendLine("</tr>");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbActa.ToString();
        }

        private void ActualizarActa()
        {
            if (Btn_crearActa.CommandArgument == "Crear")
            {
                Btn_crearActa.CommandArgument = "";
                return;
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String txtSQL = "";
            int reg = 0;
            DataTable RSActa = new DataTable();
            DataTable tabla_detalles = new DataTable();

            txtSQL = "SELECT Id_Acta FROM AcreditacionActa WHERE NomActa='" + txt_NomActaSeleccionado.Text + "' AND Id_Acta <> " + CodActa_ConsultadoCreacion;

            RSActa = consultas.ObtenerDataTable(txtSQL, "text");

            cmd = new SqlCommand("pr_ProyectosAcreditados", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodActa", HttpContext.Current.Session["CodActa_Seleccionado"].ToString());
            cmd.Parameters.AddWithValue("@codConvocatoria", HttpContext.Current.Session["CodConvocatoria_Acta"].ToString());
            try
            {
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    tabla_detalles.Load(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            if (RSActa.Rows.Count == 0)
            {

                foreach (DataRow row in tabla_detalles.Rows)
                {
                    conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    string viable = row["viableAcreditador"].ToString();
                    int viable_dato = 0;
                    if (viable == "SI")
                    {
                        viable_dato = 1;
                    }

                    txtSQL = " UPDATE AcreditacionActaProyecto SET Acreditado = " + viable_dato +
                                " WHERE CodActa = " + CodActa_ConsultadoCreacion +
                                " AND CodProyecto = " + row["id_proyecto"].ToString();

                    cmd = new SqlCommand(txtSQL, conn);

                    try
                    {
                        conn.Open();
                        reg = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }

                    if (Publicar.Checked)
                    {
                        ///Si el proyecto es viable pasa al siguiente estado "Asignación de Recursos" sino
                        ///vuelve al estado de "Aprobación Técnica" para que pueda participar en una próxima convocatoria.
                        var proyecto = (from p in consultas.Db.Proyecto1s
                                        where p.Id_Proyecto == int.Parse(row["id_proyecto"].ToString())
                                        select p).FirstOrDefault();

                        if (Boolean.Parse(row["Viable"].ToString()))
                        {
                            proyecto.CodEstado = Constantes.CONST_Acreditado;
                            consultas.Db.SubmitChanges();

                        }
                        else
                        {
                            proyecto.CodEstado = Constantes.CONST_concejo_directivo;
                            consultas.Db.SubmitChanges();
                        }

                        var proyectoContacto = (from pc in consultas.Db.ProyectoContactos
                                                where pc.CodRol == 9 && pc.Inactivo == false && pc.CodProyecto == int.Parse(row["id_proyecto"].ToString())
                                                select pc).FirstOrDefault();
                        if (proyectoContacto != null)
                        {
                            proyectoContacto.Inactivo = true;
                            proyectoContacto.FechaFin = DateTime.Now;
                            consultas.Db.SubmitChanges();
                        }

                    }
                }

                var acreditacionActa = (from aa in consultas.Db.AcreditacionActa
                                        where aa.Id_Acta == int.Parse(Session["idActaAcreditacion"].ToString())
                                        select aa).FirstOrDefault();
                if (acreditacionActa != null)
                {
                    acreditacionActa.NumActa = txt_noActaSeleccionado.Text;
                    acreditacionActa.NomActa = txt_NomActaSeleccionado.Text;
                    acreditacionActa.FechaActa = DateTime.Now;
                    acreditacionActa.Observaciones = txt_observaciones.Text;

                    if (Publicar.Checked)
                    {
                        acreditacionActa.publicado = true;
                        //Crea excel -->
                        var nombreArchivos = "ArchivoActaParcial_" + Session["idActaAcreditacion"].ToString() + ".xlsx";
                        Session["nombreActaAxcel"] = nombreArchivos;
                        CrearExcelActa(nombreArchivos, int.Parse(Session["idActaAcreditacion"].ToString()), int.Parse(Session["CODCONVOCATORIAAcreditar"].ToString()));
                    }
                    consultas.Db.SubmitChanges();
                }
                repetido = false;
            }
            else
            {
                repetido = true;
                lblError.Text = "Ya existe esa Acta, verifique el nombre!";
            }
        }

        private void EliminarActaSeleccionada(Int32 CodActa_Seleccionado)
        {
            var query = (from aa in consultas.Db.AcreditacionActa
                         where aa.Id_Acta == CodActa_Seleccionado
                         select aa).FirstOrDefault();

            if (query != null)
            {
                if (query.publicado == false)
                {
                    var proyectosActa = (from pa in consultas.Db.AcreditacionActaProyecto
                                         where pa.CodActa == CodActa_Seleccionado
                                         select pa).ToList();
                    if (proyectosActa.Count() > 0)
                    {
                        consultas.Db.AcreditacionActaProyecto.DeleteAllOnSubmit(proyectosActa);
                    }
                }

                consultas.Db.AcreditacionActa.DeleteOnSubmit(query);
                consultas.Db.SubmitChanges();
            }
        }

        private void BorrarProyecto(String id_proyecto)
        {
            String txtSQL;
            txtSQL = "DELETE FROM AcreditacionActaproyecto WHERE CodActa = " + Session["idActaAcreditacion"].ToString() + " AND CodProyecto = " + id_proyecto + "; ";
            txtSQL += "Update proyecto set CodEstado = 11 Where id_Proyecto = " + id_proyecto;

            ejecutaReader(txtSQL, 2);
            LLenarPlanesActa(int.Parse(Session["idActaAcreditacion"].ToString()));
        }

        /// <summary>
        /// Handles the RowDataBound event of the gv_resultadosActas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_resultadosActas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = e.Row.FindControl("lbl_siNoActa") as Label;
                var labelActividadPo = e.Row.FindControl("lblactividaPOI") as Label;
                var lnk = e.Row.FindControl("lnkeliminar") as LinkButton;
                var imgEditar = e.Row.FindControl("lnkeliminar") as Image;

                if (lbl != null)
                {
                    if (lbl.Text == "True")
                        lbl.Text = "SI";
                    else
                        lbl.Text = "NO";
                }

                if (lnk != null && labelActividadPo != null)
                {
                    if (labelActividadPo.Text == "False")
                    {
                        lnk.Visible = true;
                        if (imgEditar != null)
                            imgEditar.Visible = true;
                    }
                    else
                    {
                        lnk.Visible = false;
                        if (imgEditar != null)
                            imgEditar.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Sorting event of the gv_resultadosActas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gv_resultadosActas_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_resultadosActas.DataSource = HttpContext.Current.Session["dtEmpresas"];
                gv_resultadosActas.DataBind();
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gv_resultadosActas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gv_resultadosActas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar")
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                CodActa_Seleccionado = Convert.ToInt32(valores_command[0].ToString());
                if (valores_command[1].ToString() == "True")
                {
                    Publicar.Checked = true;
                }
                else
                {
                    Publicar.Checked = false;
                }


                pnlPrincipal.Visible = false;
                pnl_detalles.Visible = true;
                lbl_enunciado.Text = "ACTA PARCIAL DE ACREDITACIÓN";

                Session["idActaAcreditacion"] = CodActa_Seleccionado;
                HttpContext.Current.Session["CodActa_Seleccionado"] = CodActa_Seleccionado;
                LLenarActaSeleccionadaImpresion(CodActa_Seleccionado);
                LLenarPlanesActa(CodActa_Seleccionado);

                Btn_crearActa.Text = "Actualizar";
            }
            if (e.CommandName == "eliminar")
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                CodActa_Seleccionado = Convert.ToInt32(valores_command[0].ToString());
                EliminarActaSeleccionada(CodActa_Seleccionado);
                Response.Redirect("AcreditacionActa.aspx");
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gv_resultadosActas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gv_resultadosActas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_resultadosActas.PageIndex = e.NewPageIndex;
            gv_resultadosActas.DataSource = HttpContext.Current.Session["dtEmpresas"];
            gv_resultadosActas.DataBind();
        }

        /// <summary>
        /// Handles the RowCommand event of the gv_DetallesActa control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gv_DetallesActa_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "mostrar_acreditador")
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                HttpContext.Current.Session["IdAcreditador_Session"] = valores_command[0];
                HttpContext.Current.Session["NombreAcreditador_Session"] = valores_command[1];

                String txtSQL = "";

                txtSQL = "SELECT CodGrupo FROM GrupoContacto WHERE CodContacto = " + valores_command[0].ToString();

                var t = consultas.ObtenerDataTable(txtSQL, "text");

                HttpContext.Current.Session["CodRol_ActaAcreditacion"] = t.Rows[0]["CodGrupo"].ToString();

                Redirect(null, "../MiPerfil/VerPerfilContacto.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=430,top=100");
            }
            if (e.CommandName == "mostrar_proyecto")
            {
                img_btn_addPlanes.Visible = true;
                lnk_btn_addPlanes.Visible = true;

                var valores_command_proyecto = new string[] { };
                valores_command_proyecto = e.CommandArgument.ToString().Split(';');

                HttpContext.Current.Session["ID_PROYECTOAcreditar"] = valores_command_proyecto[0];

                Response.Redirect("ProyectoAcreditacion.aspx");
            }
            if (e.CommandName == "eliminar_detalle")
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                try
                {
                    int a = Int32.Parse(valores_command[1].ToString());
                    BorrarProyecto(valores_command[0]);
                }
                catch { }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gv_imprimir_planesNegocio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_imprimir_planesNegocio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Inicializar variables e instancias de controles.
                Label lbl = e.Row.FindControl("lbl_ViableSiNo") as Label;

                try
                {
                    if (lbl != null)
                    {
                        if (lbl.Text == "True")
                            lbl.Text = "SI";
                        else
                            lbl.Text = "NO";
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gv_DetallesActa control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_DetallesActa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnk = e.Row.FindControl("lnkeliminar_detalle") as LinkButton;

                if (lnk != null)
                {
                    if (bPublicado)
                    {
                        lnk.Visible = false;
                    }
                    else
                    {
                        lnk.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the img_btn_addPlanes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void img_btn_addPlanes_Click(object sender, ImageClickEventArgs e)
        {
            Redirect(null, "AdicionarProyectoAcreditacionActa.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// Handles the Click event of the lnk_btn_addPlanes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnk_btn_addPlanes_Click(object sender, EventArgs e)
        {
            Redirect(null, "AdicionarProyectoAcreditacionActa.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=290,top=100");
        }

        /// <summary>
        /// Handles the Click event of the btn_imprimirMemorando control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_imprimirMemorando_Click(object sender, EventArgs e)
        {
        }

        private void CrearExcelActa(string nomArchivo, int codActa, int codConvoca)
        {
            try
            {
                var ruta = ConfigurationManager.AppSettings["RutaDocumentosConvocatoriasExcel"];
                consultas.Parameters = new[]
                {
                        new SqlParameter
                        {
                            ParameterName = "@CodActa", Value = codActa
                        },
                        new SqlParameter
                        {
                            ParameterName = "@codConvocatoria", Value = codConvoca
                        }
                };

                string columna_1 = "No", columna_2= "Id Plan", columna_3= "Nombre Plan";
                string columna_4 = "Sector", columna_5 = "SubSector", columna_6 = "Fecha Acta Acreditacion";
                string columna_7 = "Nombre Unidad", columna_8 = "Id Convocatoria", columna_9 = "Nombre Convocatoria";
                string columna_10 = "Sumario";

                var dt = consultas.ObtenerDataTable("pr_ProyectosAcreditadosExcel");
                var dtFinal = new DataTable();
                dtFinal.Columns.Add(columna_1, typeof(Int32));
                dtFinal.Columns.Add(columna_2, typeof(Int32));
                dtFinal.Columns.Add(columna_3, typeof(string));
                dtFinal.Columns.Add(columna_4, typeof(string));
                dtFinal.Columns.Add(columna_5, typeof(string));
                dtFinal.Columns.Add(columna_6, typeof(DateTime));
                dtFinal.Columns.Add(columna_7, typeof(string));
                dtFinal.Columns.Add(columna_8, typeof(Int32));
                dtFinal.Columns.Add(columna_9, typeof(string));
                dtFinal.Columns.Add(columna_10, typeof(string));

                if (dt.Rows.Count > 0)
                {
                    var cont = 1;
                    foreach (DataRow f in dt.Rows)
                    {
                        dtFinal.Rows.Add(cont, int.Parse(f["id_proyecto"].ToString()), f["nomproyecto"].ToString(), f["NomSector"].ToString(),
                            f["NomSubsector"].ToString(), DateTime.Parse(f["Fecha"].ToString()), f["NomUnidad"].ToString(),
                            int.Parse(f["id_convocatoria"].ToString()), f["NomConvocatoria"].ToString(), f["Sumario"].ToString().Replace(Environment.NewLine, " "));
                        cont++;
                    }

                    //var fullRuta = ConfigurationManager.AppSettings["RutaDocumentosConvocatoriasExcel"] + nomArchivo;
                    var fullRuta = ConfigurationManager.AppSettings["RutaDocumentosEnRed"]+ "\\ConvocatoriasAcreditacionExcel\\" + nomArchivo;

                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");

                    worksheet.Cell("A1").Value = columna_1;
                    worksheet.Cell("B1").Value = columna_2;
                    worksheet.Cell("C1").Value = columna_3;
                    worksheet.Cell("D1").Value = columna_4;
                    worksheet.Cell("E1").Value = columna_5;
                    worksheet.Cell("F1").Value = columna_6;
                    worksheet.Cell("G1").Value = columna_7;
                    worksheet.Cell("H1").Value = columna_8;
                    worksheet.Cell("I1").Value = columna_9;
                    worksheet.Cell("J1").Value = columna_10;

                    worksheet.Cell("A2").Value = dtFinal;

                    //worksheet.RangeUsed().SetAutoFilter();

                    workbook.SaveAs(fullRuta);

                    //if (File.Exists(fullRuta))
                    //{
                    //    File.Delete(fullRuta);
                    //}

                    //var ds = new DataSet();
                    //ds.Tables.Add(dtFinal);

                    ////Crear archivo excel
                    //StreamWriter wr = new StreamWriter(@fullRuta, false, Encoding.Unicode);


                    //for (int i = 0; i < dtFinal.Columns.Count; i++)
                    //{
                    //    wr.Write(dtFinal.Columns[i].ToString().ToUpper() + "\t");
                    //}

                    //wr.WriteLine();

                    ////write rows to excel file
                    //for (int i = 0; i < (dtFinal.Rows.Count); i++)
                    //{
                    //    for (int j = 0; j < dtFinal.Columns.Count; j++)
                    //    {
                    //        if (dtFinal.Rows[i][j] != null)
                    //        {
                    //            wr.Write(Convert.ToString(dtFinal.Rows[i][j]) + "\t");
                    //        }
                    //        else
                    //        {
                    //            wr.Write("\t");
                    //        }
                    //    }
                    //    //go to next line
                    //    wr.WriteLine();
                    //}
                    ////close file
                    //wr.Close();


                }
            }
            catch (Exception ex)
            {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al crear el acta detalle ex :" + ex.Message + ".')", true);
            }


        }


    }
}