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
    public partial class PlanOperativo : Negocio.Base_Page
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
        public int CodigoTab { get { return Constantes.Const_PlanOperativoV2; } set { } }
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
                bRealizado = esRealizado(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());

                if (esMiembro && !bRealizado) { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }

                CargarGridActividades();
                UpdateTab();
            }
        }

        protected void CargarGridActividades()
        {

            var query = (from p in consultas.Db.ProyectoActividadPOs
                         where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                         orderby p.Item ascending
                         select new { p.Id_Actividad, p.Item, p.NomActividad });

            string consultaDetalle = "select id_actividad as CodActividad, mes,codtipofinanciacion,valor ";
            consultaDetalle += "from proyectoactividadpomes LEFT OUTER JOIN proyectoactividadPO ";
            consultaDetalle += "on id_actividad=codactividad where codproyecto={0}";
            consultaDetalle += " order by item, codactividad,mes,codtipofinanciacion";
            IEnumerable<ProyectoActividadPOMe> respuestaDetalle = consultas.Db.ExecuteQuery<ProyectoActividadPOMe>(consultaDetalle, Convert.ToInt32(CodigoProyecto));


            DataTable datos = new DataTable();
            DataTable detalle = new DataTable();
            datos.Columns.Add("CodProyecto");
            datos.Columns.Add("Id_Actividad");
            datos.Columns.Add("Item");
            datos.Columns.Add("Actividad");
            for (int i = 1; i <= 12; i++)
            {
                detalle.Columns.Add("fondo" + i);
                detalle.Columns.Add("emprendedor" + i);
            }
            detalle.Columns.Add("fondoTotal");
            detalle.Columns.Add("emprendedorTotal");

            foreach (var item in query)
            {
                DataRow dr = datos.NewRow();

                dr["CodProyecto"] = CodigoProyecto;
                dr["Id_Actividad"] = item.Id_Actividad;
                dr["Item"] = item.Item;
                dr["Actividad"] = item.NomActividad;
                datos.Rows.Add(dr);
            }
            int actividadActual = 0;
            DataRow drDet = detalle.NewRow();

            decimal totalFondo = 0;
            decimal totalEmprendedor = 0;

            foreach (ProyectoActividadPOMe registro in respuestaDetalle)
            {
                if (actividadActual != registro.CodActividad)
                {
                    if (actividadActual != 0)
                    {
                        drDet["fondoTotal"] = FieldValidate.moneyFormat(totalFondo);
                        drDet["emprendedorTotal"] = FieldValidate.moneyFormat(totalEmprendedor);
                        totalFondo = 0;
                        totalEmprendedor = 0;
                        detalle.Rows.Add(drDet);
                    }
                    drDet = detalle.NewRow();
                    actividadActual = registro.CodActividad;
                }

                if (registro.CodTipoFinanciacion == 1)
                {
                    drDet["fondo" + registro.Mes] = FieldValidate.moneyFormat(registro.Valor);
                    totalFondo += registro.Valor;
                }
                else if (registro.CodTipoFinanciacion == 2)
                {
                    drDet["emprendedor" + registro.Mes] = FieldValidate.moneyFormat(registro.Valor);
                    totalEmprendedor += registro.Valor;
                }
            }

            drDet["fondoTotal"] = FieldValidate.moneyFormat(totalFondo);
            drDet["emprendedorTotal"] = FieldValidate.moneyFormat(totalEmprendedor);
            detalle.Rows.Add(drDet);

            gw_Anexos.DataSource = datos;
            gw_Anexos.DataBind();

            gw_AnexosActividad.DataSource = detalle;
            gw_AnexosActividad.DataBind();

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