using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Datos;
using Fonade.Account;
using System.Web.Security;

namespace Fonade.PlanDeNegocioV2.Formulacion.Protagonista
{
    public partial class Protagonista : System.Web.UI.Page
    {
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean PostitVisible
        {
            get
            {
                return EsMiembro && !EsRealizado;
            }
            set { }
        }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);
            }
            set { }
        }

        public List<ProyectoProtagonistaCliente> ListClientes
        {
            get { return (List<ProyectoProtagonistaCliente>)Session["ListClientes"]; }
            set { Session["ListClientes"] = value; }
        }

        public ProyectoProtagonista EntProtagonista
        {
            get { return (ProyectoProtagonista)Session["EntProtagonista"]; }
            set { Session["EntProtagonista"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
            {
                Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
                Encabezado.CodigoTab = Constantes.CONST_Protagonista;

                SetPostIt();

                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(Constantes.CONST_Protagonista, Encabezado.CodigoProyecto);

                if (!IsPostBack)
                    CargarProtagonista();

                //if (!IsPostBack || Request["__EVENTARGUMENT"] == "updGrillaClientes")
                CargarClientes();

                gwClientes.Columns[0].Visible = AllowUpdate;

                divParentContainer.Attributes.Add("class", "parentContainer");
            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Protagonista;
        }


        void CargarClientes()
        {
            ListClientes = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.GetClientes(Encabezado.CodigoProyecto);
            gwClientes.DataSource = ListClientes;
            gwClientes.DataBind();
        }

        void CargarProtagonista()
        {
            ProyectoProtagonista entProtagonista = new ProyectoProtagonista();
            entProtagonista = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.GetProtagonista(Encabezado.CodigoProyecto);
            EntProtagonista = entProtagonista;
            if (entProtagonista != null)
            {
                DropDownPerfiles.SelectedValue = entProtagonista.PerfilesDiferentes ? "1" : "0";
                DropDownPerfiles.Attributes.Add("PerfilDiferente", DropDownPerfiles.SelectedValue);
                CKCliente.Text = entProtagonista.NecesidadesPotencialesClientes;
                CKConsumidores.Text = entProtagonista.NecesidadesPotencialesConsumidores;
                CKPerfilConsumidor.Text = entProtagonista.PerfilConsumidor;

                DropDownPerfiles_SelectedIndexChanged(DropDownPerfiles, new EventArgs());
            }
            else
            {
                this.DropDownPerfiles.Attributes.Remove("onChange");
                this.DropDownPerfiles.Attributes.Remove("onFocus");
                DropDownPerfiles.AutoPostBack = true;
            }

        }

        protected void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 540) / 2) - 20;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Clientes.aspx?IdProyecto=" + Encabezado.CodigoProyecto, "_Blank", "width=540,height=540,top=100,left=" + pos);
        }

        protected void btnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            //validar que se haya agregado clientes al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.GetClientes(Encabezado.CodigoProyecto).Count() <= 1)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(14));
                return;
            }

            ImageButton btn = (ImageButton)sender;
            string msg;
            bool resul;
            resul = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.EliminarClientes(int.Parse(btn.CommandArgument), out msg);
            if (resul)
            {
                //todo: verificar el estado del tab en ingresos y condiciones comerciales
                CargarClientes();
            }

            Alert(msg);
        }

        protected void gwClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwClientes.PageIndex = e.NewPageIndex;
            gwClientes.DataSource = ListClientes;
            gwClientes.DataBind();
        }

        protected void btnEditarCliente_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int pos = ((int.Parse(HiddenWidth.Value) - 500) / 2) - 20;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Redirect", "window.open('Clientes.aspx?IdProyecto=" + Encabezado.CodigoProyecto + "&IdCliente=" + btn.CommandArgument + "', '_Blank', 'width = 500,height = 540,top = 100,left = " + pos + "');", true);
        }

        public void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void DropDownPerfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            divPerfilConsumidor.Visible = DropDownPerfiles.SelectedValue.Equals("1");
            divConsumidores.Visible = DropDownPerfiles.SelectedValue.Equals("1");

            if (EntProtagonista != null && DropDownPerfiles.SelectedValue.Equals("0"))
            {
                this.DropDownPerfiles.Attributes.Remove("onChange");
                this.DropDownPerfiles.Attributes.Remove("onFocus");
                DropDownPerfiles.AutoPostBack = true;
            }
            else if (EntProtagonista != null)
            {
                this.DropDownPerfiles.Attributes["onChange"] = "handleChange(this)";
                this.DropDownPerfiles.Attributes["onFocus"] = "this.oldIndex = this.selectedIndex";
                DropDownPerfiles.AutoPostBack = false;
            }
        }

        protected void btnGuardarProtagonista_Click(object sender, EventArgs e)
        {
            try
            {
                //validar que se haya agregado clientes al plan
                if (Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.GetClientes(Encabezado.CodigoProyecto).Count() <= 0)
                {
                    Alert(Negocio.Mensajes.Mensajes.GetMensaje(11));
                    return;
                }


                ProyectoProtagonista entProtagonista = new ProyectoProtagonista()
                {
                    PerfilesDiferentes = DropDownPerfiles.SelectedValue.Equals("1"),
                    IdProyecto = Encabezado.CodigoProyecto,
                    NecesidadesPotencialesClientes = CKCliente.Text.Trim(),
                    NecesidadesPotencialesConsumidores = DropDownPerfiles.SelectedValue.Equals("1") ? CKConsumidores.Text.Trim() : null,
                    PerfilConsumidor = DropDownPerfiles.SelectedValue.Equals("1") ? CKPerfilConsumidor.Text.Trim() : null
                };

                string msg;
                bool resul = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.InsertarProtagonista(entProtagonista, out msg);

                FonadeUser usuario = (FonadeUser)Session["usuarioLogged"]; if (usuario == null) Response.Redirect("~/Account/Login.aspx");
                if (resul)
                {
                    Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Protagonista, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                    //actualiza el tab de condiciones comerciales en completo=false
                    if (!DropDownPerfiles.SelectedValue.Equals(DropDownPerfiles.Attributes["PerfilDiferente"]))
                    {
                        if (DropDownPerfiles.SelectedValue.Equals("1"))
                            Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Paso1IngresoCondicionesComerciales, Encabezado.CodigoProyecto, usuario.IdContacto, false);
                    }

                    ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_Protagonista, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                    Encabezado.ActualizarFecha();

                    CargarProtagonista();
                }

                Alert(msg);

            }
            catch (Exception ex)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(7));
            }

        }

        protected void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            DropDownPerfiles.SelectedValue = "";
            CKPerfilConsumidor.Text = null;
            CKCliente.Text = null;
            CKConsumidores.Text = null;
        }
    }
}