#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Archivo>CargueSNIES.cs</Archivo>

#endregion

using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// CargueSNIES
    /// </summary>    
    public partial class CargueSNIES : Negocio.Base_Page
    {
        string txtSQL;
        string error;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Selecting event of the lds_nivelestduio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_nivelestduio_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = from c in consultas.Db.NivelEstudios
                         orderby c.NomNivelEstudio
                         select new
                         {
                             c.Id_NivelEstudio,
                             c.NomNivelEstudio
                         };

            e.Result = result.ToList();
        }

        /// <summary>
        /// Handles the Click event of the btncargar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btncargar_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;

            if (!fld_cargar.HasFile)
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No ha subido ningun archivo');</script>");
                return;
            }
            else
            {
                if (fld_cargar.PostedFile.ContentLength > 10485760)
                {
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El tamaño del archivo debe ser menor a 10 Mb.');</script>");
                    return;
                }
                else
                {
                    #region Iniciar con el procesamiento del archivo.

                    string nombreArchivo;
                    string extencionArchivo;

                    nombreArchivo = System.IO.Path.GetFileName(fld_cargar.PostedFile.FileName);
                    extencionArchivo = System.IO.Path.GetExtension(fld_cargar.PostedFile.FileName);

                    if (string.IsNullOrEmpty(nombreArchivo))
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No ha subido ningun archivo');</script>");
                        return;
                    }
                    if (!(extencionArchivo.Equals(".xls") || extencionArchivo.Equals(".XLS") || extencionArchivo.Equals(".xlsx") || extencionArchivo.Equals(".XLSX")))
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Tipo De Archivo No Valido');</script>");
                        return;
                    }

                    if (string.IsNullOrEmpty(ddl_nivelestudio.SelectedValue))
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Seleccione el nivel de estudio');</script>");
                        return;
                    }

                    string saveLocation = Server.MapPath("\\FonadeDocumentos\\CargueSNIES") + "\\" + nombreArchivo;

                    if ((System.IO.File.Exists(saveLocation)))
                    {
                        System.IO.File.Delete(saveLocation);
                    }

                    if (!(System.IO.File.Exists(saveLocation)))
                    {
                        fld_cargar.PostedFile.SaveAs(saveLocation);
                    }
                    else
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Ya se encuantra almacenado un archivo con este nombre');</script>");
                        return;
                    }

                    DataTable dt = Excel.recogerExcel(saveLocation, "cargue");

                    if (dt.Rows.Count > 0)
                    {
                        error = string.Empty;

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (string.IsNullOrEmpty(dr[0].ToString())
                                && string.IsNullOrEmpty(dr[1].ToString())
                                && string.IsNullOrEmpty(dr[2].ToString())
                                && string.IsNullOrEmpty(dr[3].ToString())
                                && string.IsNullOrEmpty(dr[4].ToString())
                                && string.IsNullOrEmpty(dr[7].ToString()))
                            {
                                if (dr[0].Equals("1"))
                                {
                                    string idPrograma = dr[3].ToString();

                                    txtSQL = "IF NOT EXISTS(SELECT ID_INSTITUCIONEDUCATIVA FROM INSTITUCIONEDUCATIVA WHERE ID_INSTITUCIONEDUCATIVA=@ID)" +
                                        "INSERT INTO INSTITUCIONEDUCATIVA VALUES(" + dr[0].ToString() + ",'" + dr[1].ToString() + "')";

                                    instruccion(txtSQL, 2);

                                    txtSQL = " SELECT ID_PROGRAMAACADEMICO FROM PROGRAMAACADEMICO WHERE ID_PROGRAMAACADEMICO=" + idPrograma;

                                    SqlDataReader reader = instruccion(txtSQL, 1);

                                    if (reader.Read())
                                    {
                                        txtSQL = "Update PROGRAMAACADEMICO set NOMPROGRAMAACADEMICO=" + dr[4].ToString();
                                        txtSQL = txtSQL + ", NOMBRE='" + dr[2].ToString() + "'";
                                        txtSQL = txtSQL + ", CODINSTITUCIONEDUCATIVA=" + dr[0].ToString();
                                        txtSQL = txtSQL + ", ESTADO=" + dr[5].ToString();
                                        txtSQL = txtSQL + ", METODOLOGIA=" + dr[6].ToString();
                                        txtSQL = txtSQL + ", NOMMUNICIPIO='" + dr[7].ToString() + "'";
                                        txtSQL = txtSQL + ", NOMDEPARTAMENTO='" + dr[8].ToString() + "'";
                                        txtSQL = txtSQL + ", CODNIVELESTUDIO='" + ddl_nivelestudio.SelectedValue + "'";
                                        txtSQL = txtSQL + ", CODCIUDAD=111";
                                        txtSQL = txtSQL + " WHERE ID_PROGRAMAACADEMICO=" + idPrograma;

                                        instruccion(txtSQL, 2);
                                    }
                                    else
                                    {
                                        txtSQL = "insert into PROGRAMAACADEMICO (ID_PROGRAMAACADEMICO,NOMPROGRAMAACADEMICO,NOMBRE,CODINSTITUCIONEDUCATIVA,ESTADO,";
                                        txtSQL = txtSQL + " METODOLOGIA,NOMMUNICIPIO,NOMDEPARTAMENTO,CODNIVELESTUDIO,CODCIUDAD) values";
                                        txtSQL = txtSQL + "(" + idPrograma + ",'" + dr[4].ToString() + "','" + dr[2].ToString() + "','" + dr[0].ToString() + "',";

                                        if (string.IsNullOrEmpty(dr[5].ToString()))
                                            txtSQL = txtSQL + "null";
                                        else
                                            txtSQL = txtSQL + dr[5].ToString();

                                        if (string.IsNullOrEmpty(dr[6].ToString()))
                                            txtSQL = txtSQL + ",null";
                                        else
                                            txtSQL = txtSQL + dr[6].ToString();

                                        if (string.IsNullOrEmpty(dr[7].ToString()))
                                            txtSQL = txtSQL + ",null";
                                        else
                                            txtSQL = txtSQL + dr[7].ToString();

                                        if (string.IsNullOrEmpty(dr[8].ToString()))
                                            txtSQL = txtSQL + ",null";
                                        else
                                            txtSQL = txtSQL + dr[8].ToString();

                                        txtSQL = txtSQL + ",'" + ddl_nivelestudio.SelectedValue + "'";
                                        txtSQL = txtSQL + ",'" + "111" + "'";
                                        txtSQL = txtSQL + ")";

                                        instruccion(txtSQL, 2);
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(error))
                                    error = dr[0].ToString() + " - " + dr[1].ToString();
                                else
                                    error += dr[0].ToString() + " - " + dr[1].ToString();
                            }
                        }
                    }
                    else
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('el archivo de excel no posee la informacion de la manera correcta');</script>");
                        return;
                    }

                    if ((System.IO.File.Exists(saveLocation)))
                    {
                        System.IO.File.Delete(saveLocation);
                    }

                    if (string.IsNullOrEmpty(error))
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El archivo ha sido cargado Satisfactoriamente');</script>");
                        return;
                    }
                    else
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Los Siguientes programas: <br>" + error + " no han sido cargado debido a no estar asociados a una institución o no poseen los datos de programa (PRO_CONSECUTIVO,PROG_NOMBRE,IES_NOMBRE). Por favor Verificar');</script>");
                        return;
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// Instruccions the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public SqlDataReader instruccion(String sql, int obj)
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
                conn.Dispose();

                conn.Open();

                if (obj == 1)
                    reader = cmd.ExecuteReader();
                else
                    cmd.ExecuteReader();
            }
            catch (SqlException) { }
            finally
            {
                if (conn != null)
                    conn.Close();
                conn.Dispose();
            }

            return reader;
        }
    }
}