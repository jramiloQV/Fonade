using Datos;
using Fonade.Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class ReporteEmpleo : Base_Page
    {
        String idConvocatoriaEval;
        String nomConvocatoriaEval;
        Int64 idPagina;

        String puntajeMinimo;
        String SMLV;
        String AnioSalario;

        String txtDepartamento = "";
        String txtSector = "";

        Int64 TotalDeptoDirecto;
        Int64 TotalDeptoPrimerAno;
        Int64 TotalDepto18a24;
        Int64 TotalDeptoDesplazados;
        Int64 TotalDeptoMadres;
        Int64 TotalDeptoMinorias;
        Int64 TotalDeptoRecluidos;
        Int64 TotalDeptoDesmovilizados;
        Int64 TotalDeptoDiscapacitados;
        Int64 TotalDeptoDesvinculados;
        Int64 TotalDeptoRecomendado;
        Int64 TotalDeptoSolicitado;
        Int64 TotalDeptoNegado;

        Int64 SubTotalEmpleosSena;
        Int64 SubTotalEmpleosUniv;
        Int64 SubTotalViablesSena;
        Int64 SubTotalViablesUniv;
        Int64 SubTotalRecomendadoSena;
        Int64 SubTotalRecomendadoUniv;
        Int64 SubTotalNegadoSena;
        Int64 SubTotalNegadoUniv;
        Int64 SubTotalEmpleos;

        Int64 SubTotalDirecto;
        Int64 SubTotalPrimerAno;
        Int64 SubTotal18a24;
        Int64 SubTotalDesplazados;
        Int64 SubTotalMadres;
        Int64 SubTotalMinorias;
        Int64 SubTotalRecluidos;
        Int64 SubTotalDesmovilizados;
        Int64 SubTotalDiscapacitados;
        Int64 SubTotalDesvinculados;
        Int64 SubTotalRecomendado;
        Int64 SubTotalSolicitado;
        Int64 SubTotalNegado;

        Int64 TotalEmpleosPlan;

        Int64 TotalViablesSena;
        Int64 TotalViablesUniv;
        Int64 TotalDirecto;
        Int64 TotalPrimerAno;
        Int64 Total18a24;
        Int64 TotalDesplazados;
        Int64 TotalMadres;
        Int64 TotalMinorias;
        Int64 TotalRecluidos;
        Int64 TotalDesmovilizados;
        Int64 TotalDiscapacitados;
        Int64 TotalDesvinculados;
        Int64 TotalRecomendado;
        Int64 TotalNegado;

        Int64 TotalEmpleos;

        Table tabla;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Session["idConvocatoriaEval"].ToString()))
                {
                    idConvocatoriaEval = HttpContext.Current.Session["idConvocatoriaEval"].ToString();
                    nomConvocatoriaEval = HttpContext.Current.Session["idNombreConvocatoria"].ToString();

                    String tem = Request["codPagina"];
                    idPagina = Int64.Parse(tem);
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("ReportesEvaluacion.aspx");
            }

            switch (idPagina)
            {
                case 1:
                    L_ReportesEvaluacion.Text = "REPORTE EMPLEOS POR DEPARTAMENTO Y SECTOR - " + nomConvocatoriaEval;
                    break;
                case 2:
                    L_ReportesEvaluacion.Text = "Reporte Empleos Consolidado Nacional - " + nomConvocatoriaEval;
                    break;
                case 3:
                    L_ReportesEvaluacion.Text = "Reporte Empleos Consolidado Nacional por Sector - " + nomConvocatoriaEval;
                    break;
            }
            
            //L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;

            datosGenerales();

            switch (idPagina)
            {
                case 1:
                    caso1();
                    break;
                case 2:
                    caso2();
                    break;
                case 3:
                    caso3();
                    break;
            }
        }

        private void datosGenerales()
        {
            String sql;

            sql = "SELECT  SUM([Puntaje]) AS PUNTAJE FROM [ConvocatoriaCampo] AS CC, [Campo] AS C WHERE [id_Campo] = CC.[codCampo] AND C.[codCampo] IS NULL AND [codConvocatoria] = " + idConvocatoriaEval;

            if (Int64.Parse(idConvocatoriaEval) > 1)
            {
                sql = sql + " and id_Campo!=6";
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    puntajeMinimo = reader["PUNTAJE"].ToString();
                }
                reader.Close();
                conn.Close();
            }
            catch (SqlException)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            sql = "SELECT YEAR([FechaInicio]) AS FECHA FROM [Convocatoria] WHERE [Id_Convocatoria] = " + idConvocatoriaEval;
            cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    AnioSalario = reader["FECHA"].ToString();
                }
                reader.Close();
                conn.Close();
            }
            catch (SqlException)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            sql = "SELECT [SalarioMinimo] FROM [SalariosMinimos] WHERE [AñoSalario] = " + AnioSalario;

            cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    SMLV = reader["SalarioMinimo"].ToString();
                }
                reader.Close();
                conn.Close();
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

        private void caso3()
        {
            String sql = "";

            if (Int64.Parse(idConvocatoriaEval) == 1)
            {
                sql = @"Select nomsector, nomdepartamento, count(s.codtipoinstitucion) sena, count(u.codtipoinstitucion) univ,
                    sum(EmpleoDirecto) EmpleoDirecto, sum(EmpleoPrimerAno) EmpleoPrimerAno, sum(Empleo18a24) Empleo18a24,
                    sum(EmpleoDesplazados) EmpleoDesplazados, sum(EmpleoMadres) EmpleoMadres, sum(EmpleoMinorias) EmpleoMinorias,
                    sum(EmpleoRecluidos) EmpleoRecluidos, sum(EmpleoDesmovilizados) EmpleoDesmovilizados,
                    sum(EmpleoDiscapacitados) EmpleoDiscapacitados, sum(EmpleoDesvinculados) EmpleoDesvinculados,
                    sum(valorrecomendado) valorrecomendado, sum(recursos-valorrecomendado)  negado
                    from proyecto p
                    inner join convocatoriaproyecto cp on id_proyecto = cp.codproyecto and cp.codconvocatoria=" + idConvocatoriaEval + @" and viable=1
                    inner join subsector on id_subsector = p.codsubsector
                    inner join sector on id_sector = codsector
                    inner join ciudad on id_ciudad = p.codciudad
                    inner join departamento on id_departamento = coddepartamento
                    left join institucion s on s.id_institucion=codinstitucion and s.codtipoinstitucion=" + Constantes.CONST_Sena + @"
                    left join institucion u on u.id_institucion=codinstitucion and u.codtipoinstitucion<>" + Constantes.CONST_Sena + @"
                    inner join proyectometasocial m on id_proyecto=m.codproyecto
                    inner join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
                    inner join evaluacionobservacion e on id_proyecto=e.codproyecto and cp.codconvocatoria=e.codconvocatoria
                    group by nomsector,nomdepartamento
                    order by nomsector,nomdepartamento";
            }
            else
            {
                sql = @"Select nomsector, nomdepartamento, count(s.codtipoinstitucion) sena, count(u.codtipoinstitucion) univ,
                    sum(EmpleoDirecto) EmpleoDirecto, sum(dbo.fn_EmpleosPrimerAno(id_proyecto)) EmpleoPrimerAno, sum(Empleo18a24) Empleo18a24,
                    sum(EmpleoDesplazados) EmpleoDesplazados, sum(EmpleoMadres) EmpleoMadres, sum(EmpleoMinorias) EmpleoMinorias,
                    sum(EmpleoRecluidos) EmpleoRecluidos, sum(EmpleoDesmovilizados) EmpleoDesmovilizados,
                    sum(EmpleoDiscapacitados) EmpleoDiscapacitados, sum(EmpleoDesvinculados) EmpleoDesvinculados,
                    sum(valorrecomendado) valorrecomendado, sum(recursos-valorrecomendado)  negado
                    from proyecto p
                    inner join convocatoriaproyecto cp on id_proyecto = cp.codproyecto and cp.codconvocatoria=" + idConvocatoriaEval + @" and viable=1
                    inner join subsector on id_subsector = p.codsubsector
                    inner join sector on id_sector = codsector
                    inner join ciudad on id_ciudad = p.codciudad
                    inner join departamento on id_departamento = coddepartamento
                    left join institucion s on s.id_institucion=codinstitucion and s.codtipoinstitucion=" + Constantes.CONST_Sena + @"
                    left join institucion u on u.id_institucion=codinstitucion and u.codtipoinstitucion<>" + Constantes.CONST_Sena + @"
                    inner join fn_Empleos() m on id_proyecto=m.codproyecto
                    inner join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
                    inner join evaluacionobservacion e on id_proyecto=e.codproyecto and cp.codconvocatoria=e.codconvocatoria
                    group by nomsector,nomdepartamento
                    order by nomsector,nomdepartamento";
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();


                Boolean siLlee = reader.Read();

                while (siLlee)
                {
                    TableHeaderRow filaEncabezado;
                    TableRow filaTabla;

                    if (!txtSector.Equals(reader["nomsector"].ToString()))
                    {
                        txtSector = reader["nomsector"].ToString();
                        TotalViablesSena = 0;
			            TotalViablesUniv = 0;
			            TotalDirecto = 0;
			            TotalPrimerAno = 0;
			            Total18a24 = 0;
			            TotalDesplazados = 0;
			            TotalMadres = 0;
			            TotalMinorias = 0;
			            TotalRecluidos = 0;
			            TotalDesmovilizados = 0;
			            TotalDiscapacitados = 0;
			            TotalDesvinculados = 0;
			            TotalRecomendado = 0;
                        TotalNegado = 0;

                        tabla = new Table();
                        tabla.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                        tabla.CssClass = "Grilla";

                        filaEncabezado = new TableHeaderRow();

                        filaEncabezado.Cells.Add(crearceladtitulo("Datos Generales Del Plan De Negocio", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo(txtSector, 14, ""));

                        tabla.Rows.Add(filaEncabezado);

                        filaEncabezado = new TableHeaderRow();

                        filaEncabezado.Cells.Add(crearceladtitulo("Datos por Departamento", 3, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Empleos Generados", 10, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Recursos", 2, ""));

                        tabla.Rows.Add(filaEncabezado);

                        filaEncabezado = new TableHeaderRow();

                        filaEncabezado.Cells.Add(crearceladtitulo("Departamento", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Proyectos Viables SENA", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Proyectos Viables Universidades y Otros", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Directos", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Generados Primer Año", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Entre 18 y 24 años", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Desplazados por Violencia", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Madres Cabeza de Familia", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas en minoría Étnica (Indígenas y Negritudes)", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas recluidas en las cárceles del INPEC", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas desmovilizadas o reinsertadas", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas Discapacitadas", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas Desvinculadas de las entidades del estado", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Valor Recomendado", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Valor Negado", 1, ""));

                        tabla.Rows.Add(filaEncabezado);
                    }

                    filaTabla = new TableRow();

                    filaTabla.Cells.Add(celdaNormal(reader["NomDepartamento"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["sena"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["Univ"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDirecto"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoPrimerAno"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["Empleo18a24"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesplazados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoMadres"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoMinorias"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoRecluidos"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesmovilizados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDiscapacitados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesvinculados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal("$ " + (Int64.Parse(reader["valorrecomendado"].ToString()) * Int64.Parse(SMLV)), 1));
                    filaTabla.Cells.Add(celdaNormal("$ " + (Int64.Parse(reader["negado"].ToString()) * Int64.Parse(SMLV)), 1));

                    tabla.Rows.Add(filaTabla);

                    TotalViablesSena = TotalViablesSena + Int64.Parse(reader["sena"].ToString());
		            TotalViablesUniv = TotalViablesUniv + Int64.Parse(reader["Univ"].ToString());
		            TotalDirecto = TotalDirecto + Int64.Parse(reader["EmpleoDirecto"].ToString());
		            TotalPrimerAno = TotalPrimerAno + Int64.Parse(reader["EmpleoPrimerAno"].ToString());
		            Total18a24 = Total18a24 + Int64.Parse(reader["Empleo18a24"].ToString());
		            TotalDesplazados = TotalDesplazados + Int64.Parse(reader["EmpleoDesplazados"].ToString());
		            TotalMadres = TotalMadres + Int64.Parse(reader["EmpleoMadres"].ToString());
		            TotalMinorias = TotalMinorias + Int64.Parse(reader["EmpleoMinorias"].ToString());
		            TotalRecluidos = TotalRecluidos + Int64.Parse(reader["EmpleoRecluidos"].ToString());
		            TotalDesmovilizados = TotalDesmovilizados + Int64.Parse(reader["EmpleoDesmovilizados"].ToString());
		            TotalDiscapacitados = TotalDiscapacitados + Int64.Parse(reader["EmpleoDiscapacitados"].ToString());
		            TotalDesvinculados = TotalDesvinculados + Int64.Parse(reader["EmpleoDesvinculados"].ToString());
		            TotalRecomendado = TotalRecomendado + Int64.Parse(reader["valorrecomendado"].ToString());
		            TotalNegado = TotalNegado + Int64.Parse(reader["negado"].ToString());

                    siLlee = reader.Read();

                    if (siLlee)
                    {
                        if (!txtSector.Equals(reader["nomsector"].ToString()))
                        {
                            impresion3();
                        }
                    }
                    else
                    {
                        impresion3();
                    }

                    try
                    {
                        P_principal.Controls.Add(tabla);
                        P_principal.DataBind();
                    }
                    catch (Exception) { }
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

        private void caso2()
        {
            String sql = "";

            if (Int64.Parse(idConvocatoriaEval) == 1)
            {
                sql = @"Select nomdepartamento, count(s.codtipoinstitucion) sena, count(u.codtipoinstitucion) univ,
                    sum(EmpleoDirecto) EmpleoDirecto, sum(EmpleoPrimerAno) EmpleoPrimerAno, sum(Empleo18a24) Empleo18a24,
                    sum(EmpleoDesplazados) EmpleoDesplazados, sum(EmpleoMadres) EmpleoMadres, sum(EmpleoMinorias) EmpleoMinorias,
                    sum(EmpleoRecluidos) EmpleoRecluidos, sum(EmpleoDesmovilizados) EmpleoDesmovilizados,
                    sum(EmpleoDiscapacitados) EmpleoDiscapacitados, sum(EmpleoDesvinculados) EmpleoDesvinculados,
                    sum(valorrecomendado) valorrecomendado, sum(recursos-valorrecomendado)  negado
                    from proyecto p
                    inner join convocatoriaproyecto cp on id_proyecto = cp.codproyecto and cp.codconvocatoria=" + idConvocatoriaEval + @" and viable=1
                    inner join ciudad on id_ciudad = p.codciudad
                    inner join departamento on id_departamento = coddepartamento
                    left join institucion s on s.id_institucion=codinstitucion and s.codtipoinstitucion=" + Constantes.CONST_Sena + @"
                    left join institucion u on u.id_institucion=codinstitucion and u.codtipoinstitucion<>" + Constantes.CONST_Sena + @"
                    inner join proyectometasocial m on id_proyecto=m.codproyecto
                    inner join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
                    inner join evaluacionobservacion e on id_proyecto=e.codproyecto and cp.codconvocatoria=e.codconvocatoria
                    group by nomdepartamento
                    order by nomdepartamento";
            }
            else
            {
                sql = @"Select nomdepartamento, count(s.codtipoinstitucion) sena, count(u.codtipoinstitucion) univ,
                    sum(EmpleoDirecto) EmpleoDirecto, sum(dbo.fn_EmpleosPrimerAno(id_proyecto)) EmpleoPrimerAno, sum(Empleo18a24) Empleo18a24,
                    sum(EmpleoDesplazados) EmpleoDesplazados, sum(EmpleoMadres) EmpleoMadres, sum(EmpleoMinorias) EmpleoMinorias,
                    sum(EmpleoRecluidos) EmpleoRecluidos, sum(EmpleoDesmovilizados) EmpleoDesmovilizados,
                    sum(EmpleoDiscapacitados) EmpleoDiscapacitados, sum(EmpleoDesvinculados) EmpleoDesvinculados,
                    sum(valorrecomendado) valorrecomendado, sum(recursos-valorrecomendado)  negado
                    from proyecto p
                    inner join convocatoriaproyecto cp on id_proyecto = cp.codproyecto and cp.codconvocatoria=" + idConvocatoriaEval + @" and viable=1
                    inner join ciudad on id_ciudad = p.codciudad
                    inner join departamento on id_departamento = coddepartamento
                    left join institucion s on s.id_institucion=codinstitucion and s.codtipoinstitucion=" + Constantes.CONST_Sena + @"
                    left join institucion u on u.id_institucion=codinstitucion and u.codtipoinstitucion<>" + Constantes.CONST_Sena + @"
                    inner join fn_Empleos() m on id_proyecto=m.codproyecto
                    inner join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
                    inner join evaluacionobservacion e on id_proyecto=e.codproyecto and cp.codconvocatoria=e.codconvocatoria
                    group by nomdepartamento
                    order by nomdepartamento";
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();


                Boolean siLlee = reader.Read();

                
                    TableHeaderRow filaEncabezado;
                    TableRow filaTabla;

                tabla = new Table();
                        tabla.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                        tabla.CssClass = "Grilla";

                    filaEncabezado = new TableHeaderRow();

                    filaEncabezado.Cells.Add(crearceladtitulo("CONSOLIDADO NACIONAL", 15, ""));

                    tabla.Rows.Add(filaEncabezado);

                    filaEncabezado = new TableHeaderRow();

                    filaEncabezado.Cells.Add(crearceladtitulo("Datos por Departamento", 3, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Empleos Generados", 10, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Recursos", 2, ""));

                    tabla.Rows.Add(filaEncabezado);

                    filaEncabezado = new TableHeaderRow();

                    filaEncabezado.Cells.Add(crearceladtitulo("Departamento", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Proyectos Viables SENA", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Proyectos Viables Universidades y Otros", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Directos", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Generados Primer Año", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Entre 18 y 24 años", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Desplazados por Violencia", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Madres Cabeza de Familia", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Personas en minoría Étnica (Indígenas y Negritudes)", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Personas recluidas en las cárceles del INPEC", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Personas desmovilizadas o reinsertadas", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Personas Discapacitadas", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Personas Desvinculadas de las entidades del estado", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Valor Recomendado", 1, ""));
                    filaEncabezado.Cells.Add(crearceladtitulo("Valor Negado", 1, ""));

                    tabla.Rows.Add(filaEncabezado);

                    TotalViablesSena = 0;
		            TotalViablesUniv = 0;
		            TotalDirecto = 0;
		            TotalPrimerAno = 0;
		            Total18a24 = 0;
		            TotalDesplazados = 0;
		            TotalMadres = 0;
		            TotalMinorias = 0;
		            TotalRecluidos = 0;
		            TotalDesmovilizados = 0;
		            TotalDiscapacitados = 0;
		            TotalDesvinculados = 0;
		            TotalRecomendado = 0;
                    TotalNegado = 0;

                    while (siLlee)
                    {
                        filaTabla = new TableHeaderRow();

                        filaTabla.Cells.Add(celdaNormal(reader["NomDepartamento"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["sena"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["Univ"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoDirecto"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoPrimerAno"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["Empleo18a24"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesplazados"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoMadres"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoMinorias"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoRecluidos"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesmovilizados"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoDiscapacitados"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesvinculados"].ToString(), 1));
                        filaTabla.Cells.Add(celdaNormal("$ " + (Double.Parse(reader["valorrecomendado"].ToString()) * Double.Parse(SMLV)), 1));
                        filaTabla.Cells.Add(celdaNormal("$ " + ((Double.Parse(reader["negado"].ToString()) * Double.Parse(SMLV))), 1));

                        tabla.Rows.Add(filaTabla);

                        TotalViablesSena = TotalViablesSena + Int64.Parse(reader["sena"].ToString());
			            TotalViablesUniv = TotalViablesUniv + Int64.Parse(reader["Univ"].ToString());
			            TotalDirecto = TotalDirecto + Int64.Parse(reader["EmpleoDirecto"].ToString());
			            TotalPrimerAno = TotalPrimerAno + Int64.Parse(reader["EmpleoPrimerAno"].ToString());
			            Total18a24 = Total18a24 + Int64.Parse(reader["Empleo18a24"].ToString());
			            TotalDesplazados = TotalDesplazados + Int64.Parse(reader["EmpleoDesplazados"].ToString());
			            TotalMadres = TotalMadres + Int64.Parse(reader["EmpleoMadres"].ToString());
			            TotalMinorias = TotalMinorias + Int64.Parse(reader["EmpleoMinorias"].ToString());
			            TotalRecluidos = TotalRecluidos + Int64.Parse(reader["EmpleoRecluidos"].ToString());
			            TotalDesmovilizados = TotalDesmovilizados + Int64.Parse(reader["EmpleoDesmovilizados"].ToString());
			            TotalDiscapacitados = TotalDiscapacitados + Int64.Parse(reader["EmpleoDiscapacitados"].ToString());
			            TotalDesvinculados = TotalDesvinculados + Int64.Parse(reader["EmpleoDesvinculados"].ToString());
			            TotalRecomendado = TotalRecomendado + Int64.Parse(reader["valorrecomendado"].ToString());
			            TotalNegado = TotalNegado + Int64.Parse(reader["negado"].ToString());

                        siLlee = reader.Read();
                    }
                    reader.Close();

                TotalEmpleos =  TotalDirecto+TotalPrimerAno+ Total18a24+ TotalDesplazados+ TotalMadres+ TotalMinorias+ TotalRecluidos+ TotalDesmovilizados+ TotalDiscapacitados+ TotalDesvinculados;

                filaTabla = new TableHeaderRow();
                filaTabla.CssClass = "sdP_principal";

                filaTabla.Cells.Add(celdaNormal("Total", 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalViablesSena, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalViablesUniv, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalDirecto, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalPrimerAno, 1));
                filaTabla.Cells.Add(celdaNormal("" + Total18a24, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalDesplazados, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalMadres, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalMinorias, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalRecluidos, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalDesmovilizados, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalDiscapacitados, 1));
                filaTabla.Cells.Add(celdaNormal("" + TotalDesvinculados, 1));
                filaTabla.Cells.Add(celdaNormal("$ " + (TotalRecomendado*Int64.Parse(SMLV)), 1));
                filaTabla.Cells.Add(celdaNormal("$ " + (TotalNegado*Int64.Parse(SMLV)), 1));

                tabla.Rows.Add(filaTabla);

                filaTabla = new TableHeaderRow();
                filaTabla.CssClass = "sdP_principal";

                filaTabla.Cells.Add(celdaNormal("Total Empleos Convocatoria", 8));
                filaTabla.Cells.Add(celdaNormal(""+TotalEmpleos, 7));

                tabla.Rows.Add(filaTabla);

                try
                    {
                        P_principal.Controls.Add(tabla);
                        P_principal.DataBind();
                    }
                    catch (Exception) { }
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

        private void caso1()
        {
            String sql = "";
            txtDepartamento = "";
            txtSector = "";

            if (Int64.Parse(idConvocatoriaEval) == 1)
            {
                sql = @"select nomdepartamento,nomsector, nomciudad, id_proyecto, nomproyecto, nomunidad  + ' (' +nominstitucion+')' as Unidad,
                    CodTipoInstitucion, EmpleoPrimerAno, EmpleoDirecto,  Empleo18a24, EmpleoDesplazados, EmpleoMadres,
                    EmpleoMinorias, EmpleoRecluidos, EmpleoDesmovilizados, EmpleoDiscapacitados, EmpleoDesvinculados,
                    isnull(valorrecomendado,0) valorrecomendado, recursos
                    from proyecto p
                    inner join convocatoriaproyecto cp on id_proyecto = cp.codproyecto and viable=1 and cp.codconvocatoria=" + idConvocatoriaEval + @"
                    inner join ciudad on id_ciudad = p.codciudad
                    inner join departamento on id_departamento = coddepartamento
                    inner join subsector on id_subsector = codsubsector
                    inner join sector on id_sector = codsector
                    inner join institucion on id_institucion=codinstitucion
                    inner join proyectometasocial m on id_proyecto=m.codproyecto
                    inner join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
                    inner join evaluacionobservacion e on id_proyecto=e.codproyecto and cp.codconvocatoria=e.codconvocatoria
                    order by nomdepartamento,nomsector, nomciudad";
            }
            else
            {
                sql = @"select nomdepartamento,nomsector, nomciudad, id_proyecto, nomproyecto, nomunidad  + ' (' +nominstitucion+')' as Unidad,
                    CodTipoInstitucion, m.* , dbo.fn_EmpleosPrimerAno(id_proyecto) as EmpleoPrimerAno,
                    isnull(valorrecomendado,0) valorrecomendado, recursos
                    from proyecto p
                    inner join convocatoriaproyecto cp on id_proyecto = cp.codproyecto and viable=1 and cp.codconvocatoria=" + idConvocatoriaEval + @"
                    inner join ciudad on id_ciudad = p.codciudad
                    inner join departamento on id_departamento = coddepartamento
                    inner join subsector on id_subsector = codsubsector
                    inner join sector on id_sector = codsector
                    inner join institucion on id_institucion=codinstitucion
                    inner join fn_Empleos() m on id_proyecto=m.codproyecto
                    inner join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
                    inner join evaluacionobservacion e on id_proyecto=e.codproyecto and cp.codconvocatoria=e.codconvocatoria
                    order by nomdepartamento,nomsector, nomciudad";
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();


                Boolean siLlee = reader.Read();

                while (siLlee)
                {
                    TableHeaderRow filaEncabezado;
                    TableRow filaTabla;
                    if (!txtDepartamento.Equals(reader["nomdepartamento"].ToString()))
                    {
                        txtDepartamento = reader["nomdepartamento"].ToString();
                        TotalDeptoDirecto = 0;
			            TotalDeptoPrimerAno = 0;
			            TotalDepto18a24 = 0;
			            TotalDeptoDesplazados = 0;
			            TotalDeptoMadres = 0;
			            TotalDeptoMinorias = 0;
			            TotalDeptoRecluidos = 0;
			            TotalDeptoDesmovilizados = 0;
			            TotalDeptoDiscapacitados = 0;
			            TotalDeptoDesvinculados = 0;
			            TotalDeptoRecomendado = 0;
			            TotalDeptoSolicitado = 0;
			            TotalDeptoNegado = 0;

			            SubTotalEmpleosSena = 0;
			            SubTotalEmpleosUniv = 0;
			            SubTotalEmpleos = 0;
			            SubTotalViablesSena = 0;
			            SubTotalViablesUniv = 0;
			            SubTotalRecomendadoSena = 0;
			            SubTotalRecomendadoUniv = 0;
			            SubTotalNegadoSena = 0;
                        SubTotalNegadoUniv = 0;

                        tabla = new Table();
                        tabla.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                        tabla.CssClass = "Grilla";
                    }

                    if (!txtSector.Equals(reader["nomsector"].ToString()))
                    {
                        txtSector = reader["nomsector"].ToString();
                        SubTotalDirecto = 0;
			            SubTotalPrimerAno = 0;
			            SubTotal18a24 = 0;
			            SubTotalDesplazados = 0;
			            SubTotalMadres = 0;
			            SubTotalMinorias = 0;
			            SubTotalRecluidos = 0;
			            SubTotalDesmovilizados = 0;
			            SubTotalDiscapacitados = 0;
			            SubTotalDesvinculados = 0;
			            SubTotalRecomendado = 0;
			            SubTotalSolicitado = 0;
                        SubTotalNegado = 0;

                        filadeDos("Departamento", reader["nomdepartamento"].ToString());
                        filadeDos("Sector CIIU", reader["nomsector"].ToString());

                        filaEncabezado = new TableHeaderRow();

                        filaEncabezado.Cells.Add(crearceladtitulo("Datos Generales Del Plan De Negocio", 4, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Empleos Generados", 10, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Recursos", 2, ""));

                        tabla.Rows.Add(filaEncabezado);

                        filaEncabezado = new TableHeaderRow();

                        filaEncabezado.Cells.Add(crearceladtitulo("ID", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Plan De Negocio", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Ciudad", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Unidad de Emprendimiento", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Directos", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Generados Primer Año", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Entre 18 y 24 años", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Desplazados por Violencia", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Madres Cabeza de Familia", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas en minoría Étnica (Indígenas y Negritudes)", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas recluidas en las cárceles del INPEC", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas desmovilizadas o reinsertadas", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas Discapacitadas", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Personas Desvinculadas de las entidades del estado", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Valor Recomendado", 1, ""));
                        filaEncabezado.Cells.Add(crearceladtitulo("Valor Negado", 1, ""));

                        tabla.Rows.Add(filaEncabezado);
                    }

                    filaTabla = new TableHeaderRow();

                    filaTabla.Cells.Add(celdaNormal(reader["ID_Proyecto"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["NomProyecto"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["nomciudad"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["Unidad"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDirecto"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoPrimerAno"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["Empleo18a24"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesplazados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoMadres"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoMinorias"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoRecluidos"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesmovilizados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDiscapacitados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal(reader["EmpleoDesvinculados"].ToString(), 1));
                    filaTabla.Cells.Add(celdaNormal("$ " + (Double.Parse(reader["valorrecomendado"].ToString()) * Double.Parse(SMLV)), 1));
                    filaTabla.Cells.Add(celdaNormal("$ " + ((Double.Parse(reader["recursos"].ToString()) - Double.Parse(reader["valorrecomendado"].ToString())) * Double.Parse(SMLV)), 1));

                    tabla.Rows.Add(filaTabla);

                    SubTotalDirecto = SubTotalDirecto + Int64.Parse(reader["EmpleoDirecto"].ToString());
                    TotalDeptoDirecto = TotalDeptoDirecto + Int64.Parse(reader["EmpleoDirecto"].ToString());

                    SubTotalPrimerAno = SubTotalPrimerAno + Int64.Parse(reader["EmpleoPrimerAno"].ToString());
                    TotalDeptoPrimerAno = TotalDeptoPrimerAno + Int64.Parse(reader["EmpleoPrimerAno"].ToString());

                    SubTotal18a24 = SubTotal18a24 + Int64.Parse(reader["Empleo18a24"].ToString());
                    TotalDepto18a24 = TotalDepto18a24 + Int64.Parse(reader["Empleo18a24"].ToString());

                    SubTotalDesplazados = SubTotalDesplazados + Int64.Parse(reader["EmpleoDesplazados"].ToString());
                    TotalDeptoDesplazados = TotalDeptoDesplazados + Int64.Parse(reader["EmpleoDesplazados"].ToString());

                    SubTotalMadres = SubTotalMadres + Int64.Parse(reader["EmpleoMadres"].ToString());
                    TotalDeptoMadres = TotalDeptoMadres + Int64.Parse(reader["EmpleoMadres"].ToString());

                    SubTotalMinorias = SubTotalMinorias + Int64.Parse(reader["EmpleoMinorias"].ToString());
                    TotalDeptoMinorias = TotalDeptoMinorias + Int64.Parse(reader["EmpleoMinorias"].ToString());

                    SubTotalRecluidos = SubTotalRecluidos + Int64.Parse(reader["EmpleoRecluidos"].ToString());
                    TotalDeptoRecluidos = TotalDeptoRecluidos + Int64.Parse(reader["EmpleoRecluidos"].ToString());

                    SubTotalDesmovilizados = SubTotalDesmovilizados + Int64.Parse(reader["EmpleoDesmovilizados"].ToString());
                    TotalDeptoDesmovilizados = TotalDeptoDesmovilizados + Int64.Parse(reader["EmpleoDesmovilizados"].ToString());

                    SubTotalDiscapacitados = SubTotalDiscapacitados + Int64.Parse(reader["EmpleoDiscapacitados"].ToString());
                    TotalDeptoDiscapacitados = TotalDeptoDiscapacitados + Int64.Parse(reader["EmpleoDiscapacitados"].ToString());

                    SubTotalDesvinculados = SubTotalDesvinculados + Int64.Parse(reader["EmpleoDesvinculados"].ToString());
                    TotalDeptoDesvinculados = TotalDeptoDesvinculados + Int64.Parse(reader["EmpleoDesvinculados"].ToString());

                    TotalEmpleosPlan = Int64.Parse(reader["EmpleoDirecto"].ToString()) + Int64.Parse(reader["EmpleoPrimerAno"].ToString()) + Int64.Parse(reader["Empleo18a24"].ToString()) + Int64.Parse(reader["EmpleoDesplazados"].ToString()) + Int64.Parse(reader["EmpleoMadres"].ToString())+ Int64.Parse(reader["EmpleoMinorias"].ToString()) + Int64.Parse(reader["EmpleoRecluidos"].ToString())  + Int64.Parse(reader["EmpleoDesmovilizados"].ToString())  + Int64.Parse(reader["EmpleoDiscapacitados"].ToString()) + Int64.Parse(reader["EmpleoDesvinculados"].ToString());
                    SubTotalEmpleos = SubTotalEmpleos + TotalEmpleosPlan;

                    if (reader["CodTipoInstitucion"].ToString().Equals("" + Constantes.CONST_Sena))
                    {
                        SubTotalEmpleosSena = SubTotalEmpleosSena + TotalEmpleosPlan;
	   	                SubTotalViablesSena = SubTotalViablesSena + 1;
                        SubTotalNegadoSena = SubTotalNegadoSena + (Int64.Parse(reader["recursos"].ToString()) - Int64.Parse(reader["valorrecomendado"].ToString()));
	   	                SubTotalRecomendadoSena = SubTotalRecomendadoSena + Int64.Parse(reader["valorrecomendado"].ToString());
                    }
                    else{
                        SubTotalEmpleosUniv = SubTotalEmpleosUniv + TotalEmpleosPlan;
                        SubTotalViablesUniv = SubTotalViablesUniv + 1;
                        SubTotalNegadoUniv= SubTotalNegadoUniv + (Int64.Parse(reader["recursos"].ToString()) - Int64.Parse(reader["valorrecomendado"].ToString()));
                        SubTotalRecomendadoUniv = SubTotalRecomendadoUniv + Int64.Parse(reader["valorrecomendado"].ToString());
                    }

                    SubTotalSolicitado = SubTotalSolicitado + Int64.Parse(reader["recursos"].ToString());
		            TotalDeptoSolicitado = TotalDeptoSolicitado + Int64.Parse(reader["recursos"].ToString());

		            SubTotalRecomendado = SubTotalRecomendado + Int64.Parse(reader["valorrecomendado"].ToString());
		            TotalDeptoRecomendado = TotalDeptoRecomendado + Int64.Parse(reader["valorrecomendado"].ToString());

		            SubTotalNegado = SubTotalNegado + (Int64.Parse(reader["recursos"].ToString())-Int64.Parse(reader["valorrecomendado"].ToString()));
		            TotalDeptoNegado = TotalDeptoNegado + (Int64.Parse(reader["recursos"].ToString())-Int64.Parse(reader["valorrecomendado"].ToString()));

                    try
                    {
                        siLlee = reader.Read();
                        if (siLlee)
                        {
                            if (!txtSector.Equals(reader["NomSector"].ToString()))
                            {
                                impresion1();
                            }

                            if (!txtDepartamento.Equals(reader["NomDepartamento"].ToString()))
                            {
                                impresion2();
                            }
                        }
                        else
                        {
                            impresion1();
                            impresion2();
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        P_principal.Controls.Add(tabla);
                        P_principal.DataBind();
                    }
                    catch (Exception) { }
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

        private void filadeDos(String tituloText, String mensaje)
        {
            TableHeaderRow fila1 = new TableHeaderRow();

            fila1.Cells.Add(crearceladtitulo(tituloText, 2, ""));
            fila1.Cells.Add(crearceladtitulo(mensaje, 14, "cclasecelad"));

            tabla.Rows.Add(fila1);
        }

        private TableHeaderCell crearceladtitulo(String mensaje, Int32 colspan, String cssestilo)
        {
            TableHeaderCell celda1 = new TableHeaderCell();
            celda1.ColumnSpan = colspan;
            celda1.CssClass = cssestilo;

            Label titulo1 = new Label();
            titulo1.Text = mensaje;
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private TableCell celdaNormal(String mensaje, Int32 colspan)
        {
            TableCell celda1 = new TableCell();
            celda1.ColumnSpan = colspan;

            Label titulo1 = new Label();
            titulo1.Text = mensaje;
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private void impresion1()
        {
            TableRow pie1 = new TableRow();
            pie1.CssClass = "sdP_principal";

            pie1.Cells.Add(celdaNormal("Total Empleos por Aspecto Sector", 4));
            pie1.Cells.Add(celdaNormal("" + SubTotalDirecto, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalPrimerAno, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotal18a24, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalDesplazados, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalMadres, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalMinorias, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalRecluidos, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalDesmovilizados, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalDiscapacitados, 1));
            pie1.Cells.Add(celdaNormal("" + SubTotalDesvinculados, 1));
            pie1.Cells.Add(celdaNormal("$ " + (SubTotalRecomendado * Int64.Parse(SMLV)), 1));
            pie1.Cells.Add(celdaNormal("$ " + (SubTotalNegado * Int64.Parse(SMLV)), 1));

            tabla.Rows.Add(pie1);
        }

        private void impresion2()
        {
            TableRow pie1 = new TableRow();
            pie1.CssClass = "sdP_principal";

            pie1.Cells.Add(celdaNormal("Total Empleos por Aspecto " + txtDepartamento, 4));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoDirecto, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoPrimerAno, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDepto18a24, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoDesplazados, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoMadres, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoMinorias, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoRecluidos, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoDesmovilizados, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoDiscapacitados, 1));
            pie1.Cells.Add(celdaNormal("" + TotalDeptoDesvinculados, 1));
            pie1.Cells.Add(celdaNormal("$ " + (TotalDeptoRecomendado * Int64.Parse(SMLV)), 1));
            pie1.Cells.Add(celdaNormal("$ " + (TotalDeptoNegado * Int64.Parse(SMLV)), 1));

            tabla.Rows.Add(pie1);

            pie1 = new TableRow();
            pie1.CssClass = "sdP_principal";

            pie1.Cells.Add(celdaNormal("", 4));
            pie1.Cells.Add(celdaNormal("Total empleos por planes de negocio de Universidades y otros", 2));
            pie1.Cells.Add(celdaNormal("" + SubTotalEmpleosUniv, 2));
            pie1.Cells.Add(celdaNormal("Número de planes viables por Universidades y otros", 2));
            pie1.Cells.Add(celdaNormal("" + SubTotalViablesUniv, 2));
            pie1.Cells.Add(celdaNormal("Valor Recomendado - Negado para planes Avalados por Universidades y Otros", 2));
            pie1.Cells.Add(celdaNormal("$ " + (SubTotalRecomendadoUniv * Int64.Parse(SMLV)), 1));
            pie1.Cells.Add(celdaNormal("$ " + (SubTotalNegadoUniv * Int64.Parse(SMLV)), 1));

            tabla.Rows.Add(pie1);

            pie1 = new TableRow();
            pie1.CssClass = "sdP_principal";

            pie1.Cells.Add(celdaNormal("", 4));
            pie1.Cells.Add(celdaNormal("Total empleos por planes de negocio del SENA", 2));
            pie1.Cells.Add(celdaNormal("" + SubTotalEmpleosSena, 2));
            pie1.Cells.Add(celdaNormal("Número de planes viables por el SENA", 2));
            pie1.Cells.Add(celdaNormal("" + SubTotalViablesSena, 2));
            pie1.Cells.Add(celdaNormal("Valor Recomendado - Negado para planes Avalados por el SENA", 2));
            pie1.Cells.Add(celdaNormal("$ " + (SubTotalRecomendadoSena * Int64.Parse(SMLV)), 1));
            pie1.Cells.Add(celdaNormal("$ " + (SubTotalNegadoSena * Int64.Parse(SMLV)), 1));

            tabla.Rows.Add(pie1);

            pie1 = new TableRow();
            pie1.CssClass = "sdP_principal";

            pie1.Cells.Add(celdaNormal("", 4));
            pie1.Cells.Add(celdaNormal("Total empleos " + txtDepartamento, 2));
            pie1.Cells.Add(celdaNormal("" + SubTotalEmpleos, 2));
            pie1.Cells.Add(celdaNormal("", 8));

            tabla.Rows.Add(pie1);
        }

        private void impresion3()
        {
            TotalEmpleos = TotalDirecto + TotalPrimerAno + Total18a24 + TotalDesplazados + TotalMadres + TotalMinorias + TotalRecluidos + TotalDesmovilizados + TotalDiscapacitados + TotalDesvinculados;
            TableRow filaTabla;
            filaTabla = new TableHeaderRow();

            filaTabla.Cells.Add(celdaNormal("Total", 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalViablesSena, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalViablesUniv, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalDirecto, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalPrimerAno, 1));
            filaTabla.Cells.Add(celdaNormal("" + Total18a24, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalDesplazados, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalMadres, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalMinorias, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalRecluidos, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalDesmovilizados, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalDiscapacitados, 1));
            filaTabla.Cells.Add(celdaNormal("" + TotalDesvinculados, 1));
            filaTabla.Cells.Add(celdaNormal("$ " + (TotalRecomendado * (Double.Parse(SMLV))), 1));
            filaTabla.Cells.Add(celdaNormal("$ " + (TotalNegado * (Double.Parse(SMLV))), 1));

            tabla.Rows.Add(filaTabla);

            filaTabla = new TableHeaderRow();

            filaTabla.Cells.Add(celdaNormal("Total Empleos por Sector", 8));
            filaTabla.Cells.Add(celdaNormal("" + TotalEmpleos, 7));

            tabla.Rows.Add(filaTabla);
        }
    }
}