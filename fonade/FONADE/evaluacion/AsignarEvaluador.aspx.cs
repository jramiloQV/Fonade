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

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// AsignarEvaluador
    /// </summary>
    
    public partial class AsignarEvaluador : Negocio.Base_Page
    {
        /// <summary>
        /// The e value asignado
        /// </summary>
        protected int EValAsignado;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarCombo();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddl_evals control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddl_evals_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboProyectos();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddl_proyectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddl_proyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarProyectos();
        }

        /// <summary>
        /// Handles the Click event of the btn_asignar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_asignar_Click(object sender, EventArgs e)
        {
            PanellistadoCoordinadores.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the btn_actualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        private void CargarCombo()
        {
            var sectores = (from s in consultas.Db.Sector
                            orderby s.NomSector
                            select new {
                                id_Sector = s.Id_Sector,
                                nomSector = s.NomSector
                            }
                            ).ToList();
            sectores.Insert(0, new { id_Sector = -1, nomSector = "Todos los sectores" });

            ddl_evals.DataSource = sectores;
            ddl_evals.DataTextField = "nomSector";
            ddl_evals.DataValueField = "id_Sector";
            ddl_evals.DataBind();
            ddl_evals.Items.Insert(0, new ListItem("Seleccione", "0"));

            ddl_proyectos.Items.Insert(0, new ListItem("Seleccione", "0"));
        }
        /// <summary>
        /// carga en dropdownlist los proyectos
        /// </summary>
        private void ComboProyectos()
        {
            ddl_proyectos.Items.Clear();
            var RSProyecto = new DataTable();

            var txtSQL = string.Empty;
            if (ddl_evals.SelectedValue.Equals("-1"))
            {
                txtSQL = " SELECT Id_Proyecto, NomProyecto, pc.CodContacto " +
                        " FROM Proyecto P " +
                        " INNER JOIN Subsector S ON P.codsubSector=S.id_subsector " +
                        " LEFT JOIN Proyectocontacto pc on id_proyecto=codproyecto " +
                        " and pc.inactivo=0 and codrol = " + Constantes.CONST_RolEvaluador +
                        " WHERE CodEstado = " + Constantes.CONST_Acreditado + " or codestado =" + Constantes.CONST_Evaluacion + "order by Id_Proyecto desc";
            }
            else
            {
                txtSQL = " SELECT Id_Proyecto, NomProyecto, pc.CodContacto " +
                        " FROM Proyecto P " +
                        " INNER JOIN Subsector S ON P.codsubSector=S.id_subsector and codSector = " + ddl_evals.SelectedValue +
                        " LEFT JOIN Proyectocontacto pc on id_proyecto=codproyecto " +
                        " and pc.inactivo=0 and codrol = " + Constantes.CONST_RolEvaluador +
                        " WHERE CodEstado = " + Constantes.CONST_Acreditado + " or codestado =" + Constantes.CONST_Evaluacion + "order by Id_Proyecto desc";
            }
           
            RSProyecto = consultas.ObtenerDataTable(txtSQL, "text");

            ListItem item = new ListItem();
            foreach (DataRow row in RSProyecto.Rows)
            {
                item = new ListItem();
                item.Value = row["Id_Proyecto"].ToString();
                if (String.IsNullOrEmpty(row["CodContacto"].ToString()))
                {
                    item.Attributes.Add("title", "Plan de Negocio sin evaluador");
                    item.Attributes.Add("style", "color:#CC0000; font-weight:bold;");
                    item.Text = row["Id_Proyecto"].ToString() + " " + row["NomProyecto"].ToString() + " !! Plan de Negocio sin evaluador";
                }
                else
                    item.Text = row["Id_Proyecto"].ToString() + " " + row["NomProyecto"].ToString();
                ddl_proyectos.Items.Add(item);
            }
            ddl_proyectos.Items.Insert(0, new ListItem("Seleccione", "0"));
        }
        /// <summary>
        /// carga el proyecto
        /// </summary>
        private void CargarProyectos()
        {
            if(ddl_proyectos.SelectedIndex != 0)
            {
                var query = (from P in consultas.cargarProyectoSumarioActual(Convert.ToInt32(ddl_proyectos.SelectedValue))
                            select P).ToList();
                if (query.Count() == 0)
                {
                    DltEvaluacion.DataSource = null;
                    DltEvaluacion.DataBind();
                    Panelproyectos.Visible = false;
                }
                else
                {
                    DltEvaluacion.DataSource = query;
                    DltEvaluacion.DataBind();
                    Panelproyectos.Visible = true;
                }

                var query2 = (from P in consultas.Db.MD_VerEvalDelProyecto(Convert.ToInt32(ddl_proyectos.SelectedValue))
                              select P).ToList();

                gvcoorAsigando.DataSource = query2;
                gvcoorAsigando.DataBind();
                if(query2.Count() > 0)
                {
                    llenarProgramas(query2[0].codcontacto.ToString());
                }
                else
                {
                    llenarProgramas("0");
                }
                PanellistadoCoordinadores.Visible = false;
                
            }
        }
        /// <summary>
        /// llenar programas recibe argumento el coordinador
        /// </summary>
        /// <param name="coordinador"></param>
        private void llenarProgramas(string coordinador)
        {
            rbl_coordinEval.ClearSelection();
            rbl_coordinEval.Items.Clear();
            rbl_coordinEval.Dispose();
            var codigoSector = 0;

            if (ddl_evals.SelectedValue.Equals("-1"))
            {
                var RSProyecto = new DataTable();
                var txtSQL = " SELECT Top 1 S.CodSector " +
                            " FROM Proyecto P " +
                            " INNER JOIN Subsector S ON P.codsubSector=S.id_subsector" +
                            " LEFT JOIN Proyectocontacto pc on id_proyecto=codproyecto " +
                            " and pc.inactivo=0 and pc.FechaFin is null and codrol = " + Constantes.CONST_RolEvaluador +
                            " WHERE CodEstado = " + Constantes.CONST_Acreditado + " or codestado =" + Constantes.CONST_Evaluacion + " and id_proyecto =" + ddl_proyectos.SelectedValue + " order by Id_Proyecto desc";
                RSProyecto = consultas.ObtenerDataTable(txtSQL, "text");

                codigoSector = Convert.ToInt32(RSProyecto.Rows[0]["CodSector"].ToString());
            }
            else
            {
                codigoSector = Convert.ToInt32(ddl_evals.SelectedValue);
            }

            var query = from x in consultas.Db.MD_VerEvalSector(codigoSector)
                        orderby x.nombre
                        select new
                        {
                            nombreCoord = x.nombre,
                            id_coord = x.Id_Contacto,
                        };

            rbl_coordinEval.DataSource = query.ToList();
            rbl_coordinEval.DataTextField = "nombreCoord";
            rbl_coordinEval.DataValueField = "id_coord";
            rbl_coordinEval.DataBind();
            if(coordinador != "0")
            {
                rbl_coordinEval.SelectedValue = coordinador;
            }
        }
        /// <summary>
        /// Actualizacion evaluadores
        /// </summary>
        private void Actualizar()
        {
            var query = (from x in consultas.Db.ConvocatoriaProyectos
                         orderby x.Fecha descending
                         where x.CodProyecto == Convert.ToInt32(ddl_proyectos.SelectedValue)
                         select new
                         {
                             x.CodConvocatoria
                         }).FirstOrDefault();
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_updateEvalAsignado", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodProyecto", Convert.ToInt32(ddl_proyectos.SelectedValue));
            cmd.Parameters.AddWithValue("@CodConvocatoria", query.CodConvocatoria);
            cmd.Parameters.AddWithValue("@CodEvalNuevo", Convert.ToInt32(rbl_coordinEval.SelectedValue));
            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
            con.Open();
            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd2.Dispose();
            cmd.Dispose();
            gvcoorAsigando.DataBind();
            PanellistadoCoordinadores.Visible = false;
            var proyecto = consultas.Db.Proyecto.FirstOrDefault(o => o.Id_Proyecto == int.Parse(ddl_proyectos.SelectedValue.ToString()));
            if (proyecto.CodEstado != 4)
            {
                proyecto.CodEstado = 4;
                consultas.Db.SubmitChanges();
            }

            var query2 = (from P in consultas.Db.MD_VerEvalDelProyecto(Convert.ToInt32(ddl_proyectos.SelectedValue))
                          select P).FirstOrDefault();
            if (query2 != null)
            {
                llenarProgramas(query2.codcontacto.ToString());
            }
            else
            {
                llenarProgramas("0");
            }
            PanellistadoCoordinadores.Visible = false;

            Response.Redirect("AsignarEvaluador.aspx");
        }
    }       
}