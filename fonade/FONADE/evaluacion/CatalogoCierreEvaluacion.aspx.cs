using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoCierreEvaluacion
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CatalogoCierreEvaluacion : Negocio.Base_Page //: System.Web.UI.Page---Se comento esta parte para poder hacer la herencia de Negocio.Base_Page y acceder a la variable usuario
    {
        Consultas consultas = new Consultas();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// retorna datatable de acuerdo a los parametros de la convocatoria
        /// </summary>
        /// <returns></returns>
        public DataTable contacto()
        {
            DataTable datatable = new DataTable();

              var parametrosConvocatoria = new DataTable();
            parametrosConvocatoria.Columns.Add("id_Parametro");
            parametrosConvocatoria.Columns.Add("nomParametro");

            var convocatorias = (from c in consultas.Db.Convocatoria
                                 where c.codOperador == usuario.CodOperador
                                 select c).ToList();
            if(convocatorias.Count() > 0)
            {
                foreach(var fila in convocatorias)
                {
                    var parametro = (from p in consultas.Db.parametro
                                     where p.nomparametro == "FechaCierreEvaluacion" + fila.NomConvocatoria.Trim()
                                     select p).FirstOrDefault();
                    if(parametro != null)
                    {
                        var row = parametrosConvocatoria.NewRow();
                        row["id_Parametro"] = parametro.id_parametro.ToString();
                        row["nomParametro"] = parametro.nomparametro;
                        parametrosConvocatoria.Rows.Add(row);
                    }
                    else
                    {
                        var row = new parametro
                        {
                            nomparametro = "FechaCierreEvaluacion" + fila.NomConvocatoria.Trim(),
                            valor = ""
                        };
                        consultas.Db.parametro.InsertOnSubmit(row);
                        consultas.Db.SubmitChanges();

                        var row2 = parametrosConvocatoria.NewRow();
                        row2["id_Parametro"] = row.id_parametro.ToString();
                        row2["nomParametro"] = row.nomparametro;
                        parametrosConvocatoria.Rows.Add(row2);
                    }
                }
            }


            return parametrosConvocatoria;
        }

        /// <summary>
        /// Modificars the specified nom parametro.
        /// </summary>
        /// <param name="nomParametro">The nom parametro.</param>
        /// <param name="id_Parametro">The identifier parametro.</param>
        public void Modificar(String nomParametro, Int32 id_Parametro)
        {

        }

        /// <summary>
        /// Handles the Click event of the LB_Editar_Nombre control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void LB_Editar_Nombre_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow filaDta = GV_Reporte.Rows[indicefila];

            Int64 idNomParametro = Int64.Parse(GV_Reporte.DataKeys[filaDta.RowIndex].Value.ToString());

            HttpContext.Current.Session["idNomParametro"] = idNomParametro;

            Response.Redirect("CatalogoCierreEvaluacionModificar.aspx");
        }
    }
}