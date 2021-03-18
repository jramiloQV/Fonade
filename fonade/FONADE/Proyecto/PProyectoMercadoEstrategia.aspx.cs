using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using Fonade.Negocio;
using Fonade.Clases;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoMercadoEstrategia : Negocio.Base_Page
    {
        public int CodigoProyecto { get; set; }
        public int CodigoTab
        {
            get
            {
                return Constantes.CONST_EstrategiasMercado;
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
                AllowCheckTab = ProyectoGeneral.AllowCheckTab(usuario.CodGrupo, CodigoProyecto, CodigoTab , EsMiembro);

                if (!IsPostBack)
                {
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), CodigoTab);
                    var entity = getProyectoMercadoEstrategia(CodigoProyecto);
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

        private void SetDatos(ProyectoMercadoEstrategia proyectoMercado)
        {
            if (proyectoMercado != null)
            {
                txtConcepto.Text = proyectoMercado.ConceptoProducto.htmlDecode();
                txtEstrategiasDistribucion.Text = proyectoMercado.EstrategiasDistribucion.htmlDecode();
                txtEstrategiasPrecio.Text = proyectoMercado.EstrategiasPrecio.htmlDecode();
                txtEstrategiaPromocion.Text = proyectoMercado.EstrategiasPromocion.htmlDecode();
                txtEstrategiaComunicacion.Text = proyectoMercado.EstrategiasComunicacion.htmlDecode();
                txtEstrategiaServicio.Text = proyectoMercado.EstrategiasServicio.htmlDecode();
                txtPresupuestoMercadeo.Text = proyectoMercado.PresupuestoMercado.htmlDecode();
                txtEstrategiaAprovisionamiento.Text = proyectoMercado.EstrategiasAprovisionamiento.htmlDecode();
            }
        }

        private ProyectoMercadoEstrategia getProyectoMercadoEstrategia(int codigoProyecto)
        {
            var entity = from mercado in consultas.Db.ProyectoMercadoEstrategias
                         where mercado.CodProyecto == codigoProyecto
                         select mercado;

            return entity.FirstOrDefault();
        }

        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                ProyectoMercadoEstrategiaNegocio pmeNegocio = new ProyectoMercadoEstrategiaNegocio();
                var query = getProyectoMercadoEstrategia(CodigoProyecto);

                if (query == null)
                {
                    query = new ProyectoMercadoEstrategia()
                    {
                        CodProyecto = CodigoProyecto,
                        ConceptoProducto = txtConcepto.Text.htmlEncode(),
                        EstrategiasDistribucion = txtEstrategiasDistribucion.Text.htmlEncode(),
                        EstrategiasPrecio = txtEstrategiasPrecio.Text.htmlEncode(),
                        EstrategiasPromocion = txtEstrategiaPromocion.Text.htmlEncode(),
                        EstrategiasComunicacion = txtEstrategiaComunicacion.Text.htmlEncode(),
                        EstrategiasServicio = txtEstrategiaServicio.Text.htmlEncode(),
                        EstrategiasAprovisionamiento = txtEstrategiaAprovisionamiento.Text.htmlEncode(),
                        PresupuestoMercado = txtPresupuestoMercadeo.Text.htmlEncode()
                    };

                    pmeNegocio.Agregar(query);
                }
                else
                {                        
                        query.ConceptoProducto = txtConcepto.Text.htmlEncode();
                        query.EstrategiasDistribucion = txtEstrategiasDistribucion.Text.htmlEncode();
                        query.EstrategiasPrecio = txtEstrategiasPrecio.Text.htmlEncode();
                        query.EstrategiasPromocion = txtEstrategiaPromocion.Text.htmlEncode();
                        query.EstrategiasComunicacion = txtEstrategiaComunicacion.Text.htmlEncode();
                        query.EstrategiasServicio = txtEstrategiaServicio.Text.htmlEncode();
                        query.EstrategiasAprovisionamiento = txtEstrategiaAprovisionamiento.Text.htmlEncode();
                        query.PresupuestoMercado = txtPresupuestoMercadeo.Text.htmlEncode();

                    pmeNegocio.Modificar(query);
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

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "MercaEstrategia";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = CodigoTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "MercaEstrategia";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = CodigoTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
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

        protected void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            txtConcepto.Text = string.Empty;
            txtEstrategiasDistribucion.Text = string.Empty;
            txtEstrategiasPrecio.Text = string.Empty;
            txtEstrategiaPromocion.Text = string.Empty;
            txtEstrategiaComunicacion.Text = string.Empty;
            txtEstrategiaServicio.Text = string.Empty;
            txtEstrategiaAprovisionamiento.Text = string.Empty;
            txtPresupuestoMercadeo.Text = string.Empty;
        }
    }
}