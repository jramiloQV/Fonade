using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;
using Fonade.Clases;
using Fonade.Negocio;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// AdministrarUsuarios
    /// </summary>    
    public partial class AdministrarUsuarios : Negocio.Base_Page
    {
        string crearAdmin = "Crear Administrador";
        string crearGerenteEvaluador = "Crear GerenteEvaluador";
        string crearCallCenter = "Crear Call Center";
        string crearGerenteInterventor = "Crear Gerente Interventor";
        string crearPerfilFiduciario = "Crear Perfil Fiduciario";
        string crearLiderRegionarl = "Crear Líder Regioal";
        string crearAcreditador = "Crear Acreditador";
        string crearAbogado = "Crear Abogado";
        string crearCallCenterOperador = "Crear Call Center Operador";
        String grupos1;
        public String[] grupos = { "0" };
        int idusuario;
        string[] codgrupousuario;
        delegate void Del(string idn);
        // <summary>
        // Variable que contiene la URL actual.
        // </summary>
        //string var_url;

        /// <summary>
        /// Voids the modificar datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="Grupocontacto">The grupocontacto.</param>
        /// <exception cref="ApplicationException">el tamaño del numero de identificación supera el limite aceptado</exception>
        protected void void_ModificarDatos(string[] codgrupo, int Grupocontacto, int? _codOperador)
        {
            if (Request.QueryString["CodContacto"] != null)
            {
                int codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);


                var query = (from c in consultas.Db.Contacto
                             where c.Id_Contacto == codigoContacto
                             select c);
                Contacto cnt = null;
                foreach (Contacto c in query)
                {
                    c.Apellidos = tb_apelidos.Text;
                    c.Cargo = tb_cargo.Text;
                    c.Email = tb_email.Text;
                    c.Fax = tb_fax.Text;

                    var verificarCedula = Int64.Parse(tb_no.Text.Replace(".", ""));
                    if (verificarCedula >= Int32.MaxValue)
                        throw new ApplicationException(" el tamaño del numero de identificación supera el limite aceptado");

                    c.Identificacion = Int32.Parse(tb_no.Text.Replace(".", ""));
                    c.Nombres = tb_nombres.Text;
                    c.Telefono = tb_telefono.Text;
                    c.CodTipoIdentificacion = short.Parse(ddl_identificacion.SelectedValue);
                    c.codOperador = _codOperador;
                    cnt = c;
                    if (Session["grupoLider"] != null)
                    {
                        c.CodTipoAprendiz = int.Parse(ddlRegionales.SelectedValue);
                        if (ddlEstadoLider.SelectedValue == "0")
                        {
                            c.InactivoAsignacion = false;
                        }
                        else
                        {
                            c.InactivoAsignacion = true;
                        }
                    }

                    consultas.Db.SubmitChanges();
                }
                try
                {
                    consultas.Db.ExecuteCommand(UsuarioActual());
                    consultas.Db.SubmitChanges();
                    new Clases.Correo(System.Configuration.ConfigurationManager.AppSettings["Email"], System.Configuration.ConfigurationManager.AppSettings["WebSite"], cnt.Email, string.Format("{0} {1}", cnt.Nombres, cnt.Apellidos),
                      "Registro de usuario FONADE", string.Format("Correo de prueba notificaión de registro {2} Usuario: {0} {2} Clave: {1} ", cnt.Email, cnt.Clave, "\n")).Enviar();
                    void_show("La información del usuario ha sido actualizado correctamente", true);
                }
                catch (Exception ex) { }
            }
        }

        /// <summary>
        /// Voids the crear datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        /// <exception cref="ApplicationException">el tamaño del numero de identificación supera el limite aceptado</exception>
        protected void void_CrearDatos(string[] codgrupo, int codusuario, int? _codOperador = null)
        {
            try
            {
                string nuevaclave;

                nuevaclave = GeneraClave();
                String wclave = "";

                var verificarCedula = Int64.Parse(tb_no.Text.Replace(".", ""));
                if (verificarCedula >= Int32.MaxValue)
                    throw new ApplicationException(" el tamaño del numero de identificación supera el limite aceptado");

                Contacto contacto = new Contacto()
                {
                    Apellidos = tb_apelidos.Text,
                    Cargo = tb_cargo.Text,
                    Email = tb_email.Text,
                    Fax = tb_fax.Text,
                    Identificacion = Int64.Parse(tb_no.Text.Replace(".", "")),
                    Nombres = tb_nombres.Text,
                    Clave = Utilidades.Encrypt.GetSHA256(nuevaclave),
                    //Clave = nuevaclave,
                    Telefono = tb_telefono.Text,
                    CodTipoIdentificacion = short.Parse(ddl_identificacion.SelectedValue),
                    codOperador = _codOperador,
                    fechaCambioClave = DateTime.Now
                };

                //Si se va a guardar los datos de un lider regional, el Id de la regional sera guardada en la
                //columna CodTipoAprendiz de la tabla contacto. Esto para no alterar o crear nuevas tablas
                // Igualmente en la columna InactivoAsignacion para almacenar el estado activo / inactivo del rol Lider regional
                if (ddl_perfil.SelectedValue == Constantes.CONST_LiderRegional.ToString())
                {
                    //Valida si la regional tiene lider asignado
                    var lider = VerificarLiderRegional(int.Parse(ddlRegionales.SelectedValue));
                    if (lider == null)
                    {
                        contacto.CodTipoAprendiz = int.Parse(ddlRegionales.SelectedValue);
                        if (ddlEstadoLider.SelectedValue == "0")
                        {
                            contacto.InactivoAsignacion = false;
                        }
                        else
                        {
                            contacto.InactivoAsignacion = true;
                        }
                    }
                    else
                    {
                        void_show("La regional " + ddlRegionales.SelectedItem + " ya tiene lider regional asignado: " + lider.Nombres + " " + lider.Apellidos + ". Desactivelo primero.", true);
                        return;
                    }
                }
                GrupoContacto Grupocontacto = new GrupoContacto()
                {
                    CodGrupo = Int32.Parse(ddl_perfil.SelectedValue),
                    CodContacto = contacto.Id_Contacto
                };
                //enviar datos 
                try
                {
                    var vaidarcontacto = (from c1 in consultas.Db.Contacto
                                          where c1.Email == contacto.Email
                                          select c1).FirstOrDefault();

                    if (vaidarcontacto == null)
                    {

                        //*************************************AQUI MAIL WPLAZAS JUNIO 2-2015 *********************************//

                        String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
                        bool Enviado = false; //Variable que determina si el mensaje fue enviado o no "como resultado de la re-activación".
                        bool correcto = false;
                        Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");
                        //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
                        string wrol = "";

                        if (Request.QueryString["accion"] == "Crear")
                        {
                            switch (Request.QueryString["codGrupo"])
                            {
                                case "8":
                                    wrol = "Call Center";
                                    break;
                                case "12":
                                    wrol = "Gerente Interventor";
                                    break;
                                case "9":
                                    wrol = "Gerente Evaluador";
                                    break;
                                case "2":
                                    wrol = "Administrador";
                                    break;
                                case "3":
                                    wrol = "Administrador";
                                    break;
                                case "2,3":
                                    wrol = "Administrador";
                                    break;
                                case "15":
                                    wrol = "Fiducia";
                                    break;
                                case "16":
                                    wrol = "Lider Regional";
                                    break;
                                case "18":
                                    wrol = "Abogado";
                                    break;
                                case "19":
                                    wrol = "Acreditador";
                                    break;
                                case "20":
                                    wrol = "Call Center Operador";
                                    break;

                            }


                            consultas.Db.Contacto.InsertOnSubmit(contacto);
                            consultas.Db.SubmitChanges();
                            var nuevocontacto = (from c in consultas.Db.Contacto
                                                 where c.Email == contacto.Email
                                                 select c).FirstOrDefault();
                            Grupocontacto.CodContacto = nuevocontacto.Id_Contacto;
                            consultas.Db.GrupoContactos.InsertOnSubmit(Grupocontacto);
                            consultas.Db.SubmitChanges();

                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                            if (con != null)
                            {
                                if (con.State != ConnectionState.Open || con.State != ConnectionState.Broken) { con.Open(); }
                            }
                            SqlCommand cmd3 = new SqlCommand(LogAud(nuevocontacto.Id_Contacto, "Usuario Creado"), con);
                            cmd3.ExecuteNonQuery();
                            cmd3.Dispose();
                            con.Close();

                            wclave = nuevaclave;
                            Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", wrol);
                            Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", this.tb_email.Text);
                            Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", wclave);
                            Texto_Obtenido = Texto_Obtenido.Replace("{{nombre}}", contacto.Nombres.ToUpper() + " " + contacto.Apellidos.ToUpper());

                            try
                            {
                                //Generar y enviar mensaje.
                                Correo correo = new Correo(usuario.Email,
                                                           "Fondo Emprender",
                                                           tb_email.Text.ToString(),
                                                           this.tb_nombres.Text.Trim() + " " + this.tb_apelidos.Text.Trim(),
                                                           "Generación Mail " + wrol.ToString(),
                                                           Texto_Obtenido);
                                correo.Enviar();

                                //El mensaje fue enviado.
                                Enviado = true;

                                void_show("el usuario " + tb_email.Text.ToString() + " ha sido agregado", true);

                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);


                            }
                        }
                        //*************************************AQUI FIN MAIL WPLAZAS JUNIO 2-2015 *********************************//

                        //new Clases.Correo(System.Configuration.ConfigurationManager.AppSettings["Email"] ,System.Configuration.ConfigurationManager.AppSettings["WebSite"],contacto.Email, string.Format("{0} {1}", contacto.Nombres, contacto.Apellidos),
                        //  "Registro de usuario FONADE", string.Format("Correo de prueba notificaión de registro {2} Usuario: {0} {2} Clave: {1} ", contacto.Email, contacto.Clave, "\n")).Enviar();
                        //void_show("el usuario " + nuevocontacto.Email + " ha sido agregado", true);                             
                    }
                    else
                    {
                        void_show("el usuario " + vaidarcontacto.Email + " ya existe intente nuevamente", true);

                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);


            }
        }

        /// <summary>
        /// Voids the show.
        /// </summary>
        /// <param name="texto">The texto.</param>
        /// <param name="mostrar">if set to <c>true</c> [mostrar].</param>
        protected void void_show(string texto, bool mostrar)
        {
            //lbl_popup.Visible = mostrar;           
            //lbl_popup.Text = texto;
            //mpe1.Enabled = mostrar;
            //mpe1.Show();
            string queryRedir = Request.Url.Query;

            queryRedir = queryRedir.Substring(0, queryRedir.IndexOf('&'));

            string redirePagina = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath, queryRedir);

            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('" + texto + "');location.href='" + redirePagina + "'</script>");
        }

        private void cargarDllOperador()
        {
            var operadores = operadorController.getAllOperador();

            ddlOperador.DataTextField = "NombreOperador";
            ddlOperador.DataValueField = "idOperador";
            ddlOperador.DataSource = operadores;
            ddlOperador.DataBind();
        }

        /// <summary>
        /// Voids the hide controls.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="codgrupo">The codgrupo.</param>
        protected void void_HideControls(string accion, string[] codgrupo)
        {
            //Mostrar combo Operador
            if (aplicaPerfilOperador(codgrupo))
            {
                //lblOperador.Visible = true;
                //ddlOperador.Visible = true;
                tbl_infousuario.Rows[8].Visible = true;

                cargarDllOperador();
            }
            else
            {
                tbl_infousuario.Rows[8].Visible = false;
            }

            switch (accion)
            {
                case "Crear":
                    tbl_infousuario.Rows[3].Visible = false;
                    if (codgrupo.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
                        codgrupo.Contains(Constantes.CONST_AdministradorSena.ToString()) || codgrupo.Equals("" + Constantes.CONST_AdministradorSistema.ToString() + "," + Constantes.CONST_AdministradorSena.ToString()))
                    {
                        tbl_infousuario.Rows[6].Visible = true;
                    }
                    else
                    {
                        tbl_infousuario.Rows[6].Visible = false;
                    };
                    tbl_infousuario.Rows[5].Visible = false;

                    if (codgrupo.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
                        codgrupo.Contains(Constantes.CONST_AdministradorSena.ToString()) || codgrupo.Equals("" + Constantes.CONST_AdministradorSistema.ToString() + "," + Constantes.CONST_AdministradorSena.ToString()))
                    {
                        tbl_infousuario.Rows[5].Visible = true;
                    }



                    btn_crearActualizar.Text = "Crear";
                    break;
                case "Editar":
                    tbl_infousuario.Rows[3].Visible = false;
                    if (codgrupo.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
                        codgrupo.Contains(Constantes.CONST_AdministradorSena.ToString()))
                    {
                        tbl_infousuario.Rows[6].Visible = false;
                        Page.Header.Controls.Add(
                        new LiteralControl(
                            @"<style type='text/css'>
                                    select#bodyContentPlace_ddl_perfil option:nth-child(n+4)
                                    {
                                        display : none !important;
                                    }
                                    select#bodyContentPlace_ddl_perfil option:nth-child(1)
                                    {
                                        display : none !important;
                                    }
                            </style>
                            <script>
                                window.onload = function list() {
                                   document.getElementById('bodyContentPlace_ddl_perfil').selectedIndex = '1';
                                }
                            </script>
                            "
                        ));
                    }
                    else
                    {
                        tbl_infousuario.Rows[6].Visible = false;
                    };
                    tbl_infousuario.Rows[5].Visible = false;

                    //Seleccion de operador
                    tbl_infousuario.Rows[8].Visible = false;

                    btn_crearActualizar.Text = "Actualizar";

                    break;
                default: break;
            }

        }

        /// <summary>
        /// Voids the traer datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        protected void void_traerDatos(string[] codgrupo, int codusuario)
        {
            var tipoidentificación = (from ti in consultas.Db.TipoIdentificacions
                                      select new
                                      {
                                          Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
                                          NomTipoIdentificacion = ti.NomTipoIdentificacion,

                                      });
            var Roles = (from r in consultas.Db.Grupos
                         select new
                         {
                             Rol = r.NomGrupo,
                             codRol = r.Id_Grupo,
                         });
            if (codgrupo != null)
            {
                Roles.Where(r => codgrupo.Contains(r.codRol.ToString()));
            }

            ddl_identificacion.DataTextField = "NomTipoIdentificacion";
            ddl_identificacion.DataValueField = "Id_TipoIdentificacion";
            ddl_identificacion.DataSource = tipoidentificación;
            ddl_identificacion.DataBind();
            ddl_perfil.DataTextField = "Rol";
            ddl_perfil.DataValueField = "codRol";
            ddl_perfil.DataSource = Roles;
            ddl_perfil.DataBind();

            var contacto = (from c in consultas.Db.Contacto
                            from gc in consultas.Db.GrupoContactos
                            where c.Id_Contacto == codusuario & c.Id_Contacto == gc.CodContacto
                            select new
                            {
                                nombres = c.Nombres,
                                apellidos = c.Apellidos,
                                codtipoidentificacion = c.CodTipoIdentificacion,
                                Tipoidentificacion = c.TipoIdentificacion,
                                identificacion = c.Identificacion,
                                cargo = c.Cargo,
                                email = c.Email,
                                telefono = c.Telefono,
                                fax = c.Fax,
                                codgrupo = gc.CodGrupo,
                                NomGrupo = gc.Grupo,
                                EstadoLider = c.CodTipoAprendiz,
                                codOperador = c.codOperador
                            }).FirstOrDefault();

            tb_apelidos.Text = contacto.apellidos;
            tb_cargo.Text = contacto.cargo;
            tb_email.Text = contacto.email;
            tb_fax.Text = contacto.fax;
            tb_no.Text = contacto.identificacion.ToString();
            tb_nombres.Text = contacto.nombres;
            tb_telefono.Text = contacto.telefono;
            ddl_identificacion.SelectedValue = contacto.codtipoidentificacion.ToString();

            string[] cadenaGrupo = new string[1];
            cadenaGrupo[0] = contacto.codgrupo.ToString();

            if (aplicaPerfilOperador(cadenaGrupo))
            {
                ddlOperador.SelectedValue = contacto.codOperador.ToString();
            }

            if (contacto.codgrupo.ToString() == Constantes.CONST_LiderRegional.ToString())
            {
                rowEstado.Visible = true;

            }
        }

        /// <summary>
        /// Voids the traer datos rol.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        protected void void_traerDatosRol(string[] codgrupo, int codusuario)
        {
            var tipoidentificación = (from ti in consultas.Db.TipoIdentificacions
                                      select new
                                      {
                                          Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
                                          NomTipoIdentificacion = ti.NomTipoIdentificacion,
                                      });
            var Roles = (from g in consultas.Db.Grupos
                         where codgrupo.Contains(g.Id_Grupo.ToString())
                         select new
                         {
                             Rol = g.NomGrupo,
                             codRol = g.Id_Grupo,
                         });
            ddl_identificacion.DataTextField = "NomTipoIdentificacion";
            ddl_identificacion.DataValueField = "Id_TipoIdentificacion";
            ddl_identificacion.DataSource = tipoidentificación;
            ddl_identificacion.DataBind();
            ddl_perfil.DataTextField = "Rol";
            ddl_perfil.DataValueField = "codRol";
            ddl_perfil.DataSource = Roles;
            ddl_perfil.DataBind();
            var contacto = (from c in consultas.Db.Contacto
                            from gc in consultas.Db.GrupoContactos
                            where c.Id_Contacto == codusuario & c.Id_Contacto == gc.CodContacto
                            select new
                            {
                                nombres = c.Nombres,
                                apellidos = c.Apellidos,
                                codtipoidentificacion = c.CodTipoIdentificacion,
                                Tipoidentificacion = c.TipoIdentificacion,
                                identificacion = c.Identificacion,
                                cargo = c.Cargo,
                                email = c.Email,
                                telefono = c.Telefono,
                                fax = c.Fax,
                                codgrupo = gc.CodGrupo,
                                NomGrupo = gc.Grupo,
                                idContacto = c.Id_Contacto,
                                codAprendiz = c.CodTipoAprendiz,
                                CodEstadoLider = c.InactivoAsignacion,
                                codOperador = c.codOperador
                            }).FirstOrDefault();

            if (grupos.Contains(Constantes.CONST_LiderRegional.ToString()))
            {
                Session["grupoLider"] = Constantes.CONST_LiderRegional.ToString();
                var regionales = (from r in consultas.Db.departamento
                                  select r).ToList();
                ddlRegionales.DataSource = regionales;
                ddlRegionales.DataTextField = "NomDepartamento";
                ddlRegionales.DataValueField = "Id_Departamento";
                ddlRegionales.DataBind();
                ddlRegionales.Items.Insert(0, new ListItem("Seleccione", "0"));
                cellblRegional.Visible = true;
                cellddlRegional.Visible = true;
                rowEstado.Visible = true;
            }
            else
            {
                cellblRegional.Visible = false;
                cellddlRegional.Visible = false;
                rowEstado.Visible = false;
            }

            if (contacto != null)
            {
                var grupo = (from g in consultas.Db.GrupoContactos
                             where g.CodContacto == contacto.idContacto
                             select g).FirstOrDefault();
                if (grupo != null)
                {
                    if (grupo.CodGrupo.ToString() == Constantes.CONST_LiderRegional.ToString())
                    {
                        var regionales = (from r in consultas.Db.departamento
                                          select r).ToList();
                        ddlRegionales.DataSource = regionales;
                        ddlRegionales.DataTextField = "NomDepartamento";
                        ddlRegionales.DataValueField = "Id_Departamento";
                        ddlRegionales.DataBind();
                        ddlRegionales.Items.Insert(0, new ListItem("Seleccione", "0"));
                        cellblRegional.Visible = true;
                        cellddlRegional.Visible = true;

                        cellblRegional.Visible = true;
                        cellddlRegional.Visible = true;
                        ddlRegionales.SelectedValue = contacto.codAprendiz.ToString();

                        ddlEstadoLider.Items.Insert(0, new ListItem("Activo", "0"));
                        ddlEstadoLider.Items.Insert(1, new ListItem("Inactivo", "1"));
                        if (contacto.CodEstadoLider.ToString() == "True")
                        {
                            ddlEstadoLider.SelectedValue = "1";
                        }
                        else
                        {
                            ddlEstadoLider.SelectedValue = "0";
                        }
                        ddlEstadoLider.Visible = true;
                    }
                }
            }

        }
        /// <summary>
        /// Voids the obtener parametros.
        /// </summary>
        protected void void_ObtenerParametros()
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
                {
                    grupos1 = Request.QueryString["codGrupo"];
                    grupos = grupos1.Split(',');
                }
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                    pnl_Administradores.Visible = false;
                    pnl_crearEditar.Visible = true;
                    AgregarUsuario.Visible = false;
                    if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                    {
                        if (Request.QueryString["Accion"].ToString() == "Editar")
                        {
                            AgregarUsuario.Text = "Actualizar";
                            idusuario = Int32.Parse(Request.QueryString["CodContacto"]);
                            void_traerDatos(codgrupousuario, idusuario);
                        }
                        if (Request.QueryString["Accion"].ToString() == "Crear")
                        {
                            AgregarUsuario.Text = "Crear";

                            if (Request.QueryString["codGrupo"].ToString() == Constantes.CONST_LiderRegional.ToString())
                            {
                                ddlEstadoLider.Items.Insert(0, new ListItem("Activo", "0"));
                                ddlEstadoLider.Items.Insert(1, new ListItem("Inactivo", "1"));
                            }
                        }
                    }
                }
            }
            catch
            {
                #region Código que redirecciona al usuario de acuerdo a la URL determinada.

                //Instanciar la variable que contiene la URL.
                string val = "";
                val = HttpContext.Current.Session["URL_redirect"] != null ? val = HttpContext.Current.Session["URL_redirect"].ToString() : "";

                //Si NO contiene  URL
                if (val == "")
                { Response.Redirect("../MiPerfil/Home.aspx"); }
                else //De lo contrario, la usa.
                { Response.Redirect(val); }

                #endregion
            }
        }
        /// <summary>
        /// Redirecciona al usuario a la página correspondiente.
        /// </summary>        
        protected void void_ModificarTextolink()
        {
            if (grupos.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
               grupos.Contains(Constantes.CONST_AdministradorSena.ToString()))
            {
                AgregarUsuario.Text = crearAdmin;

            }
            if (grupos.Contains(Constantes.CONST_GerenteEvaluador.ToString()))
            {
                AgregarUsuario.Text = crearGerenteEvaluador;
            }
            if (grupos.Contains(Constantes.CONST_CallCenter.ToString()))
            {
                AgregarUsuario.Text = crearCallCenter;
            }
            if (grupos.Contains(Constantes.CONST_GerenteInterventor.ToString()))
            {
                AgregarUsuario.Text = crearGerenteInterventor;
            }
            if (grupos.Contains(Constantes.CONST_Perfil_Fiduciario.ToString()))
            {
                AgregarUsuario.Text = crearPerfilFiduciario;
            }
            if (grupos.Contains(Constantes.CONST_LiderRegional.ToString()))
            {
                AgregarUsuario.Text = crearLiderRegionarl;
            }
            if (grupos.Contains(Constantes.CONST_PerfilAcreditador.ToString()))
            {
                AgregarUsuario.Text = crearAcreditador;
            }
            if (grupos.Contains(Constantes.CONST_PerfilAbogado.ToString()))
            {
                AgregarUsuario.Text = crearAbogado;
            }
            if (grupos.Contains(Constantes.CONST_CallCenterOperador.ToString()))
            {
                AgregarUsuario.Text = crearCallCenterOperador;
            }

            string url = Request.Url.GetLeftPart(UriPartial.Path);
            url += (Request.QueryString.ToString() == "") ? "?Accion=Crear" : "?" + Request.QueryString.ToString() + "&Accion=Crear";
            AgregarUsuario.NavigateUrl = url;
        }

        public void ValidateUsers()
        {
            try
            {
                if (!(usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        private void ocultarColumnas(int _codGrupo)
        {
            if (_codGrupo == Constantes.CONST_AdministradorSistema)
            {
                gv_administradores.Columns[4].Visible = false;
                gv_administradores.Columns[5].Visible = false;
            }

            string[] cadenaGrupo = new string[1];
            cadenaGrupo[0] = _codGrupo.ToString();

            if (aplicaPerfilOperador(cadenaGrupo))
            {
                gv_administradores.Columns[3].Visible = false;
                gv_administradores.Columns[5].Visible = false;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidateUsers();

                string Accion = "";
                string codigoGrupo = "0";
                int codigoContacto = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["CodGrupo"]))
                {
                    codigoGrupo = Request.QueryString["CodGrupo"];
                    grupos1 = Request.QueryString["codGrupo"];
                    grupos = grupos1.Split(',');
                }
                if (!String.IsNullOrEmpty(Request.QueryString["CodContacto"]))
                {
                    codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                    Accion = Request.QueryString["Accion"].ToString();
                    void_HideControls(Accion, grupos);
                };
                void_traerDatosRol(grupos, codigoContacto);
                pnl_crearEditar.Visible = false;
                string url = Request.Url.GetLeftPart(UriPartial.Path);
                url += (Request.QueryString.ToString() == "") ? "?Accion=Crear" : "?" + Request.QueryString.ToString() + "&Accion=Crear";
                AgregarUsuario.NavigateUrl = url;
                void_ObtenerParametros();
                lbl_Titulo.Text = void_establecerTitulo(grupos, Accion, "Texto");
                void_ModificarTextolink();
            }

        }

        /// <summary>
        /// Handles the DataBound event of the gv_administradores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gv_administradores_DataBound(object sender, EventArgs e)
        {
            String[] grupos2 = { "0" };
            String grupos3;
            if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
            {
                grupos3 = Request.QueryString["codGrupo"];
                grupos2 = grupos1.Split(',');
                if (grupos2.Contains(Constantes.CONST_AdministradorSistema.ToString()) | grupos2.Contains(Constantes.CONST_AdministradorSena.ToString()))
                {
                    gv_administradores.Columns[3].Visible = true;
                }
                else
                {
                    gv_administradores.Columns[3].Visible = false;
                }
            }

        }

        /// <summary>
        /// Handles the RowDataBound event of the gv_administradores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_administradores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if(gv_administradores.Rows.Count > 0)
            //{
            //    HyperLink nombreProyecto = new HyperLink();
            //    nombreProyecto = ((HyperLink)e.Row.Cells[1].FindControl("hl_nombre"));
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
            //            nombreProyecto.NavigateUrl = nombreProyecto.NavigateUrl + "&codGrupo=" + Request.QueryString["codGrupo"].ToString();
            //    }
            //}
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gv_administradores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gv_administradores_PageIndexChanged(object sender, GridViewPageEventArgs e)
        {

        }

        OperadorController operadorController = new OperadorController();

        /// <summary>
        /// Handles the Selecting event of the lds_Administradores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_Administradores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CodGrupo"]))
            {
                grupos1 = Request.QueryString["codGrupo"];
                grupos = grupos1.Split(',');
            }

            IList<users> usuarios;

            if (!grupos.Contains(Constantes.CONST_LiderRegional.ToString()))
            {
                usuarios = (from c in consultas.Db.Contacto
                            from gc in consultas.Db.GrupoContactos
                            from g in consultas.Db.Grupos
                            orderby c.Nombres ascending
                            where c.Inactivo == false & c.Id_Contacto == gc.CodContacto &
                                  g.Id_Grupo == gc.CodGrupo & grupos.Contains(gc.CodGrupo.ToString())
                            select new users
                            {
                                Id_contacto = c.Id_Contacto,
                                Nombres = c.Nombres + ' ' + c.Apellidos,
                                Email = c.Email,
                                codgrupo = gc.CodGrupo,
                                nomgrupo = g.NomGrupo,
                                regional = Convert.ToInt32(c.CodTipoAprendiz == null ? 0 : c.CodTipoAprendiz),
                                idOperador = c.codOperador
                            }).ToList();

                //Actualizar el campo operador
                if (aplicaPerfilOperador(grupos))
                {
                    foreach (var u in usuarios)
                    {
                        if (u.idOperador != null)
                            u.operador = operadorController.getOperador(u.idOperador).NombreOperador;
                    }
                }
            }
            else
            {
                usuarios = (from c in consultas.Db.Contacto
                            from gc in consultas.Db.GrupoContactos
                            from g in consultas.Db.Grupos
                            orderby c.Nombres ascending
                            where c.Inactivo == false & c.Id_Contacto == gc.CodContacto &
                                  g.Id_Grupo == gc.CodGrupo & grupos.Contains(gc.CodGrupo.ToString()) &&
                                  c.InactivoAsignacion == false
                            select new users
                            {
                                Id_contacto = c.Id_Contacto,
                                Nombres = c.Nombres + ' ' + c.Apellidos,
                                Email = c.Email,
                                codgrupo = gc.CodGrupo,
                                nomgrupo = g.NomGrupo,
                                regional = (int)c.CodTipoAprendiz
                            }).ToList();
            }

            e.Arguments.TotalRowCount = usuarios.Count();
            if (e.Arguments.TotalRowCount == 0)
            {
                Lbl_Resultados.Visible = true;
                Lbl_Resultados.Text = "No existen datos relacionados.";
            }
            else
            {
                Lbl_Resultados.Visible = false;
            }

            ocultarColumnas(Convert.ToInt32(grupos[0]));

            e.Result = usuarios.ToList();
        }

        /// <summary>
        /// Handles the click event of the btn_Inactivar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        protected void btn_Inactivar_click(object sender, CommandEventArgs e)
        {
            GrupoContacto Grupocontacto = new GrupoContacto()
            {
                CodGrupo = Int32.Parse(ddl_perfil.SelectedValue),
                CodContacto = Int32.Parse(e.CommandArgument.ToString())
            };
            consultas.Db.GrupoContactos.Attach(Grupocontacto);
            consultas.Db.GrupoContactos.DeleteOnSubmit(Grupocontacto);
            consultas.Db.SubmitChanges();
            var query = (from c in consultas.Db.Contacto
                         where c.Id_Contacto == Int32.Parse(e.CommandArgument.ToString())
                         select c);
            foreach (Contacto c in query)
            {
                c.Inactivo = true;
            }
            consultas.Db.SubmitChanges();
            RequiredFieldValidator1.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
            RequiredFieldValidator4.Enabled = false;
            RequiredFieldValidator8.Enabled = false;
            RequiredFieldValidator9.Enabled = false;
            RegularExpressionValidator1fax.Enabled = false;
            gv_administradores.DataBind();
        }

        private bool aplicaPerfilOperador(string[] _grupos)
        {
            bool aplica = false;

            if (_grupos.Contains(Constantes.CONST_PerfilAbogado.ToString())
                    || _grupos.Contains(Constantes.CONST_GerenteEvaluador.ToString())
                    || _grupos.Contains(Constantes.CONST_PerfilAcreditador.ToString())
                    || _grupos.Contains(Constantes.CONST_GerenteInterventor.ToString())
                    || _grupos.Contains(Constantes.CONST_PerfilFiduciaria.ToString())
                    || _grupos.Contains(Constantes.CONST_CallCenterOperador.ToString()))
            {
                aplica = true;
            }

            return aplica;
        }

        /// <summary>
        /// Handles the onclick event of the btn_crearActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_crearActualizar_onclick(object sender, EventArgs e)
        {
            int codigoContacto = 0;
            grupos1 = Request.QueryString["codGrupo"];
            if (!String.IsNullOrEmpty(Request.QueryString["CodGrupo"]))
            {
                grupos1 = Request.QueryString["codGrupo"];
                grupos = grupos1.Split(',');
                Menu_Data(grupos1);
            }
            if (!String.IsNullOrEmpty(Request.QueryString["CodContacto"]))
            {
                codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);
            }
            try
            {
                int? codOperador = null;
                if (aplicaPerfilOperador(grupos))
                {
                    codOperador = Convert.ToInt32(ddlOperador.SelectedValue);
                }


                string emailContacto = tb_email.Text;

                var ijn = false;
                Del d = delegate (string idntf)
                {
                    ijn = new Consultas().Db
                                  .Contacto
                                  .Where(c => (c.Identificacion == Int64.Parse(idntf)
                                          && c.codOperador.Equals(codOperador))
                                          )
                                  .ToList()
                                  .Count > 0;
                };

                d.Invoke(tb_no.Text.Replace(".", ""));
                if (ijn && Equals(Request.QueryString["Accion"].ToString(), "Crear"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                        "qwe", "alert('Ya existe un usuario con el mismo numero de documento')", true); return;
                }
                string accion = Request.QueryString["Accion"].ToString();
                switch (accion)
                {
                    case "Crear":
                        void_CrearDatos(grupos, codigoContacto, codOperador);
                        break;
                    case "Editar":
                        void_ModificarDatos(grupos, codigoContacto, codOperador);
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                Lbl_Resultados.Text = "Error :: Por favor revise tipos de datos y  longitudes . Entrada No Valida! detalle :" + ex.Message;
                return;
            }
        }

        /// <summary>
        /// Handles the RowCreated event of the gv_administradores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_administradores_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        LinkButton lnk = (LinkButton)tc.Controls[0];
                        if (lnk != null && gv_administradores.SortExpression == lnk.CommandArgument)
                        {
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            img.ImageUrl = "/Images/ImgFlechaOrden" + (gv_administradores.SortDirection == SortDirection.Ascending ? "Up" : "Down") + ".gif";
                            tc.Controls.Add(new LiteralControl(" "));
                            tc.Controls.Add(img);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 27/06/2014.
        /// De acuerdo a la pantalla activa y el usuario a crear, redirigir al usuario según el rol.
        /// </summary>
        private string Menu_Data(string valor)
        {
            try
            {
                if (valor == "8") //Call Center
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=8";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=8";
                }
                if (valor == "2") //Administrador
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2,3";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
                }
                if (valor == "12") //Gerente Interventor
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=12";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=12";
                }
                if (valor == "9") //Gerente Evaluador
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=9";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=9";
                }
                if (valor == "15") //Perfil fiduciaria
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=15";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=15";
                }
                if (valor == "16") //Lider Regional
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=16";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=16";
                }
                if (valor == "18") //Abogado
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=18";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=18";
                }
                if (valor == "19") //Acreditador
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=19";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=19";
                }
                if (valor == "20") //Call Center Operador
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=20";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=20";
                }
                else
                {
                    HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
                    return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
                }
            }
            catch
            {
                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
            }
        }

        /// <summary>
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_administradores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "eliminar":
                    #region Eliminar usuario seleccionado.
                    try
                    {
                        GrupoContacto Grupocontacto = new GrupoContacto()
                        {
                            CodGrupo = Int32.Parse(ddl_perfil.SelectedValue),
                            CodContacto = Int32.Parse(e.CommandArgument.ToString())
                        };
                        consultas.Db.GrupoContactos.Attach(Grupocontacto);
                        consultas.Db.GrupoContactos.DeleteOnSubmit(Grupocontacto);
                        consultas.Db.SubmitChanges();
                        var query = (from c in consultas.Db.Contacto
                                     where c.Id_Contacto == Int32.Parse(e.CommandArgument.ToString())
                                     select c);
                        foreach (Contacto c in query)
                        {
                            c.Inactivo = true;
                        }
                        consultas.Db.SubmitChanges();
                        RequiredFieldValidator1.Enabled = false;
                        RequiredFieldValidator2.Enabled = false;
                        RequiredFieldValidator3.Enabled = false;
                        RequiredFieldValidator4.Enabled = false;
                        RequiredFieldValidator8.Enabled = false;
                        RequiredFieldValidator9.Enabled = false;
                        RegularExpressionValidator1fax.Enabled = false;
                        gv_administradores.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Usuario eliminado correctamente.')", true);
                    }
                    catch
                    { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo eliminar el usuario seleccionado.')", true); }

                    #endregion
                    break;
                default:
                    break;
            }
        }

        private Contacto VerificarLiderRegional(int idRegional)
        {
            var lider = (from c in consultas.Db.Contacto
                         join gc in consultas.Db.GrupoContactos on c.Id_Contacto equals gc.CodContacto
                         where c.CodTipoAprendiz == idRegional && c.InactivoAsignacion == false && gc.CodGrupo == Constantes.CONST_LiderRegional
                         select c).FirstOrDefault();
            return lider;
        }

        /// <summary>
        /// Nombres the regional.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string NombreRegional(string id)
        {
            if (id != "0")
            {
                var regional = (from d in consultas.Db.departamento
                                where d.Id_Departamento == int.Parse(id)
                                select d).FirstOrDefault();
                if (regional != null)
                {
                    return regional.NomDepartamento;
                }
                else
                {
                    return "";
                }
            }
            else
                return "";

        }
    }

    /// <summary>
    /// users
    /// </summary>
    public class users
    {
        /// <summary>
        /// Gets or sets the identifier contacto.
        /// </summary>
        /// <value>
        /// The identifier contacto.
        /// </value>
        public int Id_contacto { get; set; }
        /// <summary>
        /// Gets or sets the nombres.
        /// </summary>
        /// <value>
        /// The nombres.
        /// </value>
        public string Nombres { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the codgrupo.
        /// </summary>
        /// <value>
        /// The codgrupo.
        /// </value>
        public int codgrupo { get; set; }
        /// <summary>
        /// Gets or sets the nomgrupo.
        /// </summary>
        /// <value>
        /// The nomgrupo.
        /// </value>
        public string nomgrupo { get; set; }
        /// <summary>
        /// Gets or sets the IDOPERADOR.
        /// </summary>
        /// <value>
        /// The IDOPERADOR.
        /// </value>
        public int? idOperador { get; set; }
        /// <summary>
        /// Gets or sets the operador.
        /// </summary>
        /// <value>
        /// The operador.
        /// </value>
        public string operador { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [estado lider].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [estado lider]; otherwise, <c>false</c>.
        /// </value>
        public bool estadoLider { get; set; }
        /// <summary>
        /// Gets or sets the regional.
        /// </summary>
        /// <value>
        /// The regional.
        /// </value>
        public int regional { get; set; }
    }

    #region AntiguoCODE
    /// <summary>
    /// AdministrarUsuarios
    /// </summary>    
    //public partial class AdministrarUsuarios : Negocio.Base_Page
    //{
    //    string crearAdmin = "Crear Administrador";
    //    string crearGerenteEvaluador = "Crear GerenteEvaluador";
    //    string crearCallCenter = "Crear Call Center";
    //    string crearGerenteInterventor = "Crear Gerente Interventor";
    //    string crearPerfilFiduciario = "Crear Perfil Fiduciario";
    //    string crearLiderRegionarl = "Crear Líder Regioal";
    //    string crearAcreditador = "Crear Acreditador";
    //    string crearAbogado = "Crear Abogado";
    //    string crearCallCenterOperador = "Crear Call Center Operador";
    //    String grupos1;
    //    public String[] grupos = { "0" };
    //    int idusuario;
    //    string[] codgrupousuario;
    //    delegate void Del(string idn);
    //    // <summary>
    //    // Variable que contiene la URL actual.
    //    // </summary>
    //    //string var_url;

    //    /// <summary>
    //    /// Voids the modificar datos.
    //    /// </summary>
    //    /// <param name="codgrupo">The codgrupo.</param>
    //    /// <param name="Grupocontacto">The grupocontacto.</param>
    //    /// <exception cref="ApplicationException">el tamaño del numero de identificación supera el limite aceptado</exception>
    //    protected void void_ModificarDatos(string[] codgrupo, int Grupocontacto, int? _codOperador)
    //    {
    //        if (Request.QueryString["CodContacto"] != null)
    //        {
    //            int codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);


    //            var query = (from c in consultas.Db.Contacto
    //                         where c.Id_Contacto == codigoContacto
    //                         select c);
    //            Contacto cnt = null;
    //            foreach (Contacto c in query)
    //            {
    //                c.Apellidos = tb_apelidos.Text;
    //                c.Cargo = tb_cargo.Text;
    //                c.Email = tb_email.Text;
    //                c.Fax = tb_fax.Text;

    //                var verificarCedula = Int64.Parse(tb_no.Text.Replace(".", ""));
    //                if (verificarCedula >= Int32.MaxValue)
    //                    throw new ApplicationException(" el tamaño del numero de identificación supera el limite aceptado");

    //                c.Identificacion = Int32.Parse(tb_no.Text.Replace(".", ""));
    //                c.Nombres = tb_nombres.Text;
    //                c.Telefono = tb_telefono.Text;
    //                c.CodTipoIdentificacion = short.Parse(ddl_identificacion.SelectedValue);
    //                c.codOperador = _codOperador;
    //                cnt = c;
    //                if (Session["grupoLider"] != null)
    //                {
    //                    c.CodTipoAprendiz = int.Parse(ddlRegionales.SelectedValue);
    //                    if (ddlEstadoLider.SelectedValue == "0")
    //                    {
    //                        c.InactivoAsignacion = false;
    //                    }
    //                    else
    //                    {
    //                        c.InactivoAsignacion = true;
    //                    }
    //                }

    //                consultas.Db.SubmitChanges();
    //            }
    //            try
    //            {
    //                consultas.Db.ExecuteCommand(UsuarioActual());
    //                consultas.Db.SubmitChanges();
    //                new Clases.Correo(System.Configuration.ConfigurationManager.AppSettings["Email"], System.Configuration.ConfigurationManager.AppSettings["WebSite"], cnt.Email, string.Format("{0} {1}", cnt.Nombres, cnt.Apellidos),
    //                  "Registro de usuario FONADE", string.Format("Correo de prueba notificaión de registro {2} Usuario: {0} {2} Clave: {1} ", cnt.Email, cnt.Clave, "\n")).Enviar();
    //                void_show("La información del usuario ha sido actualizado correctamente", true);
    //            }
    //            catch (Exception ex) { }
    //        }
    //    }

    //    /// <summary>
    //    /// Voids the crear datos.
    //    /// </summary>
    //    /// <param name="codgrupo">The codgrupo.</param>
    //    /// <param name="codusuario">The codusuario.</param>
    //    /// <exception cref="ApplicationException">el tamaño del numero de identificación supera el limite aceptado</exception>
    //    protected void void_CrearDatos(string[] codgrupo, int codusuario, int? _codOperador = null)
    //    {
    //        try
    //        {
    //            string nuevaclave;
    //            nuevaclave = GeneraClave();
    //            String wclave = "";

    //            var verificarCedula = Int64.Parse(tb_no.Text.Replace(".", ""));
    //            if (verificarCedula >= Int32.MaxValue)
    //                throw new ApplicationException(" el tamaño del numero de identificación supera el limite aceptado");

    //            Contacto contacto = new Contacto()
    //            {
    //                Apellidos = tb_apelidos.Text,
    //                Cargo = tb_cargo.Text,
    //                Email = tb_email.Text,
    //                Fax = tb_fax.Text,
    //                Identificacion = Int64.Parse(tb_no.Text.Replace(".", "")),
    //                Nombres = tb_nombres.Text,
    //                Clave = Utilidades.Encrypt.GetSHA256(nuevaclave),
    //                //Clave = nuevaclave,
    //                Telefono = tb_telefono.Text,
    //                CodTipoIdentificacion = short.Parse(ddl_identificacion.SelectedValue),
    //                codOperador = _codOperador,
    //                fechaCambioClave = DateTime.Now
    //            };

    //            //Si se va a guardar los datos de un lider regional, el Id de la regional sera guardada en la
    //            //columna CodTipoAprendiz de la tabla contacto. Esto para no alterar o crear nuevas tablas
    //            // Igualmente en la columna InactivoAsignacion para almacenar el estado activo / inactivo del rol Lider regional
    //            if (ddl_perfil.SelectedValue == Constantes.CONST_LiderRegional.ToString())
    //            {
    //                //Valida si la regional tiene lider asignado
    //                var lider = VerificarLiderRegional(int.Parse(ddlRegionales.SelectedValue));
    //                if (lider == null)
    //                {
    //                    contacto.CodTipoAprendiz = int.Parse(ddlRegionales.SelectedValue);
    //                    if (ddlEstadoLider.SelectedValue == "0")
    //                    {
    //                        contacto.InactivoAsignacion = false;
    //                    }
    //                    else
    //                    {
    //                        contacto.InactivoAsignacion = true;
    //                    }
    //                }
    //                else
    //                {
    //                    void_show("La regional " + ddlRegionales.SelectedItem + " ya tiene lider regional asignado: " + lider.Nombres + " " + lider.Apellidos + ". Desactivelo primero.", true);
    //                    return;
    //                }
    //            }
    //            GrupoContacto Grupocontacto = new GrupoContacto()
    //            {
    //                CodGrupo = Int32.Parse(ddl_perfil.SelectedValue),
    //                CodContacto = contacto.Id_Contacto
    //            };
    //            //enviar datos 
    //            try
    //            {
    //                var vaidarcontacto = (from c1 in consultas.Db.Contacto
    //                                      where c1.Email == contacto.Email
    //                                      select c1).FirstOrDefault();

    //                if (vaidarcontacto == null)
    //                {

    //                    //*************************************AQUI MAIL WPLAZAS JUNIO 2-2015 *********************************//

    //                    String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
    //                    bool Enviado = false; //Variable que determina si el mensaje fue enviado o no "como resultado de la re-activación".
    //                    bool correcto = false;
    //                    Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");
    //                    //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
    //                    string wrol = "";

    //                    if (Request.QueryString["accion"] == "Crear")
    //                    {
    //                        switch (Request.QueryString["codGrupo"])
    //                        {
    //                            case "8":
    //                                wrol = "Call Center";
    //                                break;
    //                            case "12":
    //                                wrol = "Gerente Interventor";
    //                                break;
    //                            case "9":
    //                                wrol = "Gerente Evaluador";
    //                                break;
    //                            case "2":
    //                                wrol = "Administrador";
    //                                break;
    //                            case "3":
    //                                wrol = "Administrador";
    //                                break;
    //                            case "2,3":
    //                                wrol = "Administrador";
    //                                break;
    //                            case "15":
    //                                wrol = "Fiducia";
    //                                break;
    //                            case "16":
    //                                wrol = "Lider Regional";
    //                                break;
    //                            case "18":
    //                                wrol = "Abogado";
    //                                break;
    //                            case "19":
    //                                wrol = "Acreditador";
    //                                break;
    //                            case "20":
    //                                wrol = "Call Center Operador";
    //                                break;

    //                        }


    //                        consultas.Db.Contacto.InsertOnSubmit(contacto);
    //                        consultas.Db.SubmitChanges();
    //                        var nuevocontacto = (from c in consultas.Db.Contacto
    //                                             where c.Email == contacto.Email
    //                                             select c).FirstOrDefault();
    //                        Grupocontacto.CodContacto = nuevocontacto.Id_Contacto;
    //                        consultas.Db.GrupoContactos.InsertOnSubmit(Grupocontacto);
    //                        consultas.Db.SubmitChanges();


    //                        wclave = nuevaclave;
    //                        Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", wrol);
    //                        Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", this.tb_email.Text);
    //                        Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", wclave);
    //                        Texto_Obtenido = Texto_Obtenido.Replace("{{nombre}}", contacto.Nombres.ToUpper() + " " + contacto.Apellidos.ToUpper());

    //                        try
    //                        {
    //                            //Generar y enviar mensaje.
    //                            Correo correo = new Correo(usuario.Email,
    //                                                       "Fondo Emprender",
    //                                                       tb_email.Text.ToString(),
    //                                                       this.tb_nombres.Text.Trim() + " " + this.tb_apelidos.Text.Trim(),
    //                                                       "Generación Mail " + wrol.ToString(),
    //                                                       Texto_Obtenido);
    //                            correo.Enviar();

    //                            //El mensaje fue enviado.
    //                            Enviado = true;

    //                            void_show("el usuario " + tb_email.Text.ToString() + " ha sido agregado", true);

    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);


    //                        }
    //                    }
    //                    //*************************************AQUI FIN MAIL WPLAZAS JUNIO 2-2015 *********************************//

    //                    //new Clases.Correo(System.Configuration.ConfigurationManager.AppSettings["Email"] ,System.Configuration.ConfigurationManager.AppSettings["WebSite"],contacto.Email, string.Format("{0} {1}", contacto.Nombres, contacto.Apellidos),
    //                    //  "Registro de usuario FONADE", string.Format("Correo de prueba notificaión de registro {2} Usuario: {0} {2} Clave: {1} ", contacto.Email, contacto.Clave, "\n")).Enviar();
    //                    //void_show("el usuario " + nuevocontacto.Email + " ha sido agregado", true);                             
    //                }
    //                else
    //                {
    //                    void_show("el usuario " + vaidarcontacto.Email + " ya existe intente nuevamente", true);

    //                }

    //            }
    //            catch (Exception ex)
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);


    //        }
    //    }

    //    /// <summary>
    //    /// Voids the show.
    //    /// </summary>
    //    /// <param name="texto">The texto.</param>
    //    /// <param name="mostrar">if set to <c>true</c> [mostrar].</param>
    //    protected void void_show(string texto, bool mostrar)
    //    {
    //        //lbl_popup.Visible = mostrar;           
    //        //lbl_popup.Text = texto;
    //        //mpe1.Enabled = mostrar;
    //        //mpe1.Show();
    //        string queryRedir = Request.Url.Query;

    //        queryRedir = queryRedir.Substring(0, queryRedir.IndexOf('&'));

    //        string redirePagina = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath, queryRedir);

    //        ClientScriptManager cm = this.ClientScript;
    //        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('" + texto + "');location.href='" + redirePagina + "'</script>");
    //    }

    //    private void cargarDllOperador()
    //    {
    //        var operadores = operadorController.getAllOperador();

    //        ddlOperador.DataTextField = "NombreOperador";
    //        ddlOperador.DataValueField = "idOperador";
    //        ddlOperador.DataSource = operadores;
    //        ddlOperador.DataBind();
    //    }

    //    /// <summary>
    //    /// Voids the hide controls.
    //    /// </summary>
    //    /// <param name="accion">The accion.</param>
    //    /// <param name="codgrupo">The codgrupo.</param>
    //    protected void void_HideControls(string accion, string[] codgrupo)
    //    {
    //        //Mostrar combo Operador
    //        if (aplicaPerfilOperador(codgrupo))
    //        {
    //            //lblOperador.Visible = true;
    //            //ddlOperador.Visible = true;
    //            tbl_infousuario.Rows[8].Visible = true;

    //            cargarDllOperador();
    //        }
    //        else
    //        {
    //            tbl_infousuario.Rows[8].Visible = false;
    //        }

    //        switch (accion)
    //        {
    //            case "Crear":
    //                tbl_infousuario.Rows[3].Visible = false;
    //                if (codgrupo.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
    //                    codgrupo.Contains(Constantes.CONST_AdministradorSena.ToString()) || codgrupo.Equals("" + Constantes.CONST_AdministradorSistema.ToString() + "," + Constantes.CONST_AdministradorSena.ToString()))
    //                {
    //                    tbl_infousuario.Rows[6].Visible = true;
    //                }
    //                else
    //                {
    //                    tbl_infousuario.Rows[6].Visible = false;
    //                };
    //                tbl_infousuario.Rows[5].Visible = false;

    //                if (codgrupo.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
    //                    codgrupo.Contains(Constantes.CONST_AdministradorSena.ToString()) || codgrupo.Equals("" + Constantes.CONST_AdministradorSistema.ToString() + "," + Constantes.CONST_AdministradorSena.ToString()))
    //                {
    //                    tbl_infousuario.Rows[5].Visible = true;
    //                }



    //                btn_crearActualizar.Text = "Crear";
    //                break;
    //            case "Editar":
    //                tbl_infousuario.Rows[3].Visible = false;
    //                if (codgrupo.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
    //                    codgrupo.Contains(Constantes.CONST_AdministradorSena.ToString()))
    //                {
    //                    tbl_infousuario.Rows[6].Visible = false;
    //                    Page.Header.Controls.Add(
    //                    new LiteralControl(
    //                        @"<style type='text/css'>
    //                                select#bodyContentPlace_ddl_perfil option:nth-child(n+4)
    //                                {
    //                                    display : none !important;
    //                                }
    //                                select#bodyContentPlace_ddl_perfil option:nth-child(1)
    //                                {
    //                                    display : none !important;
    //                                }
    //                        </style>
    //                        <script>
    //                            window.onload = function list() {
    //                               document.getElementById('bodyContentPlace_ddl_perfil').selectedIndex = '1';
    //                            }
    //                        </script>
    //                        "
    //                    ));
    //                }
    //                else
    //                {
    //                    tbl_infousuario.Rows[6].Visible = false;
    //                };
    //                tbl_infousuario.Rows[5].Visible = false;

    //                //Seleccion de operador
    //                tbl_infousuario.Rows[8].Visible = false;

    //                btn_crearActualizar.Text = "Actualizar";

    //                break;
    //            default: break;
    //        }

    //    }

    //    /// <summary>
    //    /// Voids the traer datos.
    //    /// </summary>
    //    /// <param name="codgrupo">The codgrupo.</param>
    //    /// <param name="codusuario">The codusuario.</param>
    //    protected void void_traerDatos(string[] codgrupo, int codusuario)
    //    {
    //        var tipoidentificación = (from ti in consultas.Db.TipoIdentificacions
    //                                  select new
    //                                  {
    //                                      Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
    //                                      NomTipoIdentificacion = ti.NomTipoIdentificacion,

    //                                  });
    //        var Roles = (from r in consultas.Db.Grupos
    //                     select new
    //                     {
    //                         Rol = r.NomGrupo,
    //                         codRol = r.Id_Grupo,
    //                     });
    //        if (codgrupo != null)
    //        {
    //            Roles.Where(r => codgrupo.Contains(r.codRol.ToString()));
    //        }

    //        ddl_identificacion.DataTextField = "NomTipoIdentificacion";
    //        ddl_identificacion.DataValueField = "Id_TipoIdentificacion";
    //        ddl_identificacion.DataSource = tipoidentificación;
    //        ddl_identificacion.DataBind();
    //        ddl_perfil.DataTextField = "Rol";
    //        ddl_perfil.DataValueField = "codRol";
    //        ddl_perfil.DataSource = Roles;
    //        ddl_perfil.DataBind();

    //        var contacto = (from c in consultas.Db.Contacto
    //                        from gc in consultas.Db.GrupoContactos
    //                        where c.Id_Contacto == codusuario & c.Id_Contacto == gc.CodContacto
    //                        select new
    //                        {
    //                            nombres = c.Nombres,
    //                            apellidos = c.Apellidos,
    //                            codtipoidentificacion = c.CodTipoIdentificacion,
    //                            Tipoidentificacion = c.TipoIdentificacion,
    //                            identificacion = c.Identificacion,
    //                            cargo = c.Cargo,
    //                            email = c.Email,
    //                            telefono = c.Telefono,
    //                            fax = c.Fax,
    //                            codgrupo = gc.CodGrupo,
    //                            NomGrupo = gc.Grupo,
    //                            EstadoLider = c.CodTipoAprendiz,
    //                            codOperador = c.codOperador
    //                        }).FirstOrDefault();

    //        tb_apelidos.Text = contacto.apellidos;
    //        tb_cargo.Text = contacto.cargo;
    //        tb_email.Text = contacto.email;
    //        tb_fax.Text = contacto.fax;
    //        tb_no.Text = contacto.identificacion.ToString();
    //        tb_nombres.Text = contacto.nombres;
    //        tb_telefono.Text = contacto.telefono;
    //        ddl_identificacion.SelectedValue = contacto.codtipoidentificacion.ToString();

    //        string[] cadenaGrupo = new string[1];
    //        cadenaGrupo[0] = contacto.codgrupo.ToString();

    //        if (aplicaPerfilOperador(cadenaGrupo))
    //        {
    //            ddlOperador.SelectedValue = contacto.codOperador.ToString();
    //        }

    //        if (contacto.codgrupo.ToString() == Constantes.CONST_LiderRegional.ToString())
    //        {
    //            rowEstado.Visible = true;

    //        }
    //    }

    //    /// <summary>
    //    /// Voids the traer datos rol.
    //    /// </summary>
    //    /// <param name="codgrupo">The codgrupo.</param>
    //    /// <param name="codusuario">The codusuario.</param>
    //    protected void void_traerDatosRol(string[] codgrupo, int codusuario)
    //    {
    //        var tipoidentificación = (from ti in consultas.Db.TipoIdentificacions
    //                                  select new
    //                                  {
    //                                      Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
    //                                      NomTipoIdentificacion = ti.NomTipoIdentificacion,
    //                                  });
    //        var Roles = (from g in consultas.Db.Grupos
    //                     where codgrupo.Contains(g.Id_Grupo.ToString())
    //                     select new
    //                     {
    //                         Rol = g.NomGrupo,
    //                         codRol = g.Id_Grupo,
    //                     });
    //        ddl_identificacion.DataTextField = "NomTipoIdentificacion";
    //        ddl_identificacion.DataValueField = "Id_TipoIdentificacion";
    //        ddl_identificacion.DataSource = tipoidentificación;
    //        ddl_identificacion.DataBind();
    //        ddl_perfil.DataTextField = "Rol";
    //        ddl_perfil.DataValueField = "codRol";
    //        ddl_perfil.DataSource = Roles;
    //        ddl_perfil.DataBind();
    //        var contacto = (from c in consultas.Db.Contacto
    //                        from gc in consultas.Db.GrupoContactos
    //                        where c.Id_Contacto == codusuario & c.Id_Contacto == gc.CodContacto
    //                        select new
    //                        {
    //                            nombres = c.Nombres,
    //                            apellidos = c.Apellidos,
    //                            codtipoidentificacion = c.CodTipoIdentificacion,
    //                            Tipoidentificacion = c.TipoIdentificacion,
    //                            identificacion = c.Identificacion,
    //                            cargo = c.Cargo,
    //                            email = c.Email,
    //                            telefono = c.Telefono,
    //                            fax = c.Fax,
    //                            codgrupo = gc.CodGrupo,
    //                            NomGrupo = gc.Grupo,
    //                            idContacto = c.Id_Contacto,
    //                            codAprendiz = c.CodTipoAprendiz,
    //                            CodEstadoLider = c.InactivoAsignacion,
    //                            codOperador = c.codOperador
    //                        }).FirstOrDefault();

    //        if (grupos.Contains(Constantes.CONST_LiderRegional.ToString()))
    //        {
    //            Session["grupoLider"] = Constantes.CONST_LiderRegional.ToString();
    //            var regionales = (from r in consultas.Db.departamento
    //                              select r).ToList();
    //            ddlRegionales.DataSource = regionales;
    //            ddlRegionales.DataTextField = "NomDepartamento";
    //            ddlRegionales.DataValueField = "Id_Departamento";
    //            ddlRegionales.DataBind();
    //            ddlRegionales.Items.Insert(0, new ListItem("Seleccione", "0"));
    //            cellblRegional.Visible = true;
    //            cellddlRegional.Visible = true;
    //            rowEstado.Visible = true;
    //        }
    //        else
    //        {
    //            cellblRegional.Visible = false;
    //            cellddlRegional.Visible = false;
    //            rowEstado.Visible = false;
    //        }

    //        if (contacto != null)
    //        {
    //            var grupo = (from g in consultas.Db.GrupoContactos
    //                         where g.CodContacto == contacto.idContacto
    //                         select g).FirstOrDefault();
    //            if (grupo != null)
    //            {
    //                if (grupo.CodGrupo.ToString() == Constantes.CONST_LiderRegional.ToString())
    //                {
    //                    var regionales = (from r in consultas.Db.departamento
    //                                      select r).ToList();
    //                    ddlRegionales.DataSource = regionales;
    //                    ddlRegionales.DataTextField = "NomDepartamento";
    //                    ddlRegionales.DataValueField = "Id_Departamento";
    //                    ddlRegionales.DataBind();
    //                    ddlRegionales.Items.Insert(0, new ListItem("Seleccione", "0"));
    //                    cellblRegional.Visible = true;
    //                    cellddlRegional.Visible = true;

    //                    cellblRegional.Visible = true;
    //                    cellddlRegional.Visible = true;
    //                    ddlRegionales.SelectedValue = contacto.codAprendiz.ToString();

    //                    ddlEstadoLider.Items.Insert(0, new ListItem("Activo", "0"));
    //                    ddlEstadoLider.Items.Insert(1, new ListItem("Inactivo", "1"));
    //                    if (contacto.CodEstadoLider.ToString() == "True")
    //                    {
    //                        ddlEstadoLider.SelectedValue = "1";
    //                    }
    //                    else
    //                    {
    //                        ddlEstadoLider.SelectedValue = "0";
    //                    }
    //                    ddlEstadoLider.Visible = true;
    //                }
    //            }
    //        }

    //    }
    //    /// <summary>
    //    /// Voids the obtener parametros.
    //    /// </summary>
    //    protected void void_ObtenerParametros()
    //    {
    //        try
    //        {
    //            if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
    //            {
    //                grupos1 = Request.QueryString["codGrupo"];
    //                grupos = grupos1.Split(',');
    //            }
    //            if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
    //            {
    //                pnl_Administradores.Visible = false;
    //                pnl_crearEditar.Visible = true;
    //                AgregarUsuario.Visible = false;
    //                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
    //                {
    //                    if (Request.QueryString["Accion"].ToString() == "Editar")
    //                    {
    //                        AgregarUsuario.Text = "Actualizar";
    //                        idusuario = Int32.Parse(Request.QueryString["CodContacto"]);
    //                        void_traerDatos(codgrupousuario, idusuario);
    //                    }
    //                    if (Request.QueryString["Accion"].ToString() == "Crear")
    //                    {
    //                        AgregarUsuario.Text = "Crear";

    //                        if (Request.QueryString["codGrupo"].ToString() == Constantes.CONST_LiderRegional.ToString())
    //                        {
    //                            ddlEstadoLider.Items.Insert(0, new ListItem("Activo", "0"));
    //                            ddlEstadoLider.Items.Insert(1, new ListItem("Inactivo", "1"));
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            #region Código que redirecciona al usuario de acuerdo a la URL determinada.

    //            //Instanciar la variable que contiene la URL.
    //            string val = "";
    //            val = HttpContext.Current.Session["URL_redirect"] != null ? val = HttpContext.Current.Session["URL_redirect"].ToString() : "";

    //            //Si NO contiene  URL
    //            if (val == "")
    //            { Response.Redirect("../MiPerfil/Home.aspx"); }
    //            else //De lo contrario, la usa.
    //            { Response.Redirect(val); }

    //            #endregion
    //        }
    //    }
    //    /// <summary>
    //    /// Redirecciona al usuario a la página correspondiente.
    //    /// </summary>        
    //    protected void void_ModificarTextolink()
    //    {
    //        if (grupos.Contains(Constantes.CONST_AdministradorSistema.ToString()) ||
    //           grupos.Contains(Constantes.CONST_AdministradorSena.ToString()))
    //        {
    //            AgregarUsuario.Text = crearAdmin;

    //        }
    //        if (grupos.Contains(Constantes.CONST_GerenteEvaluador.ToString()))
    //        {
    //            AgregarUsuario.Text = crearGerenteEvaluador;
    //        }
    //        if (grupos.Contains(Constantes.CONST_CallCenter.ToString()))
    //        {
    //            AgregarUsuario.Text = crearCallCenter;
    //        }
    //        if (grupos.Contains(Constantes.CONST_GerenteInterventor.ToString()))
    //        {
    //            AgregarUsuario.Text = crearGerenteInterventor;
    //        }
    //        if (grupos.Contains(Constantes.CONST_Perfil_Fiduciario.ToString()))
    //        {
    //            AgregarUsuario.Text = crearPerfilFiduciario;
    //        }
    //        if (grupos.Contains(Constantes.CONST_LiderRegional.ToString()))
    //        {
    //            AgregarUsuario.Text = crearLiderRegionarl;
    //        }
    //        if (grupos.Contains(Constantes.CONST_PerfilAcreditador.ToString()))
    //        {
    //            AgregarUsuario.Text = crearAcreditador;
    //        }
    //        if (grupos.Contains(Constantes.CONST_PerfilAbogado.ToString()))
    //        {
    //            AgregarUsuario.Text = crearAbogado;
    //        }
    //        if (grupos.Contains(Constantes.CONST_CallCenterOperador.ToString()))
    //        {
    //            AgregarUsuario.Text = crearCallCenterOperador;
    //        }

    //        string url = Request.Url.GetLeftPart(UriPartial.Path);
    //        url += (Request.QueryString.ToString() == "") ? "?Accion=Crear" : "?" + Request.QueryString.ToString() + "&Accion=Crear";
    //        AgregarUsuario.NavigateUrl = url;
    //    }

    //    public void ValidateUsers()
    //    {
    //        try
    //        {
    //            if (!(usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
    //            {
    //                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
    //        }
    //    }

    //    private void ocultarColumnas(int _codGrupo)
    //    {
    //        if (_codGrupo == Constantes.CONST_AdministradorSistema)
    //        {
    //            gv_administradores.Columns[4].Visible = false;
    //            gv_administradores.Columns[5].Visible = false;
    //        }

    //        string[] cadenaGrupo = new string[1];
    //        cadenaGrupo[0] = _codGrupo.ToString();

    //        if (aplicaPerfilOperador(cadenaGrupo))
    //        {
    //            gv_administradores.Columns[3].Visible = false;
    //            gv_administradores.Columns[5].Visible = false;
    //        }
    //    }

    //    /// <summary>
    //    /// Handles the Load event of the Page control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        if (!IsPostBack)
    //        {
    //            ValidateUsers();

    //            string Accion = "";
    //            string codigoGrupo = "0";
    //            int codigoContacto = 0;
    //            if (!String.IsNullOrEmpty(Request.QueryString["CodGrupo"]))
    //            {
    //                codigoGrupo = Request.QueryString["CodGrupo"];
    //                grupos1 = Request.QueryString["codGrupo"];
    //                grupos = grupos1.Split(',');
    //            }
    //            if (!String.IsNullOrEmpty(Request.QueryString["CodContacto"]))
    //            {
    //                codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);
    //            }
    //            if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
    //            {
    //                Accion = Request.QueryString["Accion"].ToString();
    //                void_HideControls(Accion, grupos);
    //            };
    //            void_traerDatosRol(grupos, codigoContacto);
    //            pnl_crearEditar.Visible = false;
    //            string url = Request.Url.GetLeftPart(UriPartial.Path);
    //            url += (Request.QueryString.ToString() == "") ? "?Accion=Crear" : "?" + Request.QueryString.ToString() + "&Accion=Crear";
    //            AgregarUsuario.NavigateUrl = url;
    //            void_ObtenerParametros();
    //            lbl_Titulo.Text = void_establecerTitulo(grupos, Accion, "Texto");
    //            void_ModificarTextolink();
    //        }

    //    }

    //    /// <summary>
    //    /// Handles the DataBound event of the gv_administradores control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    //    protected void gv_administradores_DataBound(object sender, EventArgs e)
    //    {
    //        String[] grupos2 = { "0" };
    //        String grupos3;
    //        if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
    //        {
    //            grupos3 = Request.QueryString["codGrupo"];
    //            grupos2 = grupos1.Split(',');
    //            if (grupos2.Contains(Constantes.CONST_AdministradorSistema.ToString()) | grupos2.Contains(Constantes.CONST_AdministradorSena.ToString()))
    //            {
    //                gv_administradores.Columns[3].Visible = true;
    //            }
    //            else
    //            {
    //                gv_administradores.Columns[3].Visible = false;
    //            }
    //        }

    //    }

    //    /// <summary>
    //    /// Handles the RowDataBound event of the gv_administradores control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
    //    protected void gv_administradores_RowDataBound(object sender, GridViewRowEventArgs e)
    //    {
    //        //if(gv_administradores.Rows.Count > 0)
    //        //{
    //        //    HyperLink nombreProyecto = new HyperLink();
    //        //    nombreProyecto = ((HyperLink)e.Row.Cells[1].FindControl("hl_nombre"));
    //        //    if (e.Row.RowType == DataControlRowType.DataRow)
    //        //    {
    //        //        if (!String.IsNullOrEmpty(Request.QueryString["codGrupo"]))
    //        //            nombreProyecto.NavigateUrl = nombreProyecto.NavigateUrl + "&codGrupo=" + Request.QueryString["codGrupo"].ToString();
    //        //    }
    //        //}
    //    }

    //    /// <summary>
    //    /// Handles the PageIndexChanged event of the gv_administradores control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
    //    protected void gv_administradores_PageIndexChanged(object sender, GridViewPageEventArgs e)
    //    {

    //    }

    //    OperadorController operadorController = new OperadorController();

    //    /// <summary>
    //    /// Handles the Selecting event of the lds_Administradores control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
    //    protected void lds_Administradores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    //    {
    //        if (!String.IsNullOrEmpty(Request.QueryString["CodGrupo"]))
    //        {
    //            grupos1 = Request.QueryString["codGrupo"];
    //            grupos = grupos1.Split(',');
    //        }

    //        IList<users> usuarios;

    //        if (!grupos.Contains(Constantes.CONST_LiderRegional.ToString()))
    //        {
    //            usuarios = (from c in consultas.Db.Contacto
    //                        from gc in consultas.Db.GrupoContactos
    //                        from g in consultas.Db.Grupos
    //                        orderby c.Nombres ascending
    //                        where c.Inactivo == false & c.Id_Contacto == gc.CodContacto &
    //                              g.Id_Grupo == gc.CodGrupo & grupos.Contains(gc.CodGrupo.ToString())
    //                        select new users
    //                        {
    //                            Id_contacto = c.Id_Contacto,
    //                            Nombres = c.Nombres + ' ' + c.Apellidos,
    //                            Email = c.Email,
    //                            codgrupo = gc.CodGrupo,
    //                            nomgrupo = g.NomGrupo,
    //                            regional = Convert.ToInt32(c.CodTipoAprendiz == null ? 0 : c.CodTipoAprendiz),
    //                            idOperador = c.codOperador
    //                        }).ToList();

    //            //Actualizar el campo operador
    //            if (aplicaPerfilOperador(grupos))
    //            {
    //                foreach (var u in usuarios)
    //                {
    //                    if (u.idOperador != null)
    //                        u.operador = operadorController.getOperador(u.idOperador).NombreOperador;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            usuarios = (from c in consultas.Db.Contacto
    //                        from gc in consultas.Db.GrupoContactos
    //                        from g in consultas.Db.Grupos
    //                        orderby c.Nombres ascending
    //                        where c.Inactivo == false & c.Id_Contacto == gc.CodContacto &
    //                              g.Id_Grupo == gc.CodGrupo & grupos.Contains(gc.CodGrupo.ToString()) &&
    //                              c.InactivoAsignacion == false
    //                        select new users
    //                        {
    //                            Id_contacto = c.Id_Contacto,
    //                            Nombres = c.Nombres + ' ' + c.Apellidos,
    //                            Email = c.Email,
    //                            codgrupo = gc.CodGrupo,
    //                            nomgrupo = g.NomGrupo,
    //                            regional = (int)c.CodTipoAprendiz
    //                        }).ToList();
    //        }

    //        e.Arguments.TotalRowCount = usuarios.Count();
    //        if (e.Arguments.TotalRowCount == 0)
    //        {
    //            Lbl_Resultados.Visible = true;
    //            Lbl_Resultados.Text = "No existen datos relacionados.";
    //        }
    //        else
    //        {
    //            Lbl_Resultados.Visible = false;
    //        }

    //        ocultarColumnas(Convert.ToInt32(grupos[0]));

    //        e.Result = usuarios.ToList();
    //    }

    //    /// <summary>
    //    /// Handles the click event of the btn_Inactivar control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
    //    protected void btn_Inactivar_click(object sender, CommandEventArgs e)
    //    {
    //        GrupoContacto Grupocontacto = new GrupoContacto()
    //        {
    //            CodGrupo = Int32.Parse(ddl_perfil.SelectedValue),
    //            CodContacto = Int32.Parse(e.CommandArgument.ToString())
    //        };
    //        consultas.Db.GrupoContactos.Attach(Grupocontacto);
    //        consultas.Db.GrupoContactos.DeleteOnSubmit(Grupocontacto);
    //        consultas.Db.SubmitChanges();
    //        var query = (from c in consultas.Db.Contacto
    //                     where c.Id_Contacto == Int32.Parse(e.CommandArgument.ToString())
    //                     select c);
    //        foreach (Contacto c in query)
    //        {
    //            c.Inactivo = true;
    //        }
    //        consultas.Db.SubmitChanges();
    //        RequiredFieldValidator1.Enabled = false;
    //        RequiredFieldValidator2.Enabled = false;
    //        RequiredFieldValidator3.Enabled = false;
    //        RequiredFieldValidator4.Enabled = false;
    //        RequiredFieldValidator8.Enabled = false;
    //        RequiredFieldValidator9.Enabled = false;
    //        RegularExpressionValidator1fax.Enabled = false;
    //        gv_administradores.DataBind();
    //    }

    //    private bool aplicaPerfilOperador(string[] _grupos)
    //    {
    //        bool aplica = false;

    //        if (_grupos.Contains(Constantes.CONST_PerfilAbogado.ToString())
    //                || _grupos.Contains(Constantes.CONST_GerenteEvaluador.ToString())
    //                || _grupos.Contains(Constantes.CONST_PerfilAcreditador.ToString())
    //                || _grupos.Contains(Constantes.CONST_GerenteInterventor.ToString())
    //                || _grupos.Contains(Constantes.CONST_PerfilFiduciaria.ToString())
    //                || _grupos.Contains(Constantes.CONST_CallCenterOperador.ToString()))
    //        {
    //            aplica = true;
    //        }

    //        return aplica;
    //    }

    //    /// <summary>
    //    /// Handles the onclick event of the btn_crearActualizar control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    //    protected void btn_crearActualizar_onclick(object sender, EventArgs e)
    //    {
    //        int codigoContacto = 0;
    //        grupos1 = Request.QueryString["codGrupo"];
    //        if (!String.IsNullOrEmpty(Request.QueryString["CodGrupo"]))
    //        {
    //            grupos1 = Request.QueryString["codGrupo"];
    //            grupos = grupos1.Split(',');
    //            Menu_Data(grupos1);
    //        }
    //        if (!String.IsNullOrEmpty(Request.QueryString["CodContacto"]))
    //        {
    //            codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);
    //        }
    //        try
    //        {
    //            int? codOperador = null;
    //            if (aplicaPerfilOperador(grupos))
    //            {
    //                codOperador = Convert.ToInt32(ddlOperador.SelectedValue);
    //            }


    //            string emailContacto = tb_email.Text;

    //            var ijn = false;
    //            Del d = delegate (string idntf)
    //            {
    //                ijn = new Consultas().Db
    //                              .Contacto
    //                              .Where(c => (c.Identificacion == Int64.Parse(idntf)
    //                                      && c.codOperador.Equals(codOperador))
    //                                      )
    //                              .ToList()
    //                              .Count > 0;
    //            };

    //            d.Invoke(tb_no.Text.Replace(".", ""));
    //            if (ijn && Equals(Request.QueryString["Accion"].ToString(), "Crear"))
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
    //                    "qwe", "alert('Ya existe un usuario con el mismo numero de documento')", true); return;
    //            }
    //            string accion = Request.QueryString["Accion"].ToString();
    //            switch (accion)
    //            {
    //                case "Crear":
    //                    void_CrearDatos(grupos, codigoContacto, codOperador);
    //                    break;
    //                case "Editar":
    //                    void_ModificarDatos(grupos, codigoContacto, codOperador);
    //                    break;
    //                default: break;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Lbl_Resultados.Text = "Error :: Por favor revise tipos de datos y  longitudes . Entrada No Valida! detalle :" + ex.Message;
    //            return;
    //        }
    //    }

    //    /// <summary>
    //    /// Handles the RowCreated event of the gv_administradores control.
    //    /// </summary>
    //    /// <param name="sender">The source of the event.</param>
    //    /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
    //    protected void gv_administradores_RowCreated(object sender, GridViewRowEventArgs e)
    //    {
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            foreach (TableCell tc in e.Row.Cells)
    //            {
    //                if (tc.HasControls())
    //                {
    //                    LinkButton lnk = (LinkButton)tc.Controls[0];
    //                    if (lnk != null && gv_administradores.SortExpression == lnk.CommandArgument)
    //                    {
    //                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
    //                        img.ImageUrl = "/Images/ImgFlechaOrden" + (gv_administradores.SortDirection == SortDirection.Ascending ? "Up" : "Down") + ".gif";
    //                        tc.Controls.Add(new LiteralControl(" "));
    //                        tc.Controls.Add(img);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Mauricio Arias Olave.
    //    /// 27/06/2014.
    //    /// De acuerdo a la pantalla activa y el usuario a crear, redirigir al usuario según el rol.
    //    /// </summary>
    //    private string Menu_Data(string valor)
    //    {
    //        try
    //        {
    //            if (valor == "8") //Call Center
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=8";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=8";
    //            }
    //            if (valor == "2") //Administrador
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2,3";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
    //            }
    //            if (valor == "12") //Gerente Interventor
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=12";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=12";
    //            }
    //            if (valor == "9") //Gerente Evaluador
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=9";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=9";
    //            }
    //            if (valor == "15") //Perfil fiduciaria
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=15";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=15";
    //            }
    //            if (valor == "16") //Lider Regional
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=16";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=16";
    //            }
    //            if (valor == "18") //Abogado
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=18";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=18";
    //            }
    //            if (valor == "19") //Acreditador
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=19";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=19";
    //            }
    //            if (valor == "20") //Call Center Operador
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=20";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=20";
    //            }
    //            else
    //            {
    //                HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
    //                return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
    //            }
    //        }
    //        catch
    //        {
    //            HttpContext.Current.Session["URL_redirect"] = "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
    //            return "~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=2";
    //        }
    //    }

    //    /// <summary>
    //    /// RowCommand.
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    protected void gv_administradores_RowCommand(object sender, GridViewCommandEventArgs e)
    //    {
    //        switch (e.CommandName)
    //        {
    //            case "eliminar":
    //                #region Eliminar usuario seleccionado.
    //                try
    //                {
    //                    GrupoContacto Grupocontacto = new GrupoContacto()
    //                    {
    //                        CodGrupo = Int32.Parse(ddl_perfil.SelectedValue),
    //                        CodContacto = Int32.Parse(e.CommandArgument.ToString())
    //                    };
    //                    consultas.Db.GrupoContactos.Attach(Grupocontacto);
    //                    consultas.Db.GrupoContactos.DeleteOnSubmit(Grupocontacto);
    //                    consultas.Db.SubmitChanges();
    //                    var query = (from c in consultas.Db.Contacto
    //                                 where c.Id_Contacto == Int32.Parse(e.CommandArgument.ToString())
    //                                 select c);
    //                    foreach (Contacto c in query)
    //                    {
    //                        c.Inactivo = true;
    //                    }
    //                    consultas.Db.SubmitChanges();
    //                    RequiredFieldValidator1.Enabled = false;
    //                    RequiredFieldValidator2.Enabled = false;
    //                    RequiredFieldValidator3.Enabled = false;
    //                    RequiredFieldValidator4.Enabled = false;
    //                    RequiredFieldValidator8.Enabled = false;
    //                    RequiredFieldValidator9.Enabled = false;
    //                    RegularExpressionValidator1fax.Enabled = false;
    //                    gv_administradores.DataBind();
    //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Usuario eliminado correctamente.')", true);
    //                }
    //                catch
    //                { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo eliminar el usuario seleccionado.')", true); }

    //                #endregion
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    private Contacto VerificarLiderRegional(int idRegional)
    //    {
    //        var lider = (from c in consultas.Db.Contacto
    //                     join gc in consultas.Db.GrupoContactos on c.Id_Contacto equals gc.CodContacto
    //                     where c.CodTipoAprendiz == idRegional && c.InactivoAsignacion == false && gc.CodGrupo == Constantes.CONST_LiderRegional
    //                     select c).FirstOrDefault();
    //        return lider;
    //    }

    //    /// <summary>
    //    /// Nombres the regional.
    //    /// </summary>
    //    /// <param name="id">The identifier.</param>
    //    /// <returns></returns>
    //    public string NombreRegional(string id)
    //    {
    //        if (id != "0")
    //        {
    //            var regional = (from d in consultas.Db.departamento
    //                            where d.Id_Departamento == int.Parse(id)
    //                            select d).FirstOrDefault();
    //            if (regional != null)
    //            {
    //                return regional.NomDepartamento;
    //            }
    //            else
    //            {
    //                return "";
    //            }
    //        }
    //        else
    //            return "";

    //    }
    //}

    ///// <summary>
    ///// users
    ///// </summary>
    //public class users
    //{
    //    /// <summary>
    //    /// Gets or sets the identifier contacto.
    //    /// </summary>
    //    /// <value>
    //    /// The identifier contacto.
    //    /// </value>
    //    public int Id_contacto { get; set; }
    //    /// <summary>
    //    /// Gets or sets the nombres.
    //    /// </summary>
    //    /// <value>
    //    /// The nombres.
    //    /// </value>
    //    public string Nombres { get; set; }
    //    /// <summary>
    //    /// Gets or sets the email.
    //    /// </summary>
    //    /// <value>
    //    /// The email.
    //    /// </value>
    //    public string Email { get; set; }
    //    /// <summary>
    //    /// Gets or sets the codgrupo.
    //    /// </summary>
    //    /// <value>
    //    /// The codgrupo.
    //    /// </value>
    //    public int codgrupo { get; set; }
    //    /// <summary>
    //    /// Gets or sets the nomgrupo.
    //    /// </summary>
    //    /// <value>
    //    /// The nomgrupo.
    //    /// </value>
    //    public string nomgrupo { get; set; }
    //    /// <summary>
    //    /// Gets or sets the IDOPERADOR.
    //    /// </summary>
    //    /// <value>
    //    /// The IDOPERADOR.
    //    /// </value>
    //    public int? idOperador { get; set; }
    //    /// <summary>
    //    /// Gets or sets the operador.
    //    /// </summary>
    //    /// <value>
    //    /// The operador.
    //    /// </value>
    //    public string operador { get; set; }
    //    /// <summary>
    //    /// Gets or sets a value indicating whether [estado lider].
    //    /// </summary>
    //    /// <value>
    //    ///   <c>true</c> if [estado lider]; otherwise, <c>false</c>.
    //    /// </value>
    //    public bool estadoLider { get; set; }
    //    /// <summary>
    //    /// Gets or sets the regional.
    //    /// </summary>
    //    /// <value>
    //    /// The regional.
    //    /// </value>
    //    public int regional { get; set; }
    //}
    #endregion
}