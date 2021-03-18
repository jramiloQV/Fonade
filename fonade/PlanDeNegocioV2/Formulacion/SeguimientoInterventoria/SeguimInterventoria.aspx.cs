using Datos;
using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.SeguimientoInterventoria
{
    public partial class SeguimInterventoria : Negocio.Base_Page//: System.Web.UI.Page
    {
        public bool mostrarEliminar
        {
            get
            {
                if (usuario == null)
                    return false;
                if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                    || usuario.CodGrupo == Constantes.CONST_Interventor)
                    return true;
                else
                    return false;
            }
            set { }
        }
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        private string BaseDirectory { get { return ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "SeguiVirtualInterventoria\\"; } set { } }
        protected FonadeUser Usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                habilitarPanelInterventoria(Usuario.CodGrupo);
                habilitarDDLActasCargar();
                mostrarGrilla();
                cargarGrillas();
            }
        }

        private void cargarGrillas()
        {
            //archivos Acta 1
            gvArchivosActa1.DataSource = SeguimientoBLL.archivosActas(CodigoProyecto, 1);
            gvArchivosActa1.DataBind();
            //archivos Acta 2
            gvArchivosActa2.DataSource = SeguimientoBLL.archivosActas(CodigoProyecto, 2);
            gvArchivosActa2.DataBind();
            //archivos Acta 3
            gvArchivosActa3.DataSource = SeguimientoBLL.archivosActas(CodigoProyecto, 3);
            gvArchivosActa3.DataBind();
            //archivos Acta 4
            gvArchivosActa4.DataSource = SeguimientoBLL.archivosActas(CodigoProyecto, 4);
            gvArchivosActa4.DataBind();
            //archivos Acta 5
            gvArchivosActa5.DataSource = SeguimientoBLL.archivosActas(CodigoProyecto, 5);
            gvArchivosActa5.DataBind();
        }

        private void mostrarGrilla()
        {
            panelActa1.Visible = SeguimientoBLL.existeArchivoActa(CodigoProyecto, 1);
            panelActa2.Visible = SeguimientoBLL.existeArchivoActa(CodigoProyecto, 2);
            panelActa3.Visible = SeguimientoBLL.existeArchivoActa(CodigoProyecto, 3);
            panelActa4.Visible = SeguimientoBLL.existeArchivoActa(CodigoProyecto, 4);
            panelActa5.Visible = SeguimientoBLL.existeArchivoActa(CodigoProyecto, 5);            
        }

        private void habilitarDDLActasCargar()
        {
            List<ActasIdNomModel> opciones = new List<ActasIdNomModel>();

            opciones = SeguimientoBLL.getListActasHabilitadas(CodigoProyecto);

            ddlActaACargar.DataSource = opciones;
            ddlActaACargar.DataTextField = "NomActa"; // FieldName of Table in DataBase
            ddlActaACargar.DataValueField = "idActa";
            ddlActaACargar.DataBind();

            ddlDeshabilitarActa.DataSource = opciones;
            ddlDeshabilitarActa.DataTextField = "NomActa"; // FieldName of Table in DataBase
            ddlDeshabilitarActa.DataValueField = "idActa";
            ddlDeshabilitarActa.DataBind();
        }

        SeguimientoInterventoriaBLL SeguimientoBLL = new SeguimientoInterventoriaBLL();

        protected void btnHabilitar_Click(object sender, EventArgs e)
        {
            if (ddlHabilitarActa.SelectedValue.ToString() == "S")
            {
                Alert("Debe seleccionar un Acta");
            }
            else
            {
                //Agregar Registro en la BD
                int numActa = Convert.ToInt32(ddlHabilitarActa.SelectedValue.ToString());
                string error = "";
                if(SeguimientoBLL.agregarHabilitarActa(CodigoProyecto, Usuario.IdContacto, numActa, ref error))
                {
                    habilitarDDLActasCargar();
                    Alert("Se habilitó el cargue de archivos del acta de manera exitosa.");
                }
                else
                {
                    Alert(error);
                }
            }
        }

        private void habilitarPanelInterventoria(int _codGrupo)
        {
            if ((_codGrupo == Constantes.CONST_Interventor)
                ||(_codGrupo == Constantes.CONST_CoordinadorInterventor)
                || (_codGrupo == Constantes.CONST_GerenteInterventor)
                || (_codGrupo == Constantes.CONST_AdministradorSistema))
            {
                panelInterventor.Visible = true;
            }
            else
            {
                panelInterventor.Visible = false;
            }
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }
        
        
        protected void btnSubirArchivo_Click(object sender, EventArgs e)
        {
            if (ddlActaACargar.SelectedValue.ToString() == "S")
            {
                Alert("Debe seleccionar un Acta");
            }
            else
            {
                //Agregar Registro en la BD
                string rutaArchivo = BaseDirectory;               
                try
                {
                    int numActa = Int32.Parse(ddlActaACargar.SelectedValue.ToString());
                    if (SeguimientoBLL.archivoActaHabilitado(CodigoProyecto, numActa))
                    {                        
                        if (Archivo.HasFile)
                        {
                            if (!(Archivo.PostedFile.ContentLength < 1048576000))
                                throw new ApplicationException("El archivo es muy pesado, maximo 100 megas.");

                            //Cargar Archivo
                            string error = "";
                            if (!SeguimientoBLL.UploadFile(Archivo, rutaArchivo, CodigoProyecto
                                                        , usuario.IdContacto
                                                        , numActa, Archivo.FileName, ref error))
                                throw new ApplicationException("No se logró cargar el archivo: " + error);

                            //Ingresar ruta Archivo en BD
                            if (SeguimientoBLL.ingresarRegistroArchivo(CodigoProyecto
                                , usuario.IdContacto, Archivo.FileName
                                , numActa, ref error))
                            {
                                mostrarGrilla();
                                cargarGrillas();
                                Alert("Archivo Cargado!");
                            }
                            else
                            {
                                Alert("No se logró registrar el archivo: " + error);
                            }
                        }                       
                    }
                    else
                    {
                        Alert("Se bloqueó el cargue de archivos de esta acta. Favor refrescar el navegador e informe a su interventor.");
                    }
                }
                catch (ApplicationException ex)
                {
                    Alert("Advertencia, detalle: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Alert("Lo lamentamos sucedió un error: " + ex.Message);
                }
            }
        }

        protected void gvArchivosActa1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("VerArchivo"))
            {
                if (e.CommandArgument != null)
                {
                    string[] parametros;
                    parametros = e.CommandArgument.ToString().Split(';');

                    var nombreArchivo = parametros[0];
                    var urlArchivo = ConfigurationManager.AppSettings.Get("RutaIP") + parametros[1];
                    string id = parametros[2];                   

                    Response.Clear();
                    //Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + CodigoProyecto + "_" + nombreArchivo);
                    Response.TransmitFile(urlArchivo);
                    Response.End();
                }
            }
            if (e.CommandName.Equals("Borrar"))
            {
                if (e.CommandArgument != null)
                {
                    int idArchivoActa = Convert.ToInt32(e.CommandArgument.ToString());

                    if (SeguimientoBLL.eliminarArchivo(idArchivoActa, usuario.IdContacto))
                    {
                        Alert("Archivo eliminado!");
                        cargarGrillas();
                    }
                }
            }

        }

        protected void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            if (ddlDeshabilitarActa.SelectedValue.ToString() == "S")
            {
                Alert("Debe seleccionar un Acta");
            }
            else
            {
                //Agregar Registro en la BD
                int numActa = Convert.ToInt32(ddlDeshabilitarActa.SelectedValue.ToString());
                string error = "";
                if (SeguimientoBLL.deshabilitarHabilitarActa(CodigoProyecto, numActa, ref error))
                {
                    habilitarDDLActasCargar();
                    Alert("Se deshabilitó el cargue de archivos del acta de manera exitosa.");
                }
                else
                {
                    Alert("Lo lamentamos, no pudimos deshabilitar el acta: " + error);
                }
            }
        }
    }
}