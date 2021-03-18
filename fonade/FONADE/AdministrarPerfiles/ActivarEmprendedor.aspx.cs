using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using Fonade.Clases;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// ActivarEmprendedor
    /// </summary>    
    public partial class ActivarEmprendedor : Negocio.Base_Page
    {
        /// <summary>
        /// The cod contacto
        /// </summary>
        public Int64 CodContacto;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodContacto = Convert.ToInt64(Request.QueryString["CodContacto"]);

            if (!IsPostBack)
            {
                try
                {
                    lbl_Titulo.Text = "EMPRENDEDORES INACTIVOS - REACTIVAR";
                    var query = (from Cont in consultas.Db.Contacto
                                 where Cont.Id_Contacto == CodContacto
                                 select new
                                 {
                                     NombreInactivo = Cont.Nombres,
                                     ApellidoInactivo = Cont.Apellidos,
                                     IdentInactivo = Cont.Identificacion,
                                     emailInactivo = Cont.Email,
                                     direccionInactivo = Cont.Direccion,
                                     telefonoInactivo = Cont.Telefono
                                 }).FirstOrDefault();

                    txnombres.Text = Convert.ToString(query.NombreInactivo);
                    txapellidos.Text = Convert.ToString(query.ApellidoInactivo);
                    txdireccion.Text = Convert.ToString(query.direccionInactivo);
                    txemail.Text = Convert.ToString(query.emailInactivo);
                    txidentificacion.Text = Convert.ToString(query.IdentInactivo);
                    txtelefono.Text = Convert.ToString(query.telefonoInactivo);
                }

                catch (Exception ex) {
                    base.errorMessageDetail = ex.Message;
                }
            }
        }

        /// <summary>
        /// Reactivar asesor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnReactivar_Click(object sender, EventArgs e)
        {   
            //Inicializar variables.
            String nombre = txnombres.Text;
            String apellido = txapellidos.Text;
            String Email = txemail.Text;
            String Identificacion = txidentificacion.Text;
            String dire = txdireccion.Text;
            String telefono = txtelefono.Text;
            String txtClave = "";
            string sentenciaSQL1 = "";
            DataTable rsupdate = new DataTable();
            DataTable rsClave = new DataTable();
            String txtMensaje = "";
            bool varExitoso = false;

            try
            {
                if (nombre.Contains("'")) { nombre = nombre.Replace("'", ""); }

                if (apellido.Contains("'")) { apellido = apellido.Replace("'", ""); }

                if (Email.Contains("'")) { Email = Email.Replace("'", ""); }

                if (Identificacion.Contains("'")) { Identificacion = Identificacion.Replace("'", ""); }

                if (dire.Contains("'")) { dire = dire.Replace("'", ""); }

                if (telefono.Contains("'")) { telefono = telefono.Replace("'", ""); }

                sentenciaSQL1 = "select Email from contacto where email='" + Email + "' and id_contacto<>" + CodContacto;
                rsupdate = consultas.ObtenerDataTable(sentenciaSQL1, "text");

                if (rsupdate.Rows.Count == 0)
                {
                    SaveLogin(Convert.ToInt32(CodContacto));

                    sentenciaSQL1 = " Update contacto set Inactivo=0, nombres='" + nombre + "', apellidos='" + apellido + "'," +
                                    " Email='" + Email + "', identificacion=" + Identificacion + "," +
                                    " direccion='" + dire + "',telefono='" + telefono + "'" +
                                    " where id_contacto=" + CodContacto;
                 
                    ejecutaReader(sentenciaSQL1, 2);

                    //se actualiza tabla logIngreso
                    sentenciaSQL1 = " update LogIngreso" +
                                    " set FechaUltimoIngreso = GETDATE()" +
                                    " where CodContacto = " + CodContacto;

                    ejecutaReader(sentenciaSQL1, 2);

                    sentenciaSQL1 = "select Clave from Contacto where id_contacto=" + CodContacto;
                    rsClave = consultas.ObtenerDataTable(sentenciaSQL1, "text");

                    if (rsClave.Rows.Count > 0)
                    { txtClave = rsClave.Rows[0]["Clave"].ToString(); }
                    
                    rsClave = null;
                    
                    sentenciaSQL1 = " insert into ContactoReActivacion (CodContacto, FechaReActivacion, CodContactoQReActiva) " +
                                    " values(" + CodContacto + ", GETDATE(), " + usuario.IdContacto + ")";

                    ejecutaReader(sentenciaSQL1, 2);

                    sentenciaSQL1 = " insert into ContactoActualizoReactivacion (CodContacto,ActualizoDatos,CambioClave, FechaReActivacion) " +
                                    " values(" + CodContacto + ",0,0, GETDATE())";
                   
                    ejecutaReader(sentenciaSQL1, 2);                  

                    txtMensaje = Texto("TXT_EMAILENVIOCLAVE");

                    if (txtMensaje.Contains("Señor Usuario") || txtMensaje.Trim() == null)
                    {
                        txtMensaje = "Señor Usuario Con el usuario {{Email}} y contraseña {{Clave}},  podrá acceder al sistema de información por medio de la pagina www.fondoemprender.com,  allí encontrara en la parte superior del sistema específicamente en el botón con el signo de interrogación  (?) el manual de su perfil ''{{Rol}}''";
                    }

                    txtMensaje = txtMensaje.Replace("{{Rol}}", "Emprendedor");
                    txtMensaje = txtMensaje.Replace("{{Email}}", Email.Trim());
                    txtMensaje = txtMensaje.Replace("{{Clave}}", txtClave);
                   
                    try
                    {
                        //Generar y enviar mensaje.
                        Correo correo = new Correo(ConfigurationManager.AppSettings.Get("Email").ToString(),
                                                   "Fondo Emprender",
                                                   Email.Trim(),
                                                   nombre.Trim() + " " + apellido.Trim(),
                                                   "Re-Activación a Fondo Emprender",
                                                   txtMensaje);
                        correo.Enviar();

                        //El mensaje fue enviado.
                        varExitoso = true;

                        //Inserción en tabla "LogEnvios".
                        prLogEnvios("Fondo Emprender", ConfigurationManager.AppSettings.Get("Email").ToString(), Email.Trim(), "Reactivación emprendedor", 0, varExitoso);
                    }
                    catch(Exception ex)
                    {
                        base.errorMessageDetail = ex.Message;
                        //El mensaje no pudo ser enviado.
                        if (!varExitoso)
                        {                            
                            prLogEnvios("Fondo Emprender", ConfigurationManager.AppSettings.Get("Email").ToString(), Email.Trim(), "Reactivación emprendedor", 0, varExitoso);
                        }
                    }                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(BtnReactivar, this.GetType(), "Mensaje", "alert('El Email Ingresado ya existe Por favor Verificar')", true);
                    return;
                }
            }
            catch (Exception ex){
                base.errorMessageDetail = ex.Message;
            }
            
            Response.Redirect("FiltroEmprendedorInactivo.aspx");  
        }

        /// <summary>
        /// Saves the login.
        /// </summary>
        /// <param name="codigoContacto">codigo contacto.</param>
        public static void SaveLogin(int codigoContacto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.LogIngresos.FirstOrDefault(filter => filter.CodContacto == codigoContacto);

                if (entity != null)
                {
                    entity.FechaUltimoIngreso = DateTime.Now;
                }
                else
                {
                    var newEntity = new LogIngreso
                    {
                        CodContacto = codigoContacto,
                        FechaUltimoIngreso = DateTime.Now
                    };

                    db.LogIngresos.InsertOnSubmit(newEntity);
                }
                db.SubmitChanges();
            }
        }
    }
}