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
using Fonade.Clases;
using System.Text.RegularExpressions;
using System.Web;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// DesactivarUnidadEmprende
    /// </summary>    
    public partial class DesactivarUnidadEmprende : Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Código de la institución seleccionada para su desactivación.
        /// </summary>
        string Cod_Seleccionado;

        /// <summary>
        /// Variable que contiene las consultas SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Fecha actual.
        /// </summary>
        DateTime fecha_actual = DateTime.Today;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Cod_Seleccionado = Session["idUnidad"].ToString();

                txtSQL = " SELECT NomInstitucion, NomUnidad FROM Institucion WHERE Id_Institucion = " + Cod_Seleccionado;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                lbl_titulo_data_desactivar.Text = "Desactivar " + dt.Rows[0]["NomInstitucion"].ToString() + " - " + dt.Rows[0]["NomUnidad"].ToString();

                dt = null;

                #region Cargar la fecha actual.
                if (!IsPostBack)
                {
                    //Cargar la fecha actual.
                    lbl_fechaInicio.Text = fecha_actual.ToString("M/dd/yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                    DDL_Dia.SelectedValue = fecha_actual.Day.ToString();
                    DDL_Mes.SelectedValue = fecha_actual.Month.ToString();
                    DD_Anio.SelectedValue = fecha_actual.Year.ToString();
                }
                #endregion
            }
            catch
            {
                ClientScriptManager cm = this.ClientScript;
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>javascript:window.opener.location.reload(true);self.close();</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "RefrescarVentanaPadre()", true);
            }
        }

        /// <summary>
        /// Desactivar institución seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_desactivar_Click(object sender, EventArgs e)
        {
            DesactivarUnidad();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "RefrescarVentanaPadre()", true);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 26/06/2014.
        /// Desactivar unidad de emprendimiento seleccionada / institución seleccionado.
        /// </summary>
        /// <returns>TRUE = Proceso correcto. // FALSE = Error.</returns>
        private void DesactivarUnidad()
        {
            //Inicializar variables.
            var fecInicio = new DateTime();
            var fecFin = new DateTime();
            var RS = new DataTable();
            var CodJefeUnidad = "";
            var rstProyecto = new DataTable();
            var Caso = "";

            //Obtener las fechas de inicio y fin.
            fecInicio = fecha_actual.Date;
            fecFin = Convert.ToDateTime(DD_Anio.SelectedValue + "/" + DDL_Mes.SelectedValue + "/" + DDL_Dia.SelectedValue).Date;
            //fecFin = DateTime.ParseExact(DDL_Mes.SelectedValue + "-" + DDL_Dia.SelectedValue + "-" + DD_Anio.SelectedValue, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            #region Actualizar datos de acuerdo a si ha chequeado el CheckBox "Desactivar indefinidamente".

            if (!txtIndefinido.Checked)
            {
                Caso = "TRUE";
            }
            else
            {
                Caso = "FALSE";
            }

            ////Ejecutar consulta SQL "con fecha".
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                //NEW RESULTS:

                SqlCommand cmd = new SqlCommand("MD_DesactivarUnidadEmprendimiento", con);

                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Caso", Caso);
                cmd.Parameters.AddWithValue("@CodInstitucion ", Cod_Seleccionado);
                cmd.Parameters.AddWithValue("@FechaInicioInactivo", fecInicio);
                cmd.Parameters.AddWithValue("@FechaFinInactivo", fecFin);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error' : " + ex.Message + " );", true);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            #endregion

            #region Deja Inactivos los usuarios Jefes de unidad y asesores que pertenecen a esa institución.

            txtSQL = " UPDATE Contacto SET Inactivo = 1, CodInstitucion = NULL " +
                     " WHERE CodInstitucion = " + Cod_Seleccionado +
                     " AND Id_Contacto IN(SELECT CodContacto FROM GrupoContacto WHERE CodGrupo IN(" + Constantes.CONST_JefeUnidad + "," + Constantes.CONST_Asesor + ")) ";

            //Ejecutar consulta SQL.
            ejecutaReader(txtSQL, 2);

            #endregion

            #region Trae el jefe de la unidad.

            txtSQL = " SELECT CodContacto FROM InstitucionContacto WHERE FechaFin IS NULL AND CodInstitucion = " + Cod_Seleccionado;

            RS = consultas.ObtenerDataTable(txtSQL, "text");

            if (RS.Rows.Count > 0)
            {
                CodJefeUnidad = RS.Rows[0]["CodContacto"].ToString();

                //Deja a los jefes de unidad sin Rol.
                txtSQL = " DELETE FROM GrupoContacto WHERE CodContacto = " + CodJefeUnidad;

                //Ejecutar consulta SQL.
                ejecutaReader(txtSQL, 2);
            }

            #endregion

            #region Cierra la relacion entre la Institución y el jefe de la unidad.

            txtSQL = " UPDATE InstitucionContacto SET FechaFin = GETDATE() WHERE FechaFin IS NULL " +
                     " AND CodInstitucion = " + Cod_Seleccionado;

            //Ejecutar consulta SQL.
            ejecutaReader(txtSQL, 2);

            #endregion

            #region Reasigna los proyectos de la unidad.

            txtSQL = " SELECT Id_Proyecto, NomProyecto, CodEstado FROM Proyecto WHERE CodInstitucion = " + Cod_Seleccionado;

            rstProyecto = consultas.ObtenerDataTable(txtSQL, "text");

            if (rstProyecto.Rows.Count > 0)
            {
                foreach (DataRow row_rstProyecto in rstProyecto.Rows)
                {
                    //Trae los asesores del proyecto.
                    txtSQL = " SELECT PC.CodContacto FROM ProyectoContacto PC, Proyecto P " +
                             " WHERE PC.CodProyecto = P.Id_Proyecto" +
                             " AND PC.CodRol IN(" + Constantes.CONST_RolAsesorLider + "," + Constantes.CONST_RolAsesor + ")" +
                             " AND PC.FechaFin IS NULL" +
                             " AND PC.Inactivo = 0" +
                             " AND PC.CodProyecto = " + row_rstProyecto["Id_Proyecto"].ToString();

                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                    #region Deja a los asesores sin rol.

                    if (RS.Rows.Count > 0)
                    {
                        foreach (DataRow row_RS in RS.Rows)
                        {
                            txtSQL = " DELETE FROM GrupoContacto WHERE CodContacto = " + row_RS["CodContacto"].ToString();

                            //Ejecutar consulta SQL.
                            ejecutaReader(txtSQL, 2);
                        }
                    }


                    #endregion

                    #region Deja el proyecto sin asesores.

                    txtSQL = " UPDATE ProyectoContacto SET FechaFin = GETDATE(), Inactivo = 1 " +
                             " WHERE CodProyecto = " + row_rstProyecto["Id_Proyecto"].ToString() +
                             " AND CodRol IN(" + Constantes.CONST_RolAsesorLider + "," + Constantes.CONST_RolAsesor + ") ";

                    //Ejecutar consulta SQL.
                    ejecutaReader(txtSQL, 2);

                    #endregion

                    //Si al proyecto no le han asignado recursos pasara a REASIGNACION POR ASIGNACION:
                    if (Int32.Parse(row_rstProyecto["CodEstado"].ToString()) < Constantes.CONST_AsignacionRecursos)
                    {
                        #region Ejecutar bloque #1.

                        #region Actualización #1.

                        txtSQL = " UPDATE Proyecto SET CodInstitucion = " + Constantes.CONST_UnidadTemporal +
                                                 " WHERE Id_Proyecto = " + row_rstProyecto["Id_Proyecto"].ToString();

                        //Ejecutar consulta SQL.
                        ejecutaReader(txtSQL, 2);

                        #endregion

                        #region Los emprendedores pasan a la unidad temporal junto con sus proyectos (Actualización #2).

                        txtSQL = " UPDATE Contacto set CodInstitucion = " + Constantes.CONST_UnidadTemporal +
                                 " where id_contacto in (select codcontacto from proyectocontacto " +
                                 " where codproyecto = " + row_rstProyecto["Id_Proyecto"].ToString() + " and inactivo=0) ";

                        //Ejecutar consulta SQL.
                        ejecutaReader(txtSQL, 2);

                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region Ejecutar bloque #2.

                        #region Si al proyecto ya le han asignado recursos pasara a REASIGNACION POR SEGUIMIENTO.

                        txtSQL = " UPDATE Proyecto SET CodInstitucion = " + Constantes.CONST_UnidadTemporal +
                                 " WHERE Id_Proyecto = " + row_rstProyecto["Id_Proyecto"].ToString();

                        //Ejecutar consulta SQL.
                        ejecutaReader(txtSQL, 2);

                        #endregion

                        #region Los emprendedores pasan a la unidad temporal junto con sus proyectos.

                        txtSQL = " UPDATE Contacto set CodInstitucion = " + Constantes.CONST_UnidadTemporal +
                                 " where id_contacto in (select codcontacto from proyectocontacto " +
                                 " where codproyecto = " + row_rstProyecto["Id_Proyecto"].ToString() +
                                 " and inactivo = 0) ";

                        //Ejecutar consulta SQL.
                        ejecutaReader(txtSQL, 2);

                        #endregion

                        #endregion
                    }
                }
            }

            #endregion
        }
    }
}