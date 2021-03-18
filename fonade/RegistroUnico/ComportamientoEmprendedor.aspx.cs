using Datos;
using Fonade.Negocio.Utilidades;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.RegistroUnico
{
    public partial class ComportamientoEmprendedor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string valor = Convert.ToString(Request.QueryString["doc"]);
                string actionForm = ConfigurationManager.AppSettings.Get("RutaWebSite") + "RegistroUnico/ComportamientoEmprendedor.aspx?doc=" + valor;
                form1.Action = actionForm;//"http://localhost:5153/RegistroUnico/ComportamientoEmprendedor.aspx?doc=Mod1.Uni1";
                cargarcombobox(cmbDepartamentoExpedicion, llenarDepartamento().Cast<Object>().ToList(), "Nombre", "Id");
               
            }
        }


        private void cargarcombobox(DropDownList ddl, List<object> Lista, string Nombre, string id)
        {

            ddl.DataSource = Lista;
            ddl.DataTextField = Nombre;
            ddl.DataValueField = id;
            ddl.DataBind();
            //ddl.Items.Insert(0, new ListItem("Seleccione..", "0"));

        }

        [WebMethod]
        public static List<Ciudad> llenarciudad(int CodDepartamento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var Ciudades = (from ciu in db.Ciudad
                                where ciu.CodDepartamento == CodDepartamento
                                select new Ciudad
                                {
                                    Id = ciu.Id_Ciudad,
                                    Nombre = ciu.NomCiudad,
                                    CodigoDepartamento = ciu.CodDepartamento
                                }).ToList();

                return Ciudades;
            }
        }

        public class Ciudad
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int CodigoDepartamento { get; set; }
        }

        public static List<Departamento> llenarDepartamento()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var departamentos = (from dep in db.departamento
                                     select new Departamento
                                     {
                                         Id = dep.Id_Departamento,
                                         Nombre = dep.NomDepartamento
                                     }).ToList();
                return departamentos;
            }
        }

        public class Departamento
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        
        private bool validarCampos()
        {
            bool valido = true;

            if (txtNombres.Text == "" || txtApellidos.Text == "" || txtIdentificacion.Text == ""
                || txtTelefono.Text == "" || txtCorreo.Text == "")
            {
                valido = false;
            }
            if (txtNombres.Text.Length<1 || txtApellidos.Text.Length<1 )
            {
                valido = false;
            }

            long num = 0;

            if (!(Int64.TryParse(txtIdentificacion.Text, out num)) || !(Int64.TryParse(txtTelefono.Text, out num)))
            {
                valido = false;
            }

            if (txtTelefono.Text.Length < 7 || txtTelefono.Text.Length > 10)
            {
                valido = false;
            }

            if (!IsValidEmail(txtCorreo.Text))
            {
                valido = false;
            }

            return valido;
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [WebMethod]    
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            
            if (validarCampos())
            {
                string valor = Convert.ToString(Request.QueryString["doc"]);

                string modulo = valor.Split('.')[0];
                string unidad = valor.Split('.')[1];

                valor = valor.Replace(".", string.Empty);

                using (RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
                {
                    SharePointComportamientoEmprendedor comportamientoEmprendedor = new SharePointComportamientoEmprendedor()
                    {
                        Nombres = txtNombres.Text,
                        Apellidos = txtApellidos.Text,
                        TipoIdentificacion = cmbTipoIdentificacion.SelectedItem.Text,
                        Identificacion = txtIdentificacion.Text,
                        Telefono = Convert.ToInt64(txtTelefono.Text),
                        Email = txtCorreo.Text,
                        codCiudad = Convert.ToInt32(CodCiudadExpedicion.Text),
                        Modulo = modulo,
                        Unidad = unidad,
                        RutaArchivoDescargado = rutaArchivo(valor),
                        emailEnviado = false,
                        archivoDescargado = false,
                        ParticiparCharlasHablemosDe = cmbParticiparCharlas.Value,
                        fechaIngreso = DateTime.Now,
                        ServicioOrientacion = chkOrientacion.Checked,
                        ServicioAsesoriaCreacionEmpresa = chkAsesoriaCreacionEmpresa.Checked,
                        ServicioAsesoriaFormulacionPlan = chkAsesoriaFormulacionPlan.Checked,
                        ServicioFortalecimientoEmpresarial = chkFortalecimientoEmpresarial.Checked
                    };

                    db.SharePointComportamientoEmprendedor.InsertOnSubmit(comportamientoEmprendedor);
                    db.SubmitChanges();

                    //Descargar Archivo
                    comportamientoEmprendedor.archivoDescargado = descargarArchivo(comportamientoEmprendedor.RutaArchivoDescargado);

                    //Enviar correo y confirmar envio
                    comportamientoEmprendedor.emailEnviado = enviarCorreo(comportamientoEmprendedor);

                    db.SubmitChanges();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myScript", "<script>javascript: alert('Por favor diligencie el formulario correctamente.');</script>");
            }

        }

        private string rutaArchivo(string parametro)
        {
            string ruta = "";

            using (RegistroUnicoDataContext db = new RegistroUnicoDataContext(conexion))
            {
                ruta = (from r in db.SharePointArchivosComportEmprendedor
                        where r.RutaArchivo.Contains(parametro)
                        select r.RutaArchivo
                        ).FirstOrDefault();
            }

            return ruta;
        }

        private bool descargarArchivo(string rutaArhivo)
        {
            bool descargado = false;
            try
            {
                string url = ConfigurationManager.AppSettings.Get("RutaIP") + rutaArhivo;
                Utilidades.DescargarArchivo(url);
                descargado = true;
            }
            catch (Exception)
            {
                descargado = false;
            }

            return descargado;
        }

        private bool enviarCorreo(SharePointComportamientoEmprendedor comportamientoEmprendedor)
        {
            bool enviado = false;

            try
            {
                string mensajeEnviar = MensajeComportamientoEmprendedor(comportamientoEmprendedor);
                string paraEmail = ConfigurationManager.AppSettings.Get("EmailComportamientoEmprendedor");

                CorreoAdvanced correo = new CorreoAdvanced(""
                    , "Descarga de archivo comportamiento emprendedor"
                    , paraEmail
                    , "William Daza"
                    , "Descarga de archivo comportamiento emprendedor"
                    , mensajeEnviar);
                string mensaje = correo.Enviar();
                if (mensaje != "OK")
                {                   
                    enviado = false;
                }
                else
                {
                    enviado = true;                   
                }

            }
            catch (Exception e)
            {               
                enviado = false;
            }

            return enviado;
        }

        private string MensajeComportamientoEmprendedor(SharePointComportamientoEmprendedor emprendedor)
        {
            string mensaje = "Se ha registrado una nueva descarga de archivo para comportamiento emprendedor."
                + "<br />"+"<br />" 
                + "Nombre: "+emprendedor.Nombres + "<br />"
                + "Apellido: " + emprendedor.Apellidos + "<br />"
                + emprendedor.TipoIdentificacion+": " + emprendedor.Identificacion + "<br />"
                + "Telefono: " + emprendedor.Telefono + "<br />"
                + "Email: " + emprendedor.Email + "<br />"
                + "Ciudad: " + getCiudad(emprendedor.codCiudad)  + "<br />"
                + "Departamento: " + getDepartamento(emprendedor.codCiudad) + "<br />"
                + "Archivo: " + emprendedor.Unidad + "<br />"
                + "Fecha Registro: " + emprendedor.fechaIngreso + "<br />"
                + "¿Le gustaría participar de las Charlas \"Hablemos de\"? : " + emprendedor.ParticiparCharlasHablemosDe + "<br />"
                + "¿A cuál(es) de los servicios de emprendimiento del SENA le gustaría acceder?: " + "<br />"
                + "- Orientación: " + (emprendedor.ServicioOrientacion.GetValueOrDefault(false) ? "SI" : "NO") + "<br />"
                + "- Asesoría para la creación de empresa: " + (emprendedor.ServicioAsesoriaCreacionEmpresa.GetValueOrDefault(false) ? "SI" : "NO") + "<br />"
                + "- Asesoría para la formulación del plan de negocios a Convocatorias Fondo Emprender: " + (emprendedor.ServicioAsesoriaFormulacionPlan.GetValueOrDefault(false) ? "SI" : "NO") + "<br />"
                + "- Fortalecimiento empresarial: " + (emprendedor.ServicioFortalecimientoEmpresarial.GetValueOrDefault(false) ? "SI" : "NO") + "<br />"
               ;           

            return mensaje;
        }

        private string getCiudad(int _codCiudad)
        {
            string ciudad = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                ciudad = (from r in db.Ciudad
                        where r.Id_Ciudad == _codCiudad
                        select r.NomCiudad
                        ).FirstOrDefault();
            }

            return ciudad;
        }

        private string getDepartamento(int _codCiudad)
        {
            string depto = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                depto = (from r in db.Ciudad
                         join d in db.departamento on r.CodDepartamento equals d.Id_Departamento
                          where r.Id_Ciudad == _codCiudad
                          select d.NomDepartamento
                        ).FirstOrDefault();
            }

            return depto;
        }

    }
}