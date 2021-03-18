using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// AnexoEmprendedor
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class AnexoEmprendedor : System.Web.UI.Page
    {
        String CodProyecto;
        String CodContacto;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(HttpContext.Current.Session["commadArgumentAnexoEmprendedor"].ToString()))
                {
                    Response.Redirect("ReporteFinalAcreditacion.aspx");
                }
                else
                {
                    String[] valores = HttpContext.Current.Session["commadArgumentAnexoEmprendedor"].ToString().Split(';');
                    CodProyecto = valores[0];
                    CodContacto = valores[1];
                }
            }
            catch (Exception) { }

            String sql = "SELECT NOMBRES  + ' ' + APELLIDOS FROM CONTACTO WHERE ID_CONTACTO=" + CodContacto;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    L_Conctat.Text = "Anexos del emprendedor: " + reader[0].ToString();
                }

            }
            catch (SqlException) { }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            buscarAnexos();
        }
        /// <summary>
        /// sentencias sql para la busqueda de contacto archivos anexos
        /// </summary>
        private void buscarAnexos()
        {

            String strTipoArchivo;
  		    String strDescripcion;

            DataTable puDatatable = new DataTable();

            puDatatable.Columns.Add("Archivo");
            puDatatable.Columns.Add("Tipo");
            puDatatable.Columns.Add("Descripcion");

            DataTable Anexos = new DataTable();

            Consultas consulta = new Consultas();

            // Se crea una nueva consulta

            String sql = @"SELECT  cast(a.Ruta as varchar(100)) as Ruta, cast(e.TituloObtenido as varchar(100)) as TituloObtenido, cast(es.NomNivelEstudio as varchar(100)) as NomNivelEstudio, cast(c.Nombres as varchar(100)) as Nombres, cast(c.Apellidos as varchar(100)) as Apellidos, cast(T1.Texto as varchar(100)) as TipoArchivo, cast(T2.Texto as varchar(100)) as TipoArchivoDescripcion 
            , cast((case when a.CodContactoEstudio is null then '' else  e.TituloObtenido + ' (' +  es.NomNivelEstudio + ')' end) as varchar(100) ) as Descripcion  
            , cast((isnull(e.anotitulo,datepart(year,getdate())) ) as varchar(10)) as ano_titulo 
            FROM ContactoArchivosAnexos AS a 
            LEFT OUTER JOIN Contacto c ON c.Id_Contacto = a.CodContacto 
            LEFT OUTER JOIN ContactoEstudio AS e  on e.Id_ContactoEstudio = a.CodContactoEstudio
            LEFT OUTER JOIN NivelEstudio AS es ON e.CodNivelEstudio = es.Id_NivelEstudio 
            LEFT OUTER JOIN texto AS T1 ON T1.NomTexto=A.TipoArchivo 
            LEFT OUTER JOIN texto AS T2 ON T2.NomTexto=CONCAT(A.TipoArchivo,'_desc')  
            WHERE a.CodProyecto = " + CodProyecto + @" AND TipoArchivo ='Anexo1'
            ORDER BY a.TipoArchivo, c.Id_Contacto, ano_titulo DESC";

            Anexos = consulta.ObtenerDataTable(sql, "text");

            if (Anexos != null)
            {
                if (Anexos.Rows.Count > 0)
                {
                    DataRow fila = puDatatable.NewRow();

                    strTipoArchivo = Anexos.Rows[0]["TipoArchivo"].ToString();
                    strDescripcion = strTipoArchivo + " - " + Anexos.Rows[0]["TipoArchivoDescripcion"].ToString();

                    fila["Archivo"] = Anexos.Rows[0]["ruta"].ToString();
                    fila["Tipo"] = strTipoArchivo;
                    fila["Descripcion"] = strDescripcion;
                    puDatatable.Rows.Add(fila);
                }
            }

            String sql2 = @"SELECT  cast(a.Ruta as varchar(100)) as Ruta, cast(e.TituloObtenido as varchar(100)) as TituloObtenido, cast(es.NomNivelEstudio as varchar(100)) as NomNivelEstudio, cast(c.Nombres as varchar(100)) as Nombres, cast(c.Apellidos as varchar(100)) as Apellidos, cast(T1.Texto as varchar(100)) as TipoArchivo, cast(T2.Texto as varchar(100)) as TipoArchivoDescripcion 
            , cast((case when a.CodContactoEstudio is null then '' else  e.TituloObtenido + ' (' +  es.NomNivelEstudio + ')' end) as varchar(100) ) as Descripcion  
            , cast((isnull(e.anotitulo,datepart(year,getdate())) ) as varchar(10)) as ano_titulo 
            FROM ContactoArchivosAnexos AS a 
            LEFT OUTER JOIN Contacto c ON c.Id_Contacto = a.CodContacto 
            LEFT OUTER JOIN ContactoEstudio AS e  on e.Id_ContactoEstudio = a.CodContactoEstudio
            LEFT OUTER JOIN NivelEstudio AS es ON e.CodNivelEstudio = es.Id_NivelEstudio 
            LEFT OUTER JOIN texto AS T1 ON T1.NomTexto=A.TipoArchivo 
            LEFT OUTER JOIN texto AS T2 ON T2.NomTexto=CONCAT(A.TipoArchivo,'_desc')  
            WHERE a.CodProyecto = " + CodProyecto + " AND a.codContacto= " + CodContacto + @"
            ORDER BY a.TipoArchivo, c.Id_Contacto, ano_titulo Desc";

            Anexos = consulta.ObtenerDataTable(sql2, "text");

            if (Anexos != null)
            {
                if (Anexos.Rows.Count > 0)
                {
                    for (int i = 0; i < Anexos.Rows.Count; i++)
                    {
                        DataRow fila = puDatatable.NewRow();

                        strTipoArchivo = Anexos.Rows[i]["TipoArchivo"].ToString();
                        strDescripcion = strTipoArchivo + " - " + Anexos.Rows[i]["TipoArchivoDescripcion"].ToString();

                        fila["Archivo"] = Anexos.Rows[i]["ruta"].ToString();
                        fila["Tipo"] = strTipoArchivo;
                        fila["Descripcion"] = strDescripcion;
                        puDatatable.Rows.Add(fila);
                    }
                }
            }

            GV_Anexos.DataSource = puDatatable;
            GV_Anexos.DataBind();
        }
    }
}