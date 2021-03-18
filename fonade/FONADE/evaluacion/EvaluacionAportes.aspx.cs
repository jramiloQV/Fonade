using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionAportes : Negocio.Base_Page
    {
        /// <summary>
        /// Determina el comportamiento de Evaluacion aportes
        /// </summary>

        private double totalInversion, totalCapital, totalDiferido, totalInversion1, totalCapital1, totalDiferido1;
        public int CodProyecto, CodConvocatoria;
        public int _salariominimo;
        private static Consultas _consultas = new Consultas();
        public int _anioConvocatoria = 0;
        public bool bandera;
        private GridViewHelper helper;
        public double TotalSolicitadoAporte, TotalRecomendadoAporte,
            TotalSolicitadoAporteC, TotalRecomendadoAporteC, TotalSolicitadoAporteD, TotalRecomendadoAporteD;
        // variable para acoumular el porcentaje de los totales solicitados y recomendados
        public float dif1D, dif2D, dif3D, dif1A, dif2A, dif3A, dif1C, dif2c, dif3c;
        public double aporteTotal;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si "está" o "no" realizado...
        /// </summary>
        public Boolean bRealizado;
        /// <summary>
        /// Variable que se debe cambiar.
        /// </summary>
        String Accion = "CAMBIAR";
        /// <summary>
        /// Contiene las consultas SQL.
        /// </summary>
        String txtSQL;

       

       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["CodProyecto"] == null)
                CodProyecto = 0;
            else
                CodProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);

            if (HttpContext.Current.Session["CodConvocatoria"] == null)
                CodConvocatoria = 0;
            else
                CodConvocatoria = Convert.ToInt32(HttpContext.Current.Session["CodConvocatoria"]);

            inicioEncabezado(Convert.ToString(CodProyecto), Convert.ToString(CodConvocatoria),
                             Constantes.CONST_subAportes);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodProyecto.ToString());

            //Consultar si está "realizado".
            bRealizado = esRealizado(Constantes.CONST_subAportes, CodProyecto, CodConvocatoria);

            if (esMiembro && !bRealizado)
            {
                this.div_Post_It1.Visible = true;
                this.Post_It1._mostrarPost = true;
            }
            else
            {
                this.div_Post_It1.Visible = false;
                this.Post_It1._mostrarPost = false;
                txtobservaciones.Enabled = false;
            }
            
            if (!IsPostBack)
            {

                ObtenerAñoConvocatoria();
                Permisos();
                ObtenerSalariosMinimos();
                LlenarGrillas();

                ObtenerDatosUltimaActualizacion();
                
            }

            if (!esMiembro || bRealizado || Constantes.CONST_Evaluador != usuario.CodGrupo)
            {
                //this.div_Post_It1.Visible = false;
                //Mostrar labels.
                lbl_solicitado.Visible = true;
                lbl_recomendado.Visible = true;

                //Inhabilitar y ocultar determinados campos.
                pnlagregar.Visible = false;

                txtsolicitado.Visible = false;
                txtrecomendado.Visible = false;
                Btnupdate.Visible = false;

                try { GrAportes.Columns[0].Visible = false; }
                catch { }
                try { GrAportes.Columns[1].Visible = false; }
                catch { }

                try { GrvCapital.Columns[0].Visible = false; }
                catch { }
                try { GrvCapital.Columns[1].Visible = false; }
                catch { }

                try { GrvDifereridas.Columns[0].Visible = false; }
                catch { }
                try { GrvDifereridas.Columns[1].Visible = false; }
                catch { }

                GrvIntegrantes.Columns[0].Visible = false;
            }
            else
            {
                //Mostrar labels.
                lbl_solicitado.Visible = false;
                lbl_recomendado.Visible = false;

                //Inhabilitar y ocultar determinados campos.
                pnlagregar.Visible = true;
                txtsolicitado.Visible = true;
                txtrecomendado.Visible = true;
                Btnupdate.Visible = true;

                try { GrAportes.Columns[0].Visible = true; }
                catch { }
                try { GrAportes.Columns[1].Visible = true; }
                catch { }

                try { GrvCapital.Columns[0].Visible = true; }
                catch { }
                try { GrvCapital.Columns[1].Visible = true; }
                catch { }

                try { GrvDifereridas.Columns[0].Visible = true; }
                catch { }
                try { GrvDifereridas.Columns[1].Visible = true; }
                catch { }

                GrvIntegrantes.Columns[0].Visible = true;
            }
        }
       

        public void LlenarGrillas()
        {
            if (CodConvocatoria != 0 && CodProyecto != 0)
            {
               
                // cargo la grilla de aportes

                var list = consultas.ObtenerAportes(Convert.ToString(CodProyecto), Convert.ToString(CodConvocatoria));
                var lstaportes = new List<EvaluacionProyectoAporte>();
                var lstcapital = new List<EvaluacionProyectoAporte>();
                var lstdiferencia = new List<EvaluacionProyectoAporte>();

                if (list.Any())
                {
                    foreach (var items in list)
                    {
                        if (items.id_TipoIndicador == 1)
                        {
                            lstaportes = consultas.Db.EvaluacionProyectoAportes
                                .Where(
                                    ea =>
                                    ea.CodProyecto == CodProyecto && ea.CodConvocatoria == CodConvocatoria &&
                                    ea.CodTipoIndicador == items.id_TipoIndicador).ToList();

                            var d = !string.IsNullOrEmpty(items.TotalSolicitado.ToString()) ? items.TotalSolicitado : 0;
                            if (d != null)
                                TotalSolicitadoAporte = (double)d;

                            TotalRecomendadoAporte = !string.IsNullOrEmpty(items.TotalRecomendado.ToString()) ? items.TotalRecomendado : 0;
                        }
                        else if (items.id_TipoIndicador == 2)
                        {
                            lstcapital = consultas.Db.EvaluacionProyectoAportes
                                .Where(
                                    ea =>
                                    ea.CodProyecto == CodProyecto && ea.CodConvocatoria == CodConvocatoria &&
                                    ea.CodTipoIndicador == items.id_TipoIndicador).ToList();

                            var d = !string.IsNullOrEmpty(items.TotalSolicitado.ToString()) ? items.TotalSolicitado : 0;
                            if (d != null)
                                TotalSolicitadoAporteC = (double)d;

                            TotalRecomendadoAporteC = !string.IsNullOrEmpty(items.TotalRecomendado.ToString()) ? items.TotalRecomendado : 0;
                        }
                        else if (items.id_TipoIndicador == 3)
                        {
                            lstdiferencia = consultas.Db.EvaluacionProyectoAportes
                                .Where(
                                    ea =>
                                    ea.CodProyecto == CodProyecto && ea.CodConvocatoria == CodConvocatoria &&
                                    ea.CodTipoIndicador == items.id_TipoIndicador).ToList();

                            var d = !string.IsNullOrEmpty(items.TotalSolicitado.ToString()) ? items.TotalSolicitado : 0;
                            if (d != null)
                                TotalSolicitadoAporteD = (double)d;

                            TotalRecomendadoAporteD = !string.IsNullOrEmpty(items.TotalRecomendado.ToString()) ? items.TotalRecomendado : 0;
                        }
                    }



                    if (lstaportes.Any())
                    {
                        GrAportes.DataSource = lstaportes;
                        GrAportes.DataBind();
                    }
                    else PnlInversiones.Visible = false;

                    if (lstdiferencia.Any())
                    {
                        GrvDifereridas.DataSource = lstdiferencia;
                        GrvDifereridas.DataBind();
                    }
                    else
                    { }

                    if (lstcapital.Any())
                    {
                        GrvCapital.DataSource = lstcapital;
                        GrvCapital.DataBind();
                    }
                    else
                    {
                        //panelcapital.Visible = false;
                        var sql = "INSERT INTO EvaluacionProyectoAporte SELECT " + CodProyecto + ", " + CodConvocatoria + ", 2, Componente, Observacion, Valor, null, 1, null ";
                        sql += "FROM proyectoCapital WHERE codProyecto = " + CodProyecto;
                        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                        var cmd = new SqlCommand(sql, conn);
                        conn.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                        //LlenarGrillas();
                    }

                }
                else
                {
                    var sql = "INSERT INTO EvaluacionProyectoAporte  SELECT codProyecto, " + CodConvocatoria + ", CASE WHEN UPPER(TipoInversion) = 'FIJA' THEN 1  WHEN UPPER(TipoInversion) = 'DIFERIDA' THEN 3 END, Concepto , Concepto, Valor, null, 1, null ";
                    sql +=  "FROM ProyectoInversion WHERE upper(TipoInversion) in('FIJA', 'DIFERIDA') AND codProyecto = " + CodProyecto;
                    var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    var cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();

                    //LlenarGrillas();
                }

                // cargo la grilla de integrantes
                var consulta = consultas.ObtenerIntegrantesIniciativa(Convert.ToString(CodProyecto),
                                                                        Convert.ToString(CodConvocatoria));
                if (consulta.Any())
                {
                    GrvIntegrantes.DataSource = consulta;
                    GrvIntegrantes.DataBind();
                }
                

                

                var info = (from tig in consultas.Db.TipoIndicadorGestions
                           from epa in consultas.Db.EvaluacionProyectoAportes
                           where epa.CodProyecto == CodProyecto
                           && epa.CodConvocatoria == CodConvocatoria
                           && epa.CodTipoIndicador == tig.Id_TipoIndicador
                           group new { tig, epa } by new { tig.nomTipoIndicador, tig.Id_TipoIndicador } into tig1
                           //orderby tig1.Select(x => x.tig.Id_TipoIndicador)
                           select new
                           {
                               nomTipoIndicador = tig1.Select(x => x.tig.nomTipoIndicador),
                               Id_TipoIndicador = tig1.Select(x => x.tig.Id_TipoIndicador),
                               TotalSolicitado = tig1.Select(x => x.epa.Solicitado == null ? 0 : x.epa.Solicitado).Sum(),
                               TotalRecomendado = tig1.Select(x => x.epa.Recomendado == null ? 0 : x.epa.Recomendado).Sum()
                           }
                           );

                foreach (var rs in info)
                {
                    var result = (from epa in consultas.Db.EvaluacionProyectoAportes
                                  where epa.CodProyecto == CodProyecto
                                  && epa.CodConvocatoria == CodConvocatoria
                                  && epa.CodTipoIndicador.Equals(rs.Id_TipoIndicador)
                                  orderby epa.Id_Aporte
                                  select new
                                  {
                                      epa.Id_Aporte,
                                      epa.Nombre,
                                      epa.Detalle,
                                      epa.Solicitado,
                                      recomendado = epa.Recomendado == null ? 0 : epa.Recomendado,
                                      epa.Protegido,
                                      epa.CodTipoIndicador
                                  });

                    switch (rs.Id_TipoIndicador.ToString())
                    {
                        case "1":
                            GrAportes.DataSource = result;
                            GrAportes.DataBind();
                            break;
                        case "2":
                            GrvCapital.DataSource = result;
                            GrvCapital.DataBind();
                            break;
                        case "3":
                            GrvDifereridas.DataSource = result;
                            GrvDifereridas.DataBind();
                            break;
                    }
                }
             
            }

            //Valor SMLMV recomendado
            float total = 0;
            var sql2 = "SELECT nomTipoIndicador, id_TipoIndicador, sum(solicitado) as TotalSolicitado, isnull(sum(Recomendado),0) as TotalRecomendado ";
            sql2 += "FROM TipoIndicadorGestion T, EvaluacionProyectoAporte E WHERE E.codProyecto="+ CodProyecto +" AND E.codConvocatoria="+ CodConvocatoria +" AND ";
            sql2 += "codTipoIndicador= id_tipoindicador GROUP BY nomTipoIndicador, id_TipoIndicador ORDER BY id_TipoIndicador";
            var recomendado = consultas.ObtenerDataTable(sql2, "text");

            foreach (DataRow row in recomendado.Rows)
            {
                total += float.Parse(row["TotalRecomendado"].ToString());
            }

            var objSalario = consultas.Db.SalariosMinimos.OrderByDescending(s => s.Id_SalariosMinimos).FirstOrDefault();
            txtrecomendado.Text = string.Format("{0:#,##0}", (total / objSalario.SalarioMinimo));
        }

        public void Permisos()
        {
            int codgrupo = Convert.ToInt32(usuario.CodGrupo);


            if (codgrupo == Constantes.CONST_Evaluador)
            {
                //miembro && !realizado &&
                
                Btnupdate.Enabled = true;
                GrAportes.Columns[1].Visible = false;
                GrvDifereridas.Columns[1].Visible = false;
                GrvCapital.Columns[1].Visible = false;

            }
            else
            {
                if (codgrupo == Constantes.CONST_GerenteEvaluador)
                {
                    pnlagregar.Visible = false;
                    GrvCapital.Columns[0].Visible = false;
                    GrvDifereridas.Columns[0].Visible = false;
                    GrvIntegrantes.Columns[0].Visible = false;
                    GrAportes.Columns[0].Visible = false;

                    GrvCapital.Columns[1].Visible = false;
                    GrvDifereridas.Columns[1].Visible = false;
                    GrAportes.Columns[1].Visible = false;

                    Btnupdate.Visible = false;
                }
                else
                {
                    //txtobservaciones.Enabled = false;
                    txtobservaciones.Enabled = false;
                    GrAportes.Columns[1].Visible = false;
                    GrvDifereridas.Columns[1].Visible = false;
                    GrvCapital.Columns[1].Visible = false;
                }
            }

            CargarObservaciones();
        }

        public void CargarObservaciones()
        {
            var LeftJoin = (from p in _consultas.Db.ProyectoFinanzasIngresos
                            join e in _consultas.Db.EvaluacionObservacions
                                on p.CodProyecto equals e.CodProyecto
                                into joinedEmpDept
                            from e in joinedEmpDept.DefaultIfEmpty()
                            where p.CodProyecto == CodProyecto && e.CodConvocatoria == CodConvocatoria
                            select new
                            {
                                p.Recursos,
                                valorRe = e.ValorRecomendado != null ? e.ValorRecomendado.Value : 0,
                                e.EquipoTrabajo

                            }).FirstOrDefault();
            if (LeftJoin != null)
            {

                txtsolicitado.Text = LeftJoin.Recursos.ToString();
                lbl_solicitado.Text = LeftJoin.Recursos.ToString();
                txtrecomendado.Text = LeftJoin.valorRe.ToString();
                lbl_recomendado.Text = LeftJoin.valorRe.ToString();
                txtobservaciones.Text = LeftJoin.EquipoTrabajo;
                lblvalidador.Text = "validar";

            }
            else
            {
                lblvalidador.Text = "";
            }
        }

        public void ObtenerAñoConvocatoria()
        {
            if (CodConvocatoria != 0)
            {
                DateTime fechainicio =
                    _consultas.Db.Convocatoria.Single(c => c.Id_Convocatoria == CodConvocatoria).FechaInicio;
                _anioConvocatoria = fechainicio.Year;
            }


        }

        public void ObtenerSalariosMinimos()
        {
            if (CodProyecto != 0)
            {
                _salariominimo =
                    _consultas.Db.ProyectoGastosPersonals.Count(p => p.CodProyecto == CodProyecto) +
                    _consultas.Db.ProyectoInsumos.Count(
                        pi => pi.codTipoInsumo == 2 && pi.CodProyecto == CodProyecto);

                if (_salariominimo <= 5)
                {
                    _salariominimo = 150;
                }
                else if (_salariominimo >= 6)
                {
                    _salariominimo = 180;
                }
                else _salariominimo = 150;
            }


        }

        [WebMethod]
        public static string Eliminar(string codigo)
        {
            string mensajeDeError = string.Empty;

            var entity = _consultas.Db.EvaluacionProyectoAportes.Single(
                p => p.Id_Aporte == Convert.ToInt64(codigo));

            if (entity.Id_Aporte != 0)
            {
                _consultas.Db.EvaluacionProyectoAportes.DeleteOnSubmit(entity);
                _consultas.Db.SubmitChanges();
                ////Actualizar fecha de actualización.
            
                mensajeDeError = "ok";
            }
            else
            {
                mensajeDeError = "El registro no se puede eliminar";
            }


            return JsonConvert.SerializeObject(new
            {
                mensaje = mensajeDeError
            });
        }

        protected void GrvDiferidas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var ltotalsolicitado = e.Row.FindControl("lblTotalSolicitadoD") as Label;
                var ltotalRecomendado = e.Row.FindControl("lblTotalRecomendadoD") as Label;
                var PorcentajeSolicitado = e.Row.FindControl("lblPorcentajeSolicitadoD") as Label;
                var PorcentajeRecomendado = e.Row.FindControl("lblPorcentajeRecomendadoD") as Label;


                double dcantidad = !string.IsNullOrEmpty(ltotalsolicitado.Text)
                                       ? double.Parse(ltotalsolicitado.Text.Replace("$", ""))
                                       : 0;
                double dvalor = !string.IsNullOrEmpty(ltotalRecomendado.Text)
                                    ? double.Parse(ltotalRecomendado.Text.Replace("$", ""))
                                    : 0;
                // Sumamos el total de la cantidad solicitada
                if (dcantidad != 0)
                {
                    totalDiferido1 += dcantidad;
                    LblTotalDiferida.Text = totalDiferido1.ToString("C");
                }
                else ltotalsolicitado.Text = dcantidad.ToString("C");

                //sumamos el total de la cantidad recomendad
                if (dvalor != 0)
                {
                    totalDiferido += dvalor;
                    ldif2.Text = totalDiferido.ToString("C");
                }
                else ltotalRecomendado.Text = dvalor.ToString("C");

                // Sacamos el porcentaje de lo solicitado y tambien de lo recomendado

                if (TotalSolicitadoAporteD != 0)
                {
                    PorcentajeSolicitado.Text = string.Format("{0:00.00}", ((dcantidad * 100) / TotalSolicitadoAporteD)).ToString();
                    dif1D += (float)Convert.ToDecimal(PorcentajeSolicitado.Text);
                    ldif1.Text = dif1D.ToString();
                    //ldif1.Text = TotalSolicitadoAporteD.ToString("C");
                }
                else { PorcentajeSolicitado.Text = "0"; ldif1.Text = "0"; }

                if (TotalRecomendadoAporteD != 0)
                {
                    PorcentajeRecomendado.Text = string.Format("{0:00.00}",
                                                              ((dvalor * 100) / TotalRecomendadoAporteD).ToString());
                    dif2D += (float)Convert.ToDecimal(PorcentajeRecomendado.Text);
                    ldif3.Text = dif2D.ToString();
                    //ldif2.Text = TotalRecomendadoAporteD.ToString("C");
                }
                else { PorcentajeRecomendado.Text = "0"; ldif3.Text = "0"; }

            }


        }

        protected void GrvCapital_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ltotalsolicitado = e.Row.FindControl("lblTotalSolicitadoC") as Label;
                var ltotalRecomendado = e.Row.FindControl("lblTotalRecomendadoC") as Label;
                var PorcentajeSolicitado = e.Row.FindControl("lblPorcentajeSolicitadoC") as Label;
                var PorcentajeRecomendado = e.Row.FindControl("lblPorcentajeRecomendadoC") as Label;

                double dcantidad = !string.IsNullOrEmpty(ltotalsolicitado.Text)
                                       ? double.Parse(ltotalsolicitado.Text.Replace("$", ""))
                                       : 0;

                ltotalsolicitado.Text = dcantidad.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                double dvalor = !string.IsNullOrEmpty(ltotalRecomendado.Text)
                                    ? double.Parse(ltotalRecomendado.Text.Replace("$", ""))
                                    : 0;

                ltotalRecomendado.Text = dvalor.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                // Sumamos el total de la cantidad solicitada
                if (dcantidad != 0)
                {
                    totalCapital1 += dcantidad;
                    LblTotalCapital.Text = totalCapital1.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }
                else ltotalsolicitado.Text = dcantidad.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                //sumamos el total de la cantidad recomendad
                if (dvalor != 0)
                {
                    totalCapital += dvalor;
                    ldif2c.Text = totalCapital.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }
                else ltotalRecomendado.Text = dvalor.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                // Sacamos el porcentaje de lo solicitado y tambien de lo recomendado

                if (TotalSolicitadoAporteC != 0)
                {
                    PorcentajeSolicitado.Text = string.Format("{0:00.00}", ((dcantidad * 100) / TotalSolicitadoAporteC)).ToString();
                    dif1C += (float)Convert.ToDecimal(PorcentajeSolicitado.Text);
                    ldif1c.Text = dif1C.ToString();
                }
                else { PorcentajeSolicitado.Text = "0"; ldif1c.Text = "0"; }

                if (TotalRecomendadoAporteC != 0)
                {
                    PorcentajeRecomendado.Text = string.Format("{0:00.00}", ((dvalor * 100) / TotalRecomendadoAporteC).ToString());
                    dif2c += (float)Convert.ToDecimal(PorcentajeRecomendado.Text);
                    ldif3c.Text = dif2c.ToString();
                    //ldif2c.Text = TotalRecomendadoAporteC.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }
                else
                {
                    PorcentajeRecomendado.Text = "0";
                    ldif3c.Text = "0";
                }

                var obj = (EvaluacionProyectoAporte)e.Row.DataItem;
                if (obj.Protegido)
                {
                    e.Row.Cells[1].Controls.Clear();
                }
            }
        }

        protected void GrAportes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ltotalsolicitado = e.Row.FindControl("lblTotalSolicitado") as Label;
                var ltotalRecomendado = e.Row.FindControl("lblTotalRecomendado") as Label;
                var PorcentajeSolicitado = e.Row.FindControl("lblPorcentajeSolicitado") as Label;
                var PorcentajeRecomendado = e.Row.FindControl("lblPorcentajeRecomendado") as Label;
                var img_1 = e.Row.FindControl("imgeditar") as Image;

                double dcantidad = !string.IsNullOrEmpty(ltotalsolicitado.Text)
                                       ? double.Parse(ltotalsolicitado.Text.Replace("$", ""))
                                       : 0;
                ltotalsolicitado.Text = dcantidad.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                double dvalor = !string.IsNullOrEmpty(ltotalRecomendado.Text)
                                    ? double.Parse(ltotalRecomendado.Text.Replace("$", ""))
                                    : 0;

                ltotalRecomendado.Text = dvalor.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                //sumamos el total de la cantidad recomendad
                if (dvalor != 0)
                {
                    totalInversion1 += dvalor;
                    ldif2A.Text = totalInversion1.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }
                else ltotalRecomendado.Text = dvalor.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                // Sumamos el total de la cantidad solicitada
                if (dcantidad != 0)
                {
                    totalInversion += dcantidad;
                    LblTotal.Text = totalInversion.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }
                else ltotalsolicitado.Text = dcantidad.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                // Sacamos el porcentaje de lo solicitado y tambien de lo recomendado

                if (TotalSolicitadoAporte != 0)
                {
                    PorcentajeSolicitado.Text = string.Format("{0:00.00}", ((dcantidad * 100) / TotalSolicitadoAporte)).ToString();
                    dif1A += (float)Convert.ToDecimal(PorcentajeSolicitado.Text);
                    ldif1A.Text = dif1A.ToString();
                    //ldif1A.Text = TotalSolicitadoAporte.ToString();
                }
                else { PorcentajeSolicitado.Text = "0"; ldif1A.Text = "0"; }

                if (TotalRecomendadoAporte != 0)
                {
                    PorcentajeRecomendado.Text = string.Format("{0:00.00}", ((dvalor * 100) / TotalRecomendadoAporte)).ToString();
                    dif2A += (float)Convert.ToDecimal(PorcentajeRecomendado.Text);
                    ldif3A.Text = dif2A.ToString();
                    //ldif2A.Text = TotalRecomendadoAporte.ToString("C");
                }
                else
                {
                    PorcentajeRecomendado.Text = "0";
                    ldif3A.Text = "0";
                }

                var obj = (EvaluacionProyectoAporte)e.Row.DataItem;
                if(obj.Protegido)
                {
                    e.Row.Cells[1].Controls.Clear();
                }
            }
        }

        protected void GrvIntegrantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var laporteDinero = e.Row.FindControl("lblAporteDinero") as Label;
                var laporteEspecie = e.Row.FindControl("lblAporteEspecie") as Label;
                var laporteTotal = e.Row.FindControl("lblAporteTotal") as Label;
                var lbeneficiario = e.Row.FindControl("lblEmprendedor") as Label;
                var lotro = e.Row.FindControl("lblotro") as Label;

                if (!string.IsNullOrEmpty(laporteDinero.Text)
                    && laporteDinero.Text != "0" && !string.IsNullOrEmpty(laporteEspecie.Text)
                    && laporteEspecie.Text != "0")
                {
                    aporteTotal += ((Convert.ToDouble(laporteDinero.Text) + Convert.ToDouble(laporteEspecie.Text)) / 1000);
                    laporteTotal.Text = aporteTotal.ToString();
                }
                else
                {
                    laporteTotal.Text = "0";
                }
                if (lbeneficiario.Text == "True")
                {
                    lotro.Text = string.Empty;
                    lbeneficiario.Text = "<img src='../../Images/chulo.gif' />";
                }
                else
                {
                    lbeneficiario.Text = string.Empty;
                    lotro.Text = "<img src='../../Images/chulo.gif' />";
                }

            }
        }

        protected void GrAportes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrAportes.PageIndex = e.NewPageIndex;
            LlenarGrillas();
        }

        protected void GrvDiferidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvDifereridas.PageIndex = e.NewPageIndex;
            LlenarGrillas();
        }

        protected void GrvCapital_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvCapital.PageIndex = e.NewPageIndex;
            LlenarGrillas();
        }


        protected void GrvIntegrantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvIntegrantes.PageIndex = e.NewPageIndex;
            LlenarGrillas();
        }

        protected void Btnupdate_Click(object sender, EventArgs e)
        {
            Actualizar();
            prActualizarTabEval(Constantes.CONST_subAportes.ToString(), CodProyecto.ToString(), CodConvocatoria.ToString());
            ObtenerDatosUltimaActualizacion();
            Response.Redirect("EvaluacionAportes.aspx");
        }

        public void Actualizar()
        {
            CodProyecto = (int)(!string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? Convert.ToInt64(HttpContext.Current.Session["CodProyecto"]) : 0);
            CodConvocatoria = (int)(!string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? Convert.ToInt64(HttpContext.Current.Session["CodConvocatoria"]) : 0);

            var evaluacionProyectoAporte = consultas.Db.EvaluacionObservacions.FirstOrDefault(o => o.CodProyecto == CodProyecto && o.CodConvocatoria == CodConvocatoria);
            var evaluacion = new EvaluacionObservacion();


            if (evaluacionProyectoAporte != null)
            {
                evaluacionProyectoAporte.ValorRecomendado = (short)Convert.ToInt64(txtrecomendado.Text);
                evaluacionProyectoAporte.EquipoTrabajo = txtobservaciones.Text;
                consultas.Db.SubmitChanges();
            }
            else
            {
                evaluacion.CodProyecto = CodProyecto;
                evaluacion.CodConvocatoria = CodConvocatoria;
                evaluacion.Actividades = string.Empty;
                evaluacion.ProductosServicios = string.Empty;
                evaluacion.EstrategiaMercado = string.Empty;
                evaluacion.ProcesoProduccion = string.Empty;
                evaluacion.EstructuraOrganizacional = string.Empty;
                evaluacion.TamanioLocalizacion = string.Empty;
                evaluacion.Generales = string.Empty;
                evaluacion.ValorRecomendado = (short)Convert.ToInt64(txtrecomendado.Text);
                evaluacion.EquipoTrabajo = txtobservaciones.Text;
                consultas.Db.EvaluacionObservacions.InsertOnSubmit(evaluacion);
            }

        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bNuevo = true; //Indica si las aprobaciones de las pestañas pueden ser levantadas por el evaluador.
            bRealizado = false;
            bool bEnActa = false; //Determinar si el proyecto esta incluido en un acta de comite evaluador.
            bool EsMiembro = false;
            Int32 CodigoEstado = 0;
            StringBuilder sbQuery = new StringBuilder();

            try
            {
                //Consultar si es "Nuevo".
                bNuevo = es_bNuevo(CodProyecto.ToString());

                //Determinar si "está en acta".
                bEnActa = es_EnActa(CodProyecto.ToString(), CodConvocatoria.ToString());

                //Consultar si es "Miembro".
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodProyecto.ToString());

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(Constantes.CONST_subAportes.ToString(), CodProyecto.ToString(), CodConvocatoria.ToString());

                #region Obtener el rol.

                //Consulta.
                //txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                //         " Where CodProyecto = " + CodProyecto + " And CodContacto = " + usuario.IdContacto +
                //         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";
                sbQuery = new StringBuilder();
                sbQuery.Append( " SELECT CodContacto, CodRol From ProyectoContacto " );
                sbQuery.Append( " Where CodProyecto = " + CodProyecto + " And CodContacto = " + usuario.IdContacto );
                sbQuery.Append( " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ");

                //Asignar variables a DataTable.
                //var rs = consultas.ObtenerDataTable(txtSQL, "text");
                var rs = consultas.ObtenerDataTable(sbQuery.ToString(), "text");

                if (rs.Rows.Count > 0)
                {
                    //Crear la variable de sesión.
                    HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString();
                }

                //Destruir la variable.
                rs = null;

                #endregion

                string codProyecto2 = HttpContext.Current.Session["CodProyecto"].ToString();
                string codConvocatoria2 = HttpContext.Current.Session["CodConvocatoria"].ToString();

                // 2015/06/05 Roberto Alvarado
                // Se coloca un StringBuilder para mejorar el performance de la creacion de las cadenas de texto.
                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                //txtSQL = " select nombres+' '+apellidos as nombre, fechamodificacion, realizado  " +
                //         " from tabEvaluacionproyecto, contacto " +
                //         " where id_contacto = codcontacto and codtabEvaluacion = " + Constantes.CONST_subAportes +
                //         " and codproyecto = " + codProyecto2 +
                //         " and codconvocatoria = " + codConvocatoria2;
                sbQuery = new StringBuilder();
                sbQuery.Append(" select nombres+' '+apellidos as nombre, fechamodificacion, realizado  ");
                sbQuery.Append(" from tabEvaluacionproyecto, contacto " );
                sbQuery.Append(" where id_contacto = codcontacto and codtabEvaluacion = " + Constantes.CONST_subAportes );
                sbQuery.Append(" and codproyecto = " + codProyecto2 );
                sbQuery.Append(" and codconvocatoria = " + codConvocatoria2);

                //Asignar resultados de la consulta a variable DataTable.
                //tabla = consultas.ObtenerDataTable(txtSQL, "text");
                tabla = consultas.ObtenerDataTable(sbQuery.ToString(), "text");

                //Si tiene datos "y debe tenerlos" ejecuta el siguiente código.
                if (tabla.Rows.Count > 0)
                {
                    //Nombre del usuario quien hizo la actualización.
                    lbl_nombre_user_ult_act.Text = tabla.Rows[0]["nombre"].ToString().ToUpperInvariant();

                    #region Formatear la fecha.

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(tabla.Rows[0]["FechaModificacion"].ToString()); }
                    catch { fecha = DateTime.Today; }

                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                    //Obtener la hora en minúscula.
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();

                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }

                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    #endregion

                    //Valor "bRealizado".
                    bRealizado = Convert.ToBoolean(tabla.Rows[0]["Realizado"].ToString());
                }

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                ////Evaluar "habilitación" del CheckBox.
                ////if (!(EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString()) || lbl_nombre_user_ult_act.Text.Trim() == "" || CodigoEstado != Constantes.CONST_Evaluacion || bEnActa)
                //if (!(EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString()) || lbl_nombre_user_ult_act.Text.Trim() == "" || CodigoEstado != Constantes.CONST_Evaluacion || bEnActa)
                //{ 
                //    chk_realizado.Enabled = false;
                    
                //}

                ////if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString() && lbl_nombre_user_ult_act.Text.Trim() != "" && CodigoEstado == Constantes.CONST_Evaluacion && (!bEnActa))
                //if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString() && lbl_nombre_user_ult_act.Text.Trim() != "" && CodigoEstado == Constantes.CONST_Evaluacion && (!bEnActa))
                //{
                //    btn_guardar_ultima_actualizacion.Enabled = true;
                //    btn_guardar_ultima_actualizacion.Visible = true;
                //}

                //Nuevos controles para los check
                //Si es coordinador de evaluacion debe tener habilitado los checks
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                {
                    btn_guardar_ultima_actualizacion.Visible = true;
                    chk_realizado.Enabled = true;
                }
                else
                {
                    btn_guardar_ultima_actualizacion.Visible = false;
                    chk_realizado.Enabled = false;
                }

                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                tabla = null;
                txtSQL = null;
                return;
            }
        }

        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;

            //Hallar numero de post it por tab
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == CodProyecto
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }

        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            int flag = 0;
            flag = Marcar(Constantes.CONST_subAportes.ToString(), CodProyecto.ToString(), CodConvocatoria.ToString(), chk_realizado.Checked);
            ObtenerDatosUltimaActualizacion();

            if (flag == 1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }
        }

        
    }
}




