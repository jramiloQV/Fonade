using Datos;
using Datos.DataType;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionProductividad : System.Web.UI.UserControl
    {
        #region Variables

        public List<Datos.DataType.EquipoTrabajo> ListEmprendedores { get; set; }

        public List<ProyectoGastosPersonal> ListCargos { get; set; }

        public ProyectoProductividad Formulario { get; set; }

        /// <summary>
        /// Total solicitado fondo emprender
        /// </summary>
        public string TotalFondoEmprender { get; set; }

        /// <summary>
        /// Total aportes emprendedor
        /// </summary>
        public string TotalAportesEmprendedor { get; set; }

        /// <summary>
        /// Total ingresos por ventas
        /// </summary>
        public string TotalIngresosVentas { get; set; }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Enlazar();
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Enlaza los datos consultados a los diferentes controles
        /// </summary>
        public void Enlazar()
        {
            if (Formulario == null)
            {
                Formulario = new ProyectoProductividad();
            }

            lblPregunta16.Text = Formulario.CapacidadProductiva;

            CargarEmprendedores();

            CargarCargos();

        }

        /// <summary>
        /// Carga el listado de emprendedores y sus perfiles
        /// </summary>
        private void CargarEmprendedores()
        {
            gw_pregunta171.DataSource = ListEmprendedores;
            gw_pregunta171.DataBind();
        }

        /// <summary>
        /// Carga el listado de emprendedores y sus perfiles
        /// </summary>
        private void CargarCargos()
        {
            List<CargosPlanNegocio> lst = null;

            if (ListCargos != null)
            {
                lst = ListCargos.Select(
                       x => new CargosPlanNegocio()
                       {
                           AportesEmprendedorCadena = x.AportesEmprendedor.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                           IngresosVentasCadena = x.IngresosVentas.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                           OtrosGastosCadena = x.OtrosGastos.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                           Cargo = x.Cargo,
                           TiempoVinculacion = x.TiempoVinculacion,
                           UnidadTiempo = x.UnidadTiempo == Constantes.CONST_UniTiempoMes ? "Mes" : "Días",
                           Id_Cargo = x.Id_Cargo,
                           ValorFondoEmprenderCadena = x.ValorFondoEmprender.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                           ValorRemunCadena = x.ValorRemuneracion.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                           ValorPrestacionesCadena = (x.ValorRemuneracion + x.OtrosGastos).Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                           ValorRemunPrimerAnioCadena = ((x.ValorRemuneracion + x.OtrosGastos) * x.TiempoVinculacion).Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture)
                       }
                    ).ToList();

                //Se calculan los totales

                TotalAportesEmprendedor = ListCargos.Sum(x => x.AportesEmprendedor).Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                TotalFondoEmprender = ListCargos.Sum(x => x.ValorFondoEmprender).Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                TotalIngresosVentas = ListCargos.Sum(x => x.IngresosVentas).Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture);

                gwPregunta172A.DataSource = lst;
                gwPregunta172A.DataBind();

                gwPregunta172B.DataSource = lst;
                gwPregunta172B.DataBind();
            }
            else
            {
                lst = new List<CargosPlanNegocio>();

                gwPregunta172A.DataSource = lst;
                gwPregunta172A.DataBind();
            }


            
        }

        #endregion
    }
}