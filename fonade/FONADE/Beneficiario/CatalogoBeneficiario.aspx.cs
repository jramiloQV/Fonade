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

namespace Fonade.FONADE.Beneficiario
{
    /// <summary>
    /// CatalogoBeneficiario
    /// </summary>    
    public partial class CatalogoBeneficiario : Negocio.Base_Page
    {
     
        /// <summary>
        /// Variable que contiene las consultas SQL.
        /// </summary>
        String txtSQL;
        string errorMessageDetail;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrid();
                lbl_Titulo.Text = void_establecerTitulo("BENEFICIARIOS");
                MostrarTabla();
            }
        }

        //protected void lds_beneficiarios_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    try
        //    {
        //        //var consulta = (from Proy in consultas.Db.ProyectoContactos
        //        //                where Proy.CodContacto == usuario.IdContacto
        //        //                & Proy.CodRol == 3
        //        //                select new
        //        //                {
        //        //                    codigo = Proy.CodProyecto
        //        //                }).FirstOrDefault();
        //        var consulta = (from ec in consultas.Db.EmpresaContactos
        //                        join em in consultas.Db.Empresas on ec.codempresa equals em.id_empresa
        //                        where ec.codcontacto == usuario.IdContacto
        //                        select em).FirstOrDefault();

                

        //        if(consulta != null)
        //        {
        //            var codigoProyecto = consulta.codproyecto; // consulta.codigo;
        //            var query = from P in consultas.Db.MD_MostrarBeneficiarios(codigoProyecto)
        //                        select P;
        //            e.Result = query;
        //        }
        //        else
        //        {
        //            e.Result = null;
        //        }
                
        //    }
        //    catch (Exception ex) { errorMessageDetail = ex.Message; }
        //}

        private void CargarGrid()
        {

            var codProyecto = int.Parse(Session["CodProyecto"].ToString());
            if(codProyecto != null)
            {
                var query = (from P in consultas.Db.MD_MostrarBeneficiarios(codProyecto)
                             select P).ToList();

                if(query.Count() > 0)
                {
                    gv_verBeneficiarios.DataSource = query;
                }
                else
                {
                    gv_verBeneficiarios.DataSource = null;
                }
                gv_verBeneficiarios.DataBind();
            }

                        
        }

        /// <summary>
        /// Handles the DataBound event of the gv_verBeneficiarios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gv_verBeneficiarios_DataBound(object sender, EventArgs e)
        {
            l_conteoBenef.Text = gv_verBeneficiarios.Rows.Count.ToString() + " beneficiarios registrados";
        }

        /// <summary>
        /// Handles the Click event of the Img_AgregarBenef control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void Img_AgregarBenef_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Beneficiario.aspx?LoadCode=0");
        }

        /// <summary>
        /// Handles the benef event of the Eliminar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        protected void Eliminar_benef(object sender, CommandEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                
                SqlCommand cmd = new SqlCommand("MD_EliminarBeneficiario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pagobenef", Convert.ToInt32(e.CommandArgument));
                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                
                cmd2.Dispose();
                cmd.Dispose();
                this.gv_verBeneficiarios.DataBind();
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Mostrar la tabla para administración de beneficiaros o los controles según sea el caso.
        /// </summary>
        private void MostrarTabla()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            Int32 CodEstadoProyecto = 0;
            String CodProyecto;

            try
            {
                txtSQL = " SELECT CodEstado, CodProyecto FROM Proyecto P, ProyectoContacto PC " +
                         " WHERE P.id_proyecto = PC.CodProyecto AND PC.Inactivo = 0 " +
                         " AND PC.CodContacto = " + usuario.IdContacto;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    CodEstadoProyecto = Int32.Parse(RS.Rows[0]["CodEstado"].ToString());
                    CodProyecto = RS.Rows[0]["CodProyecto"].ToString();
                }

                if (CodEstadoProyecto < Constantes.CONST_Ejecucion || CodEstadoProyecto >= Constantes.CONST_Asignado_para_acreditacion)
                { tabla_default.Visible = true; tabla_normal.Visible = false; }
                else { tabla_default.Visible = false; tabla_normal.Visible = true; }

            }
            catch { tabla_default.Visible = false; tabla_normal.Visible = true; }
        }
    }
}