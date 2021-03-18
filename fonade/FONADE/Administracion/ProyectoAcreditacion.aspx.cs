using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// RATProyAcreditacion
    /// </summary>    
    public partial class RATProyAcreditacion :  Negocio.Base_Page //System.Web.UI.Page
    {
        #region Variables globales.

        String txtSQL;

        #endregion

        #region Variables del formulario "ProyectoAcreditacion.asp".

        String mCodproyecto;
        String mCodConvocatoria;
        String mCodEstadoProyecto;
        //String mNuevoCodEstadoProyecto;
        //String mAsesorLider;
        //String mAsesor;
        //String mNomConvocatoria;
        String mNomProyecto;
        //String mNomCiudad;
        //String mNomUnidadEmprendimiento;
        //String mRadicacionCRIF;
        //String mAsuntoDefault;
        //String mFechaAval;
        String mSentencia;
        //String mCodActa;
        //Boolean mSoloLectura;
        //Boolean mTextoSoloLectura;

        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                mCodproyecto = HttpContext.Current.Session["ID_PROYECTOAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString()) ? HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString() : "0";
                mCodConvocatoria = HttpContext.Current.Session["CODCONVOCATORIAAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString()) ? HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString() : "0";

                #region Si las variables NO contienen valores váidos (es decir, 0), se deben obtener los valores de otras variables de sesión declaradas en "PlanesaAcreditar.aspx".

                if (mCodproyecto == "0" || mCodConvocatoria == "0")
                {
                    if (mCodproyecto == "0")
                    { mCodproyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0"; }

                    if (mCodConvocatoria == "0")
                    { mCodConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0"; }
                }

                #endregion
            }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

            ///Si después de haber obtenido los valores en las variables de sesión siguen sin datos, 
            ///debe haber un error muy oculto en el cóodigo fuente.
            if (mCodproyecto == "0" && mCodConvocatoria == "0") { }

            txtSQL = "SELECT TOP 1  CODESTADO,FECHA FROM PROYECTOACREDITACION WHERE CODPROYECTO =" + mCodproyecto + " AND CODCONVOCATORIA=" + mCodConvocatoria + " ORDER BY FECHA DESC";

            //SqlDataReader reader = ejecutaReader(txtSQL, 1);
            var td = consultas.ObtenerDataTable(txtSQL, "text");

            if (td.Rows.Count > 0)
            { mCodEstadoProyecto = td.Rows[0].ItemArray[0].ToString(); }

            //if (!IsPostBack)
            //{
            llenardivCuerpoPrograma();
            lenartablaacreditacion();
            //}

            if (mCodEstadoProyecto != null)
            {
                if (mCodEstadoProyecto.Equals("13") || mCodEstadoProyecto.Equals("14"))
                {
                    pnlestadoproyec.Visible = true;

                    txtSQL = "SELECT TOP 1 OBSERVACIONFINAL FROM PROYECTOACREDITACION WHERE  CODCONVOCATORIA = " + mCodConvocatoria + " AND CODPROYECTO = " + mCodproyecto + " AND CODESTADO= " + mCodEstadoProyecto + " AND  OBSERVACIONFINAL <> '' ORDER BY FECHA DESC";

                    //reader = ejecutaReader(txtSQL, 1);
                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt.Rows.Count > 0)
                    {
                        if (mCodEstadoProyecto.Equals("13"))
                            txtobservacionproyecto.Text = "ACREDITADO:" + dt.Rows[0].ItemArray[0].ToString();
                        else
                            txtobservacionproyecto.Text = "NO ACREDITADO:" + dt.Rows[0].ItemArray[0].ToString();
                    }
                }
                else
                    pnlestadoproyec.Visible = false;
            }
        }

        private void llenardivCuerpoPrograma()
        {
            txtSQL = "SELECT NOMCONVOCATORIA FROM CONVOCATORIA WHERE ID_CONVOCATORIA =" + mCodConvocatoria;
            pintar(mNomConvocatoria, txtSQL);


            consultas.Parameters = new[]
            {
                new SqlParameter{
                    ParameterName = "@Accion",
                    Value = "RsProyecto"
                },
                new SqlParameter{
                    ParameterName = "@Codproyecto",
                    Value = mCodproyecto
                },
                new SqlParameter{
                    ParameterName = "@CodConvocatoria",
                    Value = mCodConvocatoria
                }
            };
            var dt = consultas.ObtenerDataTable("MD_ConcultarInfoAcreditacion");

            if (dt.Rows.Count > 0)
            {
                mNomUnidadEmprendimiento.Text = dt.Rows[0]["NOMUNIDAD"].ToString() + " - " + dt.Rows[0]["NOMINSTITUCION"].ToString() + "";
                lblmNomproyecto.Text = mCodproyecto + " - " + dt.Rows[0]["NOMPROYECTO"].ToString();
                lblmNomCiudad.Text = dt.Rows[0]["NOMCIUDAD"].ToString() + " (" + dt.Rows[0]["NOMDEPARTAMENTO"].ToString() + ")";
            }

            txtSQL = "SELECT DISTINCT (NOMBRES + ' ' + APELLIDOS) 'NOMBRE' FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (PC.CODROL=1 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO) WHERE PC.CODPROYECTO =" + mCodproyecto;
            pintar(mAsesorLider, txtSQL);

            txtSQL = "SELECT DISTINCT (NOMBRES + ' ' + APELLIDOS) 'NOMBRE' FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (PC.CODROL=2 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO AND (PC.ACREDITADOR IS NULL OR PC.ACREDITADOR=0)  ) WHERE PC.CODPROYECTO =" + mCodproyecto;
            pintar(mAsesor, txtSQL);

            txtSQL = "SELECT FECHA FROM PROYECTOFORMALIZACION WHERE CODPROYECTO =" + mCodproyecto + " AND CODCONVOCATORIA=" + mCodConvocatoria;
            pintar(lblmFechaAval, txtSQL);


            if (Constantes.CONST_Asignado_para_acreditacion == 1)
                rbAsignado.Checked = true;
            if (Constantes.CONST_Pendiente == 1)
                rbPendiente.Checked = true;
            if (Constantes.CONST_Subsanado == 1)
                rbSubsanado.Checked = true;
            if (Constantes.CONST_Acreditado == 1)
                rbAcreditado.Checked = true;
            if (Constantes.CONST_No_acreditado == 1)
                rbNoAcreditado.Checked = true;
        }

        private void lenartablaacreditacion()
        {
            consultas.Parameters = new[]
            {
                new SqlParameter{
                    ParameterName = "@Accion",
                    Value = "Rsacreditacio"
                },
                new SqlParameter{
                    ParameterName = "@Codproyecto",
                    Value = mCodproyecto
                },
                new SqlParameter{
                    ParameterName = "@CodConvocatoria",
                    Value = mCodConvocatoria
                }
            };
            var dt = consultas.ObtenerDataTable("MD_ConcultarInfoAcreditacion");

            foreach (DataRow dr in dt.Rows)
            {
                TableRow fila = new TableRow();

                fila.Cells.Add(crearceldanormal((new LinkButton()
                {
                    Text = dr["NOMBRE"].ToString(),
                    CommandArgument = dr["ID_PROYECTOACREDITACIONDOCUMENTO"].ToString() + ";" + dr["CodContacto"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new ImageButton()
                {
                    ImageUrl = obtieneImagen(dr["OBSERVACIONPENDIENTE"].ToString()),
                    CommandArgument = "PENDIENTE;" + dr["PENDIENTE"].ToString() + ";" +
                                        dr["ASUNTOPENDIENTE"].ToString() + ";" +
                                        dr["OBSERVACIONPENDIENTE"].ToString() + ";" +
                                        dr["FECHAPENDIENTE"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new ImageButton()
                {
                    ImageUrl = obtieneImagen(dr["OBSERVACIONSUBSANADO"].ToString()),
                    CommandArgument = "SUBSANADO;" + dr["SUBSANADO"].ToString() + ";" +
                                        dr["ASUNTOSUBSANADO"].ToString() + ";" +
                                        dr["OBSERVACIONSUBSANADO"].ToString() + ";" +
                                        dr["FECHASUBSANADO"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new ImageButton()
                {
                    ImageUrl = obtieneImagen(dr["OBSERVACIONACREDITADO"].ToString()),
                    CommandArgument = "ACREDITADO;" + dr["ACREDITADO"].ToString() + ";" +
                                        dr["ASUNTOACREDITADO"].ToString() + ";" +
                                        dr["OBSERVACIONACREDITADO"].ToString() + ";" +
                                        dr["FECHAACREDITADO"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new ImageButton()
                {
                    ImageUrl = obtieneImagen(dr["OBSERVACIONNOACREDITADO"].ToString()),
                    CommandArgument = "NOACREDITADO;" + dr["NOACREDITADO"].ToString() + ";" +
                                        dr["ASUNTONOACREDITADO"].ToString() + ";" +
                                        dr["OBSERVACIONNOACREDITADO"].ToString() + ";" +
                                        dr["FECHANOACREDITADO"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new CheckBox()
                {
                    Checked = Convert.ToBoolean(dr["FLAGANEXO1"].ToString()),
                    AutoPostBack = true,
                    ToolTip = dr["ACREDITADO"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new CheckBox()
                {
                    Checked = Convert.ToBoolean(dr["FLAGANEXO2"].ToString()),
                    AutoPostBack = true,
                    ToolTip = dr["ACREDITADO"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new CheckBox()
                {
                    Checked = Convert.ToBoolean(dr["FLAGANEXO3"].ToString()),
                    AutoPostBack = true,
                    ToolTip = dr["ACREDITADO"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new CheckBox()
                {
                    Checked = Convert.ToBoolean(dr["FLAGDI"].ToString()),
                    AutoPostBack = true,
                    ToolTip = dr["ACREDITADO"].ToString()
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new CheckBox()
                {
                    Checked = Convert.ToBoolean(dr["FLAGCERTIFICACIONES"].ToString()),
                    AutoPostBack = true,
                    ToolTip = "FLAGCERTIFICACIONES"
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new CheckBox()
                {
                    Checked = Convert.ToBoolean(dr["FLAGDIPLOMA"].ToString()),
                    AutoPostBack = true,
                    ToolTip = "FLAGDIPLOMA"
                }), 1, 1, ""));

                fila.Cells.Add(crearceldanormal((new CheckBox()
                {
                    Checked = Convert.ToBoolean(dr["FLAGACTA"].ToString()),
                    AutoPostBack = true,
                    ToolTip = "FLAGACTA"
                }), 1, 1, ""));

                tproyectoacreditacion.Rows.Add(fila);
            }
        }

        private string obtieneImagen(string valor)
        {
            string image;
            if (string.IsNullOrEmpty(valor))
                image = "~/Images/icoSinObservacion.gif";
            else
                image = "~/Images/icoConObservacion.gif";

            return image;
        }

        private void pintar(Control control, string sqlstring)
        {
            //SqlDataReader reader;

            //reader = ejecutaReader(sqlstring, 1);
            var dt = consultas.ObtenerDataTable(sqlstring, "text");
            if (dt.Rows.Count > 0)
            {
                if (control is Label)
                    ((Label)control).Text = dt.Rows[0].ItemArray[0].ToString();
                if (control is LinkButton)
                    ((LinkButton)control).Text = dt.Rows[0].ItemArray[0].ToString();
            }
        }

        /// <summary>
        /// Handles the Click event of the mAsesorLider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mAsesorLider_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["mTipoConsulta"] = "1";
            Redirect(null, "InfoAsesor.aspx", "_Blank", "width=550,height=250");
        }

        /// <summary>
        /// Handles the Click event of the mAsesor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mAsesor_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["mTipoConsulta"] = "2";
            Redirect(null, "InfoAsesor.aspx", "_Blank", "width=550,height=250");
        }

        private TableCell crearceldanormal(string mensaje, int colspan, int rowspan, string cssestilo)
        {
            TableCell celda1 = new TableCell()
            {
                ColumnSpan = colspan,
                RowSpan = rowspan,
                CssClass = cssestilo
            };

            celda1.Controls.Add(new Label() { Text = mensaje });

            return celda1;
        }

        private TableCell crearceldanormal(Control mensaje, int colspan, int rowspan, string cssestilo)
        {
            TableCell celda1 = new TableCell()
            {
                ColumnSpan = colspan,
                RowSpan = rowspan,
                CssClass = cssestilo
            };

            if (mensaje is LinkButton)
            {
                ((LinkButton)mensaje).Click += new EventHandler(myButton_Click);
            }

            if (mensaje is CheckBox)
            {
                ((CheckBox)mensaje).CheckedChanged += new EventHandler(myCheck_CheckedChanged);
            }

            celda1.Controls.Add(mensaje);

            return celda1;
        }

        /// <summary>
        /// Handles the Click event of the myButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void myButton_Click(object sender, EventArgs e)
        {
            LinkButton boton = (LinkButton)sender;

            HttpContext.Current.Session["CodContactoEval"] = boton.CommandArgument.ToString().Split(';')[1];
            Redirect(null, "InfoEmprendedor.aspx", "_Blank", "width=450,height=670");
        }

        /// <summary>
        /// Handles the CheckedChanged event of the myCheck control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void myCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;

            if (!string.IsNullOrEmpty(checkbox.ToolTip.ToString()))
            {
                if (checkbox.ToolTip.ToString().ToLower().Equals("true"))
                {
                    checkbox.Checked = true;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No puede modificar el anexo si el emprendedor se encuentra en estado Acreditado')", true);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the lnknotificaciones control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnknotificaciones_Click(object sender, EventArgs e)
        {
            Redirect(null, "NotificacionesEnviadas.aspx", "_Blank", "width=750, height=500");
        }

        /// <summary>
        /// Handles the Click event of the lnkvertodos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkvertodos_Click(object sender, EventArgs e)
        {
            Redirect(null, "CrifIngresados.aspx", "_Blank", "width=750, height=500");
        }

        /// <summary>
        /// Handles the Click event of the btnguardar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnguardar_Click(object sender, EventArgs e)
        {
            //lenartablaacreditacion();
            foreach (TableRow fila in tproyectoacreditacion.Rows)
            {
                foreach (TableCell celda in fila.Cells)
                {
                    foreach (Control control in celda.Controls)
                    {
                        if (control is ImageButton)
                        {
                            if (((ImageButton)control).CommandArgument.ToString().Split(';')[0].Equals("PENDIENTE"))
                            {
                                if (((ImageButton)control).ImageUrl.ToString().Equals("~/Images/icoConObservacion.gif"))
                                    rbPendiente.Checked = true;
                                else
                                    rbPendiente.Checked = false;
                            }
                            if (((ImageButton)control).CommandArgument.ToString().Split(';')[0].Equals("SUBSANADO"))
                            {
                                if (((ImageButton)control).ImageUrl.ToString().Equals("~/Images/icoConObservacion.gif"))
                                    rbSubsanado.Checked = true;
                                else
                                    rbSubsanado.Checked = false;
                            }
                            if (((ImageButton)control).CommandArgument.ToString().Split(';')[0].Equals("ACREDITADO"))
                            {
                                if (((ImageButton)control).ImageUrl.ToString().Equals("~/Images/icoConObservacion.gif"))
                                    rbAcreditado.Checked = true;
                                else
                                    rbAcreditado.Checked = false;
                            }
                            if (((ImageButton)control).CommandArgument.ToString().Split(';')[0].Equals("NOACREDITADO"))
                            {
                                if (((ImageButton)control).ImageUrl.ToString().Equals("~/Images/icoConObservacion.gif"))
                                    rbNoAcreditado.Checked = true;
                                else
                                    rbNoAcreditado.Checked = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnfinalizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnfinalizar_Click(object sender, EventArgs e)
        {
            if (mCodEstadoProyecto.Equals("13") || mCodEstadoProyecto.Equals("14"))
            {
                if (mCodEstadoProyecto.Equals("13"))
                {
                    txtSQL = "UPDATE PROYECTO SET CODESTADO = 11 WHERE ID_PROYECTO=" + mCodproyecto;
                    ejecutaReader(txtSQL, 2);
                }
                else
                {
                    txtSQL = "UPDATE PROYECTO SET CODESTADO = 12 WHERE ID_PROYECTO=" + mCodproyecto;
                    ejecutaReader(txtSQL, 2);
                }

                txtSQL = "INSERT INTO PROYECTOACREDITACION (CODPROYECTO,CODCONVOCATORIA,OBSERVACIONFINAL,FECHA,CODESTADO) VALUES(" + mCodproyecto + "," + mCodConvocatoria + ",'" + txtobservacionproyecto.Text + "',Getdate()," + mCodEstadoProyecto + ")";
                ejecutaReader(txtSQL, 2);

                Response.Redirect("PlanesaAcreditar.aspx");
            }
        }

        #region /********************************************** NUEVOS MÉTODOS **************************************************/

        /************************************ NUEVOS MÉTODOS ****************************************/

        private string Guardar()
        {
            //Inicializar variables.
            String msg = "";
            String[] mNuevoCodEstadoProyecto; //Contiene los códigos de los emprendedores del plan del negocio.
            String mRadicacionCRIF;

            try
            {
                mNuevoCodEstadoProyecto = mCodEstadoProyecto.Split(';');
                mRadicacionCRIF = txtradificacion.Text.Trim();

                //Se obtiene cada uno de los códigos de los registros (Id de la tabla PROYECTOACREDITACIONDOCUMENTO)

                foreach (string lregistro in mNuevoCodEstadoProyecto)
                {

                }

                return msg;
            }
            catch (Exception ex) { msg = "Error: " + ex.Message; return msg; }
        }

        private void CargarEmprendedores()
        {
            //Inicializar variables.
            DataTable RsProyecto = new DataTable();

            try
            {
                mSentencia = " SELECT P.NOMPROYECTO,C.NOMCIUDAD,D.NOMDEPARTAMENTO, I.NOMINSTITUCION,I.NOMUNIDAD FROM PROYECTO P " +
                             " JOIN CIUDAD C ON (C.ID_CIUDAD = P.CODCIUDAD) " +
                             " JOIN DEPARTAMENTO D ON (D.ID_DEPARTAMENTO = C.CODDEPARTAMENTO) " +
                             " JOIN INSTITUCION I ON (P.CODINSTITUCION= I.ID_INSTITUCION) " +
                             " WHERE P.ID_PROYECTO =" + mCodproyecto;

                RsProyecto = consultas.ObtenerDataTable(mSentencia, "text");

                if (RsProyecto.Rows.Count > 0)
                {
                    mNomProyecto = RsProyecto.Rows[0]["NOMPROYECTO"].ToString();
                    lblmNomCiudad.Text = RsProyecto.Rows[0]["NOMCIUDAD"].ToString() + "(" + RsProyecto.Rows[0]["NOMDEPARTAMENTO"].ToString() + ")";
                    mNomUnidadEmprendimiento.Text = RsProyecto.Rows[0]["NOMUNIDAD"] + " - " + RsProyecto.Rows[0]["NOMINSTITUCION"].ToString();
                }

                RsProyecto = new DataTable();
                mAsesor.Text = getAsesor(mCodproyecto, mCodConvocatoria);
                mAsesorLider.Text = getAsesorLider(mCodproyecto, mCodConvocatoria);
                mNomConvocatoria.Text = getNomConvocatoria(mCodConvocatoria);
                lblmFechaAval.Text = getFechaAval(mCodproyecto, mCodConvocatoria).ToString("dd/MM/yyyy");

                validarPrecondiciones(mCodproyecto, mCodConvocatoria);
                //mCodEstadoProyecto = (getEstadoProyecto(mCodConvocatoria, mCodproyecto));
                //ARRAY!
            }
            catch { }
        }

        #region Grupo de métodos.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getAsesorLider
        /// </summary>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <returns>string</returns>
        private string getAsesorLider(String pCodProyecto, String pCodConvocatoria)
        {
            String lSentencia;
            String Nombre = "";
            DataTable rs = new DataTable();
            lSentencia = "SELECT DISTINCT (NOMBRES + ' ' + APELLIDOS) 'NOMBRE' FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (PC.CODROL=1 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO) WHERE PC.CODPROYECTO =" + pCodProyecto + " AND PC.CODCONVOCATORIA=" + pCodConvocatoria;

            rs = consultas.ObtenerDataTable(lSentencia, "text");
            if (rs.Rows.Count > 0) { Nombre = rs.Rows[0]["NOMBRE"].ToString(); }

            rs = null;
            return Nombre;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// eliminarProyectoDeActa
        /// </summary>
        /// <param name="mCodProyecto">mCodProyecto</param>
        /// <param name="mCodConvocatoria">mCodConvocatoria</param>
        private void eliminarProyectoDeActa(String mCodProyecto, String mCodConvocatoria)
        {
            String lSentencia;
            String lCodActa;
            lSentencia = " DELETE FROM AcreditacionActaProyecto WHERE CODPROYECTO=" + mCodProyecto +
                         " AND CODACTA in (SELECT ID_ACTA FROM AcreditacionActa WHERE CODCONVOCATORIA=" + mCodConvocatoria + ")";
            ejecutaReader(lSentencia, 2);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getAsesor
        /// </summary>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <returns>string</returns>
        private string getAsesor(String pCodProyecto, String pCodConvocatoria)
        {
            String Asesor = "";
            String lSentencia = "";
            DataTable rs = new DataTable();

            lSentencia = " SELECT DISTINCT (NOMBRES + ' ' + APELLIDOS) 'NOMBRE' " +
                         " FROM CONTACTO C JOIN PROYECTOCONTACTO PC " +
                         " ON (PC.CODROL=2 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO " +
                         " AND (PC.ACREDITADOR IS NULL OR PC.ACREDITADOR=0)  ) " +
                         " WHERE PC.CODPROYECTO =" + pCodProyecto + " AND PC.CODCONVOCATORIA=" + pCodConvocatoria;

            rs = consultas.ObtenerDataTable(lSentencia, "text");
            if (rs.Rows.Count > 0) { Asesor = rs.Rows[0]["NOMBRE"].ToString(); }

            rs = null;
            return Asesor;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getNomConvocatoria
        /// </summary>        
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <returns>string</returns>
        private string getNomConvocatoria(String pCodConvocatoria)
        {
            String NomConvocatoria = "";
            String lSentencia = "";
            DataTable rs = new DataTable();

            lSentencia = " SELECT NOMCONVOCATORIA FROM CONVOCATORIA WHERE ID_CONVOCATORIA = " + pCodConvocatoria;

            rs = consultas.ObtenerDataTable(lSentencia, "text");
            if (rs.Rows.Count > 0) { NomConvocatoria = rs.Rows[0]["NOMCONVOCATORIA"].ToString(); }

            rs = null;
            return NomConvocatoria;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getNomProyecto
        /// </summary>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <returns>string</returns>
        private string getNomProyecto(String pCodProyecto)
        {
            String NomProyecto = "";
            String lSentencia = "";
            DataTable rs = new DataTable();

            lSentencia = "SELECT NOMPROYECTO FROM PROYECTO WHERE ID_PROYECTO =" + pCodProyecto;

            rs = consultas.ObtenerDataTable(lSentencia, "text");
            if (rs.Rows.Count > 0) { NomProyecto = rs.Rows[0]["NOMPROYECTO"].ToString(); }

            rs = null;
            return NomProyecto;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getFechaAval
        /// </summary>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <returns>DateTime</returns>
        private DateTime getFechaAval(String pCodProyecto, String pCodConvocatoria)
        {
            DateTime FechaAval = new DateTime();
            String lSentencia = "";
            DataTable rs = new DataTable();
            try
            {
                lSentencia = "SELECT FECHA FROM PROYECTOFORMALIZACION WHERE CODPROYECTO =" + pCodProyecto + " AND CODCONVOCATORIA=" + pCodConvocatoria;

                rs = consultas.ObtenerDataTable(lSentencia, "text");
                if (rs.Rows.Count > 0) { FechaAval = DateTime.Parse(rs.Rows[0]["FECHA"].ToString()); }

                rs = null;
                return FechaAval;
            }
            catch { return FechaAval; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getEstadoProyectoFromProyecto
        /// </summary>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <returns>string</returns>
        private string getEstadoProyectoFromProyecto(String pCodConvocatoria, String pCodProyecto)
        {
            String lSentencia;
            String CodEstado = "";
            DataTable rs = new DataTable();
            lSentencia = "SELECT CODESTADO  FROM PROYECTO WHERE ID_PROYECTO =" + pCodProyecto;

            rs = consultas.ObtenerDataTable(lSentencia, "text");
            if (rs.Rows.Count > 0) { CodEstado = rs.Rows[0]["CODESTADO"].ToString(); }

            rs = null;
            return CodEstado;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getEstadoProyecto
        /// </summary>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <returns>string y datetime en arreglo.</returns>
        private string[] getEstadoProyecto(String pCodConvocatoria, String pCodProyecto)
        {
            String lSentencia;
            String[] arr_datos = new string[2];
            DateTime Fecha = new DateTime();
            DataTable rs = new DataTable();

            try
            {
                lSentencia = " SELECT TOP 1  CODESTADO,FECHA FROM PROYECTOACREDITACION WHERE CODPROYECTO =" + pCodProyecto +
                             " AND CODCONVOCATORIA = " + pCodConvocatoria + " ORDER BY FECHA DESC";

                rs = consultas.ObtenerDataTable(lSentencia, "text");
                if (rs.Rows.Count > 0)
                {
                    arr_datos[0] = rs.Rows[0]["CODESTADO"].ToString();
                    arr_datos[1] = DateTime.Parse(rs.Rows[0]["FECHA"].ToString()).ToString("dd/MM/yyyy");
                }

                rs = null;
                return arr_datos;
            }
            catch { return arr_datos; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getObservacion
        /// </summary>
        /// <param name="pCodproyecto">pCodproyecto</param>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <param name="pCodEstado">pCodEstado</param>
        /// <returns>string</returns>
        private string getObservacion(String pCodproyecto, String pCodConvocatoria, String pCodEstado)
        {
            String lSentencia;
            String ObservacionFinal = "";
            DataTable rs = new DataTable();
            lSentencia = " SELECT TOP 1 OBSERVACIONFINAL FROM PROYECTOACREDITACION WHERE  CODCONVOCATORIA = " + pCodConvocatoria +
                         " AND CODPROYECTO = " + pCodproyecto + " AND CODESTADO = " + pCodEstado + " AND  OBSERVACIONFINAL <> '' ORDER BY FECHA DESC";

            rs = consultas.ObtenerDataTable(lSentencia, "text");
            if (rs.Rows.Count > 0) { ObservacionFinal = rs.Rows[0]["OBSERVACIONFINAL"].ToString(); }

            rs = null;
            return ObservacionFinal;
        }

        /// <summary>
        /// NOTA: SE DEBE CREAR UN PROCEDIMIENTO ALMACENADO PARA CREAR EL REGISTRO.
        /// </summary>
        /// <param name="pCodproyecto"></param>
        /// <param name="pCodConvocatoria"></param>
        /// <param name="pCodEstado"></param>
        /// <param name="pObservacion"></param>
        private void insertarProyectoAcreditacion(String pCodproyecto, String pCodConvocatoria, String pCodEstado, String pObservacion)
        {
            //Dim lSentencia
            //pObservacion = replace(pObservacion,"'","")
            //pObservacion = left(pObservacion,1000)
            //lSentencia = "INSERT INTO PROYECTOACREDITACION (CODPROYECTO,CODCONVOCATORIA,OBSERVACIONFINAL,FECHA,CODESTADO) VALUES(@CODPROYECTO,@CODCONVOCATORIA,'"& pObservacion&"',Getdate(),"&pCodEstado&")"
            //        lSentencia = replace(lSentencia,"@CODPROYECTO",pCodProyecto)
            //        lSentencia = replace(lSentencia,"@CODCONVOCATORIA",pCodConvocatoria)
            //        Conn.Execute(lSentencia)
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// insertarCRIF
        /// </summary>
        /// <param name="pCRIF">pCRIF</param>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        private void insertarCRIF(String pCRIF, String pCodProyecto, String pCodConvocatoria)
        {
            String lSentencia = "";
            if (pCRIF.Contains("'")) { pCRIF = pCRIF.Replace("'", ""); }
            lSentencia = "IF NOT EXISTS(SELECT Id_ProyectoAcreditaciondocumentosCRIF FROM ProyectoAcreditaciondocumentosCRIF WHERE CRIF = '" + pCRIF + "' AND CODPROYECTO=" + pCodProyecto + ")" +
                         " INSERT INTO ProyectoAcreditaciondocumentosCRIF VALUES(" + pCodProyecto + "," + pCodConvocatoria + ", '" + pCRIF + "',GETDATE())";
            ejecutaReader(txtSQL, 2);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// getCRIF
        /// </summary>
        /// <param name="pCodProyecto">pCodProyecto</param>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <returns>string</returns>
        private string getCRIF(String pCodProyecto, String pCodConvocatoria)
        {
            String lSentencia;
            String CRIF = "";
            DataTable RS = new DataTable();

            lSentencia = " SELECT TOP 1 CRIF FROM ProyectoAcreditaciondocumentosCRIF WHERE CODPROYECTO= " + pCodProyecto + "AND CODCONVOCATORIA=" + pCodConvocatoria +
                         " ORDER BY FECHA DESC";

            RS = consultas.ObtenerDataTable(lSentencia, "text");
            if (RS.Rows.Count > 0) { CRIF = RS.Rows[0]["CRIF"].ToString(); }

            RS = null;
            return CRIF;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// validarPrecondiciones
        /// </summary>
        /// <param name="pCodproyecto">pCodproyecto</param>
        /// <param name="pCodConvocatoria">pCodConvocatoria</param>
        /// <returns>string con datos = redirecciona a (PlanesaAcreditar.aspx) // vacío.</returns>
        private string validarPrecondiciones(String pCodproyecto, String pCodConvocatoria)
        {
            String Redirigir = "";
            String lSentencia = "";
            DataTable lCantidad = new DataTable();

            lSentencia = "SELECT COUNT(CODCONVOCATORIA) FROM CONVOCATORIAPROYECTO WHERE CODPROYECTO=" + pCodproyecto + " AND CODCONVOCATORIA=" + pCodConvocatoria;
            lCantidad = consultas.ObtenerDataTable(lSentencia, "text");

            if (lCantidad.Rows.Count > 0)
            {
                lSentencia = "SELECT COUNT(*) AS Conteo FROM PROYECTOACREDITACIONDOCUMENTO WHERE CODPROYECTO=" + pCodproyecto + " AND CODCONVOCATORIA=" + pCodConvocatoria;
                lCantidad = consultas.ObtenerDataTable(lSentencia, "text");

                if (Int32.Parse(lCantidad.Rows[0]["Conteo"].ToString()) == 0)
                {
                    lSentencia = "INSERT INTO PROYECTOACREDITACIONDOCUMENTO (CODPROYECTO,CODCONTACTO,CODCONVOCATORIA,PENDIENTE,SUBSANADO,ACREDITADO,NOACREDITADO,FLAGANEXO1,FLAGANEXO2,FLAGANEXO3,FLAGCERTIFICACIONES,FLAGDIPLOMA,FLAGACTA) " +
                                 "SELECT " + pCodproyecto + ",CODCONTACTO," + pCodConvocatoria + ",0,0,0,0,0,0,0,0,0,0 FROM PROYECTOCONTACTO WHERE INACTIVO =0 AND CODROL=3 AND CODPROYECTO=" + pCodproyecto;

                    ejecutaReader(txtSQL, 2);

                    //Se insertan el registro en la tabla proyectoAcreditacion
                    insertarProyectoAcreditacion(pCodproyecto, pCodConvocatoria, Constantes.CONST_Asignado_para_acreditacion.ToString(), "");
                }
            }
            else
            { Redirigir = "PlanesaAcreditar.aspx"; }

            return Redirigir;
        }

        #endregion

        #endregion
    }
}