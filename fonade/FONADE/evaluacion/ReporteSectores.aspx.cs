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
    public partial class ReporteSectores : System.Web.UI.Page
    {
        String idConvocatoriaEval;
        String nomConvocatoriaEval;
        String idCodSector;
        String []varidCodSector;
        String idViable;

        String puntajeMinimo;
        String SMLV;
        String AnioSalario;

        DataTable informacionPrincipal;

        Int32 TotalSolicitado =  0;
        Int32 TotalRecomendado =  0;
        Int32 TotalRecomendadoAprobados =  0;
        Int32  TotalRecomendadoViables =  0;
        Int32 TotalProyectos =  0;
        Int32 TotalAprobados =  0;
        Int32 TotalViables =  0;
        Int32 TotalAvalados = 0;

        Int32 SubTotalSolicitado = 0;
		Int32 SubTotalRecomendado = 0;
		Int32 SubTotalRecomendadoAprobados = 0;
		Int32 SubTotalRecomendadoViables = 0;
		Int32 SubTotalProyectos = 0;
		Int32 SubTotalAprobados = 0;
		Int32 SubTotalViables = 0;
        Int32 SubTotalAvalados = 0;

        Int32 PuntajeTotal = 0;
        Int32 NumAspectos = 0;

        String nombreSector = "";
        Int32 NumColspan = 0;

        Table tabla;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Session["idConvocatoriaEval"].ToString()))
                {
                    idConvocatoriaEval = HttpContext.Current.Session["idConvocatoriaEval"].ToString();
                    nomConvocatoriaEval = HttpContext.Current.Session["idNombreConvocatoria"].ToString();
                    idCodSector = HttpContext.Current.Session["idCIIUEval"].ToString();
                    idViable = HttpContext.Current.Session["idViable"].ToString();

                    varidCodSector = idCodSector.Split(' ');

                    if (idViable.Equals("1"))
                    {
                        idViable = "viable=1 and";
                    }
                    else
                    {
                        if (idViable.Equals("2"))
                        {
                            idViable = "viable=2 and";
                        }
                        else
                        {
                            idViable = "";
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                Response.Redirect("ReportesEvaluacion");
            }

            L_ReportesEvaluacion.Text = "REPORTE CONSOLIDADO POR SECTOR - " + nomConvocatoriaEval;
            //L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;

            datosGenerales();

            CrearTabla();

            llennarTabla();
        }

        private void datosGenerales()
        {
            String sql;

            sql = "SELECT  SUM([Puntaje]) AS PUNTAJE FROM [ConvocatoriaCampo] AS CC, [Campo] AS C WHERE [id_Campo] = CC.[codCampo] AND C.[codCampo] IS NULL AND [codConvocatoria] = " + idConvocatoriaEval;

            if (Int32.Parse(idConvocatoriaEval) > 1)
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

        private void CrearTabla()
        {

            informacionPrincipal = new DataTable();

            informacionPrincipal.Columns.Add("id_proyecto");
            informacionPrincipal.Columns.Add("nomproyecto");
            informacionPrincipal.Columns.Add("Municipio");
            informacionPrincipal.Columns.Add("nomsector");
            informacionPrincipal.Columns.Add("Emprendimiento");
            informacionPrincipal.Columns.Add("montosolicitado");
            informacionPrincipal.Columns.Add("montorecomendado");
            informacionPrincipal.Columns.Add("viable");

            for (int i = 1; i < varidCodSector.Length; i++)
            {
                String sql;

                sql = "SELECT id_proyecto, nomproyecto, nomciudad + ' (' + nomdepartamento + ')' AS Municipio , nomsector, nomunidad + ' (' + nominstitucion + ')' AS Emprendimiento, ISNULL(recursos,0) AS montosolicitado, ISNULL(valorrecomendado,0) AS montorecomendado, CASE WHEN ISNULL(viable, 0)=1 THEN 'SI' ELSE 'NO' END AS viable FROM proyecto INNER JOIN convocatoriaproyecto AS cp ON id_proyecto=cp.codproyecto AND " + idViable + " codconvocatoria=" + idConvocatoriaEval + " INNER JOIN ciudad ON id_ciudad=codciudad INNER JOIN departamento ON id_departamento=coddepartamento INNER JOIN subsector ON id_subsector = codsubsector INNER JOIN sector ON id_sector = codsector INNER JOIN institucion ON id_institucion = codinstitucion LEFT JOIN proyectofinanzasingresos AS pfi ON id_proyecto=pfi.codproyecto LEFT JOIN evaluacionobservacion AS ev ON id_proyecto=ev.codproyecto AND ev.codconvocatoria = cp.codconvocatoria WHERE codsector IN (" + varidCodSector[i] + ") ORDER BY nomsector";

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DataRow fila = informacionPrincipal.NewRow();

                        fila["id_proyecto"] = reader["id_proyecto"].ToString();
                        fila["nomproyecto"] = reader["nomproyecto"].ToString();
                        fila["Municipio"] = reader["Municipio"].ToString();
                        fila["nomsector"] = reader["nomsector"].ToString();
                        fila["Emprendimiento"] = reader["Emprendimiento"].ToString();
                        fila["montosolicitado"] = reader["montosolicitado"].ToString();
                        fila["montorecomendado"] = reader["montorecomendado"].ToString();
                        fila["viable"] = reader["viable"].ToString();

                        informacionPrincipal.Rows.Add(fila);
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

        private void llennarTabla()
        {
            TotalSolicitado = 0;
            TotalRecomendado = 0;
            TotalRecomendadoAprobados = 0;
            TotalRecomendadoViables = 0;
            TotalProyectos = 0;
            TotalAprobados = 0;
            TotalViables = 0;
            TotalAvalados = 0;

            for (int i = 0; i < informacionPrincipal.Rows.Count; i++)
            {
                PuntajeTotal = 0;

                if (!nombreSector.Equals(informacionPrincipal.Rows[i]["nomsector"].ToString()))
                {
                    nombreSector = informacionPrincipal.Rows[i]["nomsector"].ToString();
                    SubTotalSolicitado = 0;
			        SubTotalRecomendado = 0;
			        SubTotalRecomendadoAprobados = 0;
			        SubTotalRecomendadoViables = 0;
			        SubTotalProyectos = 0;
			        SubTotalAprobados = 0;
			        SubTotalViables = 0;
                    SubTotalAvalados = 0;

                    tabla = new Table();
                    tabla.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    tabla.CssClass = "Grilla";

                    filadeDos("Sector CIIU", nombreSector);
                    filadeDos("Puntaje Mín Aprobatorio", puntajeMinimo);
                    fila3Tabla();
                    filaTitulos();
                }

                TableRow fila1 = new TableRow();

                fila1 = filaNormal(fila1, i);

                NumAspectos = 5;

                for (int j = 1; j <= NumAspectos; j++)
                {
                    String sql = @"select sum(ec.puntaje) puntaje from evaluacioncampo ec inner join campo c on c.id_campo = ec.codcampo inner join campo v on v.id_campo = c.codcampo inner join campo a on a.id_campo = v.codcampo where codproyecto=" + informacionPrincipal.Rows[i]["id_proyecto"].ToString() + " and codconvocatoria=" + idConvocatoriaEval + " and a.id_campo=" + j;

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            fila1.Cells.Add(celdaNormal(reader["puntaje"].ToString(),1));
                            try
                            {
                                PuntajeTotal = PuntajeTotal + Int32.Parse(reader["puntaje"].ToString());
                            }
                            catch (FormatException) { }
                            catch (Exception) { }
                        }
                        else
                        {
                            fila1.Cells.Add(celdaNormal("",1));
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

                fila1.Cells.Add(celdaNormal("" + PuntajeTotal,1));
                

                fila1.Cells.Add(celdaNormal(informacionPrincipal.Rows[i]["montosolicitado"].ToString(),1));
                fila1.Cells.Add(celdaNormal("$ " + (Int64.Parse(informacionPrincipal.Rows[i]["montosolicitado"].ToString()) * Int64.Parse(SMLV)),1));
                fila1.Cells.Add(celdaNormal(informacionPrincipal.Rows[i]["montorecomendado"].ToString(),1));
                fila1.Cells.Add(celdaNormal("$ " + (Int64.Parse(informacionPrincipal.Rows[i]["montorecomendado"].ToString()) * Int64.Parse(SMLV)),1));
                fila1.Cells.Add(celdaNormal(informacionPrincipal.Rows[i]["viable"].ToString(),1));

                if (informacionPrincipal.Rows[i]["viable"].ToString().Equals("SI"))
                {
                    SubTotalViables = SubTotalViables + 1;
                    TotalViables = TotalViables + 1;

                    SubTotalRecomendadoViables = SubTotalRecomendadoViables + Int32.Parse(informacionPrincipal.Rows[i]["montorecomendado"].ToString());
			        TotalRecomendadoViables = TotalRecomendadoViables + Int32.Parse(informacionPrincipal.Rows[i]["montorecomendado"].ToString());
                }

                Int32 vlor = 0;
                try{
                    vlor = Int32.Parse(consulta("SELECT isnull(COUNT(0),0) as Total FROM TabEvaluacion WHERE codTabEvaluacion is NULL", "Total")) - 2;
                }catch(FormatException){}

                String sqlCodProyecto = consulta("select codproyecto from tabevaluacionproyecto tep, tabevaluacion te where id_tabevaluacion=tep.codtabevaluacion  and realizado=1 and te.codtabevaluacion is null and codproyecto=" + informacionPrincipal.Rows[i]["id_proyecto"].ToString() + " group by codproyecto HAVING count(tep.codtabevaluacion)= " + vlor, "codproyecto");

                //<img src="../../Images/chulo.gif" alt="chulo.gif" />

                if (!String.IsNullOrEmpty(sqlCodProyecto))
                {
                    fila1.Cells.Add(celdaImagen(informacionPrincipal.Rows[i]["id_proyecto"].ToString()));
                    SubTotalAvalados = SubTotalAvalados + 1;
                    TotalAvalados = TotalAvalados + 1;
                }

                tabla.Rows.Add(fila1);

                SubTotalSolicitado = SubTotalSolicitado + Int32.Parse(informacionPrincipal.Rows[i]["montosolicitado"].ToString());
		        TotalSolicitado = TotalSolicitado + Int32.Parse(informacionPrincipal.Rows[i]["montosolicitado"].ToString());

		        SubTotalRecomendado = SubTotalRecomendado + Int32.Parse(informacionPrincipal.Rows[i]["montorecomendado"].ToString());
                TotalRecomendado = TotalRecomendado + Int32.Parse(informacionPrincipal.Rows[i]["montorecomendado"].ToString());

		        SubTotalProyectos = SubTotalProyectos + 1;
		        TotalProyectos = TotalProyectos + 1;

                try
                {
                    if (PuntajeTotal >= Int32.Parse(puntajeMinimo))
                    {
                        SubTotalAprobados = SubTotalAprobados + 1;
                        TotalAprobados = TotalAprobados + 1;

                        SubTotalRecomendadoAprobados = SubTotalRecomendadoAprobados + Int32.Parse(informacionPrincipal.Rows[i]["montorecomendado"].ToString());
                        TotalRecomendadoAprobados = TotalRecomendadoAprobados + Int32.Parse(informacionPrincipal.Rows[i]["montorecomendado"].ToString());
                    }
                }
                catch (FormatException)
                {

                }

                NumColspan = 5 + NumAspectos;

                try
                {
                    
                    if (!nombreSector.Equals(informacionPrincipal.Rows[i+1]["nomsector"].ToString()) && i != 0)
                    {
                        imprimirPies(i, NumColspan);
                    }
                }
                catch (InvalidExpressionException) { }
                catch (IndexOutOfRangeException) { }

                try
                {
                    P_principal.Controls.Add(tabla);
                    P_principal.DataBind();
                }
                catch (Exception) { }
            }

            imprimirPies(informacionPrincipal.Rows.Count-1, NumColspan);
            tablaFinal();
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

        private void filadeDos(String tituloText, String mensaje)
        {
            TableHeaderRow fila1 = new TableHeaderRow();

            fila1.Cells.Add(crearceladtitulo(tituloText,2,""));
            fila1.Cells.Add(crearceladtitulo(mensaje, 15, "cclasecelad"));

            tabla.Rows.Add(fila1);
        }

        private void fila3Tabla()
        {
            TableHeaderRow fila1 = new TableHeaderRow();

            fila1.Cells.Add(crearceladtitulo("SMLV",2, ""));
            fila1.Cells.Add(crearceladtitulo(SMLV,2, ""));
            fila1.Cells.Add(crearceladtitulo("Aspectos Evaluados", 7, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Monto Solicitado", 2, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Monto Recomendado", 2, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("",1, ""));

            tabla.Rows.Add(fila1);
        }

        private void filaTitulos()
        {
            TableHeaderRow fila1 = new TableHeaderRow();

            fila1.Cells.Add(crearceladtitulo("ID", 1, ""));
            fila1.Cells.Add(crearceladtitulo("Plan de Negocio", 1, ""));
            fila1.Cells.Add(crearceladtitulo("Municipio", 1, ""));
            fila1.Cells.Add(crearceladtitulo("Unidad de Emprendimiento", 1, ""));
            fila1.Cells.Add(crearceladtitulo("Generales", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Comerciales", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Técnicos", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Organizacionales", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Financieros", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("PuntajeTotal", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("SMLV", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Pesos", 1, ""));
            fila1.Cells.Add(crearceladtitulo("SMLV", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Pesos", 1, ""));
            fila1.Cells.Add(crearceladtitulo("Viable", 1, "cclasecelad"));
            fila1.Cells.Add(crearceladtitulo("Aval Coord.", 1, ""));

            tabla.Rows.Add(fila1);
        }

        private TableRow filaNormal(TableRow fila1, Int32 i)
        {
            TableRow fila2;
            fila2 = fila1;

            fila2.Cells.Add(celdaNormal(informacionPrincipal.Rows[i]["id_proyecto"].ToString(),1));
            fila2.Cells.Add(celdaNormal(informacionPrincipal.Rows[i]["nomproyecto"].ToString(),1));
            fila2.Cells.Add(celdaNormal(informacionPrincipal.Rows[i]["Municipio"].ToString(),1));
            fila2.Cells.Add(celdaNormal(informacionPrincipal.Rows[i]["Emprendimiento"].ToString(),1));

            return fila2;
        }
        
        private String consulta(String sql, String resultado)
        {
            String vlor = "";

            vlor = sql;
            //String sql = "SELECT isnull(COUNT(0),0) as Total FROM TabEvaluacion WHERE codTabEvaluacion is NULL";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(vlor, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    vlor = reader[resultado].ToString();
                }
                else
                {
                    vlor = "";
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

            return vlor;
        }

        private TableCell celdaImagen(String i)
        {
            TableCell celda1 = new TableCell();

            Image image = new Image();
            image.ID = "image" + i;
            image.ImageUrl = "~/Images/chulo.gif";

            celda1.Controls.Add(image);

            return celda1;
        }

        private void imprimirPies(Int32 i, Int32 NumColspan)
        {
            try
            {
                TableRow pie1 = new TableRow();
                pie1.CssClass = "sdP_principal";
                pie1.Cells.Add(celdaNormal("SubTotal Sector", NumColspan));
                pie1.Cells.Add(celdaNormal("" + SubTotalSolicitado, 1));
                pie1.Cells.Add(celdaNormal("$ " + SubTotalSolicitado * Int64.Parse(SMLV), 1));
                pie1.Cells.Add(celdaNormal("", 4));
                tabla.Rows.Add(pie1);

                TableRow pie2 = new TableRow();
                pie2.CssClass = "sdP_principal";
                pie2.Cells.Add(celdaNormal("SubTotal Aprobados Sector", NumColspan));
                pie2.Cells.Add(celdaNormal("", 2));
                pie2.Cells.Add(celdaNormal("" + SubTotalRecomendadoAprobados, 1));
                pie2.Cells.Add(celdaNormal("$ " + SubTotalRecomendadoAprobados * Int64.Parse(SMLV), 1));
                pie2.Cells.Add(celdaNormal("", 2));
                tabla.Rows.Add(pie2);

                TableRow pie3 = new TableRow();
                pie3.CssClass = "sdP_principal";
                pie3.Cells.Add(celdaNormal("SubTotal Aprobados Sector", NumColspan));
                pie3.Cells.Add(celdaNormal("", 2));
                pie3.Cells.Add(celdaNormal("" + SubTotalRecomendadoViables, 1));
                pie3.Cells.Add(celdaNormal("$ " + SubTotalRecomendadoViables * Int64.Parse(SMLV), 1));
                pie3.Cells.Add(celdaNormal("", 2));
                tabla.Rows.Add(pie3);

                TableRow pie4 = new TableRow();
                pie4.CssClass = "sdP_principal";
                pie4.Cells.Add(celdaNormal("Numero de Planes", NumColspan));
                pie4.Cells.Add(celdaNormal("" + SubTotalProyectos, 1));
                pie4.Cells.Add(celdaNormal("", 5));
                tabla.Rows.Add(pie4);

                TableRow pie5 = new TableRow();
                pie5.CssClass = "sdP_principal";
                pie5.Cells.Add(celdaNormal("Numero de Planes Aprobados", NumColspan));
                pie5.Cells.Add(celdaNormal("" + SubTotalAprobados, 1));
                pie5.Cells.Add(celdaNormal("", 5));
                tabla.Rows.Add(pie5);

                TableRow pie6 = new TableRow();
                pie6.CssClass = "sdP_principal";
                pie6.Cells.Add(celdaNormal("Numero de Planes Viables", NumColspan));
                pie6.Cells.Add(celdaNormal("" + SubTotalViables, 1));
                pie6.Cells.Add(celdaNormal("", 5));
                tabla.Rows.Add(pie6);

                TableRow pie7 = new TableRow();
                pie7.CssClass = "sdP_principal";
                pie7.Cells.Add(celdaNormal("Numero de Planes Avalados por Coordinador", NumColspan));
                pie7.Cells.Add(celdaNormal("" + SubTotalAvalados, 1));
                pie7.Cells.Add(celdaNormal("", 5));
                tabla.Rows.Add(pie7);

                TableRow pie8 = new TableRow();
                pie8.CssClass = "sdP_principal";
                pie8.Cells.Add(celdaNormal("Numero de Planes Negados (No Viables)", NumColspan));
                pie8.Cells.Add(celdaNormal("" + (SubTotalProyectos - SubTotalViables), 1));
                pie8.Cells.Add(celdaNormal("", 5));
                tabla.Rows.Add(pie8);

                P_principal.Controls.Add(tabla);
                P_principal.DataBind();
            }
            catch (NullReferenceException) { }
        }

        private void tablaFinal()
        {
            tabla = new Table();
            tabla.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            //tabla.CssClass = "sdP_principal";

            TableRow pie1 = new TableRow();
            pie1.CssClass = "sdP_principal";
            pie1.Cells.Add(celdaNormal("Total", NumColspan));
            pie1.Cells.Add(celdaNormal("" + TotalSolicitado, 1));
            pie1.Cells.Add(celdaNormal("$ " + TotalSolicitado * Int64.Parse(SMLV), 1));
            pie1.Cells.Add(celdaNormal("", 4));
            tabla.Rows.Add(pie1);

            TableRow pie2 = new TableRow();
            pie2.CssClass = "sdP_principal";
            pie2.Cells.Add(celdaNormal("Total Aprobados Sector", NumColspan));
            pie2.Cells.Add(celdaNormal("", 2));
            pie2.Cells.Add(celdaNormal("" + TotalRecomendadoAprobados, 1));
            pie2.Cells.Add(celdaNormal("$ " + TotalRecomendadoAprobados * Int64.Parse(SMLV), 1));
            pie2.Cells.Add(celdaNormal("", 2));
            tabla.Rows.Add(pie2);

            TableRow pie3 = new TableRow();
            pie3.CssClass = "sdP_principal";
            pie3.Cells.Add(celdaNormal("Total Viables Sector", NumColspan));
            pie3.Cells.Add(celdaNormal("", 2));
            pie3.Cells.Add(celdaNormal("" + TotalRecomendadoViables, 1));
            pie3.Cells.Add(celdaNormal("$ " + TotalRecomendadoViables * Int64.Parse(SMLV), 1));
            pie3.Cells.Add(celdaNormal("", 2));
            tabla.Rows.Add(pie3);

            TableRow pie4 = new TableRow();
            pie4.CssClass = "sdP_principal";
            pie4.Cells.Add(celdaNormal("Total Numero de Planes", NumColspan));
            pie4.Cells.Add(celdaNormal("" + TotalProyectos, 1));
            pie4.Cells.Add(celdaNormal("", 5));
            tabla.Rows.Add(pie4);

            TableRow pie5 = new TableRow();
            pie5.CssClass = "sdP_principal";
            pie5.Cells.Add(celdaNormal("Total Numero de Planes Aprobados", NumColspan));
            pie5.Cells.Add(celdaNormal("" + TotalAprobados, 1));
            pie5.Cells.Add(celdaNormal("", 5));
            tabla.Rows.Add(pie5);

            TableRow pie6 = new TableRow();
            pie6.CssClass = "sdP_principal";
            pie6.Cells.Add(celdaNormal("Total Numero de Planes Viables", NumColspan));
            pie6.Cells.Add(celdaNormal("" + TotalViables, 1));
            pie6.Cells.Add(celdaNormal("", 5));
            tabla.Rows.Add(pie6);

            TableRow pie7 = new TableRow();
            pie7.CssClass = "sdP_principal";
            pie7.Cells.Add(celdaNormal("Total Numero de Planes Avalados por Coordinador", NumColspan));
            pie7.Cells.Add(celdaNormal("" + TotalAvalados, 1));
            pie7.Cells.Add(celdaNormal("", 5));
            tabla.Rows.Add(pie7);

            TableRow pie8 = new TableRow();
            pie8.CssClass = "sdP_principal";
            pie8.Cells.Add(celdaNormal("Total Numero de Planes Negados (No Viables)", NumColspan));
            pie8.Cells.Add(celdaNormal("" + (TotalProyectos - TotalViables), 1));
            pie8.Cells.Add(celdaNormal("", 5));
            tabla.Rows.Add(pie8);

            P_principal.Controls.Add(tabla);
            P_principal.DataBind();
        }
    }
}