
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Runtime.Caching;
using Datos;
using Fonade.Negocio;
using Fonade.Account;
using System.Data;
using Fonade.Clases;
using System.Configuration;
using Fonade.Negocio.Proyecto;

namespace Fonade
{
    public partial class Master : System.Web.UI.MasterPage
    {
        /// <summary>
        /// carga usuario
        /// </summary>
        [ContextStatic]        
        protected FonadeUser usuario;
        /// <summary>
        /// Mensaje de Error
        /// </summary>
        protected string erroMessage = string.Empty;

        /// <summary>
        /// Obtenemos información de el usuario
        /// </summary>        
        protected override void OnLoad(EventArgs e)
        {
            if (Session["usuarioLogged"] == null) { Response.Redirect("~/Account/Login.aspx"); return; }
            usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            base.OnLoad(e);
        }

        /// <summary>
        /// Generación del menu
        /// de por rol
        /// </summary>        
        void void_Menu()
        {
            try
            {
                Consultas consultas = new Consultas();
                var menuUsuario = new List<Pagina>();
                if (Session["menuUsuario"] != null)
                {
                    menuUsuario = (List<Pagina>)Session["menuUsuario"];
                    if (!menuUsuario.Any())
                    {
                        menuUsuario = (from grupoDePaginas in consultas.Db.Pagina_Grupos from paginas in consultas.Db.Paginas where grupoDePaginas.Id_Grupo == usuario.CodGrupo & paginas.Id_Pagina == grupoDePaginas.Id_Pagina & paginas.Id_Pagina != 4123 orderby paginas.Orden select (paginas)).ToList();

                        string txtSQL = "SELECT flagAcreditador,flagActaParcial,flagGeneraReporte FROM Contacto WHERE Id_Contacto =" + usuario.IdContacto;

                        var Flag = consultas.ObtenerDataTable(txtSQL, "text");

                        #region Acreditador Funcion quemada
                        if (Flag.Rows.Count > 0)
                        {
                            var Acreditador = Flag.Rows[0]["flagAcreditador"] != null && !string.IsNullOrEmpty(Flag.Rows[0]["flagAcreditador"].ToString()) ? bool.Parse(Flag.Rows[0]["flagAcreditador"].ToString()) : false;
                            var ActaParcial = Flag.Rows[0]["flagActaParcial"] != null && !string.IsNullOrEmpty(Flag.Rows[0]["flagActaParcial"].ToString()) ? bool.Parse(Flag.Rows[0]["flagActaParcial"].ToString()) : false;
                            var GenerarReporte = Flag.Rows[0]["flagGeneraReporte"] != null && !string.IsNullOrEmpty(Flag.Rows[0]["flagGeneraReporte"].ToString()) ? bool.Parse(Flag.Rows[0]["flagGeneraReporte"].ToString()) : false;

                            if (Acreditador)
                            {
                                Pagina pagina = new Pagina();
                                pagina.Titulo = "Mis Planes De Negocio A Acreditar";
                                pagina.url_Pagina = "/Fonade/Interventoria/PlanesaAcreditar.aspx";
                                menuUsuario.Add(pagina);
                            }

                            if (ActaParcial)
                            {
                                Pagina pagina = new Pagina();
                                pagina.Titulo = "Crear Acta Parcial De Acreditacion";
                                pagina.url_Pagina = "/Fonade/Administracion/AcreditacionActa.aspx";
                                menuUsuario.Add(pagina);
                            }

                            if (GenerarReporte)
                            {
                                Pagina pagina = new Pagina();
                                pagina.Titulo = "Reporte Final Acreditación";
                                pagina.url_Pagina = "/Fonade/evaluacion/ActasReporteFinalAcreditacion.aspx";
                                menuUsuario.Add(pagina);
                            }
                        }
                        #endregion
                        Session["menuUsuario"] = menuUsuario;
                    }
                }
                else
                {
                    menuUsuario = (from grupoDePaginas in consultas.Db.Pagina_Grupos from paginas in consultas.Db.Paginas where grupoDePaginas.Id_Grupo == usuario.CodGrupo & paginas.Id_Pagina == grupoDePaginas.Id_Pagina & paginas.Id_Pagina != 4123 orderby paginas.Orden select (paginas)).ToList();

                    string txtSQL = "SELECT flagAcreditador,flagActaParcial,flagGeneraReporte FROM Contacto WHERE Id_Contacto =" + usuario.IdContacto;

                    var Flag = consultas.ObtenerDataTable(txtSQL, "text");

                    if (Flag.Rows.Count > 0)
                    {
                        var Acreditador = Flag.Rows[0]["flagAcreditador"] != null && !string.IsNullOrEmpty(Flag.Rows[0]["flagAcreditador"].ToString()) ? bool.Parse(Flag.Rows[0]["flagAcreditador"].ToString()) : false;
                        var ActaParcial = Flag.Rows[0]["flagActaParcial"] != null && !string.IsNullOrEmpty(Flag.Rows[0]["flagActaParcial"].ToString()) ? bool.Parse(Flag.Rows[0]["flagActaParcial"].ToString()) : false;
                        var GenerarReporte = Flag.Rows[0]["flagGeneraReporte"] != null && !string.IsNullOrEmpty(Flag.Rows[0]["flagGeneraReporte"].ToString()) ? bool.Parse(Flag.Rows[0]["flagGeneraReporte"].ToString()) : false;

                        #region Acreditador Menu Quemado

                        if (Acreditador)
                        {
                            Pagina pagina = new Pagina();
                            pagina.Titulo = "Mis Planes De Negocio A Acreditar";
                            pagina.url_Pagina = "/Fonade/Interventoria/PlanesaAcreditar.aspx";
                            menuUsuario.Add(pagina);
                        }

                        if (ActaParcial)
                        {
                            Pagina pagina = new Pagina();
                            pagina.Titulo = "Crear Acta Parcial De Acreditacion";
                            pagina.url_Pagina = "/Fonade/Administracion/AcreditacionActa.aspx";
                            menuUsuario.Add(pagina);
                        }

                        if (GenerarReporte)
                        {
                            Pagina pagina = new Pagina();
                            pagina.Titulo = "Reporte Final Acreditación";
                            pagina.url_Pagina = "/Fonade/evaluacion/ActasReporteFinalAcreditacion.aspx";
                            menuUsuario.Add(pagina);
                        }

                        #endregion

                    }

                    Session["menuUsuario"] = menuUsuario;
                }

                foreach (Pagina paginas in menuUsuario)
                {
                    try
                    {
                        if (paginas.Id_Pagina == 21)
                        {
                            var proyectousuario = (from proyectoContactos in consultas.Db.ProyectoContactos
                                                   join proyecto in consultas.Db.Proyecto on proyectoContactos.CodProyecto equals proyecto.Id_Proyecto
                                                   where proyectoContactos.Inactivo == false
                                                         & proyectoContactos.CodContacto == usuario.IdContacto
                                                         & proyecto.Inactivo.Equals(false)
                                                   select (proyectoContactos)
                                                   ).FirstOrDefault();
                            HttpContext.Current.Session["CodProyecto"] = proyectousuario.CodProyecto.ToString();
                        }

                        paginas.url_Pagina = paginas.url_Pagina.TrimEnd();
                    }
                    catch (Exception ex)
                    {
                        erroMessage = ex.Message;
                    }
                }

                gv_Menu.DataSource = menuUsuario;
                gv_Menu.DataBind();

                if (menuUsuario.Count() == 0)
                {
                    gv_Menu.Visible = false;
                }
                else
                {
                    gv_Menu.Visible = true;
                }
            }
            catch (Exception)
            {
                gv_Menu.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FechaSesion.Text = DateTime.Now.getFechaConFormato();
            void_Menu();

            validarDocumentosCompletos();

            if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
                ValidarInformacionLiderRegional();


        }

        private bool validarTerminosSCD(int _codUsuario)
        {
            ProyectoController proyectoController = new ProyectoController();
            return proyectoController.validarTerminosSCDProyecto(_codUsuario);
        }


        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Limpieza de variables y cookies al 
        /// cierre de sesión
        /// </summary>      
        protected void HeadLoginStatus_onloggedout(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            MemoryCache.Default.Dispose();
            Response.Cache.SetAllowResponseInBrowserHistory(true);

            try
            {
                foreach (var allCookies in Response.Cookies)
                {
                    HttpCookie responseCookie = (HttpCookie)Response.Cookies[allCookies.ToString()];
                    responseCookie.Expires = DateTime.Now.AddDays(-1);
                }
                Application["UsersOnline"] = Convert.ToInt32(Application["UsersOnline"]) - 1;
            }
            catch (InvalidOperationException ex)
            {
                errorMessage = ex.Message;
            }

            Response.Cookies[".ASPXAUTH"].Value = string.Empty;
            Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Clear();

            if (Clases.genericQueries._usr == null)
            {
                Clases.genericQueries._usr = new Dictionary<string, FonadeUser>();
            }

            Clases.genericQueries._usr.Remove(HttpContext.Current.User.Identity.Name);

            Session.Clear();
            Session.Abandon();

            FormsAuthentication.SignOut();
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        }

        /// <summary>
        /// Cierre de sesión
        /// </summary>
        protected void LoginStatus_LoggedOut(Object sender, System.EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
        }
        /// <summary>
        /// Redirección a consultas mediante click en botón de consultas.
        /// </summary>
        protected void img_BuscarConsulta_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["consultarMaster"] = txt_busqueda.Value;
            Response.Redirect("/FONADE/MiPerfil/Consultas.aspx");
        }

        /// <summary>
        /// Método para alternar si la opción seleccionada es "Agendar Tarea".
        /// </summary>
        protected void DynamicCommand_Redirect(Object sender, CommandEventArgs e)
        {
            string[] valores = e.CommandArgument.ToString().Split(';');

            if (valores[1].ToString() == "3") { HttpContext.Current.Session["Id_tareaRepeticion"] = null; }
            Response.Redirect(valores[0].ToString());
        }

        /// <summary>
        /// Valida que el emprendedor tenga todos los documentos personales como
        /// Fotocopia de cedula y al menos un certificado de estudios cargado.
        /// </summary>
        protected void validarDocumentosCompletos()
        {
            string paginaActual = string.Empty;
            try
            {
                Consultas consultas = new Consultas();
                ContentPlaceHolder contenidoMasterPage = (ContentPlaceHolder)FindControl("bodyContentPlace"); //Estructura de elementos del master.page
                paginaActual = contenidoMasterPage.Page.ToString(); //Pagina actual

                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    if (!usuario.AceptoTerminosYCondiciones)
                        throw new ApplicationException("Debe aceptar los términos y condiciones de Fondo Emprender");
                    //Consultamos los documentos de cedula y certificado de estudios
                    var documentos = (from archivosAnexos in consultas.Db.ContactoArchivosAnexos where archivosAnexos.CodContacto == usuario.IdContacto select archivosAnexos).ToList();
                    var fotocopiaCedula = documentos.FirstOrDefault(d => d.TipoArchivo == "FotocopiaDocumento");
                    var certificadoEstudio = documentos.FirstOrDefault(d => d.TipoArchivo == "CertificacionEstudios");

                    if (fotocopiaCedula == null)
                        throw new ApplicationException("Debe adjuntar fotocopia de cedula");
                    if (certificadoEstudio == null)
                        throw new ApplicationException("Debe adjuntar Certificado de estudios");

                    if(validarTerminosSCD(usuario.IdContacto))
                        throw new ApplicationException("Debe aceptar los términos y condiciones de Servicio de Certificación Digital");
                }
            }
            catch (ApplicationException ex)
            {
                //Mensaje de advertencia para que el emprendedor adjunte documentos obligatorios.        
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message + ".');", true);

                if (ex.Message == "Debe aceptar los términos y condiciones de Fondo Emprender")
                {
                    //Hacemos redirect a cargar documentos
                    if (paginaActual != "ASP.plandenegociov2_formulacion_terminosycondiciones_terminosycondiciones_aspx")
                    {
                        String url = "~/PlanDeNegocioV2/Formulacion/TerminosYCondiciones/TerminosYCondiciones.aspx";
                        Response.Redirect(url);
                    }
                }
                else if (ex.Message == "Debe aceptar los términos y condiciones de Servicio de Certificación Digital")
                {
                    //Hacemos redirect a cargar documentos
                    if (paginaActual != "ASP.plandenegociov2_ejecucion_terminosscd_terminosscd_aspx")
                    {
                        String url = "~/PlanDeNegocioV2/Ejecucion/TerminosSCD/TerminosSCD.aspx";
                        Response.Redirect(url);
                    }
                }
                else
                {
                    //Hacemos redirect a cargar documentos
                    if (paginaActual != "ASP.fonade_miperfil_miperfil_aspx")
                    {
                        String url = "/Fonade/MiPerfil/MiPerfil.aspx";
                        Response.Redirect(url);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert(' Sucedio un error inesperado, porfavor intente de nuevo. Detalle de error :" + ex.Message + ".');", true);
            }

        }

        /// <summary>
        /// Valida que el perfil de lider regional
        /// tenga toda la información de datos personales completos
        /// </summary>
        private void ValidarInformacionLiderRegional()
        {
            Consultas consultas = new Consultas();
            var contenidoMasterPage = (ContentPlaceHolder)FindControl("bodyContentPlace");
            var lider = (from c in consultas.Db.Contacto where c.Id_Contacto == usuario.IdContacto select c).FirstOrDefault();

            if (string.IsNullOrEmpty(lider.Experiencia) || lider.Dedicacion == null || string.IsNullOrEmpty(lider.HojaVida) || string.IsNullOrEmpty(lider.Intereses))
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe completar la información de su perfil!');", true);

                var paginaActual = contenidoMasterPage.Page.ToString();

                if (paginaActual != "ASP.fonade_miperfil_miperfil_aspx")
                {
                    String url = "/Fonade/MiPerfil/MiPerfil.aspx";
                    Response.Redirect(url);
                }
            }

            var estudiosLider = (from ce in consultas.Db.ContactoEstudios where ce.CodContacto == lider.Id_Contacto select ce).FirstOrDefault();

            if (estudiosLider == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe completar la información de los estudios realizados!');", true);

                var paginaActual = contenidoMasterPage.Page.ToString();

                if (paginaActual != "ASP.fonade_miperfil_miperfil_aspx")
                {
                    String url = "/Fonade/MiPerfil/MiPerfil.aspx";
                    Response.Redirect(url);
                }
            }
        }

        /// <summary>
        /// Descargar archivo
        /// </summary>
        /// <param name="path">Ruta.</param>
        public void DescargaArchivo(string path)
        {
            System.IO.FileInfo toDownload;
            toDownload = new System.IO.FileInfo(path);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
            HttpContext.Current.Response.AddHeader("Content-Length", toDownload.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.WriteFile(path);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Control lnkManuales para descargar los manuales.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkManuales_Click(object sender, EventArgs e)
        {
            try
            {
                var ruta = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + @"FonadeDocumentos\manuales\";

                switch (usuario.CodGrupo)
                {
                    case 1:
                        DescargaArchivo(ruta + Constantes.Const_ManualGerenteAdmin);
                        break;
                    case 2:
                        DescargaArchivo(ruta + Constantes.Const_ManualAdminFonade);
                        break;
                    case 4:
                        DescargaArchivo(ruta + Constantes.Const_ManualJefeUnidad);
                        break;
                    case 5:
                        DescargaArchivo(ruta + Constantes.Const_ManualAsesor);
                        break;
                    case 6:
                        DescargaArchivo(ruta + Constantes.Const_ManualEmprendedor);
                        break;
                    case 8:
                        DescargaArchivo(ruta + Constantes.Const_ManualCallCenter);
                        break;
                    case 9:
                        DescargaArchivo(ruta + Constantes.Const_ManualGerenteEval);
                        break;
                    case 10:
                        DescargaArchivo(ruta + Constantes.Const_ManualCoordEval);
                        break;
                    case 11:
                        DescargaArchivo(ruta + Constantes.Const_ManualEvaluador);
                        break;
                    case 12:
                        DescargaArchivo(ruta + Constantes.Const_ManualGerenteInter);
                        break;
                    case 13:
                        DescargaArchivo(ruta + Constantes.Const_ManualCoordInter);
                        break;
                    case 14:
                        DescargaArchivo(ruta + Constantes.Const_ManualInterventor);
                        break;
                    case 15:
                        DescargaArchivo(ruta + Constantes.Const_ManualFiducia);
                        break;
                    case 16:
                        DescargaArchivo(ruta + Constantes.Const_ManualLiderRegional);
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo obtener el manual de usuario, intentelo de nuevo !'); "+ex.Message, true);

            }
        }

    }
}