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
using System.Globalization;

namespace Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public partial class Aportes : Negocio.Base_Page
    {
        /// <summary>
        /// Determina el comportamiento de Evaluacion aportes
        /// </summary>

        private double totalInversion, totalCapital, totalDiferido, totalInversion1, totalCapital1, totalDiferido1;
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_AportesV2; } set { } }
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
            EncabezadoEval.IdProyecto = CodigoProyecto;
            EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
            EncabezadoEval.IdTabEvaluacion = CodigoTab;

            inicioEncabezado(Convert.ToString(CodigoProyecto), Convert.ToString(CodigoConvocatoria), CodigoTab);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

            //Consultar si está "realizado".
            bRealizado = esRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

            if (esMiembro && !bRealizado)
            {
                this.div_Post_It1.Visible = true;
                Post_It1._mostrarPost = true;
            }
            else
            {
                this.div_Post_It1.Visible = false;
                Post_It1._mostrarPost = false;
                txtobservaciones.Enabled = false;
            }

            if (!IsPostBack)
            {

                ObtenerAñoConvocatoria();
                Permisos();
                ObtenerSalariosMinimos();
                LlenarGrillas();
                if (Constantes.CONST_Evaluador == usuario.CodGrupo)
                    Actualizar();
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
            if (CodigoConvocatoria != 0 && CodigoProyecto != 0)
            {
                // cargo la grilla de aportes
                GetAportes(CodigoProyecto, CodigoConvocatoria);
                var list = consultas.ObtenerAportes(Convert.ToString(CodigoProyecto), Convert.ToString(CodigoConvocatoria));
                var lstaportes = new List<Aporte>();
                var lstcapital = new List<Aporte>();
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
                                    ea.CodProyecto == CodigoProyecto && ea.CodConvocatoria == CodigoConvocatoria &&
                                    ea.CodTipoIndicador == items.id_TipoIndicador)
                                .Select(
                                    selector => new Aporte()
                                    {
                                        Id_Aporte = selector.Id_Aporte,
                                        CodProyecto = selector.CodProyecto,
                                        CodConvocatoria = selector.CodConvocatoria,
                                        CodTipoIndicador = selector.CodTipoIndicador,
                                        Nombre = selector.Nombre,
                                        Detalle = selector.Detalle,
                                        Solicitado = selector.Solicitado,
                                        Recomendado = selector.Recomendado,
                                        Protegido = selector.Protegido,
                                        IdFuenteFinanciacion = selector.IdFuenteFinanciacion,
                                        FuenteFinanciacionTMP = selector.IdFuenteFinanciacion != null ? consultas.Db.FuenteFinanciacions.FirstOrDefault(filter => filter.IdFuente.Equals(selector.IdFuenteFinanciacion)).DescFuente : string.Empty
                                    })
                                    .OrderBy(orderBy => orderBy.Id_Aporte)
                                    .ToList();

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
                                    ea.CodProyecto == CodigoProyecto && ea.CodConvocatoria == CodigoConvocatoria &&
                                    ea.CodTipoIndicador == items.id_TipoIndicador).Select(
                                    selector => new Aporte()
                                    {
                                        Id_Aporte = selector.Id_Aporte,
                                        CodProyecto = selector.CodProyecto,
                                        CodConvocatoria = selector.CodConvocatoria,
                                        CodTipoIndicador = selector.CodTipoIndicador,
                                        Nombre = selector.Nombre,
                                        Detalle = selector.Detalle.Replace("(Emprendedor)", string.Empty).Replace("(Fondo)", string.Empty).Replace("(Ventas)", string.Empty),
                                        Solicitado = selector.Solicitado,
                                        Recomendado = selector.Recomendado,
                                        Protegido = selector.Protegido,
                                        IdFuenteFinanciacion = selector.IdFuenteFinanciacion,
                                        FuenteFinanciacionTMP = selector.IdFuenteFinanciacion != null ? consultas.Db.FuenteFinanciacions.FirstOrDefault(filter => filter.IdFuente.Equals(selector.IdFuenteFinanciacion)).DescFuente : string.Empty,
                                        TipoAporte = selector.Nombre.Equals("Nómina") ? GetTipoAporte(selector.Detalle) : string.Empty
                                    })
                                    .OrderBy(orderBy => orderBy.Id_Aporte)
                                    .ToList();

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
                                    ea.CodProyecto == CodigoProyecto && ea.CodConvocatoria == CodigoConvocatoria &&
                                    ea.CodTipoIndicador == items.id_TipoIndicador)
                                    .OrderBy(orderBy => orderBy.Id_Aporte)
                                    .ToList();

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
                    else
                    {
                        PnlInversiones.Visible = false;
                    };

                    if (lstdiferencia.Any())
                    {
                        GrvDifereridas.DataSource = lstdiferencia;
                        GrvDifereridas.DataBind();
                    }

                    if (lstcapital.Any())
                    {
                        GrvCapital.DataSource = lstcapital;
                        GrvCapital.DataBind();
                    }
                }

                // cargo la grilla de integrantes
                var consulta = consultas.ObtenerIntegrantesIniciativa(Convert.ToString(CodigoProyecto),
                                                                        Convert.ToString(CodigoConvocatoria));
                if (consulta.Any())
                {
                    GrvIntegrantes.DataSource = consulta;
                    GrvIntegrantes.DataBind();
                }




                var info = (from tig in consultas.Db.TipoIndicadorGestions
                            from epa in consultas.Db.EvaluacionProyectoAportes
                            where epa.CodProyecto == CodigoProyecto
                            && epa.CodConvocatoria == CodigoConvocatoria
                            && epa.CodTipoIndicador == tig.Id_TipoIndicador
                            group new { tig, epa } by new { tig.nomTipoIndicador, tig.Id_TipoIndicador } into tig1
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
                                  where epa.CodProyecto == CodigoProyecto
                                  && epa.CodConvocatoria == CodigoConvocatoria
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
            var sql2 = "SELECT nomTipoIndicador, id_TipoIndicador, sum(solicitado) as TotalSolicitado, isnull(sum(Recomendado),0) as TotalRecomendado, MAX(YEAR(Convocatoria.FechaInicio)) as AnnoConvocatoria ";
            sql2 += "FROM TipoIndicadorGestion T, EvaluacionProyectoAporte E  inner join Convocatoria on E.CodConvocatoria = Convocatoria.Id_Convocatoria WHERE E.codProyecto=" + CodigoProyecto + " AND E.codConvocatoria=" + CodigoConvocatoria + " AND ";
            sql2 += "codTipoIndicador= id_tipoindicador GROUP BY nomTipoIndicador, id_TipoIndicador ORDER BY id_TipoIndicador";
            var recomendado = consultas.ObtenerDataTable(sql2, "text");
            float annoConvocatoria = 0;

            foreach (DataRow row in recomendado.Rows)
            {
                total += float.Parse(row["TotalRecomendado"].ToString());
                annoConvocatoria = float.Parse(row["AnnoConvocatoria"].ToString());
            }


            var objSalario = consultas.Db.SalariosMinimos.Where(filter => filter.AñoSalario.Equals(annoConvocatoria)).FirstOrDefault();
            //txtrecomendado.Text = string.Format("{0:#,##0}", (total / objSalario.SalarioMinimo));

            txtrecomendado.Text = (total / objSalario.SalarioMinimo).ToString();

        }

        public string GetTipoAporte(string tipoAporte)
        {
            if (tipoAporte.Contains("(Emprendedor)"))
                return "Aportes Emprendedores";
            if (tipoAporte.Contains("(Fondo)"))
                return "Fondo Emprender";
            if (tipoAporte.Contains("(Ventas)"))
                return "Ingresos por Ventas";

            return string.Empty;
        }

        public void GetAportes(int codigoProyecto, int codigoConvocatoria)
        {
            try
            {
                var entities = consultas.ObtenerAportes(Convert.ToString(CodigoProyecto), Convert.ToString(codigoConvocatoria));

                if (!entities.Any())
                {
                    GetInversionesDiferidas(codigoProyecto, codigoConvocatoria);
                    GetCapitalTrabajo(codigoProyecto, codigoConvocatoria);
                    GetNomina(codigoProyecto, codigoConvocatoria);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void GetNomina(int codigoProyecto, int codigoConvocatoria)
        {
            var entities = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Productividad.getCargos(codigoProyecto);

            var valorAporteEmprendedor = entities.Sum(sumatory => sumatory.AportesEmprendedor);
            var valorAporteFondoEmprender = entities.Sum(sumatory => sumatory.ValorFondoEmprender);
            var valorIngresoPorVentas = entities.Sum(sumatory => sumatory.IngresosVentas);

            var aporteEmprendedor = new EvaluacionProyectoAporte
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTipoIndicador = 2,
                Nombre = "Nómina",
                Detalle = "Gastos de personal (Emprendedor)",
                Solicitado = (double)valorAporteEmprendedor.GetValueOrDefault(0),
                Protegido = true
            };

            var aporteFondoEmprender = new EvaluacionProyectoAporte
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTipoIndicador = 2,
                Nombre = "Nómina",
                Detalle = "Gastos de personal (Fondo)",
                Solicitado = (double)valorAporteFondoEmprender.GetValueOrDefault(0),
                Protegido = true
            };

            var aporteIngresoPorVentas = new EvaluacionProyectoAporte
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTipoIndicador = 2,
                Nombre = "Nómina",
                Detalle = "Gastos de personal (Ventas)",
                Solicitado = (double)valorIngresoPorVentas.GetValueOrDefault(0),
                Protegido = true
            };

            Negocio.PlanDeNegocioV2.Evaluacion.ConceptoFinal.Aportes.Insert(aporteEmprendedor);
            Negocio.PlanDeNegocioV2.Evaluacion.ConceptoFinal.Aportes.Insert(aporteFondoEmprender);
            Negocio.PlanDeNegocioV2.Evaluacion.ConceptoFinal.Aportes.Insert(aporteIngresoPorVentas);
        }

        public void GetCapitalTrabajo(int codigoProyecto, int codigoConvocatoria)
        {
            var sql = "INSERT INTO EvaluacionProyectoAporte SELECT " + codigoProyecto + ", " + codigoConvocatoria + ", 2, Componente, Observacion, Valor, null, 1, codFuenteFinanciacion ";
            sql += "FROM proyectoCapital WHERE codProyecto = " + codigoProyecto;
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        private void GetInversionesDiferidas(int codigoProyecto, int codigoConvocatoria)
        {
            var sql = "INSERT INTO EvaluacionProyectoAporte  SELECT codProyecto, " + codigoConvocatoria + ", CASE WHEN UPPER(TipoInversion) = 'FIJA' THEN 1  WHEN UPPER(TipoInversion) = 'DIFERIDA' THEN 3 END, Concepto , Concepto, Valor, null, 1, idfuentefinanciacion ";
            sql += "FROM ProyectoInversion WHERE upper(TipoInversion) in('FIJA', 'DIFERIDA') AND codProyecto = " + codigoProyecto;
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public void Permisos()
        {
            int codgrupo = Convert.ToInt32(usuario.CodGrupo);


            if (codgrupo == Constantes.CONST_Evaluador)
            {
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
                            where p.CodProyecto == CodigoProyecto && e.CodConvocatoria == CodigoConvocatoria
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
            if (CodigoConvocatoria != 0)
            {
                DateTime fechainicio =
                    _consultas.Db.Convocatoria.Single(c => c.Id_Convocatoria == CodigoConvocatoria).FechaInicio;
                _anioConvocatoria = fechainicio.Year;
            }


        }

        public void ObtenerSalariosMinimos()
        {
            if (CodigoProyecto != 0)
            {
                _salariominimo =
                    _consultas.Db.ProyectoGastosPersonals.Count(p => p.CodProyecto == CodigoProyecto) +
                    _consultas.Db.ProyectoInsumos.Count(
                        pi => pi.codTipoInsumo == 2 && pi.CodProyecto == CodigoProyecto);

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
                }
                else { PorcentajeSolicitado.Text = "0"; ldif1.Text = "0"; }

                if (TotalRecomendadoAporteD != 0)
                {
                    PorcentajeRecomendado.Text = string.Format("{0:00.00}",
                                                              ((dvalor * 100) / TotalRecomendadoAporteD).ToString());
                    dif2D += (float)Convert.ToDecimal(PorcentajeRecomendado.Text);
                    ldif3.Text = dif2D.ToString();
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
                    //dif2c += (float) Convert.ToDecimal(PorcentajeRecomendado.Text);
                    dif2c += (float)Decimal.Parse(PorcentajeRecomendado.Text, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                    ldif3c.Text = dif2c.ToString();
                }
                else
                {
                    PorcentajeRecomendado.Text = "0";
                    ldif3c.Text = "0";
                }

                var obj = (Aporte)e.Row.DataItem;
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

                var obj = (Aporte)e.Row.DataItem;
                if (obj.Protegido)
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
                    lbeneficiario.Text = "<img src='../../../Images/chulo.gif' />";
                }
                else
                {
                    lbeneficiario.Text = string.Empty;
                    lotro.Text = "<img src='../../../Images/chulo.gif' />";
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
            UpdateTab();

            Response.Redirect("Aportes.aspx?codproyecto=" + CodigoProyecto);
        }

        public void Actualizar()
        {
            var evaluacionProyectoAporte = consultas.Db.EvaluacionObservacions.FirstOrDefault(o => o.CodProyecto == CodigoProyecto && o.CodConvocatoria == CodigoConvocatoria);
            var evaluacion = new EvaluacionObservacion();

            float valorRecomendado = 0;

            if (txtrecomendado.Text != "" || txtrecomendado.Text != null)
            {
                valorRecomendado = float.Parse(txtrecomendado.Text);
            }


            if (evaluacionProyectoAporte != null)
            {
                //evaluacionProyectoAporte.ValorRecomendado = (short)Convert.ToInt64(txtrecomendado.Text.Replace(".",string.Empty).Replace(",",string.Empty));
                evaluacionProyectoAporte.ValorRecomendado = valorRecomendado;
                evaluacionProyectoAporte.EquipoTrabajo = txtobservaciones.Text;
                consultas.Db.SubmitChanges();
            }
            else
            {
                evaluacion.CodProyecto = CodigoProyecto;
                evaluacion.CodConvocatoria = CodigoConvocatoria;
                evaluacion.Actividades = string.Empty;
                evaluacion.ProductosServicios = string.Empty;
                evaluacion.EstrategiaMercado = string.Empty;
                evaluacion.ProcesoProduccion = string.Empty;
                evaluacion.EstructuraOrganizacional = string.Empty;
                evaluacion.TamanioLocalizacion = string.Empty;
                evaluacion.Generales = string.Empty;
                //evaluacion.ValorRecomendado = (short)Convert.ToInt64(txtrecomendado.Text);
                evaluacion.ValorRecomendado = valorRecomendado;
                evaluacion.EquipoTrabajo = txtobservaciones.Text;
                consultas.Db.EvaluacionObservacions.InsertOnSubmit(evaluacion);
                consultas.Db.SubmitChanges();
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

        private void UpdateTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)CodigoTab,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };

            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
            Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");
            EncabezadoEval.GetUltimaActualizacion();
        }

    }

    public class Aporte
    {

        public int Id_Aporte { get; set; }
        public int CodProyecto { get; set; }
        public int CodConvocatoria { get; set; }
        public int CodTipoIndicador { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public Double Solicitado { get; set; }
        public double? Recomendado { get; set; }
        public bool Protegido { get; set; }
        public int? IdFuenteFinanciacion { get; set; }
        public string FuenteFinanciacionTMP { get; set; }
        public string FuenteFinanciacion
        {
            get
            {
                if (Nombre.Equals("Nómina") && !String.IsNullOrEmpty(TipoAporte))
                    return TipoAporte;
                else
                {
                    if (IdFuenteFinanciacion != null)
                        return FuenteFinanciacionTMP;
                    else
                        return string.Empty;
                }
            }
            set { }
        }
        public string TipoAporte { get; set; }
    }
}