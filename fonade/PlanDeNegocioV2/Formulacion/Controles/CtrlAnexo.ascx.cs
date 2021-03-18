using Datos;
using Datos.DataType;
using Fonade.Account;
using Fonade.Clases;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles
{
    public partial class CtrlAnexo : System.Web.UI.UserControl
    {
        #region Variables

        /// <summary>
        /// Acción realizada por el usuario
        /// </summary>
        public string Accion
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("Accion") ? Request.QueryString["Accion"].ToString() : "";
            }
        }

        /// <summary>
        /// Tab que invoca el archivo anexo
        /// </summary>
        public string TabInvoca
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("TabInvoca") ? Request.QueryString["TabInvoca"].ToString() : "";
            }
        }

        /// <summary>
        /// Código Proyecto
        /// </summary>
        public int CodigoProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("CodigoProyecto") ? int.Parse(Request.QueryString["CodigoProyecto"].ToString()) : 0;
            }
        }

        /// <summary>
        /// Código Tab
        /// </summary>
        public int CodigoTab
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("CodigoTab") ? int.Parse(Request.QueryString["CodigoTab"].ToString()) : 0;
            }
        }

        /// <summary>
        /// Determina si el anexo es nuevo
        /// </summary>
        public bool EsNuevo
        {
            get
            {
                return (bool)ViewState["EsNuevo"];
            }

            set
            {
                ViewState["EsNuevo"] = value;
            }
        }

        /// <summary>
        /// Usuario logueado
        /// </summary>
        protected FonadeUser usuario;

        /// <summary>
        /// Estado del proyecto
        /// </summary>
        public int EstadoProyecto { get; set; }

        /// <summary>
        /// Identifica si el usuario logueado es miembro del equipo de proyecto
        /// </summary>
        public Boolean EsMiembro { get; set; }

        /// <summary>
        /// Permite la habilitación del botón eliminar
        /// </summary>
        public Boolean HabilitaBoton
        {
            get
            {
                return EsMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor &&
               (EstadoProyecto == Constantes.CONST_Evaluacion || EstadoProyecto == Constantes.CONST_Inscripcion);
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Se almacena el usuario de la sesión
                usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"]
                    : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);

                //Se captura el código del proyecto
                if (CodigoProyecto > 0)
                {
                    //Se consulta el estado del proyecto
                    EstadoProyecto = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.getEstadoProyecto(CodigoProyecto);

                    //Se verifica si el usuario es miembro del proyecto y si ya se realizó el registro completo de la pestaña
                    EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);

                    if (!Page.IsPostBack)
                    {
                        EjecutarAccion();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }

        }

        protected void gv_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void btn_Accion_Click(object sender, EventArgs e)
        {
            try
            {
                //Si es un registro nuevo
                if (btn_Accion.Text != "Actualizar")
                {
                    //Si no se a seleccionado un archivo
                    if (!Archivo.HasFile)
                    {
                        Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(146), this, "Alert");
                    }
                    else
                    {
                        //Determina si es actualización o creación del anexo
                        if (hddIdDocumento.Value == "")
                        {
                            string txtFormato = "Link";
                            string codFormato = String.Empty;
                            string NomFormato = String.Empty;
                            string RutaHttpDestino = String.Empty;
                            int CodCarpeta = 0;

                            //Valida que las siguientes extensiones no se suban
                            if (txtFormato.ToLower() == ".asp" || txtFormato.ToLower() == "php" || txtFormato.ToLower() == "xml" || txtFormato.ToLower() == "aspx" || txtFormato.ToLower() == "exe")
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(154), this, "Alert");
                            }

                            //Se extrae la información de la extensión del archivo. Si no existe se crea su registro en la base de datos
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

                            //Se trae el nombre de la sección escogida para generar el directorio
                            string nomtab = Negocio.PlanDeNegocioV2.Utilidad.General.getNombreTab(CodigoTab);

                            CodCarpeta = Convert.ToInt32(CodigoProyecto) / 2000;
                            RutaHttpDestino = ConfigurationManager.AppSettings.Get("DirVirtual") + "FonadeDocumentos\\Anexos\\Usuario" + usuario.IdContacto + "\\" + nomtab + "\\";
                            var rutaFull = ConfigurationManager.AppSettings.Get("RutaIP") + RutaHttpDestino;

                            //Si el archivo existe se descarta
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

                                //Se salva el archivo
                                Archivo.SaveAs(rutaFull + Archivo.FileName);
                                RutaHttpDestino = RutaHttpDestino + Archivo.FileName;

                                //Se genera el registro del documento
                                Documento nuevodoc = new Documento()
                                {
                                    NomDocumento = NomDocumento.Text.Trim(),
                                    URL = RutaHttpDestino,
                                    Fecha = DateTime.Now,
                                    CodProyecto = CodigoProyecto,
                                    CodDocumentoFormato = byte.Parse(codFormato),
                                    CodContacto = usuario.IdContacto,
                                    Comentario = Comentario.Text.Trim(),
                                    CodTab = (short)CodigoTab,
                                    CodEstado = (byte)EstadoProyecto
                                };

                                if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumento(nuevodoc, true, false))
                                {
                                    Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(148), this, "Alert");
                                    Cerrar();
                                }
                                else
                                {
                                    Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(149), this, "Alert");
                                }
                            }
                        }
                        else
                        {
                            //Se crea el registro del documento
                            Documento actualdoc = new Documento()
                            {
                                NomDocumento = NomDocumento.Text.Trim(),
                                URL = "",
                                Comentario = Comentario.Text.Trim(),
                                CodTab = (short)CodigoTab,
                                Id_Documento = int.Parse(hddIdDocumento.Value)
                            };

                            if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumento(actualdoc, false, false))
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(148), this, "Alert");
                                Cerrar();
                            }
                            else
                            {
                                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(149), this, "Alert");
                            }

                            hddIdDocumento.Value = string.Empty;
                        }


                    }
                }
                //Se actualiza el documento existente
                else
                {
                    var documento = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoNormal(Convert.ToInt32(ViewState["idDocumento2"].ToString()));

                    if (documento != null)
                    {
                        documento.NomDocumento = NomDocumento.Text.Trim();
                        documento.Comentario = Comentario.Text.Trim();

                        Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.setDocumento(documento, false, true);

                        Cerrar();
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

        protected void Btn_cerrar_Click(object sender, EventArgs e)
        {
            try
            {
                Cerrar();
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Cierra la ventana
        /// </summary>
        private void Cerrar()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;window.close();", true);
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
                        CargarDocumento(Id_Documento);
                        break;
                    case "Borrar":
                        Id_Documento = int.Parse(argumento);
                        Eliminar(Id_Documento);
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        /// <summary>
        /// Carga la información de un documento
        /// </summary>
        /// <param name="idDocumento">Identificador primario del documento</param>
        private void CargarDocumento(int idDocumento)
        {
            GrillaDocumentos doc = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumento(idDocumento);
            ViewState["idDocumento2"] = idDocumento;

            if (doc != null)
            {
                lblTitulo.Text = "EDITAR DOCUMENTO";
                NomDocumento.Text = doc.NombreDocumento;
                Comentario.Text = doc.Comentario;
                btn_Accion.Text = "Actualizar";
                EsNuevo = true;
            }
        }

        /// <summary>
        /// Desactiva un documento borrado
        /// </summary>
        /// <param name="idDocumento">Identificador primario del documento</param>
        private void Eliminar(int idDocumento)
        {
            if (Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.updBorradoDocumento(idDocumento))
            {
                string path = ConfigurationManager.AppSettings.Get("RutaIP") + Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentoNormal(idDocumento).URL;

                System.IO.File.Delete(path);

                CargarGridDocumentos(); 
            }

        }

        /// <summary>
        /// Depende de la acción realizada (nuevo anexo, ver anexo acreditación, editar) se hace visible los paneles
        /// y se realizan las respectivas consultas de documentos
        /// </summary>
        private void EjecutarAccion()
        {
            switch (Accion)
            {
                case "Nuevo":
                    EsNuevo = true;
                    LimpiarCampos();
                    break;
                case "Acreditacion":
                    EsNuevo = false;
                    CargarGridDocumentosAcreditacion();
                    break;
                default:
                    EsNuevo = false;
                    CargarGridDocumentos();
                    break;
            }
        }

        /// <summary>
        /// Limpia los campos del formulario de creación / edición del documento
        /// </summary>
        private void LimpiarCampos()
        {
            NomDocumento.Text = "";
            Comentario.Text = "";
            lblTitulo.Text = "NUEVO DOCUMENTO";
            btn_Accion.Text = "Crear";
        }

        /// <summary>
        /// Consulta y carga los documentos en la grilla respectiva
        /// </summary>
        /// <param name="codEstado">Código de estado</param>
        protected void CargarGridDocumentos()
        {
            //Se consulta el listado de documentos de evaluación
            List<GrillaDocumentos> datos = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentosEvalCtrlAnexo(TabInvoca, CodigoProyecto, CodigoTab);

            gv_Documentos.DataSource = datos;
            gv_Documentos.DataBind();

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

        #endregion
    }
}