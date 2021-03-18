#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>15 - 03 - 2014</Fecha>
// <Archivo>ImprimirActa.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using System.Threading;
using System.Globalization;
#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class ImprimirActa : Base_Page
    {
        private int lvalor;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BuscarActaId();
            }
        }

        private void BuscarActaId()
        {
            int idacta = !string.IsNullOrEmpty(Request["CodActa"]) ? Convert.ToInt32(Request["CodActa"]) : 0;
            ViewState["idacta"] = idacta;

            if (idacta != 0)
            {
                var actas = consultas.Db.EvaluacionActas.FirstOrDefault(a => a.Id_Acta == idacta);

                if (actas != null && actas.Id_Acta != 0)
                {
                    string NombreConvocatoria = "";
                    string NumeroActa = "";
                    string dia = "", ano="";
                    int mes = 0;
                    

                    nroconvocatoria.Text = actas.NumActa;
                    nomconvocatoria.Text = actas.NomActa;

                    NombreConvocatoria = actas.Convocatoria.NomConvocatoria;
                    NumeroActa = actas.NumActa;

                    fecha.Text = actas.FechaActa.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    dia = actas.FechaActa.Day.ToString();
                    mes = actas.FechaActa.Month;
                    ano = actas.FechaActa.Year.ToString();

                    observaciones.Text = actas.Observaciones;
                    convocatoria.Text = actas.Convocatoria.NomConvocatoria;
                    int salario = actas.Convocatoria.FechaInicio.Year;
                    Obtenersalariominimo(salario);
                    if (actas.CodConvocatoria != null) CargarProyectoNegocio((int)actas.CodConvocatoria);


                    lblTituloActa.Text = "DESFORMALIZACIÓN DE PLANES DE NEGOCIO PRESENTADOS A LA " + NombreConvocatoria.ToUpper() + ","
                                        + "SEGÚN ACTA DEL CONSEJO DIRECTIVO NACIONAL DEL SENA N° " + NumeroActa.ToUpper() + ", "
                                        + "CORRESPONDIENTE A LA SESIÓN REALIZADA EL "+dia+" DE "+NombreMes(mes).ToUpper() +" DE "+ ano;
                }
            }
        }

        public string NombreMes(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        void Obtenersalariominimo(int ano)
        {
            var salario = consultas.Db.SalariosMinimos.FirstOrDefault(s => s.AñoSalario == ano);

            if (salario != null && salario.Id_SalariosMinimos != 0)
            {
                ViewState["salario"] = salario.SalarioMinimo;
            }
        }

        private void CargarProyectoNegocio(int _convocatoria)
        {
            try
            {
                var proyectoDeEvaluacion = (from p in consultas.Db.pr_ProyectosEvaluados(Convert.ToInt32(ViewState["idacta"]), _convocatoria) select p).ToList();

                if (proyectoDeEvaluacion.Count != 0)
                {

                    GrvPlanesNegocio.DataSource = proyectoDeEvaluacion;
                    GrvPlanesNegocio.DataBind();
                }
            }
            catch (Exception ex)
            {
                GrvPlanesNegocio.DataSource = null;
                GrvPlanesNegocio.DataBind();
            }
        }

        protected void GrvPlanesNegocioRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var valor = e.Row.FindControl("lvalor") as Label;


                if (valor != null && !string.IsNullOrEmpty(valor.Text))
                {
                    lvalor += Convert.ToInt32(valor.Text);
                    ltsalario.Text = lvalor.ToString();

                }

                ltotal.Text = ((lvalor) * (Convert.ToDouble(ViewState["salario"].ToString()))).ToString("c");
            }
        }
    }
}
