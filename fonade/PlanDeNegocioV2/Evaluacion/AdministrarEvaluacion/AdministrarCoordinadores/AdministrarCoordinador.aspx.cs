using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarCoordinador;

namespace Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarCoordinadores
{
    public partial class AdministrarCoordinador : Negocio.Base_Page //: System.Web.UI.Page---Se comento esta parte para poder hacer la herencia de Negocio.Base_Page y acceder a la variable usuario
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                gvEvaluadores.DataSource = null;
                gvEvaluadores.DataBind();
            }
        }

        public List<CoordinadorEvaluacion> GetCoordinadores()
        {
            return AsignarCoordinador.GetCoordinadores(usuario.CodOperador);
        }
        
        public List<EvaluadorEvaluacion> GetEvaluadores(int codigoCoordinador)
        {
            return AsignarCoordinador.GetEvaluadores(codigoCoordinador, usuario.CodOperador);
        }

        protected void gvCoordinadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var codigoCoordinador = Convert.ToInt32(e.CommandArgument);
            hdCodigoCoordinador.Value = codigoCoordinador.ToString();

            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            LinkButton coordinadorSeleccionado = (LinkButton)row.Cells[0].FindControl("verEvaluadores");

            btnAsignar.Text = "Asignar evaluadores a coordinador " + coordinadorSeleccionado.Text;
            btnAsignar.Visible = true;

            gvEvaluadores.DataSource = GetEvaluadores(codigoCoordinador);
            gvEvaluadores.DataBind();
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                var codigoCoordinador = Convert.ToInt32(hdCodigoCoordinador.Value);

                foreach (GridViewRow currentRow in gvEvaluadores.Rows)
                {
                    CheckBox checkProyecto = (CheckBox)currentRow.FindControl("chkEvaluador");
                    CheckBox isCurrentOwner = (CheckBox)currentRow.FindControl("chkIsCurrentOwner"); //isCurrentOwner es para saber si estaba asignado al inicio, el usuario lo desmarco y se debe desasignar
                    HiddenField codigoEvaluador = (HiddenField)currentRow.FindControl("hdIdEvaluador");

                    if (checkProyecto.Checked || isCurrentOwner.Checked)
                    {
                        if (!checkProyecto.Checked && isCurrentOwner.Checked)
                        {
                            AsignarCoordinador.AsignarCoordinadorAEvaluador(Convert.ToInt32(codigoEvaluador.Value), codigoCoordinador, true);
                        }

                        if (checkProyecto.Checked && !isCurrentOwner.Checked)
                        {
                            AsignarCoordinador.AsignarCoordinadorAEvaluador(Convert.ToInt32(codigoEvaluador.Value), codigoCoordinador);
                        }
                    }
                }

                gvEvaluadores.DataSource = GetEvaluadores(codigoCoordinador);
                gvEvaluadores.DataBind();

                Formulacion.Utilidad.Utilidades.PresentarMsj("Información guardada con exito.", this, "Alert");
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj("Sucedio un error : " + ex.Message, this, "Alert");
            }
        }

    }
}