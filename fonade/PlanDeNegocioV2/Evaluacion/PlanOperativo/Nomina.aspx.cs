using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using Fonade.Clases;

namespace Fonade.PlanDeNegocioV2.Evaluacion.PlanOperativo
{
    public partial class Nomina : Negocio.Base_Page
    {
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_NominaV2; } set { } }
        public bool esMiembro;
        /// <summary>
        /// Indica si está o no "realizado".
        /// </summary>
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            EncabezadoEval.IdProyecto = CodigoProyecto;
            EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
            EncabezadoEval.IdTabEvaluacion = CodigoTab;

            if (!IsPostBack)
            {
                
                esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());                
                bRealizado = esRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                if (esMiembro && !bRealizado) { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }

                CargarGridPersonalCalificado();
                CargarGridManodeObraDirecta();
                UpdateTab();
            }
            prActualizarTabEval(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());

        }

        protected void CargarGridPersonalCalificado()
        {

            var query = (from p in consultas.Db.ProyectoGastosPersonals
                         where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                         orderby p.Id_Cargo ascending
                         select new { p.Id_Cargo, p.Cargo });
            if (query.Count() != 0)
            {

                var query2 = (from p1 in consultas.Db.MD_MostrarEvalNominaPersonal(Convert.ToInt32(CodigoProyecto))
                              select new
                              {
                                  p1,
                              });

                DataTable datos = new DataTable();
                DataTable detalle = new DataTable();
                datos.Columns.Add("CodProyecto");
                datos.Columns.Add("Id_Cargo");
                datos.Columns.Add("Cargo");
                for (int i = 0; i <= 12; i++)
                {
                    detalle.Columns.Add("Sueldo" + i);
                    detalle.Columns.Add("Prestaciones" + i);
                }
                detalle.Columns.Add("SueldoTotal");
                detalle.Columns.Add("PrestacionesTotal");

                foreach (var cargo in query)
                {
                    DataRow dr = datos.NewRow();

                    dr["CodProyecto"] = CodigoProyecto;
                    dr["Id_Cargo"] = cargo.Id_Cargo;
                    dr["Cargo"] = cargo.Cargo;
                    datos.Rows.Add(dr);
                }

                foreach (var consulta in query2)
                {
                    DataRow drDet = detalle.NewRow();                   
                    for (int i = Convert.ToInt32(consulta.p1.GeneradoPrimerAno); i <= 12; i++)                   
                    {
                        drDet["Sueldo" + i] = "$" + consulta.p1.valormensual.ToString("0,0.00", CultureInfo.InvariantCulture);
                        drDet["Prestaciones" + i] = "$" + ((decimal)consulta.p1.prestaciones).ToString("0,0.00", CultureInfo.InvariantCulture);
                    }
                    drDet["SueldoTotal"] = "$" + (consulta.p1.valormensual * (12 - (Convert.ToInt32(consulta.p1.GeneradoPrimerAno) - 1))).ToString("0,0.00", CultureInfo.InvariantCulture);
                    drDet["PrestacionesTotal"] = "$" + (((decimal)consulta.p1.prestaciones) * (12 - (Convert.ToInt32(consulta.p1.GeneradoPrimerAno) - 1))).ToString("0,0.00", CultureInfo.InvariantCulture);
                    detalle.Rows.Add(drDet);
                }

                gw_Anexos.DataSource = datos;
                gw_Anexos.DataBind();

                gw_AnexosActividad.DataSource = detalle;
                gw_AnexosActividad.DataBind();

                PanelPersonalCalificado.Visible = true;
            }

        }

        protected void CargarGridManodeObraDirecta()
        {
            try
            {
                var query = (from p in consultas.Db.ProyectoInsumos
                             where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             && p.codTipoInsumo == 2
                             orderby p.Id_Insumo ascending
                             select new { p.Id_Insumo, p.nomInsumo });
                if (query.Count() != 0)
                {

                    var query2 = (from p1 in consultas.Db.MD_MostrarEvalNominaManoObra(Convert.ToInt32(CodigoProyecto))
                                  select new
                                  {
                                      p1,
                                  });

                    DataTable datos = new DataTable();
                    DataTable detalle = new DataTable();
                    datos.Columns.Add("CodProyecto");
                    datos.Columns.Add("Id_Insumo");
                    datos.Columns.Add("nomInsumo");

                    for (int i = 1; i <= 12; i++)
                    {
                        detalle.Columns.Add("Sueldo" + i);
                        detalle.Columns.Add("Prestaciones" + i);
                    }
                    detalle.Columns.Add("SueldoTotal");
                    detalle.Columns.Add("PrestacionesTotal");

                    foreach (var cargo in query)
                    {
                        DataRow dr = datos.NewRow();

                        dr["CodProyecto"] = CodigoProyecto;
                        dr["Id_Insumo"] = cargo.Id_Insumo;
                        dr["nomInsumo"] = cargo.nomInsumo;
                        datos.Rows.Add(dr);
                    }

                    foreach (var consulta in query2)
                    {
                        DataRow drDet = detalle.NewRow();

                        for (int i = Convert.ToInt32(consulta.p1.GeneradoPrimerAno); i <= 12; i++)
                        {
                            drDet["Sueldo" + i] = "$" + consulta.p1.sueldomes.ToString("0,0.00", CultureInfo.InvariantCulture);
                        }
                        drDet["SueldoTotal"] = "$" + (consulta.p1.sueldomes * (12 - (Convert.ToInt32(consulta.p1.GeneradoPrimerAno) - 1))).ToString("0,0.00", CultureInfo.InvariantCulture);
                        drDet["PrestacionesTotal"] = "$" + 0.ToString("0,0.00", CultureInfo.InvariantCulture);
                        detalle.Rows.Add(drDet);
                    }

                    gw_Anexos2.DataSource = datos;
                    gw_Anexos2.DataBind();

                    gw_AnexosActividad2.DataSource = detalle;
                    gw_AnexosActividad2.DataBind();
                    PanelManodeObraDirecta.Visible = true;
                }
            }
            catch (Exception)
            { }
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
            EncabezadoEval.GetUltimaActualizacion();
        }
    }
}