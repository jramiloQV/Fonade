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
    public partial class ImpresionRequerimientos : System.Web.UI.UserControl
    {
        #region Variables

        public List<RequerimientosNeg> ListRequerimientos { get; set; }

        public ProyectoRequerimiento Formulario { get; set; }

        /// <summary>
        /// Total requerimientos Infraestructura
        /// </summary>
        public string TotalG1421 { get; set; }

        /// <summary>
        /// Total requerimientos Maquinaria y Equipo
        /// </summary>
        public string TotalG1422 { get; set; }

        /// <summary>
        /// Total requerimientos Equipo de Comunicación y Computación
        /// </summary>
        public string TotalG1423 { get; set; }

        /// <summary>
        /// Total requerimientos Muebles, Enseres y Otros
        /// </summary>
        public string TotalG1424 { get; set; }

        /// <summary>
        /// Total requerimientos Otros
        /// </summary>
        public string TotalG1425 { get; set; }

        /// <summary>
        /// Total requerimientos Gastos Preoperativos
        /// </summary>
        public string TotalG1426 { get; set; }

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
                Formulario = new ProyectoRequerimiento();
            }
            if (Formulario.IdProyecto != 0)
            {
                lblSel141.Text = Formulario.TieneLugarFisico ? "SI" : "NO";
            }

            lblPregunta141.Text = Formulario.LugarFisico;

            CargarRequerimientos();



        }

        /// <summary>
        /// Carga los requerimientos de un plan de negocio
        /// </summary>
        private void CargarRequerimientos()
        {
            GridView[] listagrillas = (pnltabRequerimiento.Controls
                                        .OfType<GridView>()).ToArray();

            foreach (GridView item in listagrillas)
            {
                EnlazarGrillas(item);

            }
        }

        /// <summary>
        /// Enlaza los datos a la grilla seleccionada
        /// </summary>
        /// <param name="gv">Grilla a enlazar</param>
        private void EnlazarGrillas(GridView gv)
        {
            gv.DataSource = SepararInfrestructura(gv.ID);
            gv.DataBind();
        }

        /// <summary>
        /// De acuerdo al tipo de infraestructura retorna el listado de requerimientos
        /// </summary>
        /// <param name="nombreGrilla">Nombre de la grilla a presentar</param>
        /// <returns></returns>
        private List<RequerimientosNeg> SepararInfrestructura(string nombreGrilla)
        {
            List<RequerimientosNeg> lst = null;

            if (ListRequerimientos == null)
            {
                lst = new List<RequerimientosNeg>();
            }
            else
            {

                switch (nombreGrilla)
                {
                    case "gwPregunta1421":
                        lst = (ListRequerimientos.Where(
                              reg => reg.CodTipoInfraestructura == Constantes.CONST_Infraestructura_Adecuaciones)
                              .Select(y => new RequerimientosNeg
                              {
                                  IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                                  Cantidad = y.Cantidad,
                                  CodTipoInfraestructura = y.CodTipoInfraestructura,
                                  FuenteFinanciacion = y.FuenteFinanciacion,
                                  IdFuente = y.IdFuente,
                                  NomInfraestructura = y.NomInfraestructura,
                                  RequisitosTecnicos = y.RequisitosTecnicos,
                                  ValorUnidadCadena = y.ValorUnidad.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                                  ValorUnidad = y.ValorUnidad

                              })).ToList();

                        TotalG1421 = lst.Select(x => ((float)x.Cantidad * (float)x.ValorUnidad)).Sum().ToString("$ 0,0.00", CultureInfo.InvariantCulture);

                        break;
                    case "gwPregunta1422":
                        lst = (ListRequerimientos.Where(
                              reg => reg.CodTipoInfraestructura == Constantes.CONST_MaquinariayEquipo)
                              .Select(y => new RequerimientosNeg
                              {
                                  IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                                  Cantidad = y.Cantidad,
                                  CodTipoInfraestructura = y.CodTipoInfraestructura,
                                  FuenteFinanciacion = y.FuenteFinanciacion,
                                  IdFuente = y.IdFuente,
                                  NomInfraestructura = y.NomInfraestructura,
                                  RequisitosTecnicos = y.RequisitosTecnicos,
                                  ValorUnidadCadena = y.ValorUnidad.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                                  ValorUnidad = y.ValorUnidad

                              })).ToList();

                        TotalG1422 = lst.Select(x => ((float)x.Cantidad * (float)x.ValorUnidad)).Sum().ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        break;
                    case "gwPregunta1423":
                        lst = (ListRequerimientos.Where(
                              reg => reg.CodTipoInfraestructura == Constantes.CONST_EquiposComuniCompu)
                              .Select(y => new RequerimientosNeg
                              {
                                  IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                                  Cantidad = y.Cantidad,
                                  CodTipoInfraestructura = y.CodTipoInfraestructura,
                                  FuenteFinanciacion = y.FuenteFinanciacion,
                                  IdFuente = y.IdFuente,
                                  NomInfraestructura = y.NomInfraestructura,
                                  RequisitosTecnicos = y.RequisitosTecnicos,
                                  ValorUnidadCadena = y.ValorUnidad.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                                  ValorUnidad = y.ValorUnidad

                              })).ToList();

                        TotalG1423 = lst.Select(x => ((float)x.Cantidad * (float)x.ValorUnidad)).Sum().ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        break;
                    case "gwPregunta1424":
                        lst = (ListRequerimientos.Where(
                              reg => reg.CodTipoInfraestructura == Constantes.CONST_MueblesEnseresOtros)
                              .Select(y => new RequerimientosNeg
                              {
                                  IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                                  Cantidad = y.Cantidad,
                                  CodTipoInfraestructura = y.CodTipoInfraestructura,
                                  FuenteFinanciacion = y.FuenteFinanciacion,
                                  IdFuente = y.IdFuente,
                                  NomInfraestructura = y.NomInfraestructura,
                                  RequisitosTecnicos = y.RequisitosTecnicos,
                                  ValorUnidadCadena = y.ValorUnidad.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                                  ValorUnidad = y.ValorUnidad

                              })).ToList();

                        TotalG1424 = lst.Select(x => ((float)x.Cantidad * (float)x.ValorUnidad)).Sum().ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        break;
                    case "gwPregunta1425":
                        lst = (ListRequerimientos.Where(
                              reg => reg.CodTipoInfraestructura == Constantes.CONST_Otros)
                              .Select(y => new RequerimientosNeg
                              {
                                  IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                                  Cantidad = y.Cantidad,
                                  CodTipoInfraestructura = y.CodTipoInfraestructura,
                                  FuenteFinanciacion = y.FuenteFinanciacion,
                                  IdFuente = y.IdFuente,
                                  NomInfraestructura = y.NomInfraestructura,
                                  RequisitosTecnicos = y.RequisitosTecnicos,
                                  ValorUnidadCadena = y.ValorUnidad.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                                  ValorUnidad = y.ValorUnidad

                              })).ToList();

                        TotalG1425 = lst.Select(x => ((float)x.Cantidad * (float)x.ValorUnidad)).Sum().ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        break;
                    default:
                        lst = (ListRequerimientos.Where(
                              reg => reg.CodTipoInfraestructura == Constantes.CONST_GastoPreoperativos)
                              .Select(y => new RequerimientosNeg
                              {
                                  IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                                  Cantidad = y.Cantidad,
                                  CodTipoInfraestructura = y.CodTipoInfraestructura,
                                  FuenteFinanciacion = y.FuenteFinanciacion,
                                  IdFuente = y.IdFuente,
                                  NomInfraestructura = y.NomInfraestructura,
                                  RequisitosTecnicos = y.RequisitosTecnicos,
                                  ValorUnidadCadena = y.ValorUnidad.Value.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                                  ValorUnidad = y.ValorUnidad

                              })).ToList();

                        TotalG1426 = lst.Select(x => ((float)x.Cantidad * (float)x.ValorUnidad)).Sum().ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        break;
                }
            }

            return lst;
        }

        #endregion
    }
}