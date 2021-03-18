using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// AdministrarAsesores
    /// </summary>    
    public partial class AdministrarAsesores : Negocio.Base_Page
    {
        //int TotalFilasActuales = 0;
        //int TotalFilas = 0;

        /// <summary>
        /// The page size
        /// </summary>
        public const int PAGE_SIZE = 10;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            pnl_asesores.Visible = false;
            if (usuario.CodGrupo == Constantes.CONST_GerenteAdministrador)
            {

                pnl_asesores.Visible = false;
            }
            if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
            {

                pnl_asesores.Visible = true;
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gw_Asesores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gw_Asesores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Conteo;
            Conteo = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Conteo"));
            ImageButton imbtton = new ImageButton();
            imbtton = ((ImageButton)e.Row.Cells[0].FindControl("btn_Inactivar"));
            Button hlConteo = new Button();
            if (e.Row.Cells.Count > 1)
                hlConteo = ((Button)e.Row.Cells[4].FindControl("hl_conteo"));
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Conteo != "0")
                { imbtton.Visible = false; }
                else
                { imbtton.Visible = true;
                if (e.Row.Cells.Count > 1)
                    hlConteo.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Handles the RowCreated event of the gw_Asesores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gw_Asesores_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        // buscar el link del header
                        LinkButton lnk = (LinkButton)tc.Controls[0];
                        if (lnk != null && gw_Asesores.SortExpression == lnk.CommandArgument)
                        {
                            // inicializar nueva imagen
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            // url de la imagen dinamicamente
                            img.ImageUrl = "/Images/ImgFlechaOrden" + (gw_Asesores.SortDirection == SortDirection.Ascending ? "Up" : "Down") + ".gif";
                            // a ñadir el espacio de la imagen
                            tc.Controls.Add(new LiteralControl(" "));
                            tc.Controls.Add(img);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gw_Asesores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gw_Asesores_PageIndexChanged(object sender, GridViewPageEventArgs e)
        { }

        /// <summary>
        /// Handles the Selecting event of the lds_Asesores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_Asesores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var query = (
                from c in consultas.Db.Contacto
                from gc in consultas.Db.GrupoContactos

                where c.CodInstitucion == usuario.CodInstitucion &
                       c.Id_Contacto == gc.CodContacto &
                       gc.CodGrupo == 5
                select new
                {
                    CodTipoIdentificacion = c.CodTipoIdentificacion,
                    Identificacion = c.Identificacion,
                    Nombres = c.Nombres,
                    Apellidos = c.Apellidos,
                    Email = c.Email,
                    Experiencia = c.Experiencia,
                    Dedicacion = c.Dedicacion,
                    HojaVida = c.HojaVida,
                    Intereses = c.Intereses,
                    Id_contacto = c.Id_Contacto,
                    Conteo = (from pc in consultas.Db.ProyectoContactos
                              where pc.CodContacto == c.Id_Contacto &
                              pc.Inactivo == false & pc.FechaFin == null
                              select pc.CodProyecto).Count()
                });
            e.Arguments.TotalRowCount = query.Count();
            if (e.Arguments.TotalRowCount == 0)
            {
                Lbl_Resultados.Visible = true;
                Lbl_Resultados.Text = "No tiene actividades para este rango de fechas";
            }
            else
            { Lbl_Resultados.Visible = false; }

            query = query.Skip(gw_Asesores.PageIndex * PAGE_SIZE).Take(PAGE_SIZE);
            e.Result = query.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 08 - 07 - 2014
        /// llama a un nuevo formulario
        /// que permite asignar asesores a un plan de negocio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkasesores_Click(object sender, EventArgs e)
        {
            Redirect(null, "FrameAsesorProyecto.aspx", "_Blank", "width=1024,height=668");
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/08/2014.
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_Asesores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "eliminar":
                    #region Eliminar... LINQ.

                    //int conteo;
                    //var query = from tu in consultas.Db.TareaUsuarios
                    //            from tur in consultas.Db.TareaUsuarioRepeticions
                    //            where tu.Id_TareaUsuario == tur.CodTareaUsuario & tur.FechaCierre == null
                    //                   & tu.CodContacto == Int32.Parse(e.CommandArgument.ToString())
                    //            select new
                    //            {
                    //                IdTareaUsuario = tu.Id_TareaUsuario
                    //            };

                    //conteo = query.Count();
                    //if (conteo == 0)
                    //{
                    //    var query2 = (from gc in
                    //                      consultas.Db.GrupoContactos
                    //                  where gc.CodContacto == Int32.Parse(e.CommandArgument.ToString())
                    //                  select gc
                    //                     );
                    //    consultas.Db.GrupoContactos.DeleteAllOnSubmit(query2);
                    //    consultas.Db.SubmitChanges();
                    //    Lbl_Resultados.Visible = true;
                    //    Lbl_Resultados.Text = e.CommandArgument.ToString();
                    //}
                    //else
                    //{
                    //    Lbl_Resultados.Visible = true;
                    //    Lbl_Resultados.Text = "No se puede borrar el usuario porque tiene tareas pendientes abiertas";
                    //}

                    #endregion

                    #region Eliminar... SQL.

                    //Inicializar variables.
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    SqlCommand cmd = new SqlCommand();
                    String txtSQL = "";
                    String CodContacto = e.CommandArgument.ToString();

                    //Revisar si tiene tareas pendientes.
                    txtSQL = " select count(id_tareausuario) as cuantos from tareausuario,tareausuariorepeticion " +
                             " where id_tareausuario=codtareausuario and  fechacierre is null and codcontacto=" + CodContacto;
                    var RS = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RS.Rows.Count > 0)
                    {
                        if (Int32.Parse(RS.Rows[0]["Cuantos"].ToString()) == 0)
                        {
                            #region Eliminación.

                            txtSQL = "DELETE FROM GrupoContacto WHERE CodContacto = " + CodContacto;

                            try
                            {
                                //NEW RESULTS:
                                cmd = new SqlCommand(txtSQL, con);

                                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                cmd.CommandType = CommandType.Text;
                                cmd.ExecuteNonQuery();
                                
                                cmd.Dispose();
                            }
                            catch { }
                            finally
                            {
                                con.Close();
                                con.Dispose();
                            }

                            #endregion

                            #region Actualización.

                            txtSQL = " UPDATE Contacto SET Inactivo = 1, codinstitucion=null WHERE Id_Contacto = " + CodContacto;

                            try
                            {
                                //NEW RESULTS:
                                cmd = new SqlCommand(txtSQL, con);

                                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                cmd.CommandType = CommandType.Text;
                                cmd.ExecuteNonQuery();
                                
                                cmd.Dispose();
                            }
                            catch { }
                            finally
                            {
                                con.Close();
                                con.Dispose();
                            }

                            #endregion
                        }
                        else
                        {
                            //Response.write "<script>alert('No se puede borrar el usuario porque tiene tareas pendientes abiertas')</script>";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se puede borrar el usuario porque tiene tareas pendientes abiertas')", true);
                            return;
                        }
                    }

                    #endregion

                    //Recargar la pantalla.
                    Response.Redirect("AdministrarAsesores.aspx");

                    break;
                default:
                    break;
            }
        }
    }
}