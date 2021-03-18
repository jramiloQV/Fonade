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

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// crea coordinador de evaluaciones y envia mail
    /// </summary>
    public partial class CoordinadorEvaluador : Negocio.Base_Page
    {
        protected int CodContacto;

        protected void Page_Load(object sender, EventArgs e)
        {
            CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoEvaluador"]);

            if (!IsPostBack)
            {
                llenarTipoIdentificacion();
                l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
                if (CodContacto == 0)
                {
                    lbl_Titulo.Text = void_establecerTitulo("CREAR COORDINADOR DE EVALUADORES");
                    btn_crear.Visible = true;
                }
                else
                {
                    lbl_Titulo.Text = void_establecerTitulo("MODIFICAR COORDINADOR DE EVALUADORES");
                    PanelEdit.Visible = true;
                    btn_actualizar.Visible = true;
                    llenarDatos();
                }
            }
        }

        protected void llenarTipoIdentificacion()
        {
            var query = (from x in consultas.Db.TipoIdentificacions
                         select new
                         {
                             id_tipo = x.Id_TipoIdentificacion,
                             nom_tipo = x.NomTipoIdentificacion,
                         });
            ddl_tidentificacion.DataSource = query;
            ddl_tidentificacion.DataTextField = "nom_tipo";
            ddl_tidentificacion.DataValueField = "id_tipo";
            ddl_tidentificacion.DataBind();
        }

        protected void llenarDatos()
        {
            var query = (from x in consultas.Db.Contacto
                         where x.Id_Contacto == CodContacto
                         select new
                         {
                             x
                         }).FirstOrDefault();

            txt_nombre.Text = query.x.Nombres;
            txt_apellidos.Text = query.x.Apellidos;
            txt_email.Text = query.x.Email;
            txt_nidentificación.Text = query.x.Identificacion.ToString();
            ddl_tidentificacion.SelectedValue = query.x.TipoIdentificacion.ToString();
            l_cargo.Text = query.x.Cargo;
            l_fax.Text = query.x.Fax;
            l_telefono.Text = query.x.Telefono;
        }

        protected void btn_crear_Click(object sender, EventArgs e)
        {
            validarInserUpdate("Create", 0);
        }

        protected void validarInserUpdate(string caso, int IdContacto)
        {
            if (IdContacto==0)
            {
                var query = (from x in consultas.Db.Contacto
                             where x.Email == txt_email.Text
                             || (x.Identificacion == Convert.ToInt64(txt_nidentificación.Text))
                             select new { x }).Count();
                if (query == 0)
                {

                    insertupdatecoordEval(caso, IdContacto);

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un usuario con ese correo electrónico o documento de identificación!')", true);
                }
            }
            else
            {
                var query = (from x in consultas.Db.Contacto
                             where x.Id_Contacto != IdContacto
                             && x.Email == txt_email.Text
                             select new { x }).Count();

                if (query == 0)
                {
                    var query2 = (from x2 in consultas.Db.Contacto
                                 where x2.Id_Contacto != IdContacto
                                 && x2.Identificacion == Convert.ToInt64(txt_nidentificación.Text)
                                 select new { x2 }).Count();

                    if (query2 == 0)
                    {
                        insertupdatecoordEval(caso, IdContacto);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un usuario con ese documento de identificación!')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un usuario con ese correo electrónico!')", true);
                }
            }
            
        }

        protected void insertupdatecoordEval(string caso, int IdContacto)
        {
            String wclave = "";
            wclave = GeneraClave();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_InsertUpdateCoordinadorEvaluador", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombres", txt_nombre.Text);
            cmd.Parameters.AddWithValue("@apellidos", txt_apellidos.Text);
            cmd.Parameters.AddWithValue("@codtipoIdentificacion", Convert.ToInt32(ddl_tidentificacion.SelectedValue));
            cmd.Parameters.AddWithValue("@identificacion", Convert.ToInt64(txt_nidentificación.Text));
            cmd.Parameters.AddWithValue("@email", txt_email.Text);
            cmd.Parameters.AddWithValue("@clave", wclave);
            cmd.Parameters.AddWithValue("@CodGrupo", Constantes.CONST_CoordinadorEvaluador);
            cmd.Parameters.AddWithValue("@Id_Contacto", IdContacto);
            cmd.Parameters.AddWithValue("@caso", caso);
            cmd.Parameters.AddWithValue("@codOperador", usuario.CodOperador);
            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
            con.Open();
            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd2.Dispose();
            cmd.Dispose();

            if (caso == "Create")
            {



                //*************************************AQUI MAIL WPLAZAS JUNIO 2-2015 *********************************//
                String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
                bool Enviado = false; //Variable que determina si el mensaje fue enviado o no "como resultado de la re-activación".
                bool correcto = false;
                Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");
                //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
                Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", "Coordinador Evaluador");
                Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", this.txt_email.Text);
                Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", wclave);

                try
                {
                    //Generar y enviar mensaje.
                    Correo correo = new Correo(usuario.Email,
                                               "Fondo Emprender",
                                               txt_email.Text.Trim(),
                                               this.txt_nombre.Text.Trim() + " " + this.txt_apellidos.Text.Trim(),
                                               "Generación Mail Coordinador",
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

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente!'); window.opener.location.reload(); window.close();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificado exitosamente!'); window.opener.location.reload(); window.close();", true);
            }

        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            validarInserUpdate("Update", CodContacto);
        }


    }
}