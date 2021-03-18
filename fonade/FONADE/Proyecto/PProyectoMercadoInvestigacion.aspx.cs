using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using Fonade.Negocio.Proyecto;
using Fonade.Negocio;
using System.Text.RegularExpressions;
using Fonade.Clases;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoMercadoInvestigacion : Negocio.Base_Page
    {
        public int CodigoProyecto { get; set; }
        public int CodigoTab
        {
            get
            {
                return Constantes.CONST_InvestigacionMercados;
            }
            set { }
        }
        public int CodigoConvocatoria { get; set; }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean PostitVisible
        {
            get
            {
                return EsMiembro && !EsRealizado;
            }
            set { }
        }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);
            }
            set { }
        }
        public Boolean AllowCheckTab { get; set; }
   
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");

                CodigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
                CodigoConvocatoria = Session["CodConvocatoria"] != null ? Convert.ToInt32(Session["CodConvocatoria"]) : 0;

                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto);
                AllowCheckTab = ProyectoGeneral.AllowCheckTab(usuario.CodGrupo, CodigoProyecto, CodigoTab, EsMiembro);

                if (!IsPostBack)
                {
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), CodigoTab);

                    var entity = getProyectoMercadoInvestigacions(CodigoProyecto);
                    SetDatos(entity);

                    ProyectoGeneral.GetUltimaActualizacion(lblUltimaActualizacion, lblFechaUltimaActualizacion, chkEsRealizado, btnUpdateTab, CodigoTab, CodigoProyecto);
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);

                if (Session["CodProyecto"] == null)
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }            
        }

        private ProyectoMercadoInvestigacion getProyectoMercadoInvestigacions(int codigoProyecto)
        {
            var entity = (from mercado in consultas.Db.ProyectoMercadoInvestigacions
                          where mercado.CodProyecto == Convert.ToInt32(codigoProyecto)
                          select mercado).FirstOrDefault();

            return entity;
        }
      
        private void SetDatos(ProyectoMercadoInvestigacion entity)
        {
            if (entity != null)
            {
                txt_objetivos.Text = entity.Objetivos.htmlDecode();
                txt_justificacion.Text = entity.Justificacion.htmlDecode();
                txt_analisisS.Text = entity.AnalisisSector.htmlDecode();
                txt_analisisM.Text = entity.AnalisisMercado.htmlDecode();
                txt_analisisC.Text = entity.AnalisisCompetencia.htmlDecode();
            }
        }

        protected void btn_limpiarCampos_Click(object sender, EventArgs e)
        {            
            txt_objetivos.Text = String.Empty;
            txt_justificacion.Text = String.Empty;
            txt_analisisS.Text = String.Empty;
            txt_analisisM.Text = String.Empty;
            txt_analisisC.Text = String.Empty;
        }

        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {
            try
            {            
                ProyectoMercadoInvestigacionNegocio pmiNeg = new ProyectoMercadoInvestigacionNegocio();
                var query = getProyectoMercadoInvestigacions(CodigoProyecto);

                ProyectoMercadoInvestigacion pmv = new ProyectoMercadoInvestigacion()
                {
                    CodProyecto = Convert.ToInt32(CodigoProyecto),
                    AnalisisSector = txt_analisisS.Text.htmlEncode(),
                    AnalisisMercado = txt_analisisM.Text.htmlEncode(),
                    AnalisisCompetencia = txt_analisisC.Text.htmlEncode(),
                    Objetivos = txt_objetivos.Text.htmlEncode(),
                    Justificacion = txt_justificacion.Text.htmlEncode()
                };
            
                if (query == null)
                {                          
                    pmiNeg.Agregar(pmv);
                }
                else
                {            
                    pmiNeg.Modificar(pmv);
                }

                ProyectoGeneral.UpdateTab(CodigoTab, CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, chkEsRealizado.Checked);
                ProyectoGeneral.GetUltimaActualizacion(lblUltimaActualizacion, lblFechaUltimaActualizacion, chkEsRealizado, btnUpdateTab, CodigoTab, CodigoProyecto);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void btnUpdateTab_Click(object sender, EventArgs e)
        {
            try
            {
                ProyectoGeneral.UpdateTab(CodigoTab, CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, chkEsRealizado.Checked);
            }
            catch (ApplicationException ex)
            {
                chkEsRealizado.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                chkEsRealizado.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "MercaInvestiga";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = CodigoTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "MercaInvestiga";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = CodigoTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }
        
    }
}