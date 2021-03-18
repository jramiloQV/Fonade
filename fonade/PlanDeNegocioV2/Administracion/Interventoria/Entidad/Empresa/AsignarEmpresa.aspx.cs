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
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Empresa
{
    public partial class AsignarEmpresa : System.Web.UI.Page
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
                CargarDrops();

                MostrarPanel();
            }
        }

        private void MostrarPanel()
        {
            
            if (ddlOperador.SelectedItem.Value == "0")
            {
                UpdatePanel1.Visible = false;
            }
            else
            {
                if (ExistenEntidades(Convert.ToInt32(ddlOperador.SelectedItem.Value)))
                {
                    UpdatePanel1.Visible = true;

                    //cargar datos
                    cargarEntidadesDLL(Convert.ToInt32(ddlOperador.SelectedItem.Value));

                    codOperadorDLL = Convert.ToInt32(ddlOperador.SelectedItem.Value);

                    cargarGridInterventores(Convert.ToInt32(cmbEntidades.SelectedItem.Value));
                }
                else
                {
                    UpdatePanel1.Visible = false;
                    Formulacion.Utilidad.Utilidades.PresentarMsj("El operador no tiene entidades creadas.", this, "Alert");
                }
            }
        }

        private void cargarEntidadesDLL(int _codOperador)
        {
            var entOperador = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades
                                .Entidad.GetXOperador(_codOperador);

            cmbEntidades.DataSource = entOperador;
            //cmbEntidades.DataTextField = "Id"; // FieldName of Table in DataBase
            //cmbEntidades.DataValueField = "Nombre";
            cmbEntidades.DataBind();

        }

        private bool ExistenEntidades(int _codOperador)
        {
            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades
                                .Entidad.ExistenEntidades(_codOperador); 
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

        //public List<Datos.EntidadInterventoria> GetEntidades()
        //{
        //    return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.Get(Usuario.CodOperador);
        //}

        public List<InterventorEntidadDTO> Get(int idEntidad)
        {
            return Negocio.PlanDeNegocioV2.Administracion.Interventoria
                        .Entidades.Entidad.GetInterventoresByEntidad(idEntidad);
        }

        public List<ProyectoInterventoriaDTO> GetProyectos(int idInterventor, int codigoProyecto, int startIndex, int maxRows)
        {
            if (idInterventor == 0)
                return new List<ProyectoInterventoriaDTO>();

            int codOperador = Convert.ToInt32(codOperadorDLL);

            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas
                    .Empresa.GetProyectos(idInterventor, codigoProyecto, startIndex, maxRows, codOperador);
        }

        public Int32 CountProyectos(int idInterventor, int codigoProyecto)
        {
            if (idInterventor == 0)
                return 0;

            int codOperador = Convert.ToInt32(codOperadorDLL);

            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas
                    .Empresa.Count(idInterventor, codigoProyecto, codOperador);
        }

        protected void btnAsignarEvaluador_Click(object sender, EventArgs e)
        {
            try
            {
                var idInterventor = Convert.ToInt32(hdMainValue.Value);

                foreach (GridViewRow currentRow in gvContainer.Rows)
                {
                    CheckBox checkProyecto = (CheckBox)currentRow.FindControl("chkContainer");
                    CheckBox isCurrentOwner = (CheckBox)currentRow.FindControl("chkIsCurrentOwner");
                    HiddenField empresa = (HiddenField)currentRow.FindControl("hdIdEmpresa");
                    DropDownList contrato = (DropDownList)currentRow.FindControl("DropDownList1");
                    HiddenField contratoCurrent = (HiddenField)currentRow.FindControl("hdCodigoContrato");

                    int? idContrato = contrato.SelectedValue == "0" ? (int?)null : Convert.ToInt32(contrato.SelectedValue);
                    int codigoEmpresa = Convert.ToInt32(empresa.Value);

                    if (checkProyecto.Checked || isCurrentOwner.Checked)
                    {
                        if (!checkProyecto.Checked && isCurrentOwner.Checked)
                        {
                            Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.Empresa.DesAsignarInterventorEmpresa(idInterventor, codigoEmpresa);
                        }
                        if (checkProyecto.Checked)
                        {
                            if (!isCurrentOwner.Checked)
                            {
                                Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.Empresa.AsignarInterventorEmpresa(idInterventor, codigoEmpresa, idContrato);
                            }
                            else
                            {
                                var codigoContrato = String.IsNullOrEmpty(contratoCurrent.Value) ? 0 : Convert.ToInt32(contratoCurrent.Value);
                                if (codigoContrato != idContrato.GetValueOrDefault(0))
                                {
                                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.Empresa.AsignarInterventorEmpresa(idInterventor, codigoEmpresa, idContrato);
                                }
                            }
                        }
                    }
                }

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
            LinkButton filaSeleccionado = (LinkButton)row.Cells[0].FindControl("verMain");

            btnSave.Text = "Asignar empresas a interventor " + filaSeleccionado.Text;
            btnSave.Visible = true;

            gvContainer.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateNumeric("Código de proyecto", txtCodigo.Text, true);

                lblError.Visible = false;
                gvMain.DataSource = Get(Convert.ToInt32(cmbEntidades.SelectedItem.Value));
                gvMain.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<ContratoEntidadDTO> GetContratosByInterventor(int idInterventor)
        {
            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos.Contrato.GetContratosByInterventor(idInterventor);
        }

        protected void gvProyectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList cmbZonas = (e.Row.FindControl("DropDownList1") as DropDownList);

                HiddenField codigoProyecto = (HiddenField)e.Row.FindControl("hdCodigoProyecto");
                HiddenField contrato = (HiddenField)e.Row.FindControl("hdCodigoContrato");
                var codigoInterventor = Convert.ToInt32(hdMainValue.Value);
                var codigoContrato = String.IsNullOrEmpty(contrato.Value) ? "0" : contrato.Value;

                var contratos = GetContratosByInterventor(codigoInterventor);

                cmbZonas.DataSource = contratos;
                cmbZonas.DataBind();

                cmbZonas.Items.Insert(0, new ListItem("Sin contrato", "0"));
                cmbZonas.ClearSelection();

                foreach (var currentContract in contratos)
                {
                    if (currentContract.Id.ToString() == codigoContrato)
                    {
                        var contratoActivo = cmbZonas.Items.FindByValue(codigoContrato);
                        cmbZonas.Items.FindByValue(codigoContrato).Selected = true;
                        if (currentContract.ContratoVencido)
                        {
                            contratoActivo.Attributes.Add("style", "color:#fc0000");
                        }


                    }
                    else
                    {
                        if (currentContract.ContratoVencido)
                        {
                            cmbZonas.Items.Remove(cmbZonas.Items.FindByValue(currentContract.Id.ToString()));
                        }
                    }
                }


            }
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
            //cargarGridInterventores(Convert.ToInt32(cmbEntidades.SelectedItem.Value));
            gvContainer.DataSource = null;
            gvContainer.DataBind();
        }

        protected void cmbEntidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarGridInterventores(Convert.ToInt32(cmbEntidades.SelectedItem.Value));
        }

        private void cargarGridInterventores(int idEntidad)
        {
            gvMain.DataSource = Get(idEntidad);
            gvMain.DataBind();
        }
    }
}