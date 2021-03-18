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

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// Clase para agregar o actualizar indicadores
    /// Modificación : Marcel Solera
    /// Ultima modificación : Mejora de rendimiento de aplicación.
    /// </summary>
    public partial class CatalogoIndicador : Negocio.Base_Page
    {
        public string Accion;

        /// <summary>
        /// Gets or sets the codproyecto.
        /// </summary>
        /// <value>
        /// The codproyecto.
        /// </value>
        public int Codproyecto
        {
            get { return Convert.ToInt32(ViewState["proyecto"].ToString()); }
            set { ViewState["proyecto"] = value; }
        }

        /// <summary>
        /// Gets or sets the codconvocatoria.
        /// </summary>
        /// <value>
        /// The codconvocatoria.
        /// </value>
        public int Codconvocatoria
        {
            get { return Convert.ToInt32(ViewState["convocatoria"].ToString()); }
            set { ViewState["convocatoria"] = value; }
        }
        public string CodIndicador;

        //Pestaña actual
        Int32 pestanaActual = Constantes.ConstSubIndicadoresFinancieros;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Accion = HttpContext.Current.Session["AccionAporteEvaluacion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["AccionAporteEvaluacion"].ToString()) ? HttpContext.Current.Session["AccionAporteEvaluacion"].ToString() : "Crear";
            Codproyecto = Convert.ToInt32(HttpContext.Current.Session["Codproyecto"].ToString());
            Codconvocatoria = Convert.ToInt32(HttpContext.Current.Session["Codconvocatoria"].ToString());
            
            CodIndicador = HttpContext.Current.Session["CodAporte"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodAporte"].ToString()) ? HttpContext.Current.Session["CodAporte"].ToString() : "0";

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
                    Int64 codigoIndicador = Convert.ToInt64(CodIndicador);

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
                txtValor.Text = indicador.Valor.ToString();
                cmbTipo.SelectedValue = indicador.Tipo.ToString();
                cmbTipo.Enabled = !indicador.Protegido;

            }
        }

        /// <summary>
        /// Metodo para Crear o Actualizar un nuevo Indicador.
        /// </summary>
        public void AccionGuardarOrUpdate()
        {                      
            Int64 codigoIndicador = Convert.ToInt64(CodIndicador);
            string descripcionIndicador = txtDetalle.Text;                                
            double valorIndicador = double.Parse(txtValor.Text.Replace(",", "").Replace(".", ","));
            char tipoIndicador = char.Parse(cmbTipo.SelectedValue);
            bool protegido = false;

            if (Accion == "Crear")
            {
                InsertarIndicador(descripcionIndicador,valorIndicador,tipoIndicador,protegido);
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
                if (db.EvaluacionProyectoIndicadors.Where(registro => registro.codProyecto == Codproyecto && registro.codConvocatoria == Codconvocatoria && registro.Descripcion.Contains(_descripcionIndicador)).Any())
                    throw new ApplicationException("Existe un indicador ese mismo nombre.");

                EvaluacionProyectoIndicador nuevoIndicador = new EvaluacionProyectoIndicador()
                {
                    codProyecto = Codproyecto,
                    codConvocatoria = Codconvocatoria,
                    Descripcion = _descripcionIndicador,
                    Valor = _valorIndicador,
                    Tipo = _tipoIndicador,
                    Protegido = _protegido
                };

                db.EvaluacionProyectoIndicadors.InsertOnSubmit(nuevoIndicador);
                db.SubmitChanges();

                actualizarFechaActualizacionTab();
                //Limpiar campos
                LimpiarCampos();
            }   
        }

        /// <summary>
        /// Actualizar la fecha de actualización de la tab actual.
        /// </summary>
        private void actualizarFechaActualizacionTab() 
        {            
            prActualizarTabEval(pestanaActual.ToString(), Codproyecto.ToString(), Codconvocatoria.ToString());
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

                if (db.EvaluacionProyectoIndicadors.Where(registro => registro.id_Indicador != _codigoIndicador && registro.codProyecto == Codproyecto && registro.codConvocatoria == Codconvocatoria && registro.Descripcion.Contains(_descripcionIndicador) && registro.Protegido.Equals(false)).Any())
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

            //Limpiar variables de sesión.
            HttpContext.Current.Session["CodAporte"] = null;
            HttpContext.Current.Session["AccionAporteEvaluacion"] = null;
        }

        /// <summary>
        /// Evento click boton de crear o editar indicador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EventoClickCreateOrUpdate(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();
                AccionGuardarOrUpdate();

                Response.Redirect("EvaluacionIndicadoresFinancieros.aspx");
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
                throw new ApplicationException(" El detalle no puede estar vacío y debe tener menos de 255 caracteres.");

            Int64 numero;
            if(!Int64.TryParse(txtValor.Text.Replace(",","").Replace(".",""), out numero))
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
            Response.Redirect("EvaluacionIndicadoresFinancieros.aspx");
        }
        
    }
}