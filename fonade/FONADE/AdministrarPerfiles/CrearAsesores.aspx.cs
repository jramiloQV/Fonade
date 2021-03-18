using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// CrearAsesores
    /// </summary>    
    public partial class CrearAsesores : Negocio.Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             este programa crea asesores, los crea llamado un popup llamado SELECCIONAR ASESOR ASPX.
             * ese programa selecciona de una lista los usuarios, determina si ya eson asesores o si ya tienen un rol
             * y en caso de que no tengan un rol, entonces trae los datos directamente al programa padre (este)
             * para que se ejecute la funcion crearUsuarioAsesor.
             * 
             * recibe por la url el parmetro _cod_contacto, en caso de recirbirlo, entonces,
             * crea en la tabla grupocontacto el registro necesario.
             * 
             * este codigo de contacto se pasaba anteriormente via la session.
             * 
             */


            // llenar opciondes del dropdown de t.ipo de identificación
            //Detección de click en el panel de busqueda de asesor
            var controlName = Request.Params.Get("__EVENTTARGET");
            var argument = Request.Params.Get("__EVENTARGUMENT");
            tb_ApellidoAsesor.ReadOnly = true;
            tb_Email.ReadOnly = true;
            tb_NombreAsesor.ReadOnly = true;
            tb_NumeroIdentificacion.ReadOnly = true;
            tb_TipodeIdentificacion.ReadOnly = true;
            try
            {
                /*
                 si existe un parametro pro la url de contacto:
                 */
                if (Request.Params.Get("_cod_contacto") != null)
                {
                    int cod_contacto = 0;
                    Int32.TryParse(Request.Params.Get("_cod_contacto").ToString(), out cod_contacto);
                    /*
                     buscar contacto en base de datos por codigo:
                     */
                    if (cod_contacto != 0)
                    {
                        var identificacion = (
                            from c in consultas.Db.Contacto
                            where c.Id_Contacto == cod_contacto
                            select new
                            {
                                Nombres = c.Nombres,
                                Apellidos = c.Apellidos,
                                IdContacto = c.Id_Contacto,
                                Email = c.Email,
                                NumeroDocumento = c.Identificacion,
                                TipoID = c.TipoIdentificacion

                            }).FirstOrDefault();

                        HttpContext.Current.Session["Flag"] = null;
                        tb_ApellidoAsesor.Enabled = true;
                        tb_Email.Enabled = true;
                        tb_NombreAsesor.Enabled = true;
                        tb_NumeroIdentificacion.Enabled = true;
                        tb_TipodeIdentificacion.Enabled = true;
                        tb_ApellidoAsesor.Text = identificacion.Apellidos;
                        tb_Email.Text = identificacion.Email;
                        tb_NombreAsesor.Text = identificacion.Nombres;
                        tb_NumeroIdentificacion.Text = identificacion.NumeroDocumento.ToString(); // identificacion.TipoID.Id_TipoIdentificacion.ToString();
                        tb_TipodeIdentificacion.Text =  HttpContext.Current.Session["NombreTipoId"] == null ? identificacion.TipoID.ToString() : HttpContext.Current.Session["NombreTipoId"].ToString();
                        HttpContext.Current.Session["Flag"] = "3";

                        CrearUsuarioAsesor();

                    }



                }
                if (!Equals(HttpContext.Current.Session["Flag"], "1"))
                {
                    if (controlName == "Panel1" && argument == "Click")
                    {
                        HttpContext.Current.Session["Flag"] = null;
                        HttpContext.Current.Session["Flag"] = "2";
                        Redirect(null, "SeleccionarAsesor.aspx", "_Blank", "width=300,height=300");

                    }

                }
                if (!Page.IsPostBack)
                {
                    HttpContext.Current.Session["Flag"] = "0";
                    tb_ApellidoAsesor.Enabled = false;
                    tb_Email.Enabled = false;
                    tb_NombreAsesor.Enabled = false;
                    tb_NumeroIdentificacion.Enabled = false;
                    tb_TipodeIdentificacion.Enabled = false;


                }




            }
            catch(Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Handles the onclick event of the CrearPerfil control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void CrearPerfil_onclick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_NumeroIdentificacion.Text))
            {
                CrearUsuarioAsesor();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Asesor fue creado exitosamente');window.location='AdministrarAsesores.aspx'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El número de identificación del usuario es requerido!')", true);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private SqlDataReader ejecutaReader(String sql, int obj)
        {
            SqlDataReader reader = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

                if (conn != null)
                    conn.Close();

                conn.Open();

                if (obj == 1)
                    reader = cmd.ExecuteReader();
                else
                    cmd.ExecuteReader();
            }
            catch (SqlException se)
            {
                if (conn != null)
                    conn.Close();
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return reader;
        }

        private void CrearUsuarioAsesor()
        {
            /* esta funcion crea asesores,
             insertando en grupocontacto,
             valida si ya existe el email.
             * 
             */
            SqlDataReader reader = null;

            string txtNombres = tb_NombreAsesor.Text;
            string txtApellidos = tb_ApellidoAsesor.Text;
            string txtEmail = tb_Email.Text;
            string CodTipoIdentificacion = tb_TipodeIdentificacion.Text; // HttpContext.Current.Session["tb_TipodeIdentificacion"].ToString();
            string numIdentificacion = tb_NumeroIdentificacion.Text; // HttpContext.Current.Session["tb_NumeroDocumento"].ToString();
            string Texto_Obtenido;

            //#region validarCampos
            //if (String.IsNullOrEmpty(txtNombres) || String.IsNullOrEmpty(txtApellidos) || String.IsNullOrEmpty(txtEmail) || String.IsNullOrEmpty(numIdentificacion))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Todos los campos son requeridos')", true);
            //    return;
            //}

            //if (!IsValidEmail(txtEmail))
            //{
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El correo no es valido')", true);
            //        return;
            //    }
            //}

            //Int64 valida;

            //if (!Int64.TryParse(numIdentificacion, out valida))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo numero de identificacion tiene que ser numero')", true);
            //    return;
            //}
            //#endregion
            //modificacion se crea variable txtclave y se llama de la tabla creada en seleccionar asesor, 
            //el error era que estaba volviendo a generar la clave wplazas mayo 26-15
            string txtClave = "";

            string txtSQL = "SELECT Email,Clave FROM Contacto WHERE Email like'%" + txtEmail + "%'";

            var resul = consultas.ObtenerDataTable(txtSQL, "text");


            if (resul == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El usuario ya tiene un rol asignado y no puede ser cambiado.')", true);
                return;
            }
            else
            {
                /* busca codigo de contacto, si es nuulo indica cero.*/
                var cdcntct = HttpContext.Current.Session["CodContacto"] ?? "0";
                int CodContacto = Int32.Parse(cdcntct.ToString());
                txtClave = resul.Rows[0].ItemArray[1].ToString();
                txtSQL = "SELECT Id_Grupo FROM Grupo WHERE NomGrupo = 'Asesor'";
                //reader = ejecutaReader(txtSQL, 1);
                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                int CodGrupo = 5;
                if(dt.Rows.Count > 0)
                {
                    CodGrupo = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                }
                //if (reader != null) { if (reader.Read()) { CodGrupo = Convert.ToInt32(reader[0].ToString()); } }
                /* reactiva el usuario en caso de que estuviera inactivo. */
                txtSQL = "UPDATE contacto SET Inactivo = 0, CodInstitucion = " + usuario.CodInstitucion + " WHERE Id_Contacto = " + CodContacto;

                ejecutaReader(txtSQL, 2);
                /*desasigna el rol previo si hay alguno.*/
                txtSQL = "DELETE FROM GrupoContacto WHERE CodContacto = " + CodContacto;

                ejecutaReader(txtSQL, 2);
                /* asigna rol al usuario */
                txtSQL = "INSERT INTO GrupoContacto (CodGrupo,CodContacto) VALUES (" + CodGrupo + "," + CodContacto + ")";

                ejecutaReader(txtSQL, 2);
                #region Envio de correo Asesor Creado
                //Consultar el "TEXTO".
                Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");

                //Sólo por si acaso, si el resultado de "Texto_Obtenido" NO devuelve los datos según el texto esperado,
                //se debe asignar el texto tal cual se vió en BD el "28/04/2014".


                //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
                Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", "Asesor");
                Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", txtEmail.Trim());
                Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", txtClave);

                try
                {
                    //Generar y enviar mensaje.
                    Correo correo = new Correo(usuario.Email,
                                               "Fondo Emprender",
                                               txtEmail.Trim(),
                                               txtNombres.Trim() + " " + txtApellidos.Trim(),
                                               "Registro a Fondo Emprender",
                                               Texto_Obtenido);
                    correo.Enviar();
                    prLogEnvios("Registro a Fondo Emprender", usuario.Email, txtEmail.Trim(), "Crear Asesor", 0, true);

                }
                catch
                {
                    //El mensaje no pudo ser enviado.


                    //Inserción en tabla "LogEnvios".
                    prLogEnvios("Registro a Fondo Emprender", usuario.Email, txtEmail.Trim(), "Crear Asesor", 0, false);
                }

                #endregion
                tb_ApellidoAsesor.Enabled = false;
                tb_Email.Enabled = false;
                tb_NombreAsesor.Enabled = false;
                tb_NumeroIdentificacion.Enabled = false;
                tb_TipodeIdentificacion.Enabled = false;
                HttpContext.Current.Session["tb_ApellidoAsesor"] = null;
                HttpContext.Current.Session["tb_Email"] = null;
                HttpContext.Current.Session["tb_NombreAsesor"] = null;
                HttpContext.Current.Session["tb_NumeroDocumento"] = null;
                HttpContext.Current.Session["tb_TipodeIdentificacion"] = null;
                // Response.Redirect("AdministrarAsesores.aspx");

            }
        }

        /// <summary>
        /// Determines whether [is valid email] [the specified string in].
        /// </summary>
        /// <param name="strIn">The string in.</param>
        /// <returns>
        ///   <c>true</c> if [is valid email] [the specified string in]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidEmail(string strIn)
        {
            return

            System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Handles the TextChanged event of the tb_NumeroIdentificacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tb_NumeroIdentificacion_TextChanged(object sender, EventArgs e)
        {
            tb_ApellidoAsesor.Text = tb_NumeroIdentificacion.Text;
        }

    }
}