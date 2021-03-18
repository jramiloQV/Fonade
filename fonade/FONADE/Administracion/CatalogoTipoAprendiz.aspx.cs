using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Configuration;
using System.Web.UI;
using System.Web;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// CatalogoTipoAprendiz
    /// </summary>    
    public partial class CatalogoTipoAprendiz : Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Valor que SOLO debe volverse NULL para habilitar la
        /// sección de nuevos datos.
        /// </summary>
        private bool NuevoDato = false;

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Establecer el título de la página actual.
                this.Page.Title = "FONADE - TIPO DE APRENDIZ";

                EvaluarEnunciado();
                CargarTiposDeAprendices();
            }
            catch { Response.Redirect("~/Account/Login.aspx"); }
        }

        #region Métodos para mostrar los títulos.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Mostrar el nombre completo del interventor (usuario logueado), así como la fecha actual formateada según FONADE Clásico.
        /// </summary>
        private void MostrarInterventor()
        {
            String sql;

            sql = "SELECT Nombres + ' ' + Apellidos AS Nombre from Contacto where id_Contacto = " + usuario.IdContacto;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //lbl_Interventor.Text = reader["Nombre"].ToString();
                    //DateTime fecha = DateTime.Now;
                    //string sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
                    //lbl_tiempo.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                }
                //reader.Close();
            }
            catch (SqlException) { }
            finally { conn.Close(); conn.Dispose(); }
        }

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        #endregion

        #region Inserción.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Se llama a este método cuando se requiere activar los campos para la creación
        /// de TiposAprendiz.
        /// </summary>
        private void HabilitarCampos_NUEVO()
        {
            pnl_detalles.Visible = true;
            pnlPrincipal.Visible = false;
            txt_nmb_tipoAprendiz.Visible = true;
            B_Adicionar.Visible = true;
            B_Acion.Visible = false;
            B_Borrar.Visible = false;
            lbl_enunciado.Text = "NUEVO";
            NuevoDato = true;
            EvaluarEnunciado();
            NuevoDato = false;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Habilitar campos para creación de TipoAprendiz "LinkButton".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            HabilitarCampos_NUEVO();
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Habilitar campos para creación de TipoAprendiz "ImageButton".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Adicionar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            HabilitarCampos_NUEVO();
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Llamada a método que genera un nuevo registro en "TipoAprendiz".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Adicionar_Click(object sender, EventArgs e)
        {
            if (AgregarTipoAprendiz() == true)
            {
                #region Comentarios.
                ////Ocultar los campos.
                //pnl_detalles.Visible = false;
                //pnlPrincipal.Visible = true;

                //hdf_id.Value = "";
                //txt_nmb_tipoAprendiz.Text = ""; 
                #endregion

                //Ocultar los campos.
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                hdf_id.Value = "";
                txt_nmb_tipoAprendiz.Text = "";
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                EvaluarEnunciado();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Insertar registro en la tabla"TipoAprendiz".
        /// </summary>       
        private bool AgregarTipoAprendiz()
        {
            //Inicializar variables.
            bool correcto = false;
            //Obtiene la conexión
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            //Inicializa la variable para generar la consulta.
            String sqlConsulta = "";
            bool final = false;

            try
            {
                if (txt_nmb_tipoAprendiz.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del tipo de aprendiz.')", true);
                    return final;
                }
                else
                {
                    //Consulta
                    sqlConsulta = "INSERT INTO TipoAprendiz (NomTipoAprendiz) VALUES ('" + txt_nmb_tipoAprendiz.Text.Trim() + "')";

                    //Asignar SqlCommand para su ejecución.
                    SqlCommand cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == false)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear el Tipo Aprendiz.')", true);
                        return final;
                    }
                    else { final = true; }
                }

                return final;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear el Tipo Aprendiz.')", true);
                return final;
            }
        }

        #endregion

        #region Edición.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Actualizar el registro "TipoAprendiz" seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Acion_Click(object sender, EventArgs e)
        {
            if (EditarTipoAprendiz() == true)
            {
                //Ocultar los campos.
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                hdf_id.Value = "";
                txt_nmb_tipoAprendiz.Text = "";
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                EvaluarEnunciado();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 26/06/2014.
        /// Editar tipo de aprendiz seleccionado.
        /// </summary>
        private bool EditarTipoAprendiz()
        {
            //Inicializar variables.
            Int32 idTIpoAprendiz = 0;
            bool correcto = false;
            //Obtiene la conexión
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            //Inicializa la variable para generar la consulta.
            String sqlConsulta = "";
            bool final = false;

            try
            {
                //Se verifica que si se haya pasado el ID a la variable oculta.
                if (!String.IsNullOrEmpty(hdf_id.Value))
                {
                    //Obtener el tipo de aprendiz.
                    idTIpoAprendiz = Convert.ToInt32(hdf_id.Value);

                    if (txt_nmb_tipoAprendiz.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del tipo de aprendiz.')", true);
                        return final;
                    }
                    else
                    {
                        //Consulta
                        sqlConsulta = "UPDATE TipoAprendiz SET NomTipoAprendiz = '" + txt_nmb_tipoAprendiz.Text.Trim() + "' WHERE Id_TipoAprendiz = " + idTIpoAprendiz;

                        //Asignar SqlCommand para su ejecución.
                        SqlCommand cmd = new SqlCommand(sqlConsulta, conn);

                        //Ejecutar SQL.
                        correcto = EjecutarSQL(conn, cmd);

                        if (correcto == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo actualizar el Tipo Aprendiz seleccionado.')", true);
                            return final;
                        }
                        else { final = true; return final; }
                    }
                }
                else { return final; }

                //return final;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo actualizar el Tipo Aprendiz seleccionado.')", true);
                return final;
            }
        }

        #endregion

        #region Eliminación.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Borrar el registro "TipoAprendiz" seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Borrar_Click(object sender, EventArgs e)
        {
            if (EliminarTipoAprendiz() == true)
            {
                //Ocultar los campos.
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                hdf_id.Value = "";
                txt_nmb_tipoAprendiz.Text = "";
                pnl_detalles.Visible = false;
                pnlPrincipal.Visible = true;
                EvaluarEnunciado();
            }
        }

        /// <summary>
        /// Eliminar el tipo de aprendiz seleccionado.
        /// </summary>
        private bool EliminarTipoAprendiz()
        {
            //Inicializar variables.
            Int32 idTIpoAprendiz = 0;
            bool correcto = false;
            //Obtiene la conexión
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            //Inicializa la variable para generar la consulta.
            String sqlConsulta = "";
            bool final = false;

            try
            {
                //Se verifica que si se haya pasado el ID a la variable oculta.
                if (!String.IsNullOrEmpty(hdf_id.Value))
                {
                    //Obtener el tipo de aprendiz.
                    idTIpoAprendiz = Convert.ToInt32(hdf_id.Value);

                    if (txt_nmb_tipoAprendiz.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del tipo de aprendiz.')", true);
                        return final;
                    }
                    else
                    {
                        //Consulta
                        sqlConsulta = "DELETE FROM TipoAprendiz WHERE Id_TipoAprendiz = " + idTIpoAprendiz;

                        //Asignar SqlCommand para su ejecución.
                        SqlCommand cmd = new SqlCommand(sqlConsulta, conn);

                        //Ejecutar SQL.
                        correcto = EjecutarSQL(conn, cmd);

                        if (correcto == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo eliminar el Tipo Aprendiz seleccionado.')", true);
                            return final;
                        }
                        else { final = true; return final; }
                    }
                }
                else { return final; }

                return final;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo eliminar el Tipo Aprendiz seleccionado.')", true);
                return final;
            }
        }

        #endregion

        #region Métodos generales.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Dependiendo de qué panel está visible, se establecen ciertos valores.
        /// </summary>
        private void EvaluarEnunciado()
        {
            if (pnlPrincipal.Visible == true)
            {
                pnl_detalles.Visible = false;
                lbl_enunciado.Text = "TIPO DE APRENDIZ";

                CargarTiposDeAprendices();
            }
            if (pnl_detalles.Visible == true)
            {
                if (NuevoDato == true)
                {
                    pnl_detalles.Visible = true;
                    pnlPrincipal.Visible = false;
                    txt_nmb_tipoAprendiz.Visible = true;
                    B_Adicionar.Visible = true;
                    B_Acion.Visible = false;
                    B_Borrar.Visible = false;
                    lbl_enunciado.Text = "NUEVO";
                }
                else
                {
                    pnl_detalles.Visible = true;
                    pnlPrincipal.Visible = false;
                    txt_nmb_tipoAprendiz.Visible = true;
                    B_Adicionar.Visible = false;
                    B_Acion.Visible = true;
                    B_Borrar.Visible = true;
                    lbl_enunciado.Text = "EDITAR";
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Cargar la grilla de "Tipos de Aprendices".
        /// </summary>
        private void CargarTiposDeAprendices()
        {
            //Inicializar variables.
            DataTable tabla = new DataTable();
            String sqlConsulta;

            #region LINQ Comentado.
            //var rs = (from t in consultas.Db.TipoAprendizs
            //          select new
            //          {
            //              t.Id_TipoAprendiz,
            //              t.NomTipoAprendiz
            //          }).ToList(); 
            #endregion

            sqlConsulta = " SELECT Id_TipoAprendiz, NomTipoAprendiz FROM TipoAprendiz ";

            tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

            HttpContext.Current.Session["dt_TiposAprendices"] = tabla;

            gv_tiposDeAprendices.DataSource = tabla;
            gv_tiposDeAprendices.DataBind();

            #region Comentarios
            ////Inicializar variables.
            //var respuestaDetalle = new DataTable();


            //consultas.Parameters = new[]
            //                               {
            //                                   new SqlParameter
            //                                       {
            //                                           ParameterName = "@CodProyecto" ,Value = CodProyecto //CodCargo
            //                                       }
            //                               };

            //respuestaDetalle = consultas.ObtenerDataTable("MD_ListaDePersonalCalificado_Nomina", "text");

            //if (respuestaDetalle.Rows.Count != 0)
            //{
            //    gv_personalCalificado.DataSource = respuestaDetalle;
            //    gv_personalCalificado.DataBind();
            //} 
            #endregion
        }

        #endregion

        #region Eventos del GridView.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Establecer opciones de acuerdo al CommandArgument.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_tiposDeAprendices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar")
            {
                #region Código que separa el Id,  el nombre del TipoAprendiz seleccionado y muestra el panel.

                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Asignación de los valores depurados a los campos correspondientes.
                hdf_id.Value = valores_command[0];
                lbl_idSelected.Text = "Id: " + valores_command[0];
                txt_nmb_tipoAprendiz.Text = valores_command[1];

                //Mostrar la información en los campos correspondientes y evaluar.
                pnlPrincipal.Visible = false;
                pnl_detalles.Visible = true;
                EvaluarEnunciado();

                #endregion
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// Navegación de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_tiposDeAprendices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["dt_TiposAprendices"] as DataTable;

            if (dt != null)
            {
                gv_tiposDeAprendices.PageIndex = e.NewPageIndex;
                gv_tiposDeAprendices.DataSource = dt;
                gv_tiposDeAprendices.DataBind();
            }
        }

        #endregion

        #region Métodos SQL.

        /// <summary>
        /// Mauricio Arias Olave.
        /// Ejecutar SQL.
        /// Método que recibe la conexión y la consulta SQL y la ejecuta.
        /// </summary>
        /// <param name="p_connection">Conexión</param>
        /// <param name="p_cmd">Consulta SQL.</param>
        /// <returns>TRUE = Sentencia SQL ejecutada correctamente. // FALSE = Error.</returns>
        private bool EjecutarSQL(SqlConnection p_connection, SqlCommand p_cmd)
        {
            //Ejecutar controladamente la consulta SQL.
            try
            {
                p_connection.Open();
                p_cmd.ExecuteReader();
                return true;
            }
            catch (Exception) { return false; }
            finally { p_connection.Close(); p_connection.Dispose(); }
            //finally
            //{ p_connection.Close(); }
        }

        #endregion
    }
}