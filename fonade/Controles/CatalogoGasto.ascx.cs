using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;
using Fonade.Clases;

namespace Fonade.Controles
{
    /// <summary>
    /// CatalogoGasto
    /// </summary>
    /// <seealso cref="System.Web.UI.UserControl" />
    public partial class CatalogoGasto : System.Web.UI.UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public enum Accion {
            /// <summary>
            /// The nuevo
            /// </summary>
            Nuevo,
            /// <summary>
            /// The editar
            /// </summary>
            Editar,
            /// <summary>
            /// The borrar
            /// </summary>
            Borrar
        };

        /// <summary>
        /// The consulta
        /// </summary>
        protected Consultas consulta = new Consultas();

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error { get; set; }


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            txtValor.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
            txtValor.Attributes.Add("onchange", "MoneyFormat(this)");            
        }

        /// <summary>
        /// Metodo utilizado para diligenciar el fomulario, si la accion es Borrar el metodo le retornar la confirmacion.
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="codProyecto"></param>
        /// <param name="idGasto"></param>
        /// <param name="tipo"></param>
        public string Cargar(Accion accion, string codProyecto, string idGasto,string tipo)
        {
            hddAccion.Value = accion.ToString();
            hddCodProyecto.Value = codProyecto;
            hddIdGasto.Value = idGasto;
            hddTipo.Value = tipo;

            if (accion == Accion.Nuevo)
            {
                btnGasto.Text = "Crear";
                LimpiarCampos();
            }

            if (accion == Accion.Editar)
            {
                btnGasto.Text = "Actualizar";
                LimpiarCampos();
                llenarCampos();
            }

            if (accion == Accion.Borrar)
            {
                return BorrarCargo();
            }
            return "OK";
        }

        /// <summary>
        /// Limpiar campos.
        /// </summary>
        protected void LimpiarCampos()
        {
            txtDescripcion.Text = "";
            txtValor.Text = "";
            txtDescripcion.ReadOnly = false;
        }

        /// <summary>
        /// Llenar campos.
        /// </summary>
        protected void llenarCampos()
        {
            ProyectoGasto dato = RegistroActual();
            txtDescripcion.Text = dato.Descripcion;
            txtValor.Text = dato.Valor.ToString("0,0.00", CultureInfo.InvariantCulture);
            if (dato.Protegido)
            {
                txtDescripcion.ReadOnly = true;
            }
        }

        /// <summary>
        /// Registro actual.
        /// </summary>
        /// <returns></returns>
        protected ProyectoGasto RegistroActual()
        {
            var query = (from p in consulta.Db.ProyectoGastos
                         where p.Id_Gasto == Convert.ToInt32(hddIdGasto.Value)
                         select p).First();

            return query;
        }

        /// <summary>
        /// Handles the Click event of the btnGasto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnGasto_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Descripción", txtDescripcion.Text, true, 255);
                FieldValidate.ValidateNumeric("Valor", txtValor.Text,true);
               
                if (hddAccion.Value == Accion.Nuevo.ToString())
                {
                    CrearNuevo();
                }
                else if (hddAccion.Value == Accion.Editar.ToString())
                {
                    ActualizarRegistro();
                }

                consulta.Db.SubmitChanges();

                Response.Redirect("../Proyecto/PProyectoOrganizacionCostos.aspx");
              
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + " ');", true);
            }            
        }

        /// <summary>
        /// Crear nuevo.
        /// </summary>
        /// <exception cref="ApplicationException">Ya existe un gasto con esa descripción.</exception>
        protected void CrearNuevo()
        {            
                var query = (from p in consulta.Db.ProyectoGastos
                             where p.Descripcion == txtDescripcion.Text &&
                             p.CodProyecto == Convert.ToInt32(hddCodProyecto.Value)
                             select new { p.Descripcion });
                if (query.Count() == 0)
                {
                    ProyectoGasto dato = new ProyectoGasto();
                    dato.CodProyecto = Convert.ToInt32(hddCodProyecto.Value);
                    dato.Descripcion = txtDescripcion.Text;
                    dato.Valor = Convert.ToDecimal(txtValor.Text.Replace(",", "").Replace(".", ","));
                    dato.Tipo = hddTipo.Value;
                    consulta.Db.ProyectoGastos.InsertOnSubmit(dato);       
                }
                else
                {
                    throw new ApplicationException("Ya existe un gasto con esa descripción.");                   
                }
        }

        /// <summary>
        /// Actualizar registro.
        /// </summary>
        /// <exception cref="ApplicationException">Ya existe un gasto con esa descripción.</exception>
        protected void ActualizarRegistro()
        {
            var query = (from p in consulta.Db.ProyectoGastos 
                            where p.Descripcion==txtDescripcion.Text &&
                            p.CodProyecto==Convert.ToInt32(hddCodProyecto.Value) &&
                            p.Id_Gasto != Convert.ToInt32(hddIdGasto.Value)
                            select new{p.Descripcion});
              if (query.Count() == 0)
              {
                  ProyectoGasto dato = RegistroActual();
                  dato.Descripcion = txtDescripcion.Text;
                  dato.Valor = Convert.ToDecimal(txtValor.Text.Replace(",", "").Replace(".", ","));
                  Error = "OK";
              }
              else
              {
                  throw new ApplicationException("Ya existe un gasto con esa descripción.");   
              }
        }

        /// <summary>
        /// Handles the Click event of the btnCancelarGasto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelarGasto_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            hddAccion.Value = "";
            hddCodProyecto.Value = "";
            hddIdGasto.Value = "";
            hddTipo.Value = "";
        }

        /// <summary>
        /// Borrar cargo.
        /// </summary>
        /// <returns></returns>
        protected string BorrarCargo()
        {

            consulta.Db.ExecuteCommand("Delete from ProyectoGastos where protegido=0 and Id_Gasto={0}", Convert.ToInt32(hddIdGasto.Value));
            consulta.Db.SubmitChanges();
            string respuesta = "OK";

            return respuesta;
        }


    }
}