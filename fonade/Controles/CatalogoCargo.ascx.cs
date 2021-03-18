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
    /// Control de usuario de catalogo
    /// </summary>
    /// <seealso cref="System.Web.UI.UserControl" />
    public partial class CatalogoCargo : System.Web.UI.UserControl
    {
        /// <summary>
        /// Accion
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
        /// The numero SMLVNV
        /// </summary>
        protected string NumeroSMLVNV;

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error
        {            
            get; 
            set;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            txtValorAnual.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
            txtValorMensual.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
            txtOtrosGastos.Attributes.Add("onkeypress", "javascript:return validarNro(event)");

            txtValorAnual.Attributes.Add("onchange", "MoneyFormat(this)");
            txtValorMensual.Attributes.Add("onchange", "MoneyFormat(this)");
            txtOtrosGastos.Attributes.Add("onchange", "MoneyFormat(this)");

        }

        /// <summary>
        /// Metodo utilizado para diligenciar el fomulario, si la accion es Borrar el metodo le retornar la confirmacion.
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="codProyecto"></param>
        /// <param name="idCargo"></param>
        public string Cargar(Accion accion, string codProyecto, string idCargo)
        {
           
            hddAccion.Value = accion.ToString();
            hddCodProyecto.Value = codProyecto;
            hddIdCargo.Value = idCargo;
            if (accion == Accion.Nuevo)
            {
                btnCargo.Text = "Crear";
                LimpiarCampos();
            }
            if (accion == Accion.Editar)
            {
                btnCargo.Text = "Actualizar";
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
        /// Limpiars the campos.
        /// </summary>
        protected void LimpiarCampos()
        {
            txtCargo.Text = "";
            ddlDedicacion.SelectedIndex = 0;
            ddlTipoContratacion.SelectedIndex = 0;
            txtValorMensual.Text = "";
            txtValorAnual.Text = "";
            txtOtrosGastos.Text = "";
            txtObservacion.Text = "";
        }

        /// <summary>
        /// Llenars the campos.
        /// </summary>
        protected void llenarCampos()
        {            
            ProyectoGastosPersonal dato = RegistroActual();
            ddlDedicacion.SelectedIndex = -1;
            ddlTipoContratacion.SelectedIndex = -1;
            txtCargo.Text = dato.Cargo;
            ddlDedicacion.SelectedValue = dato.Dedicacion;
            ddlTipoContratacion.SelectedValue = dato.TipoContratacion;
            txtValorMensual.Text = dato.ValorMensual.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtValorAnual.Text = dato.ValorAnual.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtOtrosGastos.Text = dato.OtrosGastos.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtObservacion.Text = dato.Observacion;
        }

        /// <summary>
        /// Registro actual.
        /// </summary>
        /// <returns></returns>
        protected ProyectoGastosPersonal RegistroActual()
        {
            var query = (from p in consulta.Db.ProyectoGastosPersonals
                         where p.Id_Cargo == Convert.ToInt32(hddIdCargo.Value)
                         select p).First();
            return query;
        }

        /// <summary>
        /// Handles the Click event of the btnCargo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCargo_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Cargo", txtCargo.Text, true, 100);
                FieldValidate.ValidateString("Dedicación", ddlDedicacion.SelectedValue, true, 50);
                FieldValidate.ValidateString("Tipo de contratación", ddlTipoContratacion.SelectedValue, true, 20);
                FieldValidate.ValidateNumeric("Valor mensual", txtValorMensual.Text, true);
                FieldValidate.ValidateNumeric("Valor anual", txtValorAnual.Text, true);
                FieldValidate.ValidateNumeric("Otros gastos", txtOtrosGastos.Text, true);
                FieldValidate.ValidateString("Observación", txtObservacion.Text, false, 400);

                if (hddAccion.Value == Accion.Nuevo.ToString())
                {
                    ProyectoGastosPersonal dato = new ProyectoGastosPersonal();
                    dato.CodProyecto = Convert.ToInt32(hddCodProyecto.Value);
                    dato.Cargo = txtCargo.Text;
                    dato.Dedicacion = ddlDedicacion.SelectedValue;
                    dato.TipoContratacion = ddlTipoContratacion.SelectedValue;
                    var mensual = txtValorMensual.Text.Split('.');
                    dato.ValorMensual = Convert.ToDecimal(mensual[0].Replace(",", ""));
                    var anual = txtValorAnual.Text.Split('.');
                    dato.ValorAnual = Convert.ToDecimal(anual[0].Replace(",", ""));
                    var otros = txtOtrosGastos.Text.Split('.');
                    dato.OtrosGastos = Convert.ToDecimal(otros[0].Replace(",", ""));
                    dato.Observacion = txtObservacion.Text;
                    consulta.Db.ProyectoGastosPersonals.InsertOnSubmit(dato);
                }
                else if (hddAccion.Value == Accion.Editar.ToString())
                {
                    ProyectoGastosPersonal dato = RegistroActual();
                    dato.Cargo = txtCargo.Text;
                    dato.Dedicacion = ddlDedicacion.SelectedValue;
                    dato.TipoContratacion = ddlTipoContratacion.SelectedValue;
                    var mensual = txtValorMensual.Text.Split('.');
                    dato.ValorMensual = Convert.ToDecimal(mensual[0].Replace(",", ""));
                    var anual = txtValorAnual.Text.Split('.');
                    dato.ValorAnual = Convert.ToDecimal(anual[0].Replace(",", ""));
                    var otros = txtOtrosGastos.Text.Split('.');
                    dato.OtrosGastos = Convert.ToDecimal(otros[0].Replace(",", ""));
                    dato.Observacion = txtObservacion.Text;
                }

                consulta.Db.SubmitChanges();
                Error = "OK";            
             }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancelarCargo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelarCargo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            hddAccion.Value = "";
            hddCodProyecto.Value = "";
            hddIdCargo.Value = "";
        }

        /// <summary>
        /// Borrars the cargo.
        /// </summary>
        /// <returns></returns>
        protected string BorrarCargo()
        {
            //int numeroEmpleosNV = 0;
            int codigoConvocatoria = 0;
            //int recauctual = 0;
            string respuesta = "";

            var convocatoria = (from c in consulta.Db.ConvocatoriaProyectos
                                where c.CodProyecto == Convert.ToInt32(hddCodProyecto.Value)
                                select c).FirstOrDefault();
            if (convocatoria == null)
            {
                var convoca = (from c in consulta.Db.Convocatoria
                               orderby c.Id_Convocatoria descending
                               select c).FirstOrDefault();
                codigoConvocatoria = convoca.Id_Convocatoria;
            }


            //try
            //{
            //    var queryConvoca = (from cp in consulta.Db.ProyectoFinanzasIngresos
            //                        where cp.CodProyecto == Convert.ToInt32(hddCodProyecto.Value)
            //                        select new { cp.Recursos }
            //                            ).First();
            //    recauctual = queryConvoca.Recursos;
            //}
            //catch
            //{
            //    recauctual = 0;
            //}
            //try
            //{
            //    var queryConvoca = (from cp in consulta.Db.ConvocatoriaProyectos
            //                        where cp.CodProyecto == Convert.ToInt32(hddCodProyecto.Value)
            //                        select new { cp.CodConvocatoria }
            //                            ).First();
            //    codigoConvocatoria = queryConvoca.CodConvocatoria;
            //}
            //catch
            //{
            //    codigoConvocatoria = 0;
            //}
            //try
            //{
            //    var queryCant = (from p in consulta.Db.ProyectoGastosPersonals
            //                     where p.CodProyecto == Convert.ToInt32(hddCodProyecto.Value)
            //                     select new { p.CodProyecto });
            //    var queryCant2 = (from p in consulta.Db.ProyectoInsumos
            //                      from pi in consulta.Db.ProyectoProductoInsumos
            //                      where p.Id_Insumo == pi.CodInsumo &&
            //                      p.codTipoInsumo == 2 &&
            //                      p.CodProyecto == Convert.ToInt32(hddCodProyecto.Value)
            //                      select new { p.CodProyecto });
            //    numeroEmpleosNV = (queryCant.ToList().Count + queryCant2.ToList().Count) - 1;
            //}
            //catch
            //{
            //    numeroEmpleosNV = 0;
            //}
            //ConsultarSalarioSMLVNV(1, numeroEmpleosNV, codigoConvocatoria);
            //ConsultarSalarioSMLVNV(2, numeroEmpleosNV, codigoConvocatoria);
            //ConsultarSalarioSMLVNV(3, numeroEmpleosNV, codigoConvocatoria);
            //ConsultarSalarioSMLVNV(4, numeroEmpleosNV, codigoConvocatoria);
            //ConsultarSalarioSMLVNV(5, numeroEmpleosNV, codigoConvocatoria);
            //ConsultarSalarioSMLVNV(6, numeroEmpleosNV, codigoConvocatoria);


            //if (recauctual > Convert.ToInt32(NumeroSMLVNV))
            //{
            //    respuesta = "No se puede borrar. La cantidad de recursos solicitados (smlv) son superiores a los permitidos según la cantidad de empleos generados. Modifíquelos y asegúrese que sea menor o igual a " + NumeroSMLVNV + " (smlv)";
            //}
            //else
            //{
                consulta.Db.ExecuteCommand("Delete from ProyectoEmpleoCargo where codCargo={0}", Convert.ToInt32(hddIdCargo.Value));
                consulta.Db.ExecuteCommand("Delete from ProyectoGastosPersonal where Id_Cargo={0}", Convert.ToInt32(hddIdCargo.Value));
                consulta.Db.SubmitChanges();
                respuesta = "OK";
            //}
            return respuesta;
        }

        private void ConsultarSalarioSMLVNV(int regla, int numeroEmpleosNV, int codigoConvocatoria)
        {

            try
            {
                var queryRegla = (from p in consulta.Db.ConvocatoriaReglaSalarios
                                  where p.NoRegla == regla && p.CodConvocatoria == codigoConvocatoria
                                  select p).FirstOrDefault();

                int empv1 = queryRegla.EmpleosGenerados1;
                int? empv11 = queryRegla.EmpleosGenerados2;
                string lista1 = queryRegla.ExpresionLogica;
                int Salmin1 = queryRegla.SalariosAPrestar;

                switch (lista1)
                {
                    case "=":
                        if (numeroEmpleosNV == empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case "<":
                        if (numeroEmpleosNV < empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case ">":
                        if (numeroEmpleosNV > empv1)
                            NumeroSMLVNV = Salmin1.ToString();

                        break;
                    case "<=":
                        if (numeroEmpleosNV <= empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case ">=":
                        if (numeroEmpleosNV >= empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case "Entre":
                        if (numeroEmpleosNV >= empv1 && numeroEmpleosNV <= empv11)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                }
            }
            catch { }
        }

    }
}