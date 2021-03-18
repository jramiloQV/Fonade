using Datos;
using Fonade.Account;
using Fonade.Negocio;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionMetasSociales : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        static string cadenaConexion
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }

        /// <summary>
        /// Usuario logueado
        /// </summary>
        protected FonadeUser usuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                usuario = HttpContext.Current.Session["usuarioLogged"] != null
                    ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"]
                    : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);

                CargarTextArea();
                CargarGridEmpleos();
                CargarGridEmprendedores();
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
            
        }

        protected void CargarTextArea()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                txtPlanRegional.Text = string.Empty;
                txtPlanNacional.Text = string.Empty;
                txtCluster.Text = string.Empty;
                txtEmpleosIndirectos.Text = string.Empty;

                var query = (from p in consultas.ProyectoMetaSocials
                             where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             select p).FirstOrDefault();

                if (query != null)
                {
                    txtPlanRegional.Text = query.PlanRegional;
                    txtPlanNacional.Text = query.PlanNacional;
                    txtCluster.Text = query.Cluster;

                    var sumatoriaEmpleos = consultas.ProyectoMetaSocials.Where(m => m.CodProyecto == Convert.ToInt32(CodigoProyecto) && m.EmpleoIndirecto != null).Count() > 0 ? consultas.ProyectoMetaSocials.Where(m => m.CodProyecto == Convert.ToInt32(CodigoProyecto)).Sum(i => i.EmpleoIndirecto).Value : 0;
                    txtEmpleosIndirectos.Text = sumatoriaEmpleos.ToString();
                }
            }
        }

        private void CargarGridEmpleos()
        {

            DataTable respuesta = new DataTable();
            DataTable respuesta2 = new DataTable();

            Datos.Consultas consultas = new Consultas();

            string consulta =
            " select cast(id_cargo as int) as IdCargo, cast(cargo as varchar(100)) as Cargo, valormensual as ValorMensual, ";
            consulta += " cast(GeneradoPrimerAno as varchar) as GeneradoPrimerAnio, Joven as EsJoven, Desplazado as EsDesplazado, Madre as EsMadre, ";
            consulta += " Minoria as EsMinoria, Recluido as EsRecluido, Desmovilizado as EsDesmovilizado, Discapacitado as EsDiscapacitado,  Desvinculado as EsDesvinculado ";
            consulta += " from  proyectoempleocargo right OUTER JOIN proyectogastospersonal ";
            consulta += "on id_cargo=codcargo where codproyecto= " + CodigoProyecto;

            respuesta =  consultas.ObtenerDataTable(consulta, "text");

            string consulta2 = "select " +
                            " cast(id_insumo as int) as IdCargo, cast(nominsumo as varchar(100)) as Cargo, Convert(Numeric, sueldomes) as ValorMensual, cast(GeneradoPrimerAno as varchar)  as GeneradoPrimerAnio, Joven  as EsJoven, " +
                            " Desplazado as EsDesplazado, Madre as EsMadre, Minoria as EsMinoria,Recluido as EsRecluido, Desmovilizado as EsDesmovilizado, " +
                            " Discapacitado as EsDiscapacitado, Desvinculado as EsDesvinculado " +
                            " from proyectoinsumo " +
                            " inner join ProyectoProductoInsumo on id_Insumo = CodInsumo " +
                            " left join proyectoempleomanoobra on id_Insumo = CodManoObra " +
                            " where codTipoInsumo = 2 and CodProyecto = " + CodigoProyecto;

            respuesta2 = consultas.ObtenerDataTable(consulta2, "text");

            //Se crea nueva funcionalidad para enumerar cantidad de empleos a generar
            string consulta3 = "select * from ProyectoMetaSocial where codproyecto=" + CodigoProyecto;

            var empleosGenerar = consultas.ObtenerDataTable(consulta3, "text");

            var kjh = empleosGenerar.Compute("SUM([EmpleoIndirecto])", string.Format("[CodProyecto]={0}", CodigoProyecto)) ?? 0;

            primer_ano.Text = empleosGenerar.Rows.Count.ToString();

            Total_empleos.Text = string.IsNullOrEmpty(kjh.ToString()) ? "0" : kjh.ToString();
            var qry = "select (select COUNT(*) from proyectoinsumo inner join ProyectoProductoInsumo on CodInsumo = Id_Insumo LEFT OUTER JOIN" + " proyectoempleomanoobra  on id_insumo=codmanoobra where codtipoinsumo=2 and codproyecto={0}) + " +
              "(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal  on id_cargo=codcargo where codproyecto={0}) as Conteototal, " +
"(select COUNT(*) from proyectoinsumo  inner join ProyectoProductoInsumo  on CodInsumo = Id_Insumo LEFT OUTER JOIN proyectoempleomanoobra on id_insumo=codmanoobra" + " where codtipoinsumo=2 and codproyecto={0} and GeneradoPrimerAno  is not null and GeneradoPrimerAno!=0) + " +
"(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal on id_cargo=codcargo  where codproyecto={0} and GeneradoPrimerAno is not null" + " and GeneradoPrimerAno!=0) as ConteoAño";
            var xct = new Clases.genericQueries().executeQueryReader(string.Format(qry, CodigoProyecto));
            DataTable dtEmpTtl = new DataTable();
            dtEmpTtl.Load(xct, LoadOption.OverwriteChanges);
            primer_ano.Text = dtEmpTtl.Rows[0]["ConteoAño"].ToString();
            Total_empleos.Text = dtEmpTtl.Rows[0]["Conteototal"].ToString();
            if (respuesta2.Rows.Count > 0)
            {
                Label_ManoObra.Visible = true;
            }


            gw_Empleos.DataSource = respuesta;
            gw_ManoObra.DataSource = respuesta2;
            gw_Empleos.DataBind();
            gw_ManoObra.DataBind();
        }

        private void CargarGridEmprendedores()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoContactos
                             from c in consultas.Contacto
                             where p.CodContacto == c.Id_Contacto &&
                              p.CodProyecto == Convert.ToInt32(CodigoProyecto) &&
                              p.CodRol == Constantes.CONST_RolEmprendedor &&
                              p.Inactivo == false
                             orderby c.Nombres, c.Apellidos ascending
                             select new
                             {
                                 c.Id_Contacto,
                                 nombres = c.Nombres + " " + c.Apellidos,
                                 Beneficiario = (p.Beneficiario != null) ? p.Beneficiario : false,
                                 Participacion = (p.Participacion != null) ? p.Participacion : 0
                             });

                gw_emprendedores.DataSource = query;
                gw_emprendedores.DataBind();

                for (int i = 0; i < gw_emprendedores.Rows.Count; i++)
                {

                    if (gw_emprendedores.DataKeys[i].Value.ToString() != usuario.IdContacto.ToString())
                    {
                        ((CheckBox)gw_emprendedores.Rows[i].FindControl("chkBeneficiario")).Checked = true;

                    }
                }
            }
        }
    }

    public class BORespuestaEmpleos
    {
        public int IdCargo { get; set; }
        public string Cargo { get; set; }
        public decimal ValorMensual { get; set; }
        public int GeneradoPrimerAnio { get; set; }
        public int EsJoven { get; set; }
        public int EsDesplazado { get; set; }
        public int EsMadre { get; set; }
        public int EsMinoria { get; set; }
        public int EsRecluido { get; set; }
        public int EsDesmovilizado { get; set; }
        public int EsDiscapacitado { get; set; }
        public int EsDesvinculado { get; set; }
    }
}