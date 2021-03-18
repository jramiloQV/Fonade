using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Ejecucion.Contrato
{
    public partial class Contrato : Negocio.Base_Page
    {
        string CodProyecto;
        string CodEmpresa;
        string CodConvocatoria;
        string anioConvocatoria;
        string[] arr_meses = { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        string txtSQL;

        public bool perfilVisibilidad
        {
            get
            {
                if (usuario == null)
                    return false;
                if (usuario.CodGrupo == Constantes.CONST_GerenteAdministrador)
                    return true;
                else
                    return false;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

            if (CodEmpresa == "0")
            {
                CodEmpresa = (from em in consultas.Db.Empresas
                              where em.codproyecto == int.Parse(CodProyecto)
                              select em.id_empresa).FirstOrDefault().ToString();
            }

            //codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodConvocatoria = Convert.ToString(HttpContext.Current.Session["CodConvocatoria"] ?? "0");

            txtSQL = "SELECT Max(CodConvocatoria) AS CodConvocatoria FROM ConvocatoriaProyecto WHERE CodProyecto = " + CodProyecto;

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
                CodConvocatoria = dt.Rows[0]["CodConvocatoria"].ToString();

            if (!string.IsNullOrEmpty(CodConvocatoria))
            {
                txtSQL = "select year(fechainicio) from convocatoria where id_Convocatoria=" + CodConvocatoria;

                dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                    anioConvocatoria = dt.Rows[0][0].ToString();
            }


            lenarInfo();
        }

        private void lenarInfo()
        {
            //Inicializar variables.
            DateTime fecha = new DateTime();
            string sMes;
            string[] palabras;

            if (!string.IsNullOrEmpty(CodEmpresa) && !string.IsNullOrEmpty(CodProyecto))
            {
                txtSQL = "SELECT a.* FROM ContratoEmpresa as a, Empresa as b where a.CodEmpresa = b.Id_Empresa and b.CodProyecto = " + CodProyecto + " and a.CodEmpresa=" + CodEmpresa;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    lblNumContrato.Text = dt.Rows[0]["NumeroContrato"].ToString();
                    lblplazoMeses.Text = dt.Rows[0]["PlazoContratoMeses"].ToString();

                    #region Fecha del Acta.
                    //Obtener fecha "del acta".
                    try
                    { fecha = Convert.ToDateTime(dt.Rows[0]["FechaDeInicioContrato"].ToString()); }
                    catch { fecha = DateTime.Today; }

                    //Cambiar fecha según FONADE clásico.
                    palabras = fecha.ToString("dd/MM/yyyy").Split('/');
                    string a = arr_meses[Int32.Parse(palabras[1])]; //Prueba, estaba en "la posición 0" y saliá error interno.
                    lblFechaActa.Text = palabras[0] + "/" + arr_meses[Int32.Parse(palabras[1])] + "/" + palabras[2];
                    #endregion

                    lblNumAppresupuestal.Text = dt.Rows[0]["NumeroAPContrato"].ToString();
                    lblObjeto.Text = dt.Rows[0]["ObjetoContrato"].ToString();

                    #region Fecha del ap.
                    //Obtener fecha "del ap".
                    try { fecha = Convert.ToDateTime(dt.Rows[0]["fechaap"].ToString()); }
                    catch { fecha = DateTime.Today; }

                    //Cambiar fecha según FONADE clásico.
                    //palabras = fecha.ToString("dd/MM/yyyy").Split('/');
                    //lblFechaAp.Text = palabras[1] + "/" + arr_meses[Int32.Parse(palabras[0])] + "/" + palabras[2];
                    sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture);
                    lblFechaAp.Text = fecha.Day + "/" + sMes + "/" + fecha.Year;
                    #endregion

                    #region Fecha de firma del contrato.
                    //Obtener fecha "de firma del contrato".
                    try { fecha = Convert.ToDateTime(dt.Rows[0]["FechaFirmaDelContrato"].ToString()); }
                    catch { fecha = DateTime.Today; }

                    //Cambiar fecha según FONADE clásico.
                    palabras = fecha.ToString("dd/MM/yyyy").Split('/');
                    lblFechaFirmaContrato.Text = palabras[0] + "/" + arr_meses[Int32.Parse(palabras[1])] + "/" + palabras[2];
                    #endregion

                    lblPolizaSeguro.Text = dt.Rows[0]["numeropoliza"].ToString();
                    lblCompaniaSeguroVida.Text = dt.Rows[0]["companiaseguros"].ToString();
                    lblValorInicial.Text = Double.Parse(dt.Rows[0]["ValorInicialEnPesos"].ToString()).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }
            }

            if (!string.IsNullOrEmpty(CodProyecto))
            {
                txtSQL = "select distinct * from ContratosArchivosAnexos where CodProyecto=" + CodProyecto;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add(new DataColumn("filePath"));
                    var xpr = System.Configuration.ConfigurationManager.AppSettings["FServerNuevo"].Replace("{0}", string.Empty);
                    dt.Columns["filePath"].Expression = string.Format("'{0}'+ruta", xpr);
                    DataList1.DataSource = dt;
                    DataList1.DataBind();
                }
            }
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
                lnkSuberArchivo.Visible = true;
                lnkSuberArchivo.Enabled = true;
            }
            else
            {
                lnkSuberArchivo.Visible = false;
                lnkSuberArchivo.Enabled = false;
            }

            //            Adjunto.Visible = false;
            //            Adjunto.Enabled = false;
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            var ygv = ((Button)sender);
            var ujm = ygv.Text;
            var wsx = ygv.CommandArgument;
            var edc = this.CodProyecto;
            txtSQL = string.Format("delete from ContratosArchivosAnexos where Ruta = '{0}' and Nombrearchivo = '{1}' and CodProyecto = {2}", wsx, ujm, edc);
            ejecutaReader(txtSQL, 2);
        }

        protected void lnkSuberArchivo_Click1(object sender, EventArgs e)
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
                panexosagre.Visible = false;
                panexosagre.Enabled = false;

                Adjunto.Visible = true;
                Adjunto.Enabled = true;
            }
            else
            {
                Adjunto.Visible = false;
                Adjunto.Enabled = false;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Adjunto.Visible = false;
            Adjunto.Enabled = false;

            panexosagre.Visible = true;
            panexosagre.Enabled = true;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/05/2014.
        /// Limitado por tamaño la carga de archivos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubirDocumento_Click(object sender, EventArgs e)
        {
            if (fuArchivo.HasFile)
            {
                if (fuArchivo.PostedFile.ContentLength > 10485760) // = 10)
                {
                    lblErrorDocumento.Visible = true;
                    lblErrorDocumento.Text = "El tamaño del archivo debe ser menor a 10 Mb.";
                    return;
                }
                else
                {
                    #region Procesar el archivo seleccionado.

                    String NombreArchivo = System.IO.Path.GetFileName(fuArchivo.PostedFile.FileName);
                    String extension = System.IO.Path.GetExtension(fuArchivo.PostedFile.FileName);

                    string saveLocation = string.Format("Documentos/Proyecto/Proyecto_{0}/{1}", CodProyecto, NombreArchivo);

                    var folder = Server.MapPath("~\\Documentos\\Proyecto\\Proyecto_" + CodProyecto);
                    if (!System.IO.Directory.Exists(folder))
                    {
                        System.IO.Directory.CreateDirectory(folder);
                    }

                    fuArchivo.SaveAs(Server.MapPath("~\\Documentos\\Proyecto\\Proyecto_" + CodProyecto + "\\") + NombreArchivo);

                    txtSQL = "insert into ContratosArchivosAnexos (CodProyecto,ruta,NombreArchivo) values (" + CodProyecto + ",'" + saveLocation + "','" + NombreArchivo + "')";

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    SqlCommand cmd = new SqlCommand(txtSQL, conn);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteReader();
                    }
                    catch (SqlException) { }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }

                    Adjunto.Visible = false;
                    Adjunto.Enabled = false;

                    panexosagre.Visible = true;
                    panexosagre.Enabled = true;

                    Response.Redirect("InterContratoFrame.aspx");

                    #endregion
                }
            }
            else
            {
                lblErrorDocumento.Visible = true;
                lblErrorDocumento.Text = "No ha seleccionado un archivo.";
                return;
            }
        }

        #region Métodos de Mauricio Arias Olave.

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

        #endregion

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}