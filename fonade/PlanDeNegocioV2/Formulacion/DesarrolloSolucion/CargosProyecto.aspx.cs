using Datos;
using Fonade.Account;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    public partial class CargosProyecto : System.Web.UI.Page
    {
        #region Variables

        public int IdCargo
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdCargo") ? int.Parse(Request.QueryString["IdCargo"].ToString()) : 0;
            }
        }

        public bool EsEdicion
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("EsEdicion") ? bool.Parse(Request.QueryString["EsEdicion"].ToString()) : false;
            }
        }

        public int CodigoProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("CodigoProyecto") ? int.Parse(Request.QueryString["CodigoProyecto"].ToString()) : 0;
            }
        }

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso6ProductividadEquipoDeTrabajo; } }

        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack && IdCargo > 0)
                {
                    CargarFormulario();

                    
                }

                
                ViewState["PararEvento"] = false;

                SetMaxLength();
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarTiempoVinculacion())
            {

                bool esnuevo = IdCargo == 0 ;

                //Se valida si existe un cargo con el mismo nombre
                if (!Productividad.existeCargo(IdCargo, txtNomCargo.Text.Trim(), CodigoProyecto))
                {
                    if (Convert.ToDecimal(txtVlrFondoEmprender.Text.Replace(",", "").Replace(".", ",")) <= 0 &&
                        Convert.ToDecimal(txtVlrIngresosVentas.Text.Replace(",", "").Replace(".", ",")) <= 0 &&
                        Convert.ToDecimal(txtVlrAportesEmprendedor.Text.Replace(",", "").Replace(".", ",")) <= 0)
                    {
                        Utilidades.PresentarMsj(Mensajes.GetMensaje(156), this, "Alert");
                    }
                    else
                    {
                        ProyectoGastosPersonal item = new ProyectoGastosPersonal()
                        {
                            AportesEmprendedor = Convert.ToDecimal(txtVlrAportesEmprendedor.Text.Replace(",", "").Replace(".", ",")),
                            Cargo = txtNomCargo.Text.Trim(),
                            CodProyecto = CodigoProyecto,
                            Dedicacion = ddlDedicacion.SelectedItem.Text.Trim(),
                            ExperienciaEspecifica = txtExpEspecifica.Text.Trim(),
                            ExperienciaGeneral = txtExpGeneral.Text.Trim(),
                            Formacion = txtFormacion.Text.Trim(),
                            Funciones = txtFunciones.Text.Trim(),
                            IngresosVentas = Convert.ToDecimal(txtVlrIngresosVentas.Text.Replace(",", "").Replace(".", ",")),
                            OtrosGastos = Convert.ToDecimal(txtVlrOtros.Text.Replace(",", "").Replace(".", ",")),
                            TiempoVinculacion = Convert.ToInt32(txtTiempoVinculacion.Text.Trim()),
                            TipoContratacion = ddlTipoContrato.SelectedItem.Text.Trim(),
                            UnidadTiempo = Convert.ToInt32(ddlUnidadTiempo.SelectedValue),
                            ValorAnual = 0,
                            ValorFondoEmprender = Convert.ToDecimal(txtVlrFondoEmprender.Text.Replace(",", "").Replace(".", ",")),
                            ValorMensual = 0,
                            ValorRemuneracion = Convert.ToDecimal(txtVlrRemunUnitario.Text.Replace(",", "").Replace(".", ",")),
                        };

                        if (!esnuevo)
                        {
                            item.Id_Cargo = IdCargo;
                        }

                        if (!Productividad.setCargo(item, esnuevo))
                        {
                            Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
                        }
                        else
                        {
                            //actualizar la grilla de la pagina principal
                            Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "upd", "window.opener.__doPostBack('', 'updGrilla');", true);
                            ClientScript.RegisterStartupScript(this.GetType(), "Close", "<script>window.close();</script> ");
                        }
                    }
                }
                else
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(5), this, "Alert");
                }
            }
        }

        protected void txtCalculos_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;

                IniciarCalculos();

                if (!(bool)ViewState["PararEvento"])
                {

                    switch (txt.ID)
                    {
                        case "txtTiempoVinculacion":
                            Page.SetFocus(txtVlrRemunUnitario);
                            break;
                        case "txtVlrRemunUnitario":
                            Page.SetFocus(txtVlrOtros);
                            break;
                        default:
                            Page.SetFocus(txtVlrFondoEmprender);
                            break;
                    }

                    ViewState["PararEvento"] = true;

                }


            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga el formulario con los datos existentes o vacío si es nuevo
        /// </summary>
        private void CargarFormulario()
        {
            ProyectoGastosPersonal formulario = Productividad.getCargo(IdCargo);

            txtExpEspecifica.Text = formulario.ExperienciaEspecifica;
            txtExpGeneral.Text = formulario.ExperienciaGeneral;
            txtFormacion.Text = formulario.Formacion;
            txtFunciones.Text = formulario.Funciones;
            txtNomCargo.Text = formulario.Cargo;
            txtTiempoVinculacion.Text = formulario.TiempoVinculacion.ToString();
            txtVlrAportesEmprendedor.Text = formulario.AportesEmprendedor.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtVlrFondoEmprender.Text = formulario.ValorFondoEmprender.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtVlrIngresosVentas.Text = formulario.IngresosVentas.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtVlrOtros.Text = formulario.OtrosGastos.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtVlrRemunUnitario.Text = formulario.ValorRemuneracion.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
            CalcularValores(formulario.ValorRemuneracion.Value, formulario.OtrosGastos, formulario.TiempoVinculacion);
            ddlDedicacion.SelectedValue = formulario.Dedicacion.Equals("Completo") ? Constantes.CONST_DedicacionCompleta.ToString() : Constantes.CONST_DedicacionParcial.ToString() ;

            switch (formulario.TipoContratacion)
            {
                case "Jornal":
                    ddlTipoContrato.SelectedValue = Constantes.CONST_ContratoJornal.ToString();
                    break;
                case "Nómina":
                    ddlTipoContrato.SelectedValue = Constantes.CONST_ContratoNomina.ToString();
                    break;
                default:
                    ddlTipoContrato.SelectedValue = Constantes.CONST_ContratoPrestacion.ToString();
                    break;
            }

            
            ddlUnidadTiempo.SelectedValue = formulario.UnidadTiempo.Value.ToString();
            
        }

        /// <summary>
        /// Calcula el valor prestaciones y de remuneración del primera año
        /// </summary>
        /// <param name="vlrRemunUni">Valor remuneración unitario</param>
        /// <param name="vlrOtrosGastos">Valor otros gastos</param>
        /// <param name="tiempoVinculacion">Tiempo de vinculación</param>
        private void CalcularValores(decimal? vlrRemunUni, decimal? vlrOtrosGastos, int? tiempoVinculacion)
        {
            decimal? valPrestaciones = vlrRemunUni != null ? vlrRemunUni + vlrOtrosGastos : 0;
            decimal? valRemunUltAnio = tiempoVinculacion != null ? valPrestaciones * tiempoVinculacion : 0;

            txtVlrPrestaciones.Text = valPrestaciones.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtVlrRemunPrimerAnio.Text = valRemunUltAnio.Value.ToString("0,0.00", CultureInfo.InvariantCulture);

        }

        /// <summary>
        /// Valida el tiempo de vinculación en relación con la unidad de tiempo seleccionada
        /// </summary>
        /// <returns>Verdadero si la validación es exitosa. Falso en otro caso</returns>
        private bool ValidarTiempoVinculacion()
        {
            int vlr = 0;

            //Se valida la selección en la unidad de tiempo
            if(ddlUnidadTiempo.SelectedValue.Trim() == "")
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(150), this, "Alert");
                Page.SetFocus(ddlUnidadTiempo);
                return false;
            }


            //Se valida que se haya ingresado un valor numérico en el tiempo de vinculación
            if(!int.TryParse(txtTiempoVinculacion.Text.Trim(),out vlr))
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(145), this, "Alert");
                return false;
            }

            //Se trae los valores de unidad seleccionada y el tiempo de vinculación
            int unidadsel = Convert.ToInt32(ddlUnidadTiempo.SelectedValue);
            int tiemposel = txtTiempoVinculacion.Text.Trim().Count() > 0 ? Convert.ToInt32(txtTiempoVinculacion.Text.Trim()) : 0;

            //Si se seleccionó mes se valida que el tiempo no sea menor a 1 mes y mayor a 12 meses
            if(unidadsel == Constantes.CONST_UniTiempoMes)
            {
                if(tiemposel < 1 || tiemposel > 12)
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(143), this, "Alert");
                    return false;
                }
            }
            //Si se seleccionó días se valida que el tiempo no sea menor a 1 y mayor a 365 días
            else if(unidadsel == Constantes.CONST_UniTiempoDias)
            {
                if (tiemposel < 1 || tiemposel > 365)
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(144), this, "Alert");
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Realiza los calculos de los valores calculados
        /// </summary>
        private void IniciarCalculos()
        {
            if (ValidarTiempoVinculacion())
            {
                decimal? vlrRemunUni = txtVlrRemunUnitario.Text.Trim().Count() > 0 ? Convert.ToDecimal(txtVlrRemunUnitario.Text.Replace(",", "").Replace(".", ",")) : 0;
                decimal? vlrOtrosGastos = txtVlrOtros.Text.Trim().Count() > 0 ? Convert.ToDecimal(txtVlrOtros.Text.Replace(",", "").Replace(".", ",")) : 0;
                int? tiempoVin = txtTiempoVinculacion.Text.Trim().Count() > 0 ? Convert.ToInt32(txtTiempoVinculacion.Text.Trim()) : 0;

                CalcularValores(vlrRemunUni, vlrOtrosGastos, tiempoVin);
            }
        }

        /// <summary>
        /// Configura los tamaños máximos de los campos multiline
        /// </summary>
        void SetMaxLength()
        {
            string max = "250";
            txtFormacion.Attributes.Add("maxlength", max);
            txtFunciones.Attributes.Add("maxlength", max);
        }

        #endregion

    }
}