using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera
{
    public partial class CatalogoIndicador : Negocio.Base_Page
    {
        public string Accion {
            get {
                return Request.QueryString["accion"].ToString();
            } set { }
        }
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public Int64 CodigoIndicador {
            get {
                return Convert.ToInt64(Request.QueryString["codindicador"]);
            }
            set { }
        }

        //Pestaña actual        
        public int txtTab = Constantes.Const_IndicadoresFinancierosEvaluacionV2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbltitle0.Text = DateTime.Now.ToShortDateString();
                CargarInformacion();
            }

        }

        /// <summary>
        /// Cargar la información del indicador seleccionado.
        /// </summary>
        public void CargarInformacion()
        {
            try
            {
                if (Accion == "Crear")
                {
                    btnCrearOrUpdate.Text = "Crear";
                    lbltitle.Text = "Crear Indicador";
                    LimpiarCampos();
                }
                else if (Accion == "Modificar")
                {
                    btnCrearOrUpdate.Text = "Modificar";
                    lbltitle.Text = "Modificar Indicador";
                    Int64 codigoIndicador = Convert.ToInt64(CodigoIndicador);

                    getIndicador(codigoIndicador);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Metodo para obtener los datos del indicador a actualizar.
        /// </summary>
        private void getIndicador(Int64 _codigoIndicador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                EvaluacionProyectoIndicador indicador = db.EvaluacionProyectoIndicadors.Single(ById => ById.id_Indicador == _codigoIndicador);

                txtDetalle.Text = indicador.Descripcion;
                txtDetalle.Enabled = !indicador.Protegido;
                txtValor.Text = indicador.Valor.ToString("0,0.0000", CultureInfo.InvariantCulture);                
                cmbTipo.SelectedValue = indicador.Tipo.ToString();
                cmbTipo.Enabled = !indicador.Protegido;

            }
        }

        /// <summary>
        /// Metodo para Crear o Actualizar un nuevo Indicador.
        /// </summary>
        public void AccionGuardarOrUpdate()
        {
            Int64 codigoIndicador = Convert.ToInt64(CodigoIndicador);
            string descripcionIndicador = txtDetalle.Text;
            double valorIndicador = double.Parse(txtValor.Text.Replace(",", "").Replace(".", ","));
            char tipoIndicador = char.Parse(cmbTipo.SelectedValue);
            bool protegido = false;

            if (Accion == "Crear")
            {
                InsertarIndicador(descripcionIndicador, valorIndicador, tipoIndicador, protegido);
            }
            else if (Accion == "Modificar")
            {
                ActualizarIndicador(codigoIndicador, descripcionIndicador, valorIndicador, tipoIndicador, protegido);
            }
        }

        /// <summary>
        /// Metodo para insertar un nuevo indicador
        /// </summary>
        /// <param name="_descripcionIndicador"></param>
        /// <param name="_valorIndicador"></param>
        /// <param name="_tipoIndicador"></param>
        /// <param name="_protegido"></param>
        private void InsertarIndicador(string _descripcionIndicador, double _valorIndicador, char _tipoIndicador, bool _protegido)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (db.EvaluacionProyectoIndicadors.Where(registro => registro.codProyecto == CodigoProyecto && registro.codConvocatoria == CodigoConvocatoria && registro.Descripcion.Contains(_descripcionIndicador)).Any())
                    throw new ApplicationException("Existe un indicador ese mismo nombre.");

                EvaluacionProyectoIndicador nuevoIndicador = new EvaluacionProyectoIndicador()
                {
                    codProyecto = CodigoProyecto,
                    codConvocatoria = CodigoConvocatoria,
                    Descripcion = _descripcionIndicador,
                    Valor = _valorIndicador,
                    Tipo = _tipoIndicador,
                    Protegido = _protegido
                };

                db.EvaluacionProyectoIndicadors.InsertOnSubmit(nuevoIndicador);
                db.SubmitChanges();

                actualizarFechaActualizacionTab();
                
                LimpiarCampos();
            }
        }

        /// <summary>
        /// Actualizar la fecha de actualización de la tab actual.
        /// </summary>
        private void actualizarFechaActualizacionTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)txtTab,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };

            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
            Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");
        }

        /// <summary>
        /// Metodo para actualizar el indicador
        /// </summary>
        /// <param name="_codigoIndicador"></param>
        /// <param name="_descripcionIndicador"></param>
        /// <param name="_valorIndicador"></param>
        /// <param name="_tipoIndicador"></param>
        /// <param name="_protegido"></param>
        public void ActualizarIndicador(Int64 _codigoIndicador, string _descripcionIndicador, double _valorIndicador, char _tipoIndicador, bool _protegido)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                EvaluacionProyectoIndicador indicador = db.EvaluacionProyectoIndicadors.Single(ById => ById.id_Indicador == _codigoIndicador);

                if (db.EvaluacionProyectoIndicadors.Where(registro => registro.id_Indicador != _codigoIndicador && registro.codProyecto == CodigoProyecto && registro.codConvocatoria == CodigoConvocatoria && registro.Descripcion.Contains(_descripcionIndicador) && registro.Protegido.Equals(false)).Any())
                    throw new ApplicationException("Existe un indicador ese mismo nombre.");

                //Actualizamos el indicador

                indicador.Valor = _valorIndicador;

                //Si esta protegido no se puede actualizar el tipo y descripción.
                if (!indicador.Protegido)
                {
                    indicador.Descripcion = _descripcionIndicador;
                    indicador.Tipo = _tipoIndicador;
                }

                db.SubmitChanges();

                actualizarFechaActualizacionTab();
            }
        }

        /// <summary>
        /// Limpia campos y variables de sesión asociadas al proceso.
        /// </summary>
        public void LimpiarCampos()
        {
            txtDetalle.Text = "";
            txtValor.Text = "";
            lbltitle0.Text = "";         
        }
       
        protected void EventoClickCreateOrUpdate(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();
                AccionGuardarOrUpdate();

                Response.Redirect("EvaluacionIndicadoresFinancieros.aspx?codproyecto="+CodigoProyecto);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Valida los campos de los formularios
        /// </summary>
        /// <returns> ApplicationException si encuentra error. </returns>
        private void ValidarCampos()
        {
            if (String.IsNullOrEmpty(txtDetalle.Text) || txtDetalle.Text.Length > 255)
                throw new ApplicationException(" El detalle no puede estar vacio y debe tener menos de 255 caracteres.");

            Int64 numero;
            if (!Int64.TryParse(txtValor.Text.Replace(",", "").Replace(".", ""), out numero))
                throw new ApplicationException(" El valor debe ser numerico.");
        }

        /// <summary>
        /// Metodo para dirigir a la pagina de indicadores y limpia campos y variables asociadas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EventCancelar(object sender, EventArgs e)
        {
            LimpiarCampos();
            Response.Redirect("EvaluacionIndicadoresFinancieros.aspx?codproyecto="+CodigoProyecto);
        }
    }
}