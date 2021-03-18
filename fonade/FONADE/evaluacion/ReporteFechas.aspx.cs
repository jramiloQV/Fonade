using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class ReporteFechas : Negocio.Base_Page
    {
        String idConvocatoriaEval;
        String nomConvocatoriaEval;

        DataTable informacionGeneral;

        DataTable datatable;

        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!String.IsNullOrEmpty(HttpContext.Current.Session["idConvocatoriaEval"].ToString()))
            //    {
                    idConvocatoriaEval = HttpContext.Current.Session["idConvocatoriaEval"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["idConvocatoriaEval"].ToString()) ? HttpContext.Current.Session["idConvocatoriaEval"].ToString() : "0";
                    nomConvocatoriaEval = HttpContext.Current.Session["idNombreConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["idNombreConvocatoria"].ToString()) ? HttpContext.Current.Session["idNombreConvocatoria"].ToString() : "0";
            //    }
            //}
            //catch (NullReferenceException)
            //{
            //    Response.Redirect("ReportesEvaluacion");
            //}

            L_ReportesEvaluacion.Text = "REPORTE #EVALUADORES POR SECTOR - " + nomConvocatoriaEval;
            ////L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;

            informacionInicial();

            llenarGrilla();

            GV_Datos.DataSource = datatable;
            GV_Datos.DataBind();
        }

        private void informacionInicial()
        {
            #region Diego Quiñonez - 16 de Enero de 2015

            string txtSQL = "select fechainicio as FechaInicio, DATEDIFF(day, fechainicio, getdate()) as DIAS, nombres+' '+apellidos as Evaluador, " +
            "id_proyecto as Id_Proyecto, nomproyecto as NomProyecto, codigo as Codigo, nomsubsector as NomSubSector " +
            "from proyecto " +
            "inner join proyectocontacto pc on id_proyecto=pc.codproyecto and codrol=" + Constantes.CONST_RolEvaluador + " and pc.inactivo=0 " +
            "inner join convocatoriaproyecto cp on pc.codconvocatoria=cp.codconvocatoria and id_proyecto = cp.codproyecto and cp.codconvocatoria=" + idConvocatoriaEval + " " +
            "inner join contacto on id_contacto = pc.codcontacto " +
            "inner join subsector on id_subsector=codsubsector " +
            "order by id_contacto";

            informacionGeneral = consultas.ObtenerDataTable(txtSQL, "text");

            #endregion

            #region codigo anterior
            //informacionGeneral = new DataTable();

            //informacionGeneral.Columns.Add("FechaInicio");
            //informacionGeneral.Columns.Add("DIAS");
            //informacionGeneral.Columns.Add("Evaluador");
            //informacionGeneral.Columns.Add("Id_Proyecto");
            //informacionGeneral.Columns.Add("NomProyecto");
            //informacionGeneral.Columns.Add("Codigo");
            //informacionGeneral.Columns.Add("NomSubSector");

            //StringBuilder sql= new StringBuilder() ;

            //sql.Append("SELECT [FechaInicio], DATEDIFF(DAY , [FechaInicio], GETDATE()) AS DIAS, [Nombres] + ' ' ");
            //sql.Append(" [Apellidos] AS Evaluador, [Id_Proyecto], [NomProyecto], [Codigo], [NomSubSector] ");
            //sql.Append(" FROM [Proyecto] ");
            //sql.Append(" INNER JOIN [ProyectoContacto] AS PC ON [Id_Proyecto] = PC.[CodProyecto] AND ");
            //sql.Append("[CodRol] = " + Constantes.CONST_RolEvaluador + " AND PC.[Inactivo] = 0 ");
            //sql.Append(" INNER JOIN [ConvocatoriaProyecto] AS CP ON PC.[CodConvocatoria] = CP.[CodConvocatoria] AND ");
            //sql.Append("[Id_Proyecto] = CP.[CodProyecto] AND CP.[CodConvocatoria] = " + idConvocatoriaEval + "  ");
            //sql.Append(" INNER JOIN [Contacto] ON [Id_Contacto] = PC.[CodContacto] ");
            //sql.Append(" INNER JOIN [SubSector] ON [Id_SubSector] = [CodSubSector] ORDER BY [Id_Contacto]");

            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            //SqlCommand cmd = new SqlCommand(sql.ToString(), conn);

            //try
            //{
            //    conn.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        DataRow fila = informacionGeneral.NewRow();
            //        fila["FechaInicio"] = reader["FechaInicio"].ToString();
            //        fila["DIAS"] = reader["DIAS"].ToString();
            //        fila["Evaluador"] = reader["Evaluador"].ToString();
            //        fila["Id_Proyecto"] = reader["Id_Proyecto"].ToString();
            //        fila["NomProyecto"] = reader["NomProyecto"].ToString();
            //        fila["Codigo"] = reader["Codigo"].ToString();
            //        fila["NomSubS ector"] = reader["NomSubSector"].ToString();
            //        informacionGeneral.Rows.Add(fila);
            //    }
            //    reader.Close();
            //}
            //catch (SqlException)
            //{
            //}
            //finally
            //{
            //    conn.Close();
            //}
            #endregion
        }

        private void llenarGrilla()
        {
            datatable = new DataTable();

            datatable.Columns.Add("FechaInicio");
            datatable.Columns.Add("DIAS");
            datatable.Columns.Add("Evaluador");
            datatable.Columns.Add("NomProyecto");
            datatable.Columns.Add("CIIU");
            datatable.Columns.Add("Tarea1");
            datatable.Columns.Add("Tarea2");
            datatable.Columns.Add("Tarea3");
            datatable.Columns.Add("Agendo");
            datatable.Columns.Add("Responsable");

            for (int i = 0; i < informacionGeneral.Rows.Count; i++)
            {
                DataRow fila = datatable.NewRow();

                fila["FechaInicio"] = informacionGeneral.Rows[i]["FechaInicio"].ToString();
                fila["DIAS"] = informacionGeneral.Rows[i]["DIAS"].ToString();
                fila["Evaluador"] = informacionGeneral.Rows[i]["Evaluador"].ToString();
                fila["NomProyecto"] = informacionGeneral.Rows[i]["NomProyecto"].ToString();
                fila["CIIU"] = informacionGeneral.Rows[i]["Codigo"].ToString() + " - " + informacionGeneral.Rows[i]["NomSubSector"].ToString();

                string txtSQL = "select nomtareausuario, fecha, DATEDIFF(hour, fecha, getdate()) dias, "+
					"a.nombres+' '+a.apellidos as agendo, ag.nomgrupo grupoagendo, "+
					"c.nombres+' '+c.apellidos as responsable, cg.nomgrupo gruporesponsable "+
					"from tareausuario tu "+
					"inner join tareausuariorepeticion on id_tareausuario =codtareausuario and fechacierre is null "+
					"inner join contacto a on a.id_contacto = tu.codcontactoagendo and a.inactivo=0 "+
					"inner join contacto c on c.id_contacto = tu.codcontacto and c.inactivo=0 "+
					"inner join grupocontacto gc on c.id_contacto = gc.codcontacto "+
					"inner join grupocontacto ga on a.id_contacto = ga.codcontacto "+
					"inner join grupo cg on cg.id_grupo = gc.codgrupo "+
					"inner join grupo ag on ag.id_grupo = ga.codgrupo "+
                    "where codproyecto=" + informacionGeneral.Rows[i]["Id_Proyecto"].ToString() + " and codtareaprograma = " + Constantes.CONST_PostIt + " " +
					"order by dias";

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand(txtSQL, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        do
                        {
                            fila["Tarea1"] = "";
                            fila["Tarea2"] = "";
                            fila["Tarea3"] = "";
                            if (Int32.Parse(reader["DIAS"].ToString()) >= 24 && Int32.Parse(reader["DIAS"].ToString()) < 48)
                            {
                                fila["Tarea1"] = reader["NomTareaUsuario"].ToString();
                            }
                            else
                            {
                                if (Int32.Parse(reader["DIAS"].ToString()) >= 48 && Int32.Parse(reader["DIAS"].ToString()) < 72)
                                {
                                    fila["Tarea2"] = reader["NomTareaUsuario"].ToString();
                                }
                                else
                                {
                                    fila["Tarea3"] = reader["NomTareaUsuario"].ToString();
                                }
                            }

                            fila["Agendo"] = reader["agendo"].ToString() + " (" + reader["GrupoAgendo"].ToString() + ")";
                            fila["Responsable"] = reader["responsable"].ToString() + " (" + reader["GrupoResponsable"].ToString() + ")";

                            datatable.Rows.Add(fila);
                            fila = datatable.NewRow();
                            fila["FechaInicio"] = "";
                            fila["DIAS"] = "";
                            fila["Evaluador"] = "";
                            fila["NomProyecto"] = "";
                            fila["CIIU"] = "";
                        } while (reader.Read());
                    }
                    else
                    {
                        fila["Tarea1"] = "";
                        fila["Tarea2"] = "";
                        fila["Tarea3"] = "";
                        fila["Agendo"] = "";
                        fila["Responsable"] = "";
                        datatable.Rows.Add(fila);
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        protected void GV_Datos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells[5].Text == "&nbsp;"
                && e.Row.Cells[6].Text == "&nbsp;"
                && e.Row.Cells[7].Text == "&nbsp;"
                && e.Row.Cells[8].Text == "&nbsp;"
                && e.Row.Cells[9].Text == "&nbsp;")
            {
                e.Row.Cells[5].ColumnSpan = 5;
                e.Row.Cells[5].Text = "No hay Tareas Pendientes";
            }
        }
    }
}