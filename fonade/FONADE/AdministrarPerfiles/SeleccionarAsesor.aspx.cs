using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// SeleccionarAsesor
    /// </summary>    
    public partial class SeleccionarAsesor : Negocio.Base_Page
    {
        [ContextStatic]
        private static int CodContacto_ = 0;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             este programa posee tres paneles, los cuales se muestran o se ocultan dependiendo del estado del programa
             * primero se muestra una opcion para que el usuario busque por documento a un usuario existente, si no existe
             * ofrece la posibilidad al usuario de crear un nuevo usuario, este programa pasa al programa padre (crear asesores aspx)
             * un codigo de contacto, que puede ser nuevo o existente via la url, para que se continue el proceso.
             * 
             */
            if (!Page.IsPostBack)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                Panel3.Visible = false;

                /* 
                * traer lista de tipos de documento desde la base de datos
                */

                var query3 = (from ti in consultas.Db.TipoIdentificacions
                              select new
                              {
                                  Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
                                  NomTipoIdentificacion = ti.NomTipoIdentificacion,

                              }
                             );
                ddl_tipoDocumento.DataTextField = "NomTipoIdentificacion";

                ddl_tipoDocumento.DataValueField = "Id_TipoIdentificacion";

                ddl_tipoDocumento.DataSource = query3;
                ddl_tipoDocumento.DataBind();
            }

        }

        /// <summary>
        /// Handles the onclick event of the Buscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Buscar_onclick(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
            HttpContext.Current.Session["tb_TipodeIdentificacion"] = ddl_tipoDocumento.SelectedValue.ToString();
            HttpContext.Current.Session["tb_NumeroDocumento"] = tb_NumeroDocumento.Text;
            short prueba1 = short.Parse(ddl_tipoDocumento.SelectedValue);
            double prueb2 = double.Parse(tb_NumeroDocumento.Text);
            /* buscar usuario por documento */
            var identificacion = (
                         from c in consultas.Db.Contacto
                         where c.Identificacion == prueb2
                              && c.CodTipoIdentificacion == prueba1
                         select new
                         {
                             Nombres = c.Nombres,
                             Apellidos = c.Apellidos,
                             IdContacto = c.Id_Contacto,
                             Email = c.Email,
                             NumeroDocumento = c.Identificacion,
                             TipoID = c.TipoIdentificacion

                         }).FirstOrDefault();

            if (identificacion != null && identificacion.IdContacto != 0)
            {
                /* si el usuario existe buscar si el usuario pertenece a un grupo: */
                CodContacto_ = identificacion.IdContacto;

                var grupo = (

                 from g in consultas.Db.Grupos
                 from gc in consultas.Db.GrupoContactos

                 where gc.CodGrupo == g.Id_Grupo
                       & gc.CodContacto == identificacion.IdContacto
                 select new
                 {

                     Rol = g.NomGrupo
                 }).FirstOrDefault();

                /* si el usuario esta en un grupo informar al jefe de que este usaurio ya posee un rol:*/
                if (grupo != null)
                {
                    btn_SeleccionarAsesor.Enabled = false;
                    btn_SeleccionarAsesor.CssClass += "boton_Link_Grid";
                    pruebas.Text = " " + prueb2 + " - " + identificacion.Nombres + " " + identificacion.Apellidos + " ";
                    btn_SeleccionarAsesor.Text = pruebas.Text;
                    lbl_rol.Text = grupo.Rol;
                    Label3.Text = "El usuario ya tiene un rol asignado y no puede ser cambiado.";

                    HttpContext.Current.Session["tb_Email"] = identificacion.Email;
                    HttpContext.Current.Session["tb_NombreAsesor"] = identificacion.Nombres;
                    HttpContext.Current.Session["tb_NumeroDocumento"] = prueb2.ToString();
                    HttpContext.Current.Session["tb_ApellidoAsesor"] = identificacion.Apellidos;
                    HttpContext.Current.Session["tb_NumeroIdentificacion"] = Convert.ToString(identificacion.NumeroDocumento);
                    HttpContext.Current.Session["tb_TipodeIdentificacion"] = identificacion.TipoID.NomTipoIdentificacion;
                }
                else
                {
                    /* si el usuario NO esta en un grupo, entonces enviar el codigo de contacto al programa padre via url*/
                    btn_SeleccionarAsesor.Enabled = true;
                    pruebas.Text = " " + prueb2 + " - " + identificacion.Nombres + " " + identificacion.Apellidos + " ";
                    btn_SeleccionarAsesor.Text = pruebas.Text;
                    lbl_rol.Text = "Usuario sin Rol";
                    Label3.Text = "Seleccione el usuario a asignar";
                    HttpContext.Current.Session["tb_Email"] = identificacion.Email;
                    HttpContext.Current.Session["tb_NombreAsesor"] = identificacion.Nombres;
                    HttpContext.Current.Session["tb_NumeroDocumento"] = prueb2.ToString();
                    HttpContext.Current.Session["tb_ApellidoAsesor"] = identificacion.Apellidos;
                    HttpContext.Current.Session["tb_NumeroIdentificacion"] = Convert.ToString(identificacion.NumeroDocumento);
                    HttpContext.Current.Session["tb_TipodeIdentificacion"] = identificacion.TipoID.NomTipoIdentificacion;
                    HttpContext.Current.Session["CodContacto"] = identificacion.IdContacto;

                }

            }
            else
            {
                /* como el usuario no fue encontrado, habilitar el formulario para grabar nuevos usuarios: */
                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = true;
                tb_NumeroDocumento.Enabled = true;
                tb_Email.Enabled = true;
                tb_NombreAsesor.Enabled = true;
                tb_ApellidoAsesor.Enabled = true;


            }




        }

        /// <summary>
        /// Handles the click event of the btn_Asesor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_Asesor_click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;

            /*  entonces enviar el codigo de contacto al programa padre via url, esto llama una funcion de javascript en el modulo padre. */


            cm.RegisterClientScriptBlock(this.GetType(), "",
            "<script type='text/javascript'>window.opener.enviaCodigoAsesor('" + CodContacto_.ToString() + "');window.close();</script>");

        }

        /// <summary>
        /// Handles the click event of the btn_nuevabusqueda control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_nuevabusqueda_click(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
            HttpContext.Current.Session["tb_Email"] = null;
            HttpContext.Current.Session["tb_NombreAsesor"] = null;
            HttpContext.Current.Session["tb_NumeroDocumento"] = null;
            HttpContext.Current.Session["tb_ApellidoAsesor"] = null;
            HttpContext.Current.Session["tb_NumeroIdentificacion"] = null;
            HttpContext.Current.Session["tb_TipodeIdentificacion"] = null;
            tb_NumeroDocumento.Text = null;
            pruebas.Text = "";


        }

        /// <summary>
        /// Handles the onclick event of the CrearPerfil control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void CrearPerfil_onclick(object sender, EventArgs e)
        {

            CrearNuevoAsesor();


        }

        private void CrearNuevoAsesor()
        {
            /* esta funcion inserta un registro en la tabla de contacto, si el contacto no existe previamente
             y envia el codigo del nuevo contacto creado al programa padre para su posterior creacion de registro
             en la tabla grupocontacto.
             */
            //SqlDataReader reader = null;

            string txtNombres = tb_NombreAsesor.Text;
            string txtApellidos = tb_ApellidoAsesor.Text;
            string txtEmail = tb_Email.Text;
            string CodTipoIdentificacion = HttpContext.Current.Session["tb_TipodeIdentificacion"].ToString();
            string numIdentificacion = HttpContext.Current.Session["tb_NumeroDocumento"].ToString();
            HttpContext.Current.Session["tb_Email"] = txtEmail;
            HttpContext.Current.Session["tb_NombreAsesor"] = txtNombres;
            HttpContext.Current.Session["tb_NumeroDocumento"] = tb_NumeroDocumento.Text;
            HttpContext.Current.Session["tb_ApellidoAsesor"] = txtApellidos;
            HttpContext.Current.Session["tb_NumeroIdentificacion"] = numIdentificacion;
            HttpContext.Current.Session["NombreTipoId"] = ddl_tipoDocumento.SelectedItem;

            #region validarCampos
            if (String.IsNullOrEmpty(txtNombres) || String.IsNullOrEmpty(txtApellidos) || String.IsNullOrEmpty(txtEmail) || String.IsNullOrEmpty(numIdentificacion))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Todos los campos son requeridos')", true);
                return;
            }

            if (!IsValidEmail(txtEmail))
            {
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El correo no es valido')", true);
                    return;
                }
            }

            Int64 valida;

            if (!Int64.TryParse(numIdentificacion, out valida))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El campo numero de identificacion tiene que ser numero')", true);
                return;
            }
            #endregion


            string txtClave = GeneraClave();

            string txtSQL = "SELECT Email FROM Contacto WHERE Email like'%" + txtEmail + "%'";


            var resul = consultas.ObtenerDataTable(txtSQL, "text");

            if (resul.Rows.Count != 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El usuario con el correo electrónico ingresado ya existe en el sistema.')", true);
                return;
            }
            else
            {
                #region crearAsesor
                txtSQL = "INSERT INTO Contacto (Nombres, Apellidos, CodTipoIdentificacion, Identificacion,Email,Clave) VALUES('" + txtNombres + "','" + txtApellidos + "'," + CodTipoIdentificacion + "," + numIdentificacion + ",'" + txtEmail + "','" + txtClave + "')";
                ejecutaReader(txtSQL, 2);

                txtSQL = "SELECT Id_Contacto FROM Contacto WHERE CodTipoIdentificacion = " + CodTipoIdentificacion + " AND Identificacion = " + numIdentificacion;
                //reader = ejecutaReader(txtSQL, 1);
                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                int CodContacto = 0;
                string txtNomTipoDoc;

                if(dt.Rows.Count > 0)
                {
                    CodContacto = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                }
                //if (reader != null)
                //    if (reader.Read())
                //        CodContacto = Convert.ToInt32(reader[0].ToString());
                HttpContext.Current.Session["CodContacto"] = CodContacto;//no se usa.


                txtSQL = "SELECT NomTipoIdentificacion FROM TipoIdentificacion WHERE Id_TipoIdentificacion = " + CodTipoIdentificacion;
                //reader = ejecutaReader(txtSQL, 1);
                var dt2 = consultas.ObtenerDataTable(txtSQL, "text");

                if(dt2.Rows.Count > 0)
                {
                    txtNomTipoDoc = dt2.Rows[0].ItemArray[0].ToString();
                }
                //if (reader != null)
                //    if (reader.Read())
                //        txtNomTipoDoc = reader[0].ToString();
                #endregion

                CodContacto_ = CodContacto;//si se usa.

                //ejecuta javascript en el programa padre.

                ClientScriptManager cm = this.ClientScript;
                cm.RegisterClientScriptBlock(this.GetType(), "",
       "<script type='text/javascript'>window.opener.enviaCodigoAsesor('" + CodContacto_.ToString() + "');window.close();</script>");


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

    }
}