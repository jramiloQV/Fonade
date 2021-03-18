using Datos;
using Fonade.Account;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo
{
    public partial class ActaDetalle : System.Web.UI.Page
    {
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        public int CodigoActa {
            get {
                return Convert.ToInt32(Request.QueryString["codacta"]);
            } set { }
        }

        public bool IsCreate {
            get {
                return CodigoActa.Equals(0);
            }
            set { }
        }

        public bool AllowUpdate
        {
            get
            {
                return !chkPublicado.Checked;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (!IsCreate)
                        GetDetails();
                }
                else {
                    if (Request["__EVENTTARGET"].ToString().Equals("Acta"))
                    {
                        if (!IsCreate)
                            GetDetails();
                    }
                }                                        
            }
            catch (Exception)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }

        protected void GetDetails() {
            var acta = Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.GetById(CodigoActa);

            txtNombre.Text = acta.Nombre;
            txtNumero.Text = acta.Numero;
            txtObservacion.Text = acta.Observacion;
            txtfecha.Text = acta.Fecha.ToShortDateString();
            chkPublicado.Checked = acta.Publicado;

            cmbConvocatoria.DataBind();
            cmbConvocatoria.ClearSelection();
            cmbConvocatoria.Items.FindByValue(acta.CodigoConvocatoria.ToString()).Selected = true;

            if (acta.Publicado) {
                chkPublicado.Enabled = false;
            }

            GetProyectoActa();
        }

        protected void GetProyectoActa (){
            var proyectos = Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.GetProyectosByActa(CodigoActa);
            gvMain.DataSource = proyectos;
            gvMain.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();
                InsertActa();

            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }

        protected void InsertActa()
        {
            if (Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.Exist(txtNumero.Text))
                throw new ApplicationException("Ya existe un acta con ese mismo numero");

            var newEntity = new Datos.ConcejoDirectivoActa
            {
                Numero = txtNumero.Text,
                Nombre = txtNombre.Text,
                Observaciones = txtObservacion.Text,
                CodConvocatoria = Convert.ToInt32(cmbConvocatoria.SelectedValue),
                Fecha = DateTime.ParseExact(txtfecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Publicado = false
            };

            Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.Insert(newEntity);

            Response.Redirect("ActaDetalle.aspx?codacta="+ newEntity.Id_acta);
        }
        
        protected void ValidateFields() {
            FieldValidate.ValidateString("Número", txtNumero.Text, true, 100);
            FieldValidate.ValidateString("Nombre", txtNombre.Text, true, 250);
            FieldValidate.ValidateString("observación", txtObservacion.Text, true, 1500);
            FieldValidate.ValidateString("Convocatoria", cmbConvocatoria.SelectedValue, true);
            FieldValidate.ValidateString("Fecha", txtfecha.Text, true);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkPublicado.Checked)
                {
                    var proyectos = Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.GetProyectosByActa(CodigoActa);

                    foreach (var proyecto in proyectos) {
                        if (Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.getEstadoProyecto(proyecto.CodigoProyecto).Equals(Constantes.CONST_concejo_directivo)) {
                            Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.DesmarcarProyecto(proyecto.CodigoProyecto);
                            Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.ChangeEstado(proyecto.CodigoProyecto, Constantes.CONST_Registro_y_Asesoria);
                        }
                    }

                    var entity = new Datos.ConcejoDirectivoActa
                    {
                        Id_acta = CodigoActa,         
                        Observaciones = txtObservacion.Text,                       
                        Publicado = chkPublicado.Checked
                    };

                    Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.Update(entity);

                    Response.Redirect("~/PlanDeNegocioV2/Administracion/ConcejoDirectivo/ActaConcejoDirectivo.aspx");
                }
                else
                {
                    var entity = new Datos.ConcejoDirectivoActa
                    {
                        Id_acta = CodigoActa,
                        Observaciones = txtObservacion.Text,
                        Publicado = chkPublicado.Checked
                    };

                    Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.Update(entity);
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/ConcejoDirectivo/ActaConcejoDirectivo.aspx");
                }
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }

        protected void linkAddProject_Click(object sender, EventArgs e)
        {
            Fonade.Proyecto.Proyectos.Redirect(Response, "ProyectosActaConcejoDirectivo.aspx?codacta=" + CodigoActa + "&codConvocatoria=" + cmbConvocatoria.SelectedValue, "_Blank", "width=1000,height=1000,top=0,left=0,scrollbars=yes,resizable=yes,toolbar=yes");
            GetDetails();
        }

        protected void imgAddProject_Click(object sender, ImageClickEventArgs e)
        {
            Fonade.Proyecto.Proyectos.Redirect(Response, "ProyectosActaConcejoDirectivo.aspx?codacta=" + CodigoActa+"&codConvocatoria="+cmbConvocatoria.SelectedValue, "_Blank", "width=1000,height=1000,top=0,left=0,scrollbars=yes,resizable=yes,toolbar=yes");
            GetDetails();
        }

        public List<Datos.Convocatoria> GetConvocatorias()
        {
            return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatorias(usuario.CodOperador);
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Eliminar"))
                {
                    if (e.CommandArgument != null)
                    {
                        string[] data = e.CommandArgument.ToString().Split(';');

                        var codigoActa = Convert.ToInt32(data[0]);
                        var codigoProyecto = Convert.ToInt32(data[1]);

                        Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.DeleteProyectoByActa(codigoActa,codigoProyecto);

                        GetDetails();                        
                    }
                }                
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
    }
}