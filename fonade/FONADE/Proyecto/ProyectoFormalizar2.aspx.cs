using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Fonade.Negocio;
using Fonade.Account;
using Fonade.Negocio.Proyecto;

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoFormalizar2 : Negocio.Base_Page
    {
        public string codProyecto;
        public string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
        //public string directorioBase = ConfigurationManager.AppSettings.Get("RutaDocumentos");
        ValidacionCuenta validacionCuenta = new ValidacionCuenta();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lbl_Titulo.Text = void_establecerTitulo("FORMALIZAR PROYECTO");

                codProyecto = Request.QueryString["codProyecto"] == null || String.IsNullOrEmpty(Request.QueryString["codProyecto"]) ? "0" : Request.QueryString["codProyecto"].ToString();
                
                insertarDatos();
                if (!Page.IsPostBack)
                {
                    if(usuario.CodGrupo != Constantes.CONST_Emprendedor)//Se valida si el usuario es emprendedor, en caso de no serlo se redirige al Home
                    {
                        Response.Redirect(validacionCuenta.rutaHome(), true);
                    }
                    else
                    {
                        if (proyectoController.codProyectoXEmprendedor(usuario.IdContacto) == Convert.ToInt32(codProyecto))//validar emprendedor x Proyecto
                        {
                            var query = from Conv in consultas.Db.Convocatoria
                                        where
                                            Conv.FechaInicio <= DateTime.Now
                                            && Conv.FechaFin > DateTime.Now
                                            && Conv.Publicado.Equals(true)
                                        select new
                                        {
                                            Id_convoct = Conv.Id_Convocatoria,
                                            Nombre_Convocatoria = Conv.NomConvocatoria,
                                        };
                            System.Web.UI.WebControls.ListItem itemSelect = new System.Web.UI.WebControls.ListItem();
                            itemSelect.Value = "0";
                            itemSelect.Text = "Seleccione...";
                            DropDownListConvoct.Items.Add(itemSelect);
                            foreach (var itemlist in query)
                            {
                                System.Web.UI.WebControls.ListItem itemConvocatoria = new System.Web.UI.WebControls.ListItem();
                                itemConvocatoria.Value = itemlist.Id_convoct.ToString();
                                itemConvocatoria.Text = itemlist.Nombre_Convocatoria;
                                DropDownListConvoct.Items.Add(itemConvocatoria);
                            }
                            DropDownListConvoct.DataBind();
                        }
                        else
                        {
                            Response.Redirect(validacionCuenta.rutaHome(), true);
                        }
                        
                    }
                }
            }
            catch (Exception)
            { }
        }

        ProyectoController proyectoController = new ProyectoController();

        protected void VerificarConvocatoriaValida(int codigoProyecto, int codigoConvocatoria) {
            if (!Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.VerificarVersionConvocatoria(codigoConvocatoria, codigoProyecto))
            {
                throw new ApplicationException("El plan de negocio no aplica a esta convocatoria, dada la estructura con la cual fue creado.");
            }
        }

        protected void VerificarTopeConvocatoria(int codigoConvocatoria, int codigoProyecto) {

            var topeConvocatoria =  Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetTopeConvocatoria(codigoConvocatoria);

            if (topeConvocatoria != 0) {
                var salariosSolicitados = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.GetSalariosSolicitados(codigoProyecto);

                var salariosRestantesConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetPresupuestoFormalizado(codigoConvocatoria);

                if (salariosSolicitados > salariosRestantesConvocatoria)
                    throw new ApplicationException("El SENA - Fondo Emprender se permite informar que las solicitudes de recursos para esta convocatoria han llegado al 120% de la bolsa de recursos disponibles. Teniendo en cuenta lo establecido en la reglamentación del Fondo Emprender en su artículo 12 a partir de este momento no se admiten la formalización de más planes de negocios. \\n \\n Lo invitamos a presentar y formalizar su plan de negocio en las próximas convocatorias, las cuales serán informadas oportunamente. Un saludo");
            }            
        }

        protected void ButtonGuardar_Click(object sender, EventArgs e)
        {
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
               
                codProyecto = Request.QueryString["codProyecto"].ToString();
                var codigoProyecto = Convert.ToInt32(Request.QueryString["codProyecto"].ToString());

                if (!(proyectoController.codProyectoXEmprendedor(usuario.IdContacto) == codigoProyecto))//validar emprendedor x Proyecto
                    throw new ApplicationException("El proyecto no se encuentra asociado con el emprendedor.");

                var codigoConvocatoria = Convert.ToInt32(DropDownListConvoct.SelectedValue);

                VerificarConvocatoriaValida(codigoProyecto, codigoConvocatoria);

                var isFormalized = consultas.Db.ProyectoFormalizacions.Any(filter => filter.codProyecto.Equals(codigoProyecto)
                                                                                     && filter.CodConvocatoria.Equals(codigoConvocatoria));
                if (isFormalized)
                    throw new ApplicationException("El proyecto ya se encuentra formalizado con esta convocatoria y no puede ser modificado de nuevo.");

                VerificarTopeConvocatoria(codigoConvocatoria, codigoProyecto);

                Session["FormalizarProyecto"] = true;
                Session["FormalizarCodigoProyecto"] = codProyecto;
                Session["FormalizarTextoAval"] = Txt_Aval.Text;
                Session["FormalizarObservaciones"] = txt_Observaciones.Text;
                Session["FormalizarConvocatoria"] = Convert.ToInt32(DropDownListConvoct.SelectedValue);

                String url = "~/PlanDeNegocioV2/Formulacion/TerminosYCondiciones/TerminosYCondiciones.aspx";
                Response.Redirect(url, false);

                //SqlCommand cmd = new SqlCommand("MD_Insert_ProyectoFormalizar", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@CodProyecto", Convert.ToInt32(codProyecto));
                //cmd.Parameters.AddWithValue("@CONST_Inscripcion", Constantes.CONST_Inscripcion);
                //cmd.Parameters.AddWithValue("@CodUsuario", usuario.IdContacto);
                //cmd.Parameters.AddWithValue("@CONST_PlanAprobado", Constantes.CONST_PlanAprobado);
                //cmd.Parameters.AddWithValue("@AVAL", Txt_Aval.Text);
                //cmd.Parameters.AddWithValue("@Observacione", txt_Observaciones.Text);
                //cmd.Parameters.AddWithValue("@CodConvocatoriaFormal", Convert.ToInt32(DropDownListConvoct.SelectedValue));
                //cmd.Parameters.AddWithValue("@CONST_Convocatoria", Constantes.CONST_Convocatoria);
                //con.Open();
                //cmd.ExecuteReader();

                //cmd.Dispose();
                //generarAnexos();
                //Response.Redirect("ProyectoFormalizar.aspx");
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", @"alert(""" + ex.Message + @""");", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error : " + ex.Message + " ');", true);
            }
            finally {
                //con.Close();
                //con.Dispose();
            }
        }
        protected void lds_proyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                int casoAsesor = 2;
                var query = from P in consultas.VerFormalizacion(Convert.ToInt32(codProyecto), Constantes.CONST_RolAsesor, Constantes.CONST_RolAsesorLider, Constantes.CONST_RolEmprendedor, casoAsesor)
                            select P;
                e.Result = query;

            }
            catch (Exception)
            { }

        }

        protected void lds_proyectos_SelectingEmprend(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                lds_proyectos.Dispose();
                var query = from P in consultas.VerFormalizacionEmprendedor(Convert.ToInt32(codProyecto), Constantes.CONST_RolEmprendedor)
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }

        protected void lds_proyectos_SelectingEmpleo(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                lds_proyectos.Dispose();
                var query = from P in consultas.VerFormalizacionEmpleos(Convert.ToInt32(codProyecto))
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }
        private void insertarDatos()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {                
                SqlCommand cmd = new SqlCommand("MD_VerFormalizacionQuerys", con);
                SqlDataReader r;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_proyecto", Convert.ToInt32(codProyecto));
                cmd.Parameters.AddWithValue("@CONST_RolAsesor", Constantes.CONST_RolAsesor);
                cmd.Parameters.AddWithValue("@CONST_RolAsesorLider", Constantes.CONST_RolAsesorLider);
                cmd.Parameters.AddWithValue("@CONST_RolEmprendedor", Constantes.CONST_RolEmprendedor);
                cmd.Parameters.AddWithValue("@caso", 1);
                con.Open();
                r = cmd.ExecuteReader();
                r.Read();
                LabelCiudad.Text = Convert.ToString(r["nomCiudadQ1"]);
                LabelEstado.Text = Convert.ToString(r["nomEstadoQ1"]);
                LabelFechaCreac.Text = Convert.ToDateTime(r["FechaCreacionQ1"]).ToString("dd 'de' MMMM 'de' yyyy");
                LabelID.Text = Convert.ToString(r["idproyectoQ1"]);
                LabelNombre.Text = Convert.ToString(r["nomProyectoQ1"]);
                LabelSector.Text = Convert.ToString(r["nomSectorQ1"]);
                LabelSumario.Text = Convert.ToString(r["sumarioQ1"]);
                LabelTipoP.Text = Convert.ToString(r["nomTipoProyectoQ1"]);
                LabelIdent2.Text = Convert.ToString(r["nomTipoIdentificacionQ1"] + " N° " + r["IdentificacionQ1"]);
                LabelInsti2.Text = Convert.ToString(r["nomInstitucionQ1"]);
                LabelJefe2.Text = Convert.ToString(r["nombresQ1"] + " " + r["apellidosQ1"]);
                LabelUnidad2.Text = Convert.ToString(r["nomUnidadQ1"]);
                LabelCluster3.Text = Convert.ToString(r["ClusterQ1"]);
                LabelPN3.Text = Convert.ToString(r["PlanNacionalQ1"]);
                LabelPR3.Text = Convert.ToString(r["PlanRegionalQ1"]);

                cmd.Dispose();
            }
            catch (Exception ex) {}
            finally {
                con.Close();
                con.Dispose();
            }
        }

        protected void BotonAnexos_Click(object sender, EventArgs e)
        {            
            try
            {
                var codigoProyecto = Convert.ToInt32(Request.QueryString["codProyecto"].ToString());
                var codigoConvocatoria = Convert.ToInt32(DropDownListConvoct.SelectedValue);

                var isFormalized = consultas.Db.ProyectoFormalizacions.Any(filter => filter.codProyecto.Equals(codigoProyecto)
                                                                                     && filter.CodConvocatoria.Equals(codigoConvocatoria));
                if (isFormalized)
                    throw new ApplicationException("El proyecto ya se encuentra formalizado con esta convocatoria y sus anexos no pueden ser modificados.");

                VerificarTopeConvocatoria(codigoConvocatoria, codigoProyecto);

                generarAnexos();

                HttpContext.Current.Session["CodProyecto"] = codProyecto;
                HttpContext.Current.Session["Accion"] = "Acreditacion";
                Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al ver el listado de anexos, intentelo de nuevo."+ ex.Message +  " ');", true);
            }            
        }

        void generarAnexos()
        { 
                string codigoEmprendedor = "";
                string strSQL = "SELECT     c.*, gc.CodGrupo, ci.NomCiudad";
                strSQL = strSQL + " FROM         Contacto AS c INNER JOIN";
                strSQL = strSQL + " ProyectoContacto AS pc ON c.Id_Contacto = pc.CodContacto INNER JOIN";
                strSQL = strSQL + " GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto and gc.CodGrupo="+Constantes.CONST_Emprendedor ;
                strSQL = strSQL + " Left join Ciudad ci ON c.LugarExpedicionDI = ci.Id_Ciudad";
                strSQL = strSQL + " WHERE pc.inactivo=0  and  (pc.CodProyecto = '" + codProyecto + "')";
                Consultas consulta = new Consultas();
                var resul = consulta.ObtenerDataTable(strSQL, "text");

                codigoEmprendedor = resul.Rows[0].ItemArray[0].ToString();                
                
                try
                {
                    anexo1(codigoEmprendedor);
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
                        anexo2(codigoEmprendedores,index);
                        index = index == null ? 1 : index + 1;
                    }                    
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error Anexo 2, intentelo de nuevo. detalle : " + ex.Message + " ');", true); }            
                try
                {
                    anexo3(codigoEmprendedor);
                }
                catch (Exception ex)
                { 
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error Anexo 3, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
                }                
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="index">Cuando hay multiples emprendedores se crea un indices por anexo</param>
        /// <returns></returns>
        String creardirectorio(string tipoAnexo, string codigoCarpeta,string codigoProyecto, string codigoUsuario, int? index = null)
        {
                string murl = directorioBase + "\\contactoAnexos\\" + codigoCarpeta + "\\ProyectoAnexo_" + codigoProyecto + "/";
               
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
                
                if(index != null){
                    string strdelete = "delete from ContactoArchivosAnexos where codproyecto = " + codProyecto + " and TipoArchivo = '" + tipoAnexo + "' and CodContacto=" + codigoUsuario;
                    ejecutaReader(strdelete, 2);
                }   else
	            {
                    string strdelete = "delete from ContactoArchivosAnexos where codproyecto = " + codProyecto + " and TipoArchivo = '" + tipoAnexo +"'";
                    ejecutaReader(strdelete, 2);
	            } 
            
                string directorioRelativo = string.Empty;
                if (index != null)
                {
                    directorioRelativo = "contactoAnexos/" + wresult.ToString() + "/ProyectoAnexo_" + codigoProyecto + "/" + tipoAnexo+index + ".pdf";
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
                ejecutaReader(strinsert , 2);
            return murl;
        }
        
        void anexo1(string codigoEmprendedor)
        {
             codProyecto = codProyecto.ToString();

            string file = "";
            
            file = creardirectorio("Anexo1", codigoEmprendedor, codProyecto,"null");            

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
        
        if (resul1.Rows.Count>0)
        {    strTitulo = resul.Rows[0].ItemArray[1].ToString();
             strinstitucion = resul.Rows[0].ItemArray[3].ToString();
             strAnoTitulo = resul.Rows[0].ItemArray[2].ToString();
             strSemestresCursados = resul.Rows[0].ItemArray[12].ToString();
             strFechainicio = resul.Rows[0].ItemArray[13].ToString();
             strFechaFinMaterias = resul.Rows[0].ItemArray[16].ToString();
             strFechagrado = resul.Rows[0].ItemArray[10].ToString();
             strFechaultimocorte = resul.Rows[0].ItemArray[11].ToString();
        
        for (x = 0; x <= resul1.Rows.Count -1; x++)
            {

                strTextoTablaTRAnexo1 = strTextoTablaTRAnexo1 + Texto("TXT_TEXTOTABLATRANEXO1");
                strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strNombreEmprendedor]", resul1.Rows[x].ItemArray[1].ToString() + " " + resul1.Rows[x].ItemArray[2].ToString()));
                strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strCedulaEmprendedor]", resul1.Rows[x].ItemArray[4].ToString()));
                strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strTitulo]", strTitulo ));
                strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strAno]", strAnoTitulo ));
                strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strSemestres]", strSemestresCursados ));
                if (strFechainicio.Trim() == "")
                {
                    strTextoTablaTRAnexo1 = (strTextoTablaTRAnexo1.Replace("[strFechaInicio]", ""));
                }
                else {
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

                if (strFechagrado.Trim() == "") {
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
            strTextoAnexo1 = (strTextoAnexo1.Replace("[strFecha]", DateTime.Now.ToString() ));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaImg]logo.jpg", strRutaImg));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[rutadoc]", strRutaDoc));
            strTextoAnexo1 = (strTextoAnexo1.Replace("[RutaDoc]estilos_generales.css", ConfigurationManager.AppSettings["RutaWebSite"] + "/Styles/estilos_generales.css"));
                    
            string html = strTextoAnexo1;

            HtmlToPdf(html, file, PageSize.LETTER, 30, 30, 30, 65,true);                   
        }

        void HtmlToPdf(string htmlAnexo, string rutaDestinoAnexo, Rectangle tamanoPagina, int margenDerecho, int margenIzquierdo, int margenTop, int margenBotton, Boolean rotate)
        {
            Byte[] bytes;
            
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document(tamanoPagina, margenDerecho, margenIzquierdo, margenTop, margenBotton))
                {
                    if(rotate)
                        doc.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {                        
                        doc.Open();
                        
                        var example_html = htmlAnexo;
                        
                        using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                        {                            
                            using (var sr = new StringReader(example_html))
                            {
                                htmlWorker.Parse(sr);
                            }
                        }
                        
                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            
            System.IO.File.WriteAllBytes(rutaDestinoAnexo, bytes);    
        }

        void anexo2(string codigoEmprendedor, int? index = null)
        {            
            codProyecto = codProyecto.ToString();

            string file = "";

            file = creardirectorio("Anexo2", codigoEmprendedor, codProyecto, "'" + codigoEmprendedor + "'",index);
           

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

            string strNombreProyecto = getNomproyecto(codProyecto);

            for (x = 0; x <= resul.Rows.Count-1; x++)
            {
                strTextoAnexo1 =  Texto("TXT_TEXTOANEXO2");
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

        void anexo3(string codigoEmprendedor){
            
            codProyecto = codProyecto.ToString();

            string file = "";            
                file = creardirectorio("Anexo3", codigoEmprendedor, codProyecto, "'" + codigoEmprendedor + "'");          

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

            string strNombreProyecto = getNomproyecto(codProyecto);

            for (x = 0; x <= resul.Rows.Count - 1; x++)
            {
                strTextoAnexo1 =  Texto("TXT_TEXTOANEXO3");
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreEmprendedor]", resul.Rows[x].ItemArray[1].ToString() + " " + resul.Rows[x].ItemArray[2].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strNombreProyecto]", strNombreProyecto));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strCedulaEmprendedor]", resul.Rows[x].ItemArray[4].ToString()));
                //strTextoAnexo1 = (strTextoAnexo1.Replace("[strCiudadCedulaEmprendedor]", resul.Rows[x].ItemArray[32].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strCiudadCedulaEmprendedor]", resul.Rows[x]["NomCiudad"].ToString()));
                strTextoAnexo1 = (strTextoAnexo1.Replace("[strFecha]", DateTime.Now.ToString() ));
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

        DataTable  getEstudios(string pCodContacto)
        {
            string lSentencia = "";

            if (pCodContacto != " ")
            {
                lSentencia = "select top 1 * from ContactoEstudio where CodContacto='" + pCodContacto + "' order by FlagIngresadoAsesor Desc, Finalizado Desc,FechaGrado Desc, fechaInicio Desc";
            }

            Consultas consulta = new Consultas();
            var resul = consulta.ObtenerDataTable(lSentencia, "text");

            return resul ;
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



       string getCiudadCedulaContacto(string pCodContacto)
       {
           string lSentencia = "";

           if (pCodContacto != " ")
           {
               lSentencia = "SELECT LugarExpedicionDI, NomCiudad FROM CONTACTO c Left JOIN CIUDAD ci ON c.LugarExpedicionDI = ci.Id_Ciudad  WHERE ID_CONTACTO=" + pCodContacto;
           }

           Consultas consulta = new Consultas();
           var resul = consulta.ObtenerDataTable(lSentencia, "text");
           String wdato = resul.Rows[0].ItemArray[0].ToString();
           return wdato;
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

       string getFechaFormalizacion(string pCodProyecto)
       {
           string lSentencia = "";

           if (pCodProyecto != " ")
           {
               lSentencia = "SELECT TOP 1 Fecha FROM ProyectoFormalizacion WHERE CodProyecto = '" + pCodProyecto + "'" ;
               lSentencia = lSentencia + " order by fecha desc ";
           }

           Consultas consulta = new Consultas();
           var resul = consulta.ObtenerDataTable(lSentencia, "text");
           String wdato = System.DateTime.Now.ToString();
           if (resul.Rows.Count != 0)
           {
               wdato = resul.Rows[0].ItemArray[0].ToString();
           }
           
           return wdato;
       }

    }
}
