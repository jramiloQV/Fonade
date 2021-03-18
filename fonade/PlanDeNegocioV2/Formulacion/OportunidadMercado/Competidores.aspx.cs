using Datos;
using Fonade.Account;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.OportunidadMercado
{
    public partial class Competidores : System.Web.UI.Page
    {
        public int IdProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdProyecto") ? int.Parse(Request.QueryString["IdProyecto"]) : 0;
            }
        }

        public int IdCompetidor
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdCompetidor") ? int.Parse(Request.QueryString["IdCompetidor"]) : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CargarMensajes();
                SetMaxLength();
            }

            if (!IsPostBack && IdCompetidor > 0)
            {
                ConsultarCompetidor();
            }
        }

        void SetMaxLength()
        {
            // se establece por codigo el max length ya que el control no lo carga en textarea
            string max = "1000";
            txtLocalizacion.Attributes.Add("maxlength", max);
            TxtLogistica.Attributes.Add("maxlength", max);
            txtOtroCual.Attributes.Add("maxlength", max);
            txtPrecios.Attributes.Add("maxlength", max);
            txtProductos.Attributes.Add("maxlength", max);
        }

        void ConsultarCompetidor()
        {
            ProyectoOportunidadMercadoCompetidore entCompetidor = new ProyectoOportunidadMercadoCompetidore();

            entCompetidor = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.GetCompetidor(IdCompetidor);
            txtNombreCompetidor.Text = entCompetidor.Nombre;
            txtLocalizacion.Text = entCompetidor.Localizacion;
            TxtLogistica.Text = entCompetidor.LogisticaDistribucion;
            txtOtroCual.Text = entCompetidor.OtroCual;
            txtPrecios.Text = entCompetidor.Precios;
            txtProductos.Text = entCompetidor.ProductosServicios;
            
            LabelTitulo.Text = "EDITAR COMPETIDOR";
        }

        void CargarMensajes()
        {
            RequiredNombre.ErrorMessage = string.Format(Negocio.Mensajes.Mensajes.GetMensaje(104), "Nombre Competidor");
            RequiredLocalizacion.ErrorMessage = string.Format(Negocio.Mensajes.Mensajes.GetMensaje(104), "Localización");
            RequiredLogistica.ErrorMessage = string.Format(Negocio.Mensajes.Mensajes.GetMensaje(104), "Logística");
            RequiredProductos.ErrorMessage = string.Format(Negocio.Mensajes.Mensajes.GetMensaje(104), "Productos");
            RequiredPrecios.ErrorMessage = string.Format(Negocio.Mensajes.Mensajes.GetMensaje(104), "Precios");
        }

        protected void btnGuardarCompetidor_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                bool resul;

                ProyectoOportunidadMercadoCompetidore entCompetidor = new ProyectoOportunidadMercadoCompetidore()
                {
                    IdProyecto = IdProyecto,
                    Localizacion = txtLocalizacion.Text.Trim(),
                    LogisticaDistribucion = TxtLogistica.Text.Trim(),
                    Nombre = txtNombreCompetidor.Text.Trim(),
                    OtroCual = txtOtroCual.Text.Trim(),
                    Precios = txtPrecios.Text.Trim(),
                    ProductosServicios = txtProductos.Text.Trim()
                };

                //idcliente > 0 editar
                if (IdCompetidor > 0)
                {
                    entCompetidor.IdCompetidor = IdCompetidor;
                    resul = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.EditarCompetidor(entCompetidor, out msg);
                }
                else//insertar
                {
                    entCompetidor.IdProyecto = IdProyecto;
                    resul = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.InsertarCompetidores(entCompetidor, out msg);
                }
                //actualizar la grilla de la pagina principal
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "upd", "window.opener.__doPostBack('', 'updGrillaCompetidores');", true);

                if (resul)
                {
                    FonadeUser usuario = (FonadeUser)Session["usuarioLogged"];
                    ClientScript.RegisterStartupScript(this.GetType(), "Close", "<script>window.close();</script> ");
                    ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_OportunidadMercado, IdProyecto, usuario.IdContacto, usuario.CodGrupo, false);
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

        private void Alert(string mensaje)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Msg", "<script>alert('" + mensaje + "');</script> ");
        }
    }
}