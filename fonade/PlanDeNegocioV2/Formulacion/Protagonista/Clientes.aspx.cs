using Datos;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.Mensajes;
using Fonade.Account;

namespace Fonade.PlanDeNegocioV2.Formulacion.Protagonista
{
    public partial class Clientes : System.Web.UI.Page
    {
        public int IdProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdProyecto") ? int.Parse(Request.QueryString["IdProyecto"]) : 0;
            }
        }
        public int IdCliente
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdCliente") ? int.Parse(Request.QueryString["IdCliente"]) : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && IdCliente > 0)
            {
                ConsultarCliente();

            }
            SetMaxLength();
        }

        void SetMaxLength()
        {
            // se establece por codigo el max length ya que el control no lo carga en textarea
            string max = "1000";
            txtJustificacion.Attributes.Add("maxlength", max);
            txtLocalizacion.Attributes.Add("maxlength", max);
            txtPerfil.Attributes.Add("maxlength", max);
        }

        void ConsultarCliente()
        {
            ProyectoProtagonistaCliente entCliente = new ProyectoProtagonistaCliente();

            entCliente = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.GetCliente(IdCliente);
            txtNombreCliente.Text = entCliente.Nombre;
            txtPerfil.Text = entCliente.Perfil;
            txtLocalizacion.Text = entCliente.Localizacion;
            txtJustificacion.Text = entCliente.Justificacion;

            LabelTitulo.Text = "EDITAR CLIENTE";
        }

        protected void btnGuardarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                bool resul;

                ProyectoProtagonistaCliente entCliente = new ProyectoProtagonistaCliente()
                {
                    Nombre = txtNombreCliente.Text.Trim(),
                    Perfil = txtPerfil.Text.Trim(),
                    Localizacion = txtLocalizacion.Text.Trim(),
                    Justificacion = txtJustificacion.Text.Trim(),
                    IdProyecto = IdProyecto
                };

                //idcliente > 0 editar
                if (IdCliente > 0)
                {
                    entCliente.IdCliente = IdCliente;
                    resul = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.EditarClientes(entCliente, out msg);
                }
                else//insertar
                {
                    resul = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.InsertarClientes(entCliente, out msg);
                }
                //actualizar la grilla de la pagina principal
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "upd", "window.opener.__doPostBack('', 'updGrillaClientes');", true);

                if (resul)
                {
                    FonadeUser usuario = (FonadeUser)Session["usuarioLogged"];
                    if (Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.VerificarTabSiEstaCompleta(Constantes.CONST_Paso1IngresoCondicionesComerciales, usuario.IdContacto))
                        Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Paso1IngresoCondicionesComerciales, IdProyecto, usuario.IdContacto, false);

                    ClientScript.RegisterStartupScript(this.GetType(), "Close", "<script>window.close();</script> ");
                    ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_Protagonista, IdProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                }

                else
                    Alert(msg);
            }
            catch (Exception ex)
            {
                //todo guardar log
                Alert(ex.Message);
            }

        }



        public void Alert(string mensaje)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Msg", "<script>alert('" + mensaje + "');</script> ");
        }




    }
}