using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using System.Globalization;

namespace Fonade.FONADE.Priorizacion_de_Proyectos
{
    public partial class VerActaAsignacionRecursos : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                if (Session["Id_Acta"] != null)
                {
                    int idActa = Convert.ToInt32(Session["Id_Acta"]);
                    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        var acta = (from actas in db.AsignacionActas
                                    join convocatoria in db.Convocatoria on actas.CodConvocatoria equals convocatoria.Id_Convocatoria
                                    where actas.Id_Acta == idActa
                                    select new
                                    {
                                        Numero = actas.NumActa,
                                        Nombre = actas.NomActa,
                                        Fecha = actas.FechaActa.GetValueOrDefault(DateTime.Now).Date,
                                        Observaciones = actas.Observaciones
                                    }).ToList().FirstOrDefault();

                        if(acta == null)
                            throw new ApplicationException("No se pudo obtener la información del acta, intentelo de nuevo.");

                        lblNumero.Text = acta.Numero;
                        lblNombre.Text = acta.Nombre;
                        lblFecha.Text = acta.Fecha.ToString("DD/MM/YYYY");
                        txtObservaciones.Text = acta.Observaciones;                        
                    }
                    calcularSalariosYRecursos(idActa);               
                }
                else
                {
                    throw new ApplicationException("No se pudo obtener el codigo del acta, intentelo de nuevo.");
                }                
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

        private void calcularSalariosYRecursos(int idActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from actas in db.AsignacionActas
                                join proyectosActa in db.AsignacionActaProyecto on actas.Id_Acta equals proyectosActa.CodActa
                                join evaluacionObservacion in db.EvaluacionObservacions on proyectosActa.CodProyecto equals evaluacionObservacion.CodProyecto
                                join convocatoria in db.Convocatoria on actas.CodConvocatoria equals convocatoria.Id_Convocatoria
                                join salarios in db.SalariosMinimos on convocatoria.FechaFin.Year equals salarios.AñoSalario
                                join proyecto in db.Proyecto on proyectosActa.CodProyecto equals proyecto.Id_Proyecto
                                where actas.Id_Acta == idActa
                                select new ProyectoActa
                                {
                                    Codigo = evaluacionObservacion.CodProyecto,
                                    Nombre = proyecto.NomProyecto,
                                    NombreConvocatoria = convocatoria.NomConvocatoria,
                                    ValorRecomendado = evaluacionObservacion.ValorRecomendado.GetValueOrDefault(0),
                                    Asignado = proyectosActa.Asignado.GetValueOrDefault(false),
                                    SalarioMinimo = salarios.SalarioMinimo,
                                    Anio = salarios.AñoSalario
                                }).ToList();    

                Decimal totalSalarios = 0;
                Decimal totalRecursos = 0;
                entities.ForEach(filter =>
                {
                    totalSalarios += Convert.ToDecimal(filter.ValorRecomendado);
                    totalRecursos += Convert.ToDecimal((filter.ValorRecomendado * filter.SalarioMinimo));
                });

                if (totalSalarios == 0)
                    lblTotalSalariosMinimos.Text = "0";
                else
                    lblTotalSalariosMinimos.Text = FieldValidate.moneyFormat(totalSalarios, false);

                if (totalRecursos == 0)
                    lblTotalRecursos.Text = "$0.00";
                else
                    lblTotalRecursos.Text = FieldValidate.moneyFormat(totalRecursos, true) + ".00";   
            }
        }

        protected void btn_Imprimir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('ImprimirActaAsignacion.aspx','_blank','width=680,height=680,toolbar=no, scrollbars=1, resizable=no');", true);
        }

        /// <summary>
        /// Listado de proyectos acta
        /// </summary>                
        public List<ProyectoActa> getProyectosActa(int idActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener los proyectos
                var entities = (from actas in db.AsignacionActas
                                join proyectosActa in db.AsignacionActaProyecto on actas.Id_Acta equals proyectosActa.CodActa
                                join evaluacionObservacion in db.EvaluacionObservacions on proyectosActa.CodProyecto equals evaluacionObservacion.CodProyecto
                                join convocatoria in db.Convocatoria on actas.CodConvocatoria equals convocatoria.Id_Convocatoria
                                join salarios in db.SalariosMinimos on convocatoria.FechaFin.Year equals salarios.AñoSalario
                                join proyecto in db.Proyecto on proyectosActa.CodProyecto equals proyecto.Id_Proyecto
                                where actas.Id_Acta == idActa
                                select new ProyectoActa
                                {
                                    Codigo = evaluacionObservacion.CodProyecto,
                                    Nombre = proyecto.NomProyecto,                                    
                                    NombreConvocatoria = convocatoria.NomConvocatoria,
                                    ValorRecomendado = evaluacionObservacion.ValorRecomendado.GetValueOrDefault(0),
                                    Asignado = proyectosActa.Asignado.GetValueOrDefault(false),                                    
                                    SalarioMinimo = salarios.SalarioMinimo,
                                    Anio = salarios.AñoSalario                                    
                                }).ToList();          
                
                return entities;           
        }
    }

        public class ProyectoActa
        {
            public int Codigo { get; set; }
            public string Nombre { get; set; }
            public string NombreConvocatoria { get; set; }
            public Double ValorRecomendado { get; set; }
            public Boolean Asignado { get; set; }
            public string Recursos
            {
                get
                {
                    return Asignado == true ? "SI" : "NO";
                }
                set
                {
                }
            }
            public Int64 SalarioMinimo { get; set; }
            public int Anio { get; set; }
        }        
    }
}