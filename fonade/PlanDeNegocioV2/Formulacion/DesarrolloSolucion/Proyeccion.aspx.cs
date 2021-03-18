using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;
using System.Web.Security;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;

namespace Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    public partial class Proyeccion : System.Web.UI.Page
    {
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoTab { get { return Constantes.CONST_Paso2Proyeccion; } set { } }
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
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

        public Boolean AllowUpdateProyeccion
        {
            get
            {
                return !cmbTiempoProyeccion.SelectedValue.Equals(string.Empty);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto);

                Encabezado.CodigoProyecto = CodigoProyecto;
                Encabezado.CodigoTab = CodigoTab;

                SetPostIt();

                var existeTiempoProyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(CodigoProyecto);
                if(existeTiempoProyeccion != null)
                    cmbTiempoProyeccion.Attributes["onchange"] = "alertTiempo();";
                else
                    cmbTiempoProyeccion.Attributes["onchange"] = "";

                if (!Page.IsPostBack)
                    GetTiempoProyeccion(CodigoProyecto);

            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentalo de nuevo. Detalle : " + ex.Message;
            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Paso2Proyeccion;
        }

        protected void GetTiempoProyeccion(int codigoProyecto)
        {
            var proyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(CodigoProyecto);

            if (proyeccion != null)
            {
                cmbTiempoProyeccion.SelectedValue = proyeccion.TiempoProyeccion.ToString();
                ShowIngresosPorVentas(codigoProyecto, (int)proyeccion.TiempoProyeccion);
            }
        }

        public void ShowIngresosPorVentas(int codigoProyecto, int tiempoProyeccion)
        {
            VisibilidadTiempoProyeccion(tiempoProyeccion);                                   
        }

        protected void VisibilidadTiempoProyeccion(int tiempoProyeccion)
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i <= tiempoProyeccion)
                    gvIngresosPorVentas.Columns[i].Visible = true;
                else
                    gvIngresosPorVentas.Columns[i].Visible = false;
            }
        }

        public List<Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.IngresosPorVentas> GetIngresosPorVentas(int codigoProyecto) {
            return Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetIngresosPorVentas(codigoProyecto);
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("actualizar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoProducto = Convert.ToInt32(e.CommandArgument.ToString());
                        Fonade.Proyecto.Proyectos.Redirect(Response, "VentasPorMes.aspx?codproducto=" + codigoProducto + "&codproyecto=" + CodigoProyecto, "_Blank", "width=1000,height=1000,top=0,left=0,scrollbars=yes,resizable=yes,toolbar=yes");
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTiempoProyeccion.SelectedValue.Equals(string.Empty))
                    throw new ApplicationException("Debe seleccionar el tiempo de proyección.");

                var proyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(CodigoProyecto);
                if (proyeccion == null)
                {
                    var nuevaProyeccion = new ProyectoMercadoProyeccionVenta
                    {
                        CodProyecto = CodigoProyecto,
                        FechaArranque = DateTime.Now,
                        CodPeriodo = 1,
                        TiempoProyeccion = (byte)Convert.ToInt16(cmbTiempoProyeccion.SelectedValue),
                        MetodoProyeccion = "N/A",
                        PoliticaCartera = "N/A",
                        CostoVenta = "N/A",
                        justificacion = "N/A"
                    };

                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.Insert(nuevaProyeccion);
                }
                else
                {
                    proyeccion.TiempoProyeccion = (byte)Convert.ToInt16(cmbTiempoProyeccion.SelectedValue);

                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.Update(proyeccion);
                }
                //marcar tab como completado
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Paso2Proyeccion, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_Paso2Proyeccion, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                Encabezado.ActualizarFecha();                
                Formulacion.Utilidad.Utilidades.PresentarMsj(Negocio.Mensajes.Mensajes.GetMensaje(8), this, "Alert");
                lblError.Visible = false;
                gvIngresosPorVentas.DataBind();
                GetTiempoProyeccion(CodigoProyecto);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentalo de nuevo. Detalle : " + ex.Message;
            }
        }
    }
}