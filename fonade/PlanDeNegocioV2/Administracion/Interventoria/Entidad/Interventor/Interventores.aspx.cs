using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using System.Net.Mail;
using System.Configuration;
using Fonade.Account;
using System.Web.Security;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Interventor
{
    public partial class Interventores : System.Web.UI.Page
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //gvContainer.DataSource = null;
                //gvContainer.DataBind();

                CargarDrops();

                MostrarPanel();
            }
        }

        private void CargarDrops()
        {
            //operador
            cargarDDL(ddlOperador, Usuario.CodOperador);
        }

        OperadorController operadorController = new OperadorController();

        private void cargarDDL(DropDownList ddl, int? _codOperador)
        {
            List<OperadorModel> opciones = new List<OperadorModel>();

            opciones = operadorController.cargaDLLOperador(_codOperador);

            ddl.DataSource = opciones;
            ddl.DataTextField = "NombreOperador"; // FieldName of Table in DataBase
            ddl.DataValueField = "idOperador";
            ddl.DataBind();
        }

        private void MostrarPanel()
        {
            if (ddlOperador.SelectedItem.Value == "0")
            {
                UpdatePanel1.Visible = false;
            }
            else
            {
                UpdatePanel1.Visible = true;

                //cargar datos
               

                codOperadorDLL = Convert.ToInt32(ddlOperador.SelectedItem.Value);

                cargarGridEntidades(codOperadorDLL);
                gvContainer.DataSource = null;
                gvContainer.DataBind();
            }
        }

        //public List<Datos.EntidadInterventoria> GetEntidades()
        //{
        //    return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.Get(Usuario.CodOperador);
        //}
        
        public List<InterventorEntidadDTO> GetInterventoresByEntidad(int idEntidad, int? _codOperador)
        {
            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.GetInterventores(idEntidad, _codOperador);
        }

        protected void btnAsignarEvaluador_Click(object sender, EventArgs e)
        {
            try
            {
                var idEntidad = Convert.ToInt32(hdMainValue.Value);

                foreach (GridViewRow currentRow in gvContainer.Rows)
                {
                    CheckBox checkProyecto = (CheckBox)currentRow.FindControl("chkContainer");
                    CheckBox isCurrentOwner = (CheckBox)currentRow.FindControl("chkIsCurrentOwner"); //isCurrentOwner es para saber si estaba asignado al inicio, el usuario lo desmarco y se debe desasignar
                    HiddenField codigoInterventor = (HiddenField)currentRow.FindControl("hdCodigoContainer");

                    if (checkProyecto.Checked || isCurrentOwner.Checked)
                    {
                        if (!checkProyecto.Checked && isCurrentOwner.Checked)
                        {
                            Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.DesAsignarInterventorEntidad(Convert.ToInt32(codigoInterventor.Value), Usuario.IdContacto);                            
                        }
                        if (checkProyecto.Checked && !isCurrentOwner.Checked)
                        {
                            Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.AsignarInterventorEntidad(idEntidad, Convert.ToInt32(codigoInterventor.Value), Usuario.IdContacto);                            
                        }
                    }
                }

                gvContainer.DataSource = GetInterventoresByEntidad(idEntidad, codOperadorDLL);
                gvContainer.DataBind();

                Formulacion.Utilidad.Utilidades.PresentarMsj("Información guardada con exito.", this, "Alert");
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj("Sucedio un error : " + ex.Message, this, "Alert");
            }
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var id = Convert.ToInt32(e.CommandArgument);
            hdMainValue.Value = id.ToString();

            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            LinkButton evaluadorSeleccionado = (LinkButton)row.Cells[0].FindControl("verMain");

            btnSave.Text = "Asignar interventores a entidad " + evaluadorSeleccionado.Text;
            btnSave.Visible = true;

            gvContainer.DataSource = GetInterventoresByEntidad(id, codOperadorDLL);
            gvContainer.DataBind();
        }

        protected int codOperadorDLL
        {
            get
            {
                return Convert.ToInt32(Session["idOperador"].ToString());
            }
            set
            {
                Session["idOperador"] = value;
            }
        }

        

        protected void ddlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            codOperadorDLL = Convert.ToInt32(ddlOperador.SelectedItem.Value);
            MostrarPanel();           
            
        }

        private void cargarGridEntidades(int _codOperador)
        {
            var entOperador = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades
                                .Entidad.GetXOperador(_codOperador);

            gvMain.DataSource = entOperador;
            gvMain.DataBind();
        }
    }
}