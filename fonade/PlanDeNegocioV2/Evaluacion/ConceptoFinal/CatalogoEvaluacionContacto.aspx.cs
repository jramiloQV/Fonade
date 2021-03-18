using System;
using System.Linq;
using System.Web.UI;
using Datos;
using System.Web;
using Fonade.Account;
using System.Web.Security;

namespace Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public partial class CatalogoEvaluacionContacto : System.Web.UI.Page
    {
        private Consultas _consultas;
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
        public int CodigoTab { get { return Constantes.Const_AportesV2; } set { } }

        public int CodigoContacto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codContacto"]);
            }
            set { }
        }
        public FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        public void CargarDatos()
        {
            _consultas = new Consultas();

            // cargamos la informacion de la evaluacion del contacto
            var evalucacion = _consultas.Db.EvaluacionContactos.FirstOrDefault(ec => ec.CodProyecto == CodigoProyecto
                                                                            && ec.CodConvocatoria == CodigoConvocatoria
                                                                            && ec.CodContacto == CodigoContacto);


            //  cargamos la informacion del contacto
            var query = _consultas.Db.Contacto.Join(_consultas.Db.ProyectoContactos,
                                                     c => c.Id_Contacto, pc => pc.CodContacto,
                                                     (c, pc) => new
                                                     {
                                                         Idcontacto = c.Id_Contacto,
                                                         NombreCompleto =
                                                         string.Concat(c.Nombres, c.Apellidos),
                                                         Beneficiarios = pc.Beneficiario,
                                                         Inactivos = c.Inactivo
                                                     }).FirstOrDefault(
                                                                        p =>
                                                                        p.Idcontacto == CodigoContacto &&
                                                                        p.Inactivos == false);

            if (evalucacion != null)
            {

                txtdinero.Text = evalucacion.AporteDinero.ToString();
                txtespecie.Text = evalucacion.AporteEspecie.ToString();
                txtDetalleespecie.Text = evalucacion.DetalleEspecie;


            }

            if (query.Idcontacto != 0)
            {


                lblemprendedor.Text = query.Beneficiarios == true ? "SI" : "NO";
                TxtNombre.Text = query.NombreCompleto;
            }


        }
        /// <summary>
        /// actualiza entidad evaluacion
        /// </summary>
        public void Actulizar()
        {                        
            _consultas = new Consultas();
            var evaluacionContacto = new EvaluacionContacto();

            var evalucacion = _consultas.Db.EvaluacionContactos.FirstOrDefault(ec => ec.CodProyecto == CodigoProyecto
                                                                             && ec.CodConvocatoria == CodigoConvocatoria
                                                                             && ec.CodContacto == CodigoContacto);
            if (!string.IsNullOrEmpty(txtdinero.Text) || !string.IsNullOrEmpty(txtespecie.Text))
            {
                if (evalucacion != null)
                {

                    evalucacion.AporteDinero = !string.IsNullOrEmpty(txtdinero.Text) ? Convert.ToDouble(txtdinero.Text) : 0;
                    evalucacion.AporteEspecie = !string.IsNullOrEmpty(txtespecie.Text) ? Convert.ToDouble(txtespecie.Text) : 0;
                    evalucacion.DetalleEspecie = txtDetalleespecie.Text;
                    _consultas.Db.SubmitChanges();
                    Redireccionar("Su Registro fue modificado exitosamente!");

                }
                else
                {

                    evaluacionContacto.AporteDinero = !string.IsNullOrEmpty(txtdinero.Text) ? Convert.ToDouble(txtdinero.Text) : 0;
                    evaluacionContacto.AporteEspecie = !string.IsNullOrEmpty(txtespecie.Text) ? Convert.ToDouble(txtespecie.Text) : 0;
                    evaluacionContacto.DetalleEspecie = txtDetalleespecie.Text;
                    evaluacionContacto.Entidades = string.Empty;
                    evaluacionContacto.CodConvocatoria = CodigoConvocatoria;
                    evaluacionContacto.CodProyecto = CodigoProyecto;
                    evaluacionContacto.CuentasCorrientes = 0;
                    evaluacionContacto.ValorCartera = 0;
                    evaluacionContacto.ValorOtrasCarteras = 0;
                    evaluacionContacto.CodContacto = CodigoContacto;
                    evaluacionContacto.Entidades = string.Empty;
                    _consultas.Db.EvaluacionContactos.InsertOnSubmit(evaluacionContacto);
                    _consultas.Db.SubmitChanges();
                    Redireccionar("Registro Exitoso!");
                }
            }
            else
            {
                txtdinero.Focus();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "alert('Debe ingresar el valor en dinero y/o valor especie');", true);
            }

        }
        public void Redireccionar(string mensaje)
        {
            UpdateTab();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.opener.location.reload();window.close();", true);
        }

        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            Actulizar();

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Redireccionar("");
        }

        private void UpdateTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)CodigoTab,
                CodContacto = Usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };

            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
            Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");            
        }
    }
}