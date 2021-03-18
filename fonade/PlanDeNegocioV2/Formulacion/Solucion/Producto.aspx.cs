using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Datos;
using Fonade.Account;
using System.Web.Security;

namespace Fonade.PlanDeNegocioV2.Formulacion.Solucion
{
    public partial class Producto : System.Web.UI.Page
    {
        /// <summary>
        /// Valida si es creación o actualización 
        /// False = Creación
        /// True = actualización
        /// </summary>
        public Boolean IsCreate
        {
            get {
                return !Request.QueryString.AllKeys.Contains("codproducto");
            }
            set { }
        }
        
        public int CodigoProyecto
        {
            get {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }

        public int CodigoProducto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproducto"]);
            }
            set { }
        }

        public int CodigoTab { get { return Constantes.CONST_Parte2FichaTecnica; } set { } }
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ValidateParameters();

                if (!IsCreate && !Page.IsPostBack)
                    GetProductoDetails(CodigoProducto);               
            }
            catch (Exception ex)
            {
                btnCreate.Visible = false;
                btnUpdate.Visible = false;

                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentalo de nuevo. Detalle : " + ex.Message;               
            }
        }

        protected void GetProductoDetails(int codigoProducto) {
            var producto = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductoById(codigoProducto);

            txtNombreProducto.Text = producto.NomProducto;
            txtNombreComercial.Text = producto.NombreComercial;
            txtUnidadMedida.Text = producto.UnidadMedida;
            txtDescripcionGeneral.Text = producto.DescripcionGeneral;
            txtCondicionesEspeciales.Text = producto.CondicionesEspeciales;
            txtComposicion.Text = producto.Composicion;
            txtOtros.Text = producto.Otros;
        }

        protected void ValidateParameters() {

            if (!(Request.QueryString.AllKeys.Contains("codproyecto") || Request.QueryString.AllKeys.Contains("codproducto")))
            {
                btnCreate.Visible = btnUpdate.Visible = false;
                throw new Exception("No se logro obtener la información necesaria para continuar.");
            }

            if (IsCreate)
            {
                var codigoProyecto = Convert.ToInt32(Request.QueryString["codproyecto"]);
                if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.ProyectoExist(codigoProyecto))
                    throw new Exception("No se logro obtener la información necesaria para continuar.");
            }
            else
            {
                var codigoProducto = Convert.ToInt32(Request.QueryString["codproducto"]);
                if (!Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.ProductoExist(codigoProducto))
                    throw new Exception("No se logro obtener la información necesaria para continuar.");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseAndRefreshParent(false);
        }

        protected void ValidateFields()
        {
            FieldValidate.ValidateString("Nombre especifico del producto", txtNombreProducto.Text, true, 100);
            FieldValidate.ValidateString("Nombre comercial", txtNombreComercial.Text, true, 100);
            FieldValidate.ValidateString("Unidad de medida", txtUnidadMedida.Text, true, 100);
            FieldValidate.ValidateString("Descripción general", txtDescripcionGeneral.Text, true, 250);
            FieldValidate.ValidateString("Condiciones especiales", txtCondicionesEspeciales.Text, true, 250);
            FieldValidate.ValidateString("Composición", txtComposicion.Text, false, 250);
            FieldValidate.ValidateString("Otros", txtOtros.Text, false, 250);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();

                if (Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.ExistProductoByName(txtNombreProducto.Text, txtUnidadMedida.Text, CodigoProyecto))
                    throw new ApplicationException("Ya existe un producto con ese mismo nombre y unidad de medida.");

                if (Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.CountProductos(CodigoProyecto) >= 10)
                    throw new ApplicationException("Ya has agredado el número máximo de productos que es 10.");

                ProyectoProducto nuevoProducto = new ProyectoProducto() {
                                                                        CodProyecto = CodigoProyecto,
                                                                        NomProducto = txtNombreProducto.Text,
                                                                        NombreComercial = txtNombreComercial.Text,  
                                                                        UnidadMedida = txtUnidadMedida.Text,
                                                                        DescripcionGeneral = txtDescripcionGeneral.Text,
                                                                        CondicionesEspeciales = txtCondicionesEspeciales.Text,
                                                                        Composicion = txtComposicion.Text,
                                                                        Otros = txtOtros.Text                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
                                                                        };

                Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.Insert(nuevoProducto);

                Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(CodigoTab, CodigoProyecto, usuario.IdContacto, true);
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Paso2Proyeccion,CodigoProyecto, usuario.IdContacto, false);

                CloseAndRefreshParent(true);
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

        /// <summary>
        /// Cierra la ventana emergente y actualiza el formulario padre
        /// </summary>
        /// <param name="refreshParent">Actualiza el formulario padre</param>
        protected void CloseAndRefreshParent(bool refreshParent = false) {

            if(refreshParent)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.__doPostBack();window.close();", true);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }
       
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();

                if (!Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.ProductoExist(CodigoProducto))
                    throw new ApplicationException("No existe el producto.");

                var producto = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductoById(CodigoProducto);
                
                if (Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.ExistProductoById(txtNombreProducto.Text, txtUnidadMedida.Text, CodigoProducto, producto.CodProyecto))
                    throw new ApplicationException("El registro ya existe. Por favor ingrese uno nuevo.");
                
                producto.NomProducto = txtNombreProducto.Text;
                producto.NombreComercial = txtNombreComercial.Text;
                producto.UnidadMedida = txtUnidadMedida.Text;
                producto.DescripcionGeneral = txtDescripcionGeneral.Text;
                producto.CondicionesEspeciales = txtCondicionesEspeciales.Text;
                producto.Composicion = txtComposicion.Text;
                producto.Otros = txtOtros.Text;                
                producto.FormaDePago = string.Empty;
                producto.Justificacion = string.Empty;
                producto.Iva = 0;

                Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.Update(producto);

                Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, producto.CodProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(CodigoTab, producto.CodProyecto, usuario.IdContacto,true);                

                CloseAndRefreshParent(true);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message;
            }
        }
    }
}