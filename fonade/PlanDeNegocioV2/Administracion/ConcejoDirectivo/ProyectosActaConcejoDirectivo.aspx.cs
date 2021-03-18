using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo
{
    public partial class ProyectosActaConcejoDirectivo : System.Web.UI.Page
    {
        public int CodigoActa
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codacta"]);
            }
            set { }
        }

        public int CodigoConvocatoria
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codconvocatoria"]);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetProyectoActa();
            }
        }

        protected void GetProyectoActa()
        {
            var proyectos = Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.GetProyectosConcejoDirectivo(CodigoConvocatoria);
            gvMain.DataSource = proyectos;
            gvMain.DataBind();
        }

        protected void btn_Adicionar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow fila in gvMain.Rows)
                {
                    CheckBox proyectoChecked = (CheckBox)fila.FindControl("ckProyecto");
                    HiddenField codigoProyecto = (HiddenField)fila.FindControl("hdCodigoProyecto");

                    if (proyectoChecked.Checked)
                    {
                        var proyectoActa = new Datos.ConcejoDirectivoActaProyecto {
                            CodActa = CodigoActa,
                            CodProyecto = Convert.ToInt32(codigoProyecto.Value)
                        };

                        Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.InsertProyectoActa(proyectoActa);
                    }
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.__doPostBack('Acta');window.close();", true);                
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }
    }
}