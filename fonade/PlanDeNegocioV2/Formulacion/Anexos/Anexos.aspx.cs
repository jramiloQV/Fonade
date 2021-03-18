using Datos;
using Datos.DataType;
using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using Fonade.Negocio.Mensajes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System.Configuration;
using Ionic.Zip;
using System.IO;
using Fonade.Negocio.Proyecto;

namespace Fonade.PlanDeNegocioV2.Formulacion.Anexos
{
    public partial class Anexos : System.Web.UI.Page
    {
        #region Variables

        /// <summary>
        /// Usuario logueado
        /// </summary>
        protected FonadeUser usuario;

        /// <summary>
        /// Código del proyecto
        /// </summary>
        int CodigoProyecto { get; set; }

        /// <summary>
        /// Estado del proyecto
        /// </summary>
        public int EstadoProyecto { get; set; }

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_AnexosV2; } }

        /// <summary>
        /// Identifica si el usuario logueado es miembro del equipo de proyecto
        /// </summary>
        public Boolean EsMiembro { get; set; }

        /// <summary>
        /// Identifica si un tab es realizado
        /// </summary>
        public Boolean EsRealizado { get; set; }


        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);
            }
        }

        public Boolean HabilitaBoton
        {
            get
            {
                return EsMiembro
                       && usuario.CodGrupo == Constantes.CONST_Emprendedor
                       && EstadoProyecto == Constantes.CONST_Inscripcion
                       && !ProyectoCompleto;
            }
        }

        public Boolean HabilitaBotonEvaluacion
        {
            get
            {
                return EsMiembro
                       && usuario.CodGrupo == Constantes.CONST_Emprendedor
                       && EstadoProyecto == Constantes.CONST_Evaluacion;
            }
        }

        public Boolean ProyectoCompleto;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Se almacena el usuario de la sesión
                usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);

                //Se captura el código del proyecto
                if (Request.QueryString.AllKeys.Contains("codproyecto"))
                {
                    CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());

                    //Se verifica si el usuario es miembro del proyecto y si ya se realizó el registro completo de la pestaña
                    EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);
                    EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto);


                    //Se consulta el estado del proyecto
                    EstadoProyecto = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.getEstadoProyecto(CodigoProyecto);
                    ProyectoCompleto = Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.NumerosTabsCompletos(CodigoProyecto) == 27 && EstadoProyecto == Constantes.CONST_Inscripcion;
                    //Si el rol autenticado pertenece al grupo emprendedor y el estado del proyecto es inscripción se activa la
                    //opción de anexar documentos
                    if (EsMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && EstadoProyecto == Constantes.CONST_Inscripcion && !ProyectoCompleto)
                    {
                        pnlAdicionarAnexos.Visible = true;
                        pnlAdicionalDocAcreditacion.Visible = true;
                    }

                    if (EsMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && EstadoProyecto == Constantes.CONST_Inscripcion && ProyectoCompleto)
                    {
                        errorProyectoCompleta.Visible = true;
                        errorProyectoCompletaAcreditacion.Visible = true;
                    }

                    //Si el estado del proyecto esta en inscripción no se presenta los documentos de evaluación
                    if (EstadoProyecto == Constantes.CONST_Inscripcion)
                    {
                        pnlDocumentosDeEvaluacion.Visible = false;
                    }
                    else
                    {
                        pnlDocumentosDeEvaluacion.Visible = true;
                    }

                    //Si el estado se encuentra en evaluación y el rol autenticado es emprendedor se activa la opción de anexar
                    //documentos de evaluación
                    if (EstadoProyecto == Constantes.CONST_Evaluacion)
                    {
                        tb_eval.Visible = true;

                        if (EsMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor)
                        { pnlAdicionarDocumentoEvaluacion.Visible = true; }
                    }

                    //Se cargan las grillas
                    if (!IsPostBack)
                    {
                        //validar panel de carga certificado digital autenticado
                        if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                        {
                            validarCargaCertificadoDigital(CodigoProyecto, usuario.IdContacto);
                        }
                        else
                        {
                            pnlCargarCertificadoAutenticado.Visible = false;
                        }

                        CargarGridAnexos(Constantes.CONST_Inscripcion);
                        CargarGridDocumentosEvaluacion(Constantes.CONST_Evaluacion);
                        CargarGridDocumentosAcreditacion();
                        CargarArchivosContrato(CodigoProyecto);

                    }

                    //Se almacena el nombre del archivo en una variable de sesión
                    if (Archivo.HasFile)
                    {
                        Session["NombreArchivo"] = Archivo.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        ProyectoController proyectoController = new ProyectoController();

        private void validarCargaCertificadoDigital(int _codProyecto, int _codUsuario)
        {
            pnlCargarCertificadoAutenticado.Visible = proyectoController.validarCargaCertificacionDigital(_codProyecto, _codUsuario);
        }

        protected void gw_DocumentosEvaluacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void gw_Anexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void gw_DocumentosAcreditacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void btnCrearAnexo_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnCrearAnexo.Text != "Actualizar")
                {
                    if (!Archivo.HasFile)
                    {
                        Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(146), this, "Alert");
                    }
                    else
                    {
                        if (hddIdDocumento.Value == "")
                        {
                            string txtFormato = "Link";
                            string codFormato = String.Empty;
                            string NomFormato = String.Empty;
                            string RutaHttpDestino = String.Empty;
                            int CodCarpeta = 0;

                            if (txtFormato.ToLower() == ".asp" || txtFormato.ToLower() == "php" || txtFormato.ToLower() == "xml" || txtFormato.ToLower() == "aspx" || txtFormato.ToLower() == "exe")
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(154), this, "Alert");
                            }

                            string[] extension = Archivo.PostedFile.FileName.ToString().Trim().Split('.');
                            txtFormato = "." + extension[extension.Length - 1];

                            var query = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoFormato(txtFormato);

                            if (query == null)
                            {
                                DocumentoFormato datos = new DocumentoFormato();
                                datos.NomDocumentoFormato = "Archivo " + txtFormato;
                                datos.Extension = txtFormato;
                                datos.Icono = "IcoDocNormal.gif";

                                if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.insDocumentoFormato(datos))
                                {
                                    var consulta = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoFormato(txtFormato);

                                    codFormato = consulta.Id_DocumentoFormato.ToString();
                                    NomFormato = consulta.NomDocumentoFormato;
                                }
                            }
                            else
                            {
                                codFormato = query.Id_DocumentoFormato.ToString();
                                NomFormato = query.NomDocumentoFormato;
                            }

                            string nomtab = Negocio.PlanDeNegocioV2.Utilidad.General.getNombreTab(CodigoTab);

                            CodCarpeta = Convert.ToInt32(CodigoProyecto) / 2000;
                            RutaHttpDestino = ConfigurationManager.AppSettings.Get("DirVirtual") + "FonadeDocumentos\\Anexos\\Usuario" + usuario.IdContacto + "\\" + nomtab + "\\";
                            var rutaFull = ConfigurationManager.AppSettings.Get("RutaIP") + RutaHttpDestino;

                            if (System.IO.File.Exists(rutaFull + Archivo.FileName))
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(155), this, "Alert");
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(rutaFull))
                                {
                                    System.IO.Directory.CreateDirectory(rutaFull);
                                }

                                Archivo.SaveAs(rutaFull + Archivo.FileName);
                                RutaHttpDestino = RutaHttpDestino + Archivo.FileName;


                                Documento nuevodoc = new Documento()
                                {
                                    NomDocumento = txtNombreDocumento.Text.Trim(),
                                    URL = RutaHttpDestino,
                                    Fecha = DateTime.Now,
                                    CodProyecto = CodigoProyecto,
                                    CodDocumentoFormato = byte.Parse(codFormato),
                                    CodContacto = usuario.IdContacto,
                                    Comentario = txtComentario.Text.Trim(),
                                    CodTab = (short)CodigoTab,
                                    CodEstado = (byte)EstadoProyecto
                                };

                                if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumento(nuevodoc, true, false))
                                {
                                    Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(148), this, "Alert");
                                }
                                else
                                {
                                    Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(149), this, "Alert");
                                }
                            }
                        }
                        else
                        {
                            Documento actualdoc = new Documento()
                            {
                                NomDocumento = txtNombreDocumento.Text.Trim(),
                                URL = "",
                                Comentario = txtComentario.Text.Trim(),
                                CodTab = (short)CodigoTab,
                                Id_Documento = int.Parse(hddIdDocumento.Value)
                            };

                            if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumento(actualdoc, false, false))
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(148), this, "Alert");
                            }
                            else
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(149), this, "Alert");
                            }

                            hddIdDocumento.Value = string.Empty;
                        }

                        CargarGridAnexos(Constantes.CONST_Inscripcion);
                        pnlPrincipal.Visible = true;
                        pnlCrearDocumento.Visible = false;
                    }

                }
                else
                {
                    var documento = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoNormal(Convert.ToInt32(ViewState["idDocumento2"].ToString()));

                    if (documento != null)
                    {
                        documento.NomDocumento = txtNombreDocumento.Text.Trim();
                        documento.Comentario = txtComentario.Text.Trim();

                        Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumento(documento, false, true);

                        CargarGridAnexos(Constantes.CONST_Inscripcion);
                        pnlPrincipal.Visible = true;
                        pnlCrearDocumento.Visible = false;
                        tdSubir.Visible = true;
                    }
                    else
                    {
                        Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(154), this, "Alert");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnCerrarAnexo_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlCrearDocumento.Visible = false;
            hddIdDocumento.Value = "";
        }

        protected void btnAdicionarInversion_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlCrearDocumento.Visible = true;
            btnCrearAnexo.Text = "Crear";
            txtNombreDocumento.Text = "";
            txtLink.Text = "";
            txtComentario.Text = "";
        }

        protected void Image2_Click(object sender, ImageClickEventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlCrearDocumento.Visible = true;
            btnCrearAnexo.Text = "Crear";
            txtNombreDocumento.Text = "";
            txtComentario.Text = "";
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Consulta y carga los documentos anexos en la grilla respectiva
        /// </summary>
        /// <param name="codEstado">Código de estado</param>
        protected void CargarGridAnexos(int codEstado)
        {
            //Se consulta el listado de documentos anexos
            List<GrillaDocumentos> datos = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentos(codEstado, CodigoProyecto);

            gw_Anexos.DataSource = datos;
            gw_Anexos.DataBind();
        }

        /// <summary>
        /// Consulta y carga los documentos de evaluación en la grilla respectiva
        /// </summary>
        /// <param name="codEstado">Código de estado</param>
        protected void CargarGridDocumentosEvaluacion(int codEstado)
        {
            //Se consulta el listado de documentos de evaluación
            List<GrillaDocumentos> datos = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentos(codEstado, CodigoProyecto);

            gw_DocumentosEvaluacion.DataSource = datos;
            gw_DocumentosEvaluacion.DataBind();

        }

        /// <summary>
        /// Consulta y carga los documentos de acreditación en la grilla respectiva
        /// </summary>
        protected void CargarGridDocumentosAcreditacion()
        {
            //Se consulta el listado de documentos de acreditación
            List<GrillaDocumentos> datos = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentosAcreditacion(CodigoProyecto);

            gw_DocumentosAcreditacion.DataSource = datos;
            gw_DocumentosAcreditacion.DataBind();
        }

        /// <summary>
        /// Activa las acciones ejecutadas en los eventos del grid
        /// </summary>
        /// <param name="accion">Evento ejecutado</param>
        /// <param name="argumento">párametro enviado en la ejecución</param>
        protected void AccionGrid(string accion, string argumento)
        {
            try
            {
                int Id_Documento = -1;

                switch (accion)
                {
                    case "VerDocumento":
                        string url = ConfigurationManager.AppSettings.Get("RutaIP") + argumento;
                        Utilidades.DescargarArchivo(url);
                        break;
                    case "Editar":
                        Id_Documento = int.Parse(argumento);
                        CargarFormularioEdicion(Id_Documento);
                        break;
                    case "Borrar":
                        Id_Documento = int.Parse(argumento);
                        if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.updBorradoDocumento(Id_Documento))
                        {
                            string path = ConfigurationManager.AppSettings.Get("RutaIP") + Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoNormal(Id_Documento).URL;

                            System.IO.File.Delete(path);
                            CargarGridAnexos(Constantes.CONST_Inscripcion);
                            CargarGridDocumentosEvaluacion(Constantes.CONST_Evaluacion);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        /// <summary>
        /// Carga el formulario de edición seleccionado
        /// </summary>
        /// <param name="idDocumento">Identificador primario del documento</param>
        private void CargarFormularioEdicion(int idDocumento)
        {
            pnlPrincipal.Visible = false;
            pnlCrearDocumento.Visible = true;
            ViewState["idDocumento2"] = idDocumento;

            txtNombreDocumento.Text = "";
            txtLink.Text = "";
            txtComentario.Text = "";

            GrillaDocumentos query = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumento(idDocumento);

            txtNombreDocumento.Text = query.NombreDocumento;
            tdLink.Visible = false;
            if (query.Extension.Trim().ToLower() == "link")
            {
                txtLink.Text = query.URL;
                tdLink.Visible = true;
            }

            hddIdDocumento.Value = idDocumento.ToString();
            tdSubir.Visible = false;
            txtComentario.Text = query.Comentario;
            btnCrearAnexo.Text = "Actualizar";
        }


        #endregion

        protected void btnCrearDocumentoAcreditacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnCrearDocumentoAcreditacion.Text != "Actualizar")
                {
                    if (!flDocumentoAcreditacion.HasFile)
                    {
                        Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(146), this, "Alert");
                    }
                    else
                    {
                        if (hdIdDocAcreditacion.Value == "")
                        {
                            string txtFormato = "Link";
                            string codFormato = String.Empty;
                            string NomFormato = String.Empty;
                            string RutaHttpDestino = String.Empty;
                            string RutaDestino = String.Empty;
                            int CodCarpeta = 0;

                            if (txtFormato.ToLower() == ".asp" || txtFormato.ToLower() == "php" || txtFormato.ToLower() == "xml" || txtFormato.ToLower() == "aspx" || txtFormato.ToLower() == "exe")
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(154), this, "Alert");
                            }

                            string[] extension = flDocumentoAcreditacion.PostedFile.FileName.ToString().Trim().Split('.');
                            txtFormato = "." + extension[extension.Length - 1];

                            var query = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoFormato(txtFormato);

                            if (query == null)
                            {
                                DocumentoFormato datos = new DocumentoFormato();
                                datos.NomDocumentoFormato = "Archivo " + txtFormato;
                                datos.Extension = txtFormato;
                                datos.Icono = "IcoDocNormal.gif";

                                if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.insDocumentoFormato(datos))
                                {
                                    var consulta = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoFormato(txtFormato);

                                    codFormato = consulta.Id_DocumentoFormato.ToString();
                                    NomFormato = consulta.NomDocumentoFormato;
                                }
                            }
                            else
                            {
                                codFormato = query.Id_DocumentoFormato.ToString();
                                NomFormato = query.NomDocumentoFormato;
                            }

                            CodCarpeta = Convert.ToInt32(CodigoProyecto) / 2000;
                            RutaHttpDestino = ConfigurationManager.AppSettings.Get("DirVirtual") + "contactoAnexos\\" + CodCarpeta + "\\ContactoAnexo_" + usuario.IdContacto + "\\";
                            RutaDestino = "contactoAnexos\\" + CodCarpeta + "\\ContactoAnexo_" + usuario.IdContacto + "\\";
                            var rutaFull = ConfigurationManager.AppSettings.Get("RutaIP") + RutaHttpDestino;

                            if (System.IO.File.Exists(rutaFull + flDocumentoAcreditacion.FileName))
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(155), this, "Alert");
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(rutaFull))
                                {
                                    System.IO.Directory.CreateDirectory(rutaFull);
                                }

                                flDocumentoAcreditacion.SaveAs(rutaFull + flDocumentoAcreditacion.FileName);
                                RutaHttpDestino = RutaHttpDestino + Archivo.FileName;

                                ContactoArchivosAnexo nuevoDocAcreditacion = new ContactoArchivosAnexo()
                                {
                                    CodContacto = usuario.IdContacto,
                                    CodContactoEstudio = null,
                                    ruta = RutaDestino + flDocumentoAcreditacion.FileName,
                                    NombreArchivo = flDocumentoAcreditacion.FileName,
                                    TipoArchivo = "DocumentoAcreditacion",
                                    CodProyecto = CodigoProyecto,
                                    Bloqueado = false,
                                    Descripcion = txtNombreDocAcreditacion.Text.Trim(),
                                    Observacion = txtObservacionDocAcreditacion.Text.Trim()
                                };

                                if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumentoAcreditacion(nuevoDocAcreditacion, true, false))
                                {
                                    Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(148), this, "Alert");
                                }
                                else
                                {
                                    Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(149), this, "Alert");
                                }
                            }
                        }
                        else
                        {
                            ContactoArchivosAnexo actualdoc = new ContactoArchivosAnexo()
                            {
                                CodContacto = usuario.IdContacto,
                                CodContactoEstudio = null,
                                NombreArchivo = Archivo.FileName,
                                TipoArchivo = "DocumentoAcreditacion",
                                CodProyecto = CodigoProyecto,
                                Bloqueado = false,
                                Descripcion = txtNombreDocumento.Text.Trim(),
                                Observacion = txtObservacionDocAcreditacion.Text.Trim()
                            };

                            if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumentoAcreditacion(actualdoc, false, false))
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(148), this, "Alert");
                            }
                            else
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(149), this, "Alert");
                            }

                            hddIdDocumento.Value = string.Empty;
                        }

                        CargarGridDocumentosAcreditacion();
                        pnlPrincipal.Visible = true;
                        pnlDocumentosAcreditacion.Visible = false;
                    }

                }
                else
                {
                    var documento = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoNormalAcreditacion(Convert.ToInt32(ViewState["idDocumentoAcreditacion"].ToString()));

                    if (documento != null)
                    {
                        documento.Descripcion = txtNombreDocAcreditacion.Text.Trim();
                        documento.Observacion = txtObservacionDocAcreditacion.Text.Trim();

                        Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumentoAcreditacion(documento, false, true);

                        CargarGridDocumentosAcreditacion();
                        pnlPrincipal.Visible = true;
                        pnlDocumentosAcreditacion.Visible = false;
                        tdSubir2.Visible = true;
                    }
                    else
                    {
                        Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(154), this, "Alert");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnCancelarDocumentoAcreditacion_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlDocumentosAcreditacion.Visible = false;
            hddIdDocumento.Value = "";
        }

        protected void imgAddDocAcreditacion_Click(object sender, ImageClickEventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlDocumentosAcreditacion.Visible = true;
            btnCrearDocumentoAcreditacion.Text = "Crear";
            txtNombreDocAcreditacion.Text = "";
            txtLinkDocAcreditacion.Text = "";
            txtObservacionDocAcreditacion.Text = "";
        }

        protected void btnAddDocAcreditacion_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlDocumentosAcreditacion.Visible = true;
            btnCrearDocumentoAcreditacion.Text = "Crear";
            txtNombreDocAcreditacion.Text = "";
            txtLinkDocAcreditacion.Text = "";
            txtObservacionDocAcreditacion.Text = "";
        }

        protected void gvDocAcreditacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGridAcreditacion(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void gvFormulacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("VerArchivo"))
                {
                    if (e.CommandArgument != null)
                    {
                        string[] parametros;
                        parametros = e.CommandArgument.ToString().Split(';');

                        var nombreArchivo = parametros[0];
                        var urlArchivo = parametros[1];

                        string id = parametros[2];

                        string identificacionEmprendedor = ObtenerIdentificacionXArchivo(id);

                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "attachment; filename="+ identificacionEmprendedor+"_" + CodigoProyecto+"_"+nombreArchivo);
                        Response.TransmitFile(urlArchivo);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string ObtenerIdentificacionXArchivo(string idArchivo)
        {
            string identificacion = proyectoController.verIdentificacionXArchivo(idArchivo);
            return identificacion;
        }

        public void CargarArchivosContrato(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.ContratosArchivosAnexos
                         .Where(
                                selector =>
                                selector.CodProyecto.Equals(Convert.ToInt32(codigoProyecto)))
                         .Select(
                                filter => new SoporteHelper.Archivos.ArchivoContrato
                                {
                                    Id = filter.IdContratoArchivoAnexo,
                                    Nombre = filter.NombreArchivo,
                                    CodigoProyecto = filter.CodProyecto.GetValueOrDefault(0),
                                    Url = filter.ruta
                                }).ToList();

                gvContratos.DataSource = entities;
                gvContratos.DataBind();
            }
        }

        protected void AccionGridAcreditacion(string accion, string argumento)
        {
            try
            {
                int Id_Documento = -1;

                switch (accion)
                {
                    case "VerDocumento":
                        string url = ConfigurationManager.AppSettings.Get("RutaIP") + argumento;
                        Utilidades.DescargarArchivo(url);
                        break;
                    case "Editar":
                        Id_Documento = int.Parse(argumento);
                        CargarFormularioEdicionAcreditacion(Id_Documento);
                        break;
                    case "Borrar":
                        Id_Documento = int.Parse(argumento);
                        if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.updBorradoDocumentoAcreditacion(Id_Documento))
                        {
                            CargarGridDocumentosAcreditacion();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        private void CargarFormularioEdicionAcreditacion(int idDocumento)
        {
            pnlPrincipal.Visible = false;
            pnlDocumentosAcreditacion.Visible = true;
            ViewState["idDocumentoAcreditacion"] = idDocumento;

            txtNombreDocAcreditacion.Text = "";
            txtLinkDocAcreditacion.Text = "";
            txtObservacionDocAcreditacion.Text = "";

            GrillaDocumentos query = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoAcreditacion(idDocumento);

            txtNombreDocAcreditacion.Text = query.NombreDocumento;
            tdSubir2.Visible = false;
            if (query.Extension.Trim().ToLower() == "link")
            {
                txtLinkDocAcreditacion.Text = query.URL;
                tdSubir2.Visible = true;
            }

            hdIdDocAcreditacion.Value = idDocumento.ToString();
            tdSubir2.Visible = false;
            txtObservacionDocAcreditacion.Text = query.Comentario;
            btnCrearDocumentoAcreditacion.Text = "Actualizar";
        }

        private void DescargarAnexo(string titulo, GridView gridView)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName(titulo);
                foreach (GridViewRow row in gridView.Rows)
                {
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        string filePath = "";
                        if (gridView.ID == "gvContratos")
                        {
                            filePath = (row.FindControl("lblFilePath") as Label).Text;
                        }
                        else if (gridView.ID == "gw_DocumentosAcreditacion")
                        {
                            filePath = ConfigurationManager.AppSettings.Get("RutaIP")
                                + ConfigurationManager.AppSettings.Get("DirVirtual")
                                + (row.FindControl("lblFilePath") as Label).Text;
                        }
                        else
                        {
                            filePath = ConfigurationManager.AppSettings.Get("RutaIP") + (row.FindControl("lblFilePath") as Label).Text;
                        }

                        zip.AddFile(filePath, titulo);
                    }
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("{0}_{1}_{2}.zip", titulo, CodigoProyecto.ToString(), DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }

        protected void btnDescargarAnexos_Click(object sender, EventArgs e)
        {
            DescargarAnexo("Anexos", gw_Anexos);
        }

        protected void btnDescargarEvaluacion_Click(object sender, EventArgs e)
        {
            DescargarAnexo("Evaluacion", gw_DocumentosEvaluacion);
        }

        protected void btnDescargarContratos_Click(object sender, EventArgs e)
        {
            DescargarAnexo("Contratos", gvContratos);
        }

        protected void btnDescargarAcreditacion_Click(object sender, EventArgs e)
        {
            DescargarAnexo("Acreditacion", gw_DocumentosAcreditacion);
        }

        protected void btnCargarCertificado_Click(object sender, EventArgs e)
        {
            if (fuCargaCertificado.HasFile)
            {
                string rutaArchivo = CreateDirectory(CodigoProyecto, fuCargaCertificado);

                IngresarInfoContrato(usuario.IdContacto, CodigoProyecto, rutaArchivo);

                pnlCargarCertificadoAutenticado.Visible = false;

                CargarArchivosContrato(CodigoProyecto);
            }
            else
            {
                lblError.Text = "Debe seleccionar un archivo <br/>";
                lblError.Visible = true;
            }
        }

        public string baseDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
        public string fileName = "SolicitudCertificadoDigitalAutenticada.pdf";
        public string CreateDirectory(int codigoProyecto, FileUpload fileUpload)
        {
            string nombreFinal = codigoProyecto + "_" +usuario.IdContacto+"_"+ fileName;
            var partialDirectory = "Proyecto\\Proyecto_" + codigoProyecto + "\\";
            var finalDirectory = baseDirectory + partialDirectory;
            var virtualDirectory = partialDirectory + nombreFinal;

            if (!Directory.Exists(finalDirectory))
                Directory.CreateDirectory(finalDirectory);

            if (File.Exists(finalDirectory + nombreFinal))
                File.Delete(finalDirectory + nombreFinal);

            fileUpload.SaveAs(finalDirectory + nombreFinal);

            return ConfigurationManager.AppSettings.Get("DirVirtual") + virtualDirectory;
        }

        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        private void IngresarInfoContrato(int codigoContacto, int codigoProyecto, string rutaArchivo)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                string nombreArchivo = usuario.IdContacto + "_" + fileName;

                var currentEntity = db.ContratosArchivosAnexos
                                        .FirstOrDefault(filter => filter.CodContacto.Equals(codigoContacto)
                                                        && filter.NombreArchivo.Contains(nombreArchivo));

                if (currentEntity == null)
                {
                    var documento = new ContratosArchivosAnexo
                    {
                        CodProyecto = codigoProyecto,
                        ruta = rutaArchivo,
                        NombreArchivo = nombreArchivo,
                        CodContacto = codigoContacto,
                        FechaIngreso = DateTime.Now
                    };

                    db.ContratosArchivosAnexos.InsertOnSubmit(documento);
                }
                else
                {
                    currentEntity.CodProyecto = codigoProyecto;
                    currentEntity.ruta = rutaArchivo;
                }

                db.SubmitChanges();
            }
        }
    }
}