using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;
using Fonade.Clases;
using Fonade.Error;

namespace Fonade.FONADE.interventoria
{
    public partial class Interventor : Negocio.Base_Page
    {
        protected int CodContacto;
        /// <summary>
        /// Grupo que tiene el contacto seleccionado.
        /// </summary>
        protected int CodGrupo_ContactoSeleccionado;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (ListBox1.Items.Count == 0 || LB_Sectores.Items.Count == 0) { BindearListBoxes(); }

                    CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoInterventor"]);

                    if (CodContacto == 0)
                    {
                        #region Código de inserción.
                        //Importante.
                        PanelCrear.Visible = true;
                        PanelModificar.Visible = false;

                        //Establecer valores.
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            this.Page.Title = "FONDO EMPRENDER - Administrar Interventores";
                            lbl_Titulo.Text = "NUEVO USUARIO INTERVENTORES";

                            //Mostrar ciertos campos.
                            tb_cr_int.Visible = true;
                        }
                        if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            this.Page.Title = "FONDO EMPRENDER - Administrar Coordinadores de Interventoria";
                            lbl_Titulo.Text = "NUEVO USUARIO COORDINADOR DE INTERVENTORíA";

                            //Ocultar ciertos campos.
                            t_Otrossectores.Visible = false;
                            lblPersona.Visible = false;
                            dd_persona.Visible = false;
                            pnlSectores.Visible = false;
                            ddl_tidentificacionmod.Visible = false;
                            lbl_s_salario.Text = "Salario Mensual: ";
                        }

                        //Termina el if activando esta línea.
                        llenarTipoIdentificacion(ddl_tidentificacionCrear);
                        #endregion
                    }
                    else
                    {
                        #region Código de edición.
                        //Importante.
                        PanelCrear.Visible = false;
                        PanelModificar.Visible = true;

                        //Establecer valores.
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            this.Page.Title = "FONDO EMPRENDER - Administrar Interventores";
                            lbl_Titulo.Text = "EDITAR USUARIO INTERVENTORES";
                            //Nuevo = Se cambia el código para que edite la información correctamente.
                            btn_modificar.Text = "Actualizar";

                            //Ocultar ciertos campos.
                            t_mod_persona.Visible = true;
                        }
                        if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            this.Page.Title = "FONDO EMPRENDER - Administrar Coordinadores de Interventoria";
                            lbl_Titulo.Text = "EDITAR USUARIO COORDINADOR DE INTERVENTORÍA";
                            llenarTipoIdentificacion(ddl_tidentificacionCrear);
                            //Nuevo = Se cambia el código para que edite la información correctamente.
                            btn_modificar.Text = "Actualizar";

                            //Ocultar ciertos campos.
                            t_mod_persona.Visible = false;
                            t_Otrossectores.Visible = false;
                            //lblPersona.Visible = false;
                            ddl_tidentificacionmod.Visible = false;
                            lbl_s_salario.Text = "Salario: ";
                        }

                        //Termina el else activando estas líneas...
                        PanelModificar.Visible = true;
                        llenarTipoIdentificacion(dd_TipoIdentidicacionModificar);
                        llenarModificacion();

                        #region Consultar sectores seleccionados.

                        string SQL_str = " SELECT S.nomSector, S.id_Sector, " +
                                        " Case When E.codcontacto is null then '' else 'selected' end AS Sel " +
                                        " FROM Sector S LEFT JOIN interventorSector E  " +
                                        " ON S.id_Sector=E.codSector AND E.codContacto = " + CodContacto +
                                        " WHERE isnull(E.Experiencia, 'A')='A' " +
                                        " ORDER BY nomSector";

                        var dt = consultas.ObtenerDataTable(SQL_str, "text");

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < ListBox1.Items.Count; j++)
                            {
                                ListItem item = ListBox1.Items[j];
                                string valort = dt.Rows[i]["id_Sector"].ToString();
                                //Si el valor del ítem es igual al valor de la tabla.
                                if (item.Value == valort && dt.Rows[i]["Sel"].ToString() != "")
                                { item.Selected = true; }
                            }

                        }

                        dt = null;
                        SQL_str = null;

                        #endregion

                        #endregion
                    }
                }
                catch
                { Response.Redirect("CatalogoInterventor.aspx"); }
            }
        }

        /// <summary>
        /// Bindear los ListBox de esta página.
        /// </summary>
        private void BindearListBoxes()
        {
            try
            {
                String txtSQL = "";
                DataTable tabla = new DataTable();

                //var sector = from s in consultas.Db.Sectors

                //             orderby s.NomSector ascending
                //             select new
                //             {
                //                 id_Sector = s.Id_Sector,
                //                 nomSector = s.NomSector
                //             };

                txtSQL = " SELECT id_Sector, nomSector FROM Sector ";
                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                ListBox1.Items.Clear();
                LB_Sectores.Items.Clear();

                foreach (DataRow row in tabla.Rows)
                {
                    ListItem it = new ListItem();
                    it.Value = row["id_Sector"].ToString();
                    it.Text = row["nomSector"].ToString();
                    ListBox1.Items.Add(it);
                    LB_Sectores.Items.Add(it);
                }

                txtSQL = null;
                tabla = null;

                //LB_Sectores.DataSource = sector;
                //LB_Sectores.DataTextField = "nomSector";
                //LB_Sectores.DataValueField = "id_Sector";
                //LB_Sectores.DataBind();

                //ListBox1.DataSource = sector;
                //ListBox1.DataTextField = "nomSector";
                //ListBox1.DataValueField = "id_Sector";
                //ListBox1.DataBind();
            }
            catch (Exception) { }
        }

        protected void llenarTipoIdentificacion(DropDownList lista)
        {
            var SQL = (from x in consultas.Db.TipoIdentificacions
                       select new
                       {
                           id_tipo = x.Id_TipoIdentificacion,
                           nom_tipo = x.NomTipoIdentificacion,
                       });
            lista.DataSource = SQL;
            lista.DataTextField = "nom_tipo";
            lista.DataValueField = "id_tipo";
            lista.DataBind();
        }

        protected void validarInserUpdate(string caso, int IdContacto, double Salario, string nombre, string apellido, string email, string tipoidentificacion, string identificacion)
        {
            if (IdContacto == 0)
            {
                
                var sql = (from i in consultas.Db.Contacto
                           where i.Email == email
                           || (i.Identificacion == Convert.ToDouble(identificacion)
                                && i.codOperador == usuario.CodOperador)
                           select i).Count();
                
                if (sql == 0)
                {
                    insertupdateInterventor(caso, IdContacto, Salario, nombre, apellido, email, tipoidentificacion, identificacion);
                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un usuario registrado con el correo electronico o identificacion ingresado')", true);
                }
            }

            else
            {
                var sql = (from x in consultas.Db.Contacto
                           where x.Id_Contacto != IdContacto
                           && x.Email == email
                           select new { x }).Count();

                if (sql == 0)
                {
                    var query2 = (from x2 in consultas.Db.Contacto
                                  where x2.Id_Contacto != IdContacto
                                  && x2.CodTipoIdentificacion == Convert.ToInt32(tipoidentificacion)
                                  && x2.Identificacion == Convert.ToInt64(identificacion)
                                  select new { x2 }).Count();

                    if (query2 == 0)
                    {
                        insertupdateInterventor(caso, IdContacto, Salario, nombre, apellido, email, tipoidentificacion, identificacion);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El codigo de Usuario ya se encuentra registrado')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un usuario con ese correo electrónico!')", true);
                }
            }
        }

        protected void insertupdateInterventor(string caso, int IdContacto, double Salario, string nombre, string apellido, string email, string tipoidentificacion, string identificacion)
        {
            String wclave = "";
            wclave = GeneraClave();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                //SqlCommand cmd = new SqlCommand("MD_InsertUpdateInterventor", con);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@nombres", nombre);
                cmd.Parameters.AddWithValue("@apellidos", apellido);
                cmd.Parameters.AddWithValue("@codtipoIdentificacion", Convert.ToInt32(tipoidentificacion));
                cmd.Parameters.AddWithValue("@identificacion", Convert.ToInt64(identificacion));
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@clave", wclave);
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)//El Coordinador CREA interventores.
                {
                    cmd.Parameters.AddWithValue("@CodGrupo", Constantes.CONST_Interventor);
                    cmd.Parameters.AddWithValue("@codCoordinador", usuario.IdContacto);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CodGrupo", Constantes.CONST_CoordinadorInterventor);
                }
                cmd.Parameters.AddWithValue("@Id_Contacto", IdContacto);
                cmd.Parameters.AddWithValue("@Salario", Convert.ToDouble(Salario));
                cmd.Parameters.AddWithValue("@caso", caso);
                cmd.Parameters.AddWithValue("@codOperador", usuario.CodOperador);
                cmd.CommandText = "MD_InsertUpdateInterventor";

                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);

                con.Open();

                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
                cmd2.Dispose();
                cmd.Dispose();
            }
            finally {

                con.Close();
                con.Dispose();
            }

            #region crea el interventor

            var result = (from c in consultas.Db.Contacto
                          where c.Email == email
                          select c.Id_Contacto).FirstOrDefault();

            if (result > 0)
            {
                try
                {
                    Datos.GrupoContacto grupocontacto = new Datos.GrupoContacto();

                    grupocontacto.CodContacto = result;
                    

                    //Se agrega validación para que el sistema grabe correctamente en grupocontacto 
                    // teniendo en cuenta si el usuario es o no coordinador interventor
                    // Modificado por Alex Flautero -> Noviembre 13 de 2014
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)//El Coordinador CREA interventores.
                    {
                        grupocontacto.CodGrupo = Constantes.CONST_Interventor;
                    }
                    else
                    {
                        grupocontacto.CodGrupo = Constantes.CONST_CoordinadorInterventor;
                    }
                    
                    
                    consultas.Db.GrupoContactos.InsertOnSubmit(grupocontacto);



                    consultas.Db.SubmitChanges();

                    Datos.Interventor interventor = new Datos.Interventor();

                    interventor.CodContacto = result;
                    interventor.Salario = Convert.ToDecimal(Salario);
                    interventor.CodCoordinador = usuario.IdContacto;
                    interventor.fechaActualizacion = DateTime.Now;

                    consultas.Db.Interventors.InsertOnSubmit(interventor);

                    consultas.Db.SubmitChanges();
                }
                catch (LinqDataSourceValidationException)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo generar el registro');", true);
                }
                catch (Exception ex)
                {
                    string url = Request.Url.ToString();

                    string mensaje = ex.Message.ToString();
                    string data = ex.Data.ToString();
                    string stackTrace = ex.StackTrace.ToString();
                    string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                    // Log the error
                    ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                }

            }

            #endregion

            if (caso == "Create")
            {
                var querysql = (from x in consultas.Db.Contacto
                                where x.Email == email
                                select new
                                {
                                    x.Id_Contacto
                                }).FirstOrDefault();





                //*************************************AQUI MAIL WPLAZAS JUNIO 2-2015 *********************************//
                String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
                bool Enviado = false; //Variable que determina si el mensaje fue enviado o no "como resultado de la re-activación".
                bool correcto = false;
                Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");
                //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
                Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", "Interventor");
                Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", this.txt_emailCrear.Text);
                Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", wclave);

                try
                {
                    //Generar y enviar mensaje.
                    Correo correo = new Correo(usuario.Email,
                                               "Fondo Emprender",
                                               txt_emailCrear.Text.Trim(),
                                               this.txt_nombreCrear.Text.Trim() + " " + this.txt_apellidosCrear.Text.Trim(),
                                               "Generación Mail Interventor",
                                               Texto_Obtenido);
                    correo.Enviar();

                    //El mensaje fue enviado.
                    Enviado = true;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.ToString() + "');", true);


                }
                //*************************************AQUI FIN MAIL WPLAZAS JUNIO 2-2015 *********************************//

                /*
                #region DELETE!
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    cmd = new SqlCommand(" delete from interventorsector where CodContacto = " + querysql.Id_Contacto.ToString() + " AND Experiencia = 'A' ", con);

                    cmd.CommandType = CommandType.Text;
                    cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                    cmd2.Dispose();
                    cmd.Dispose();
                }
                catch { }
                #endregion
                */
                //Recorrer ListBox1 para detectar los sectores seleccionados.
                SqlCommand cmd;
                SqlCommand cmd2;
                foreach (ListItem item in LB_Sectores.Items)
                {
                    if (item.Selected == true)
                    {
                        #region DATA!
                        con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                        try
                        {
                            
                            cmd = new SqlCommand(" INSERT INTO InterventorSector(CodContacto,CodSector,Experiencia) VALUES(" + querysql.Id_Contacto.ToString() + "," + item.Value + ",'A') ", con);

                            cmd.CommandType = CommandType.Text;
                            //cmd.Parameters.AddWithValue("@CodContacto", 93318);
                            //cmd.Parameters.AddWithValue("@CodSector", item.Value);
                            cmd2 = new SqlCommand(UsuarioActual(), con);
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            //con.Close();
                            //con.Dispose();
                            cmd2.Dispose();
                            cmd.Dispose();
                        }
                        catch { }
                        finally {
                            con.Close();
                            con.Dispose();
                        
                        }
                        #endregion
                    }
                }

                //prSectorinterventor(querysql.Id_Contacto.ToString(), sectores_seleccionados);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente!');window.location=\"CatalogoInterventor.aspx\"", true);

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificado exitosamente!');window.location=\"CatalogoInterventor.aspx\"", true);
            }
        }

        protected void llenarModificacion()
        {
            //Inicializar variables.
            DataTable RSSector = new DataTable();

            try
            {
                var queryContacto = (from x in consultas.Db.Contacto
                                     from x2 in consultas.Db.Interventors
                                     where x.Id_Contacto == CodContacto
                                     && x.Id_Contacto == x2.CodContacto
                                     select new
                                     {
                                         x,
                                         x2.Salario,
                                         x2.Persona,
                                         x.CodTipoIdentificacion
                                     }).FirstOrDefault();

                txt_nombremod.Text = queryContacto.x.Nombres;
                txt_apellidosmod.Text = queryContacto.x.Apellidos;
                txt_emailmod.Text = queryContacto.x.Email;
                hdf_email_mod.Value = queryContacto.x.Email;
                txt_nidentificacionmod.Text = queryContacto.x.Identificacion.ToString();
                ddl_tidentificacionmod.SelectedValue = queryContacto.Persona;
                txt_Salariomod.Text = Decimal.Parse(queryContacto.Salario.ToString()).ToString().Split(',')[0];

                if (String.IsNullOrEmpty(queryContacto.x.Cargo))
                { 
                    Label2.Visible = false;
                    tr_Cargo.Style.Add("display", "none");
                    tr_Cargo.Visible = false;
                }
                else
                { 
                    L_Cargo.Text = queryContacto.x.Cargo;
                    L_Cargo.Visible = false;
                }

                if (queryContacto.x.Telefono.Equals("0"))
                { Label1.Visible = false; tr_Telefono.Style.Add("display", "none"); }
                else
                { L_telefono.Text = queryContacto.x.Telefono; }

                if (String.IsNullOrEmpty(queryContacto.x.Fax))
                { L_Faxe.Visible = false; tr_Fax.Style.Add("display", "none"); }
                else
                { L_fax.Text = queryContacto.x.Fax; }

                dd_TipoIdentidicacionModificar.SelectedValue = queryContacto.x.CodTipoIdentificacion.ToString();

                #region Cargar datos adicionales "sólo si es coordinador de interventores".

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    String txtSQL = " SELECT nombres,apellidos, codtipoidentificacion,identificacion, cargo, email, " +
                                            " telefono, fax, " +
                                            " Experiencia, Intereses, HojaVida " +
                                            " FROM Contacto WHERE Id_Contacto = " + CodContacto;

                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt.Rows.Count > 0)
                    {
                        lbl_exp_general.Text = dt.Rows[0]["Experiencia"].ToString();
                        lbl_HV.Text = dt.Rows[0]["HojaVida"].ToString();
                        lbl_intereses.Text = dt.Rows[0]["Intereses"].ToString();
                        dt = null;
                        txtSQL = null;
                    }

                }
                #endregion

                #region Cargar datos adicionales "sólo si es Interventor".

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    String txtSQL = " SELECT E.*, B.nomBanco " +
                                            " FROM interventor E LEFT JOIN Banco B  " +
                                            " ON E.codBanco = B.id_Banco  " +
                                            " WHERE codContacto = " + CodContacto;

                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt.Rows.Count > 0)
                    {
                        lblCuentaNo.Text = dt.Rows[0]["Cuenta"].ToString();
                        lblEmpresasAtender.Text = dt.Rows[0]["MaximoPlanes"].ToString();
                        lblBanco.Text = dt.Rows[0]["nomBanco"].ToString();
                        dt = null;
                        txtSQL = null;
                    }
                }

                #endregion

                #region Cargar el código grupo del contacto seleccionado para cargar mas datos adicionales.

                String txtSQL2 = "SELECT CodGrupo FROM GrupoContacto WHERE CodContacto = " + CodContacto;
                var dt2 = consultas.ObtenerDataTable(txtSQL2, "text");
                if (dt2.Rows.Count > 0)
                {
                    CodGrupo_ContactoSeleccionado = Convert.ToInt32(dt2.Rows[0]["CodGrupo"].ToString());
                    txtSQL2 = null;
                    dt2 = null;

                    //Si es interventor, se le cargan los datos.
                    if (CodGrupo_ContactoSeleccionado == Constantes.CONST_Interventor)
                    {
                        #region Consultar sector principal al que aplica.

                        String txt_sql_sector_principal = " SELECT S.id_Sector, S.nomSector " +
                                                          " FROM interventorSector E, Sector S " +
                                                          " WHERE E.codSector = S.id_Sector " +
                                                          " and E.Experiencia = 'P' and E.CodContacto = " + CodContacto;
                        //Asignar resultados de la consulta.
                        var dt_principal = consultas.ObtenerDataTable(txt_sql_sector_principal, "text");

                        //Si tiene datos, se los asigna, destruyendo al final las variables usadas.
                        if (dt_principal.Rows.Count > 0)
                        { lblPrincipal.Text = dt_principal.Rows[0]["nomSector"].ToString(); txt_sql_sector_principal = null; dt_principal = null; }

                        #endregion

                        #region Consultar sector secundario al que aplica.

                        String txt_sql_sector_secundario = " SELECT S.id_Sector, S.nomSector " +
                                                           " FROM interventorSector E, Sector S " +
                                                           " WHERE E.codSector = S.id_Sector " +
                                                           " and E.Experiencia = 'S' and E.CodContacto = " + CodContacto;
                        //Asignar resultados de la consulta.
                        var dt_secundario = consultas.ObtenerDataTable(txt_sql_sector_secundario, "text");

                        //Si tiene datos, se los asigna, destruyendo al final las variables usadas.
                        if (dt_secundario.Rows.Count > 0)
                        { lblSecundario.Text = dt_secundario.Rows[0]["nomSector"].ToString(); txt_sql_sector_secundario = null; dt_secundario = null; }

                        #endregion

                        #region Consultar experiencias y datos adicionales.

                        String txt_sql_masDetalles = " SELECT E.*, B.nomBanco " +
                                                     " FROM interventor E LEFT JOIN Banco B  " +
                                                     " ON E.codBanco = B.id_Banco  " +
                                                     " WHERE CodContacto = " + CodContacto;

                        //Asignar resultados a variable DataTable.
                        var dt_masDetalles = consultas.ObtenerDataTable(txt_sql_masDetalles, "text");

                        if (dt_masDetalles.Rows.Count > 0)
                        {
                            lbl_exp_principal.Text = dt_masDetalles.Rows[0]["ExperienciaPrincipal"].ToString();
                            lbl_exp_secundaria.Text = dt_masDetalles.Rows[0]["ExperienciaSecundaria"].ToString();
                            lblCuentaNo.Text = dt_masDetalles.Rows[0]["Cuenta"].ToString();
                            lblBanco.Text = dt_masDetalles.Rows[0]["nomBanco"].ToString();
                            lblEmpresasAtender.Text = dt_masDetalles.Rows[0]["MaximoPlanes"].ToString();

                            //Destruir variables.
                            txt_sql_masDetalles = null;
                            dt_masDetalles = null;
                        }

                        #endregion

                        //Mostrar las tablas que contienen la información adicional.
                        lnk_btn_VerContratos.Visible = true;
                        tb_interventor.Visible = true;
                        tb_sectores_aplica.Visible = true;
                    }
                }

                #endregion

                #region Comentarios.

                //foreach (Control control in PanelModificar.Controls)
                //{
                //    if (control is Label)
                //    {
                //        var label = control as Label;

                //        if (!String.IsNullOrEmpty(label.Text))
                //        {
                //            label.Visible = false;
                //            label.Enabled = false;
                //        }
                //    }
                //} 
                #endregion
            }
            catch { }

        }

        protected void btn_crear_Click(object sender, EventArgs e)
        {
            validarInserUpdate("Create", 0, Convert.ToDouble(txt_Salario.Text), txt_nombreCrear.Text,
                           txt_apellidosCrear.Text,
                           txt_emailCrear.Text,
                           ddl_tidentificacionCrear.SelectedValue,
                           txt_nidentificacionCrear.Text);
        }

        protected void btn_modificar_Click(object sender, EventArgs e)
        {
            if (btn_modificar.Text == "Actualizar")
            { ActualizarInterventor(); }
            else
            { validarInserUpdate("Update", CodContacto, Convert.ToDouble(txt_Salariomod.Text), txt_nombremod.Text, txt_apellidosmod.Text, txt_emailmod.Text, dd_TipoIdentidicacionModificar.SelectedValue, txt_nidentificacionmod.Text); }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 04/06/2014.
        /// Se crea este código basado en FONADE clásico, para realizar las correcciones indicadas en el documento
        /// "SEGUNDA REVISIÓN PERFIL COORDINADOR INTERV".docx.
        /// </summary>
        private void ActualizarInterventor()
        {
            //Inicializar variables.            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            DataTable RsUsuario = new DataTable();
            String correcto = "";
            List<string> sectores_seleccionados = new List<string>();
            DataTable RS = new DataTable();
            String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
            String txtGrupo = "";
            bool Enviado = false;

            try
            {
                CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoInterventor"]);
                if (CodContacto == 0)
                {
                    //Es muy raro, pero posiblemente durante las pruebas, se crearon valores con Id = 0, por
                    //lo que NO son aptos para las pruebas y en producción.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El usuario seleccionado no es válido.')", true);
                    HttpContext.Current.Session["ContactoInterventor"] = null;
                    CodContacto = 0;
                    Response.Redirect("CatalogoInterventor.aspx");
                }

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                { txtGrupo = "Interventores"; }
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                { txtGrupo = "Coordinador de Interventoria"; }

                #region Consulta.

                txtSQL = " SELECT Id_Contacto FROM Contacto WHERE (Email = '" + txt_emailmod.Text.Trim() + "' " +
                         " OR (codOperador = " + usuario.CodOperador +
                         " and Identificacion = " + txt_nidentificacionmod.Text.Trim() + ")) " +
                         " AND Id_Contacto <> " + CodContacto;

                //Asignar resultados de la consulta.
                RsUsuario = consultas.ObtenerDataTable(txtSQL, "text");

                #endregion

                //Si la consulta anterior tiene datos.
                if (RsUsuario.Rows.Count == 0)
                {
                    #region Actualiza la información del contacto seleccionado.

                    #region Actualización #1.

                    txtSQL = " Update Contacto set Nombres ='" + txt_nombremod.Text.Trim() + "'," +
                             " Apellidos = '" + txt_apellidosmod.Text.Trim() + "', " +
                             " CodTipoIdentificacion = " + dd_TipoIdentidicacionModificar.SelectedValue + ", " +
                             " Identificacion = " + txt_nidentificacionmod.Text.Trim() + ", " +
                             " Email = '" + txt_emailmod.Text.Trim() + "' " +
                             " WHERE Id_Contacto = " + CodContacto;

                    //Ejecutar consulta.
                    cmd = new SqlCommand(txtSQL, conn);
                    correcto = String_EjecutarSQL(conn, cmd);

                    if (correcto != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Hubo un error al actualizar el usuario seleccionado: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                        return;
                    }

                    #endregion

                    #region Actualización #2.

                    txtSQL = " UPDATE interventor SET Persona = '" + ddl_tidentificacionmod.SelectedValue + "'" +
                             " WHERE CodContacto = " + CodContacto;

                    //Ejecutar consulta.
                    //cmd = new SqlCommand(txtSQL, conn);
                    //correcto = String_EjecutarSQL(conn, cmd);
                    ejecutaReader(txtSQL, 2);

                    if (correcto != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización #2: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                        return;
                    }

                    #endregion

                    #region Actualización #3.

                    txtSQL = " UPDATE interventor SET Salario = " + txt_Salariomod.Text.Trim() +
                             " WHERE CodContacto = " + CodContacto;

                    //Ejecutar consulta.
                    //cmd = new SqlCommand(txtSQL, conn);
                    //correcto = String_EjecutarSQL(conn, cmd);
                    ejecutaReader(txtSQL, 2);

                    if (correcto != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización #3: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                        return;
                    }

                    #endregion

                    #region Si el usuario es interventor, se asignan los sectores que aplican al interventor.

                    #region DELETE!
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    try
                    {

                        cmd = new SqlCommand(" delete from interventorsector where CodContacto = " + CodContacto + " AND Experiencia = 'A' ", con);

                        cmd.CommandType = CommandType.Text;
                        SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                        con.Open();
                        cmd2.ExecuteNonQuery();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        cmd2.Dispose();
                        cmd.Dispose();
                    }
                    catch { }
                    finally {
                        con.Close();
                        con.Dispose();
                    }
                    #endregion

                    //Recorrer ListBox1 para detectar los sectores seleccionados.
                    foreach (ListItem item in ListBox1.Items)
                    {
                        if (item.Selected == true)
                        {
                            #region DATA!
                            SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            try
                            {

                                cmd = new SqlCommand(" INSERT INTO InterventorSector(CodContacto,CodSector,Experiencia) VALUES(" + CodContacto + "," + item.Value + ",'A') ", con2);

                                cmd.CommandType = CommandType.Text;
                                //cmd.Parameters.AddWithValue("@CodContacto", 93318);
                                //cmd.Parameters.AddWithValue("@CodSector", item.Value);
                                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con2);
                                con2.Open();
                                cmd2.ExecuteNonQuery();
                                cmd.ExecuteNonQuery();
                                con2.Close();
                                con2.Dispose();
                                cmd2.Dispose();
                                cmd.Dispose();
                            }
                            catch(Exception ex) {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('"+ex.Message+"')", true);
                            }
                            finally {
                                con2.Close();
                                con2.Dispose();
                            }
                            #endregion
                        }
                    }

                    #endregion

                    #region Si el email cambio se envia el correo con la clave.

                    if (hdf_email_mod.Value != txt_emailmod.Text.Trim())
                    {
                        //Envíar el mail al usuario
                        txtSQL = " SELECT Nombres, Apellidos, Email, Clave FROM Contacto " +
                                 " WHERE Id_Contacto = " + CodContacto;

                        //Asignar resultados a la variable DataTable.
                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        #region Enviar el Email al usuario.

                        if (RS.Rows.Count > 0)
                        {
                            //Consultar el "TEXTO".
                            Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");

                            //Sólo por si acaso, si el resultado de "Texto_Obtenido" NO devuelve los datos según el texto esperado,
                            //se debe asignar el texto tal cual se vió en BD el "28/04/2014".
                            if (Texto_Obtenido.Contains("Señor Usuario") || Texto_Obtenido.Trim() == null)
                            {
                                Texto_Obtenido = "Señor Usuario Con el usuario {{Email}} y contraseña {{Clave}},  podrá acceder al sistema de información por medio de la pagina www.fondoemprender.com,  allí encontrara en la parte superior del sistema específicamente en el botón con el signo de interrogación  (?) el manual de su perfil ''{{Rol}}''";
                            }

                            //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
                            Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", txtGrupo);
                            Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", RS.Rows[0]["Email"].ToString());
                            Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", RS.Rows[0]["Clave"].ToString());

                            try
                            {
                                //Generar y enviar mensaje.
                                Correo correo = new Correo(usuario.Email,
                                                           "Fondo Emprender",
                                                           RS.Rows[0]["Email"].ToString(),
                                                           RS.Rows[0]["Nombres"].ToString() + " " + RS.Rows[0]["Apellidos"].ToString(),
                                                           "Registro a Fondo Emprender - Catálogo de Interventor -",
                                                           Texto_Obtenido);
                                correo.Enviar();

                                //El mensaje fue enviado.
                                Enviado = true;

                                //Inserción en tabla "LogEnvios".
                                prLogEnvios("Registro a Fondo Emprender", usuario.Email, txt_emailmod.Text.Trim(), "Catálogo de Interventor", 0, Enviado);
                            }
                            catch
                            {
                                //El mensaje fue enviado.
                                Enviado = false;

                                //Inserción en tabla "LogEnvios".
                                prLogEnvios("Registro a Fondo Emprender", usuario.Email, txt_emailmod.Text.Trim(), "Catálogo de Interventor", 0, Enviado);
                            }
                        }

                        #endregion
                    }

                    #endregion

                    if (correcto != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + correcto + ".')", true);
                        return;
                    }
                    else
                    {
                        HttpContext.Current.Session["ContactoInterventor"] = null;
                        CodContacto = 0;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada correctamente.'); window.location='CatalogoInterventor.aspx'", true);
                        //Response.Redirect("CatalogoInterventor.aspx");
                    }

                    #endregion
                }
                else
                {
                    //No lo puede editar.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un Usuario con ese email o Identificación.')", true);
                    return;
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo actualizar la información del usuario.')", true);
                return;
            }
        }

        private void prSectorinterventor(String P_CodContacto, List<string> P_Sectores)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            String correcto;

            try
            {
                #region Eliminación.

                txtSQL = " delete from interventorsector where CodContacto = " + P_CodContacto +
                         " AND Experiencia = 'A'";

                //Ejecutar consulta.
                cmd = new SqlCommand(txtSQL, conn);
                correcto = String_EjecutarSQL(conn, cmd);

                #endregion

                foreach (string item in P_Sectores)
                {
                    #region Inserción de los sectores seleccionados.

                    txtSQL = " Insert into interventorsector (CodContacto,CodSector,Experiencia) " +
                             " values(" + P_CodContacto + ", " + item + ", 'A')";

                    //Ejecutar consulta.
                    cmd = new SqlCommand(txtSQL, conn);
                    correcto = String_EjecutarSQL(conn, cmd);

                    #endregion
                }
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/06/2014.
        /// Ver los contratos que tiene el contacto seleccionado...
        /// Se deja temporalmente así la funcionalidad mientras se aclara si esta ventana emergente se aplicará
        /// al desarollo actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnk_btn_VerContratos_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
            Redirect(null, "CatalogoContratoInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=420,height=470,top=100");
          
        }
    }
}

