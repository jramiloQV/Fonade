#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Archivo>CarguesContratos.cs</Archivo>

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
    /// CarguesContratos
    /// </summary>    
    public partial class CarguesContratos : Negocio.Base_Page
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
        /// Handles the Click event of the btncargar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btncargar_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;

            string nombreArchivo;
            string extencionArchivo;

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

                    string saveLocation = string.Format(Server.MapPath(@"\FonadeDocumentos\CargueMasivo\{0}"), nombreArchivo);

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
                            // 2015/07/09 Roberto Alvarado
                            //txtSQL = string.Format("SELECT id_empresa FROM empresa where codproyecto={0}", Convert.IsDBNull(dr[1]) ? 0 : dr[1]);
                            txtSQL = string.Format("SELECT id_empresa FROM empresa where codproyecto={0}", Convert.IsDBNull(dr[0]) ? 0 : dr[0]);

                            SqlDataReader reader = instruccion(txtSQL, 1);
                            if (reader != null && !Convert.IsDBNull(dr[1]))
                            {
                                if (reader.Read())
                                {
                                    string idempresa = reader[0].ToString();
                                    txtSQL = "select * from contratoempresa where codempresa=" + idempresa;
                                    reader = instruccion(txtSQL, 1);
                                    if (reader.Read())
                                    {
                                        txtSQL = string.Format("Update contratoempresa set Numerocontrato={0}, ObjetoContrato='{1}'," +
                                        "ValorInicialEnPesos={2}, PlazoContratoMeses={3},NumeroAPContrato={4}, FechaAP=CONVERT(DATETIME,'{5}',102), FechaFirmaDelContrato=CONVERT(DATETIME,'{6}',102), " + "NumeroPoliza='{7}',CompaniaSeguros='{8}', FechaDeInicioContrato=CONVERT(DATETIME,'{9}',102) where codempresa={10}",
                                         dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(),
                                         dr[8].ToString(), dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), idempresa);
                                        instruccion(txtSQL, 2);
                                    }
                                    else
                                    {
                                        txtSQL =
                                        string.Format("insert into contratoempresa (codempresa,Numerocontrato,ObjetoContrato,ValorInicialEnPesos," + "PlazoContratoMeses, NumeroAPContrato,FechaAP,FechaFirmaDelContrato,NumeroPoliza,CompaniaSeguros," +
                                         "FechaDeInicioContrato)values({0},{1},'{2}',{3},{4},{5},CONVERT(DATETIME,'{6}',102),CONVERT(DATETIME,'{7}',102),'{8}','{9}',CONVERT(DATETIME,'{10}',102))", idempresa,
                                         dr[2] ?? string.Empty, dr[3] ?? string.Empty, dr[4] ?? string.Empty, dr[5] ?? string.Empty, dr[6] ?? string.Empty,
                                         dr[7] ?? string.Empty, dr[8] ?? string.Empty, dr[9] ?? string.Empty, dr[10] ?? string.Empty, dr[11] ?? string.Empty);
                                        instruccion(txtSQL, 2);
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
                    }
                    else
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('el archivo de excel no posee la informacion de la manera correcta {0}');</script>");
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
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Los Siguientes Contratos: " + error + " no han sido cargado debido a no estar asociados a una empresa. Por favor Verificar');</script>");
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
            try
            {
            }
            catch (SqlException se) { }
            conn.Close();
            conn.Dispose();
            //finally
            //{
            //    if (conn != null)
            //        conn.Close();
            //}

            return reader;
        }
    }
}