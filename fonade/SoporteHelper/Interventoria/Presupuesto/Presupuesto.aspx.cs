using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.Utility;

namespace Fonade.SoporteHelper.Interventoria.Presupuesto
{
    public partial class Presupuesto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                gvPrespuesto.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                gvPrespuesto.Visible = true;
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var codigoProyecto = Convert.ToInt32(txtCodigoProyecto.Text);

                    var Presupuesto = (from Observacion in db.EvaluacionObservacions
                                       join Convocatorias in db.Convocatoria on Observacion.CodConvocatoria equals Convocatorias.Id_Convocatoria
                                       join SalarioVigente in db.SalariosMinimos on Convocatorias.FechaInicio.Year equals SalarioVigente.AñoSalario
                                       where Observacion.CodProyecto.Equals(codigoProyecto)
                                       orderby Observacion.CodConvocatoria descending
                                       select new {
                                           salarioRecomendado = Observacion.ValorRecomendado,
                                           salarioAnno = SalarioVigente.SalarioMinimo
                                       }).FirstOrDefault();
                    
                    if (Presupuesto == null)
                        throw new ApplicationException("No se encontro información del proyecto.");

                    lblValorRecomendadoActual.Text = Presupuesto.salarioRecomendado.GetValueOrDefault(0).ToString();
                    lblPresupuestoTotalActual.Text = (Presupuesto.salarioAnno * Presupuesto.salarioRecomendado).GetValueOrDefault(0).moneyFormat();
                    lblSalarioMinimoVigente.Text = Presupuesto.salarioAnno.moneyFormat();
                    txtValorRecomendadoActual.Text = Presupuesto.salarioRecomendado.GetValueOrDefault(0).ToString();
                    lblPresupuestoTotalNuevo.Text = (Presupuesto.salarioAnno * Presupuesto.salarioRecomendado).GetValueOrDefault(0).moneyFormat();
                }

                lblError.Visible = false;
            }
            catch (ApplicationException ex)
            {
                gvPrespuesto.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                gvPrespuesto.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" +  ex.Message;
            }
        }

        protected void txtValorRecomendadoActual_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var codigoProyecto = Convert.ToInt32(txtCodigoProyecto.Text);

                    var Presupuesto = (from Observacion in db.EvaluacionObservacions
                                       join Convocatorias in db.Convocatoria on Observacion.CodConvocatoria equals Convocatorias.Id_Convocatoria
                                       join SalarioVigente in db.SalariosMinimos on Convocatorias.FechaInicio.Year equals SalarioVigente.AñoSalario
                                       where Observacion.CodProyecto.Equals(codigoProyecto)
                                       orderby Observacion.CodConvocatoria descending
                                       select new
                                       {
                                           salarioRecomendado = Observacion.ValorRecomendado,
                                           salarioAnno = SalarioVigente.SalarioMinimo
                                       }).FirstOrDefault();

                    if (Presupuesto == null)
                        throw new ApplicationException("No se encontro información del proyecto.");

                    if( string.IsNullOrEmpty(txtValorRecomendadoActual.Text))
                        throw new ApplicationException("No puede estar en blanco los salarios minimos recomendados.");
                   
                    var SalariosRecomendado = Convert.ToInt32(txtValorRecomendadoActual.Text);
                    var SalarioAnno = Presupuesto.salarioAnno;
                    lblPresupuestoTotalNuevo.Text = (SalarioAnno * SalariosRecomendado).moneyFormat(); 
                }

                lblError.Visible = false;
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" + ex.Message;
            }
        }

        protected void btnActualizarPresupuesto_Click(object sender, EventArgs e)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var codigoProyecto = Convert.ToInt32(txtCodigoProyecto.Text);

                    var Presupuesto = (from Observacion in db.EvaluacionObservacions
                                       where Observacion.CodProyecto.Equals(codigoProyecto)
                                       orderby Observacion.CodConvocatoria descending
                                       select Observacion).FirstOrDefault();

                    if (Presupuesto == null)
                        throw new ApplicationException("No se encontro información del proyecto.");

                    if (string.IsNullOrEmpty(txtValorRecomendadoActual.Text))
                        throw new ApplicationException("No puede estar en blanco los salarios minimos recomendados.");

                    var SalariosRecomendado = Convert.ToInt32(txtValorRecomendadoActual.Text);
                    Presupuesto.ValorRecomendado = SalariosRecomendado;

                    db.SubmitChanges();
                }

                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Text = "Presupuesto actualizado correctamente";
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" + ex.Message;
            }
        }
    }
}