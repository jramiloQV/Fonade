using Datos;
using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using iTextSharp.text.html.simpleparser;
using Fonade.Clases;
using System.Data.SqlClient;
using System.Data;

namespace Fonade.PlanDeNegocioV2.Formulacion.TerminosYCondiciones
{
    public partial class TerminosYCondiciones : Negocio.Base_Page//System.Web.UI.Page
    {
        //protected FonadeUser usuario { get { return (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, false); } set { } }
        public string baseDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
        public string fileName = "TerminosYCondiciones.pdf";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bool formalizarProyecto = false;

                if (Session["FormalizarProyecto"]!=null)
                {
                    formalizarProyecto = (bool)Session["FormalizarProyecto"];
                    btnCancelarTerminos.Visible = false;
                    btnCancelarTerminosFormalizar.Visible = true;
                }
                else
                {
                    btnCancelarTerminos.Visible = true;
                    btnCancelarTerminosFormalizar.Visible = false;
                }

                if ((usuario.CodGrupo == Constantes.CONST_Emprendedor && usuario.AceptoTerminosYCondiciones) && !formalizarProyecto)
                    Response.Redirect("~/FONADE/MiPerfil/Home.aspx");

                SetData();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentalo de nuevo.";
            }
        }
        protected void SetData() {
            lblNombreEmprendedor.Text = usuario.Nombres + " " + usuario.Apellidos;
            lblCedulaEmprendedor.Text = usuario.Identificacion.ToString();
        }

        protected void btnAceptarTerminos_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkTerminos.Checked)
                    throw new ApplicationException("Debe aceptar los términos y condiciones para poder continuar.");
                
                var virtualDirectory = CreateDirectory(usuario.IdContacto);
                var htmlTerminosYCondiciones = File.ReadAllText(HttpContext.Current.Server.MapPath("TerminosYCondiciones.html")).Replace("[FECHAYHORA]",DateTime.Now.getFechaConFormato(true)).Replace("[EMPRENDEDOR]", usuario.Nombres + " " + usuario.Apellidos).Replace("[CEDULA]",usuario.Identificacion.ToString());

                HtmlToPdf(htmlTerminosYCondiciones, baseDirectory + virtualDirectory, PageSize.LEGAL, 10, 10, 30, 65, false);

                Negocio.PlanDeNegocioV2.Formulacion.TerminosYCondiciones.TerminosYCondiciones.Update(usuario.IdContacto, true);
                var codigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);

                Negocio.PlanDeNegocioV2.Formulacion.TerminosYCondiciones.TerminosYCondiciones.InsertOrUpdate(usuario.IdContacto,codigoProyecto, virtualDirectory);

                usuario.AceptoTerminosYCondiciones = true;

                bool formalizarProyecto = false;

                if (Session["FormalizarProyecto"]!=null)
                {
                    formalizarProyecto = (bool)Session["FormalizarProyecto"];
                }

                if (formalizarProyecto)
                {
                    //Formalizar el proyecto
                    FormalizarProyecto();
                }
                else
                {
                    HttpContext.Current.Session["usuarioLogged"] = usuario;
                    Response.Redirect("~/FONADE/MiPerfil/Home.aspx", false);
                }
                
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message;           
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentalo de nuevo. Detalle : " + ex.Message;
            }
        }

        private void FormalizarProyecto()
        {
            if (Session["FormalizarCodigoProyecto"] != null && Session["FormalizarTextoAval"]!=null 
                && Session["FormalizarObservaciones"] != null && Session["FormalizarConvocatoria"] != null)
            {
                int codigoProyecto = Convert.ToInt32(Session["FormalizarCodigoProyecto"].ToString());
                string txtAval = Session["FormalizarTextoAval"].ToString();
                string txtObservaciones = Session["FormalizarObservaciones"].ToString();
                int convSeleccionada = Convert.ToInt32(Session["FormalizarConvocatoria"].ToString());

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);

                SqlCommand cmd = new SqlCommand("MD_Insert_ProyectoFormalizar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodProyecto", codigoProyecto);
                cmd.Parameters.AddWithValue("@CONST_Inscripcion", Constantes.CONST_Inscripcion);
                cmd.Parameters.AddWithValue("@CodUsuario", usuario.IdContacto);
                cmd.Parameters.AddWithValue("@CONST_PlanAprobado", Constantes.CONST_PlanAprobado);
                cmd.Parameters.AddWithValue("@AVAL", txtAval);
                cmd.Parameters.AddWithValue("@Observacione", txtObservaciones);
                cmd.Parameters.AddWithValue("@CodConvocatoriaFormal", convSeleccionada);
                cmd.Parameters.AddWithValue("@CONST_Convocatoria", Constantes.CONST_Convocatoria);
                con.Open();
                cmd.ExecuteReader();

                cmd.Dispose();
                generarAnexos(codigoProyecto);
                Response.Redirect("~/FONADE/Proyecto/ProyectoFormalizar.aspx", false);
            }

            
        }

        void generarAnexos(int codProyecto)
        {
            string codigoEmprendedor = "";
            string strSQL = "SELECT     c.*, gc.CodGrupo, ci.NomCiudad";
            strSQL = strSQL + " FROM         Contacto AS c INNER JOIN";
            strSQL = strSQL + " ProyectoContacto AS pc ON c.Id_Contacto = pc.CodContacto INNER JOIN";
            strSQL = strSQL + " GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto and gc.CodGrupo=" + Constantes.CONST_Emprendedor;
            strSQL = strSQL + " Left join Ciudad ci ON c.LugarExpedicionDI = ci.Id_Ciudad";
            strSQL = strSQL + " WHERE pc.inactivo=0  and  (pc.CodProyecto = '" + codProyecto + "')";
            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(strSQL, "text");

            codigoEmprendedor = resul.Rows[0].ItemArray[0].ToString();

            try
            {
                anexo1(codigoEmprendedor, codProyecto);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error Anexo 1, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }

            try
            {
                int? index = null;
                foreach (DataRow row in resul.Rows)
                {
                    var codigoEmprendedores = row.ItemArray[0].ToString();
                    anexo2(codigoEmprendedores, codProyecto, index);
                    index = index == null ? 1 : index + 1;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error Anexo 2, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
            try
            {
                anexo3(codigoEmprendedor, codProyecto);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error Anexo 3, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        void anexo1(string codigoEmprendedor, int codProyecto)
        {
            string file = "";

            file = creardirectorio("Anexo1", codigoEmprendedor, codProyecto.ToString(), "null");

            string strTextoAnexo1 = "";
            strTextoAnexo1 = Texto("TXT_TEXTOANEXO1");
            string strSQL = "SELECT     c.*, gc.CodGrupo, ci.NomCiudad";
            strSQL = strSQL + " FROM         Contacto AS c INNER JOIN";
            strSQL = strSQL + " ProyectoContacto AS pc ON c.Id_Contacto = pc.CodContacto INNER JOIN";
            strSQL = strSQL + " GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto and gc.CodGrupo=6";
            strSQL = strSQL + " Left join Ciudad ci ON c.LugarExpedicionDI = ci.Id_Ciudad";
            strSQL = strSQL + " WHERE pc.inactivo=0  and  (pc.CodProyecto = '" + codProyecto + "')";
            string strTextoTablaTRAnexo1 = "";
            Consultas consulta = new Consultas();
            var resul1 = consulta.ObtenerDataTable(strSQL, "text");
            int x, y = 0;
            var resul = getEstudios(codigoEmprendedor);

            String strTitulo = "";
            String strinstitucion = "";
            String strAnoTitulo = "";
            String strSemestresCursados = "";
            String strFechainicio = "";
            String strFechaFinMaterias = "";
            String strFechagrado = "";
            String strFechaultimocorte = "";

            if (resul1.Rows.Count > 0)
            {
                strTitulo = resul.Rows[0].ItemArray[1].ToString();
                strinstitucion = resul.Rows[0].ItemArray[3].ToString();
                strAnoTitulo = resul.Rows[0].ItemArray[2].ToString();
                strSemestresCursados = resul.Rows[0].ItemArray[12].ToString();
                strFechainicio = resul.Rows[0].ItemArray[13].ToString();
                strFechaFinMaterias = resul.Rows[0].ItemArray[16].ToString();
                strFechagrado = resul.Rows[0].ItemArray[10].ToString();
                strFechaultimocorte = resul.Rows[0].ItemArray[11].ToString();

                for (x = 0; x <= resul1.Rows.Count - 1; x++)
                {

                    strTextoTablaTRAnexo1 = strTextoTablaTRAnexo1 + Texto("TXT_TEXTOTABLATRANEXO1");
                    strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strNombreEmprendedor]", resul1.Rows[x].ItemArray[1].ToString() + " " + resul1.Rows[x].ItemArray[2].ToString()));
                    strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strCedulaEmprendedor]", resul1.Rows[x].ItemArray[4].ToString()));
                    strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strTitulo]", strTitulo));
                    strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strAno]", strAnoTitulo));
                    strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strSemestres]", strSemestresCursados));
                    if (strFechainicio.Trim() == "")
                    {
                        strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strFechaInicio]", ""));
                    }
                    else
                    {
                        strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strFechaInicio]", Convert.ToDateTime(strFechainicio).ToString("yyyy-MM-dd")));
                    }

                    if (strFechaFinMaterias.Trim() == "")
                    {
                        strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strFechaFinMaterias]", ""));
                    }
                    else
                    {
                        strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strFechaFinMaterias]", Convert.ToDateTime(strFechaFinMaterias).ToString("yyyy-MM-dd")));

                    }

                    if (strFechagrado.Trim() == "")
                    {
                        strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strFechaGrado]", ""));
                    }
                    else
                    {
                        strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strFechaGrado]", Convert.ToDateTime(strFechagrado).ToString("yyyy-MM-dd")));
                    }

                }

            }
            string strTextoTablaAnexo1 = Texto("TXT_TEXTOTABLAANEXO1");
            strTextoTablaAnexo1 = (strTextoTablaAnexo1.Replace("[TR_EMPRENDEDORES]", strTextoTablaTRAnexo1));

            strTextoAnexo1 = (strTextoAnexo1.Replace("[TABLAEMPRENDEDORES]", strTextoTablaAnexo1));

            string strRutaImg = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

            string strRutaDoc = "http://www.fondoemprender.com/Fonade/";
            string strCodJefeUnidad = getCodJefeUnidadEmprendimiento(codProyecto.ToString().Trim());
            strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreUnidadEmprendimiento]", getNomUnidadEmprendimiento(codProyecto.ToString().Trim())));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreJefeUnidad]", getNombre(usuario.IdContacto.ToString())));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[strCedulaJefeUnidad]", getIdentificacion(usuario.IdContacto.ToString())));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreProyecto]", getNomproyecto(codProyecto.ToString().Trim())));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[CodProyecto]", codProyecto.ToString().Trim()));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[strFecha]", DateTime.Now.ToString()));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaImg]logo.jpg", strRutaImg));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[rutadoc]", strRutaDoc));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaDoc]estilos_generales.css", ConfigurationManager.AppSettings["RutaWebSite"] + "/Styles/estilos_generales.css"));

            string html = strTextoAnexo1;

            HtmlToPdf(html, file, PageSize.LETTER, 30, 30, 30, 65, true);
        }

        void anexo2(string codigoEmprendedor, int codProyecto, int? index = null)
        {
           

            string file = "";

            file = creardirectorio("Anexo2", codigoEmprendedor, codProyecto.ToString(), "'" + codigoEmprendedor + "'", index);


            string strTextoAnexo1 = "";
            strTextoAnexo1 = Texto("TXT_TEXTOANEXO2");


            string strSQL = "SELECT     c.*, gc.CodGrupo, ci.NomCiudad";
            strSQL = strSQL + " FROM         Contacto AS c INNER JOIN";
            strSQL = strSQL + " ProyectoContacto AS pc ON c.Id_Contacto = pc.CodContacto INNER JOIN";
            strSQL = strSQL + " GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto and gc.CodGrupo=6";
            strSQL = strSQL + " Left join Ciudad ci ON c.LugarExpedicionDI = ci.Id_Ciudad";
            strSQL = strSQL + " WHERE pc.inactivo=0  and  (pc.CodProyecto = '" + codProyecto + "') and pc.codcontacto = " + codigoEmprendedor;
            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(strSQL, "text");
            int x, y = 0;

            string strNombreProyecto = getNomproyecto(codProyecto.ToString());

            for (x = 0; x <= resul.Rows.Count - 1; x++)
            {
                strTextoAnexo1 = Texto("TXT_TEXTOANEXO2");
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreEmprendedor]", resul.Rows[x].ItemArray[1].ToString() + " " + resul.Rows[x].ItemArray[2].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreProyecto]", strNombreProyecto));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strCedulaEmprendedor]", resul.Rows[x].ItemArray[4].ToString()));
                //strTextoAnexo1 = (strTextoAnexo1.Replace("[strCiudadCedulaEmprendedor]", resul.Rows[x].ItemArray[32].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strCiudadCedulaEmprendedor]", resul.Rows[x]["NomCiudad"].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strFecha]", DateTime.Now.ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[CodProyecto]", codProyecto.ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaDoc]estilos_generales.css", ConfigurationManager.AppSettings["RutaWebSite"] + "/Styles/estilos_generales.css"));
            }

            string strRutaImg = "http://www.fondoemprender.com/Fonade/" + "g/";
            strRutaImg = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

            string strRutaDoc = "http://www.fondoemprender.com/Fonade/";
            strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaImg]logo.jpg", strRutaImg));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[rutadoc]", strRutaDoc));

            string html = strTextoAnexo1;

            HtmlToPdf(html, file, PageSize.LEGAL, 10, 10, 30, 65, false);
        }

        void anexo3(string codigoEmprendedor, int codProyecto)
        {

            string file = "";
            file = creardirectorio("Anexo3", codigoEmprendedor, codProyecto.ToString(), "'" + codigoEmprendedor + "'");

            string strTextoAnexo1 = "";
            strTextoAnexo1 = Texto("TXT_TEXTOANEXO3");

            string strSQL = "SELECT     c.*, gc.CodGrupo, ci.NomCiudad";
            strSQL = strSQL + " FROM         Contacto AS c INNER JOIN";
            strSQL = strSQL + " ProyectoContacto AS pc ON c.Id_Contacto = pc.CodContacto INNER JOIN";
            strSQL = strSQL + " GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto and gc.CodGrupo=6";
            strSQL = strSQL + " Left join Ciudad ci ON c.LugarExpedicionDI = ci.Id_Ciudad";
            strSQL = strSQL + " WHERE pc.inactivo=0  and  (pc.CodProyecto = '" + codProyecto + "')";
            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(strSQL, "text");
            int x = 0;

            string strNombreProyecto = getNomproyecto(codProyecto.ToString());

            for (x = 0; x <= resul.Rows.Count - 1; x++)
            {
                strTextoAnexo1 = Texto("TXT_TEXTOANEXO3");
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreEmprendedor]", resul.Rows[x].ItemArray[1].ToString() + " " + resul.Rows[x].ItemArray[2].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreProyecto]", strNombreProyecto));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strCedulaEmprendedor]", resul.Rows[x].ItemArray[4].ToString()));
                //strTextoAnexo1 = (strTextoAnexo1.Replace("[strCiudadCedulaEmprendedor]", resul.Rows[x].ItemArray[32].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strCiudadCedulaEmprendedor]", resul.Rows[x]["NomCiudad"].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strFecha]", DateTime.Now.ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[CodProyecto]", codProyecto.ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("h1 class=\"text_1\"", "strong"));
                strTextoAnexo1 = (strTextoAnexo1.Replace("h1", "strong"));

                strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaDoc]estilos_generales.css", ConfigurationManager.AppSettings["RutaWebSite"] + "/Styles/estilos_generales.css"));
            }

            string strRutaImg = "http://www.fondoemprender.com/Fonade/" + "g/";
            strRutaImg = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

            string strRutaDoc = "http://www.fondoemprender.com/Fonade/";
            strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaImg]logo.jpg", strRutaImg));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[rutadoc]", strRutaDoc));

            string html = strTextoAnexo1;

            HtmlToPdf(html, file, PageSize.LETTER, 10, 10, 10, 10, false);
        }

        string getNomproyecto(string pCodProyecto)
        {
            string lSentencia = "";

            if (pCodProyecto != " ")
            {
                lSentencia = "SELECT NomProyecto FROM proyecto WHERE Id_Proyecto = '" + pCodProyecto + "'";
            }

            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(lSentencia, "text");
            String wdato = resul.Rows[0].ItemArray[0].ToString();
            return wdato;
        }

        DataTable getEstudios(string pCodContacto)
        {
            string lSentencia = "";

            if (pCodContacto != " ")
            {
                lSentencia = "select top 1 * from ContactoEstudio where CodContacto='" + pCodContacto + "' order by FlagIngresadoAsesor Desc, Finalizado Desc,FechaGrado Desc, fechaInicio Desc";
            }

            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(lSentencia, "text");

            return resul;
        }

        string getIdentificacion(string pCodContacto)
        {
            string lSentencia = "";

            if (pCodContacto != " ")
            {
                lSentencia = "SELECT IDENTIFICACION FROM CONTACTO WHERE ID_CONTACTO=" + pCodContacto;
            }

            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(lSentencia, "text");
            String wdato = resul.Rows[0].ItemArray[0].ToString();
            return wdato;
        }

        string getNombre(string pCodContacto)
        {
            string lSentencia = "";

            if (pCodContacto != " ")
            {

                lSentencia = "SELECT Nombres, Apellidos FROM CONTACTO WHERE ID_CONTACTO=" + pCodContacto;

            }

            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(lSentencia, "text");
            String wdato = resul.Rows[0].ItemArray[0].ToString() + " " + resul.Rows[0].ItemArray[1].ToString();
            return wdato;
        }

        string getNomUnidadEmprendimiento(string pCodProyecto)
        {
            string lSentencia = "";
            lSentencia = "SELECT * FROM ";
            lSentencia = lSentencia + " (SELECT     c.CodInstitucion, i.NomInstitucion, i.NomUnidad ";
            lSentencia = lSentencia + " FROM         Contacto AS c INNER JOIN ";
            lSentencia = lSentencia + " ProyectoContacto AS pc ON c.Id_Contacto = pc.CodContacto ";
            lSentencia = lSentencia + " INNER JOIN Institucion i on i.Id_Institucion = c.CodInstitucion ";
            lSentencia = lSentencia + " WHERE     (pc.CodProyecto = '" + pCodProyecto + "') AND (pc.CodRol = 1) AND (pc.FechaFin IS NULL) ";
            lSentencia = lSentencia + " )TBL ";

            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(lSentencia, "text");
            String wdato = resul.Rows[0].ItemArray[2].ToString();
            return wdato;
        }

        string getCodJefeUnidadEmprendimiento(string pCodProyecto)
        {
            string lSentencia = "";

            if (pCodProyecto != " ")
            {

                lSentencia = "SELECT * FROM ";
                lSentencia = lSentencia + " (SELECT     c.CodInstitucion, i.NomInstitucion, i.NomUnidad ";
                lSentencia = lSentencia + " FROM         Contacto AS c INNER JOIN ";
                lSentencia = lSentencia + " ProyectoContacto AS pc ON c.Id_Contacto = pc.CodContacto ";
                lSentencia = lSentencia + " INNER JOIN Institucion i on i.Id_Institucion = c.CodInstitucion ";
                lSentencia = lSentencia + " WHERE     (pc.CodProyecto = '" + pCodProyecto + "') AND (pc.CodRol = 1) AND (pc.FechaFin IS NULL) ";
                lSentencia = lSentencia + " )TBL ";
                lSentencia = lSentencia + " INNER JOIN  ";
                lSentencia = lSentencia + " ( ";
                lSentencia = lSentencia + " SELECT     c.CodInstitucion, ic.FechaFin, c.Nombres, c.Apellidos, c.CodTipoIdentificacion, c.Identificacion, c.LugarExpedicionDI, ci.NomCiudad, c.Id_Contacto ";
                lSentencia = lSentencia + " FROM         Contacto AS c INNER JOIN ";
                lSentencia = lSentencia + " InstitucionContacto AS ic ON c.Id_Contacto = ic.CodContacto ";
                lSentencia = lSentencia + " LEFT JOIN Ciudad ci on ci.Id_Ciudad = c.LugarExpedicionDI ";
                lSentencia = lSentencia + " WHERE     ";
                lSentencia = lSentencia + " (ic.FechaFin IS NULL)";
                lSentencia = lSentencia + " )TBL2 ";
                lSentencia = lSentencia + " ON TBL.CodInstitucion = TBL2.CodInstitucion ";
            }

            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(lSentencia, "text");
            String wdato = resul.Rows[0].ItemArray[0].ToString();
            return wdato;

        }

        void HtmlToPdf(string htmlAnexo, string rutaDestinoAnexo, Rectangle tamanoPagina, int margenDerecho, int margenIzquierdo, int margenTop, int margenBotton, Boolean rotate)
        {
            Byte[] bytes;
            PdfWriter writer = null;
            HTMLWorker htmlWorker = null;

            try
            {                        
                using (var ms = new MemoryStream())
                {
                    using (var doc = new Document(tamanoPagina, margenDerecho, margenIzquierdo, margenTop, margenBotton))
                    {
                        if (rotate)
                            doc.SetPageSize(PageSize.LEGAL.Rotate());

                        writer = PdfWriter.GetInstance(doc, ms);
                    
                        doc.Open();

                        var example_html = htmlAnexo;
                        htmlWorker = new HTMLWorker(doc);
                        
                        using (var sr = new StringReader(example_html))
                        {
                            htmlWorker.Parse(sr);
                        }                        
                        doc.Close();                    
                    }
                    bytes = ms.ToArray();
                }

                System.IO.File.WriteAllBytes(rutaDestinoAnexo, bytes);
            }
            finally
            {
                if (writer != null) writer.Dispose();
                if (htmlWorker != null) htmlWorker.Dispose();
            }
        }

        public string CreateDirectory(int codigoUsuario) {

            int hashDirectorioUsuario = Convert.ToInt32(codigoUsuario) / 2000;
            var partialDirectory = "\\contactoAnexos\\" + hashDirectorioUsuario + "\\ContactoAnexo_" + codigoUsuario + "\\"; ;
            var finalDirectory = baseDirectory + partialDirectory;
            var virtualDirectory = partialDirectory + fileName;

            if (!Directory.Exists(finalDirectory))
                Directory.CreateDirectory(finalDirectory);

            if (File.Exists(finalDirectory + fileName))
                File.Delete(finalDirectory + fileName);

            return virtualDirectory;
        }
        public string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
        String creardirectorio(string tipoAnexo, string codigoCarpeta, string codigoProyecto
            , string codigoUsuario, int? index = null)
        {
            string murl = directorioBase + "\\contactoAnexos\\" + codigoCarpeta + "\\ProyectoAnexo_" + codigoProyecto + "/";

            int codProyecto = Convert.ToInt32(codigoProyecto);

            string codcarpetadirectory;
            int wresult = Convert.ToInt32(codigoProyecto);
            wresult = wresult / 2000;
            codcarpetadirectory = directorioBase + "\\contactoAnexos\\" + (wresult.ToString());

            if (!System.IO.Directory.Exists(codcarpetadirectory))
                System.IO.Directory.CreateDirectory(codcarpetadirectory);

            codcarpetadirectory = directorioBase + "\\contactoAnexos\\" + wresult.ToString() + "\\ProyectoAnexo_" + codigoProyecto;

            if (!System.IO.Directory.Exists(codcarpetadirectory))
                System.IO.Directory.CreateDirectory(codcarpetadirectory);

            if (index != null)
                murl = codcarpetadirectory + "//" + tipoAnexo + index + ".pdf";
            else
                murl = codcarpetadirectory + "//" + tipoAnexo + ".pdf";

            if (index != null)
            {
                string strdelete = "delete from ContactoArchivosAnexos where codproyecto = " + codProyecto + " and TipoArchivo = '" + tipoAnexo + "' and CodContacto=" + codigoUsuario;
                ejecutaReader(strdelete, 2);
            }
            else
            {
                string strdelete = "delete from ContactoArchivosAnexos where codproyecto = " + codProyecto + " and TipoArchivo = '" + tipoAnexo + "'";
                ejecutaReader(strdelete, 2);
            }

            string directorioRelativo = string.Empty;
            if (index != null)
            {
                directorioRelativo = "contactoAnexos/" + wresult.ToString() + "/ProyectoAnexo_" + codigoProyecto + "/" + tipoAnexo + index + ".pdf";
            }
            else
            {
                directorioRelativo = "contactoAnexos/" + wresult.ToString() + "/ProyectoAnexo_" + codigoProyecto + "/" + tipoAnexo + ".pdf";
            }

            //Observación : Cuando se generan los tres anexos, esos anexos van relacionados con un codigo de usuario diferente
            // Anexo 1 CodigoUsuario - null
            // Anexo 2 CodigoUsuario Codigo Jefe de unidad
            // Anexo 3 Codigo Emprendedor
            string strinsert = "insert into ContactoArchivosAnexos (CodContacto, ruta, NombreArchivo,TipoArchivo,Codproyecto) ";
            strinsert += "values(";
            strinsert += codigoUsuario + ",'" + directorioRelativo.ToString().Trim() + "','" + tipoAnexo + ".pdf" + "','" + tipoAnexo + "','" + codProyecto.ToString() + "')";
            ejecutaReader(strinsert, 2);
            return murl;
        }

    }
}