#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>08 - 03 - 2014</Fecha>
// <Archivo>EvaluacionTablaFrame.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Web;
using System.Web.UI;
using Datos;
using System.Data;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionTablaFrame : Page
    {
        public int AspectoId;

        Consultas consultas = new Consultas();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                CargarCampos();
            }

            TabVisibility();
        }

        public void CargarCampos()
        {
            AspectoId = !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
        }

        private void TabVisibility()
        {
            var txtSql = "Select codCampo from ConvocatoriaCampo where codCampo in(Select id_campo from Campo where codCampo Is NULL) and codConvocatoria = " + Session["CodConvocatoria"].ToString();
            var dt = consultas.ObtenerDataTable(txtSql, "text");

            /* Id campos aspectos
             * 1: Generales
             * 2: Comerciales
             * 3: Tecnicos
             * 4: Organizacionales
             * 5: Financieros
             * 6: Medio Ambiente 
             * */

            if(dt.Rows.Count > 0)
            {
                foreach(DataRow r in dt.Rows )
                {
                    var id = int.Parse(r.ItemArray[0].ToString());
                    switch (id)
                    {
                        case 1:
                            tc_generales.Visible = true;
                            break;
                        case 2:
                            tc_frmcomercial.Visible = true;
                            break;
                        case 3:
                            tc_tecnico.Visible = true;
                            break;
                        case 4:
                            tc_organizacional.Visible = true;
                            break;
                        case 5:
                            tc_financiero.Visible = true;
                            break;
                        case 6:
                            tc_medio.Visible = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}