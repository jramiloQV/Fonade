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

namespace Fonade.PlanDeNegocioV2.Formulacion.Solucion
{
    public partial class FichaTecnica : System.Web.UI.Page
    {
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoTab { get { return Constantes.CONST_Parte2FichaTecnica; } set { } }                
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);
            EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto);

            Encabezado.CodigoProyecto = CodigoProyecto;
            Encabezado.CodigoTab = CodigoTab;

            SetPostIt();
        }


        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Parte2FichaTecnica;
        }

        public List<ProyectoProducto> Get(int codigoProyecto, int startIndex, int maxRows)
        {
            return Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosByProyecto(codigoProyecto, startIndex,maxRows);
        }

        public List<ProyectoProducto> Get(int codigoProyecto)
        {
            return Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosByProyecto(codigoProyecto);
        }

        public int CountProductos(int codigoProyecto)
        {
            return Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.CountProductos(codigoProyecto);
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            Fonade.Proyecto.Proyectos.Redirect(Response, "Producto.aspx?codproyecto=" + CodigoProyecto, "_Blank", "width=900,height=700,top=100,left=300");
        }

        protected void imgAdd_Click(object sender, ImageClickEventArgs e)
        {
            Fonade.Proyecto.Proyectos.Redirect(Response, "Producto.aspx?codproyecto=" + CodigoProyecto, "_Blank", "width=900,height=700,top=100,left=300");
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

                        Fonade.Proyecto.Proyectos.Redirect(Response, "Producto.aspx?codproducto=" + codigoProducto, "_Blank", "width=900,height=700,top=100,left=300");                       
                    }
                }
                if (e.CommandName.Equals("eliminar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoProducto = Convert.ToInt32(e.CommandArgument.ToString());

                        if (!Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.ProductoExist(codigoProducto))
                            throw new ApplicationException("No existe el producto.");

                        if (Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.CountProductos(CodigoProyecto) == 1)
                            throw new ApplicationException("El registro no se puede eliminar ya que como mínimo debe existir uno.");
                    
                        Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.Delete(codigoProducto);
                        gvProductos.DataBind();
                                                                        
                        Formulacion.Utilidad.Utilidades.PresentarMsj("El registro fue eliminado correctamente.", this, "Alert");
                    }
                }
            }
            catch (ApplicationException ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj(ex.Message, this, "Alert");                
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj("Error al eliminar el producto.", this, "Alert");                
            }
        }
    }
}