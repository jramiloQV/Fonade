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

namespace Fonade.Controles.evaluacion
{
    /// <summary>
    /// CatalogoItem
    /// </summary>
    /// <seealso cref="System.Web.UI.UserControl" />
    public partial class CatalogoItem : System.Web.UI.UserControl
    {
        int codproyecto;
        int codconvocatoria;
        int coditem;
        int puntaje;
        int codaspecto;
        string textoescala;
        string textoitem;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.Parse(HttpContext.Current.Session["CodProyecto"].ToString()) > 0 && int.Parse(HttpContext.Current.Session["CodConvocatoria"].ToString()) > 0)
                {
                    if (int.Parse(HttpContext.Current.Session["CodItem"].ToString()) > 0)
                    {
                        lt_item.Text = "Editar Item";
                        coditem = int.Parse(HttpContext.Current.Session["CodItem"].ToString());
                        DataTable dt_item = new DataTable();
                        dt_item = MD_Item_SelectRow(coditem);
                        foreach (DataRow dr_item in dt_item.Rows)
                        {
                            txt_nombreitem.Value = dr_item["NomItem"].ToString();
                        }
                        dt_item.Dispose();

                        DataTable dt_itemescala = new DataTable();
                        dt_itemescala = MD_ItemEscala_SelectAll(coditem);
                        foreach (DataRow dr_itemescala in dt_item.Rows)
                        {
                            txt_texto.Value = dr_itemescala["NomItem"].ToString();
                            txt_puntaje.Value = dr_itemescala["Puntaje"].ToString();
                        }
                        dt_item.Dispose();
                    }
                    else
                    {
                        lt_item.Text = "Crear Item";
                    }
                }
                else
                {
                    Response.Write("no esta autorizado para seguir");
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btn_crear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_crear_Click(object sender, EventArgs e)
        {
            codproyecto = int.Parse(HttpContext.Current.Session["CodProyecto"].ToString());
            codconvocatoria = int.Parse(HttpContext.Current.Session["CodConvocatoria"].ToString());
            codaspecto = int.Parse(HttpContext.Current.Session["CodAspecto"].ToString());
            puntaje = int.Parse(txt_puntaje.Value);
            textoescala = txt_texto.Value;
            textoitem = txt_nombreitem.Value;

            if (int.Parse(HttpContext.Current.Session["CodItem"].ToString()) > 0)
            {
                coditem = int.Parse(HttpContext.Current.Session["CodItem"].ToString());
                MD_ItemEscala_DeleteRow(coditem);
                MD_ItemEscala_Insert(coditem, textoescala, puntaje);
            }
            else
            {
                int id_item = (int)MD_Item_Insert(textoitem, codaspecto, true);
                MD_ItemEscala_Insert(id_item, textoescala, puntaje);
                MD_EvaluacionEvaluador_Insert(codproyecto, codconvocatoria, id_item, puntaje);
            }
        }

        /// <summary>
        /// Crear Indicador x id
        /// </summary>
        /// <param name="NomItem">The nom item.</param>
        /// <param name="CodTabEvaluacion">The cod tab evaluacion.</param>
        /// <param name="Protegido">if set to <c>true</c> [protegido].</param>
        /// <returns></returns>
        public int MD_Item_Insert(string NomItem, int CodTabEvaluacion, Boolean Protegido)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_Item_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NomItem", NomItem);
            cmd.Parameters.AddWithValue("@CodTabEvaluacion", CodTabEvaluacion);
            cmd.Parameters.AddWithValue("@Protegido", Protegido);
            try
            {
                con.Open();
                int id = (int)cmd.ExecuteScalar();
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return 0;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Actualizar Indicador x id
        /// </summary>
        /// <param name="Id_Item">The identifier item.</param>
        /// <param name="NomItem">The nom item.</param>
        protected void MD_Item_Update(int Id_Item, string NomItem)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_Item_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id_Item", Id_Item);
            cmd.Parameters.AddWithValue("@NomItem", NomItem);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Consultar  Indicador x id
        /// </summary>
        /// <param name="Id_Item">The identifier item.</param>
        /// <returns></returns>
        public DataTable MD_Item_SelectRow(int Id_Item)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_Item_SelectRow", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id_Item", Id_Item);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new DataTable();

                }
                return null;
            }
        }

        /// <summary>
        /// Crear ItemEscala x id.
        /// </summary>
        /// <param name="CodItem">The cod item.</param>
        /// <param name="Texto">The texto.</param>
        /// <param name="Puntaje">The puntaje.</param>
        /// <returns></returns>
        public int MD_ItemEscala_Insert(int CodItem, string Texto, int Puntaje)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_Item_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodItem", CodItem);
            cmd.Parameters.AddWithValue("@Texto", Texto);
            cmd.Parameters.AddWithValue("@Puntaje", Puntaje);
            try
            {
                con.Open();
                int id = (int)cmd.ExecuteScalar();
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                return 0;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Actualizar ItemEscala x id
        /// </summary>
        /// <param name="CodItem">The cod item.</param>
        /// <param name="Texto">The texto.</param>
        /// <param name="Puntaje">The puntaje.</param>
        public void MD_ItemEscala_Update(int CodItem, string Texto, int Puntaje)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_ItemEscala_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodItem", CodItem);
            cmd.Parameters.AddWithValue("@Texto", Texto);
            cmd.Parameters.AddWithValue("@Puntaje", Puntaje);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Consultar  Indicador x id
        /// </summary>
        /// <param name="CodItem">The cod item.</param>
        /// <returns></returns>
        public DataTable MD_ItemEscala_SelectAll(int CodItem)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_ItemEscala_SelectAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodItem", CodItem);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new DataTable();

                }
                return null;
            }
        }

        /// <summary>
        /// Eliminar Indicador x id
        /// </summary>
        /// <param name="CodItem">The cod item.</param>
        /// <exception cref="Exception"></exception>
        public void MD_ItemEscala_DeleteRow(int CodItem)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "MD_ItemEscala_DeleteRow";
                    com.CommandType = System.Data.CommandType.StoredProcedure;        
                    com.Parameters.AddWithValue("@CodItem", CodItem);
                    try
                    {
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        com.Dispose();
                        con.Close();
                        con.Dispose();
                    }
                }
            }

        }

        /// <summary>
        /// Crear Indicador x id
        /// </summary>
        /// <param name="cod_proyecto">The cod proyecto.</param>
        /// <param name="cod_convocatoria">The cod convocatoria.</param>
        /// <param name="CodItem">The cod item.</param>
        /// <param name="Puntaje">The puntaje.</param>
        protected void MD_EvaluacionEvaluador_Insert(int cod_proyecto, int cod_convocatoria, int CodItem, int Puntaje)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_EvaluacionEvaluador_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codProyecto", cod_proyecto);
            cmd.Parameters.AddWithValue("@codConvocatoria", cod_convocatoria);
            cmd.Parameters.AddWithValue("@CodItem", CodItem);
            cmd.Parameters.AddWithValue("@Puntaje", Puntaje);
            try
            {
                con.Open();
                int id = (int)cmd.ExecuteScalar();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Actualizar Indicador x id
        /// </summary>
        /// <param name="cod_proyecto">The cod proyecto.</param>
        /// <param name="cod_convocatoria">The cod convocatoria.</param>
        /// <param name="CodItem">The cod item.</param>
        /// <param name="Puntaje">The puntaje.</param>
        protected void MD_EvaluacionEvaluador_Update(int cod_proyecto, int cod_convocatoria, int CodItem, int Puntaje)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_EvaluacionEvaluador_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codProyecto", cod_proyecto);
            cmd.Parameters.AddWithValue("@codConvocatoria", cod_convocatoria);
            cmd.Parameters.AddWithValue("@CodItem", CodItem);
            cmd.Parameters.AddWithValue("@Puntaje", Puntaje);
            try
            {
                con.Open();
                int id = (int)cmd.ExecuteScalar();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Consultar  Indicador x id
        /// </summary>
        /// <param name="id_Indicador">The identifier indicador.</param>
        /// <returns></returns>
        public DataTable sp_EvaluacionProyectoIndicador_SelectRow(int id_Indicador)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_EvaluacionProyectoIndicador_SelectRow", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_Indicador", id_Indicador);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new DataTable();

                }
                return null;
            }
        }

    }
}