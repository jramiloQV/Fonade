using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos;
using Fonade.Negocio.Proyecto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    public partial class AccionesActasSeg : Negocio.Base_Page //System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["idProyectoActasSeg"] != null)
                {
                    cargarGrilla();
                }
            }
        }

        private void cargarInformacion(int _codProyecto)
        {
            //cargar titulo
            lblTitulo.Text = "Administrar Actas de Seguimiento Proyecto: " + NombreProyecto(_codProyecto);

            List<InfoActaSeguimientoDTO> actaSeguimientos = ActaSeguimiento.GetInfoActasByProyecto(_codProyecto);

            gvMain.DataSource = actaSeguimientos;
            gvMain.DataBind();
        }

        private string NombreProyecto(int _codProyecto)
        {
            return proyectoController.nombreProyectoXCodigo(_codProyecto);
        }

        ProyectoController proyectoController = new ProyectoController();

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Despublicar"))
            {
                if (e.CommandArgument != null)
                {
                    hdIdActa.Value = e.CommandArgument.ToString();

                    //Buscar Fila dentro del grid
                    GridViewRow row = (GridViewRow)(e.CommandSource as LinkButton).Parent.Parent;

                    //Obtener el dato cargado en el grid
                    lblNombreActa.Text = row.Cells[1].Text;

                    ModalJustificacion.Show();
                }
            }
        }

        protected void btnGuardarJustificacion_Click(object sender, EventArgs e)
        {
            if (txtJustificacion.Text.Length>0)
            {
                //Actualizar acta de Seguimiento
                int idActa = Convert.ToInt32(hdIdActa.Value.ToString());
                if (ActaSeguimiento.DespublicarActa(idActa))
                {
                    //Ingresar la informacion de quien despublicó el acta
                    if (ActaSeguimiento.InsertJustActaDespublicada(usuario.IdContacto, txtJustificacion.Text))
                    {
                        cargarGrilla();
                    }
                }
            }
            else
            {
                Alert("El campo Justificacion es obligatorio.");
            }
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        private void cargarGrilla()
        {
            string codProyecto = Session["idProyectoActasSeg"].ToString();

            cargarInformacion(Convert.ToInt32(codProyecto));
        }
    }
}