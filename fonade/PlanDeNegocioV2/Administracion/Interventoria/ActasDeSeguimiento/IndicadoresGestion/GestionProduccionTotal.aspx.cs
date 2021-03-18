using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Datos.Modelos;
using Fonade.Negocio.FonDBLight;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion
{
    public partial class GestionProduccionTotal : Negocio.Base_Page
    {
        public int CodigoProyecto
        {
            get { return Convert.ToInt32(Session["idProyecto"]); }
            set { }
        }

        public int NumeroActa
        {
            get { return Convert.ToInt32(Session["idActa"]); }
            set { }
        }

        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }

        public Boolean AllowUpdate
        {
            get
            {
                return usuario.CodGrupo.Equals(Constantes.CONST_Interventor);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarProductoMasRepresentativo(CodigoProyecto, CodigoConvocatoria);
                cargarGridMetaProduccion(CodigoProyecto, CodigoConvocatoria);
                cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                cargarGridProductos(CodigoProyecto, CodigoConvocatoria);
            }
        }
        private void cargarGridProductos(int _codproyecto, int _codConvocatoria)
        {
            var Produccion = produccionController.ListAllProductosInterventoria(_codproyecto, _codConvocatoria, NumeroActa);

            gvAgregarIndicadorProduccion.DataSource = Produccion;
            gvAgregarIndicadorProduccion.DataBind();
        }

        private void cargarProductoMasRepresentativo(int _codproyecto, int _codConvocatoria)
        {
            //string producto = metasProduccionController.productoMasRepresentativo(_codproyecto, _codConvocatoria);
            string producto = metasProduccionController.productoMasRepresentativoInterventoria(_codproyecto, _codConvocatoria);
            lblProductoMasRep.Text = producto;
        }

        MetasProduccionControllerDTO metasProduccionController = new MetasProduccionControllerDTO();
        ActaSeguimGestionProduccionController produccionController = new ActaSeguimGestionProduccionController();
        private void cargarGridMetaProduccion(int _codproyecto, int _codConvocatoria)
        {
            var metaProduccion = metasProduccionController.ListAllMetasProduccionInterventoria(_codproyecto, _codConvocatoria);

            gvMetasProduccion.DataSource = metaProduccion;
            gvMetasProduccion.DataBind();            
        }

        private void cargarGridIndicador(int _codProyecto, int _codConvocatoria)
        {
            var gestion = produccionController.GetGestionProduccion(_codProyecto, _codConvocatoria);

            gvIndicador.DataSource = gestion;
            gvIndicador.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (guardarDatos())
            {
                Alert("Datos registrados exitosamente.");
                cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
            }
            else
            {
                Alert("No se guardaron los datos.");
            }
        }
                
        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvMetasProduccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMetasProduccion.PageIndex = e.NewPageIndex;
            var Compromisos = metasProduccionController.ListAllMetasProduccionInterventoria(CodigoProyecto, CodigoConvocatoria);
            gvMetasProduccion.DataSource = Compromisos;
            gvMetasProduccion.DataBind();
        }

        protected void gvIndicador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicador.PageIndex = e.NewPageIndex;
            var Compromisos = produccionController.GetGestionProduccion(CodigoProyecto, CodigoConvocatoria);
            gvIndicador.DataSource = Compromisos;
            gvIndicador.DataBind();
        }

        private bool guardarDatos()
        {
            bool guardado = false;
            try
            {
                if (!validarCampos())
                    throw new ApplicationException("Faltan campos por llenar: los campos de Cantidad y Descripción son obligatorios.");

                if(!validarCantidades())
                    throw new ApplicationException("Las cantidades no pueden ser menores que cero");

                List<ActaSeguimGestionProduccionModel> actaListProduccion = new List<ActaSeguimGestionProduccionModel>();

                foreach (GridViewRow row in gvAgregarIndicadorProduccion.Rows)
                {
                    ActaSeguimGestionProduccionModel actaProduccion = new ActaSeguimGestionProduccionModel();
                    
                    actaProduccion.cantidad = Convert.ToInt32(((TextBox)(row.FindControl("txtCantidad"))).Text);
                    actaProduccion.Descripcion = ((TextBox)(row.FindControl("txtDescripcion"))).Text;
                    actaProduccion.medida = ((Label)(row.FindControl("lblUnidadMedida"))).Text;
                    actaProduccion.codConvocatoria = CodigoConvocatoria;
                    actaProduccion.codProyecto = CodigoProyecto;
                    actaProduccion.numActa = NumeroActa;
                    actaProduccion.visita = NumeroActa;
                    actaProduccion.codProducto = Convert.ToInt32(((Label)(row.FindControl("lblCodProducto"))).Text);
                    actaProduccion.NomProducto = ((Label)(row.FindControl("lblNomProducto"))).Text;
                    actaProduccion.productoRepresentativo = Convert.ToBoolean(((Label)(row.FindControl("lblproductoRepresentativo"))).Text);

                    actaListProduccion.Add(actaProduccion);
                }
                string mensaje = "";
                guardado = produccionController
                    .InsertOrUpdateListGestionProduccion(actaListProduccion, ref mensaje);

                if (mensaje != "")
                {
                    Alert(mensaje);
                }
            }
            catch (ApplicationException ex)
            {
                guardado = false;
                Alert(ex.Message);
            }
            return guardado;
        }

        private bool validarCampos()
        {
            bool validado = true;

            foreach (GridViewRow row in gvAgregarIndicadorProduccion.Rows)
            {
                string texto = ((TextBox)(row.FindControl("txtDescripcion"))).Text;
                string cantidad = ((TextBox)(row.FindControl("txtCantidad"))).Text;
                if (texto.Trim() == "" || texto.Trim().Equals(String.Empty))
                {
                    validado = false;
                }
                if (cantidad.Trim() == "" || cantidad.Trim().Equals(String.Empty))
                {
                    validado = false;
                }
            }

            return validado;
        }

        private bool validarCantidades()
        {
            bool validado = true;

            foreach (GridViewRow row in gvAgregarIndicadorProduccion.Rows)
            {
               
                string cantidad = ((TextBox)(row.FindControl("txtCantidad"))).Text;
                
                if (Convert.ToInt32(cantidad)<0)
                {
                    validado = false;
                }
            }

            return validado;
        }

        protected void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            //validarTextbox
            if (validaciones())
            {
                if (InsertarProducto(usuario.IdContacto, CodigoProyecto))
                {
                    cargarProductoMasRepresentativo(CodigoProyecto, CodigoConvocatoria);
                    cargarGridMetaProduccion(CodigoProyecto, CodigoConvocatoria);
                    cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                    cargarGridProductos(CodigoProyecto, CodigoConvocatoria);
                    Alert("Producto Agregado Correctamente.");
                }
            }

        }

        private bool InsertarProducto(int _codContacto, int _codProyecto)
        {
            //ProyectoProducto producto = new ProyectoProducto { 
            //   CodProyecto= CodigoProyecto,
            //   Composicion = txtComposicion.Text,
            //   CondicionesEspeciales = txtCondicionesEspeciales.Text,
            //   DescripcionGeneral = txtDescripcionGeneral.Text,
            //   FormaDePago = txtFormaPago.Text,
            //   Iva = Convert.ToInt32(txtPorcentajeIVA.Text),
            //   Justificacion = txtJustificacion.Text,
            //   NombreComercial = txtNomComercial.Text,
            //   NomProducto = txtNomProducto.Text,
            //   Otros = txtOtros.Text,
            //   PorcentajeIva = Convert.ToDouble(txtPorcentajeIVA.Text),
            //   UnidadMedida = txtUnidadMedida.Text
            //};                      
            //int metaProducto = Convert.ToInt32(txtMetaProducto.Text);

            ActaSeguimGestionProduccionEvaluacion producto = new ActaSeguimGestionProduccionEvaluacion
            {
                codContacto = _codContacto,
                codProyecto = _codProyecto,
                composicion = txtComposicion.Text,
                condicionesEspeciales = txtCondicionesEspeciales.Text,
                descripcionGeneral = txtDescripcionGeneral.Text,
                fechaUltimaActualizacion = DateTime.Now,
                formaDePago = txtFormaPago.Text,
                id_Producto = 0,
                iva = Convert.ToInt32(txtPorcentajeIVA.Text),
                justificacion = txtJustificacion.Text,
                nombreComercial = txtNomComercial.Text,
                nomProducto = txtNomProducto.Text,
                ocultar = false,
                otros = txtOtros.Text,
                porcentajeIva = Convert.ToDouble(txtPorcentajeIVA.Text),
                productoRepresentativo = false,
                unidades = Convert.ToInt32(txtMetaProducto.Text),
                unidadMedida = txtUnidadMedida.Text
            };

            return produccionController
                    .insertarProductoInterventoria(producto);
        }

        private bool validaciones()
        {
            bool valido = true;

            if (!validarTextbox(txtComposicion))
            {
                Alert("El campo composicion es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtNomProducto))
            {
                Alert("El campo Nombre de Producto es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtPorcentajeIVA))
            {
                Alert("El campo Porcentaje IVA es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtNomComercial))
            {
                Alert("El campo Nombre Comercial es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtUnidadMedida))
            {
                Alert("El campo Unidad de Medida es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtCondicionesEspeciales))
            {
                Alert("El campo Condiciones Especiales es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtDescripcionGeneral))
            {
                Alert("El campo Descripcion General es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtFormaPago))
            {
                Alert("El campo Forma de Pago es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtJustificacion))
            {
                Alert("El campo Justificacion es obligatorio.");
                valido = false;
            }


            return valido;
        }

        private bool validarTextbox(TextBox textBox)
        {
            bool validado = false;

            if (textBox.Text!="")
            {
                validado = true;
            }

            return validado;
        }

        protected void gvMetasProduccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int idProducto = Convert.ToInt32(e.CommandArgument.ToString());

                if (produccionController.ocultarProducto(idProducto, usuario.IdContacto, CodigoProyecto))
                {
                    cargarProductoMasRepresentativo(CodigoProyecto, CodigoConvocatoria);
                    cargarGridMetaProduccion(CodigoProyecto, CodigoConvocatoria);
                    cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                    cargarGridProductos(CodigoProyecto, CodigoConvocatoria);

                    Alert("Se eliminó el producto.");
                }
                else
                {
                    Alert("No se eliminó el producto, debe permanecer al menos uno en el listado. ");
                }

            }
        }

        protected void gvMetasProduccion_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMetasProduccion.EditIndex = -1;
            cargarProductoMasRepresentativo(CodigoProyecto, CodigoConvocatoria);
            cargarGridMetaProduccion(CodigoProyecto, CodigoConvocatoria);
            cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
            cargarGridProductos(CodigoProyecto, CodigoConvocatoria);
           
        }

        protected void gvMetasProduccion_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMetasProduccion.EditIndex = e.NewEditIndex;
            cargarProductoMasRepresentativo(CodigoProyecto, CodigoConvocatoria);
            cargarGridMetaProduccion(CodigoProyecto, CodigoConvocatoria);
            cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
            cargarGridProductos(CodigoProyecto, CodigoConvocatoria);
        }

        protected void gvMetasProduccion_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)e.Keys["Id_ProductoInterventoria"];


            bool productoMasRepresentativo = ((RadioButton)gvMetasProduccion.Rows[e.RowIndex].FindControl("rdProductoSeleccionado")).Checked;

            int cantidad = Convert.ToInt32(((TextBox)gvMetasProduccion.Rows[e.RowIndex].FindControl("txtUnidadesEditar")).Text);
            string unidadMedida = ((TextBox)gvMetasProduccion.Rows[e.RowIndex].FindControl("txtUnidadMedidaEditar")).Text;
            string producto = ((TextBox)gvMetasProduccion.Rows[e.RowIndex].FindControl("txtProducto")).Text;

            if (producto != "" && unidadMedida != "")
            {
                if (produccionController.actualizarProducto(id,usuario.IdContacto,cantidad,unidadMedida,producto,productoMasRepresentativo))
                {
                    Alert("Se actualizó el producto.");
                }
                else
                {
                    Alert("No se logró actualizar el producto.");
                }
            }
            else
            {
                Alert("Los campos no deben estar vacíos.");
            }

            gvMetasProduccion.EditIndex = -1;

            cargarProductoMasRepresentativo(CodigoProyecto, CodigoConvocatoria);
            cargarGridMetaProduccion(CodigoProyecto, CodigoConvocatoria);
            cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
            cargarGridProductos(CodigoProyecto, CodigoConvocatoria);

        }
    }
}