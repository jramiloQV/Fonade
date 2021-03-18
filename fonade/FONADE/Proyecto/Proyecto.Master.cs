using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Caching;
using Datos;
using Fonade.Clases;

namespace Fonade.FONADE.Proyecto
{
    public partial class Proyecto : Negocio.Base_Master
    {
        public string codProyecto;
        public string codConvocatoria;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FechaSesion.Text = DateTime.Now.getFechaConFormato();

            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";

            if (codProyecto == "0")
            {                
                Session.Abandon();
                Response.Redirect("~/Account/Login.aspx");
            }
           
            var query = (from p in consultas.Db.Proyecto
                         from i in consultas.Db.Institucions
                         from s in consultas.Db.SubSector
                         from c in consultas.Db.Ciudad
                         from d in consultas.Db.departamento
                         where p.Id_Proyecto == Convert.ToInt32(codProyecto)
                             && s.Id_SubSector == p.CodSubSector
                             && i.Id_Institucion == p.CodInstitucion
                             && p.CodCiudad == c.Id_Ciudad
                             && c.CodDepartamento == d.Id_Departamento
                         select new
                         {
                             p.Id_Proyecto,
                             p.NomProyecto,
                             s.NomSubSector,
                             i.NomUnidad,
                             c.NomCiudad,
                             d.NomDepartamento,
                             d.Id_Departamento,
                             i.NomInstitucion,
                             p.CodEstado
                         });

            foreach (var obj in query)
            {
                lbl_title.Text = obj.Id_Proyecto
                    + " - " + obj.NomProyecto
                    + " - " + obj.NomUnidad
                    + " (" + obj.NomInstitucion + ")";
                img_lt.Src = "~/Images/ImgLT" + obj.CodEstado + ".jpg";
                img_map.Src = "~/Images/Mapas/" + remplazarTilde(obj.NomDepartamento) + "Pq.gif";
                img_map.Alt = obj.NomCiudad + "(" + obj.NomDepartamento + ")";
                link_map.HRef = "~/Mapas/Mapas.aspx?ver=1&pc=" + obj.Id_Departamento + "&pid=" + obj.Id_Proyecto;
                lbl_convocatoria.Text = lbl_convocatoria.Text + obj.NomSubSector;

                Session["TituloProyectoMaster"] = lbl_title.Text;

                break;
            }

            validarDocumentosCompletos();

            if(usuario.CodGrupo == 1 || usuario.CodGrupo == 2 || usuario.CodGrupo == 8 || usuario.CodGrupo == 12)
            {
                dBuscarProyecto.Visible = true;
            }
        }

        private string remplazarTilde(string texto)
        {
            string result = texto.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            return result;
        }

        protected void LoginStatus_LoggedOut(Object sender, System.EventArgs e)
        {
            MemoryCache.Default.Dispose();
            Session.Abandon();
        }

        // Valida que el emprendedor tenga los documentos cargados.
        protected void validarDocumentosCompletos()
        {
            //Obtenemos la estructura del master.page
            ContentPlaceHolder contenidoMasterPage = (ContentPlaceHolder)FindControl("bodyHolder");
            string paginaActual = contenidoMasterPage.Page.ToString();

            if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
            {
                //Obligar a subir documento
                String txtSQL = "SELECT TipoArchivo FROM ContactoArchivosAnexos WHERE CodContacto= " + usuario.IdContacto;
                var documento = consultas.ObtenerDataTable(txtSQL, "text");                

                try
                {
                    if (documento.Select("TipoArchivo ='FotocopiaDocumento'").Count() == 0)
                    {
                        throw new ApplicationException("Debe adjuntar fotocopia de cedula");
                    }
                    else if (documento.Select("TipoArchivo ='CertificacionEstudios'").Count() == 0)
                    {
                        //Linea Comentada mientras esta la funcionalidad de pedir documentos de certificado
                        throw new ApplicationException("Debe adjuntar Certificado de estudios");
                    }
                }
                catch (ApplicationException ex)
                {
                    //Mensaje de advertencia para que el emprendedor adjunte documentos obligatorios.        
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message + ".');", true);

                    //Hacemos redirect a cargar documentos      
                    if (paginaActual != "ASP.fonade_miperfil_miperfil_aspx")
                    {
                        String url = "/Fonade/MiPerfil/MiPerfil.aspx";
                        Response.Redirect(url);
                    }
                }

            }
        }

        protected void btnBuscarPrj_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdPrjBuscar.Text.Trim()))
            {
                Session["CodProyecto"] = txtIdPrjBuscar.Text.Trim();
                Response.Redirect("~/Fonade/Proyecto/ProyectoFrameSet.aspx");
            }
            else
            {
                txtIdPrjBuscar.Focus();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "alert('Debe ingresar el Id del proyecto a consultar!');", true);
            }
        }

    }
}