using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.HubEmprendimiento
{
    public partial class hubdeemprendimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string Perfil = "";
        string Comunidad = "";
        string Notificar = "";
        char acepto;
        public void GuardarRegistro(object sender, EventArgs e)
        {
            try
            {
                using (Datos.HubDeEmprendimientoDataContext db = new Datos.HubDeEmprendimientoDataContext(conexion))
                {

                    int ValidarUsuario = (from val in db.HubDeEmprendimiento
                                          where val.Identificacion == Convert.ToInt32(txtIdentificacion.Text)
                                          select val.Identificacion).Count();

                    if (ValidarUsuario == 0)
                    {

                        if (RadioAprendiz.Checked)
                        {
                            Perfil = "APRENDIZ";

                        }
                        if (RadioEmprendedor.Checked)
                        {
                            Perfil = "EMPRENDEDOR";

                        }
                        if (RadioEmpresario.Checked)
                        {
                            Perfil = "EMPRESARIO";

                        }
                        if (RadioComprador.Checked)
                        {
                            Perfil = "COMPRADOR";

                        }
                        if (RadioInnovador.Checked)
                        {
                            Perfil = "INNOVADOR";

                        }
                        if (RadioInversionista.Checked)
                        {
                            Perfil = "INVERSIONISTA";

                        }
                        if (Radiosi.Checked)
                        {
                            Comunidad = "Si";
                        }
                        if (Radiono.Checked)
                        {
                            Comunidad = "No";
                        }
                        if (notificaciones.Checked)
                        {
                            Notificar = "SI";
                        }
                        else
                        {
                            Notificar = null;
                        }

                        if (politica.Checked)
                        {
                            acepto = Convert.ToChar("1");
                        }
                        else
                        {
                            acepto = Convert.ToChar("0");
                        }
                        Datos.HubDeEmprendimiento NuevoRegistro = new Datos.HubDeEmprendimiento
                        {


                            Nombres = txtNombres.Text,
                            Apellidos = txtApellidos.Text,
                            TipoDocumento = cmbTipoDocumento.SelectedValue,
                            Identificacion = Convert.ToInt32(txtIdentificacion.Text),
                            FechaNacimiento = Convert.ToDateTime(txtfechanacimiento.Text),
                            Correo = txtCorreo.Text,
                            Departamento = cmbDepartamento.SelectedValue,
                            Ciudad = txtCiudad.Text,
                            ComunidadSena = Comunidad,
                            Regional = cbmRegional.SelectedValue,
                            Perfil = Perfil,
                            RecibirNot = Notificar,
                            Telefono = txtTelefono.Text,
                            AceptoTerminos = acepto,
                            FechaRegistro = DateTime.Now

                        };

                        db.HubDeEmprendimiento.InsertOnSubmit(NuevoRegistro);
                        db.SubmitChanges();

                        string MensajeOk = "¡Bienvenido! Ya haces parte del Hub de Emprendimiento. " +
                            "Próximamente te enviaremos a tu correo las actividades a las que podrás acceder. " +
                            "Recuerda revisar tu bandeja de entrada o spam. ¡Gracias por participar! ";
                        string Ruta = "http://www.fondoemprender.com/hubdeemprendimiento/SitePages/Inicio.aspx";

                        Alert(MensajeOk, Ruta);
                    }
                    else
                    {
                        string MensajeValidacion = string.Format("alert('El usuario ya se encuentra registrado, solo se puede realizar el registro una vez');");
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", MensajeValidacion, true);
                    }

                }
            }
            catch (Exception ex)
            {
                string MensajeValidacion = string.Format("alert('Lo lamentamos no pudimos realizar el registro, por favor intenta nuevamente.');");
                ClientScript.RegisterStartupScript(this.GetType(), "alert", MensajeValidacion, true);
            }
        }


        private void Alert(string mensaje, string ruta = "")
        {
            if (ruta == "")
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');" +
                    "window.location='" + ruta + "';", true);
        }



        public class RegistrarInformacion
        {
            public int Id { get; set; }
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public string TipoDocumento { get; set; }
            public double Identificacion { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string Correo { get; set; }
            public string Departamento { get; set; }
            public string Ciudad { get; set; }
            public string ComunidadSena { get; set; }
            public string Regional { get; set; }
            public string Perfil { get; set; }
            public string RecibirNot { get; set; }
            public char AceptoTerminos { get; set; }



        }
    }
}