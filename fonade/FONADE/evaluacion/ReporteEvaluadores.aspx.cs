using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class ReporteEvaluadores : System.Web.UI.Page
    {
        String idConvocatoriaEval;
        String idDepartamentoEval;
        String nomConvocatoriaEval;
        String []idDepartamentos;

        DataTable datatable;
        DataTable informacion;

        String nombreDepartamentoRep = "";
        String nombreDepartamentoMos = "";
        String nombreUsuarioRep = "";
        String nombreUsuarioMos = "";
        String nombreCIIURep = "";
        String nombreCIIUMos = "";
        String nombreMunicipioRep = "";
        String nombreMunicipioMos = "";
        String nombreCuantos = "";

        String BoldNe = "";

        Int32 SubTotalEvaluadores = 0;
        Int32 SubTotalProyectos = 0;
        Int32 TotalProyectos = 0;

        Int32 j = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Session["idConvocatoriaEval"].ToString()))
                {
                    idConvocatoriaEval = HttpContext.Current.Session["idConvocatoriaEval"].ToString();
                    idDepartamentoEval = HttpContext.Current.Session["idDepartamentoEval"].ToString();
                    nomConvocatoriaEval = HttpContext.Current.Session["idNombreConvocatoria"].ToString();

                    idDepartamentos = idDepartamentoEval.Split(' ');
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("ReportesEvaluacion");
            }

            L_ReportesEvaluacion.Text = "REPORTE EVALUADORES POR DEPARTAMENTO - " + nomConvocatoriaEval;
            //L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;

            InformacionDepartamento();
            estructura();

            GV_Reporte.DataSource = datatable;
            GV_Reporte.DataBind();

            completar();

            L_textTotalPlanes.Text = "" + TotalProyectos;
        }

        private void InformacionDepartamento()
        {
            informacion = new DataTable();

            informacion.Columns.Add("Id_Contacto");
            informacion.Columns.Add("Nombre");
            informacion.Columns.Add("Codigo");
            informacion.Columns.Add("NomSubSector");
            informacion.Columns.Add("Cuantos");
            informacion.Columns.Add("NomCiudad");
            informacion.Columns.Add("NomDepartamento");

            for (int i = 1; i < idDepartamentos.Length; i++)
            {
                String sql = "SELECT distinct [Id_Contacto], [Nombres] + ' ' + [Apellidos] AS Nombre, [Codigo], [NomSubSector], COUNT([Id_Proyecto]) AS Cuantos, [NomCiudad], [NomDepartamento] FROM [Contacto] AS C INNER JOIN [ProyectoContacto] AS PC ON [Id_Contacto] = PC.[CodContacto] AND PC.[Inactivo] = 0 AND [CodRol] = " + Constantes.CONST_RolEvaluador + " INNER JOIN [Proyecto] AS P ON [Id_Proyecto] = PC.[CodProyecto] INNER JOIN [ConvocatoriaProyecto] AS CP ON PC.[CodConvocatoria] = CP.[CodConvocatoria] AND [Id_Proyecto] = CP.[CodProyecto] AND CP.[CodConvocatoria] = " + idConvocatoriaEval + " INNER JOIN [Ciudad] ON [Id_Ciudad] = P.[CodCiudad] INNER JOIN [departamento] ON [Id_Departamento] = [CodDepartamento] INNER JOIN [SubSector] ON [Id_SubSector] = [CodSubSector] WHERE C.[Inactivo] = 0 AND [CodDepartamento] IN(" + idDepartamentos[i] + ") GROUP BY [Id_Contacto], [Nombres], [Apellidos], [Codigo], [NomSubSector], [Id_Proyecto], [NomCiudad], [NomDepartamento] ORDER BY [NomDepartamento], Nombre";

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DataRow fila = informacion.NewRow();
                        fila["Id_Contacto"] = reader["Id_Contacto"].ToString();
                        fila["Nombre"] = reader["Nombre"].ToString();
                        fila["Codigo"] = reader["Codigo"].ToString();
                        fila["NomSubSector"] = reader["NomSubSector"].ToString();
                        fila["Cuantos"] = reader["Cuantos"].ToString();
                        fila["NomCiudad"] = reader["NomCiudad"].ToString();
                        fila["NomDepartamento"] = reader["NomDepartamento"].ToString();
                        informacion.Rows.Add(fila);
                    }
                    reader.Close();
                }
                catch (SqlException)
                {
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void estructura()
        {
            datatable = new DataTable();

            datatable.Columns.Add("Departamento");
            datatable.Columns.Add("Evaluador");
            datatable.Columns.Add("Profesion");
            datatable.Columns.Add("CIIU");
            datatable.Columns.Add("Planes");
            datatable.Columns.Add("Municipio");

            for (int i = 0; i < informacion.Rows.Count; i++)
            {
                String sql = @"SELECT [NomNivelEstudio], [TituloObtenido] FROM [ContactoEstudio] INNER JOIN [NivelEstudio] ON [Id_NivelEstudio] = [CodNivelEstudio] WHERE [CodContacto] = " + informacion.Rows[i]["Id_Contacto"].ToString() + " ORDER BY [NomNivelEstudio]";

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        do
                        {
                            //if (j == 0)
                            //{
                                validarRep(i);
                                DataRow filaDatatable = datatable.NewRow();
                                
                                filaDatatable["Departamento"] = nombreDepartamentoMos;
                                filaDatatable["Evaluador"] = nombreUsuarioMos;
                                try
                                {
                                    if ((informacion.Rows[i - 1]["Nombre"].ToString().Equals(informacion.Rows[i]["Nombre"].ToString())) && (!(informacion.Rows[i - 1]["Codigo"].ToString() + " - " + informacion.Rows[i - 1]["NomSubSector"].ToString()).Equals(informacion.Rows[i]["Codigo"].ToString() + " - " + informacion.Rows[i]["NomSubSector"].ToString())))
                                        filaDatatable["Profesion"] = "";
                                    else
                                        filaDatatable["Profesion"] = "- " + reader["NomNivelEstudio"].ToString() + ": " + reader["TituloObtenido"].ToString();
                                }
                                catch (IndexOutOfRangeException) {
                                    filaDatatable["Profesion"] = "- " + reader["NomNivelEstudio"].ToString() + ": " + reader["TituloObtenido"].ToString();
                                }

                                filaDatatable["CIIU"] = nombreCIIUMos;
                                filaDatatable["Planes"] = nombreCuantos;
                                filaDatatable["Municipio"] = nombreMunicipioMos;

                                if(validaImpresion(i))
                                datatable.Rows.Add(filaDatatable);
                            //}
                        }
                        while (reader.Read());
                    }
                    else
                    {
                        validarRep(i);
                        DataRow filaDatatable = datatable.NewRow();
                        
                        filaDatatable["Departamento"] = nombreDepartamentoMos;
                        filaDatatable["Evaluador"] = nombreUsuarioMos;
                        filaDatatable["Profesion"] = "";
                        filaDatatable["CIIU"] = nombreCIIUMos;
                        filaDatatable["Planes"] = informacion.Rows[i]["Cuantos"].ToString();
                        filaDatatable["Municipio"] = nombreMunicipioMos;
                        if (validaImpresion(i))
                        datatable.Rows.Add(filaDatatable);
                    }
                    reader.Close();
                }
                catch (SqlException)
                {
                    
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                if (i + 1 == informacion.Rows.Count)
                {
                    filaComplemento(i+1);
                }
            }
        }

        private Boolean validaImpresion(Int32 i)
        {
            if (String.IsNullOrEmpty(nombreDepartamentoMos) && String.IsNullOrEmpty(nombreUsuarioMos) && String.IsNullOrEmpty(nombreCIIUMos) && String.IsNullOrEmpty(nombreMunicipioMos) && String.IsNullOrEmpty(nombreCuantos))
            {
                try
                {
                    if ((informacion.Rows[i - 1]["Nombre"].ToString().Equals(informacion.Rows[i]["Nombre"].ToString())) && (!(informacion.Rows[i - 1]["Codigo"].ToString() + " - " + informacion.Rows[i - 1]["NomSubSector"].ToString()).Equals(informacion.Rows[i]["Codigo"].ToString() + " - " + informacion.Rows[i]["NomSubSector"].ToString())))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private void filaComplemento(Int32 i)
        {
            DataRow filaDatatable = datatable.NewRow();

            filaDatatable["Departamento"] = "SubTotal Evaluadores:";
            filaDatatable["Evaluador"] = SubTotalEvaluadores-1;
            filaDatatable["Profesion"] = "";
            filaDatatable["CIIU"] = "SubTotal Planes:";
            filaDatatable["Planes"] = SubTotalProyectos;
            filaDatatable["Municipio"] = "";
            datatable.Rows.Add(filaDatatable);

            BoldNe += "" + (i) + ",";
        }

        private void completar()
        {
            String []div = BoldNe.Split(',');
            for (int i = 0; i < div.Length-1; i++)
            {
                GridViewRow GV_Fila = GV_Reporte.Rows[Int32.Parse(div[i])];
                Label titulo1 = (Label)GV_Fila.FindControl("Label1");
                Label titulo2 = (Label)GV_Fila.FindControl("Label2");

                titulo1.Font.Bold = true;
                titulo2.Font.Bold = true;
            }

            GV_Reporte.DataBind();
        }

        private void validarRep(Int32 i)
        {
            try
            {
                if (!nombreDepartamentoRep.Equals(informacion.Rows[i]["NomDepartamento"].ToString()))
                {
                    nombreDepartamentoRep = informacion.Rows[i]["NomDepartamento"].ToString();
                    nombreDepartamentoMos = informacion.Rows[i]["NomDepartamento"].ToString();
                    

                    if (i != 0 || i+1 == informacion.Rows.Count)
                    {
                        filaComplemento(i+1);
                    }

                    SubTotalProyectos = Int32.Parse(informacion.Rows[i]["Cuantos"].ToString());
                    SubTotalEvaluadores = 1;
                }
                else
                {
                    nombreDepartamentoMos = "";
                    //if (i == 0)
                    //{
                    // SubTotalProyectos = Int32.Parse(informacion.Rows[i]["Cuantos"].ToString());
                    //}
                    //else
                    //{
                    if (!String.IsNullOrEmpty(nombreCuantos))
                    SubTotalProyectos += Int32.Parse(informacion.Rows[i]["Cuantos"].ToString());

                    if(!String.IsNullOrEmpty(nombreUsuarioMos))
                    SubTotalEvaluadores++;
                    //}
                }

                if (!nombreUsuarioRep.Equals(informacion.Rows[i]["Nombre"].ToString()))
                {
                    nombreUsuarioRep = informacion.Rows[i]["Nombre"].ToString();
                    nombreUsuarioMos = informacion.Rows[i]["Nombre"].ToString();

                    ///SubTotalEvaluadores++;
                }
                else
                {
                    nombreUsuarioMos = "";
                }

                if (!nombreCIIURep.Equals(informacion.Rows[i]["Codigo"].ToString() + " - " + informacion.Rows[i]["NomSubSector"].ToString()))
                {
                    nombreCIIURep = informacion.Rows[i]["Codigo"].ToString() + " - " + informacion.Rows[i]["NomSubSector"].ToString();
                    nombreCIIUMos = informacion.Rows[i]["Codigo"].ToString() + " - " + informacion.Rows[i]["NomSubSector"].ToString();
                    //j = 0;
                }
                else
                {
                    nombreCIIUMos = "";
                    //j++;
                }

                if (!nombreMunicipioRep.Equals(informacion.Rows[i]["NomCiudad"].ToString()))
                {
                    nombreMunicipioRep = informacion.Rows[i]["NomCiudad"].ToString();
                    nombreMunicipioMos = informacion.Rows[i]["NomCiudad"].ToString();
                    nombreCuantos = informacion.Rows[i]["Cuantos"].ToString();

                    TotalProyectos += Int32.Parse(nombreCuantos);
                }
                else
                {
                    TotalProyectos += Int32.Parse(nombreCuantos);
                    nombreMunicipioMos = "";
                }
            }
            catch (Exception) { }
        }
    }
}