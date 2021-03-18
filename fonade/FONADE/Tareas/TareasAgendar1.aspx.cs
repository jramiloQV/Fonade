using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Tareas
{
    public partial class TareasAgendar1 : Negocio.Base_Page
    {
        String Id_TareaUsuarioRepeticion;
        string errorMessageDetail;

        protected void lds_tareas_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ObjectCache cache = MemoryCache.Default;
            string fileContents = cache["filecontents"] as string;

            if (fileContents == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration =
                    DateTimeOffset.Now.AddSeconds(10.0);

                List<string> filePaths = new List<string>();

                try
                {
                    string cachedFilePath = Server.MapPath("TareasAgendar.aspx");

                    filePaths.Add(cachedFilePath);


                    policy.ChangeMonitors.Add(new
                        HostFileChangeMonitor(filePaths));
                    fileContents = File.ReadAllText(cachedFilePath) + "\n"
                               + DateTime.Now.ToString();

                    cache.Set("filecontents", fileContents, policy);
                }
                catch (System.IO.IOException ioe) { throw ioe; }
                catch (Exception ex) { throw ex; }

                DateTime? fechaActual = DateTime.Today;
                txtDate2.Text = DateTime.Now.ToString("dd/MM/yyyy");

                try
                {
                    Id_TareaUsuarioRepeticion = Request["Id_TareaUsuarioRepeticion"];
                }
                catch (Exception)
                {
                }

                if (!String.IsNullOrEmpty(Id_TareaUsuarioRepeticion))
                {
                    tbl1.Visible = false;
                    tbl1.Enabled = false;

                    Panel2.Visible = true;
                    Panel2.Enabled = true;

                    if (!IsPostBack)
                    {

                        menuMostar();
                    }
                }

                if (!Page.IsPostBack)
                {
                    int conteo = 0;


                    var query = from tp in consultas.Db.TareaProgramas
                                where tp.delSistema == null | tp.delSistema != 1
                                select new
                                {
                                    Id_Tarea = tp.Id_TareaPrograma,
                                    Nombre_Tarea = tp.NomTareaPrograma,
                                };
                    ddl_usuarios.DataSource = query.ToList();
                    ddl_usuarios.DataTextField = "Nombre_Tarea";
                    ddl_usuarios.DataValueField = "Id_Tarea";
                    ddl_usuarios.DataBind();


                    ListBox1.DataSource = consultas.Db.MD_AgendarTareas_Prueba(usuario.IdContacto, usuario.CodGrupo, usuario.CodInstitucion, "TraerUsuarios", null, null, null, null, null, null, null, null, null, null, null, null);
                    ListBox1.DataTextField = "Nombre";
                    ListBox1.DataValueField = "Id_Contacto";
                    ListBox1.DataBind();
                    lbl_Titulo.Text = void_establecerTitulo("AGENDAR TAREA");


                    consultas.Parameters = new[] { new SqlParameter
                                                   { 
                                                        ParameterName = "@id_usuario",
                                                        Value = usuario.IdContacto
                                                   }, new SqlParameter
                                                    {
                                                        ParameterName = "@Cod_grupo",
                                                        Value = usuario.CodGrupo
                                                    }, new SqlParameter
                                                    {
                                                        ParameterName = "@Cod_institucion",
                                                        Value = usuario.CodInstitucion
                                                    }
                };
                    DataTable dtActas = consultas.ObtenerDataTable("MD_PlanNegocio");

                    DropDownList1.DataSource = dtActas;
                    DropDownList1.DataTextField = "NomProyecto";
                    DropDownList1.DataValueField = "Id_Proyecto";
                    DropDownList1.DataBind();
                    DropDownList1.Items.Insert(0, new ListItem("", ""));
                }
            }
        }

        protected void Correo(string mailusuario, string asunto)
        {
        }

        protected void Button1_click(object sender, EventArgs e)
        {
            if (ListBox1.Items.Count != 0)
            {
                string listausuarios = "<ul>";

                foreach (ListItem li in ListBox1.Items)
                {
                    if (li.Selected)
                    {
                        listausuarios = listausuarios + "<li>" + li.Text + "</li>";

                        AgendarTarea agenda = new AgendarTarea(Int32.Parse(li.Value), tb_tarea.Text, tb_descripcion.Text, DropDownList1.SelectedValue, 1, "0", false, Int32.Parse(ddl_urgencia.SelectedValue.ToString()), false, false, usuario.IdContacto, null, null, null);

                        agenda.Agendar();

                        string txtSQL;

                        txtSQL = "SELECT Max(Id_TareaUsuario) FROM TareaUsuario WHERE CodContactoAgendo = " + usuario.IdContacto;

                        var reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);

                        if (reader.Rows.Count > 0)
                        {
                            if (reader.Rows.Count > 0)
                            {
                                txtSQL = "INSERT INTO TareaUsuarioRepeticion (Fecha, CodTareaUsuario, Parametros) VALUES (" + "Getdate()," + reader.Rows[0].ItemArray[0].ToString() + ",'" + Int32.Parse(li.Value) + tb_tarea.Text + tb_descripcion.Text + DropDownList1.SelectedValue + 1 + "0" + false + Int32.Parse(ddl_urgencia.SelectedValue.ToString()) + false + false + usuario.IdContacto + null + null + null + "'" + ")";

                                ejecutaReader(txtSQL, 2);
                            }
                        }

                        if (ddl_avisar.SelectedValue == "Sí")
                        {
                        }
                    }


                }
                listausuarios = listausuarios + "</ul>";
                if (ddl_avisar.SelectedValue == "Sí")
                {


                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('se ha enviado correo a los siguiente usuarios: " + listausuarios + "')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea " + tb_tarea.Text + " Agendada')", true);
                }
            }
        }

        protected void ListBox1_OnSelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void btn_Grabar_Click(object sender, EventArgs e)
        {
            String txtSQL = "UPDATE TareaUsuarioRepeticion SET ";

            if (CheckBox1.Checked)
            {
                txtSQL += " FechaCierre = '" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "'";
            }
            else
            {
                txtSQL += " FechaCierre = null";
            }

            if (String.IsNullOrEmpty(TextBox9.Text))
            {
                txtSQL += " Respuesta = ''";
            }
            else
            {
                txtSQL += " ,Respuesta = '" + TextBox9.Text + "'";
            }

            txtSQL += " WHERE Id_TareaUsuarioRepeticion = " + Id_TareaUsuarioRepeticion;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(txtSQL, conn);
            try
            {

                conn.Open();
                cmd.ExecuteReader();
            }
            catch (SqlException se) { errorMessageDetail = se.Message; }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            menuMostar();
        }

        private void menuMostar()
        {
            consultas.Parameters = new[] { new SqlParameter
                                                   { 
                                                        ParameterName = "@Id_TareaUsuarioRepeticion",
                                                        Value = Id_TareaUsuarioRepeticion
                                                   }
                };
            DataTable dtActas = consultas.ObtenerDataTable("MP_ReporteTareasUsuario");

            try
            {
                TextBox1.Text = dtActas.Rows[0]["NomUsuarioAgendo"].ToString();
                TextBox2.Text = dtActas.Rows[0]["NomUsuario"].ToString();
                TextBox3.Text = dtActas.Rows[0]["NomTareaPrograma"].ToString();
                TextBox4.Text = dtActas.Rows[0]["NomProyecto"].ToString();

                if (String.IsNullOrEmpty(TextBox4.Text))
                {
                    TextBox4.Text = "---------";
                }

                TextBox5.Text = dtActas.Rows[0]["NomTareaUsuario"].ToString();
                TextBox6.Text = dtActas.Rows[0]["Descripcion"].ToString();
                TextBox7.Text = dtActas.Rows[0]["SoloFecha"].ToString() + " " + dtActas.Rows[0]["SoloHora"].ToString();

                switch (Int32.Parse(dtActas.Rows[0]["NivelUrgencia"].ToString()))
                {
                    case 1:
                        TextBox8.Text = "Muy Alta";
                        break;
                    case 2:
                        TextBox8.Text = "Alta";
                        break;
                    case 3:
                        TextBox8.Text = "Normal";
                        break;
                    case 4:
                        TextBox8.Text = "Baja";
                        break;
                    default:
                        TextBox8.Text = "Muy Baja";
                        break;
                }

                TextBox9.Text = dtActas.Rows[0]["Respuesta"].ToString();

                if (String.IsNullOrEmpty(dtActas.Rows[0]["FechaCierre"].ToString()))
                {
                    CheckBox1.Enabled = true;
                    CheckBox1.Checked = false;

                }
                else
                {
                    CheckBox1.Checked = true;
                    CheckBox1.Enabled = false;
                    TextBox9.Enabled = false;
                }
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }
    }
}