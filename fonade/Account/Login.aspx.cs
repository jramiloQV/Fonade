using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Datos;
using Fonade.Clases;
using System.Runtime.Caching;
using Datos.DataType;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using System.Web.Configuration;
using Fonade.Negocio.Utilidades;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using System.Data;

namespace Fonade.Account
{
    /// <summary>
    /// Login
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Login : Negocio.Base_Page
    {
        ObjectCache cache;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            FormsAuthentication.SignOut();
            RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);

            cache = MemoryCache.Default;
        }

        /// <summary>
        /// Handles the Authenticate event of the LoginUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AuthenticateEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ApplicationException">
        /// No se logro obtener los datos de este usuario, Intente nuevamente.
        /// or
        /// Email o Contraseña incorrectos.
        /// </exception>
        [Obsolete]
        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            // RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);

            try
            {
                string nombreUsuario = LoginUser.UserName;
                string password = LoginUser.Password;

                FieldValidate.ValidateString("Email", nombreUsuario, true, 100);
                FieldValidate.ValidateString("Password", password, true);

                FonadeUser usuario = (FonadeUser)Membership.GetUser(LoginUser.UserName, false);
                RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);
                HttpContext.Current.Session["usuarioLogged"] = usuario;
                Session["menuUsuario"] = null;
                if (usuario == null)
                    throw new ApplicationException("No se logro obtener los datos de este usuario, Intente nuevamente.");

                string passEncriptado = "";

                Encriptar encriptar = new Encriptar();

                //validar si se migro el usuario a contraseña encriptada
                if (Negocio.PlanDeNegocioV2.Utilidad.User.contrasenaMigrada(usuario.IdContacto))
                {
                    //Nueva Funcion Nuevo metodo de clave
                    passEncriptado = encriptar.GetSHA256(password).ToUpper();
                }
                else
                {
                    //passEncriptado = password;
                    passEncriptado = encriptar.GetSHA256(password).ToUpper();
                }

                if (!usuario.Password1.ToUpper().Equals(passEncriptado))
                {
                    Clave ClaveActiva = Negocio.PlanDeNegocioV2.Utilidad.User.getClaveActiva(usuario.IdContacto);
                    Consultas consultas;
                    consultas = new Consultas();

                    var result = (from c in consultas.Db.Contacto
                                  where c.Email.Equals(nombreUsuario) && c.Id_Contacto == usuario.IdContacto
                                  select new
                                  {
                                      c.Nombres,
                                      c.Apellidos,
                                      c.Email,
                                      c.Clave,
                                      c.Id_Contacto,
                                      c.Inactivo
                                  }).FirstOrDefault();

                    if (ClaveActiva.ClaveTemporal != null && password.Equals(ClaveActiva.ClaveTemporal))
                    {
                        if (result.Inactivo)
                        {
                            throw new ApplicationException("No puede ingresar con la clave temporal asignada el usuario se encuentra inactivo.");

                            pnllogeo.Visible = true;
                            PanelCambioDeClave.Visible = false;
                            mensaje.Visible = false;
                            recordar.Visible = false;

                        }
                        else
                        {
                            RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);

                            CacheItemPolicy policy = new CacheItemPolicy();
                            string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
                            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Double.Parse("30"));
                            cache = MemoryCache.Default;
                            DateTime fechaTemporal;
                            if (ClaveActiva.FechaClaveTemporal != null)
                            {
                                fechaTemporal = (DateTime)ClaveActiva.FechaClaveTemporal;

                                if ((DateTime.Now - fechaTemporal).TotalMinutes > Double.Parse(tempPass))
                                {
                                    throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");
                                }
                                else
                                {

                                    pnllogeo.Visible = false;
                                    PanelCambioDeClave.Visible = true;
                                    LoginUser.Visible = false;
                                    recordar.Visible = false;
                                    mensaje.Visible = false;

                                }
                            }
                            else
                            {
                                throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");

                            }

                        }
                    }
                    else
                    {
                        if (ClaveActiva.ClaveTemporal == null && (password.Equals(usuario.Password1) || usuario.Password1.ToUpper().Equals(encriptar.GetSHA256(password).ToUpper())))
                        {
                            RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);
                            cache = MemoryCache.Default;
                            CacheItemPolicy policy = new CacheItemPolicy();
                            string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
                            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Double.Parse("30"));
                            DateTime fechaTemporal;
                            if (ClaveActiva.FechaClaveTemporal != null)
                            {
                                fechaTemporal = (DateTime)ClaveActiva.FechaClaveTemporal;

                                if ((DateTime.Now - fechaTemporal).TotalMinutes > Double.Parse(tempPass))
                                {
                                    throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");
                                }
                                else
                                {

                                    pnllogeo.Visible = false;
                                    PanelCambioDeClave.Visible = true;
                                    mensaje.Visible = false;
                                    recordar.Visible = false;
                                }
                            }
                            else
                            {

                                fnLogin();
                                //throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");

                            }
                        }

                        throw new ApplicationException("Email o Contraseña incorrectos.");
                    }
                }
                else
                {
                    fnLogin();


                }
            }
            catch (ApplicationException ex)
            {
                LoginUser.FailureText = "Advertencia :" + ex.Message;
            }
            catch (Exception)
            {
                LoginUser.FailureText = "El acceso no esta permitido para el usuario especificado. Intente nuevamente.";
            }
        }


        private void fnLogin()
        {
            var expireTime = WebConfigurationManager.AppSettings["ExpireTime"];
            string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
            Clave clave = Negocio.PlanDeNegocioV2.Utilidad.User.getClave(usuario.IdContacto);
            Clave ClaveActiva = Negocio.PlanDeNegocioV2.Utilidad.User.getClaveActiva(usuario.IdContacto);

            if ((clave.DebeCambiar == 1 && (DateTime.Now - usuario.LastPasswordChangedDate).TotalMinutes > Double.Parse(tempPass)))
            {
                System.Runtime.Caching.MemoryCache.Default.Dispose();
                Session.Abandon();
                throw new ApplicationException("La contraseña expiro!. Solicite nuevamente su clave temporal.");

            }
            else
            {

                string expirePass = WebConfigurationManager.AppSettings["VigenciaInfo"];
                if (((DateTime.Now - usuario.LastPasswordChangedDate).TotalDays > (Convert.ToInt32(expirePass) * 30)) || ClaveActiva.CambioClave != null)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual expiró, por favor realice el cambio de su contraseña')", true);

                    pnllogeo.Visible = false;
                    PanelCambioDeClave.Visible = true;
                    mensaje.Visible = false;
                    recordar.Visible = false;

                    // txtEmail.Text = string.Empty;
                    // lblEmail.Text = string.Empty;
                    // txtClave.Text = string.Empty;


                }
                else
                {

                    if (ClaveActiva.DebeCambiar == 1)
                    {
                        pnllogeo.Visible = false;
                        PanelCambioDeClave.Visible = true;
                        mensaje.Visible = false;
                        recordar.Visible = false;

                    }
                    else
                    {
                        actualizarClaveTemporal(usuario);

                        Negocio.PlanDeNegocioV2.Utilidad.User.InactivateByTime(usuario.IdContacto, Convert.ToInt32(expireTime));

                        var saveLogin = WebConfigurationManager.AppSettings["SaveLogin"];
                        if (saveLogin == "1")
                            Negocio.PlanDeNegocioV2.Utilidad.User.SaveLogin(usuario.IdContacto);

                        FormsAuthentication.RedirectFromLoginPage(usuario.UserName, false);
                    }

                }
            }


        }

        private void actualizarClaveTemporal(FonadeUser usuario)
        {
            Consultas consultas;
            consultas = new Consultas();

            var result = (from c in consultas.Db.Contacto
                          where c.Email.Equals(usuario.Email) && c.Id_Contacto == usuario.IdContacto
                          select new
                          {
                              c.Nombres,
                              c.Apellidos,
                              c.Email,
                              c.Clave,
                              c.Id_Contacto,
                              c.Inactivo
                          }).FirstOrDefault();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());


            string txtSQL = " UPDATE Clave SET ClaveTemporal = null, FechaClaveTemporal = null" +
                        " WHERE codContacto = " + result.Id_Contacto +
                        " AND YaAvisoExpiracion = 0";

            SqlCommand cmd = new SqlCommand();

            if (con != null)
            {
                if (con.State != ConnectionState.Open || con.State != ConnectionState.Broken) { con.Open(); }
            }

            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;
            cmd.CommandText = txtSQL;

            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }

        /// <summary>
        /// cambiar clave sin iniciar session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_cambiarClave_Click(object sender, EventArgs e)
        {
            bool validado = false;

            #region Validando longuitud de caracteres digitados.
            if (txt_claveActual.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual digitada no puede tener mas de 20 caracteres.')", true);
                validado = false;
            }
            else
            { validado = true; }
            if (txt_nuevaclave.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede tener mas de 20 caracteres.')", true);
                validado = false;
            }
            else { validado = true; }
            if (txt_confirmaNuevaClave.Text.Trim().Length > 20)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La confirmación de la nueva clave no puede tener mas de 20 caracteres.')", true);
                validado = false;
            }
            else { validado = true; }
            #endregion

            if (validado == true)
            {
                validarClaveActual();
            }
        }

        /// <summary>
        /// Maneja el evento Click del control hlOlvidaClave.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void hlOlvidaClave_Click(object sender, EventArgs e)
        {
            pnllogeo.Visible = false;
            pnlolvidoClave.Visible = true;
            mensaje.Visible = false;
            recordar.Visible = true;
            txtEmail.Text = string.Empty;
            lblEmail.Text = string.Empty;
            txtClave.Text = string.Empty;
        }


        /// <summary>
        /// Maneja el evento Click del control btnEnviar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            Consultas consultas;
            consultas = new Consultas();
            Consultas consulta = new Consultas();

            var result = (from c in consultas.Db.Contacto
                          where c.Email.Equals(txtEmail.Text) && c.Inactivo.Equals("0")
                          select new
                          {
                              c.Nombres,
                              c.Apellidos,
                              c.Email,
                              c.Clave,
                              c.Id_Contacto,
                              c.Cargo
                          }).FirstOrDefault();

            if (result != null)
            {
                var resultClave = (from c in consulta.Db.Clave
                                   where c.codContacto.Equals(result.Id_Contacto) && c.YaAvisoExpiracion == 0
                                   select new
                                   {
                                       c.Id_Clave,
                                       c.NomClave,
                                       c.YaAvisoExpiracion,
                                       c.ClaveTemporal,
                                       c.FechaClaveTemporal
                                   }).FirstOrDefault();


                string nuevaclave;
                nuevaclave = GeneraClave();


                String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
                bool Enviado = false; //Variable que determina si el mensaje fue enviado o no "como resultado de la re-activación".
                bool correcto = false;
                Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");
                SqlCommand cmd = new SqlCommand();

                Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", nuevaclave);
                Texto_Obtenido = Texto_Obtenido.Replace("{{nombre}}", result.Nombres.ToUpper() + " " + result.Apellidos.ToUpper());
                Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", result.Email);
                Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", result.Cargo);

                try
                {
                    //Generar y enviar mensaje.
                    Correo correo = new Correo(usuario.Email,//usuario.Email,
                                               "Fondo Emprender",
                                               result.Email.ToString(),//result.Email.ToString(),
                                               result.Nombres + " " + result.Apellidos,
                                               "Generación Mail ",
                                               Texto_Obtenido);
                    correo.Enviar();

                    //El mensaje fue enviado.
                    Enviado = true;

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Su clave temporal ha sido enviada al correo ingresado!')", true);


                    pnllogeo.Visible = true;
                    LoginUser.Visible = true;
                    pnlolvidoClave.Visible = false;
                    PanelCambioDeClave.Visible = false;
                    mensaje.Visible = false;
                    recordar.Visible = false;

                    if (Enviado)
                    {
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());


                        string txtSQL = " UPDATE Clave SET ClaveTemporal = '" + nuevaclave +
                                    "', FechaClaveTemporal = CONVERT(datetime, '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', 103) WHERE codContacto = " + result.Id_Contacto +
                                    " AND YaAvisoExpiracion = 0";

                        cmd = new SqlCommand();

                        if (con != null)
                        {
                            if (con.State != ConnectionState.Open || con.State != ConnectionState.Broken) { con.Open(); }
                        }

                        cmd.CommandType = CommandType.Text;

                        cmd.Connection = con;
                        cmd.CommandText = txtSQL;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        SqlCommand cmd3 = new SqlCommand(LogAud(result.Id_Contacto, "se solicitó clave temporal por opción de olvido"), con);

                        cmd3.ExecuteNonQuery();
                        cmd3.Dispose();
                        con.Close();
                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);


                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Usuario incorrecto o inactivo, por favor verifique, o comuniquese con el administrador";
            }


        }




        /// <summary>
        /// Maneja el evento Click del control btnEnviarCorreo.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            Consultas consultas;
            consultas = new Consultas();

            var result = (from c in consultas.Db.Contacto
                          where c.Email.Equals(lblEmail.Text) && c.Inactivo.Equals("0")
                          select new
                          {
                              c.Clave
                          }).FirstOrDefault();

            string cuerpo =
                "<h4>Reciba un cordial saludo del Fondo Emprender.</h4>" +
                "<br />" +
                "<br />" +
                "<p>Con la presente comunicación damos a conocer los datos de acceso al sistema de información del Fondo Emprender (www.fondoemprender.com).</p>" +
                "<br />" +
                "<br />" +
                "Nombre de usuario:" +
                lblEmail.Text +
                "<br />" +
                "Contraseña:" +
                result +
                "<br />" +
                "<br />" +
                "<p>Consideramos importante anotar que la información relacionada en la presente comunicación no debe ser conocida por terceras personas y la clave de acceso debe ser cambiada la primera vez que ingrese a la plataforma del Fondo Emprender con el fin de garantizar la confidencialidad y seguridad de la misma.</p>" +
                "<br />" +
                "<br />" +
                "Cordialmente," +
                "<br />" +
                "<br />" +
                "Fondo Emprender" +
                "<br />" +
                "Línea de soporte técnico (Bogotá): 3001720 - 3107816" +
                "<br />" +
                "<br />";


            Correo correo = new Correo("info@fondoemprender.com", "Fondo Emprender", lblEmail.Text, lblEmail.Text, "Clave Acceso", cuerpo);

            correo.Enviar();


            pnllogeo.Visible = true;
            pnlolvidoClave.Visible = false;
        }

        protected void Btn_Cancelar_Click(object sender, EventArgs e)
        {



            //System.Runtime.Caching.MemoryCache.Default.Dispose();
            Session.Abandon();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
            Response.Redirect("~/Account/Login.aspx");


        }



        protected void validarClaveActual()
        {
            Encriptar encriptar = new Encriptar();
            string ClaveScriptada = encriptar.GetSHA256(txt_claveActual.Text).ToUpper();
            string ClaveActualEncriptada = encriptar.GetSHA256(txt_nuevaclave.Text).ToUpper();
            var query = (from usu in consultas.Db.Contacto
                         where usu.Id_Contacto == usuario.IdContacto
                         select new
                         {
                             clave = usu.Clave,
                         }).FirstOrDefault();

            Clave ClaveActiva = Negocio.PlanDeNegocioV2.Utilidad.User.getClaveActiva(usuario.IdContacto);

            if (ClaveScriptada.ToUpper() != query.clave.ToUpper())
            {
                if (ClaveActiva.ClaveTemporal != null && ClaveActiva.ClaveTemporal != txt_claveActual.Text)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
                }
                else
                {
                    if (ClaveActiva.ClaveTemporal != null && ClaveActiva.ClaveTemporal == txt_claveActual.Text)
                    {
                        if (txt_claveActual.Text == txt_nuevaclave.Text)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede ser igual a la actual!')", true);
                        }
                        else
                        {
                            validarClavesUsadas(ClaveActualEncriptada);
                        }
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
                }

            }
            else
            {
                if (txt_claveActual.Text == txt_nuevaclave.Text)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede ser igual a la actual!')", true);
                }
                else
                {
                    validarClavesUsadas(ClaveActualEncriptada);
                }
            }
        }

        protected void validarClavesUsadas(string cifrado)
        {
            var query = (from pass in consultas.Db.Clave
                         where pass.NomClave == cifrado
                         & pass.codContacto == usuario.IdContacto
                         select new
                         {
                             pass
                         }).Count();

            if (query == 0)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    Encriptar encriptar = new Encriptar();
                    string ClaveScriptada = encriptar.GetSHA256(txt_nuevaclave.Text).ToUpper();
                    SqlCommand cmd = new SqlCommand("MD_CambiarClave", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodUsuario", usuario.IdContacto);
                    cmd.Parameters.AddWithValue("@nuevaclave", ClaveScriptada);
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    SqlCommand cmd3 = new SqlCommand(LogAud(usuario.IdContacto, "Cambiar clave"), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                    cmd2.Dispose();
                    cmd.Dispose();
                    cmd3.Dispose();
                    pnllogeo.Visible = true;
                    PanelCambioDeClave.Visible = false;
                    l_fechaActual.Visible = false;

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Clave cambiada exitosamente!. Se reiniciará la aplicación')", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.Page.GetType(), "RedirectScript", "window.close()", true);



                    actualizarClaveTemporal(usuario);

                    pnllogeo.Visible = true;
                    PanelCambioDeClave.Visible = false;
                    mensaje.Visible = false;
                    recordar.Visible = false;
                    System.Runtime.Caching.MemoryCache.Default.Dispose();
                    Session.Abandon();
                    Response.Redirect("~/Account/Login.aspx");

                    //cierra la session al cambio de la sesion


                }
                finally
                {

                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave ha sido utilizada con anterioridad, por favor utilice una clave diferente!')", true);
            }

        }

        public static Clave getClave(int codigoContacto)
        {
            Clave claveUser;
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                claveUser = db.Clave.FirstOrDefault(filter => filter.codContacto == codigoContacto);
            }
            return claveUser;
        }

    }

    #region AntiguoCode
    /// <summary>
    /// Login
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    //public partial class Login : Negocio.Base_Page
    //{
    //    ObjectCache cache;

    //    /// <summary>
    //    /// Handles the Load event of the Page control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    //    protected void Page_Load(object sender, EventArgs e)
    //    {

    //        l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

    //        FormsAuthentication.SignOut();
    //        RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);

    //        cache = MemoryCache.Default;
    //    }

    //    /// <summary>
    //    /// Handles the Authenticate event of the LoginUser control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="AuthenticateEventArgs"/> instance containing the event data.</param>
    //    /// <exception cref="ApplicationException">
    //    /// No se logro obtener los datos de este usuario, Intente nuevamente.
    //    /// or
    //    /// Email o Contraseña incorrectos.
    //    /// </exception>
    //    [Obsolete]
    //    protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
    //    {
    //        // RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);

    //        try
    //        {
    //            string nombreUsuario = LoginUser.UserName;
    //            string password = LoginUser.Password;

    //            FieldValidate.ValidateString("Email", nombreUsuario, true, 100);
    //            FieldValidate.ValidateString("Password", password, true);

    //            FonadeUser usuario = (FonadeUser)Membership.GetUser(LoginUser.UserName, false);
    //            RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);
    //            HttpContext.Current.Session["usuarioLogged"] = usuario;
    //            Session["menuUsuario"] = null;
    //            if (usuario == null)
    //                throw new ApplicationException("No se logro obtener los datos de este usuario, Intente nuevamente.");

    //            string passEncriptado = "";

    //            Encriptar encriptar = new Encriptar();

    //            //validar si se migro el usuario a contraseña encriptada
    //            if (Negocio.PlanDeNegocioV2.Utilidad.User.contrasenaMigrada(usuario.IdContacto))
    //            {
    //                //Nueva Funcion Nuevo metodo de clave
    //                passEncriptado = encriptar.GetSHA256(password).ToUpper();
    //            }
    //            else
    //            {
    //                //passEncriptado = password;
    //                passEncriptado = encriptar.GetSHA256(password).ToUpper();
    //            }

    //            //Quitar para pasar a prodiccion cifrado
    //            ///
    //            //Quitar para pasar a produccion el cifrado
    //            ///
    //            var resultQuitar = (from c in consultas.Db.Contacto
    //                          where c.Email.Equals(nombreUsuario) && c.Id_Contacto == usuario.IdContacto
    //                          select new
    //                          {
    //                              c.Nombres,
    //                              c.Apellidos,
    //                              c.Email,
    //                              c.Clave,
    //                              c.Id_Contacto,
    //                              c.Inactivo
    //                          }).FirstOrDefault();

    //            if (resultQuitar.Clave == password)
    //            {
    //                fnLogin();
    //            }
    //            else
    //            {
    //                throw new ApplicationException("Email o Contraseña incorrectos.");
    //            }
    //            //Fin Quitar
    //            /*
    //            if (!usuario.Password1.ToUpper().Equals(passEncriptado) && !usuario.Password1.Equals(password))
    //            {
    //                Clave ClaveActiva = Negocio.PlanDeNegocioV2.Utilidad.User.getClaveActiva(usuario.IdContacto);
    //                Consultas consultas;
    //                consultas = new Consultas();

    //                var result = (from c in consultas.Db.Contacto
    //                              where c.Email.Equals(nombreUsuario) && c.Id_Contacto == usuario.IdContacto
    //                              select new
    //                              {
    //                                  c.Nombres,
    //                                  c.Apellidos,
    //                                  c.Email,
    //                                  c.Clave,
    //                                  c.Id_Contacto,
    //                                  c.Inactivo
    //                              }).FirstOrDefault();

    //                if (ClaveActiva.ClaveTemporal != null && password.Equals(ClaveActiva.ClaveTemporal))
    //                {
    //                    if (result.Inactivo)
    //                    {
    //                        throw new ApplicationException("No puede ingresar con la clave temporal asignada el usuario se encuentra inactivo.");

    //                        pnllogeo.Visible = true;
    //                        PanelCambioDeClave.Visible = false;
    //                        mensaje.Visible = false;
    //                        recordar.Visible = false;

    //                    }
    //                    else
    //                    {
    //                        RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);

    //                        CacheItemPolicy policy = new CacheItemPolicy();
    //                        string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
    //                        policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Double.Parse("30"));
    //                        cache = MemoryCache.Default;
    //                        DateTime fechaTemporal;
    //                        if (ClaveActiva.FechaClaveTemporal != null)
    //                        {
    //                            fechaTemporal = (DateTime)ClaveActiva.FechaClaveTemporal;

    //                            if ((DateTime.Now - fechaTemporal).TotalMinutes > Double.Parse(tempPass))
    //                            {
    //                                throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");
    //                            }
    //                            else
    //                            {

    //                                pnllogeo.Visible = false;
    //                                PanelCambioDeClave.Visible = true;
    //                                LoginUser.Visible = false;
    //                                recordar.Visible = false;
    //                                mensaje.Visible = false;

    //                            }
    //                        }
    //                        else
    //                        {
    //                            throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");

    //                        }

    //                    }
    //                }
    //                else
    //                {
    //                    if (ClaveActiva.ClaveTemporal == null && (password.Equals(usuario.Password1) || usuario.Password1.ToUpper().Equals(encriptar.GetSHA256(password).ToUpper())))
    //                    {
    //                        RegularExpressionValidatora5.ValidationExpression = FONADE.Utilidades.Utils.l_requisitos_minimos.Replace("cuenta", usuario.Email).Replace("usuario", usuario.Email.Split('@')[0]).Replace("nombre", usuario.Nombres);
    //                        cache = MemoryCache.Default;
    //                        CacheItemPolicy policy = new CacheItemPolicy();
    //                        string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
    //                        policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Double.Parse("30"));
    //                        DateTime fechaTemporal;
    //                        if (ClaveActiva.FechaClaveTemporal != null)
    //                        {
    //                            fechaTemporal = (DateTime)ClaveActiva.FechaClaveTemporal;

    //                            if ((DateTime.Now - fechaTemporal).TotalMinutes > Double.Parse(tempPass))
    //                            {
    //                                throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");
    //                            }
    //                            else
    //                            {

    //                                pnllogeo.Visible = false;
    //                                PanelCambioDeClave.Visible = true;
    //                                mensaje.Visible = false;
    //                                recordar.Visible = false;
    //                            }
    //                        }
    //                        else
    //                        {

    //                            fnLogin();
    //                            //throw new ApplicationException("No puede ingresar con la clave temporal asignada la clave temporal ya expiro.");

    //                        }
    //                    }

    //                    throw new ApplicationException("Email o Contraseña incorrectos.");
    //                }
    //            }
    //            else
    //            {
    //                fnLogin();


    //            }*/
    //        }
    //        catch (ApplicationException ex)
    //        {
    //            LoginUser.FailureText = "Advertencia :" + ex.Message;
    //        }
    //        catch (Exception)
    //        {
    //            LoginUser.FailureText = "El acceso no esta permitido para el usuario especificado. Intente nuevamente.";
    //        }
    //    }


    //    private void fnLogin()
    //    {
    //        ///
    //        //Quitar para colocar cifrado en produccion
    //        ///
    //        FormsAuthentication.RedirectFromLoginPage(usuario.UserName, false);

    //        //var expireTime = WebConfigurationManager.AppSettings["ExpireTime"];
    //        //string tempPass = WebConfigurationManager.AppSettings["VigenciaTmp"];
    //        //Clave clave = Negocio.PlanDeNegocioV2.Utilidad.User.getClave(usuario.IdContacto);
    //        //Clave ClaveActiva = Negocio.PlanDeNegocioV2.Utilidad.User.getClaveActiva(usuario.IdContacto);

    //        //if ((clave.DebeCambiar == 1 && (DateTime.Now - usuario.LastPasswordChangedDate).TotalMinutes > Double.Parse(tempPass)))
    //        //{
    //        //    System.Runtime.Caching.MemoryCache.Default.Dispose();
    //        //    Session.Abandon();
    //        //    throw new ApplicationException("La contraseña expiro!. Solicite nuevamente su clave temporal.");

    //        //}
    //        //else
    //        //{

    //        //    string expirePass = WebConfigurationManager.AppSettings["VigenciaInfo"];
    //        //    if (((DateTime.Now - usuario.LastPasswordChangedDate).TotalDays > (Convert.ToInt32(expirePass) * 30)) || ClaveActiva.CambioClave != null)
    //        //    {

    //        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual expiró, por favor realice el cambio de su contraseña')", true);

    //        //        pnllogeo.Visible = false;
    //        //        PanelCambioDeClave.Visible = true;
    //        //        mensaje.Visible = false;
    //        //        recordar.Visible = false;

    //        //        // txtEmail.Text = string.Empty;
    //        //        // lblEmail.Text = string.Empty;
    //        //        // txtClave.Text = string.Empty;


    //        //    }
    //        //    else
    //        //    {


    //        //        actualizarClaveTemporal(usuario);

    //        //        Negocio.PlanDeNegocioV2.Utilidad.User.InactivateByTime(usuario.IdContacto, Convert.ToInt32(expireTime));

    //        //        var saveLogin = WebConfigurationManager.AppSettings["SaveLogin"];
    //        //        if (saveLogin == "1")
    //        //            Negocio.PlanDeNegocioV2.Utilidad.User.SaveLogin(usuario.IdContacto);

    //        //        FormsAuthentication.RedirectFromLoginPage(usuario.UserName, false);
    //        //    }
    //        //}


    //    }

    //    private void actualizarClaveTemporal(FonadeUser usuario)
    //    {
    //        Consultas consultas;
    //        consultas = new Consultas();

    //        var result = (from c in consultas.Db.Contacto
    //                      where c.Email.Equals(usuario.Email) && c.Id_Contacto == usuario.IdContacto
    //                      select new
    //                      {
    //                          c.Nombres,
    //                          c.Apellidos,
    //                          c.Email,
    //                          c.Clave,
    //                          c.Id_Contacto,
    //                          c.Inactivo
    //                      }).FirstOrDefault();

    //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());


    //        string txtSQL = " UPDATE Clave SET ClaveTemporal = null, FechaClaveTemporal = null" +
    //                    " WHERE codContacto = " + result.Id_Contacto +
    //                    " AND YaAvisoExpiracion = 0";

    //        SqlCommand cmd = new SqlCommand();

    //        if (con != null)
    //        {
    //            if (con.State != ConnectionState.Open || con.State != ConnectionState.Broken) { con.Open(); }
    //        }

    //        cmd.CommandType = CommandType.Text;

    //        cmd.Connection = con;
    //        cmd.CommandText = txtSQL;

    //        cmd.ExecuteNonQuery();
    //        cmd.Dispose();

    //    }

    //    /// <summary>
    //    /// cambiar clave sin iniciar session
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    protected void Btn_cambiarClave_Click(object sender, EventArgs e)
    //    {
    //        bool validado = false;

    //        #region Validando longuitud de caracteres digitados.
    //        if (txt_claveActual.Text.Trim().Length > 20)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual digitada no puede tener mas de 20 caracteres.')", true);
    //            validado = false;
    //        }
    //        else
    //        { validado = true; }
    //        if (txt_nuevaclave.Text.Trim().Length > 20)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede tener mas de 20 caracteres.')", true);
    //            validado = false;
    //        }
    //        else { validado = true; }
    //        if (txt_confirmaNuevaClave.Text.Trim().Length > 20)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La confirmación de la nueva clave no puede tener mas de 20 caracteres.')", true);
    //            validado = false;
    //        }
    //        else { validado = true; }
    //        #endregion

    //        if (validado == true)
    //        {
    //            validarClaveActual();
    //        }
    //    }

    //    /// <summary>
    //    /// Maneja el evento Click del control hlOlvidaClave.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    //    protected void hlOlvidaClave_Click(object sender, EventArgs e)
    //    {
    //        pnllogeo.Visible = false;
    //        pnlolvidoClave.Visible = true;
    //        mensaje.Visible = false;
    //        recordar.Visible = true;
    //        txtEmail.Text = string.Empty;
    //        lblEmail.Text = string.Empty;
    //        txtClave.Text = string.Empty;
    //    }


    //    /// <summary>
    //    /// Maneja el evento Click del control btnEnviar.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

    //    protected void btnEnviar_Click(object sender, EventArgs e)
    //    {
    //        Consultas consultas;
    //        consultas = new Consultas();
    //        Consultas consulta = new Consultas();

    //        var result = (from c in consultas.Db.Contacto
    //                      where c.Email.Equals(txtEmail.Text)
    //                      select new
    //                      {
    //                          c.Nombres,
    //                          c.Apellidos,
    //                          c.Email,
    //                          c.Clave,
    //                          c.Id_Contacto
    //                      }).FirstOrDefault();

    //        if (result != null)
    //        {
    //            var resultClave = (from c in consulta.Db.Clave
    //                               where c.codContacto.Equals(result.Id_Contacto) && c.YaAvisoExpiracion == 0
    //                               select new
    //                               {
    //                                   c.Id_Clave,
    //                                   c.NomClave,
    //                                   c.YaAvisoExpiracion,
    //                                   c.ClaveTemporal,
    //                                   c.FechaClaveTemporal
    //                               }).FirstOrDefault();


    //            string nuevaclave;
    //            nuevaclave = GeneraClave();


    //            String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
    //            bool Enviado = false; //Variable que determina si el mensaje fue enviado o no "como resultado de la re-activación".
    //            bool correcto = false;
    //            Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");
    //            SqlCommand cmd = new SqlCommand();

    //            Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", nuevaclave);
    //            Texto_Obtenido = Texto_Obtenido.Replace("{{nombre}}", result.Nombres.ToUpper() + " " + result.Apellidos.ToUpper());
    //            Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", result.Email);

    //            try
    //            {
    //                //Generar y enviar mensaje.
    //                Correo correo = new Correo( usuario.Email,
    //                                           "Fondo Emprender",
    //                                           result.Email.ToString(),
    //                                           result.Nombres + " " + result.Apellidos,
    //                                           "Generación Mail ",
    //                                           Texto_Obtenido);
    //                correo.Enviar();

    //                //El mensaje fue enviado.
    //                Enviado = true;

    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Su clave temporal a sido enviada al correo ingresado!')", true);


    //                pnllogeo.Visible = true;
    //                LoginUser.Visible = true;
    //                pnlolvidoClave.Visible = false;
    //                PanelCambioDeClave.Visible = false;
    //                mensaje.Visible = false;
    //                recordar.Visible = false;

    //                if (Enviado)
    //                {
    //                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

    //                    /*
    //                    string txtSQL = " UPDATE Clave SET ClaveTemporal = '" + nuevaclave +
    //                                "', FechaClaveTemporal = CONVERT(datetime, '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', 103) WHERE codContacto = " + result.Id_Contacto +
    //                                " AND YaAvisoExpiracion = 0";
    //                    */

    //                    string txtSQL = "update contacto set Clave = '" + nuevaclave +
    //                                "', fechaCambioClave = CONVERT(datetime, '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', 103) " +
    //                                "WHERE Id_Contacto = " + result.Id_Contacto;

    //                    cmd = new SqlCommand();

    //                    if (con != null)
    //                    {
    //                        if (con.State != ConnectionState.Open || con.State != ConnectionState.Broken) { con.Open(); }
    //                    }

    //                    cmd.CommandType = CommandType.Text;

    //                    cmd.Connection = con;
    //                    cmd.CommandText = txtSQL;

    //                    cmd.ExecuteNonQuery();
    //                    cmd.Dispose();

    //                    SqlCommand cmd3 = new SqlCommand(LogAud(usuario.IdContacto, "se solicitó clave temporal por opción de olvido"), con);

    //                    cmd3.ExecuteNonQuery();
    //                    cmd3.Dispose();
    //                    con.Close();
    //                }

    //            }
    //            catch (Exception ex)
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);


    //            }
    //        }
    //        else
    //        {
    //            lblError.Visible = true;
    //            lblError.Text = "Usuario incorrecto, por favor verifique";
    //        }


    //    }




    //    /// <summary>
    //    /// Maneja el evento Click del control btnEnviarCorreo.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    //    protected void btnEnviarCorreo_Click(object sender, EventArgs e)
    //    {
    //        Consultas consultas;
    //        consultas = new Consultas();

    //        var result = (from c in consultas.Db.Contacto
    //                      where c.Email.Equals(lblEmail.Text)
    //                      select new
    //                      {
    //                          c.Clave
    //                      }).FirstOrDefault();

    //        string cuerpo =
    //            "<h4>Reciba un cordial saludo del Fondo Emprender.</h4>" +
    //            "<br />" +
    //            "<br />" +
    //            "<p>Con la presente comunicación damos a conocer los datos de acceso al sistema de información del Fondo Emprender (www.fondoemprender.com).</p>" +
    //            "<br />" +
    //            "<br />" +
    //            "Nombre de usuario:" +
    //            lblEmail.Text +
    //            "<br />" +
    //            "Contraseña:" +
    //            result +
    //            "<br />" +
    //            "<br />" +
    //            "<p>Consideramos importante anotar que la información relacionada en la presente comunicación no debe ser conocida por terceras personas y la clave de acceso debe ser cambiada la primera vez que ingrese a la plataforma del Fondo Emprender con el fin de garantizar la confidencialidad y seguridad de la misma.</p>" +
    //            "<br />" +
    //            "<br />" +
    //            "Cordialmente," +
    //            "<br />" +
    //            "<br />" +
    //            "Fondo Emprender" +
    //            "<br />" +
    //            "Línea de soporte técnico (Bogotá): 3001720 - 3107816" +
    //            "<br />" +
    //            "<br />";


    //        Correo correo = new Correo("info@fondoemprender.com", "Fondo Emprender", lblEmail.Text, lblEmail.Text, "Clave Acceso", cuerpo);

    //        correo.Enviar();


    //        pnllogeo.Visible = true;
    //        pnlolvidoClave.Visible = false;
    //    }

    //    protected void Btn_Cancelar_Click(object sender, EventArgs e)
    //    {



    //        //System.Runtime.Caching.MemoryCache.Default.Dispose();
    //        Session.Abandon();
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
    //        Response.Redirect("~/Account/Login.aspx");


    //    }



    //    protected void validarClaveActual()
    //    {
    //        Encriptar encriptar = new Encriptar();
    //        string ClaveScriptada = encriptar.GetSHA256(txt_claveActual.Text).ToUpper();
    //        var query = (from usu in consultas.Db.Contacto
    //                     where usu.Id_Contacto == usuario.IdContacto
    //                     select new
    //                     {
    //                         clave = usu.Clave,
    //                     }).FirstOrDefault();

    //        Clave ClaveActiva = Negocio.PlanDeNegocioV2.Utilidad.User.getClaveActiva(usuario.IdContacto);

    //        if (ClaveScriptada != query.clave && txt_claveActual.Text != query.clave)
    //        {
    //            if (ClaveActiva.ClaveTemporal != null && ClaveActiva.ClaveTemporal != txt_claveActual.Text)
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
    //            }
    //            else
    //            {
    //                if (ClaveActiva.ClaveTemporal != null && ClaveActiva.ClaveTemporal == txt_claveActual.Text)
    //                {
    //                    if (txt_claveActual.Text == txt_nuevaclave.Text)
    //                    {
    //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede ser igual a la actual!')", true);
    //                    }
    //                    else
    //                    {
    //                        validarClavesUsadas();
    //                    }
    //                }
    //                else
    //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La clave actual es incorrecta!')", true);
    //            }

    //        }
    //        else
    //        {
    //            if (txt_claveActual.Text == txt_nuevaclave.Text)
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave no puede ser igual a la actual!')", true);
    //            }
    //            else
    //            {
    //                validarClavesUsadas();
    //            }
    //        }
    //    }

    //    protected void validarClavesUsadas()
    //    {
    //        var query = (from pass in consultas.Db.Clave
    //                     where pass.NomClave == txt_nuevaclave.Text
    //                     & pass.codContacto == usuario.IdContacto
    //                     select new
    //                     {
    //                         pass
    //                     }).Count();

    //        if (query == 0)
    //        {
    //            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
    //            try
    //            {
    //                Encriptar encriptar = new Encriptar();
    //                string ClaveScriptada = encriptar.GetSHA256(txt_nuevaclave.Text).ToUpper();
    //                SqlCommand cmd = new SqlCommand("MD_CambiarClave", con);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@CodUsuario", usuario.IdContacto);
    //                cmd.Parameters.AddWithValue("@nuevaclave", ClaveScriptada);
    //                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                SqlCommand cmd3 = new SqlCommand(LogAud(usuario.IdContacto, "Cambiar clave"), con);
    //                con.Open();
    //                cmd2.ExecuteNonQuery();
    //                cmd.ExecuteNonQuery();
    //                cmd3.ExecuteNonQuery();
    //                cmd2.Dispose();
    //                cmd.Dispose();
    //                cmd3.Dispose();
    //                pnllogeo.Visible = true;
    //                PanelCambioDeClave.Visible = false;
    //                l_fechaActual.Visible = false;

    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Clave cambiada exitosamente!. Se reiniciará la aplicación')", true);
    //                ScriptManager.RegisterClientScriptBlock(this, this.Page.GetType(), "RedirectScript", "window.close()", true);



    //                actualizarClaveTemporal(usuario);

    //                pnllogeo.Visible = true;
    //                PanelCambioDeClave.Visible = false;
    //                mensaje.Visible = false;
    //                recordar.Visible = false;
    //                System.Runtime.Caching.MemoryCache.Default.Dispose();
    //                Session.Abandon();
    //                Response.Redirect("~/Account/Login.aspx");

    //                //cierra la session al cambio de la sesion


    //            }
    //            finally
    //            {

    //                con.Close();
    //                con.Dispose();
    //            }
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La nueva clave ha sido utilizada con anterioridad, por favor utilice una clave diferente!')", true);
    //        }

    //    }

    //    public static Clave getClave(int codigoContacto)
    //    {
    //        Clave claveUser;
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            claveUser = db.Clave.FirstOrDefault(filter => filter.codContacto == codigoContacto);
    //        }
    //        return claveUser;
    //    }

    //}

    #endregion
}
