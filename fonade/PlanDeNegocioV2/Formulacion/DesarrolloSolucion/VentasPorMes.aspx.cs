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
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;

namespace Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    public partial class VentasPorMes : System.Web.UI.Page
    {
        public int CodigoProducto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproducto"]);
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

        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        public int CodigoTab { get {
                return Constantes.CONST_Paso2Proyeccion;
            } set { } }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ValidateParameters();

                if (!Page.IsPostBack)
                    GetProductoDetails(CodigoProducto);
            }
            catch (Exception ex)
            {
                btnUpdate.Visible = false;

                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentalo de nuevo. Detalle : " + ex.Message;
            }
        }

        protected void GetProductoDetails(int codigoProducto)
        {
            var producto = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductoById(codigoProducto);

            txtNombreProducto.Text = producto.NomProducto;
            txtUnidadMedida.Text = producto.UnidadMedida;
            txtJustificacion.Text = producto.Justificacion;
            txtFormaDePago.Text = producto.FormaDePago;
            txtIva.Text = producto.Iva.ToString();

            var tiempoProyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(CodigoProyecto);

            if (tiempoProyeccion == null)
                throw new Exception("No se logro obtener la información necesaria para continuar.");

            VisibilidadTiempoProyeccion((int)tiempoProyeccion.TiempoProyeccion);
        }

        protected void VisibilidadTiempoProyeccion(int tiempoProyeccion)
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i <= tiempoProyeccion)
                    gvVentasPorMes.Columns[i].Visible = true;
                else
                    gvVentasPorMes.Columns[i].Visible = false;
            }
        }

        protected void ValidateParameters()
        {
            if (!(Request.QueryString.AllKeys.Contains("codproducto")))
                throw new Exception("No se logro obtener la información necesaria para continuar.");

            if (!(Request.QueryString.AllKeys.Contains("codproyecto")))
                throw new Exception("No se logro obtener la información necesaria para continuar.");

            var codigoProducto = Convert.ToInt32(Request.QueryString["codproducto"]);
            if (!Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.ProductoExist(codigoProducto))
                throw new Exception("No se logro obtener la información necesaria para continuar.");
        }

        protected void ValidateFields()
        {
            FieldValidate.ValidateString("Forma de pago", txtFormaDePago.Text, true, 250);
            FieldValidate.ValidateString("Justificación", txtJustificacion.Text, true, 250);
            FieldValidate.ValidateNumeric("Iva", txtIva.Text, true);
        }

        protected void UpdateProyecciones()
        {
            var monthCounter = 1;
            foreach (GridViewRow proyeccionFila in gvVentasPorMes.Rows) {
                if (monthCounter <= 12) {
                    var Year1 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 1,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear1") as TextBox)
                    };

                    var Year2 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 2,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear2") as TextBox)
                    };

                    var Year3 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 3,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear3") as TextBox)
                    };
                    var Year4 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 4,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear4") as TextBox)
                    };
                    var Year5 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 5,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear5") as TextBox)
                    };
                    var Year6 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 6,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear6") as TextBox)
                    };
                    var Year7 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 7,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear7") as TextBox)
                    };
                    var Year8 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 8,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear8") as TextBox)
                    };
                    var Year9 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 9,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear9") as TextBox)
                    };
                    var Year10 = new ProyectoProductoUnidadesVenta()
                    {
                        CodProducto = CodigoProducto,
                        Mes = (Int16)monthCounter,
                        Ano = 10,
                        Unidades = GetValue(proyeccionFila.FindControl("txtYear10") as TextBox)
                    };

                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year1);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year2);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year3);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year4);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year5);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year6);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year7);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year8);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year9);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProyeccionMes(Year10);
                }
                else if (monthCounter == 13)
                {

                    var Year1 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 1,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear1") as TextBox)
                    };

                    var Year2 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 2,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear2") as TextBox)
                    };

                    var Year3 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 3,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear3") as TextBox)
                    };
                    var Year4 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 4,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear4") as TextBox)
                    };
                    var Year5 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 5,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear5") as TextBox)
                    };
                    var Year6 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 6,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear6") as TextBox)
                    };
                    var Year7 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 7,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear7") as TextBox)
                    };
                    var Year8 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 8,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear8") as TextBox)
                    };
                    var Year9 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 9,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear9") as TextBox)
                    };
                    var Year10 = new ProyectoProductoPrecio()
                    {
                        CodProducto = CodigoProducto,
                        Periodo = 10,
                        Valor = GetPriceValue(proyeccionFila.FindControl("txtYear10") as TextBox)
                    };

                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year1);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year2);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year3);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year4);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year5);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year6);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year7);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year8);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year9);
                    Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.UpdateProductoPrecio(Year10);
                }
                monthCounter += 1;
            }
        }

        protected double GetValue(TextBox texto)
        {
            return string.IsNullOrEmpty(texto.Text) ? 0 : Double.Parse(texto.Text.Replace(",", string.Empty));
        }

        protected Decimal GetPriceValue(TextBox texto)
        {
            return string.IsNullOrEmpty(texto.Text) ? 0 : Decimal.Parse(texto.Text.Replace(",",string.Empty));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();

                if (!Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.ProductoExist(CodigoProducto))
                    throw new ApplicationException("No existe el producto.");

                var producto = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductoById(CodigoProducto);
                
                producto.FormaDePago = txtFormaDePago.Text;
                producto.Justificacion = txtJustificacion.Text;
                producto.Iva = Convert.ToInt32(txtIva.Text);
                producto.PorcentajeIva = Convert.ToDouble(txtIva.Text);

                Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.Update(producto);

                UpdateProyecciones();

                Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, producto.CodProyecto, usuario.IdContacto, usuario.CodGrupo, false);                

                if (Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.VerificarProyeccionesCompletas(producto.CodProyecto))
                    Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(CodigoTab, producto.CodProyecto, usuario.IdContacto, true);
                else
                    Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(CodigoTab, producto.CodProyecto, usuario.IdContacto, false);

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

        public List<ProyeccionDeVentasPorMes> Get(int CodigoProducto)
        {
            return Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetVentasPorMes(CodigoProducto);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseAndRefreshParent(false);
        }

        /// <summary>
        /// Cierra la ventana emergente y actualiza el formulario padre
        /// </summary>
        /// <param name="refreshParent">Actualiza el formulario padre</param>
        protected void CloseAndRefreshParent(bool refreshParent = false)
        {
            if (refreshParent)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.__doPostBack();window.close();", true);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }
    }
}